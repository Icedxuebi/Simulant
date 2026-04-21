using Simulant.Core;
using Simulant.Game.ExtractedCsv;
using Simulant.Game.ExtractedCsv.Rows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Simulant.Game.ExtractedCsv.Rows.TerritoryType;

namespace Simulant.UI
{
    internal partial class SimPresetSelectForm : Form
    {
        private List<TerritoryViewItem> _allTerritories;
        private List<TerritoryViewItem> _filteredTerritories;
        private List<SimPresetBase> _allPresets;
        private List<SimPresetBase> _filteredPresets;
        private Dictionary<int, List<SimPresetBase>> _presetsByTerritoryId;

        public SimPresetBase PresetResult { get; private set; }

        public SimPresetSelectForm()
        {
            InitializeComponent();
            LoadTerritoryData();
            LoadPresetData();
            ApplySearch();
        }

        #region Territory

        private sealed class TerritoryViewItem
        {
            public int Id;
            public string IdString;
            public string PlaceName;
            public string InstanceName;
            public string RegionName;
            public string TypeName;
        }

        private void LoadTerritoryData()
        {
            var territoryData = CsvManager.Instance.Get<TerritoryType>();
            // 预留一些空间，游戏更新时不更新数据也能进入地图
            var size = territoryData.Select(t => (int)t.Key).Max() + 100;

            _allTerritories = Enumerable.Range(0, size)
                .Select(i => territoryData.TryGetValue(i, out var t) ? t : null)
                .Select((territory, i) => new TerritoryViewItem
                {
                    Id = i,
                    IdString = i.ToString(),
                    PlaceName = territory?.PlaceName?.Name ?? string.Empty,
                    InstanceName = territory?.ContentFinderCondition?.Name ?? string.Empty,
                    RegionName = territory?.RegionPlaceName?.Name ?? string.Empty,
                    TypeName = ((TerritoryIntendedUseEnum?)territory?.TerritoryIntendedUse)?.ToString() ?? string.Empty,
                })
                .ToList();
            _filteredTerritories = new List<TerritoryViewItem>(_allTerritories);
        }

        private Regex _cachedRegex;
        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _cachedRegex = string.IsNullOrWhiteSpace(txtFilter.Text) ? null : new Regex(txtFilter.Text, RegexOptions.IgnoreCase);
                (sender as TextBox).BackColor = SystemColors.Window;
                ApplySearch();
            }
            catch
            {
                (sender as TextBox).BackColor = Color.SeaShell;
            }
        }

        private void ApplySearch()
        {
            dgvTerritory.Rows.Clear(); // 不加这个就会卡

            IEnumerable<TerritoryViewItem> result;
            if (_cachedRegex == null)
            {
                result = _allTerritories.AsEnumerable();
            }
            else
            {
                result = _allTerritories.Where(t =>
                    _cachedRegex.IsMatch(t.IdString) ||
                    _cachedRegex.IsMatch(t.InstanceName) ||
                    _cachedRegex.IsMatch(t.RegionName)
                );
            }
            if (chkPresetOnly.Checked)
            {
                result = result.Where(t => _presetsByTerritoryId.ContainsKey(t.Id));
            }
            _filteredTerritories = result.ToList();
            dgvTerritory.RowCount = _filteredTerritories.Count;
            dgvTerritory_SelectionChanged(null, null);
        }

        private void dgvTerritory_SelectionChanged(object sender, EventArgs e)
        {
            var rowIndex = dgvTerritory.CurrentCell?.RowIndex ?? -1;
            var territoryId = 0;
            var territoryPresets = Enumerable.Empty<SimPresetBase>();

            if (rowIndex >= 0 && rowIndex < _filteredTerritories.Count)
            {
                var territory = _filteredTerritories[rowIndex];
                territoryId = territory.Id;
                if (_presetsByTerritoryId.TryGetValue(territoryId, out var presets))
                    territoryPresets = presets.OrderBy(p => p.Name);
            }
            LoadCurrentPresets(territoryPresets, territoryId);
        }

        private void dgvTerritory_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if ((uint)e.RowIndex >= (uint)_filteredTerritories.Count)
                return;

            var territory = _filteredTerritories[e.RowIndex];
            switch (dgvTerritory.Columns[e.ColumnIndex].Name)
            {
                case "colId":
                    e.Value = territory.IdString; break;
                case "colPlace":
                    e.Value = territory.PlaceName; break;
                case "colInstance":
                    e.Value = territory.InstanceName; break;
                case "colRegion":
                    e.Value = territory.RegionName; break;
                case "colType":
                    e.Value = territory.TypeName; break;
            }
        }

        private void dgvTerritory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

        }

        private void chkPresetOnly_CheckedChanged(object sender, EventArgs e)
        {
            ApplySearch();
        }

        #endregion Territory

        #region Preset

        private void LoadPresetData()
        {
            _allPresets = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t =>
                    typeof(SimPresetBase).IsAssignableFrom(t)
                    && !t.IsAbstract
                    && t.GetConstructor(Type.EmptyTypes) != null)
                .Select(t => (SimPresetBase)Activator.CreateInstance(t))
                .OrderBy(x => x.TerritoryId)
                .ThenBy(x => x.Name)
                .ToList();
            _filteredPresets = new List<SimPresetBase>();
            _presetsByTerritoryId = _allPresets.GroupBy(p => p.TerritoryId).ToDictionary(g => g.Key, g => g.ToList());
        }

        private void LoadCurrentPresets(IEnumerable<SimPresetBase> territoryPresets, int territoryId)
        {
            _filteredPresets.Clear();
            _filteredPresets.Add(new EnterMapOnlyPreset(territoryId));
            _filteredPresets.AddRange(territoryPresets);

            dgvPreset.Rows.Clear();
            dgvPreset.Rows.Add(_filteredPresets.Count);
            for (int i = 0; i < _filteredPresets.Count; i++)
            {
                dgvPreset.Rows[i].Cells[0].Value = _filteredPresets[i].Name;
            }

            if (_filteredPresets.Count > 0)
            {
                dgvPreset.ClearSelection();
                dgvPreset.Rows[0].Selected = true;
                if (dgvPreset.Rows[0].Cells.Count > 0)
                    dgvPreset.CurrentCell = dgvPreset.Rows[0].Cells[0];
            }
            else
            {
                txtInfo.Clear();
            }
        }

        private sealed class EnterMapOnlyPreset : SimPresetBase
        {
            private readonly int _territoryId;
            public EnterMapOnlyPreset(int territoryId)
            {
                _territoryId = territoryId;
            }

            public override int TerritoryId => _territoryId;
            public override string Name { get; } = "[仅进入地图]";
            public override string Author { get; } = null;
            public override DateTime LastUpdated { get; } = default;
            public override Type SimLogicType { get; } = null;
            public override string Description { get; } = "仅进入地图，不执行任何模拟。";
        }

        private void dgvPreset_SelectionChanged(object sender, EventArgs e)
        {
            var rowIndex = dgvPreset.CurrentCell?.RowIndex ?? -1;
            if (rowIndex < 0 || rowIndex >= _filteredPresets.Count)
            {
                txtInfo.Clear();
                return;
            }
            txtInfo.Text = GetPresetInfoText(_filteredPresets[rowIndex]);
        }

        private string GetPresetInfoText(SimPresetBase preset)
        {
            if (preset == null)
                return string.Empty;

            var sb = new StringBuilder();
            var territoryId = preset.TerritoryId;
            var territory = _allTerritories[territoryId];
            var placeName = territory.PlaceName;
            sb.AppendLine($"地图：{(string.IsNullOrEmpty(placeName) ? "未知" : placeName)} ({territoryId})");

            var instanceName = territory.InstanceName;
            if (!string.IsNullOrEmpty(instanceName))
                sb.AppendLine($"副本：{instanceName}");

            sb.AppendLine($"阶段：{preset.Name}");

            if (!string.IsNullOrEmpty(preset.Author))
                sb.AppendLine($"作者：{preset.Author}");

            if (preset.LastUpdated != default)
                sb.AppendLine($"更新：{preset.LastUpdated:yyyy-MM-dd}");

            sb.AppendLine($"说明：{preset.Description}");

            return sb.ToString();
        }



        #endregion Preset


        private void btnOk_Click(object sender, EventArgs e)
        {
            var presetIdx = dgvPreset.CurrentCell?.RowIndex ?? -1;
            if (presetIdx >= 0 && presetIdx < _filteredPresets.Count)
            {
                PresetResult = _filteredPresets[presetIdx];
            }
            else
            {
                var rowIndex = dgvTerritory.CurrentCell?.RowIndex ?? -1;
                if (rowIndex < 0 || rowIndex >= _filteredTerritories.Count)
                    return;

                PresetResult = new EnterMapOnlyPreset(_filteredTerritories[rowIndex].Id);
            }
            DialogResult = DialogResult.OK;
            Close();
        }

    }
}
