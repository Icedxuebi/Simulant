#pragma warning disable IDE1006
using Simulant.ACT;
using Simulant.Core;
using Simulant.Game;
using Simulant.Game.ExtractedCsv;
using Simulant.Game.ExtractedCsv.Rows;
using Simulant.Game.FFCS.Client.System.Framework;
using System;
using System.Collections.Generic;
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
                _host.LogCall("测试：调用日志。");
                _host.LogWarning("测试：警告日志。");
                _host.LogError("测试：错误日志。");

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
            if (!CsvManager.Instance.Get<TerritoryType>().TryGetValue(preset.TerritoryId, out var territory))
            {
                lblTerritory.Text = $"区域: 无 (0)";
                lblPhase.Text = $"阶段: 无";
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

        private unsafe void btnSimEnter_Click(object sender, EventArgs e)
        {
            _host.LogVerbose("测试：btnSimEnter_Click");
        }


        private void btnSimExit_Click(object sender, EventArgs e)
        {
            _host.LogVerbose("测试：btnSimExit_Click");
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

            _logSnapshot = _host._log.Snapshot();
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


        
        private void btnFirewallOn_Click(object sender, EventArgs e)
        {
            ReceiveHookEnable();
            SendHookEnable();
        }

        private void btnFirewallOff_Click(object sender, EventArgs e)
        {
            try
            {
                ReceiveHookDisable();
                SendHookDisable();
            }
            catch (Exception ex)
            {
                _host.LogError("测试失败：" + ex.Message);
            }
        }

        private byte[] _onReceiveOriginal;
        private void ReceiveHookEnable()
        {
            var dispatcher = Framework.Instance.NetworkModuleProxy.NetworkModule.PacketReceiverCallback.PacketDispatcher;
            _host.LogVerbose($"PacketDispatcher at {(ulong)dispatcher.Ptr:X}");

            var vFuncOnReceivePtr = dispatcher.Ptr.GetVFuncPtr(1);
            _host.LogVerbose($"OnReceivePacket at {(ulong)vFuncOnReceivePtr:X}");

            if (_onReceiveOriginal?.Length != 5)
                _onReceiveOriginal = vFuncOnReceivePtr.ReadBytes(5);

            // 可以拦截全部收包，恢复后不会掉线
            // patch:
            // xor eax, eax     31 C0
            // ret              C3
            vFuncOnReceivePtr.WriteBytes(new byte[] { 0x31, 0xC0, 0xC3, 0x90, 0x90 });
            _host.LogSim("防火墙部分开启，拦截所有收包。");
        }

        private void ReceiveHookDisable()
        {
            if (_onReceiveOriginal == null || _onReceiveOriginal.Length != 5)
            {
                _host.LogError("无法恢复收包，原始数据无效。");
                return;
            }
            var dispatcher = Framework.Instance.NetworkModuleProxy.NetworkModule.PacketReceiverCallback.PacketDispatcher;
            _host.LogVerbose($"PacketDispatcher at {(ulong)dispatcher.Ptr:X}");

            var vFuncOnReceivePtr = dispatcher.Ptr.GetVFuncPtr(1);
            _host.LogVerbose($"OnReceivePacket at {(ulong)vFuncOnReceivePtr:X}");

            vFuncOnReceivePtr.WriteBytes(_onReceiveOriginal);
            _onReceiveOriginal = null;
            _host.LogSim("防火墙已关闭。");
        }

        private byte[] _sendHookOriginal;
        private IntPtr _sendHookCave = IntPtr.Zero;
        private bool _sendHookEnabled;
        private const int SendHookLength = 15;
        private void SendHookEnable()
        {
            try
            {
                if (_sendHookEnabled)
                {
                    _host.LogSim("发送 Hook 已经开启，跳过操作。");
                    return;
                }

                var target = AddressStore.OnSendPacketFuncPtr;
                if (target == IntPtr.Zero)
                {
                    _host.LogError("启用发送 Hook 失败：OnSendPacketFuncPtr 无效。");
                    return;
                }

                _host.LogVerbose($"OnSendPacketFuncPtr at {(ulong)target:X}");

                // 函数开头前 3 条指令：
                // 48 89 5C 24 08
                // 48 89 74 24 10
                // 4C 89 64 24 18
                if (_sendHookOriginal == null || _sendHookOriginal.Length != SendHookLength)
                    _sendHookOriginal = target.ReadBytes(SendHookLength);

                // Allocate 代码洞
                _sendHookCave = NamazuInterop.Plugin.Memory.AllocateMemory(0x100);
                if (_sendHookCave == IntPtr.Zero)
                {
                    _host.LogError("启用发送 Hook 失败：无法分配代码洞。");
                    return;
                }

                _host.LogVerbose($"Send hook cave at {(ulong)_sendHookCave:X}");

                var passOpcode = AddressStore.HeartBeatOpcode;
                var returnAddress = (ulong)target + SendHookLength;

                // 代码洞内容：
                // 判断是否为心跳包，是则放行，否则直接返回（跳转至 block 处）：
                // cmp word ptr [rdx], passOpcode
                // jne block
                //
                // 放行路径：执行被覆盖的指令，跳转回补丁后：
                // <原始指令>
                // jmp qword ptr [rip+0]
                // <8 字节回跳地址>
                //
                // block 路径: return 0
                // xor eax, eax
                // ret
                var cave = new List<byte>();

                // cmp word ptr [rdx], imm16
                // 66 81 3A xx xx
                cave.AddRange(new byte[] { 0x66, 0x81, 0x3A }.Concat(BitConverter.GetBytes(passOpcode)));

                // jne block
                var jnePos = cave.Count;
                cave.AddRange(new byte[] { 0x75, 0x00 });

                // 补执行被入口覆盖掉的原始指令
                cave.AddRange(_sendHookOriginal);

                // jmp qword ptr [rip+0]
                // FF 25 00 00 00 00
                // <8 字节回跳地址>
                cave.AddRange(new byte[] { 0xFF, 0x25, 0x00, 0x00, 0x00, 0x00 });
                cave.AddRange(BitConverter.GetBytes(returnAddress));

                // block:
                var blockPos = cave.Count;
                var blockOffset = blockPos - (jnePos + 2);
                // xor eax, eax
                // ret
                cave.AddRange(new byte[] { 0x31, 0xC0 });
                cave.AddRange(new byte[] { 0xC3 });

                // 回填 jne 的相对偏移
                cave[jnePos + 1] = unchecked((byte)blockOffset);

                _sendHookCave.WriteBytes(cave.ToArray());

                // 入口补丁：
                // FF 25 00 00 00 00
                // <8字节 cave 地址>
                // 90
                var patch = new List<byte>();
                patch.AddRange(new byte[] { 0xFF, 0x25, 0x00, 0x00, 0x00, 0x00 });
                patch.AddRange(BitConverter.GetBytes((ulong)_sendHookCave));
                patch.AddRange(new byte[] { 0x90 });

                target.WriteBytes(patch.ToArray());

                _sendHookEnabled = true;
                _host.LogSim($"发送 Hook 已开启，仅放行心跳包 0x{passOpcode:X4}。");
            }
            catch (Exception ex)
            {
                _host.LogError("启用发送 Hook 失败：" + ex.Message);
            }
        }

        private void SendHookDisable()
        {
            try
            {
                if (!_sendHookEnabled)
                {
                    _host.LogSim("发送 Hook 当前未开启。");
                    return;
                }

                var target = AddressStore.OnSendPacketFuncPtr;
                if (target == IntPtr.Zero)
                {
                    _host.LogError("关闭发送 Hook 失败：OnSendPacketFuncPtr 无效。");
                    return;
                }

                if (_sendHookOriginal == null || _sendHookOriginal.Length != SendHookLength)
                {
                    _host.LogError("关闭发送 Hook 失败：原始字节无效。");
                    return;
                }

                // 恢复原函数入口指令
                target.WriteBytes(_sendHookOriginal);

                // 释放代码洞
                if (_sendHookCave != IntPtr.Zero)
                {
                    NamazuInterop.Plugin.Memory.FreeMemory(_sendHookCave);
                    _sendHookCave = IntPtr.Zero;
                }

                _sendHookEnabled = false;
                _host.LogSim("发送 Hook 已关闭。");
            }
            catch (Exception ex)
            {
                _host.LogError("关闭发送 Hook 失败：" + ex.Message);
            }
        }
    }
}
#pragma warning restore IDE1006