namespace ManagementView.MotorView
{
    partial class AxisOffSetView
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dataView = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.groupU = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.txtOffSetU = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtUMin = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtUMax = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX13 = new DevComponents.DotNetBar.LabelX();
            this.labelX14 = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.groupZ = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.txtOffSetZ = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX11 = new DevComponents.DotNetBar.LabelX();
            this.txtZMin = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtZMax = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX12 = new DevComponents.DotNetBar.LabelX();
            this.groupY = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.txtOffSetY = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX9 = new DevComponents.DotNetBar.LabelX();
            this.txtYMin = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtYMax = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX10 = new DevComponents.DotNetBar.LabelX();
            this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.txtOffSetX = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.txtXMin = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtXMax = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.txtEndIndex = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtStartIndex = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.btnSet = new DevComponents.DotNetBar.ButtonX();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataView)).BeginInit();
            this.panelEx1.SuspendLayout();
            this.groupU.SuspendLayout();
            this.groupZ.SuspendLayout();
            this.groupY.SuspendLayout();
            this.groupPanel2.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 77.30159F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.69841F));
            this.tableLayoutPanel1.Controls.Add(this.dataView, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelEx1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1260, 700);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // dataView
            // 
            this.dataView.AllowUserToResizeRows = false;
            this.dataView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataView.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataView.Location = new System.Drawing.Point(3, 3);
            this.dataView.MultiSelect = false;
            this.dataView.Name = "dataView";
            this.dataView.RowHeadersWidth = 35;
            this.dataView.RowTemplate.Height = 23;
            this.dataView.Size = new System.Drawing.Size(968, 694);
            this.dataView.TabIndex = 4;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Column1.DataPropertyName = "Id";
            this.Column1.HeaderText = "Id";
            this.Column1.Name = "Column1";
            this.Column1.Width = 42;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "Name";
            this.Column2.HeaderText = "Name";
            this.Column2.Name = "Column2";
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.groupU);
            this.panelEx1.Controls.Add(this.groupZ);
            this.panelEx1.Controls.Add(this.groupY);
            this.panelEx1.Controls.Add(this.groupPanel2);
            this.panelEx1.Controls.Add(this.groupPanel1);
            this.panelEx1.Controls.Add(this.btnSet);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(977, 3);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(280, 694);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 2;
            // 
            // groupU
            // 
            this.groupU.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupU.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupU.Controls.Add(this.txtOffSetU);
            this.groupU.Controls.Add(this.txtUMin);
            this.groupU.Controls.Add(this.txtUMax);
            this.groupU.Controls.Add(this.labelX13);
            this.groupU.Controls.Add(this.labelX14);
            this.groupU.Controls.Add(this.labelX4);
            this.groupU.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupU.Location = new System.Drawing.Point(0, 448);
            this.groupU.Name = "groupU";
            this.groupU.Size = new System.Drawing.Size(280, 116);
            // 
            // 
            // 
            this.groupU.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupU.Style.BackColorGradientAngle = 90;
            this.groupU.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupU.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupU.Style.BorderBottomWidth = 1;
            this.groupU.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupU.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupU.Style.BorderLeftWidth = 1;
            this.groupU.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupU.Style.BorderRightWidth = 1;
            this.groupU.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupU.Style.BorderTopWidth = 1;
            this.groupU.Style.CornerDiameter = 4;
            this.groupU.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupU.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupU.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupU.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupU.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupU.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupU.TabIndex = 7;
            this.groupU.Text = "U轴设置";
            // 
            // txtOffSetU
            // 
            // 
            // 
            // 
            this.txtOffSetU.Border.Class = "TextBoxBorder";
            this.txtOffSetU.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtOffSetU.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtOffSetU.Location = new System.Drawing.Point(92, 3);
            this.txtOffSetU.Name = "txtOffSetU";
            this.txtOffSetU.Size = new System.Drawing.Size(125, 23);
            this.txtOffSetU.TabIndex = 1;
            this.txtOffSetU.Text = "0";
            // 
            // txtUMin
            // 
            // 
            // 
            // 
            this.txtUMin.Border.Class = "TextBoxBorder";
            this.txtUMin.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtUMin.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUMin.Location = new System.Drawing.Point(92, 36);
            this.txtUMin.Name = "txtUMin";
            this.txtUMin.Size = new System.Drawing.Size(125, 23);
            this.txtUMin.TabIndex = 1;
            this.txtUMin.Text = "0";
            // 
            // txtUMax
            // 
            // 
            // 
            // 
            this.txtUMax.Border.Class = "TextBoxBorder";
            this.txtUMax.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtUMax.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUMax.Location = new System.Drawing.Point(92, 67);
            this.txtUMax.Name = "txtUMax";
            this.txtUMax.Size = new System.Drawing.Size(125, 23);
            this.txtUMax.TabIndex = 1;
            this.txtUMax.Text = "0";
            // 
            // labelX13
            // 
            // 
            // 
            // 
            this.labelX13.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX13.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX13.Location = new System.Drawing.Point(12, 36);
            this.labelX13.Name = "labelX13";
            this.labelX13.Size = new System.Drawing.Size(74, 23);
            this.labelX13.TabIndex = 2;
            this.labelX13.Text = "U最小值：";
            // 
            // labelX14
            // 
            // 
            // 
            // 
            this.labelX14.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX14.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX14.Location = new System.Drawing.Point(12, 67);
            this.labelX14.Name = "labelX14";
            this.labelX14.Size = new System.Drawing.Size(74, 23);
            this.labelX14.TabIndex = 2;
            this.labelX14.Text = "U最大值：";
            // 
            // labelX4
            // 
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX4.Location = new System.Drawing.Point(12, 3);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(74, 23);
            this.labelX4.TabIndex = 2;
            this.labelX4.Text = "U偏移量：";
            // 
            // groupZ
            // 
            this.groupZ.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupZ.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupZ.Controls.Add(this.txtOffSetZ);
            this.groupZ.Controls.Add(this.labelX3);
            this.groupZ.Controls.Add(this.labelX11);
            this.groupZ.Controls.Add(this.txtZMin);
            this.groupZ.Controls.Add(this.txtZMax);
            this.groupZ.Controls.Add(this.labelX12);
            this.groupZ.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupZ.Location = new System.Drawing.Point(0, 332);
            this.groupZ.Name = "groupZ";
            this.groupZ.Size = new System.Drawing.Size(280, 116);
            // 
            // 
            // 
            this.groupZ.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupZ.Style.BackColorGradientAngle = 90;
            this.groupZ.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupZ.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupZ.Style.BorderBottomWidth = 1;
            this.groupZ.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupZ.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupZ.Style.BorderLeftWidth = 1;
            this.groupZ.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupZ.Style.BorderRightWidth = 1;
            this.groupZ.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupZ.Style.BorderTopWidth = 1;
            this.groupZ.Style.CornerDiameter = 4;
            this.groupZ.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupZ.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupZ.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupZ.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupZ.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupZ.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupZ.TabIndex = 6;
            this.groupZ.Text = "Z轴设置";
            // 
            // txtOffSetZ
            // 
            // 
            // 
            // 
            this.txtOffSetZ.Border.Class = "TextBoxBorder";
            this.txtOffSetZ.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtOffSetZ.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtOffSetZ.Location = new System.Drawing.Point(92, 3);
            this.txtOffSetZ.Name = "txtOffSetZ";
            this.txtOffSetZ.Size = new System.Drawing.Size(125, 23);
            this.txtOffSetZ.TabIndex = 1;
            this.txtOffSetZ.Text = "0";
            // 
            // labelX3
            // 
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX3.Location = new System.Drawing.Point(12, 3);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(74, 23);
            this.labelX3.TabIndex = 2;
            this.labelX3.Text = "Z偏移量：";
            // 
            // labelX11
            // 
            // 
            // 
            // 
            this.labelX11.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX11.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX11.Location = new System.Drawing.Point(12, 35);
            this.labelX11.Name = "labelX11";
            this.labelX11.Size = new System.Drawing.Size(74, 23);
            this.labelX11.TabIndex = 2;
            this.labelX11.Text = "Z最小值：";
            // 
            // txtZMin
            // 
            // 
            // 
            // 
            this.txtZMin.Border.Class = "TextBoxBorder";
            this.txtZMin.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtZMin.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtZMin.Location = new System.Drawing.Point(92, 35);
            this.txtZMin.Name = "txtZMin";
            this.txtZMin.Size = new System.Drawing.Size(125, 23);
            this.txtZMin.TabIndex = 1;
            this.txtZMin.Text = "0";
            // 
            // txtZMax
            // 
            // 
            // 
            // 
            this.txtZMax.Border.Class = "TextBoxBorder";
            this.txtZMax.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtZMax.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtZMax.Location = new System.Drawing.Point(92, 68);
            this.txtZMax.Name = "txtZMax";
            this.txtZMax.Size = new System.Drawing.Size(125, 23);
            this.txtZMax.TabIndex = 1;
            this.txtZMax.Text = "0";
            // 
            // labelX12
            // 
            // 
            // 
            // 
            this.labelX12.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX12.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX12.Location = new System.Drawing.Point(12, 68);
            this.labelX12.Name = "labelX12";
            this.labelX12.Size = new System.Drawing.Size(74, 23);
            this.labelX12.TabIndex = 2;
            this.labelX12.Text = "Z最大值：";
            // 
            // groupY
            // 
            this.groupY.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupY.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupY.Controls.Add(this.txtOffSetY);
            this.groupY.Controls.Add(this.labelX2);
            this.groupY.Controls.Add(this.labelX9);
            this.groupY.Controls.Add(this.txtYMin);
            this.groupY.Controls.Add(this.txtYMax);
            this.groupY.Controls.Add(this.labelX10);
            this.groupY.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupY.Location = new System.Drawing.Point(0, 216);
            this.groupY.Name = "groupY";
            this.groupY.Size = new System.Drawing.Size(280, 116);
            // 
            // 
            // 
            this.groupY.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupY.Style.BackColorGradientAngle = 90;
            this.groupY.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupY.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupY.Style.BorderBottomWidth = 1;
            this.groupY.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupY.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupY.Style.BorderLeftWidth = 1;
            this.groupY.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupY.Style.BorderRightWidth = 1;
            this.groupY.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupY.Style.BorderTopWidth = 1;
            this.groupY.Style.CornerDiameter = 4;
            this.groupY.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupY.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupY.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupY.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupY.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupY.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupY.TabIndex = 5;
            this.groupY.Text = "Y轴设置";
            // 
            // txtOffSetY
            // 
            // 
            // 
            // 
            this.txtOffSetY.Border.Class = "TextBoxBorder";
            this.txtOffSetY.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtOffSetY.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtOffSetY.Location = new System.Drawing.Point(92, 3);
            this.txtOffSetY.Name = "txtOffSetY";
            this.txtOffSetY.Size = new System.Drawing.Size(125, 23);
            this.txtOffSetY.TabIndex = 1;
            this.txtOffSetY.Text = "0";
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX2.Location = new System.Drawing.Point(12, 3);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(74, 23);
            this.labelX2.TabIndex = 2;
            this.labelX2.Text = "Y偏移量：";
            // 
            // labelX9
            // 
            // 
            // 
            // 
            this.labelX9.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX9.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX9.Location = new System.Drawing.Point(12, 34);
            this.labelX9.Name = "labelX9";
            this.labelX9.Size = new System.Drawing.Size(74, 23);
            this.labelX9.TabIndex = 2;
            this.labelX9.Text = "Y最小值：";
            // 
            // txtYMin
            // 
            // 
            // 
            // 
            this.txtYMin.Border.Class = "TextBoxBorder";
            this.txtYMin.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtYMin.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtYMin.Location = new System.Drawing.Point(92, 34);
            this.txtYMin.Name = "txtYMin";
            this.txtYMin.Size = new System.Drawing.Size(125, 23);
            this.txtYMin.TabIndex = 1;
            this.txtYMin.Text = "0";
            // 
            // txtYMax
            // 
            // 
            // 
            // 
            this.txtYMax.Border.Class = "TextBoxBorder";
            this.txtYMax.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtYMax.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtYMax.Location = new System.Drawing.Point(92, 65);
            this.txtYMax.Name = "txtYMax";
            this.txtYMax.Size = new System.Drawing.Size(125, 23);
            this.txtYMax.TabIndex = 1;
            this.txtYMax.Text = "0";
            // 
            // labelX10
            // 
            // 
            // 
            // 
            this.labelX10.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX10.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX10.Location = new System.Drawing.Point(12, 65);
            this.labelX10.Name = "labelX10";
            this.labelX10.Size = new System.Drawing.Size(74, 23);
            this.labelX10.TabIndex = 2;
            this.labelX10.Text = "Y最大值：";
            // 
            // groupPanel2
            // 
            this.groupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel2.Controls.Add(this.txtOffSetX);
            this.groupPanel2.Controls.Add(this.labelX1);
            this.groupPanel2.Controls.Add(this.txtXMin);
            this.groupPanel2.Controls.Add(this.txtXMax);
            this.groupPanel2.Controls.Add(this.labelX7);
            this.groupPanel2.Controls.Add(this.labelX8);
            this.groupPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupPanel2.Location = new System.Drawing.Point(0, 100);
            this.groupPanel2.Name = "groupPanel2";
            this.groupPanel2.Size = new System.Drawing.Size(280, 116);
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
            this.groupPanel2.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
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
            this.groupPanel2.TabIndex = 4;
            this.groupPanel2.Text = "X轴设置";
            // 
            // txtOffSetX
            // 
            // 
            // 
            // 
            this.txtOffSetX.Border.Class = "TextBoxBorder";
            this.txtOffSetX.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtOffSetX.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtOffSetX.Location = new System.Drawing.Point(83, 3);
            this.txtOffSetX.Name = "txtOffSetX";
            this.txtOffSetX.Size = new System.Drawing.Size(125, 23);
            this.txtOffSetX.TabIndex = 1;
            this.txtOffSetX.Text = "0";
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX1.Location = new System.Drawing.Point(3, 3);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(74, 23);
            this.labelX1.TabIndex = 2;
            this.labelX1.Text = "X偏移量：";
            // 
            // txtXMin
            // 
            // 
            // 
            // 
            this.txtXMin.Border.Class = "TextBoxBorder";
            this.txtXMin.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtXMin.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtXMin.Location = new System.Drawing.Point(83, 34);
            this.txtXMin.Name = "txtXMin";
            this.txtXMin.Size = new System.Drawing.Size(125, 23);
            this.txtXMin.TabIndex = 1;
            this.txtXMin.Text = "0";
            // 
            // txtXMax
            // 
            // 
            // 
            // 
            this.txtXMax.Border.Class = "TextBoxBorder";
            this.txtXMax.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtXMax.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtXMax.Location = new System.Drawing.Point(83, 65);
            this.txtXMax.Name = "txtXMax";
            this.txtXMax.Size = new System.Drawing.Size(125, 23);
            this.txtXMax.TabIndex = 1;
            this.txtXMax.Text = "0";
            // 
            // labelX7
            // 
            // 
            // 
            // 
            this.labelX7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX7.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX7.Location = new System.Drawing.Point(3, 34);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(74, 23);
            this.labelX7.TabIndex = 2;
            this.labelX7.Text = "X最小值：";
            // 
            // labelX8
            // 
            // 
            // 
            // 
            this.labelX8.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX8.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX8.Location = new System.Drawing.Point(3, 65);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(74, 23);
            this.labelX8.TabIndex = 2;
            this.labelX8.Text = "X最大值：";
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.labelX6);
            this.groupPanel1.Controls.Add(this.txtEndIndex);
            this.groupPanel1.Controls.Add(this.txtStartIndex);
            this.groupPanel1.Controls.Add(this.labelX5);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupPanel1.Location = new System.Drawing.Point(0, 0);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(280, 100);
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
            this.groupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
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
            this.groupPanel1.TabIndex = 3;
            this.groupPanel1.Text = "序号设置";
            // 
            // labelX6
            // 
            // 
            // 
            // 
            this.labelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX6.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX6.Location = new System.Drawing.Point(3, 3);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(74, 23);
            this.labelX6.TabIndex = 2;
            this.labelX6.Text = "开始序号：";
            // 
            // txtEndIndex
            // 
            // 
            // 
            // 
            this.txtEndIndex.Border.Class = "TextBoxBorder";
            this.txtEndIndex.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtEndIndex.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtEndIndex.Location = new System.Drawing.Point(83, 41);
            this.txtEndIndex.Name = "txtEndIndex";
            this.txtEndIndex.Size = new System.Drawing.Size(125, 23);
            this.txtEndIndex.TabIndex = 1;
            this.txtEndIndex.Text = "1";
            // 
            // txtStartIndex
            // 
            // 
            // 
            // 
            this.txtStartIndex.Border.Class = "TextBoxBorder";
            this.txtStartIndex.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtStartIndex.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtStartIndex.Location = new System.Drawing.Point(83, 3);
            this.txtStartIndex.Name = "txtStartIndex";
            this.txtStartIndex.Size = new System.Drawing.Size(125, 23);
            this.txtStartIndex.TabIndex = 1;
            this.txtStartIndex.Text = "1";
            // 
            // labelX5
            // 
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX5.Location = new System.Drawing.Point(3, 41);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(74, 23);
            this.labelX5.TabIndex = 2;
            this.labelX5.Text = "结束序号：";
            // 
            // btnSet
            // 
            this.btnSet.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSet.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSet.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSet.Location = new System.Drawing.Point(95, 638);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(92, 34);
            this.btnSet.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSet.TabIndex = 0;
            this.btnSet.Text = "设置";
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // AxisOffSetView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "AxisOffSetView";
            this.Size = new System.Drawing.Size(1260, 700);
            this.Load += new System.EventHandler(this.AxisOffSetView_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataView)).EndInit();
            this.panelEx1.ResumeLayout(false);
            this.groupU.ResumeLayout(false);
            this.groupZ.ResumeLayout(false);
            this.groupY.ResumeLayout(false);
            this.groupPanel2.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtOffSetX;
        private DevComponents.DotNetBar.ButtonX btnSet;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.TextBoxX txtOffSetY;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.TextBoxX txtOffSetU;
        private DevComponents.DotNetBar.Controls.TextBoxX txtOffSetZ;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.Controls.TextBoxX txtStartIndex;
        private DevComponents.DotNetBar.Controls.TextBoxX txtEndIndex;
        private DevComponents.DotNetBar.Controls.DataGridViewX dataView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private DevComponents.DotNetBar.LabelX labelX8;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.DotNetBar.Controls.TextBoxX txtXMax;
        private DevComponents.DotNetBar.Controls.TextBoxX txtXMin;
        private DevComponents.DotNetBar.LabelX labelX14;
        private DevComponents.DotNetBar.LabelX labelX12;
        private DevComponents.DotNetBar.LabelX labelX10;
        private DevComponents.DotNetBar.LabelX labelX13;
        private DevComponents.DotNetBar.LabelX labelX11;
        private DevComponents.DotNetBar.LabelX labelX9;
        private DevComponents.DotNetBar.Controls.TextBoxX txtUMax;
        private DevComponents.DotNetBar.Controls.TextBoxX txtZMax;
        private DevComponents.DotNetBar.Controls.TextBoxX txtYMax;
        private DevComponents.DotNetBar.Controls.TextBoxX txtUMin;
        private DevComponents.DotNetBar.Controls.TextBoxX txtZMin;
        private DevComponents.DotNetBar.Controls.TextBoxX txtYMin;
        private DevComponents.DotNetBar.Controls.GroupPanel groupU;
        private DevComponents.DotNetBar.Controls.GroupPanel groupZ;
        private DevComponents.DotNetBar.Controls.GroupPanel groupY;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
    }
}
