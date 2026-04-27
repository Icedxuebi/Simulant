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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimulantUI));
            this.splitMainV = new System.Windows.Forms.SplitContainer();
            this.tableL = new System.Windows.Forms.TableLayoutPanel();
            this.btnDebug = new System.Windows.Forms.Button();
            this.btnInitPlugin = new System.Windows.Forms.Button();
            this.btnSelectTerritory = new System.Windows.Forms.Button();
            this.btnSimEnter = new System.Windows.Forms.Button();
            this.btnSimExit = new System.Windows.Forms.Button();
            this.lblTerritory = new System.Windows.Forms.Label();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.cbxPhase = new System.Windows.Forms.ComboBox();
            this.chkToggleFirewall = new System.Windows.Forms.CheckBox();
            this.lblTerritoryId = new System.Windows.Forms.Label();
            this.numTerritoryId = new System.Windows.Forms.NumericUpDown();
            this.splitMainH = new System.Windows.Forms.SplitContainer();
            this.lblPresetAbsent = new System.Windows.Forms.Label();
            this.tableLog = new System.Windows.Forms.TableLayoutPanel();
            this.chkLogFilterVerbose = new System.Windows.Forms.CheckBox();
            this.chkLogFilterSim = new System.Windows.Forms.CheckBox();
            this.chkLogFilterRuntime = new System.Windows.Forms.CheckBox();
            this.chkLogWarning = new System.Windows.Forms.CheckBox();
            this.chkLogFilterError = new System.Windows.Forms.CheckBox();
            this.chkLogFilterAll = new System.Windows.Forms.CheckBox();
            this.lblLogRegex = new System.Windows.Forms.Label();
            this.btnLogSearch = new System.Windows.Forms.Button();
            this.txtLogRegex = new System.Windows.Forms.TextBox();
            this.dgvLog = new System.Windows.Forms.DataGridView();
            this.colTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitMainV)).BeginInit();
            this.splitMainV.Panel1.SuspendLayout();
            this.splitMainV.Panel2.SuspendLayout();
            this.splitMainV.SuspendLayout();
            this.tableL.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTerritoryId)).BeginInit();
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
            this.splitMainV.SplitterDistance = 356;
            this.splitMainV.TabIndex = 0;
            // 
            // tableL
            // 
            this.tableL.AutoSize = true;
            this.tableL.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableL.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tableL.ColumnCount = 3;
            this.tableL.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableL.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.19101F));
            this.tableL.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.80899F));
            this.tableL.Controls.Add(this.btnDebug, 1, 6);
            this.tableL.Controls.Add(this.btnInitPlugin, 0, 1);
            this.tableL.Controls.Add(this.btnSelectTerritory, 0, 2);
            this.tableL.Controls.Add(this.btnSimEnter, 0, 5);
            this.tableL.Controls.Add(this.btnSimExit, 1, 5);
            this.tableL.Controls.Add(this.lblTerritory, 0, 3);
            this.tableL.Controls.Add(this.picLogo, 0, 0);
            this.tableL.Controls.Add(this.cbxPhase, 0, 4);
            this.tableL.Controls.Add(this.chkToggleFirewall, 1, 1);
            this.tableL.Controls.Add(this.lblTerritoryId, 1, 2);
            this.tableL.Controls.Add(this.numTerritoryId, 2, 2);
            this.tableL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableL.Location = new System.Drawing.Point(0, 0);
            this.tableL.Name = "tableL";
            this.tableL.RowCount = 9;
            this.tableL.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 156F));
            this.tableL.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableL.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableL.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableL.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableL.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableL.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableL.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableL.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableL.Size = new System.Drawing.Size(356, 892);
            this.tableL.TabIndex = 0;
            this.tableL.TabStop = true;
            // 
            // btnDebug
            // 
            this.btnDebug.AutoSize = true;
            this.btnDebug.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableL.SetColumnSpan(this.btnDebug, 2);
            this.btnDebug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDebug.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDebug.Location = new System.Drawing.Point(193, 476);
            this.btnDebug.Margin = new System.Windows.Forms.Padding(15, 10, 15, 10);
            this.btnDebug.Name = "btnDebug";
            this.btnDebug.Padding = new System.Windows.Forms.Padding(5);
            this.btnDebug.Size = new System.Drawing.Size(148, 42);
            this.btnDebug.TabIndex = 6;
            this.btnDebug.TabStop = false;
            this.btnDebug.Text = "调试";
            this.btnDebug.UseVisualStyleBackColor = true;
            this.btnDebug.Click += new System.EventHandler(this.btnDebug_Click);
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
            this.btnInitPlugin.Size = new System.Drawing.Size(148, 42);
            this.btnInitPlugin.TabIndex = 0;
            this.btnInitPlugin.TabStop = false;
            this.btnInitPlugin.Text = "初始化插件";
            this.btnInitPlugin.UseVisualStyleBackColor = true;
            this.btnInitPlugin.Click += new System.EventHandler(this.btnInitPlugin_Click);
            // 
            // btnSelectTerritory
            // 
            this.btnSelectTerritory.AutoSize = true;
            this.btnSelectTerritory.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSelectTerritory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSelectTerritory.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSelectTerritory.Location = new System.Drawing.Point(15, 228);
            this.btnSelectTerritory.Margin = new System.Windows.Forms.Padding(15, 10, 15, 10);
            this.btnSelectTerritory.Name = "btnSelectTerritory";
            this.btnSelectTerritory.Padding = new System.Windows.Forms.Padding(5);
            this.btnSelectTerritory.Size = new System.Drawing.Size(148, 42);
            this.btnSelectTerritory.TabIndex = 0;
            this.btnSelectTerritory.TabStop = false;
            this.btnSelectTerritory.Text = "选择地图";
            this.btnSelectTerritory.UseVisualStyleBackColor = true;
            this.btnSelectTerritory.Click += new System.EventHandler(this.btnSelectTerritory_Click);
            // 
            // btnSimEnter
            // 
            this.btnSimEnter.AutoSize = true;
            this.btnSimEnter.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSimEnter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSimEnter.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSimEnter.Location = new System.Drawing.Point(15, 414);
            this.btnSimEnter.Margin = new System.Windows.Forms.Padding(15, 10, 15, 10);
            this.btnSimEnter.Name = "btnSimEnter";
            this.btnSimEnter.Padding = new System.Windows.Forms.Padding(5);
            this.btnSimEnter.Size = new System.Drawing.Size(148, 42);
            this.btnSimEnter.TabIndex = 0;
            this.btnSimEnter.TabStop = false;
            this.btnSimEnter.Text = "加载区域";
            this.btnSimEnter.UseVisualStyleBackColor = true;
            this.btnSimEnter.Click += new System.EventHandler(this.btnSimEnter_Click);
            // 
            // btnSimExit
            // 
            this.btnSimExit.AutoSize = true;
            this.btnSimExit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableL.SetColumnSpan(this.btnSimExit, 2);
            this.btnSimExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSimExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSimExit.Location = new System.Drawing.Point(193, 414);
            this.btnSimExit.Margin = new System.Windows.Forms.Padding(15, 10, 15, 10);
            this.btnSimExit.Name = "btnSimExit";
            this.btnSimExit.Padding = new System.Windows.Forms.Padding(5);
            this.btnSimExit.Size = new System.Drawing.Size(148, 42);
            this.btnSimExit.TabIndex = 0;
            this.btnSimExit.TabStop = false;
            this.btnSimExit.Text = "退出模拟";
            this.btnSimExit.UseVisualStyleBackColor = true;
            this.btnSimExit.Click += new System.EventHandler(this.btnSimExit_Click);
            // 
            // lblTerritory
            // 
            this.lblTerritory.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTerritory.AutoSize = true;
            this.tableL.SetColumnSpan(this.lblTerritory, 3);
            this.lblTerritory.Location = new System.Drawing.Point(15, 290);
            this.lblTerritory.Margin = new System.Windows.Forms.Padding(15, 10, 15, 10);
            this.lblTerritory.Name = "lblTerritory";
            this.lblTerritory.Size = new System.Drawing.Size(106, 44);
            this.lblTerritory.TabIndex = 0;
            this.lblTerritory.Text = "选中区域：无\r\n选中副本：无";
            // 
            // picLogo
            // 
            this.tableL.SetColumnSpan(this.picLogo, 3);
            this.picLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picLogo.Image = ((System.Drawing.Image)(resources.GetObject("picLogo.Image")));
            this.picLogo.Location = new System.Drawing.Point(20, 20);
            this.picLogo.Margin = new System.Windows.Forms.Padding(20);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(316, 116);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 2;
            this.picLogo.TabStop = false;
            // 
            // cbxPhase
            // 
            this.tableL.SetColumnSpan(this.cbxPhase, 3);
            this.cbxPhase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxPhase.DropDownHeight = 500;
            this.cbxPhase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxPhase.FormattingEnabled = true;
            this.cbxPhase.IntegralHeight = false;
            this.cbxPhase.Location = new System.Drawing.Point(15, 359);
            this.cbxPhase.Margin = new System.Windows.Forms.Padding(15);
            this.cbxPhase.MaxDropDownItems = 15;
            this.cbxPhase.Name = "cbxPhase";
            this.cbxPhase.Size = new System.Drawing.Size(326, 30);
            this.cbxPhase.TabIndex = 8;
            this.cbxPhase.SelectedIndexChanged += new System.EventHandler(this.cbxPhase_SelectedIndexChanged);
            // 
            // chkToggleFirewall
            // 
            this.chkToggleFirewall.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkToggleFirewall.AutoSize = true;
            this.chkToggleFirewall.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableL.SetColumnSpan(this.chkToggleFirewall, 2);
            this.chkToggleFirewall.Location = new System.Drawing.Point(181, 169);
            this.chkToggleFirewall.Name = "chkToggleFirewall";
            this.chkToggleFirewall.Padding = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.chkToggleFirewall.Size = new System.Drawing.Size(117, 36);
            this.chkToggleFirewall.TabIndex = 7;
            this.chkToggleFirewall.Text = "启用防火墙";
            this.chkToggleFirewall.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkToggleFirewall.UseVisualStyleBackColor = true;
            this.chkToggleFirewall.CheckedChanged += new System.EventHandler(this.chkToggleFirewall_CheckedChanged);
            // 
            // lblTerritoryId
            // 
            this.lblTerritoryId.AutoSize = true;
            this.lblTerritoryId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTerritoryId.Location = new System.Drawing.Point(181, 218);
            this.lblTerritoryId.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblTerritoryId.Name = "lblTerritoryId";
            this.lblTerritoryId.Size = new System.Drawing.Size(75, 62);
            this.lblTerritoryId.TabIndex = 9;
            this.lblTerritoryId.Text = "地图 ID";
            this.lblTerritoryId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numTerritoryId
            // 
            this.numTerritoryId.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numTerritoryId.Location = new System.Drawing.Point(266, 234);
            this.numTerritoryId.Margin = new System.Windows.Forms.Padding(10, 10, 15, 10);
            this.numTerritoryId.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numTerritoryId.Name = "numTerritoryId";
            this.numTerritoryId.Size = new System.Drawing.Size(74, 29);
            this.numTerritoryId.TabIndex = 10;
            this.numTerritoryId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numTerritoryId.ValueChanged += new System.EventHandler(this.numTerritoryId_ValueChanged);
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
            this.splitMainH.Size = new System.Drawing.Size(849, 892);
            this.splitMainH.SplitterDistance = 423;
            this.splitMainH.TabIndex = 0;
            // 
            // lblPresetAbsent
            // 
            this.lblPresetAbsent.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblPresetAbsent.AutoSize = true;
            this.lblPresetAbsent.Location = new System.Drawing.Point(347, 203);
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
            this.tableLog.Controls.Add(this.chkLogFilterVerbose, 2, 5);
            this.tableLog.Controls.Add(this.chkLogFilterSim, 2, 4);
            this.tableLog.Controls.Add(this.chkLogFilterRuntime, 2, 3);
            this.tableLog.Controls.Add(this.chkLogWarning, 2, 2);
            this.tableLog.Controls.Add(this.chkLogFilterError, 2, 1);
            this.tableLog.Controls.Add(this.chkLogFilterAll, 2, 0);
            this.tableLog.Controls.Add(this.lblLogRegex, 0, 10);
            this.tableLog.Controls.Add(this.btnLogSearch, 2, 10);
            this.tableLog.Controls.Add(this.txtLogRegex, 1, 10);
            this.tableLog.Controls.Add(this.dgvLog, 0, 0);
            this.tableLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLog.Location = new System.Drawing.Point(0, 0);
            this.tableLog.Name = "tableLog";
            this.tableLog.RowCount = 11;
            this.tableLog.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLog.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLog.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLog.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLog.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLog.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLog.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLog.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLog.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLog.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLog.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLog.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLog.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLog.Size = new System.Drawing.Size(849, 465);
            this.tableLog.TabIndex = 0;
            // 
            // chkLogFilterVerbose
            // 
            this.chkLogFilterVerbose.AutoCheck = false;
            this.chkLogFilterVerbose.AutoSize = true;
            this.chkLogFilterVerbose.Checked = true;
            this.chkLogFilterVerbose.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLogFilterVerbose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkLogFilterVerbose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.chkLogFilterVerbose.Location = new System.Drawing.Point(735, 183);
            this.chkLogFilterVerbose.Margin = new System.Windows.Forms.Padding(0);
            this.chkLogFilterVerbose.Name = "chkLogFilterVerbose";
            this.chkLogFilterVerbose.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
            this.chkLogFilterVerbose.Size = new System.Drawing.Size(114, 36);
            this.chkLogFilterVerbose.TabIndex = 0;
            this.chkLogFilterVerbose.TabStop = false;
            this.chkLogFilterVerbose.Text = "详细";
            this.chkLogFilterVerbose.UseVisualStyleBackColor = true;
            this.chkLogFilterVerbose.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chkLogFilters_MouseClick);
            this.chkLogFilterVerbose.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chkLogFilters_MouseDown);
            // 
            // chkLogFilterSim
            // 
            this.chkLogFilterSim.AutoCheck = false;
            this.chkLogFilterSim.AutoSize = true;
            this.chkLogFilterSim.Checked = true;
            this.chkLogFilterSim.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLogFilterSim.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkLogFilterSim.ForeColor = System.Drawing.Color.SlateBlue;
            this.chkLogFilterSim.Location = new System.Drawing.Point(735, 147);
            this.chkLogFilterSim.Margin = new System.Windows.Forms.Padding(0);
            this.chkLogFilterSim.Name = "chkLogFilterSim";
            this.chkLogFilterSim.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
            this.chkLogFilterSim.Size = new System.Drawing.Size(114, 36);
            this.chkLogFilterSim.TabIndex = 0;
            this.chkLogFilterSim.TabStop = false;
            this.chkLogFilterSim.Text = "模拟";
            this.chkLogFilterSim.UseVisualStyleBackColor = true;
            this.chkLogFilterSim.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chkLogFilters_MouseClick);
            this.chkLogFilterSim.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chkLogFilters_MouseDown);
            // 
            // chkLogFilterRuntime
            // 
            this.chkLogFilterRuntime.AutoCheck = false;
            this.chkLogFilterRuntime.AutoSize = true;
            this.chkLogFilterRuntime.Checked = true;
            this.chkLogFilterRuntime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLogFilterRuntime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkLogFilterRuntime.ForeColor = System.Drawing.Color.SeaGreen;
            this.chkLogFilterRuntime.Location = new System.Drawing.Point(735, 111);
            this.chkLogFilterRuntime.Margin = new System.Windows.Forms.Padding(0);
            this.chkLogFilterRuntime.Name = "chkLogFilterRuntime";
            this.chkLogFilterRuntime.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
            this.chkLogFilterRuntime.Size = new System.Drawing.Size(114, 36);
            this.chkLogFilterRuntime.TabIndex = 0;
            this.chkLogFilterRuntime.TabStop = false;
            this.chkLogFilterRuntime.Text = "运行";
            this.chkLogFilterRuntime.UseVisualStyleBackColor = true;
            this.chkLogFilterRuntime.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chkLogFilters_MouseClick);
            this.chkLogFilterRuntime.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chkLogFilters_MouseDown);
            // 
            // chkLogWarning
            // 
            this.chkLogWarning.AutoCheck = false;
            this.chkLogWarning.AutoSize = true;
            this.chkLogWarning.Checked = true;
            this.chkLogWarning.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLogWarning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkLogWarning.ForeColor = System.Drawing.Color.SaddleBrown;
            this.chkLogWarning.Location = new System.Drawing.Point(735, 75);
            this.chkLogWarning.Margin = new System.Windows.Forms.Padding(0);
            this.chkLogWarning.Name = "chkLogWarning";
            this.chkLogWarning.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
            this.chkLogWarning.Size = new System.Drawing.Size(114, 36);
            this.chkLogWarning.TabIndex = 0;
            this.chkLogWarning.TabStop = false;
            this.chkLogWarning.Text = "警告";
            this.chkLogWarning.UseVisualStyleBackColor = true;
            this.chkLogWarning.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chkLogFilters_MouseClick);
            this.chkLogWarning.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chkLogFilters_MouseDown);
            // 
            // chkLogFilterError
            // 
            this.chkLogFilterError.AutoCheck = false;
            this.chkLogFilterError.AutoSize = true;
            this.chkLogFilterError.Checked = true;
            this.chkLogFilterError.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLogFilterError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkLogFilterError.ForeColor = System.Drawing.Color.Maroon;
            this.chkLogFilterError.Location = new System.Drawing.Point(735, 39);
            this.chkLogFilterError.Margin = new System.Windows.Forms.Padding(0);
            this.chkLogFilterError.Name = "chkLogFilterError";
            this.chkLogFilterError.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
            this.chkLogFilterError.Size = new System.Drawing.Size(114, 36);
            this.chkLogFilterError.TabIndex = 0;
            this.chkLogFilterError.TabStop = false;
            this.chkLogFilterError.Text = "错误";
            this.chkLogFilterError.UseVisualStyleBackColor = true;
            this.chkLogFilterError.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chkLogFilters_MouseClick);
            this.chkLogFilterError.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chkLogFilters_MouseDown);
            // 
            // chkLogFilterAll
            // 
            this.chkLogFilterAll.AutoCheck = false;
            this.chkLogFilterAll.AutoSize = true;
            this.chkLogFilterAll.Checked = true;
            this.chkLogFilterAll.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.chkLogFilterAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkLogFilterAll.Location = new System.Drawing.Point(735, 3);
            this.chkLogFilterAll.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.chkLogFilterAll.Name = "chkLogFilterAll";
            this.chkLogFilterAll.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
            this.chkLogFilterAll.Size = new System.Drawing.Size(114, 36);
            this.chkLogFilterAll.TabIndex = 0;
            this.chkLogFilterAll.TabStop = false;
            this.chkLogFilterAll.Text = "全选";
            this.chkLogFilterAll.ThreeState = true;
            this.chkLogFilterAll.UseVisualStyleBackColor = true;
            this.chkLogFilterAll.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chkLogFilterAll_MouseClick);
            this.chkLogFilterAll.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chkLogFilterAll_MouseDown);
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
            // 
            // btnLogSearch
            // 
            this.btnLogSearch.AutoSize = true;
            this.btnLogSearch.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnLogSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLogSearch.Location = new System.Drawing.Point(750, 420);
            this.btnLogSearch.Margin = new System.Windows.Forms.Padding(15, 10, 15, 10);
            this.btnLogSearch.Name = "btnLogSearch";
            this.btnLogSearch.Size = new System.Drawing.Size(84, 35);
            this.btnLogSearch.TabIndex = 0;
            this.btnLogSearch.TabStop = false;
            this.btnLogSearch.Text = "　搜索　";
            this.btnLogSearch.UseVisualStyleBackColor = true;
            this.btnLogSearch.Click += new System.EventHandler(this.btnLogSearch_Click);
            // 
            // txtLogRegex
            // 
            this.txtLogRegex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLogRegex.Location = new System.Drawing.Point(107, 423);
            this.txtLogRegex.Margin = new System.Windows.Forms.Padding(3, 13, 3, 13);
            this.txtLogRegex.Name = "txtLogRegex";
            this.txtLogRegex.Size = new System.Drawing.Size(625, 29);
            this.txtLogRegex.TabIndex = 2;
            this.txtLogRegex.TextChanged += new System.EventHandler(this.txtLogRegex_TextChanged);
            this.txtLogRegex.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtLogRegex_KeyDown);
            // 
            // dgvLog
            // 
            this.dgvLog.AllowUserToAddRows = false;
            this.dgvLog.AllowUserToDeleteRows = false;
            this.dgvLog.AllowUserToResizeRows = false;
            this.dgvLog.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTime,
            this.colType,
            this.colText});
            this.tableLog.SetColumnSpan(this.dgvLog, 2);
            this.dgvLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLog.Location = new System.Drawing.Point(3, 3);
            this.dgvLog.Name = "dgvLog";
            this.dgvLog.ReadOnly = true;
            this.dgvLog.RowHeadersVisible = false;
            this.dgvLog.RowHeadersWidth = 51;
            this.tableLog.SetRowSpan(this.dgvLog, 10);
            this.dgvLog.RowTemplate.Height = 27;
            this.dgvLog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLog.Size = new System.Drawing.Size(729, 404);
            this.dgvLog.TabIndex = 0;
            this.dgvLog.TabStop = false;
            this.dgvLog.VirtualMode = true;
            this.dgvLog.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvLog_CellFormatting);
            this.dgvLog.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgvLog_CellValueNeeded);
            // 
            // colTime
            // 
            this.colTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colTime.HeaderText = "时间";
            this.colTime.MinimumWidth = 6;
            this.colTime.Name = "colTime";
            this.colTime.ReadOnly = true;
            this.colTime.Width = 71;
            // 
            // colType
            // 
            this.colType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colType.HeaderText = "类别";
            this.colType.MinimumWidth = 6;
            this.colType.Name = "colType";
            this.colType.ReadOnly = true;
            this.colType.Width = 71;
            // 
            // colText
            // 
            this.colText.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colText.HeaderText = "信息";
            this.colText.MinimumWidth = 6;
            this.colText.Name = "colText";
            this.colText.ReadOnly = true;
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
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTerritoryId)).EndInit();
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
        private System.Windows.Forms.Label lblTerritory;
        private System.Windows.Forms.Button btnSimEnter;
        private System.Windows.Forms.Button btnSimExit;
        private System.Windows.Forms.Button btnInitPlugin;
        private System.Windows.Forms.Button btnSelectTerritory;
        private System.Windows.Forms.Label lblPresetAbsent;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colText;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.CheckBox chkLogFilterVerbose;
        private System.Windows.Forms.CheckBox chkLogFilterSim;
        private System.Windows.Forms.CheckBox chkLogFilterRuntime;
        private System.Windows.Forms.CheckBox chkLogWarning;
        private System.Windows.Forms.CheckBox chkLogFilterError;
        private System.Windows.Forms.CheckBox chkLogFilterAll;
        private System.Windows.Forms.Button btnDebug;
        private System.Windows.Forms.CheckBox chkToggleFirewall;
        private System.Windows.Forms.ComboBox cbxPhase;
        private System.Windows.Forms.Label lblTerritoryId;
        private System.Windows.Forms.NumericUpDown numTerritoryId;
    }
}
