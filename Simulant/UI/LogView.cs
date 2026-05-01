#pragma warning disable IDE1006
using Simulant.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Simulant.UI
{
    public partial class LogView : UserControl
    {
        private PluginHost _host;

        public LogView()
        {
            InitializeComponent();
        }

        internal void Bind(PluginHost host)
        {
            _host = host;
            RefreshLogView();
        }

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
                BeginInvoke(new Action(RefreshLogView));
                return;
            }

            if (_host == null)
                return;

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
                try
                {
                    if (dgvLog.Visible && dgvLog.DisplayedRowCount(false) > 0)
                        dgvLog.FirstDisplayedScrollingRowIndex = dgvLog.RowCount - 1;
                }
                catch (InvalidOperationException)
                {
                    // in case:
                    // System.InvalidOperationException: 没有可供显示行的空间。
                    // 在 System.Windows.Forms.DataGridView.set_FirstDisplayedScrollingRowIndex(Int32 value)
                }
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
    }
}
#pragma warning restore IDE1006