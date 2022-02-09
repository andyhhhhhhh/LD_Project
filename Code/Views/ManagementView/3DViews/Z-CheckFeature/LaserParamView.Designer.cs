namespace ManagementView._3DViews
{
    partial class LaserParamView
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
            this.skinLabel1 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel2 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel4 = new CCWin.SkinControl.SkinLabel();
            this.trackBarExposure = new CCWin.SkinControl.SkinTrackBar();
            this.skinLabel5 = new CCWin.SkinControl.SkinLabel();
            this.trackbarGain = new CCWin.SkinControl.SkinTrackBar();
            this.chkEnableGain = new CCWin.SkinControl.SkinCheckBox();
            this.skinToolTip1 = new CCWin.SkinToolTip(this.components);
            this.btnSave = new System.Windows.Forms.Button();
            this.skinLabel6 = new CCWin.SkinControl.SkinLabel();
            this.trackBarBrightness = new CCWin.SkinControl.SkinTrackBar();
            this.skinLabel7 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel8 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel9 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel10 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel11 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel12 = new CCWin.SkinControl.SkinLabel();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.panelView = new DevComponents.DotNetBar.PanelEx();
            this.panel = new DevComponents.DotNetBar.PanelEx();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.skinGroupBox2 = new CCWin.SkinControl.SkinGroupBox();
            this.numLaserThreshold = new ManagementView.Comment.NumInput();
            this.numZResolution = new ManagementView.Comment.NumInput();
            this.numXYResolution = new ManagementView.Comment.NumInput();
            this.numYScale = new ManagementView.Comment.NumInput();
            this.numXScale = new ManagementView.Comment.NumInput();
            this.numGain = new ManagementView.Comment.NumInput();
            this.numBrightness = new ManagementView.Comment.NumInput();
            this.numExposureTime = new ManagementView.Comment.NumInput();
            this.skinGroupBox1 = new CCWin.SkinControl.SkinGroupBox();
            this.cmbName = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.skinLabel3 = new CCWin.SkinControl.SkinLabel();
            this.skinGroupBox3 = new CCWin.SkinControl.SkinGroupBox();
            this.numProfile = new ManagementView.Comment.NumInput();
            this.btnDel = new CCWin.SkinControl.SkinButton();
            this.btnAdd = new CCWin.SkinControl.SkinButton();
            this.txtName = new ManagementView.Comment.TextInput();
            this.skinLabel13 = new CCWin.SkinControl.SkinLabel();
            this.txtIP = new ManagementView.Comment.TextInput();
            this.btnLiveImage = new DevComponents.DotNetBar.ButtonX();
            this.btnConnect = new DevComponents.DotNetBar.ButtonX();
            this.numPort = new ManagementView.Comment.NumInput();
            this.lblId = new CCWin.SkinControl.SkinLabel();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarExposure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackbarGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBrightness)).BeginInit();
            this.panelEx1.SuspendLayout();
            this.panel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.skinGroupBox2.SuspendLayout();
            this.skinGroupBox1.SuspendLayout();
            this.skinGroupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // skinLabel1
            // 
            this.skinLabel1.AutoSize = true;
            this.skinLabel1.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel1.BorderColor = System.Drawing.Color.White;
            this.skinLabel1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel1.Location = new System.Drawing.Point(16, 64);
            this.skinLabel1.Name = "skinLabel1";
            this.skinLabel1.Size = new System.Drawing.Size(46, 17);
            this.skinLabel1.TabIndex = 0;
            this.skinLabel1.Text = "IP地址:";
            // 
            // skinLabel2
            // 
            this.skinLabel2.AutoSize = true;
            this.skinLabel2.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel2.BorderColor = System.Drawing.Color.White;
            this.skinLabel2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel2.Location = new System.Drawing.Point(16, 102);
            this.skinLabel2.Name = "skinLabel2";
            this.skinLabel2.Size = new System.Drawing.Size(35, 17);
            this.skinLabel2.TabIndex = 0;
            this.skinLabel2.Text = "端口:";
            // 
            // skinLabel4
            // 
            this.skinLabel4.AutoSize = true;
            this.skinLabel4.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel4.BorderColor = System.Drawing.Color.White;
            this.skinLabel4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel4.Location = new System.Drawing.Point(17, 21);
            this.skinLabel4.Name = "skinLabel4";
            this.skinLabel4.Size = new System.Drawing.Size(68, 17);
            this.skinLabel4.TabIndex = 0;
            this.skinLabel4.Text = "曝光时间：";
            // 
            // trackBarExposure
            // 
            this.trackBarExposure.BackColor = System.Drawing.Color.Transparent;
            this.trackBarExposure.Bar = null;
            this.trackBarExposure.BarStyle = CCWin.SkinControl.HSLTrackBarStyle.Opacity;
            this.trackBarExposure.BaseColor = System.Drawing.Color.DimGray;
            this.trackBarExposure.Location = new System.Drawing.Point(17, 37);
            this.trackBarExposure.Maximum = 10000;
            this.trackBarExposure.Minimum = 1;
            this.trackBarExposure.Name = "trackBarExposure";
            this.trackBarExposure.Size = new System.Drawing.Size(202, 45);
            this.trackBarExposure.TabIndex = 3;
            this.trackBarExposure.Track = null;
            this.trackBarExposure.Value = 1;
            this.trackBarExposure.Scroll += new System.EventHandler(this.trackBarExposure_Scroll);
            // 
            // skinLabel5
            // 
            this.skinLabel5.AutoSize = true;
            this.skinLabel5.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel5.BorderColor = System.Drawing.Color.White;
            this.skinLabel5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel5.Location = new System.Drawing.Point(17, 142);
            this.skinLabel5.Name = "skinLabel5";
            this.skinLabel5.Size = new System.Drawing.Size(44, 17);
            this.skinLabel5.TabIndex = 0;
            this.skinLabel5.Text = "增益：";
            // 
            // trackbarGain
            // 
            this.trackbarGain.BackColor = System.Drawing.Color.Transparent;
            this.trackbarGain.Bar = null;
            this.trackbarGain.BarStyle = CCWin.SkinControl.HSLTrackBarStyle.Opacity;
            this.trackbarGain.BaseColor = System.Drawing.Color.DimGray;
            this.trackbarGain.Location = new System.Drawing.Point(17, 157);
            this.trackbarGain.Maximum = 7;
            this.trackbarGain.Name = "trackbarGain";
            this.trackbarGain.Size = new System.Drawing.Size(202, 45);
            this.trackbarGain.TabIndex = 3;
            this.trackbarGain.Track = null;
            this.trackbarGain.Scroll += new System.EventHandler(this.trackbarGain_Scroll);
            // 
            // chkEnableGain
            // 
            this.chkEnableGain.AutoSize = true;
            this.chkEnableGain.BackColor = System.Drawing.Color.Transparent;
            this.chkEnableGain.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.chkEnableGain.DownBack = null;
            this.chkEnableGain.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkEnableGain.Location = new System.Drawing.Point(17, 198);
            this.chkEnableGain.MouseBack = null;
            this.chkEnableGain.Name = "chkEnableGain";
            this.chkEnableGain.NormlBack = null;
            this.chkEnableGain.SelectedDownBack = null;
            this.chkEnableGain.SelectedMouseBack = null;
            this.chkEnableGain.SelectedNormlBack = null;
            this.chkEnableGain.Size = new System.Drawing.Size(75, 21);
            this.chkEnableGain.TabIndex = 5;
            this.chkEnableGain.Text = "是否使能";
            this.chkEnableGain.UseVisualStyleBackColor = false;
            this.chkEnableGain.CheckedChanged += new System.EventHandler(this.chkEnableGain_CheckedChanged);
            // 
            // skinToolTip1
            // 
            this.skinToolTip1.AutoPopDelay = 5000;
            this.skinToolTip1.InitialDelay = 500;
            this.skinToolTip1.OwnerDraw = true;
            this.skinToolTip1.ReshowDelay = 800;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.White;
            this.btnSave.BackgroundImage = global::ManagementView.Properties.Resources.Save_128px_1186318_easyicon_net;
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Location = new System.Drawing.Point(256, 289);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(54, 50);
            this.btnSave.TabIndex = 6;
            this.skinToolTip1.SetToolTip(this.btnSave, "保存数据");
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // skinLabel6
            // 
            this.skinLabel6.AutoSize = true;
            this.skinLabel6.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel6.BorderColor = System.Drawing.Color.White;
            this.skinLabel6.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel6.Location = new System.Drawing.Point(17, 82);
            this.skinLabel6.Name = "skinLabel6";
            this.skinLabel6.Size = new System.Drawing.Size(68, 17);
            this.skinLabel6.TabIndex = 0;
            this.skinLabel6.Text = "激光强度：";
            // 
            // trackBarBrightness
            // 
            this.trackBarBrightness.BackColor = System.Drawing.Color.Transparent;
            this.trackBarBrightness.Bar = null;
            this.trackBarBrightness.BarStyle = CCWin.SkinControl.HSLTrackBarStyle.Opacity;
            this.trackBarBrightness.BaseColor = System.Drawing.Color.DimGray;
            this.trackBarBrightness.Location = new System.Drawing.Point(17, 98);
            this.trackBarBrightness.Name = "trackBarBrightness";
            this.trackBarBrightness.Size = new System.Drawing.Size(202, 45);
            this.trackBarBrightness.TabIndex = 3;
            this.trackBarBrightness.Track = null;
            this.trackBarBrightness.Scroll += new System.EventHandler(this.trackBarBrightness_Scroll);
            // 
            // skinLabel7
            // 
            this.skinLabel7.AutoSize = true;
            this.skinLabel7.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel7.BorderColor = System.Drawing.Color.White;
            this.skinLabel7.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel7.Location = new System.Drawing.Point(16, 140);
            this.skinLabel7.Name = "skinLabel7";
            this.skinLabel7.Size = new System.Drawing.Size(59, 17);
            this.skinLabel7.TabIndex = 0;
            this.skinLabel7.Text = "扫描长度:";
            // 
            // skinLabel8
            // 
            this.skinLabel8.AutoSize = true;
            this.skinLabel8.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel8.BorderColor = System.Drawing.Color.White;
            this.skinLabel8.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel8.Location = new System.Drawing.Point(13, 229);
            this.skinLabel8.Name = "skinLabel8";
            this.skinLabel8.Size = new System.Drawing.Size(49, 17);
            this.skinLabel8.TabIndex = 0;
            this.skinLabel8.Text = "XScale:";
            // 
            // skinLabel9
            // 
            this.skinLabel9.AutoSize = true;
            this.skinLabel9.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel9.BorderColor = System.Drawing.Color.White;
            this.skinLabel9.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel9.Location = new System.Drawing.Point(13, 289);
            this.skinLabel9.Name = "skinLabel9";
            this.skinLabel9.Size = new System.Drawing.Size(48, 17);
            this.skinLabel9.TabIndex = 0;
            this.skinLabel9.Text = "YScale:";
            // 
            // skinLabel10
            // 
            this.skinLabel10.AutoSize = true;
            this.skinLabel10.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel10.BorderColor = System.Drawing.Color.White;
            this.skinLabel10.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel10.Location = new System.Drawing.Point(114, 229);
            this.skinLabel10.Name = "skinLabel10";
            this.skinLabel10.Size = new System.Drawing.Size(66, 17);
            this.skinLabel10.TabIndex = 0;
            this.skinLabel10.Text = "XY 分辨率:";
            // 
            // skinLabel11
            // 
            this.skinLabel11.AutoSize = true;
            this.skinLabel11.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel11.BorderColor = System.Drawing.Color.White;
            this.skinLabel11.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel11.Location = new System.Drawing.Point(114, 289);
            this.skinLabel11.Name = "skinLabel11";
            this.skinLabel11.Size = new System.Drawing.Size(58, 17);
            this.skinLabel11.TabIndex = 0;
            this.skinLabel11.Text = "Z 分辨率:";
            // 
            // skinLabel12
            // 
            this.skinLabel12.AutoSize = true;
            this.skinLabel12.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel12.BorderColor = System.Drawing.Color.White;
            this.skinLabel12.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel12.Location = new System.Drawing.Point(218, 229);
            this.skinLabel12.Name = "skinLabel12";
            this.skinLabel12.Size = new System.Drawing.Size(69, 17);
            this.skinLabel12.TabIndex = 0;
            this.skinLabel12.Text = "Threshold:";
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.panelView);
            this.panelEx1.Controls.Add(this.panel);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(1150, 680);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 7;
            // 
            // panelView
            // 
            this.panelView.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelView.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelView.Location = new System.Drawing.Point(335, 0);
            this.panelView.Name = "panelView";
            this.panelView.Size = new System.Drawing.Size(815, 680);
            this.panelView.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelView.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelView.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelView.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelView.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelView.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelView.Style.GradientAngle = 90;
            this.panelView.TabIndex = 8;
            // 
            // panel
            // 
            this.panel.CanvasColor = System.Drawing.SystemColors.Control;
            this.panel.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panel.Controls.Add(this.tableLayoutPanel1);
            this.panel.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(335, 680);
            this.panel.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panel.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panel.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panel.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panel.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panel.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panel.Style.GradientAngle = 90;
            this.panel.TabIndex = 7;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.skinGroupBox2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.skinGroupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.skinGroupBox3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(335, 680);
            this.tableLayoutPanel1.TabIndex = 15;
            // 
            // skinGroupBox2
            // 
            this.skinGroupBox2.BackColor = System.Drawing.Color.Transparent;
            this.skinGroupBox2.BorderColor = System.Drawing.Color.SteelBlue;
            this.skinGroupBox2.Controls.Add(this.trackbarGain);
            this.skinGroupBox2.Controls.Add(this.numLaserThreshold);
            this.skinGroupBox2.Controls.Add(this.skinLabel12);
            this.skinGroupBox2.Controls.Add(this.numZResolution);
            this.skinGroupBox2.Controls.Add(this.skinLabel5);
            this.skinGroupBox2.Controls.Add(this.numXYResolution);
            this.skinGroupBox2.Controls.Add(this.skinLabel6);
            this.skinGroupBox2.Controls.Add(this.skinLabel11);
            this.skinGroupBox2.Controls.Add(this.numYScale);
            this.skinGroupBox2.Controls.Add(this.skinLabel10);
            this.skinGroupBox2.Controls.Add(this.numXScale);
            this.skinGroupBox2.Controls.Add(this.trackBarBrightness);
            this.skinGroupBox2.Controls.Add(this.skinLabel4);
            this.skinGroupBox2.Controls.Add(this.btnSave);
            this.skinGroupBox2.Controls.Add(this.numGain);
            this.skinGroupBox2.Controls.Add(this.trackBarExposure);
            this.skinGroupBox2.Controls.Add(this.numBrightness);
            this.skinGroupBox2.Controls.Add(this.skinLabel8);
            this.skinGroupBox2.Controls.Add(this.chkEnableGain);
            this.skinGroupBox2.Controls.Add(this.skinLabel9);
            this.skinGroupBox2.Controls.Add(this.numExposureTime);
            this.skinGroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinGroupBox2.ForeColor = System.Drawing.Color.Black;
            this.skinGroupBox2.Location = new System.Drawing.Point(0, 330);
            this.skinGroupBox2.Margin = new System.Windows.Forms.Padding(0);
            this.skinGroupBox2.Name = "skinGroupBox2";
            this.skinGroupBox2.RectBackColor = System.Drawing.Color.Transparent;
            this.skinGroupBox2.RoundStyle = CCWin.SkinClass.RoundStyle.Top;
            this.skinGroupBox2.Size = new System.Drawing.Size(335, 350);
            this.skinGroupBox2.TabIndex = 31;
            this.skinGroupBox2.TabStop = false;
            this.skinGroupBox2.Text = "参数配置";
            this.skinGroupBox2.TitleBorderColor = System.Drawing.Color.Transparent;
            this.skinGroupBox2.TitleRadius = 6;
            this.skinGroupBox2.TitleRectBackColor = System.Drawing.Color.White;
            this.skinGroupBox2.TitleRoundStyle = CCWin.SkinClass.RoundStyle.None;
            // 
            // numLaserThreshold
            // 
            this.numLaserThreshold.BackColor = System.Drawing.Color.White;
            this.numLaserThreshold.bInt = true;
            this.numLaserThreshold.Location = new System.Drawing.Point(218, 249);
            this.numLaserThreshold.Margin = new System.Windows.Forms.Padding(0);
            this.numLaserThreshold.MaxValue = 255D;
            this.numLaserThreshold.MinValue = 0D;
            this.numLaserThreshold.Name = "numLaserThreshold";
            this.numLaserThreshold.PointNum = 0;
            this.numLaserThreshold.Size = new System.Drawing.Size(92, 23);
            this.numLaserThreshold.sText = "0";
            this.numLaserThreshold.TabIndex = 15;
            // 
            // numZResolution
            // 
            this.numZResolution.BackColor = System.Drawing.Color.White;
            this.numZResolution.bInt = false;
            this.numZResolution.Location = new System.Drawing.Point(115, 309);
            this.numZResolution.Margin = new System.Windows.Forms.Padding(0);
            this.numZResolution.MaxValue = 999999999D;
            this.numZResolution.MinValue = 0D;
            this.numZResolution.Name = "numZResolution";
            this.numZResolution.PointNum = 6;
            this.numZResolution.Size = new System.Drawing.Size(92, 23);
            this.numZResolution.sText = "0";
            this.numZResolution.TabIndex = 17;
            // 
            // numXYResolution
            // 
            this.numXYResolution.BackColor = System.Drawing.Color.White;
            this.numXYResolution.bInt = false;
            this.numXYResolution.Location = new System.Drawing.Point(115, 249);
            this.numXYResolution.Margin = new System.Windows.Forms.Padding(0);
            this.numXYResolution.MaxValue = 999999999D;
            this.numXYResolution.MinValue = 0D;
            this.numXYResolution.Name = "numXYResolution";
            this.numXYResolution.PointNum = 6;
            this.numXYResolution.Size = new System.Drawing.Size(92, 23);
            this.numXYResolution.sText = "0";
            this.numXYResolution.TabIndex = 14;
            // 
            // numYScale
            // 
            this.numYScale.BackColor = System.Drawing.Color.White;
            this.numYScale.bInt = false;
            this.numYScale.Location = new System.Drawing.Point(13, 309);
            this.numYScale.Margin = new System.Windows.Forms.Padding(0);
            this.numYScale.MaxValue = 999999999D;
            this.numYScale.MinValue = 0D;
            this.numYScale.Name = "numYScale";
            this.numYScale.PointNum = 6;
            this.numYScale.Size = new System.Drawing.Size(92, 23);
            this.numYScale.sText = "0";
            this.numYScale.TabIndex = 16;
            // 
            // numXScale
            // 
            this.numXScale.BackColor = System.Drawing.Color.White;
            this.numXScale.bInt = false;
            this.numXScale.Location = new System.Drawing.Point(14, 249);
            this.numXScale.Margin = new System.Windows.Forms.Padding(0);
            this.numXScale.MaxValue = 999999999D;
            this.numXScale.MinValue = 0D;
            this.numXScale.Name = "numXScale";
            this.numXScale.PointNum = 6;
            this.numXScale.Size = new System.Drawing.Size(92, 23);
            this.numXScale.sText = "0";
            this.numXScale.TabIndex = 13;
            // 
            // numGain
            // 
            this.numGain.BackColor = System.Drawing.Color.White;
            this.numGain.bInt = false;
            this.numGain.Location = new System.Drawing.Point(224, 168);
            this.numGain.Margin = new System.Windows.Forms.Padding(0);
            this.numGain.MaxValue = 7D;
            this.numGain.MinValue = 0D;
            this.numGain.Name = "numGain";
            this.numGain.PointNum = 0;
            this.numGain.Size = new System.Drawing.Size(86, 23);
            this.numGain.sText = "0";
            this.numGain.TabIndex = 12;
            // 
            // numBrightness
            // 
            this.numBrightness.BackColor = System.Drawing.Color.White;
            this.numBrightness.bInt = false;
            this.numBrightness.Location = new System.Drawing.Point(224, 109);
            this.numBrightness.Margin = new System.Windows.Forms.Padding(0);
            this.numBrightness.MaxValue = 100D;
            this.numBrightness.MinValue = 0D;
            this.numBrightness.Name = "numBrightness";
            this.numBrightness.PointNum = 0;
            this.numBrightness.Size = new System.Drawing.Size(86, 23);
            this.numBrightness.sText = "0";
            this.numBrightness.TabIndex = 11;
            // 
            // numExposureTime
            // 
            this.numExposureTime.BackColor = System.Drawing.Color.White;
            this.numExposureTime.bInt = false;
            this.numExposureTime.Location = new System.Drawing.Point(224, 48);
            this.numExposureTime.Margin = new System.Windows.Forms.Padding(0);
            this.numExposureTime.MaxValue = 10000D;
            this.numExposureTime.MinValue = 1D;
            this.numExposureTime.Name = "numExposureTime";
            this.numExposureTime.PointNum = 0;
            this.numExposureTime.Size = new System.Drawing.Size(86, 23);
            this.numExposureTime.sText = "1";
            this.numExposureTime.TabIndex = 10;
            // 
            // skinGroupBox1
            // 
            this.skinGroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.skinGroupBox1.BorderColor = System.Drawing.Color.SteelBlue;
            this.skinGroupBox1.Controls.Add(this.cmbName);
            this.skinGroupBox1.Controls.Add(this.skinLabel3);
            this.skinGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinGroupBox1.ForeColor = System.Drawing.Color.Black;
            this.skinGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.skinGroupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.skinGroupBox1.Name = "skinGroupBox1";
            this.skinGroupBox1.RectBackColor = System.Drawing.Color.Transparent;
            this.skinGroupBox1.RoundStyle = CCWin.SkinClass.RoundStyle.Top;
            this.skinGroupBox1.Size = new System.Drawing.Size(335, 80);
            this.skinGroupBox1.TabIndex = 30;
            this.skinGroupBox1.TabStop = false;
            this.skinGroupBox1.Text = "镭射选择";
            this.skinGroupBox1.TitleBorderColor = System.Drawing.Color.Transparent;
            this.skinGroupBox1.TitleRadius = 6;
            this.skinGroupBox1.TitleRectBackColor = System.Drawing.Color.White;
            this.skinGroupBox1.TitleRoundStyle = CCWin.SkinClass.RoundStyle.None;
            // 
            // cmbName
            // 
            this.cmbName.DisplayMember = "Text";
            this.cmbName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbName.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbName.FormattingEnabled = true;
            this.cmbName.ItemHeight = 17;
            this.cmbName.Location = new System.Drawing.Point(79, 31);
            this.cmbName.Name = "cmbName";
            this.cmbName.Size = new System.Drawing.Size(143, 23);
            this.cmbName.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbName.TabIndex = 1;
            this.cmbName.SelectedIndexChanged += new System.EventHandler(this.cmbName_SelectedIndexChanged);
            // 
            // skinLabel3
            // 
            this.skinLabel3.AutoSize = true;
            this.skinLabel3.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel3.BorderColor = System.Drawing.Color.White;
            this.skinLabel3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel3.Location = new System.Drawing.Point(14, 34);
            this.skinLabel3.Name = "skinLabel3";
            this.skinLabel3.Size = new System.Drawing.Size(66, 17);
            this.skinLabel3.TabIndex = 0;
            this.skinLabel3.Text = "Laser选择:";
            // 
            // skinGroupBox3
            // 
            this.skinGroupBox3.BackColor = System.Drawing.Color.Transparent;
            this.skinGroupBox3.BorderColor = System.Drawing.Color.SteelBlue;
            this.skinGroupBox3.Controls.Add(this.numProfile);
            this.skinGroupBox3.Controls.Add(this.btnDel);
            this.skinGroupBox3.Controls.Add(this.skinLabel2);
            this.skinGroupBox3.Controls.Add(this.btnAdd);
            this.skinGroupBox3.Controls.Add(this.skinLabel1);
            this.skinGroupBox3.Controls.Add(this.txtName);
            this.skinGroupBox3.Controls.Add(this.skinLabel13);
            this.skinGroupBox3.Controls.Add(this.txtIP);
            this.skinGroupBox3.Controls.Add(this.skinLabel7);
            this.skinGroupBox3.Controls.Add(this.btnLiveImage);
            this.skinGroupBox3.Controls.Add(this.btnConnect);
            this.skinGroupBox3.Controls.Add(this.numPort);
            this.skinGroupBox3.Controls.Add(this.lblId);
            this.skinGroupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinGroupBox3.ForeColor = System.Drawing.Color.Black;
            this.skinGroupBox3.Location = new System.Drawing.Point(0, 80);
            this.skinGroupBox3.Margin = new System.Windows.Forms.Padding(0);
            this.skinGroupBox3.Name = "skinGroupBox3";
            this.skinGroupBox3.RectBackColor = System.Drawing.Color.Transparent;
            this.skinGroupBox3.RoundStyle = CCWin.SkinClass.RoundStyle.Top;
            this.skinGroupBox3.Size = new System.Drawing.Size(335, 250);
            this.skinGroupBox3.TabIndex = 30;
            this.skinGroupBox3.TabStop = false;
            this.skinGroupBox3.Text = "镭射配置";
            this.skinGroupBox3.TitleBorderColor = System.Drawing.Color.Transparent;
            this.skinGroupBox3.TitleRadius = 6;
            this.skinGroupBox3.TitleRectBackColor = System.Drawing.Color.White;
            this.skinGroupBox3.TitleRoundStyle = CCWin.SkinClass.RoundStyle.None;
            // 
            // numProfile
            // 
            this.numProfile.BackColor = System.Drawing.Color.White;
            this.numProfile.bInt = true;
            this.numProfile.Location = new System.Drawing.Point(79, 137);
            this.numProfile.Margin = new System.Windows.Forms.Padding(0);
            this.numProfile.MaxValue = 10000D;
            this.numProfile.MinValue = 10D;
            this.numProfile.Name = "numProfile";
            this.numProfile.PointNum = 0;
            this.numProfile.Size = new System.Drawing.Size(143, 23);
            this.numProfile.sText = "10";
            this.numProfile.TabIndex = 5;
            // 
            // btnDel
            // 
            this.btnDel.BackColor = System.Drawing.Color.Transparent;
            this.btnDel.BaseColor = System.Drawing.Color.Transparent;
            this.btnDel.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnDel.DownBack = null;
            this.btnDel.GlowColor = System.Drawing.Color.Blue;
            this.btnDel.Location = new System.Drawing.Point(230, 69);
            this.btnDel.MouseBack = null;
            this.btnDel.Name = "btnDel";
            this.btnDel.NormlBack = null;
            this.btnDel.Size = new System.Drawing.Size(61, 30);
            this.btnDel.TabIndex = 7;
            this.btnDel.Text = "删除";
            this.btnDel.UseVisualStyleBackColor = false;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.Transparent;
            this.btnAdd.BaseColor = System.Drawing.Color.Transparent;
            this.btnAdd.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnAdd.DownBack = null;
            this.btnAdd.GlowColor = System.Drawing.Color.Blue;
            this.btnAdd.Location = new System.Drawing.Point(230, 23);
            this.btnAdd.MouseBack = null;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.NormlBack = null;
            this.btnAdd.Size = new System.Drawing.Size(61, 30);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "新增";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.White;
            this.txtName.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtName.IsMultiLine = false;
            this.txtName.IsPassword = false;
            this.txtName.Location = new System.Drawing.Point(79, 23);
            this.txtName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(143, 26);
            this.txtName.sText = "";
            this.txtName.TabIndex = 2;
            // 
            // skinLabel13
            // 
            this.skinLabel13.AutoSize = true;
            this.skinLabel13.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel13.BorderColor = System.Drawing.Color.White;
            this.skinLabel13.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel13.Location = new System.Drawing.Point(16, 26);
            this.skinLabel13.Name = "skinLabel13";
            this.skinLabel13.Size = new System.Drawing.Size(35, 17);
            this.skinLabel13.TabIndex = 0;
            this.skinLabel13.Text = "名称:";
            // 
            // txtIP
            // 
            this.txtIP.BackColor = System.Drawing.Color.White;
            this.txtIP.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtIP.IsMultiLine = false;
            this.txtIP.IsPassword = false;
            this.txtIP.Location = new System.Drawing.Point(79, 61);
            this.txtIP.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(143, 26);
            this.txtIP.sText = "";
            this.txtIP.TabIndex = 3;
            // 
            // btnLiveImage
            // 
            this.btnLiveImage.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLiveImage.BackColor = System.Drawing.Color.LightGray;
            this.btnLiveImage.ColorTable = DevComponents.DotNetBar.eButtonColor.Flat;
            this.btnLiveImage.Enabled = false;
            this.btnLiveImage.Location = new System.Drawing.Point(147, 184);
            this.btnLiveImage.Name = "btnLiveImage";
            this.btnLiveImage.Size = new System.Drawing.Size(75, 37);
            this.btnLiveImage.Style = DevComponents.DotNetBar.eDotNetBarStyle.Metro;
            this.btnLiveImage.TabIndex = 9;
            this.btnLiveImage.Text = "Live On";
            this.btnLiveImage.Click += new System.EventHandler(this.btnLiveImage_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnConnect.BackColor = System.Drawing.Color.LightCoral;
            this.btnConnect.ColorTable = DevComponents.DotNetBar.eButtonColor.Blue;
            this.btnConnect.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConnect.Location = new System.Drawing.Point(19, 184);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 37);
            this.btnConnect.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnConnect.TabIndex = 8;
            this.btnConnect.Text = "Connect";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // numPort
            // 
            this.numPort.BackColor = System.Drawing.Color.White;
            this.numPort.bInt = false;
            this.numPort.Location = new System.Drawing.Point(79, 99);
            this.numPort.Margin = new System.Windows.Forms.Padding(0);
            this.numPort.MaxValue = 65535D;
            this.numPort.MinValue = 1D;
            this.numPort.Name = "numPort";
            this.numPort.PointNum = 0;
            this.numPort.Size = new System.Drawing.Size(143, 23);
            this.numPort.sText = "40";
            this.numPort.TabIndex = 4;
            // 
            // lblId
            // 
            this.lblId.AutoSize = true;
            this.lblId.BackColor = System.Drawing.Color.Transparent;
            this.lblId.BorderColor = System.Drawing.Color.White;
            this.lblId.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblId.ForeColor = System.Drawing.Color.SeaGreen;
            this.lblId.Location = new System.Drawing.Point(239, 115);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(36, 31);
            this.lblId.TabIndex = 0;
            this.lblId.Text = "Id";
            // 
            // LaserParamView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panelEx1);
            this.Name = "LaserParamView";
            this.Size = new System.Drawing.Size(1150, 680);
            this.Load += new System.EventHandler(this.ImageSet_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarExposure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackbarGain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBrightness)).EndInit();
            this.panelEx1.ResumeLayout(false);
            this.panel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.skinGroupBox2.ResumeLayout(false);
            this.skinGroupBox2.PerformLayout();
            this.skinGroupBox1.ResumeLayout(false);
            this.skinGroupBox1.PerformLayout();
            this.skinGroupBox3.ResumeLayout(false);
            this.skinGroupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CCWin.SkinControl.SkinLabel skinLabel1;
        private CCWin.SkinControl.SkinLabel skinLabel2;
        private CCWin.SkinControl.SkinLabel skinLabel4;
        private CCWin.SkinControl.SkinTrackBar trackBarExposure;
        private CCWin.SkinControl.SkinLabel skinLabel5;
        private CCWin.SkinControl.SkinTrackBar trackbarGain;
        private CCWin.SkinControl.SkinCheckBox chkEnableGain;
        private System.Windows.Forms.Button btnSave;
        private CCWin.SkinToolTip skinToolTip1;
        private CCWin.SkinControl.SkinLabel skinLabel6;
        private CCWin.SkinControl.SkinTrackBar trackBarBrightness;
        private CCWin.SkinControl.SkinLabel skinLabel7;
        private CCWin.SkinControl.SkinLabel skinLabel8;
        private CCWin.SkinControl.SkinLabel skinLabel9;
        private CCWin.SkinControl.SkinLabel skinLabel10;
        private CCWin.SkinControl.SkinLabel skinLabel11;
        private CCWin.SkinControl.SkinLabel skinLabel12;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.PanelEx panel;
        private DevComponents.DotNetBar.PanelEx panelView;
        private DevComponents.DotNetBar.ButtonX btnLiveImage;
        private DevComponents.DotNetBar.ButtonX btnConnect;
        private Comment.NumInput numZResolution;
        private Comment.NumInput numLaserThreshold;
        private Comment.NumInput numYScale;
        private Comment.NumInput numXScale;
        private Comment.NumInput numXYResolution;
        private Comment.TextInput txtIP;
        private Comment.NumInput numGain;
        private Comment.NumInput numBrightness;
        private Comment.NumInput numExposureTime;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbName;
        private Comment.NumInput numPort;
        private Comment.NumInput numProfile;
        private CCWin.SkinControl.SkinLabel skinLabel3;
        private Comment.TextInput txtName;
        private CCWin.SkinControl.SkinLabel skinLabel13;
        private CCWin.SkinControl.SkinLabel lblId;
        private CCWin.SkinControl.SkinButton btnDel;
        private CCWin.SkinControl.SkinButton btnAdd;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private CCWin.SkinControl.SkinGroupBox skinGroupBox2;
        private CCWin.SkinControl.SkinGroupBox skinGroupBox1;
        private CCWin.SkinControl.SkinGroupBox skinGroupBox3;
    }
}
