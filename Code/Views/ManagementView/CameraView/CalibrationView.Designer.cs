namespace ManagementView
{
    partial class CalibrationView
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.logView1 = new ManagementView.Comment.LogView();
            this.panelView = new DevComponents.DotNetBar.PanelEx();
            this.panelParam = new DevComponents.DotNetBar.PanelEx();
            this.cmbSuck = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.txtImageAng = new ManagementView.Comment.TextInput();
            this.txtImageCol = new ManagementView.Comment.TextInput();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.btnGetImage = new DevComponents.DotNetBar.ButtonX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.txtImageRow = new ManagementView.Comment.TextInput();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.lblImageRow = new DevComponents.DotNetBar.LabelX();
            this.panelBtn = new DevComponents.DotNetBar.PanelEx();
            this.btnStopCal = new DevComponents.DotNetBar.ButtonX();
            this.btnAlgorithm = new DevComponents.DotNetBar.ButtonX();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.btnLoadImage = new DevComponents.DotNetBar.ButtonX();
            this.btnSnap = new DevComponents.DotNetBar.ButtonX();
            this.btnStartCal = new DevComponents.DotNetBar.ButtonX();
            this.btnGetSuckCamera = new DevComponents.DotNetBar.ButtonX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.txtXOffSet = new ManagementView.Comment.TextInput();
            this.txtYOffSet = new ManagementView.Comment.TextInput();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.btnSaveSuckCamera = new DevComponents.DotNetBar.ButtonX();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelParam.SuspendLayout();
            this.panelBtn.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.groupPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58.22034F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.77966F));
            this.tableLayoutPanel1.Controls.Add(this.logView1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panelView, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelParam, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelBtn, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 73.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 26.71428F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1180, 700);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // logView1
            // 
            this.logView1.BackColor = System.Drawing.Color.Transparent;
            this.logView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logView1.Location = new System.Drawing.Point(1, 514);
            this.logView1.Margin = new System.Windows.Forms.Padding(1);
            this.logView1.Name = "logView1";
            this.logView1.Size = new System.Drawing.Size(685, 185);
            this.logView1.TabIndex = 0;
            // 
            // panelView
            // 
            this.panelView.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelView.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelView.Location = new System.Drawing.Point(1, 1);
            this.panelView.Margin = new System.Windows.Forms.Padding(1);
            this.panelView.Name = "panelView";
            this.panelView.Size = new System.Drawing.Size(685, 511);
            this.panelView.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelView.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelView.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelView.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelView.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelView.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelView.Style.GradientAngle = 90;
            this.panelView.TabIndex = 1;
            // 
            // panelParam
            // 
            this.panelParam.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelParam.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelParam.Controls.Add(this.groupPanel2);
            this.panelParam.Controls.Add(this.groupPanel1);
            this.panelParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelParam.Location = new System.Drawing.Point(688, 1);
            this.panelParam.Margin = new System.Windows.Forms.Padding(1);
            this.panelParam.Name = "panelParam";
            this.panelParam.Size = new System.Drawing.Size(491, 511);
            this.panelParam.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelParam.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelParam.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelParam.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelParam.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelParam.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelParam.Style.GradientAngle = 90;
            this.panelParam.TabIndex = 1;
            // 
            // cmbSuck
            // 
            this.cmbSuck.DisplayMember = "Text";
            this.cmbSuck.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbSuck.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbSuck.FormattingEnabled = true;
            this.cmbSuck.ItemHeight = 17;
            this.cmbSuck.Location = new System.Drawing.Point(113, 20);
            this.cmbSuck.Name = "cmbSuck";
            this.cmbSuck.Size = new System.Drawing.Size(124, 23);
            this.cmbSuck.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbSuck.TabIndex = 75;
            this.cmbSuck.SelectedIndexChanged += new System.EventHandler(this.cmbSuck_SelectedIndexChanged);
            // 
            // txtImageAng
            // 
            this.txtImageAng.BackColor = System.Drawing.Color.White;
            this.txtImageAng.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtImageAng.IsMultiLine = false;
            this.txtImageAng.IsPassword = false;
            this.txtImageAng.Location = new System.Drawing.Point(113, 164);
            this.txtImageAng.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtImageAng.Name = "txtImageAng";
            this.txtImageAng.Size = new System.Drawing.Size(124, 26);
            this.txtImageAng.sText = "";
            this.txtImageAng.TabIndex = 66;
            // 
            // txtImageCol
            // 
            this.txtImageCol.BackColor = System.Drawing.Color.White;
            this.txtImageCol.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtImageCol.IsMultiLine = false;
            this.txtImageCol.IsPassword = false;
            this.txtImageCol.Location = new System.Drawing.Point(113, 115);
            this.txtImageCol.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtImageCol.Name = "txtImageCol";
            this.txtImageCol.Size = new System.Drawing.Size(124, 26);
            this.txtImageCol.sText = "";
            this.txtImageCol.TabIndex = 66;
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX1.Location = new System.Drawing.Point(9, 166);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(92, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "图像中心角度：";
            // 
            // btnGetImage
            // 
            this.btnGetImage.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnGetImage.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnGetImage.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGetImage.Location = new System.Drawing.Point(328, 156);
            this.btnGetImage.Name = "btnGetImage";
            this.btnGetImage.Size = new System.Drawing.Size(123, 33);
            this.btnGetImage.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnGetImage.TabIndex = 0;
            this.btnGetImage.Text = "获取图像中心";
            this.btnGetImage.Click += new System.EventHandler(this.btnGetImage_Click);
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX2.Location = new System.Drawing.Point(9, 117);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(92, 23);
            this.labelX2.TabIndex = 0;
            this.labelX2.Text = "图像中心Col：";
            // 
            // txtImageRow
            // 
            this.txtImageRow.BackColor = System.Drawing.Color.White;
            this.txtImageRow.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtImageRow.IsMultiLine = false;
            this.txtImageRow.IsPassword = false;
            this.txtImageRow.Location = new System.Drawing.Point(113, 66);
            this.txtImageRow.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtImageRow.Name = "txtImageRow";
            this.txtImageRow.Size = new System.Drawing.Size(124, 26);
            this.txtImageRow.sText = "";
            this.txtImageRow.TabIndex = 66;
            // 
            // labelX3
            // 
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX3.Location = new System.Drawing.Point(9, 20);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(80, 23);
            this.labelX3.TabIndex = 0;
            this.labelX3.Text = "吸嘴选择：";
            // 
            // lblImageRow
            // 
            // 
            // 
            // 
            this.lblImageRow.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblImageRow.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblImageRow.Location = new System.Drawing.Point(9, 68);
            this.lblImageRow.Name = "lblImageRow";
            this.lblImageRow.Size = new System.Drawing.Size(92, 23);
            this.lblImageRow.TabIndex = 0;
            this.lblImageRow.Text = "图像中心Row：";
            // 
            // panelBtn
            // 
            this.panelBtn.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelBtn.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelBtn.Controls.Add(this.btnStopCal);
            this.panelBtn.Controls.Add(this.btnAlgorithm);
            this.panelBtn.Controls.Add(this.btnSave);
            this.panelBtn.Controls.Add(this.btnLoadImage);
            this.panelBtn.Controls.Add(this.btnSnap);
            this.panelBtn.Controls.Add(this.btnStartCal);
            this.panelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBtn.Location = new System.Drawing.Point(688, 514);
            this.panelBtn.Margin = new System.Windows.Forms.Padding(1);
            this.panelBtn.Name = "panelBtn";
            this.panelBtn.Size = new System.Drawing.Size(491, 185);
            this.panelBtn.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelBtn.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelBtn.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelBtn.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelBtn.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelBtn.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelBtn.Style.GradientAngle = 90;
            this.panelBtn.TabIndex = 1;
            // 
            // btnStopCal
            // 
            this.btnStopCal.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnStopCal.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnStopCal.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStopCal.Location = new System.Drawing.Point(360, 114);
            this.btnStopCal.Name = "btnStopCal";
            this.btnStopCal.Size = new System.Drawing.Size(92, 44);
            this.btnStopCal.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnStopCal.TabIndex = 0;
            this.btnStopCal.Text = "停止标定";
            this.btnStopCal.Click += new System.EventHandler(this.btnStopCal_Click);
            // 
            // btnAlgorithm
            // 
            this.btnAlgorithm.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAlgorithm.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAlgorithm.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAlgorithm.Location = new System.Drawing.Point(197, 114);
            this.btnAlgorithm.Name = "btnAlgorithm";
            this.btnAlgorithm.Size = new System.Drawing.Size(92, 44);
            this.btnAlgorithm.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnAlgorithm.TabIndex = 0;
            this.btnAlgorithm.Text = "执行算法";
            this.btnAlgorithm.Click += new System.EventHandler(this.btnAlgorithm_Click);
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSave.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.Location = new System.Drawing.Point(197, 28);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(92, 44);
            this.btnSave.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "保存参数";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLoadImage
            // 
            this.btnLoadImage.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLoadImage.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnLoadImage.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLoadImage.Location = new System.Drawing.Point(34, 28);
            this.btnLoadImage.Name = "btnLoadImage";
            this.btnLoadImage.Size = new System.Drawing.Size(92, 44);
            this.btnLoadImage.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnLoadImage.TabIndex = 0;
            this.btnLoadImage.Text = "加载图片";
            this.btnLoadImage.Click += new System.EventHandler(this.btnLoadImage_Click);
            // 
            // btnSnap
            // 
            this.btnSnap.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSnap.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSnap.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSnap.Location = new System.Drawing.Point(34, 114);
            this.btnSnap.Name = "btnSnap";
            this.btnSnap.Size = new System.Drawing.Size(92, 44);
            this.btnSnap.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSnap.TabIndex = 0;
            this.btnSnap.Text = "拍照";
            this.btnSnap.Click += new System.EventHandler(this.btnSnap_Click);
            // 
            // btnStartCal
            // 
            this.btnStartCal.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnStartCal.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnStartCal.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStartCal.Location = new System.Drawing.Point(360, 28);
            this.btnStartCal.Name = "btnStartCal";
            this.btnStartCal.Size = new System.Drawing.Size(92, 44);
            this.btnStartCal.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnStartCal.TabIndex = 0;
            this.btnStartCal.Text = "开始标定";
            this.btnStartCal.Click += new System.EventHandler(this.btnStartCal_Click);
            // 
            // btnGetSuckCamera
            // 
            this.btnGetSuckCamera.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnGetSuckCamera.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnGetSuckCamera.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGetSuckCamera.Location = new System.Drawing.Point(328, 15);
            this.btnGetSuckCamera.Name = "btnGetSuckCamera";
            this.btnGetSuckCamera.Size = new System.Drawing.Size(123, 33);
            this.btnGetSuckCamera.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnGetSuckCamera.TabIndex = 0;
            this.btnGetSuckCamera.Text = "获取相机吸嘴关系";
            this.btnGetSuckCamera.Click += new System.EventHandler(this.btnGetSuckCamera_Click);
            // 
            // labelX4
            // 
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX4.Location = new System.Drawing.Point(9, 24);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(92, 23);
            this.labelX4.TabIndex = 0;
            this.labelX4.Text = "X方向偏移：";
            // 
            // labelX5
            // 
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX5.Location = new System.Drawing.Point(9, 93);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(92, 23);
            this.labelX5.TabIndex = 0;
            this.labelX5.Text = "Y方向偏移：";
            // 
            // txtXOffSet
            // 
            this.txtXOffSet.BackColor = System.Drawing.Color.White;
            this.txtXOffSet.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtXOffSet.IsMultiLine = false;
            this.txtXOffSet.IsPassword = false;
            this.txtXOffSet.Location = new System.Drawing.Point(113, 22);
            this.txtXOffSet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtXOffSet.Name = "txtXOffSet";
            this.txtXOffSet.Size = new System.Drawing.Size(124, 26);
            this.txtXOffSet.sText = "";
            this.txtXOffSet.TabIndex = 66;
            // 
            // txtYOffSet
            // 
            this.txtYOffSet.BackColor = System.Drawing.Color.White;
            this.txtYOffSet.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtYOffSet.IsMultiLine = false;
            this.txtYOffSet.IsPassword = false;
            this.txtYOffSet.Location = new System.Drawing.Point(113, 91);
            this.txtYOffSet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtYOffSet.Name = "txtYOffSet";
            this.txtYOffSet.Size = new System.Drawing.Size(124, 26);
            this.txtYOffSet.sText = "";
            this.txtYOffSet.TabIndex = 66;
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.txtImageCol);
            this.groupPanel1.Controls.Add(this.cmbSuck);
            this.groupPanel1.Controls.Add(this.lblImageRow);
            this.groupPanel1.Controls.Add(this.labelX3);
            this.groupPanel1.Controls.Add(this.txtImageAng);
            this.groupPanel1.Controls.Add(this.txtImageRow);
            this.groupPanel1.Controls.Add(this.btnGetImage);
            this.groupPanel1.Controls.Add(this.labelX2);
            this.groupPanel1.Controls.Add(this.labelX1);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupPanel1.Location = new System.Drawing.Point(0, 0);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(491, 261);
            // 
            // 
            // 
            this.groupPanel1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel1.Style.BackColorGradientAngle = 90;
            this.groupPanel1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderBottomWidth = 1;
            this.groupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderLeftWidth = 1;
            this.groupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderRightWidth = 1;
            this.groupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderTopWidth = 1;
            this.groupPanel1.Style.CornerDiameter = 4;
            this.groupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel1.TabIndex = 77;
            this.groupPanel1.Text = "图像中心";
            // 
            // groupPanel2
            // 
            this.groupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel2.Controls.Add(this.txtXOffSet);
            this.groupPanel2.Controls.Add(this.txtYOffSet);
            this.groupPanel2.Controls.Add(this.labelX4);
            this.groupPanel2.Controls.Add(this.btnSaveSuckCamera);
            this.groupPanel2.Controls.Add(this.btnGetSuckCamera);
            this.groupPanel2.Controls.Add(this.labelX5);
            this.groupPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupPanel2.Location = new System.Drawing.Point(0, 261);
            this.groupPanel2.Name = "groupPanel2";
            this.groupPanel2.Size = new System.Drawing.Size(491, 309);
            // 
            // 
            // 
            this.groupPanel2.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel2.Style.BackColorGradientAngle = 90;
            this.groupPanel2.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel2.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderBottomWidth = 1;
            this.groupPanel2.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel2.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderLeftWidth = 1;
            this.groupPanel2.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderRightWidth = 1;
            this.groupPanel2.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderTopWidth = 1;
            this.groupPanel2.Style.CornerDiameter = 4;
            this.groupPanel2.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel2.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel2.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel2.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupPanel2.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel2.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel2.TabIndex = 78;
            this.groupPanel2.Text = "吸嘴关系";
            // 
            // btnSaveSuckCamera
            // 
            this.btnSaveSuckCamera.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSaveSuckCamera.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSaveSuckCamera.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSaveSuckCamera.Location = new System.Drawing.Point(328, 84);
            this.btnSaveSuckCamera.Name = "btnSaveSuckCamera";
            this.btnSaveSuckCamera.Size = new System.Drawing.Size(123, 33);
            this.btnSaveSuckCamera.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSaveSuckCamera.TabIndex = 0;
            this.btnSaveSuckCamera.Text = "保存吸嘴关系";
            this.btnSaveSuckCamera.Click += new System.EventHandler(this.btnSaveSuckCamera_Click);
            // 
            // CalibrationView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CalibrationView";
            this.Size = new System.Drawing.Size(1180, 700);
            this.Load += new System.EventHandler(this.CalibrationView_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelParam.ResumeLayout(false);
            this.panelBtn.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Comment.LogView logView1;
        private DevComponents.DotNetBar.PanelEx panelView;
        private DevComponents.DotNetBar.PanelEx panelParam;
        private DevComponents.DotNetBar.PanelEx panelBtn;
        private DevComponents.DotNetBar.LabelX lblImageRow;
        private Comment.TextInput txtImageCol;
        private DevComponents.DotNetBar.LabelX labelX2;
        private Comment.TextInput txtImageRow;
        private DevComponents.DotNetBar.ButtonX btnStopCal;
        private DevComponents.DotNetBar.ButtonX btnStartCal;
        private DevComponents.DotNetBar.ButtonX btnAlgorithm;
        private DevComponents.DotNetBar.ButtonX btnSnap;
        private DevComponents.DotNetBar.ButtonX btnSave;
        private DevComponents.DotNetBar.ButtonX btnLoadImage;
        private DevComponents.DotNetBar.ButtonX btnGetImage;
        private Comment.TextInput txtImageAng;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbSuck;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.ButtonX btnGetSuckCamera;
        private Comment.TextInput txtYOffSet;
        private Comment.TextInput txtXOffSet;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.ButtonX btnSaveSuckCamera;
    }
}
