using Simulant.ACT;
using Simulant.Game;
using Simulant.Game.FFCS.Client.System.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simulant.Core.Firewall
{
    internal sealed class FirewallService
    {
        private readonly PluginHost _host;

        private byte[] _onReceiveOriginal;

        private byte[] _sendHookOriginal;
        private IntPtr _sendHookCave = IntPtr.Zero;
        private bool _sendHookEnabled;
        private const int SendHookLength = 15;

        public FirewallService(PluginHost host)
        {
            _host = host;
        }

        public bool IsEnabled
            => _onReceiveOriginal != null || _sendHookEnabled;

        public void Enable()
        {
            ReceiveHookEnable();
            SendHookEnable();
        }

        public void Disable()
        {
            ReceiveHookDisable();
            SendHookDisable();
        }

        public void ReceiveHookEnable()
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

        public void ReceiveHookDisable()
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

        public void SendHookEnable()
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

        public void SendHookDisable()
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