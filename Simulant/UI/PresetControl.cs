using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulant.UI
{
    public partial class PresetControl : UserControl
    {
        public PresetControl()
        {
            InitializeComponent();
        }

        #region Copy Style from dummy controls

        internal void ApplyLabelStyle(Label label)
        {
            CopyCommonStyle(dummyLblTxt, label);
        }

        internal void ApplyCheckBoxStyle(CheckBox checkBox)
        {
            CopyCommonStyle(dummyChk, checkBox);

            checkBox.AutoSize = dummyChk.AutoSize;
            checkBox.TextAlign = dummyChk.TextAlign;
            checkBox.UseVisualStyleBackColor = dummyChk.UseVisualStyleBackColor;
        }

        internal void ApplyTextBoxStyle(TextBox textBox)
        {
            CopyCommonStyle(dummyTxt, textBox);
        }

        internal void ApplyComboBoxStyle(ComboBox comboBox)
        {
            CopyCommonStyle(dummyCbx, comboBox);

            comboBox.DropDownHeight = dummyCbx.DropDownHeight;
            comboBox.DropDownStyle = dummyCbx.DropDownStyle;
            comboBox.FormattingEnabled = dummyCbx.FormattingEnabled;
            comboBox.IntegralHeight = dummyCbx.IntegralHeight;
            comboBox.TabStop = dummyCbx.TabStop;
        }

        internal void ApplyNumericUpDownStyle(NumericUpDown num)
        {
            CopyCommonStyle(dummyNud, num);
        }

        private static void CopyCommonStyle(Control source, Control target)
        {
            if (source == null || target == null)
                return;

            target.Anchor = source.Anchor;
            target.AutoSize = source.AutoSize;
            target.Dock = source.Dock;
            target.Margin = source.Margin;
            target.Padding = source.Padding;
            target.Font = source.Font;
            target.ForeColor = source.ForeColor;
            target.BackColor = source.BackColor;
            target.Cursor = source.Cursor;
        }

        #endregion Copy Style from dummy controls
    }
}
