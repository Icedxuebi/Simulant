using Simulant.Game;
using Simulant.Game.ExtractedCsv;
using Simulant.Game.ExtractedCsv.Rows;
using Simulant.Game.FFCS.Client.Game;
using Simulant.Game.FFCS.Client.Game.Event;
using System;

namespace Simulant.Core.Zone
{
    internal sealed class ZoneService
    {
        private readonly PluginHost _host;
        private readonly ZoneSelectionState _selection;

        private ushort? _loadedInstanceContentId;
        private uint? _initialTerritoryId;

        public ZoneService(PluginHost host, ZoneSelectionState selection)
        {
            _host = host;
            _selection = selection;
        }

        public ZoneSelectionState Selection
        {
            get { return _selection; }
        }

        public TerritoryType SelectPreset(SimPresetBase preset)
        {
            if (preset == null)
                throw new ArgumentNullException(nameof(preset));

            _ = CsvManager.Instance.Get<TerritoryType>().TryGetValue(preset.TerritoryId, out TerritoryType territory);

            _selection.Set(preset, preset.TerritoryId, territory);

            if (territory == null)
            {
                _host.LogWarning($"已保存区域选择，但 CSV 中未找到 TerritoryType {preset.TerritoryId}。");
            }
            else
            {
                _host.LogSim($"已保存区域选择：{preset.Name} ({preset.TerritoryId})");
            }

            return territory;
        }

        public bool EnterSelectedTerritory(int storyProgress = 0, byte a4 = 0, byte a5 = 0)
        {
            if (!_selection.HasSelection)
            {
                _host.LogWarning("请先读取区域并确认选择。");
                return false;
            }

            return EnterTerritory(_selection.TerritoryId, storyProgress, a4, a5);
        }

        public bool EnterTerritory(int territoryId, int storyProgress = 0, byte a4 = 0, byte a5 = 0)
        {
            _ = CsvManager.Instance.Get<TerritoryType>().TryGetValue(territoryId, out TerritoryType territory);

            return EnterTerritoryInternal(territoryId, territory, true, false, storyProgress, a4, a5);
        }

        public bool ExitToInitialTerritory()
        {
            if (!_initialTerritoryId.HasValue || _initialTerritoryId.Value == 0)
            {
                _host.LogWarning("尚未记录初始地图，无法退出。");
                return false;
            }

            var territoryId = (int)_initialTerritoryId.Value;
            _ = CsvManager.Instance.Get<TerritoryType>().TryGetValue(territoryId, out TerritoryType territory);

            return EnterTerritoryInternal(territoryId, territory, false, true);
        }

        private bool EnterTerritoryInternal(int territoryId, TerritoryType territory, bool updateSelection, bool isExit, int storyProgress = 0, byte a4 = 0, byte a5 = 0)
        {
            try
            {
                var gameMain = GameMain.Instance;
                if (gameMain.IsNull())
                    throw new InvalidOperationException("GameMain.Instance 为空。");

                if (!_initialTerritoryId.HasValue)
                {
                    var currentTerritoryId = gameMain.CurrentTerritoryTypeId.Get();
                    if (currentTerritoryId != 0)
                    {
                        _initialTerritoryId = currentTerritoryId;
                        _host.LogSim($"已记录初始地图：{currentTerritoryId}");
                    }
                }

                LoadTerritoryInternal(territoryId, territory, storyProgress, a4, a5);

                if (updateSelection)
                    _selection.Set(_selection.Preset, territoryId, territory);

                if (isExit)
                {
                    if (territory == null)
                    {
                        _host.LogSim($"已返回初始区域：{territoryId}");
                    }
                    else if (!string.IsNullOrWhiteSpace(territory.ContentFinderCondition.Name))
                    {
                        _host.LogSim($"已返回初始副本：{territory.ContentFinderCondition.Name} ({territoryId})");
                    }
                    else
                    {
                        _host.LogSim($"已返回初始区域：{territory.PlaceName.Name} ({territoryId})");
                    }
                }
                else
                {
                    if (territory == null)
                    {
                        _host.LogSim($"已调用进入区域：{territoryId}");
                    }
                    else if (!string.IsNullOrWhiteSpace(territory.ContentFinderCondition.Name))
                    {
                        _host.LogSim($"已调用进入副本：{territory.ContentFinderCondition.Name} ({territoryId})");
                    }
                    else
                    {
                        _host.LogSim($"已调用进入区域：{territory.PlaceName.Name} ({territoryId})");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                if (isExit)
                {
                    _host.LogError("返回初始区域失败：" + ex.Message);
                }
                else
                {
                    _host.LogError("进入区域失败：" + ex.Message);
                }
                return false;
            }
        }

        private void LoadTerritoryInternal(int territoryId, TerritoryType territory, int storyProgress = 0, byte a4 = 0, byte a5 = 0)
        {
            var eventFramework = EventFramework.Instance;
            if (eventFramework.IsNull())
                throw new InvalidOperationException("EventFramework.Instance 为空。");

            if (_loadedInstanceContentId.HasValue && _loadedInstanceContentId.Value != 0)
            {
                _host.LogCall($"FinalizeInstanceContent: 0x{0x80030000u + _loadedInstanceContentId.Value:X8}");
                FinalizeInstanceContent(_loadedInstanceContentId.Value);
                _loadedInstanceContentId = null;
            }

            var nextContentId = ResolveInstanceContentId(territory);
            if (nextContentId != 0)
            {
                _host.LogCall($"SetupInstanceContent: eventId=0x{0x80030000u + nextContentId:X8}, contentId={nextContentId}");
                SetupInstanceContent(nextContentId);
                _loadedInstanceContentId = nextContentId;
            }

            _host.LogCall($"LoadZone: territory={territoryId}, storyProgress={storyProgress}, arg4={a4}, arg5={a5}");
            GameMain.Instance.LoadZone((uint)territoryId, storyProgress, a4, a5);

            _host.LogCall($"SetTerritoryTypeId: {territoryId}");
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
            if (territory == null)
                return 0;

            if (territory.ContentFinderConditionId == 0)
                return 0;

            var cfc = territory.ContentFinderCondition;
            if (cfc == null)
                return 0;

            return (ushort)cfc.ContentId;
        }
    }
}