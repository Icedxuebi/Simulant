using Simulant.Game.ExtractedCsv;
using Simulant.Game.ExtractedCsv.Rows;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static Simulant.Game.ExtractedCsv.Rows.TerritoryType;

namespace Simulant.UI
{
    internal partial class TerritoryForm : Form
    {
        private List<TerritoryViewItem> _allTerritories;
        private List<TerritoryViewItem> _filteredTerritories;

        public int TerritoryIdResult { get; private set; }

        public TerritoryForm()
        {
            InitializeComponent();
            LoadTerritoryData();
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
                _cachedRegex = string.IsNullOrWhiteSpace(txtFilter.Text)
                    ? null
                    : new Regex(txtFilter.Text, RegexOptions.IgnoreCase);

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
                    _cachedRegex.IsMatch(t.PlaceName) ||
                    _cachedRegex.IsMatch(t.RegionName)
                );
            }

            _filteredTerritories = result.ToList();
            dgvTerritory.RowCount = _filteredTerritories.Count;
        }

        private void dgvTerritory_SelectionChanged(object sender, EventArgs e)
        {
            // preset 相关内容已移除；这里暂时不需要处理选中变化。
        }

        private void dgvTerritory_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if ((uint)e.RowIndex >= (uint)_filteredTerritories.Count)
                return;

            var territory = _filteredTerritories[e.RowIndex];

            switch (dgvTerritory.Columns[e.ColumnIndex].Name)
            {
                case "colId":
                    e.Value = territory.IdString;
                    break;

                case "colPlace":
                    e.Value = territory.PlaceName;
                    break;

                case "colInstance":
                    e.Value = territory.InstanceName;
                    break;

                case "colRegion":
                    e.Value = territory.RegionName;
                    break;

                case "colType":
                    e.Value = territory.TypeName;
                    break;
            }
        }

        private void dgvTerritory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
        }

        #endregion Territory

        private void btnOk_Click(object sender, EventArgs e)
        {
            var rowIndex = dgvTerritory.CurrentCell?.RowIndex ?? -1;
            if (rowIndex < 0 || rowIndex >= _filteredTerritories.Count)
                return;

            TerritoryIdResult = _filteredTerritories[rowIndex].Id;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}