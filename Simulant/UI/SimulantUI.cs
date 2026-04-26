#pragma warning disable IDE1006
using Simulant.Core;
using Simulant.Core.Environment;
using Simulant.Game.ExtractedCsv;
using Simulant.Game.ExtractedCsv.Rows;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
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
            UpdateTerritoryData(0);
            UpdateControlStates();
            RefreshLogView();
        }

        private async void btnInitPlugin_Click(object sender, EventArgs e)
        {
            await _host.InitAsync();
        }

        internal void UpdateControlStates()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new System.Action(UpdateControlStates));
                return;
            }

            var csvReady = _host.CsvReady && !_host.CsvLoading;
            var pluginReady = _host.PluginReady && !_host.IsInitializing;
            var firewallReady = pluginReady && _host.FirewallEnabled;

            // 初始化按钮：初始化中禁用，初始化完成后仍可再次点击
            btnInitPlugin.Enabled = !_host.IsInitializing;

            // CSV 加载完：允许选地图、输入区域 ID、选择阶段
            btnSelectTerritory.Enabled = csvReady;
            lblTerritoryId.Enabled = csvReady;
            numTerritoryId.Enabled = csvReady;
            lblTerritory.Enabled = csvReady;
            cbxPhase.Enabled = csvReady;

            // 插件初始化完成：允许防火墙和调试
            chkToggleFirewall.Enabled = pluginReady;
            btnDebug.Enabled = pluginReady;

            // 防火墙开启：允许加载区域、退出模拟
            btnSimEnter.Enabled = firewallReady;
            btnSimExit.Enabled = firewallReady;
        }

        #region Firewall

        private bool _updatingFirewallToggle;

        private void chkToggleFirewall_CheckedChanged(object sender, EventArgs e)
        {
            if (_updatingFirewallToggle)
                return;

            if (chkToggleFirewall.Checked)
            {
                TryEnableFirewall();
            }
            else
            {
                TryDisableFirewall();
            }
        }

        private void TryEnableFirewall()
        {
            try
            {
                _host.FirewallService.Enable();
                _host.LogSim("防火墙已成功开启。");
            }
            catch (Exception ex)
            {
                _host.LogError("防火墙开启失败：" + ex.Message);
                SetFirewallToggleChecked(false);
            }
            finally
            {
                UpdateControlStates();
            }
        }

        private void TryDisableFirewall()
        {
            try
            {
                _host.ZoneService.ExitToInitialTerritory();

                _host.FirewallService.Disable();
                _host.LogSim("防火墙已成功关闭。");
                SetFirewallToggleChecked(false);
            }
            catch (Exception ex)
            {
                _host.LogError("防火墙关闭失败：" + ex.Message);
                SetFirewallToggleChecked(_host.FirewallEnabled);
            }
            finally
            {
                UpdateControlStates();
            }
        }

        private void SetFirewallToggleChecked(bool value)
        {
            _updatingFirewallToggle = true;
            chkToggleFirewall.Checked = value;
            _updatingFirewallToggle = false;
        }

        #endregion Firewall

        #region Territory

        private int _selectedTerritoryId;

        private void btnSelectTerritory_Click(object sender, EventArgs e)
        {
            var form = new TerritoryForm();
            if (form.ShowDialog() != DialogResult.OK)
                return;

            var territoryId = form.TerritoryIdResult;
            UpdateTerritoryData(territoryId);
        }

        private void numTerritoryId_ValueChanged(object sender, EventArgs e)
        {
            var territoryId = (int)numTerritoryId.Value;
            UpdateTerritoryData(territoryId);
        }

        private void UpdateTerritoryData(int territoryId)
        {
            _selectedTerritoryId = territoryId;

            var territoryName = "无";
            var instanceName = "无";

            if (territoryId > 0 && CsvManager.Instance.Get<TerritoryType>().TryGetValue(_selectedTerritoryId, out var territory))
            {
                territoryName = territory.PlaceName.Name;
                if (!string.IsNullOrWhiteSpace(territory?.ContentFinderCondition.Name))
                {
                    instanceName = territory.ContentFinderCondition.Name;
                }
            }
            lblTerritory.Text = $"选中区域: {territoryName}\n选中副本：{instanceName}";

            cbxPhase.Items.Clear();
            
            var phases = PhaseData.GetPhases(_selectedTerritoryId);
            cbxPhase.Items.AddRange(phases.Cast<object>().ToArray());

            var sims = Assembly.GetExecutingAssembly().GetTypes() // 应该放在别的地方
                .Where(type =>
                    typeof(SimPresetBase).IsAssignableFrom(type)
                    && !type.IsAbstract
                    && type.GetConstructor(Type.EmptyTypes) != null)
                .Select(type => (SimPresetBase)Activator.CreateInstance(type))
                .Where(preset => preset.TerritoryId == _selectedTerritoryId)
                .OrderBy(preset => preset.Name) // to-do: 用 attribute 排序
                .ToList();
            cbxPhase.Items.AddRange(sims.Cast<object>().ToArray());
            cbxPhase.SelectedIndex = cbxPhase.Items.Count > 0 ? 0 : -1;
        }

        private void btnSimEnter_Click(object sender, EventArgs e)
        {
            if (_selectedTerritoryId <= 0)
            {
                _host.LogError("未选择有效的区域。");
                return;
            }

            PhaseData phaseData;

            if (cbxPhase.SelectedItem is PhaseData p)
            {
                phaseData = p;
            }
            else if (cbxPhase.SelectedItem is SimPresetBase s)
            {
                phaseData = s.Phase;
            }
            else
            {
                _host.LogError("未选择有效的阶段或模拟预设。");
                return;
            }

            if (!_host.ZoneService.TryEnterTerritory(_selectedTerritoryId))
                return;

            _host.ZoneService.EnterPhase(phaseData);
        }

        private void btnSimExit_Click(object sender, EventArgs e)
        {
            _host.ZoneService.ExitToInitialTerritory();
        }

        #endregion Territory

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
                case LogType.Runtime:
                    return chkLogFilterRuntime.Checked;
                case LogType.Sim:
                    return chkLogFilterSim.Checked;
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
                case LogType.Runtime:
                    e.CellStyle.BackColor = LogBgGreen;
                    break;
                case LogType.Sim:
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
            yield return chkLogFilterRuntime;
            yield return chkLogFilterSim;
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

        private async void btnDebug_Click(object sender, EventArgs e)
        {
            await new Test(_host).Run();
        }

    }
}
#pragma warning restore IDE1006