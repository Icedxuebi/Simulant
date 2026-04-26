using Simulant.Core.Environment;
using Simulant.Game;
using Simulant.Game.ExtractedCsv;
using Simulant.Game.ExtractedCsv.Rows;
using Simulant.Game.FFCS.Client.Game;
using Simulant.Game.FFCS.Client.Game.Event;
using System;
using System.Numerics;

namespace Simulant.Core.Zone
{
    internal sealed class ZoneService
    {
        private readonly PluginHost _host;
        public ZoneService(PluginHost host)
        {
            _host = host;
        }

        // 用于下次切换区域时清理旧 instance
        private ushort? _loadedInstanceContentId;

        // 进入模拟区域前记录真实地图和位置，退出时返回
        private uint? _initialTerritoryId;
        private Vector3? _initialPosition;

        private bool _isInSimulatedTerritory;
        public bool IsInSimulatedTerritory => _isInSimulatedTerritory;

        public bool TryEnterTerritory(int territoryId, int storyProgress = 0, byte a4 = 0, byte a5 = 0)
        {
            try
            {
                var gameMain = GameMain.Instance;
                if (gameMain.IsNull())
                    throw new InvalidOperationException("GameMain.Instance 为空。");

                // 只有从非模拟状态进入区域时，才记录真实地图和位置
                if (!_isInSimulatedTerritory)
                {
                    RecordInitialState(gameMain);
                }

                _ = CsvManager.Instance.Get<TerritoryType>().TryGetValue(territoryId, out TerritoryType territory);

                LoadTerritoryInternal(territoryId, territory, storyProgress, a4, a5);

                _isInSimulatedTerritory = true;

                var finalTerritoryId = territory != null ? (int)territory.Index : territoryId;
                var placeName = territory != null ? territory.PlaceName.Name : "未知区域";
                _host.LogSim($"已进入区域：{placeName} ({finalTerritoryId})");

                return true;
            }
            catch (Exception ex)
            {
                _host.LogError("进入区域失败：" + ex.Message);
                return false;
            }
        }

        public bool EnterPhase(PhaseData phaseData)
        {
            try
            {
                if (!_isInSimulatedTerritory)
                    throw new InvalidOperationException("当前不在模拟区域中，无法进入阶段。");

                ApplyPhaseData(phaseData);
                return true;
            }
            catch (Exception ex)
            {
                _host.LogError("进入阶段失败：" + ex.Message);
                return false;
            }
        }

        public bool ExitToInitialTerritory()
        {
            try
            {
                if (!_isInSimulatedTerritory)
                {
                    _host.LogWarning("当前不在模拟区域中，跳过退出。");
                    return false;
                }

                if (!_initialTerritoryId.HasValue || _initialTerritoryId.Value == 0)
                    throw new InvalidOperationException("尚未记录真实地图，无法退出。");

                var territoryId = (int)_initialTerritoryId.Value;

                _ = CsvManager.Instance.Get<TerritoryType>().TryGetValue(territoryId, out TerritoryType territory);

                LoadTerritoryInternal(territoryId, territory);

                RestoreInitialPosition();

                _isInSimulatedTerritory = false;
                _initialTerritoryId = null;
                _initialPosition = null;

                var finalTerritoryId = territory != null ? (int)territory.Index : territoryId;
                var placeName = territory != null ? territory.PlaceName.Name : "未知区域";
                _host.LogSim($"已返回真实区域：{placeName} ({finalTerritoryId})");

                return true;
            }
            catch (Exception ex)
            {
                _host.LogError("返回真实区域失败：" + ex.Message);
                return false;
            }
        }

        private void RecordInitialState(GameMain gameMain)
        {
            var currentTerritoryId = gameMain.CurrentTerritoryTypeId.Get();
            if (currentTerritoryId != 0)
            {
                _initialTerritoryId = currentTerritoryId;
                _host.LogRuntime($"已记录真实地图：{currentTerritoryId}");
            }

            var me = _host.EntityProvider.GetMyself();
            if (me != null)
            {
                _initialPosition = me.Pos3D;
                _host.LogRuntime($"已记录真实位置：{_initialPosition.Value.X}, {_initialPosition.Value.Y}, {_initialPosition.Value.Z}");
            }
        }

        private void RestoreInitialPosition()
        {
            if (!_initialPosition.HasValue)
                return;

            var me = _host.EntityProvider.GetMyself()
                ?? throw new InvalidOperationException("无法获取玩家实体，不能恢复真实位置。");

            me.Pos3D = _initialPosition.Value;
            _host.LogRuntime($"已恢复真实位置：{_initialPosition.Value.X}, {_initialPosition.Value.Y}, {_initialPosition.Value.Z}");
        }

        private void ApplyPhaseData(PhaseData phaseData)
        {
            _host.LogSim($"阶段数据：Weather={phaseData.Weather}, BGM={phaseData.BGM}");

            _host.EnvironmentService.SetWeather(phaseData.Weather);
            _host.EnvironmentService.SetBgm(phaseData.BGM);

            for (uint slot = 0; slot < phaseData.MapEffectFlags.Count; slot++)
            {
                var flag = phaseData.MapEffectFlags[(int)slot];
                _host.EnvironmentService.PlayMapEffect(slot, flag);
            }
        }

        private void LoadTerritoryInternal(int territoryId, TerritoryType territory, int storyProgress = 0, byte a4 = 0, byte a5 = 0)
        {
            var eventFramework = EventFramework.Instance;
            if (eventFramework.IsNull())
                throw new InvalidOperationException("EventFramework.Instance 为空。");

            if (_loadedInstanceContentId.HasValue && _loadedInstanceContentId.Value != 0)
            {
                FinalizeInstanceContent(_loadedInstanceContentId.Value);
                _loadedInstanceContentId = null;
            }

            var nextContentId = ResolveInstanceContentId(territory);
            if (nextContentId != 0)
            {
                SetupInstanceContent(nextContentId);
                _loadedInstanceContentId = nextContentId;
            }

            GameMain.Instance.LoadZone((uint)territoryId, storyProgress, a4, a5);
            eventFramework.SetTerritoryTypeId((ushort)territoryId);
        }

        public void SetupInstanceContent(ushort contentId)
        {
            EventFramework.Instance.SetupInstanceContent(0x80030000u + contentId, contentId);
        }

        public void FinalizeInstanceContent(ushort contentId)
        {
            EventFramework.Instance.FinalizeInstanceContent(0x80030000u + contentId);
        }

        private static ushort ResolveInstanceContentId(TerritoryType territory)
        {
            return (ushort)(territory?.ContentFinderCondition?.ContentId ?? 0);
        }
    }
}