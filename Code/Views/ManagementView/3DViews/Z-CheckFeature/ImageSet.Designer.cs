namespace ManagementView._3DViews
{
    partial class ImageSet
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
            this.txtProfile = new ManagementView.Comment.TextInput();
            this.txtPort = new ManagementView.Comment.TextInput();
            this.txtIP = new ManagementView.Comment.TextInput();
            this.numZResolution = new ManagementView.Comment.NumInput();
            this.numLaserThreshold = new ManagementView.Comment.NumInput();
            this.numYScale = new ManagementView.Comment.NumInput();
            this.numXScale = new ManagementView.Comment.NumInput();
            this.numXYResolution = new ManagementView.Comment.NumInput();
            this.btnConnect = new DevComponents.DotNetBar.ButtonX();
            this.btnLiveImage = new DevComponents.DotNetBar.ButtonX();
            this.numExposureTime = new ManagementView.Comment.NumInput();
            this.numBrightness = new ManagementView.Comment.NumInput();
            this.numGain = new ManagementView.Comment.NumInput();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarExposure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackbarGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBrightness)).BeginInit();
            this.panelEx1.SuspendLayout();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // skinLabel1
            // 
            this.skinLabel1.AutoSize = true;
            this.skinLabel1.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel1.BorderColor = System.Drawing.Color.White;
            this.skinLabel1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel1.Location = new System.Drawing.Point(18, 14);
            this.skinLabel1.Name = "skinLabel1";
            this.skinLabel1.Size = new System.Drawing.Size(22, 17);
            this.skinLabel1.TabIndex = 0;
            this.skinLabel1.Text = "IP:";
            // 
            // skinLabel2
            // 
            this.skinLabel2.AutoSize = true;
            this.skinLabel2.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel2.BorderColor = System.Drawing.Color.White;
            this.skinLabel2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel2.Location = new System.Drawing.Point(18, 56);
            this.skinLabel2.Name = "skinLabel2";
            this.skinLabel2.Size = new System.Drawing.Size(35, 17);
            this.skinLabel2.TabIndex = 0;
            this.skinLabel2.Text = "Port:";
            // 
            // skinLabel4
            // 
            this.skinLabel4.AutoSize = true;
            this.skinLabel4.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel4.BorderColor = System.Drawing.Color.White;
            this.skinLabel4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel4.Location = new System.Drawing.Point(19, 149);
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
            this.trackBarExposure.Location = new System.Drawing.Point(19, 169);
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
            this.skinLabel5.Location = new System.Drawing.Point(19, 272);
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
            this.trackbarGain.Location = new System.Drawing.Point(19, 289);
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
            this.chkEnableGain.Location = new System.Drawing.Point(19, 329);
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
            this.btnSave.Location = new System.Drawing.Point(251, 514);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(45, 42);
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
            this.skinLabel6.Location = new System.Drawing.Point(19, 210);
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
            this.trackBarBrightness.Location = new System.Drawing.Point(19, 230);
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
            this.skinLabel7.Location = new System.Drawing.Point(18, 100);
            this.skinLabel7.Name = "skinLabel7";
            this.skinLabel7.Size = new System.Drawing.Size(48, 17);
            this.skinLabel7.TabIndex = 0;
            this.skinLabel7.Text = "Profile:";
            // 
            // skinLabel8
            // 
            this.skinLabel8.AutoSize = true;
            this.skinLabel8.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel8.BorderColor = System.Drawing.Color.White;
            this.skinLabel8.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel8.Location = new System.Drawing.Point(19, 371);
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
            this.skinLabel9.Location = new System.Drawing.Point(19, 431);
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
            this.skinLabel10.Location = new System.Drawing.Point(175, 371);
            this.skinLabel10.Name = "skinLabel10";
            this.skinLabel10.Size = new System.Drawing.Size(91, 17);
            this.skinLabel10.TabIndex = 0;
            this.skinLabel10.Text = "XY Resolution:";
            // 
            // skinLabel11
            // 
            this.skinLabel11.AutoSize = true;
            this.skinLabel11.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel11.BorderColor = System.Drawing.Color.White;
            this.skinLabel11.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel11.Location = new System.Drawing.Point(177, 431);
            this.skinLabel11.Name = "skinLabel11";
            this.skinLabel11.Size = new System.Drawing.Size(83, 17);
            this.skinLabel11.TabIndex = 0;
            this.skinLabel11.Text = "Z Resolution:";
            // 
            // skinLabel12
            // 
            this.skinLabel12.AutoSize = true;
            this.skinLabel12.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel12.BorderColor = System.Drawing.Color.White;
            this.skinLabel12.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel12.Location = new System.Drawing.Point(19, 494);
            this.skinLabel12.Name = "skinLabel12";
            this.skinLabel12.Size = new System.Drawing.Size(104, 17);
            this.skinLabel12.TabIndex = 0;
            this.skinLabel12.Text = "Laser Threshold:";
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
            this.panelEx1.Size = new System.Drawing.Size(879, 634);
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
            this.panelView.Location = new System.Drawing.Point(320, 0);
            this.panelView.Name = "panelView";
            this.panelView.Size = new System.Drawing.Size(559, 634);
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
            this.panel.Controls.Add(this.txtProfile);
            this.panel.Controls.Add(this.txtPort);
            this.panel.Controls.Add(this.txtIP);
            this.panel.Controls.Add(this.numZResolution);
            this.panel.Controls.Add(this.numLaserThreshold);
            this.panel.Controls.Add(this.numYScale);
            this.panel.Controls.Add(this.numXScale);
            this.panel.Controls.Add(this.numGain);
            this.panel.Controls.Add(this.numBrightness);
            this.panel.Controls.Add(this.numExposureTime);
            this.panel.Controls.Add(this.numXYResolution);
            this.panel.Controls.Add(this.btnConnect);
            this.panel.Controls.Add(this.btnLiveImage);
            this.panel.Controls.Add(this.btnSave);
            this.panel.Controls.Add(this.skinLabel7);
            this.panel.Controls.Add(this.skinLabel1);
            this.panel.Controls.Add(this.chkEnableGain);
            this.panel.Controls.Add(this.trackBarExposure);
            this.panel.Controls.Add(this.skinLabel11);
            this.panel.Controls.Add(this.skinLabel4);
            this.panel.Controls.Add(this.trackBarBrightness);
            this.panel.Controls.Add(this.skinLabel2);
            this.panel.Controls.Add(this.trackbarGain);
            this.panel.Controls.Add(this.skinLabel10);
            this.panel.Controls.Add(this.skinLabel6);
            this.panel.Controls.Add(this.skinLabel5);
            this.panel.Controls.Add(this.skinLabel12);
            this.panel.Controls.Add(this.skinLabel8);
            this.panel.Controls.Add(this.skinLabel9);
            this.panel.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(320, 634);
            this.panel.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panel.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panel.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panel.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panel.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panel.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panel.Style.GradientAngle = 90;
            this.panel.TabIndex = 7;
            // 
            // txtProfile
            // 
            this.txtProfile.BackColor = System.Drawing.Color.AliceBlue;
            this.txtProfile.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtProfile.Location = new System.Drawing.Point(68, 96);
            this.txtProfile.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtProfile.Name = "txtProfile";
            this.txtProfile.Size = new System.Drawing.Size(72, 23);
            this.txtProfile.sText = "";
            this.txtProfile.TabIndex = 11;
            // 
            // txtPort
            // 
            this.txtPort.BackColor = System.Drawing.Color.AliceBlue;
            this.txtPort.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPort.Location = new System.Drawing.Point(68, 53);
            this.txtPort.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(73, 23);
            this.txtPort.sText = "";
            this.txtPort.TabIndex = 11;
            // 
            // txtIP
            // 
            this.txtIP.BackColor = System.Drawing.Color.AliceBlue;
            this.txtIP.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtIP.Location = new System.Drawing.Point(68, 10);
            this.txtIP.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(143, 23);
            this.txtIP.sText = "";
            this.txtIP.TabIndex = 11;
            // 
            // numZResolution
            // 
            this.numZResolution.bInt = false;
            this.numZResolution.Location = new System.Drawing.Point(178, 451);
            this.numZResolution.Margin = new System.Windows.Forms.Padding(0);
            this.numZResolution.MaxValue = 999999999D;
            this.numZResolution.MinValue = 0D;
            this.numZResolution.Name = "numZResolution";
            this.numZResolution.PointNum = 6;
            this.numZResolution.Size = new System.Drawing.Size(120, 23);
            this.numZResolution.sText = "0";
            this.numZResolution.TabIndex = 10;
            // 
            // numLaserThreshold
            // 
            this.numLaserThreshold.bInt = true;
            this.numLaserThreshold.Location = new System.Drawing.Point(19, 514);
            this.numLaserThreshold.Margin = new System.Windows.Forms.Padding(0);
            this.numLaserThreshold.MaxValue = 255D;
            this.numLaserThreshold.MinValue = 0D;
            this.numLaserThreshold.Name = "numLaserThreshold";
            this.numLaserThreshold.PointNum = 0;
            this.numLaserThreshold.Size = new System.Drawing.Size(120, 23);
            this.numLaserThreshold.sText = "0";
            this.numLaserThreshold.TabIndex = 9;
            // 
            // numYScale
            // 
            this.numYScale.bInt = false;
            this.numYScale.Location = new System.Drawing.Point(19, 451);
            this.numYScale.Margin = new System.Windows.Forms.Padding(0);
            this.numYScale.MaxValue = 999999999D;
            this.numYScale.MinValue = 0D;
            this.numYScale.Name = "numYScale";
            this.numYScale.PointNum = 6;
            this.numYScale.Size = new System.Drawing.Size(120, 23);
            this.numYScale.sText = "0";
            this.numYScale.TabIndex = 9;
            // 
            // numXScale
            // 
            this.numXScale.bInt = false;
            this.numXScale.Location = new System.Drawing.Point(20, 391);
            this.numXScale.Margin = new System.Windows.Forms.Padding(0);
            this.numXScale.MaxValue = 999999999D;
            this.numXScale.MinValue = 0D;
            this.numXScale.Name = "numXScale";
            this.numXScale.PointNum = 6;
            this.numXScale.Size = new System.Drawing.Size(120, 23);
            this.numXScale.sText = "0";
            this.numXScale.TabIndex = 9;
            // 
            // numXYResolution
            // 
            this.numXYResolution.bInt = false;
            this.numXYResolution.Location = new System.Drawing.Point(178, 391);
            this.numXYResolution.Margin = new System.Windows.Forms.Padding(0);
            this.numXYResolution.MaxValue = 999999999D;
            this.numXYResolution.MinValue = 0D;
            this.numXYResolution.Name = "numXYResolution";
            this.numXYResolution.PointNum = 6;
            this.numXYResolution.Size = new System.Drawing.Size(120, 23);
            this.numXYResolution.sText = "0";
            this.numXYResolution.TabIndex = 9;
            // 
            // btnConnect
            // 
            this.btnConnect.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnConnect.BackColor = System.Drawing.Color.LightCoral;
            this.btnConnect.ColorTable = DevComponents.DotNetBar.eButtonColor.Blue;
            this.btnConnect.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConnect.Location = new System.Drawing.Point(19, 573);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 37);
            this.btnConnect.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnConnect.TabIndex = 8;
            this.btnConnect.Text = "Connect";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnLiveImage
            // 
            this.btnLiveImage.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLiveImage.BackColor = System.Drawing.Color.LightGray;
            this.btnLiveImage.ColorTable = DevComponents.DotNetBar.eButtonColor.Flat;
            this.btnLiveImage.Enabled = false;
            this.btnLiveImage.Location = new System.Drawing.Point(115, 573);
            this.btnLiveImage.Name = "btnLiveImage";
            this.btnLiveImage.Size = new System.Drawing.Size(75, 37);
            this.btnLiveImage.Style = DevComponents.DotNetBar.eDotNetBarStyle.Metro;
            this.btnLiveImage.TabIndex = 7;
            this.btnLiveImage.Text = "Live On";
            this.btnLiveImage.Click += new System.EventHandler(this.btnLiveImage_Click);
            // 
            // numExposureTime
            // 
            this.numExposureTime.bInt = false;
            this.numExposureTime.Location = new System.Drawing.Point(226, 179);
            this.numExposureTime.Margin = new System.Windows.Forms.Padding(0);
            this.numExposureTime.MaxValue = 10000D;
            this.numExposureTime.MinValue = 1D;
            this.numExposureTime.Name = "numExposureTime";
            this.numExposureTime.PointNum = 0;
            this.numExposureTime.Size = new System.Drawing.Size(86, 23);
            this.numExposureTime.sText = "1";
            this.numExposureTime.TabIndex = 9;
            // 
            // numBrightness
            // 
            this.numBrightness.bInt = false;
            this.numBrightness.Location = new System.Drawing.Point(226, 240);
            this.numBrightness.Margin = new System.Windows.Forms.Padding(0);
            this.numBrightness.MaxValue = 100D;
            this.numBrightness.MinValue = 0D;
            this.numBrightness.Name = "numBrightness";
            this.numBrightness.PointNum = 0;
            this.numBrightness.Size = new System.Drawing.Size(86, 23);
            this.numBrightness.sText = "0";
            this.numBrightness.TabIndex = 9;
            // 
            // numGain
            // 
            this.numGain.bInt = false;
            this.numGain.Location = new System.Drawing.Point(226, 298);
            this.numGain.Margin = new System.Windows.Forms.Padding(0);
            this.numGain.MaxValue = 7D;
            this.numGain.MinValue = 0D;
            this.numGain.Name = "numGain";
            this.numGain.PointNum = 0;
            this.numGain.Size = new System.Drawing.Size(86, 23);
            this.numGain.sText = "0";
            this.numGain.TabIndex = 9;
            // 
            // ImageSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panelEx1);
            this.Name = "ImageSet";
            this.Size = new System.Drawing.Size(879, 634);
            this.Load += new System.EventHandler(this.ImageSet_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarExposure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackbarGain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBrightness)).EndInit();
            this.panelEx1.ResumeLayout(false);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
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
        private Comment.TextInput txtProfile;
        private Comment.TextInput txtPort;
        private Comment.TextInput txtIP;
        private Comment.NumInput numGain;
        private Comment.NumInput numBrightness;
        private Comment.NumInput numExposureTime;
    }
}
