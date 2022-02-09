namespace ManagementView
{
    partial class FixtureTeachView
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
            this.panelView = new DevComponents.DotNetBar.PanelEx();
            this.panelParam = new DevComponents.DotNetBar.PanelEx();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.numSocre = new DevComponents.Editors.DoubleInput();
            this.numSigma = new DevComponents.Editors.DoubleInput();
            this.numCount = new DevComponents.Editors.IntegerInput();
            this.numThreshold = new DevComponents.Editors.IntegerInput();
            this.numLength2 = new DevComponents.Editors.IntegerInput();
            this.numLength1 = new DevComponents.Editors.IntegerInput();
            this.cmbTransition = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.comboItem2 = new DevComponents.Editors.ComboItem();
            this.comboItem3 = new DevComponents.Editors.ComboItem();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.cmbSelect = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem4 = new DevComponents.Editors.ComboItem();
            this.comboItem5 = new DevComponents.Editors.ComboItem();
            this.comboItem6 = new DevComponents.Editors.ComboItem();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.labelX11 = new DevComponents.DotNetBar.LabelX();
            this.labelX12 = new DevComponents.DotNetBar.LabelX();
            this.labelX13 = new DevComponents.DotNetBar.LabelX();
            this.labelX14 = new DevComponents.DotNetBar.LabelX();
            this.labelX15 = new DevComponents.DotNetBar.LabelX();
            this.labelX16 = new DevComponents.DotNetBar.LabelX();
            this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.cmbCamera = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.cmbFixture = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.btnSavePix = new DevComponents.DotNetBar.ButtonX();
            this.btnGetPos = new DevComponents.DotNetBar.ButtonX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.btnGoFixtureSnap = new DevComponents.DotNetBar.ButtonX();
            this.labelX19 = new DevComponents.DotNetBar.LabelX();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX18 = new DevComponents.DotNetBar.LabelX();
            this.labelX9 = new DevComponents.DotNetBar.LabelX();
            this.labelX17 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX10 = new DevComponents.DotNetBar.LabelX();
            this.panelBtn = new DevComponents.DotNetBar.PanelEx();
            this.btnAlgorithm = new DevComponents.DotNetBar.ButtonX();
            this.btnDrawLine = new DevComponents.DotNetBar.ButtonX();
            this.btnSaveParam = new DevComponents.DotNetBar.ButtonX();
            this.btnSnap = new DevComponents.DotNetBar.ButtonX();
            this.btnLoadImage = new DevComponents.DotNetBar.ButtonX();
            this.labelX20 = new DevComponents.DotNetBar.LabelX();
            this.numExposureTime = new DevComponents.Editors.IntegerInput();
            this.logView1 = new ManagementView.Comment.LogView();
            this.txtCenterRow = new ManagementView.Comment.TextInput();
            this.txtCenterAngle = new ManagementView.Comment.TextInput();
            this.txtSetAngle = new ManagementView.Comment.TextInput();
            this.txtTeachAngle = new ManagementView.Comment.TextInput();
            this.txtCenterCol = new ManagementView.Comment.TextInput();
            this.txtSetY = new ManagementView.Comment.TextInput();
            this.txtTeachY = new ManagementView.Comment.TextInput();
            this.txtSetX = new ManagementView.Comment.TextInput();
            this.txtTeachX = new ManagementView.Comment.TextInput();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelParam.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSocre)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSigma)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLength2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLength1)).BeginInit();
            this.groupPanel2.SuspendLayout();
            this.panelBtn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numExposureTime)).BeginInit();
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1127, 660);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // panelView
            // 
            this.panelView.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelView.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelView.Location = new System.Drawing.Point(1, 1);
            this.panelView.Margin = new System.Windows.Forms.Padding(1);
            this.panelView.Name = "panelView";
            this.panelView.Size = new System.Drawing.Size(654, 481);
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
            this.panelParam.Controls.Add(this.groupPanel1);
            this.panelParam.Controls.Add(this.groupPanel2);
            this.panelParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelParam.Location = new System.Drawing.Point(657, 1);
            this.panelParam.Margin = new System.Windows.Forms.Padding(1);
            this.panelParam.Name = "panelParam";
            this.panelParam.Size = new System.Drawing.Size(469, 481);
            this.panelParam.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelParam.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelParam.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelParam.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelParam.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelParam.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelParam.Style.GradientAngle = 90;
            this.panelParam.TabIndex = 1;
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.numSocre);
            this.groupPanel1.Controls.Add(this.numSigma);
            this.groupPanel1.Controls.Add(this.numCount);
            this.groupPanel1.Controls.Add(this.numThreshold);
            this.groupPanel1.Controls.Add(this.numLength2);
            this.groupPanel1.Controls.Add(this.numExposureTime);
            this.groupPanel1.Controls.Add(this.numLength1);
            this.groupPanel1.Controls.Add(this.cmbTransition);
            this.groupPanel1.Controls.Add(this.labelX5);
            this.groupPanel1.Controls.Add(this.cmbSelect);
            this.groupPanel1.Controls.Add(this.labelX20);
            this.groupPanel1.Controls.Add(this.labelX6);
            this.groupPanel1.Controls.Add(this.labelX11);
            this.groupPanel1.Controls.Add(this.labelX12);
            this.groupPanel1.Controls.Add(this.labelX13);
            this.groupPanel1.Controls.Add(this.labelX14);
            this.groupPanel1.Controls.Add(this.labelX15);
            this.groupPanel1.Controls.Add(this.labelX16);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupPanel1.Location = new System.Drawing.Point(0, 272);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(469, 209);
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
            this.groupPanel1.TabIndex = 78;
            this.groupPanel1.Text = "算法参数";
            // 
            // numSocre
            // 
            // 
            // 
            // 
            this.numSocre.BackgroundStyle.BackColor = System.Drawing.Color.White;
            this.numSocre.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numSocre.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.numSocre.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.numSocre.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numSocre.Increment = 0.1D;
            this.numSocre.Location = new System.Drawing.Point(346, 81);
            this.numSocre.MinValue = 0D;
            this.numSocre.Name = "numSocre";
            this.numSocre.ShowUpDown = true;
            this.numSocre.Size = new System.Drawing.Size(118, 23);
            this.numSocre.TabIndex = 81;
            // 
            // numSigma
            // 
            // 
            // 
            // 
            this.numSigma.BackgroundStyle.BackColor = System.Drawing.Color.White;
            this.numSigma.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numSigma.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.numSigma.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.numSigma.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numSigma.Increment = 0.1D;
            this.numSigma.Location = new System.Drawing.Point(106, 47);
            this.numSigma.Name = "numSigma";
            this.numSigma.ShowUpDown = true;
            this.numSigma.Size = new System.Drawing.Size(118, 23);
            this.numSigma.TabIndex = 81;
            // 
            // numCount
            // 
            // 
            // 
            // 
            this.numCount.BackgroundStyle.BackColor = System.Drawing.Color.White;
            this.numCount.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numCount.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.numCount.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.numCount.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numCount.Location = new System.Drawing.Point(106, 81);
            this.numCount.Name = "numCount";
            this.numCount.ShowUpDown = true;
            this.numCount.Size = new System.Drawing.Size(118, 23);
            this.numCount.TabIndex = 80;
            // 
            // numThreshold
            // 
            // 
            // 
            // 
            this.numThreshold.BackgroundStyle.BackColor = System.Drawing.Color.White;
            this.numThreshold.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numThreshold.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.numThreshold.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.numThreshold.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numThreshold.Location = new System.Drawing.Point(346, 47);
            this.numThreshold.Name = "numThreshold";
            this.numThreshold.ShowUpDown = true;
            this.numThreshold.Size = new System.Drawing.Size(118, 23);
            this.numThreshold.TabIndex = 80;
            // 
            // numLength2
            // 
            // 
            // 
            // 
            this.numLength2.BackgroundStyle.BackColor = System.Drawing.Color.White;
            this.numLength2.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numLength2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.numLength2.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.numLength2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numLength2.Location = new System.Drawing.Point(346, 13);
            this.numLength2.Name = "numLength2";
            this.numLength2.ShowUpDown = true;
            this.numLength2.Size = new System.Drawing.Size(118, 23);
            this.numLength2.TabIndex = 80;
            // 
            // numLength1
            // 
            // 
            // 
            // 
            this.numLength1.BackgroundStyle.BackColor = System.Drawing.Color.White;
            this.numLength1.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numLength1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.numLength1.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.numLength1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numLength1.Location = new System.Drawing.Point(106, 13);
            this.numLength1.Name = "numLength1";
            this.numLength1.ShowUpDown = true;
            this.numLength1.Size = new System.Drawing.Size(118, 23);
            this.numLength1.TabIndex = 80;
            // 
            // cmbTransition
            // 
            this.cmbTransition.DisplayMember = "Text";
            this.cmbTransition.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbTransition.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbTransition.FormattingEnabled = true;
            this.cmbTransition.ItemHeight = 17;
            this.cmbTransition.Items.AddRange(new object[] {
            this.comboItem1,
            this.comboItem2,
            this.comboItem3});
            this.cmbTransition.Location = new System.Drawing.Point(106, 115);
            this.cmbTransition.Name = "cmbTransition";
            this.cmbTransition.Size = new System.Drawing.Size(118, 23);
            this.cmbTransition.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbTransition.TabIndex = 79;
            // 
            // comboItem1
            // 
            this.comboItem1.Text = "positive";
            // 
            // comboItem2
            // 
            this.comboItem2.Text = "negative";
            // 
            // comboItem3
            // 
            this.comboItem3.Text = "all";
            // 
            // labelX5
            // 
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX5.Location = new System.Drawing.Point(4, 115);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(89, 23);
            this.labelX5.TabIndex = 78;
            this.labelX5.Text = "寻找边线极性:";
            // 
            // cmbSelect
            // 
            this.cmbSelect.DisplayMember = "Text";
            this.cmbSelect.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbSelect.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbSelect.FormattingEnabled = true;
            this.cmbSelect.ItemHeight = 17;
            this.cmbSelect.Items.AddRange(new object[] {
            this.comboItem4,
            this.comboItem5,
            this.comboItem6});
            this.cmbSelect.Location = new System.Drawing.Point(346, 115);
            this.cmbSelect.Name = "cmbSelect";
            this.cmbSelect.Size = new System.Drawing.Size(118, 23);
            this.cmbSelect.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbSelect.TabIndex = 75;
            // 
            // comboItem4
            // 
            this.comboItem4.Text = "first";
            // 
            // comboItem5
            // 
            this.comboItem5.Text = "last";
            // 
            // comboItem6
            // 
            this.comboItem6.Text = "all";
            // 
            // labelX6
            // 
            // 
            // 
            // 
            this.labelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX6.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX6.Location = new System.Drawing.Point(247, 115);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(95, 23);
            this.labelX6.TabIndex = 0;
            this.labelX6.Text = "寻找边线排序：";
            // 
            // labelX11
            // 
            // 
            // 
            // 
            this.labelX11.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX11.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX11.Location = new System.Drawing.Point(6, 13);
            this.labelX11.Name = "labelX11";
            this.labelX11.Size = new System.Drawing.Size(93, 23);
            this.labelX11.TabIndex = 0;
            this.labelX11.Text = "寻找边线长度：";
            // 
            // labelX12
            // 
            // 
            // 
            // 
            this.labelX12.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX12.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX12.Location = new System.Drawing.Point(249, 13);
            this.labelX12.Name = "labelX12";
            this.labelX12.Size = new System.Drawing.Size(93, 23);
            this.labelX12.TabIndex = 0;
            this.labelX12.Text = "寻找边线宽度：";
            // 
            // labelX13
            // 
            // 
            // 
            // 
            this.labelX13.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX13.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX13.Location = new System.Drawing.Point(6, 47);
            this.labelX13.Name = "labelX13";
            this.labelX13.Size = new System.Drawing.Size(93, 23);
            this.labelX13.TabIndex = 0;
            this.labelX13.Text = "寻找边线平滑:";
            // 
            // labelX14
            // 
            // 
            // 
            // 
            this.labelX14.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX14.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX14.Location = new System.Drawing.Point(249, 47);
            this.labelX14.Name = "labelX14";
            this.labelX14.Size = new System.Drawing.Size(93, 23);
            this.labelX14.TabIndex = 0;
            this.labelX14.Text = "寻找边线阈值:";
            // 
            // labelX15
            // 
            // 
            // 
            // 
            this.labelX15.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX15.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX15.Location = new System.Drawing.Point(6, 81);
            this.labelX15.Name = "labelX15";
            this.labelX15.Size = new System.Drawing.Size(93, 23);
            this.labelX15.TabIndex = 0;
            this.labelX15.Text = "寻找边线数量:";
            // 
            // labelX16
            // 
            // 
            // 
            // 
            this.labelX16.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX16.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX16.Location = new System.Drawing.Point(249, 81);
            this.labelX16.Name = "labelX16";
            this.labelX16.Size = new System.Drawing.Size(93, 23);
            this.labelX16.TabIndex = 0;
            this.labelX16.Text = "寻找边线分数:";
            // 
            // groupPanel2
            // 
            this.groupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel2.Controls.Add(this.txtCenterRow);
            this.groupPanel2.Controls.Add(this.cmbCamera);
            this.groupPanel2.Controls.Add(this.labelX4);
            this.groupPanel2.Controls.Add(this.cmbFixture);
            this.groupPanel2.Controls.Add(this.labelX7);
            this.groupPanel2.Controls.Add(this.btnSavePix);
            this.groupPanel2.Controls.Add(this.txtCenterAngle);
            this.groupPanel2.Controls.Add(this.btnGetPos);
            this.groupPanel2.Controls.Add(this.txtSetAngle);
            this.groupPanel2.Controls.Add(this.txtTeachAngle);
            this.groupPanel2.Controls.Add(this.labelX3);
            this.groupPanel2.Controls.Add(this.btnGoFixtureSnap);
            this.groupPanel2.Controls.Add(this.labelX19);
            this.groupPanel2.Controls.Add(this.labelX8);
            this.groupPanel2.Controls.Add(this.txtCenterCol);
            this.groupPanel2.Controls.Add(this.labelX2);
            this.groupPanel2.Controls.Add(this.txtSetY);
            this.groupPanel2.Controls.Add(this.txtTeachY);
            this.groupPanel2.Controls.Add(this.labelX18);
            this.groupPanel2.Controls.Add(this.txtSetX);
            this.groupPanel2.Controls.Add(this.labelX9);
            this.groupPanel2.Controls.Add(this.txtTeachX);
            this.groupPanel2.Controls.Add(this.labelX17);
            this.groupPanel2.Controls.Add(this.labelX1);
            this.groupPanel2.Controls.Add(this.labelX10);
            this.groupPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupPanel2.Location = new System.Drawing.Point(0, 0);
            this.groupPanel2.Name = "groupPanel2";
            this.groupPanel2.Size = new System.Drawing.Size(469, 272);
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
            this.groupPanel2.TabIndex = 77;
            this.groupPanel2.Text = "治具示教";
            // 
            // cmbCamera
            // 
            this.cmbCamera.DisplayMember = "Text";
            this.cmbCamera.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbCamera.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbCamera.FormattingEnabled = true;
            this.cmbCamera.ItemHeight = 17;
            this.cmbCamera.Location = new System.Drawing.Point(76, 24);
            this.cmbCamera.Name = "cmbCamera";
            this.cmbCamera.Size = new System.Drawing.Size(90, 23);
            this.cmbCamera.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbCamera.TabIndex = 79;
            // 
            // labelX4
            // 
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX4.Location = new System.Drawing.Point(7, 24);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(60, 23);
            this.labelX4.TabIndex = 78;
            this.labelX4.Text = "相机选择:";
            // 
            // cmbFixture
            // 
            this.cmbFixture.DisplayMember = "Text";
            this.cmbFixture.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbFixture.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbFixture.FormattingEnabled = true;
            this.cmbFixture.ItemHeight = 17;
            this.cmbFixture.Location = new System.Drawing.Point(235, 24);
            this.cmbFixture.Name = "cmbFixture";
            this.cmbFixture.Size = new System.Drawing.Size(77, 23);
            this.cmbFixture.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbFixture.TabIndex = 75;
            this.cmbFixture.SelectedIndexChanged += new System.EventHandler(this.cmbFixture_SelectedIndexChanged);
            // 
            // labelX7
            // 
            // 
            // 
            // 
            this.labelX7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX7.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX7.Location = new System.Drawing.Point(176, 24);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(65, 23);
            this.labelX7.TabIndex = 0;
            this.labelX7.Text = "治具选择:";
            // 
            // btnSavePix
            // 
            this.btnSavePix.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSavePix.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSavePix.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSavePix.Location = new System.Drawing.Point(320, 204);
            this.btnSavePix.Name = "btnSavePix";
            this.btnSavePix.Size = new System.Drawing.Size(99, 35);
            this.btnSavePix.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSavePix.TabIndex = 0;
            this.btnSavePix.Text = "保存图像坐标";
            this.btnSavePix.Click += new System.EventHandler(this.btnSavePix_Click);
            // 
            // btnGetPos
            // 
            this.btnGetPos.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnGetPos.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnGetPos.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGetPos.Location = new System.Drawing.Point(182, 204);
            this.btnGetPos.Name = "btnGetPos";
            this.btnGetPos.Size = new System.Drawing.Size(99, 35);
            this.btnGetPos.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnGetPos.TabIndex = 0;
            this.btnGetPos.Text = "获取取放料位置";
            this.btnGetPos.Click += new System.EventHandler(this.btnGetPos_Click);
            // 
            // labelX3
            // 
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX3.Location = new System.Drawing.Point(7, 68);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(72, 23);
            this.labelX3.TabIndex = 0;
            this.labelX3.Text = "中心Row：";
            // 
            // btnGoFixtureSnap
            // 
            this.btnGoFixtureSnap.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnGoFixtureSnap.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnGoFixtureSnap.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGoFixtureSnap.Location = new System.Drawing.Point(44, 204);
            this.btnGoFixtureSnap.Name = "btnGoFixtureSnap";
            this.btnGoFixtureSnap.Size = new System.Drawing.Size(99, 35);
            this.btnGoFixtureSnap.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnGoFixtureSnap.TabIndex = 0;
            this.btnGoFixtureSnap.Text = "到治具拍照位置";
            this.btnGoFixtureSnap.Click += new System.EventHandler(this.btnGoFixtureSnap_Click);
            // 
            // labelX19
            // 
            // 
            // 
            // 
            this.labelX19.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX19.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX19.Location = new System.Drawing.Point(321, 68);
            this.labelX19.Name = "labelX19";
            this.labelX19.Size = new System.Drawing.Size(48, 23);
            this.labelX19.TabIndex = 0;
            this.labelX19.Text = "放料X:";
            // 
            // labelX8
            // 
            // 
            // 
            // 
            this.labelX8.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX8.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX8.Location = new System.Drawing.Point(176, 67);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(48, 23);
            this.labelX8.TabIndex = 0;
            this.labelX8.Text = "取料X:";
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX2.Location = new System.Drawing.Point(7, 114);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(72, 23);
            this.labelX2.TabIndex = 0;
            this.labelX2.Text = "中心Col:";
            // 
            // labelX18
            // 
            // 
            // 
            // 
            this.labelX18.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX18.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX18.Location = new System.Drawing.Point(321, 114);
            this.labelX18.Name = "labelX18";
            this.labelX18.Size = new System.Drawing.Size(48, 23);
            this.labelX18.TabIndex = 0;
            this.labelX18.Text = "放料Y:";
            // 
            // labelX9
            // 
            // 
            // 
            // 
            this.labelX9.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX9.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX9.Location = new System.Drawing.Point(176, 113);
            this.labelX9.Name = "labelX9";
            this.labelX9.Size = new System.Drawing.Size(48, 23);
            this.labelX9.TabIndex = 0;
            this.labelX9.Text = "取料Y:";
            // 
            // labelX17
            // 
            // 
            // 
            // 
            this.labelX17.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX17.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX17.Location = new System.Drawing.Point(321, 160);
            this.labelX17.Name = "labelX17";
            this.labelX17.Size = new System.Drawing.Size(48, 23);
            this.labelX17.TabIndex = 0;
            this.labelX17.Text = "放角度:";
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX1.Location = new System.Drawing.Point(7, 160);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(72, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "中心角度:";
            // 
            // labelX10
            // 
            // 
            // 
            // 
            this.labelX10.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX10.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX10.Location = new System.Drawing.Point(176, 159);
            this.labelX10.Name = "labelX10";
            this.labelX10.Size = new System.Drawing.Size(48, 23);
            this.labelX10.TabIndex = 0;
            this.labelX10.Text = "取角度:";
            // 
            // panelBtn
            // 
            this.panelBtn.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelBtn.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelBtn.Controls.Add(this.btnAlgorithm);
            this.panelBtn.Controls.Add(this.btnDrawLine);
            this.panelBtn.Controls.Add(this.btnSaveParam);
            this.panelBtn.Controls.Add(this.btnSnap);
            this.panelBtn.Controls.Add(this.btnLoadImage);
            this.panelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBtn.Location = new System.Drawing.Point(657, 484);
            this.panelBtn.Margin = new System.Windows.Forms.Padding(1);
            this.panelBtn.Name = "panelBtn";
            this.panelBtn.Size = new System.Drawing.Size(469, 175);
            this.panelBtn.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelBtn.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelBtn.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelBtn.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelBtn.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelBtn.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelBtn.Style.GradientAngle = 90;
            this.panelBtn.TabIndex = 1;
            // 
            // btnAlgorithm
            // 
            this.btnAlgorithm.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAlgorithm.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAlgorithm.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAlgorithm.Location = new System.Drawing.Point(196, 112);
            this.btnAlgorithm.Name = "btnAlgorithm";
            this.btnAlgorithm.Size = new System.Drawing.Size(92, 44);
            this.btnAlgorithm.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnAlgorithm.TabIndex = 0;
            this.btnAlgorithm.Text = "执行算法";
            this.btnAlgorithm.Click += new System.EventHandler(this.btnAlgorithm_Click);
            // 
            // btnDrawLine
            // 
            this.btnDrawLine.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDrawLine.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnDrawLine.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDrawLine.Location = new System.Drawing.Point(196, 28);
            this.btnDrawLine.Name = "btnDrawLine";
            this.btnDrawLine.Size = new System.Drawing.Size(92, 44);
            this.btnDrawLine.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnDrawLine.TabIndex = 0;
            this.btnDrawLine.Text = "画四条边线";
            this.btnDrawLine.Click += new System.EventHandler(this.btnDrawLine_Click);
            // 
            // btnSaveParam
            // 
            this.btnSaveParam.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSaveParam.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSaveParam.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSaveParam.Location = new System.Drawing.Point(359, 28);
            this.btnSaveParam.Name = "btnSaveParam";
            this.btnSaveParam.Size = new System.Drawing.Size(92, 44);
            this.btnSaveParam.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSaveParam.TabIndex = 0;
            this.btnSaveParam.Text = "保存参数";
            this.btnSaveParam.Click += new System.EventHandler(this.btnSaveParam_Click);
            // 
            // btnSnap
            // 
            this.btnSnap.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSnap.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSnap.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSnap.Location = new System.Drawing.Point(33, 28);
            this.btnSnap.Name = "btnSnap";
            this.btnSnap.Size = new System.Drawing.Size(92, 44);
            this.btnSnap.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSnap.TabIndex = 0;
            this.btnSnap.Text = "拍照";
            this.btnSnap.Click += new System.EventHandler(this.btnSnap_Click);
            // 
            // btnLoadImage
            // 
            this.btnLoadImage.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLoadImage.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnLoadImage.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLoadImage.Location = new System.Drawing.Point(33, 112);
            this.btnLoadImage.Name = "btnLoadImage";
            this.btnLoadImage.Size = new System.Drawing.Size(92, 44);
            this.btnLoadImage.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnLoadImage.TabIndex = 0;
            this.btnLoadImage.Text = "加载图片";
            this.btnLoadImage.Click += new System.EventHandler(this.btnLoadImage_Click);
            // 
            // labelX20
            // 
            // 
            // 
            // 
            this.labelX20.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX20.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX20.Location = new System.Drawing.Point(6, 149);
            this.labelX20.Name = "labelX20";
            this.labelX20.Size = new System.Drawing.Size(93, 23);
            this.labelX20.TabIndex = 0;
            this.labelX20.Text = "曝光时间：";
            this.labelX20.Visible = false;
            // 
            // numExposureTime
            // 
            // 
            // 
            // 
            this.numExposureTime.BackgroundStyle.BackColor = System.Drawing.Color.White;
            this.numExposureTime.BackgroundStyle.Class = "DateTimeInputBackground";
            this.numExposureTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.numExposureTime.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.numExposureTime.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numExposureTime.Location = new System.Drawing.Point(106, 149);
            this.numExposureTime.Name = "numExposureTime";
            this.numExposureTime.ShowUpDown = true;
            this.numExposureTime.Size = new System.Drawing.Size(118, 23);
            this.numExposureTime.TabIndex = 80;
            this.numExposureTime.Visible = false;
            // 
            // logView1
            // 
            this.logView1.BackColor = System.Drawing.Color.Transparent;
            this.logView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logView1.Location = new System.Drawing.Point(1, 484);
            this.logView1.Margin = new System.Windows.Forms.Padding(1);
            this.logView1.Name = "logView1";
            this.logView1.Size = new System.Drawing.Size(654, 175);
            this.logView1.TabIndex = 0;
            // 
            // txtCenterRow
            // 
            this.txtCenterRow.BackColor = System.Drawing.Color.White;
            this.txtCenterRow.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtCenterRow.IsMultiLine = false;
            this.txtCenterRow.IsPassword = false;
            this.txtCenterRow.Location = new System.Drawing.Point(76, 66);
            this.txtCenterRow.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCenterRow.Name = "txtCenterRow";
            this.txtCenterRow.Size = new System.Drawing.Size(90, 26);
            this.txtCenterRow.sText = "";
            this.txtCenterRow.TabIndex = 66;
            // 
            // txtCenterAngle
            // 
            this.txtCenterAngle.BackColor = System.Drawing.Color.White;
            this.txtCenterAngle.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtCenterAngle.IsMultiLine = false;
            this.txtCenterAngle.IsPassword = false;
            this.txtCenterAngle.Location = new System.Drawing.Point(76, 158);
            this.txtCenterAngle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCenterAngle.Name = "txtCenterAngle";
            this.txtCenterAngle.Size = new System.Drawing.Size(90, 26);
            this.txtCenterAngle.sText = "";
            this.txtCenterAngle.TabIndex = 66;
            // 
            // txtSetAngle
            // 
            this.txtSetAngle.BackColor = System.Drawing.Color.White;
            this.txtSetAngle.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSetAngle.IsMultiLine = false;
            this.txtSetAngle.IsPassword = false;
            this.txtSetAngle.Location = new System.Drawing.Point(380, 158);
            this.txtSetAngle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSetAngle.Name = "txtSetAngle";
            this.txtSetAngle.Size = new System.Drawing.Size(77, 26);
            this.txtSetAngle.sText = "";
            this.txtSetAngle.TabIndex = 66;
            // 
            // txtTeachAngle
            // 
            this.txtTeachAngle.BackColor = System.Drawing.Color.White;
            this.txtTeachAngle.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtTeachAngle.IsMultiLine = false;
            this.txtTeachAngle.IsPassword = false;
            this.txtTeachAngle.Location = new System.Drawing.Point(235, 157);
            this.txtTeachAngle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTeachAngle.Name = "txtTeachAngle";
            this.txtTeachAngle.Size = new System.Drawing.Size(77, 26);
            this.txtTeachAngle.sText = "";
            this.txtTeachAngle.TabIndex = 66;
            // 
            // txtCenterCol
            // 
            this.txtCenterCol.BackColor = System.Drawing.Color.White;
            this.txtCenterCol.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtCenterCol.IsMultiLine = false;
            this.txtCenterCol.IsPassword = false;
            this.txtCenterCol.Location = new System.Drawing.Point(76, 112);
            this.txtCenterCol.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCenterCol.Name = "txtCenterCol";
            this.txtCenterCol.Size = new System.Drawing.Size(90, 26);
            this.txtCenterCol.sText = "";
            this.txtCenterCol.TabIndex = 66;
            // 
            // txtSetY
            // 
            this.txtSetY.BackColor = System.Drawing.Color.White;
            this.txtSetY.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSetY.IsMultiLine = false;
            this.txtSetY.IsPassword = false;
            this.txtSetY.Location = new System.Drawing.Point(380, 112);
            this.txtSetY.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSetY.Name = "txtSetY";
            this.txtSetY.Size = new System.Drawing.Size(77, 26);
            this.txtSetY.sText = "";
            this.txtSetY.TabIndex = 66;
            // 
            // txtTeachY
            // 
            this.txtTeachY.BackColor = System.Drawing.Color.White;
            this.txtTeachY.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtTeachY.IsMultiLine = false;
            this.txtTeachY.IsPassword = false;
            this.txtTeachY.Location = new System.Drawing.Point(235, 111);
            this.txtTeachY.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTeachY.Name = "txtTeachY";
            this.txtTeachY.Size = new System.Drawing.Size(77, 26);
            this.txtTeachY.sText = "";
            this.txtTeachY.TabIndex = 66;
            // 
            // txtSetX
            // 
            this.txtSetX.BackColor = System.Drawing.Color.White;
            this.txtSetX.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSetX.IsMultiLine = false;
            this.txtSetX.IsPassword = false;
            this.txtSetX.Location = new System.Drawing.Point(380, 66);
            this.txtSetX.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSetX.Name = "txtSetX";
            this.txtSetX.Size = new System.Drawing.Size(77, 26);
            this.txtSetX.sText = "";
            this.txtSetX.TabIndex = 66;
            // 
            // txtTeachX
            // 
            this.txtTeachX.BackColor = System.Drawing.Color.White;
            this.txtTeachX.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtTeachX.IsMultiLine = false;
            this.txtTeachX.IsPassword = false;
            this.txtTeachX.Location = new System.Drawing.Point(235, 65);
            this.txtTeachX.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTeachX.Name = "txtTeachX";
            this.txtTeachX.Size = new System.Drawing.Size(77, 26);
            this.txtTeachX.sText = "";
            this.txtTeachX.TabIndex = 66;
            // 
            // FixtureTeachView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FixtureTeachView";
            this.Size = new System.Drawing.Size(1127, 660);
            this.Load += new System.EventHandler(this.FixtureTeachView_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelParam.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numSocre)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSigma)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLength2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLength1)).EndInit();
            this.groupPanel2.ResumeLayout(false);
            this.panelBtn.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numExposureTime)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Comment.LogView logView1;
        private DevComponents.DotNetBar.PanelEx panelView;
        private DevComponents.DotNetBar.PanelEx panelParam;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
        private DevComponents.DotNetBar.ButtonX btnGetPos;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbFixture;
        private DevComponents.DotNetBar.LabelX labelX7;
        private Comment.TextInput txtTeachAngle;
        private DevComponents.DotNetBar.LabelX labelX8;
        private Comment.TextInput txtTeachY;
        private DevComponents.DotNetBar.LabelX labelX9;
        private Comment.TextInput txtTeachX;
        private DevComponents.DotNetBar.ButtonX btnGoFixtureSnap;
        private DevComponents.DotNetBar.LabelX labelX10;
        private DevComponents.DotNetBar.PanelEx panelBtn;
        private DevComponents.DotNetBar.ButtonX btnAlgorithm;
        private DevComponents.DotNetBar.ButtonX btnSnap;
        private DevComponents.DotNetBar.ButtonX btnLoadImage;
        private Comment.TextInput txtCenterAngle;
        private DevComponents.DotNetBar.LabelX labelX3;
        private Comment.TextInput txtCenterCol;
        private DevComponents.DotNetBar.LabelX labelX2;
        private Comment.TextInput txtCenterRow;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbCamera;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbTransition;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbSelect;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.LabelX labelX11;
        private DevComponents.DotNetBar.LabelX labelX12;
        private DevComponents.DotNetBar.LabelX labelX13;
        private DevComponents.DotNetBar.LabelX labelX14;
        private DevComponents.DotNetBar.LabelX labelX15;
        private DevComponents.DotNetBar.LabelX labelX16;
        private DevComponents.Editors.DoubleInput numSocre;
        private DevComponents.Editors.DoubleInput numSigma;
        private DevComponents.Editors.IntegerInput numCount;
        private DevComponents.Editors.IntegerInput numThreshold;
        private DevComponents.Editors.IntegerInput numLength2;
        private DevComponents.Editors.IntegerInput numLength1;
        private DevComponents.Editors.ComboItem comboItem1;
        private DevComponents.Editors.ComboItem comboItem2;
        private DevComponents.Editors.ComboItem comboItem3;
        private DevComponents.Editors.ComboItem comboItem4;
        private DevComponents.Editors.ComboItem comboItem5;
        private DevComponents.Editors.ComboItem comboItem6;
        private DevComponents.DotNetBar.ButtonX btnSaveParam;
        private DevComponents.DotNetBar.ButtonX btnDrawLine;
        private DevComponents.DotNetBar.ButtonX btnSavePix;
        private Comment.TextInput txtSetAngle;
        private DevComponents.DotNetBar.LabelX labelX19;
        private Comment.TextInput txtSetY;
        private DevComponents.DotNetBar.LabelX labelX18;
        private Comment.TextInput txtSetX;
        private DevComponents.DotNetBar.LabelX labelX17;
        private DevComponents.Editors.IntegerInput numExposureTime;
        private DevComponents.DotNetBar.LabelX labelX20;
    }
}
