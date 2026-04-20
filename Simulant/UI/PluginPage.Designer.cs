namespace Simulant.UI
{
    partial class SimulantUI
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitMainV = new System.Windows.Forms.SplitContainer();
            this.tableL = new System.Windows.Forms.TableLayoutPanel();
            this.btnInitPlugin = new System.Windows.Forms.Button();
            this.btnLoadPreset = new System.Windows.Forms.Button();
            this.btnSimEnter = new System.Windows.Forms.Button();
            this.lblPhase = new System.Windows.Forms.Label();
            this.btnSimExit = new System.Windows.Forms.Button();
            this.lblTerritory = new System.Windows.Forms.Label();
            this.splitMainH = new System.Windows.Forms.SplitContainer();
            this.lblPresetAbsent = new System.Windows.Forms.Label();
            this.tableLog = new System.Windows.Forms.TableLayoutPanel();
            this.lblLogRegex = new System.Windows.Forms.Label();
            this.btnLogSearch = new System.Windows.Forms.Button();
            this.txtLogRegex = new System.Windows.Forms.TextBox();
            this.dgvLog = new System.Windows.Forms.DataGridView();
            this.lblLogType = new System.Windows.Forms.Label();
            this.chksLogType = new System.Windows.Forms.CheckedListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitMainV)).BeginInit();
            this.splitMainV.Panel1.SuspendLayout();
            this.splitMainV.Panel2.SuspendLayout();
            this.splitMainV.SuspendLayout();
            this.tableL.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMainH)).BeginInit();
            this.splitMainH.Panel1.SuspendLayout();
            this.splitMainH.Panel2.SuspendLayout();
            this.splitMainH.SuspendLayout();
            this.tableLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLog)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitMainV
            // 
            this.splitMainV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMainV.Font = new System.Drawing.Font("等距更纱黑体 SC", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.splitMainV.Location = new System.Drawing.Point(0, 0);
            this.splitMainV.Name = "splitMainV";
            // 
            // splitMainV.Panel1
            // 
            this.splitMainV.Panel1.Controls.Add(this.tableL);
            // 
            // splitMainV.Panel2
            // 
            this.splitMainV.Panel2.Controls.Add(this.splitMainH);
            this.splitMainV.Size = new System.Drawing.Size(1209, 892);
            this.splitMainV.SplitterDistance = 349;
            this.splitMainV.TabIndex = 0;
            // 
            // tableL
            // 
            this.tableL.AutoSize = true;
            this.tableL.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableL.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tableL.ColumnCount = 2;
            this.tableL.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableL.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableL.Controls.Add(this.btnInitPlugin, 0, 1);
            this.tableL.Controls.Add(this.btnLoadPreset, 0, 2);
            this.tableL.Controls.Add(this.btnSimEnter, 0, 5);
            this.tableL.Controls.Add(this.lblPhase, 0, 4);
            this.tableL.Controls.Add(this.btnSimExit, 1, 5);
            this.tableL.Controls.Add(this.lblTerritory, 0, 3);
            this.tableL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableL.Location = new System.Drawing.Point(0, 0);
            this.tableL.Name = "tableL";
            this.tableL.RowCount = 7;
            this.tableL.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 156F));
            this.tableL.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableL.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableL.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableL.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableL.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableL.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableL.Size = new System.Drawing.Size(349, 892);
            this.tableL.TabIndex = 0;
            this.tableL.TabStop = true;
            this.tableL.Paint += new System.Windows.Forms.PaintEventHandler(this.tableL_Paint);
            // 
            // btnInitPlugin
            // 
            this.btnInitPlugin.AutoSize = true;
            this.btnInitPlugin.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnInitPlugin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInitPlugin.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnInitPlugin.Location = new System.Drawing.Point(15, 166);
            this.btnInitPlugin.Margin = new System.Windows.Forms.Padding(15, 10, 15, 10);
            this.btnInitPlugin.Name = "btnInitPlugin";
            this.btnInitPlugin.Padding = new System.Windows.Forms.Padding(5);
            this.btnInitPlugin.Size = new System.Drawing.Size(144, 42);
            this.btnInitPlugin.TabIndex = 0;
            this.btnInitPlugin.TabStop = false;
            this.btnInitPlugin.Text = "初始化插件";
            this.btnInitPlugin.UseVisualStyleBackColor = true;
            this.btnInitPlugin.Click += new System.EventHandler(this.btnInitPlugin_Click);
            // 
            // btnLoadPreset
            // 
            this.btnLoadPreset.AutoSize = true;
            this.btnLoadPreset.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnLoadPreset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLoadPreset.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLoadPreset.Location = new System.Drawing.Point(15, 228);
            this.btnLoadPreset.Margin = new System.Windows.Forms.Padding(15, 10, 15, 10);
            this.btnLoadPreset.Name = "btnLoadPreset";
            this.btnLoadPreset.Padding = new System.Windows.Forms.Padding(5);
            this.btnLoadPreset.Size = new System.Drawing.Size(144, 42);
            this.btnLoadPreset.TabIndex = 0;
            this.btnLoadPreset.TabStop = false;
            this.btnLoadPreset.Text = "选择预设";
            this.btnLoadPreset.UseVisualStyleBackColor = true;
            // 
            // btnSimEnter
            // 
            this.btnSimEnter.AutoSize = true;
            this.btnSimEnter.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSimEnter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSimEnter.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSimEnter.Location = new System.Drawing.Point(15, 374);
            this.btnSimEnter.Margin = new System.Windows.Forms.Padding(15, 10, 15, 10);
            this.btnSimEnter.Name = "btnSimEnter";
            this.btnSimEnter.Padding = new System.Windows.Forms.Padding(5);
            this.btnSimEnter.Size = new System.Drawing.Size(144, 42);
            this.btnSimEnter.TabIndex = 0;
            this.btnSimEnter.TabStop = false;
            this.btnSimEnter.Text = "加载区域";
            this.btnSimEnter.UseVisualStyleBackColor = true;
            // 
            // lblPhase
            // 
            this.lblPhase.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPhase.AutoSize = true;
            this.tableL.SetColumnSpan(this.lblPhase, 2);
            this.lblPhase.Location = new System.Drawing.Point(15, 332);
            this.lblPhase.Margin = new System.Windows.Forms.Padding(15, 10, 15, 10);
            this.lblPhase.Name = "lblPhase";
            this.lblPhase.Size = new System.Drawing.Size(149, 22);
            this.lblPhase.TabIndex = 1;
            this.lblPhase.Text = "阶段：P6 宇宙天箭";
            this.lblPhase.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // btnSimExit
            // 
            this.btnSimExit.AutoSize = true;
            this.btnSimExit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSimExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSimExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSimExit.Location = new System.Drawing.Point(189, 374);
            this.btnSimExit.Margin = new System.Windows.Forms.Padding(15, 10, 15, 10);
            this.btnSimExit.Name = "btnSimExit";
            this.btnSimExit.Padding = new System.Windows.Forms.Padding(5);
            this.btnSimExit.Size = new System.Drawing.Size(145, 42);
            this.btnSimExit.TabIndex = 0;
            this.btnSimExit.TabStop = false;
            this.btnSimExit.Text = "退出模拟";
            this.btnSimExit.UseVisualStyleBackColor = true;
            // 
            // lblTerritory
            // 
            this.lblTerritory.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTerritory.AutoSize = true;
            this.tableL.SetColumnSpan(this.lblTerritory, 2);
            this.lblTerritory.Location = new System.Drawing.Point(15, 290);
            this.lblTerritory.Margin = new System.Windows.Forms.Padding(15, 10, 15, 10);
            this.lblTerritory.Name = "lblTerritory";
            this.lblTerritory.Size = new System.Drawing.Size(249, 22);
            this.lblTerritory.TabIndex = 0;
            this.lblTerritory.Text = "区域：欧米茄绝境验证战 (1122)";
            // 
            // splitMainH
            // 
            this.splitMainH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMainH.Font = new System.Drawing.Font("等距更纱黑体 SC", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.splitMainH.Location = new System.Drawing.Point(0, 0);
            this.splitMainH.Name = "splitMainH";
            this.splitMainH.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitMainH.Panel1
            // 
            this.splitMainH.Panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.splitMainH.Panel1.Controls.Add(this.lblPresetAbsent);
            // 
            // splitMainH.Panel2
            // 
            this.splitMainH.Panel2.Controls.Add(this.tableLog);
            this.splitMainH.Size = new System.Drawing.Size(856, 892);
            this.splitMainH.SplitterDistance = 423;
            this.splitMainH.TabIndex = 0;
            // 
            // lblPresetAbsent
            // 
            this.lblPresetAbsent.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblPresetAbsent.AutoSize = true;
            this.lblPresetAbsent.Location = new System.Drawing.Point(351, 203);
            this.lblPresetAbsent.Name = "lblPresetAbsent";
            this.lblPresetAbsent.Size = new System.Drawing.Size(138, 22);
            this.lblPresetAbsent.TabIndex = 0;
            this.lblPresetAbsent.Text = "请在左侧选择预设";
            this.lblPresetAbsent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLog
            // 
            this.tableLog.AutoSize = true;
            this.tableLog.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLog.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tableLog.ColumnCount = 3;
            this.tableLog.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLog.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLog.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLog.Controls.Add(this.lblLogRegex, 0, 2);
            this.tableLog.Controls.Add(this.btnLogSearch, 2, 2);
            this.tableLog.Controls.Add(this.txtLogRegex, 1, 2);
            this.tableLog.Controls.Add(this.dgvLog, 0, 0);
            this.tableLog.Controls.Add(this.lblLogType, 2, 0);
            this.tableLog.Controls.Add(this.chksLogType, 2, 1);
            this.tableLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLog.Location = new System.Drawing.Point(0, 0);
            this.tableLog.Name = "tableLog";
            this.tableLog.RowCount = 3;
            this.tableLog.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLog.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLog.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLog.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLog.Size = new System.Drawing.Size(856, 465);
            this.tableLog.TabIndex = 0;
            this.tableLog.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLog_Paint);
            // 
            // lblLogRegex
            // 
            this.lblLogRegex.AutoSize = true;
            this.lblLogRegex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLogRegex.Location = new System.Drawing.Point(15, 420);
            this.lblLogRegex.Margin = new System.Windows.Forms.Padding(15, 10, 15, 10);
            this.lblLogRegex.Name = "lblLogRegex";
            this.lblLogRegex.Size = new System.Drawing.Size(74, 35);
            this.lblLogRegex.TabIndex = 0;
            this.lblLogRegex.Text = "正则搜索";
            this.lblLogRegex.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblLogRegex.Click += new System.EventHandler(this.lblLogRegex_Click);
            // 
            // btnLogSearch
            // 
            this.btnLogSearch.AutoSize = true;
            this.btnLogSearch.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnLogSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLogSearch.Location = new System.Drawing.Point(757, 420);
            this.btnLogSearch.Margin = new System.Windows.Forms.Padding(15, 10, 15, 10);
            this.btnLogSearch.Name = "btnLogSearch";
            this.btnLogSearch.Size = new System.Drawing.Size(84, 35);
            this.btnLogSearch.TabIndex = 0;
            this.btnLogSearch.TabStop = false;
            this.btnLogSearch.Text = "　搜索　";
            this.btnLogSearch.UseVisualStyleBackColor = true;
            // 
            // txtLogRegex
            // 
            this.txtLogRegex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLogRegex.Location = new System.Drawing.Point(119, 423);
            this.txtLogRegex.Margin = new System.Windows.Forms.Padding(15, 13, 15, 13);
            this.txtLogRegex.Name = "txtLogRegex";
            this.txtLogRegex.Size = new System.Drawing.Size(608, 29);
            this.txtLogRegex.TabIndex = 2;
            this.txtLogRegex.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // dgvLog
            // 
            this.dgvLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLog.SetColumnSpan(this.dgvLog, 2);
            this.dgvLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLog.Location = new System.Drawing.Point(3, 3);
            this.dgvLog.Name = "dgvLog";
            this.dgvLog.RowHeadersWidth = 51;
            this.tableLog.SetRowSpan(this.dgvLog, 2);
            this.dgvLog.RowTemplate.Height = 27;
            this.dgvLog.Size = new System.Drawing.Size(736, 404);
            this.dgvLog.TabIndex = 3;
            this.dgvLog.VirtualMode = true;
            // 
            // lblLogType
            // 
            this.lblLogType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblLogType.AutoSize = true;
            this.lblLogType.Location = new System.Drawing.Point(757, 10);
            this.lblLogType.Margin = new System.Windows.Forms.Padding(15, 10, 15, 10);
            this.lblLogType.Name = "lblLogType";
            this.lblLogType.Size = new System.Drawing.Size(74, 22);
            this.lblLogType.TabIndex = 4;
            this.lblLogType.Text = "日志类别";
            this.lblLogType.Click += new System.EventHandler(this.label1_Click);
            // 
            // chksLogType
            // 
            this.chksLogType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chksLogType.FormattingEnabled = true;
            this.chksLogType.Items.AddRange(new object[] {
            "全选",
            "错误",
            "警告",
            "模拟",
            "调用",
            "杂项"});
            this.chksLogType.Location = new System.Drawing.Point(745, 45);
            this.chksLogType.Name = "chksLogType";
            this.chksLogType.Size = new System.Drawing.Size(108, 362);
            this.chksLogType.TabIndex = 5;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(106, 28);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(105, 24);
            this.toolStripMenuItem2.Text = "111";
            // 
            // SimulantUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitMainV);
            this.Name = "SimulantUI";
            this.Size = new System.Drawing.Size(1209, 892);
            this.splitMainV.Panel1.ResumeLayout(false);
            this.splitMainV.Panel1.PerformLayout();
            this.splitMainV.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMainV)).EndInit();
            this.splitMainV.ResumeLayout(false);
            this.tableL.ResumeLayout(false);
            this.tableL.PerformLayout();
            this.splitMainH.Panel1.ResumeLayout(false);
            this.splitMainH.Panel1.PerformLayout();
            this.splitMainH.Panel2.ResumeLayout(false);
            this.splitMainH.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMainH)).EndInit();
            this.splitMainH.ResumeLayout(false);
            this.tableLog.ResumeLayout(false);
            this.tableLog.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLog)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitMainV;
        private System.Windows.Forms.TableLayoutPanel tableL;
        private System.Windows.Forms.SplitContainer splitMainH;
        private System.Windows.Forms.TableLayoutPanel tableLog;
        private System.Windows.Forms.Label lblLogRegex;
        private System.Windows.Forms.Button btnLogSearch;
        private System.Windows.Forms.TextBox txtLogRegex;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.DataGridView dgvLog;
        private System.Windows.Forms.Label lblLogType;
        private System.Windows.Forms.CheckedListBox chksLogType;
        private System.Windows.Forms.Label lblTerritory;
        private System.Windows.Forms.Label lblPhase;
        private System.Windows.Forms.Button btnSimEnter;
        private System.Windows.Forms.Button btnSimExit;
        private System.Windows.Forms.Button btnInitPlugin;
        private System.Windows.Forms.Button btnLoadPreset;
        private System.Windows.Forms.Label lblPresetAbsent;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
    }
}
