using Simulant.UI;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace Simulant.Simulation
{
    public abstract class SimOptionBase
    {
        public string PropertyName { get; protected set; }
        protected string _labelText;
        protected string _hint;

        public Label Label { get; protected set; }
        public Control Control { get; protected set; }

        protected virtual Label CreateLabel(PresetControl presetControl)
        {
            var label = new Label
            {
                Text = _labelText,
            };
            presetControl.ApplyLabelStyle(label);
            return label;
        }

        protected abstract Control CreateControl(PresetControl presetControl);

        internal virtual void CreateUI(PresetControl presetControl, ToolTip toolTip)
        {
            Label = CreateLabel(presetControl);
            Control = CreateControl(presetControl);

            var table = presetControl.tableOptions;
            table.RowCount += 1;
            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            table.Controls.Add(Label, 0, table.RowCount - 1);
            if (Control != null)
            {
                table.Controls.Add(Control, 1, table.RowCount - 1);
            }
            else
            {
                table.SetColumnSpan(Label, 2);
            }

            SetToolTipSafe(toolTip, Label, _hint);
            SetToolTipSafe(toolTip, Control, _hint);
        }

        private static void SetToolTipSafe(ToolTip toolTip, Control control, string hint)
        {
            if (string.IsNullOrEmpty(hint) || toolTip == null || control == null)
                return;

            toolTip.SetToolTip(control, hint);
            control.Cursor = Cursors.Help;
        }

        public virtual T GetValue<T>()
        { 
            var genericOption = this as SimOption<T> ?? throw new System.Exception($"Option '{PropertyName}' is not of type {typeof(T).Name}");
            return genericOption.GetValue();
        }

        internal virtual void ApplyTo(object target)
        {
            // Implemented in SimOption<T> and will be ignored when the option is not SimOption<T>, such as LabelOption.
        }
    }
}
