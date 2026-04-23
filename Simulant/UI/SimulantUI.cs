#pragma warning disable IDE1006
using Simulant.ACT;
using Simulant.Core;
using Simulant.Game;
using Simulant.Game.ExtractedCsv;
using Simulant.Game.ExtractedCsv.Rows;
using Simulant.Game.FFCS.Client.System.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Simulant.UI
{
    public partial class SimulantUI : UserControl
    {
        private readonly PluginHost _host;

        internal SimulantUI(PluginHost host)
        {
            _host = host;
            InitializeComponent();
            RefreshLogView();
        }

        private void btnInitPlugin_Click(object sender, EventArgs e)
        {
            try
            {
                _host.LogVerbose("测试：点击初始化插件按钮。");

                _host.LogVerbose("正在绑定鲶鱼精邮差……");
                NamazuInterop.Init();

                _host.LogVerbose("正在扫描地址……");
                var scanResult = SigAddressScanner.Scan(); // 应该新开线程扫描
                var errorLines = scanResult
                    .Where(x => !string.IsNullOrEmpty(x.Value))
                    .Select(x => $"{x.Key}: {x.Value}")
                    .ToList();

                _host.LogVerbose($"地址扫描完成，成功：{scanResult.Count - errorLines.Count} / {scanResult.Count}");
                foreach (var line in errorLines)
                {
                    _host.LogError("地址扫描失败：" + line);
                }
            }
            catch (Exception ex)
            {
                _host.LogError("初始化插件失败：" + ex.Message);
            }
        }

        private void btnLoadPreset_Click(object sender, EventArgs e)
        {
            _host.LogVerbose("测试：btnLoadPreset_Click");

            var form = new SimPresetSelectForm();
            if (form.ShowDialog() != DialogResult.OK)
                return;

            var preset = form.PresetResult;
            var territory = _host.ZoneService.SelectPreset(preset);

            if (territory == null)
            {
                lblTerritory.Text = $"区域: 无 ({preset.TerritoryId})";
                lblPhase.Text = $"阶段: {preset.Name}";
                return;
            }

            var id = preset.TerritoryId;
            if (!string.IsNullOrWhiteSpace(territory.ContentFinderCondition.Name))
            {
                lblTerritory.Text = $"副本: {territory.ContentFinderCondition.Name} ({id})";
            }
            else
            {
                lblTerritory.Text = $"区域: {territory.PlaceName.Name} ({id})";
            }

            lblPhase.Text = $"阶段: {preset.Name}";
        }

        private void btnFirewallOn_Click(object sender, EventArgs e)
        {
            try
            {
                _host.FirewallService.Enable();
            }
            catch (Exception ex)
            {
                _host.LogError("防火墙开启失败：" + ex.Message);
            }
        }

        private void btnFirewallOff_Click(object sender, EventArgs e)
        {
            try
            {
                _host.FirewallService.Disable();
            }
            catch (Exception ex)
            {
                _host.LogError("防火墙关闭失败：" + ex.Message);
            }
        }

        private unsafe void btnSimEnter_Click(object sender, EventArgs e)
        {
            _host.LogVerbose("测试：btnSimEnter_Click");
            _host.ZoneService.EnterSelectedTerritory();
        }

        private void btnSimExit_Click(object sender, EventArgs e)
        {
            _host.LogVerbose("测试：btnSimExit_Click");
            _host.ZoneService.ExitToInitialTerritory();
        }

        #region Log

        private Dictionary<LogType, List<LogItem>> _logSnapshot = new Dictionary<LogType, List<LogItem>>();
        private List<LogItem> _virtualData = new List<LogItem>();

        private static readonly Color LogBgRed = Color.FromArgb(255, 225, 225);
        private static readonly Color LogBgYellow = Color.FromArgb(255, 245, 215);
        private static readonly Color LogBgGreen = Color.FromArgb(215, 255, 225);
        private static readonly Color LogBgBlue = Color.FromArgb(215, 235, 255);
        private static readonly Color LogBgGray = Color.FromArgb(250, 250, 250);

        internal void RefreshLogView()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new System.Action(RefreshLogView));
                return;
            }

            _logSnapshot = _host.PluginLog.Snapshot();
            ApplyLogFilter();
        }

        private bool IsLogTypeEnabled(LogType type)
        {
            switch (type)
            {
                case LogType.Error:
                    return chkLogFilterError.Checked;
                case LogType.Warning:
                    return chkLogWarning.Checked;
                case LogType.Sim:
                    return chkLogFilterSim.Checked;
                case LogType.Call:
                    return chkLogFilterCall.Checked;
                case LogType.Verbose:
                    return chkLogFilterVerbose.Checked;
                default:
                    return false;
            }
        }

        private void ApplyLogFilter()
        {
            var pattern = txtLogRegex.Text ?? string.Empty;
            Regex regex = null;

            if (pattern.Length > 0)
            {
                try
                {
                    regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    txtLogRegex.BackColor = SystemColors.Window;
                }
                catch (ArgumentException)
                {
                    txtLogRegex.BackColor = Color.MistyRose;
                    return;
                }
            }
            else
            {
                txtLogRegex.BackColor = SystemColors.Window;
            }

            _virtualData = _logSnapshot
                .Where(pair => IsLogTypeEnabled(pair.Key))
                .SelectMany(pair => pair.Value)
                .Where(log => regex == null || regex.IsMatch(log.Message ?? string.Empty))
                .OrderBy(log => log.Timestamp)
                .ToList();

            dgvLog.RowCount = _virtualData.Count;
            dgvLog.Invalidate();

            if (dgvLog.RowCount > 0)
            {
                dgvLog.ClearSelection();
                // scroll to bottom
                dgvLog.FirstDisplayedScrollingRowIndex = dgvLog.RowCount - 1;
            }
        }

        private void txtLogRegex_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtLogRegex_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            ApplyLogFilter();
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private void btnLogSearch_Click(object sender, EventArgs e)
        {
            ApplyLogFilter();
        }

        private void dgvLog_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _virtualData.Count)
                return;

            var item = _virtualData[e.RowIndex];

            switch (item.Type)
            {
                case LogType.Error:
                    e.CellStyle.BackColor = LogBgRed;
                    break;
                case LogType.Warning:
                    e.CellStyle.BackColor = LogBgYellow;
                    break;
                case LogType.Sim:
                    e.CellStyle.BackColor = LogBgGreen;
                    break;
                case LogType.Call:
                    e.CellStyle.BackColor = LogBgBlue;
                    break;
                case LogType.Verbose:
                    e.CellStyle.BackColor = LogBgGray;
                    break;
                default:
                    e.CellStyle.BackColor = Color.White;
                    break;
            }
            e.CellStyle.ForeColor = Color.Black;
            e.FormattingApplied = true;
        }

        private void dgvLog_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _virtualData.Count)
                return;

            var item = _virtualData[e.RowIndex];

            switch (e.ColumnIndex)
            {
                case 0:
                    e.Value = item.Timestamp.ToString("HH:mm:ss.f");
                    break;
                case 1:
                    e.Value = item.TypeDescription;
                    break;
                case 2:
                    e.Value = item.Message;
                    break;
            }
        }

        private IEnumerable<CheckBox> GetLogFilterItems()
        {
            yield return chkLogFilterError;
            yield return chkLogWarning;
            yield return chkLogFilterSim;
            yield return chkLogFilterCall;
            yield return chkLogFilterVerbose;
        }

        // Only for LeftClick, RightClick would not trigger MouseClick on chks
        private void chkLogFilterAll_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                chkLogFilterAll_Click();
        }

        // Only for RightClick
        private void chkLogFilterAll_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                chkLogFilterAll_Click();
        }

        private void chkLogFilterAll_Click()
        {
            var newChecked = chkLogFilterAll.CheckState != CheckState.Checked;

            chkLogFilterAll.CheckState = newChecked
                ? CheckState.Checked
                : CheckState.Unchecked;

            foreach (var item in GetLogFilterItems())
            {
                item.Checked = newChecked;
            }

            ApplyLogFilter();
        }

        // Only for LeftClick, RightClick would not trigger MouseClick on chks
        private void chkLogFilters_MouseClick(object sender, MouseEventArgs e)
        {
            if (!(sender is CheckBox current))
                return;

            if (e.Button == MouseButtons.Left)
            {
                current.Checked = !current.Checked;
                SyncLogFilterAllState();
                ApplyLogFilter();
            }
        }

        // Only for RightClick
        private void chkLogFilters_MouseDown(object sender, MouseEventArgs e)
        {
            if (!(sender is CheckBox current))
                return;

            if (e.Button == MouseButtons.Right)
            {
                foreach (var item in GetLogFilterItems())
                {
                    item.Checked = item == current;
                }

                SyncLogFilterAllState();
                ApplyLogFilter();
                return;
            }
        }

        private void SyncLogFilterAllState()
        {
            var totalCount = GetLogFilterItems().Count();
            var checkedCount = GetLogFilterItems().Count(x => x.Checked);

            if (checkedCount == 0)
            {
                chkLogFilterAll.CheckState = CheckState.Unchecked;
            }
            else if (checkedCount == totalCount)
            {
                chkLogFilterAll.CheckState = CheckState.Checked;
            }
            else
            {
                chkLogFilterAll.CheckState = CheckState.Indeterminate;
            }
        }

        #endregion Log


        private void btnDebug_Click(object sender, EventArgs e)
        {
            Benchmark();
        }

        void Benchmark()
        {
            RunOnce(1000);
            RunOnce(10000);
            RunOnce(100000);
        }

        void RunOnce(int count)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            var ptr = Framework.InstancePtrPtr;
            var value = ptr.ReadPtr(); 

            var sw = Stopwatch.StartNew();

            for (int i = 0; i < count; i++)
            {
                ptr.Write<IntPtr>(value);
            }

            sw.Stop();

            double totalMs = sw.Elapsed.TotalMilliseconds;
            double avgNs = sw.ElapsedTicks * 1000000000.0 / Stopwatch.Frequency / count;
            _host.LogSim($"{count} 次；{totalMs:F6} ms；平均 {avgNs:F2} ns/次");
        }

    }
}
#pragma warning restore IDE1006