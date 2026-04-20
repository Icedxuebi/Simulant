using Simulant.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulant.UI
{
    internal partial class SimPresetSelectForm : Form
    {
        private List<SimPresetBase> _allPresets;

        public SimPresetBase SelectedPreset { get; private set; }

        public SimPresetSelectForm(SimPresetBase currentPreset = null)
        {
            InitializeComponent();
            SelectedPreset = currentPreset;
        }

        private void SimulationPresetSelectForm_Load(object sender, EventArgs e)
        {
            _allPresets = LoadAllPresets();

        }

        private List<SimPresetBase> LoadAllPresets()
        {
            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t =>
                    typeof(SimPresetBase).IsAssignableFrom(t)
                    && !t.IsAbstract
                    && t.GetConstructor(Type.EmptyTypes) != null)
                .Select(t => (SimPresetBase)Activator.CreateInstance(t))
                .OrderBy(x => x.Kind)
                .ThenBy(x => x.TerritoryId)
                .ThenBy(x => x.Author)
                .ToList();
        }


        private void btnOk_Click(object sender, EventArgs e)
        {
            if (SelectedPreset == null)
                return;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }


        private static string GetKindDisplayName(MapType kind)
        {
            switch (kind)
            {
                case MapType.Ultimate:
                    return "绝本";
                case MapType.Savage:
                    return "零式";
                case MapType.Others:
                    return "其他";
                case MapType.Custom:
                    return "自定义";
                default:
                    return kind.ToString();
            }
        }
    }
}
