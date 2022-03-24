namespace ManagementView
{
    partial class MapView
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.btnDel = new DevComponents.DotNetBar.ButtonX();
            this.btnEnter = new DevComponents.DotNetBar.ButtonX();
            this.btnCheckWafer = new DevComponents.DotNetBar.ButtonX();
            this.loadMapPath = new ManagementView._3DViews.CommonView.LoadFileView();
            this.cmbProduct = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem21 = new DevComponents.Editors.ComboItem();
            this.comboItem22 = new DevComponents.Editors.ComboItem();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.cmbIndex = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.comboItem2 = new DevComponents.Editors.ComboItem();
            this.comboItem3 = new DevComponents.Editors.ComboItem();
            this.comboItem4 = new DevComponents.Editors.ComboItem();
            this.comboItem5 = new DevComponents.Editors.ComboItem();
            this.comboItem6 = new DevComponents.Editors.ComboItem();
            this.comboItem7 = new DevComponents.Editors.ComboItem();
            this.comboItem8 = new DevComponents.Editors.ComboItem();
            this.comboItem9 = new DevComponents.Editors.ComboItem();
            this.comboItem10 = new DevComponents.Editors.ComboItem();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.txtWaferNo = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.dataMapView = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupPanel3 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.txtCurrentOcr = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.btnChangeRow = new DevComponents.DotNetBar.ButtonX();
            this.btnRefreshData = new DevComponents.DotNetBar.ButtonX();
            this.txtAlreadyCount = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.txtGetCount = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.groupPanel4 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.txtSetBar = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.btnMapTest = new DevComponents.DotNetBar.ButtonX();
            this.btnUpdate = new DevComponents.DotNetBar.ButtonX();
            this.panelBlow = new DevComponents.DotNetBar.PanelEx();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.txtCurrentRow = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.groupPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataMapView)).BeginInit();
            this.groupPanel3.SuspendLayout();
            this.groupPanel4.SuspendLayout();
            this.panelBlow.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.3F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.7F));
            this.tableLayoutPanel1.Controls.Add(this.groupPanel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupPanel3, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupPanel4, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 135F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 365F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1000, 662);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.btnDel);
            this.groupPanel1.Controls.Add(this.btnEnter);
            this.groupPanel1.Controls.Add(this.btnCheckWafer);
            this.groupPanel1.Controls.Add(this.loadMapPath);
            this.groupPanel1.Controls.Add(this.cmbProduct);
            this.groupPanel1.Controls.Add(this.labelX7);
            this.groupPanel1.Controls.Add(this.cmbIndex);
            this.groupPanel1.Controls.Add(this.labelX2);
            this.groupPanel1.Controls.Add(this.labelX1);
            this.groupPanel1.Controls.Add(this.txtWaferNo);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupPanel1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupPanel1.Location = new System.Drawing.Point(0, 0);
            this.groupPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(703, 135);
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
            this.groupPanel1.TabIndex = 14;
            this.groupPanel1.Text = "上料Map录入";
            // 
            // btnDel
            // 
            this.btnDel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnDel.Location = new System.Drawing.Point(606, 18);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 23);
            this.btnDel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnDel.TabIndex = 4;
            this.btnDel.Text = "删除";
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnEnter
            // 
            this.btnEnter.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnEnter.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnEnter.Location = new System.Drawing.Point(506, 18);
            this.btnEnter.Name = "btnEnter";
            this.btnEnter.Size = new System.Drawing.Size(75, 23);
            this.btnEnter.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnEnter.TabIndex = 4;
            this.btnEnter.Text = "录入";
            this.btnEnter.Click += new System.EventHandler(this.btnEnter_Click);
            // 
            // btnCheckWafer
            // 
            this.btnCheckWafer.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCheckWafer.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCheckWafer.Location = new System.Drawing.Point(606, 68);
            this.btnCheckWafer.Name = "btnCheckWafer";
            this.btnCheckWafer.Size = new System.Drawing.Size(75, 23);
            this.btnCheckWafer.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnCheckWafer.TabIndex = 4;
            this.btnCheckWafer.Text = "检查Wafer";
            this.btnCheckWafer.Click += new System.EventHandler(this.btnCheckWafer_Click);
            // 
            // loadMapPath
            // 
            this.loadMapPath.FileFilter = null;
            this.loadMapPath.FileName = "文件路径";
            this.loadMapPath.FilePath = null;
            this.loadMapPath.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.loadMapPath.Location = new System.Drawing.Point(17, 63);
            this.loadMapPath.Name = "loadMapPath";
            this.loadMapPath.Size = new System.Drawing.Size(564, 28);
            this.loadMapPath.TabIndex = 3;
            this.loadMapPath.TextLength = 65;
            // 
            // cmbProduct
            // 
            this.cmbProduct.DisplayMember = "Text";
            this.cmbProduct.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbProduct.FormattingEnabled = true;
            this.cmbProduct.ItemHeight = 17;
            this.cmbProduct.Items.AddRange(new object[] {
            this.comboItem21,
            this.comboItem22});
            this.cmbProduct.Location = new System.Drawing.Point(388, 18);
            this.cmbProduct.Name = "cmbProduct";
            this.cmbProduct.Size = new System.Drawing.Size(97, 23);
            this.cmbProduct.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbProduct.TabIndex = 2;
            this.cmbProduct.Text = "25W";
            // 
            // comboItem21
            // 
            this.comboItem21.Text = "25W";
            // 
            // comboItem22
            // 
            this.comboItem22.Text = "28W";
            // 
            // labelX7
            // 
            // 
            // 
            // 
            this.labelX7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX7.Location = new System.Drawing.Point(346, 18);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(53, 23);
            this.labelX7.TabIndex = 1;
            this.labelX7.Text = "产品.";
            // 
            // cmbIndex
            // 
            this.cmbIndex.DisplayMember = "Text";
            this.cmbIndex.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbIndex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbIndex.FormattingEnabled = true;
            this.cmbIndex.ItemHeight = 17;
            this.cmbIndex.Items.AddRange(new object[] {
            this.comboItem1,
            this.comboItem2,
            this.comboItem3,
            this.comboItem4,
            this.comboItem5,
            this.comboItem6,
            this.comboItem7,
            this.comboItem8,
            this.comboItem9,
            this.comboItem10});
            this.cmbIndex.Location = new System.Drawing.Point(251, 18);
            this.cmbIndex.Name = "cmbIndex";
            this.cmbIndex.Size = new System.Drawing.Size(66, 23);
            this.cmbIndex.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbIndex.TabIndex = 2;
            // 
            // comboItem1
            // 
            this.comboItem1.Text = "1";
            // 
            // comboItem2
            // 
            this.comboItem2.Text = "2";
            // 
            // comboItem3
            // 
            this.comboItem3.Text = "3";
            // 
            // comboItem4
            // 
            this.comboItem4.Text = "4";
            // 
            // comboItem5
            // 
            this.comboItem5.Text = "5";
            // 
            // comboItem6
            // 
            this.comboItem6.Text = "6";
            // 
            // comboItem7
            // 
            this.comboItem7.Text = "7";
            // 
            // comboItem8
            // 
            this.comboItem8.Text = "8";
            // 
            // comboItem9
            // 
            this.comboItem9.Text = "9";
            // 
            // comboItem10
            // 
            this.comboItem10.Text = "10";
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(209, 18);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(53, 23);
            this.labelX2.TabIndex = 1;
            this.labelX2.Text = "序号.";
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(20, 17);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(53, 23);
            this.labelX1.TabIndex = 1;
            this.labelX1.Text = "蓝膜No.";
            // 
            // txtWaferNo
            // 
            // 
            // 
            // 
            this.txtWaferNo.Border.Class = "TextBoxBorder";
            this.txtWaferNo.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtWaferNo.Location = new System.Drawing.Point(84, 18);
            this.txtWaferNo.Name = "txtWaferNo";
            this.txtWaferNo.Size = new System.Drawing.Size(100, 23);
            this.txtWaferNo.TabIndex = 0;
            // 
            // groupPanel2
            // 
            this.groupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel2.Controls.Add(this.dataMapView);
            this.groupPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupPanel2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupPanel2.Location = new System.Drawing.Point(0, 135);
            this.groupPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.groupPanel2.Name = "groupPanel2";
            this.tableLayoutPanel1.SetRowSpan(this.groupPanel2, 2);
            this.groupPanel2.Size = new System.Drawing.Size(703, 527);
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
            this.groupPanel2.TabIndex = 15;
            this.groupPanel2.Text = "录入数据";
            // 
            // dataMapView
            // 
            this.dataMapView.AllowUserToAddRows = false;
            this.dataMapView.AllowUserToResizeRows = false;
            this.dataMapView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataMapView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataMapView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataMapView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column5,
            this.Column3,
            this.Column2});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataMapView.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataMapView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataMapView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataMapView.Location = new System.Drawing.Point(0, 0);
            this.dataMapView.Margin = new System.Windows.Forms.Padding(0);
            this.dataMapView.Name = "dataMapView";
            this.dataMapView.RowTemplate.Height = 23;
            this.dataMapView.Size = new System.Drawing.Size(697, 501);
            this.dataMapView.TabIndex = 1;
            this.dataMapView.SelectionChanged += new System.EventHandler(this.dataMapView_SelectionChanged);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Column1.DataPropertyName = "Index";
            this.Column1.HeaderText = "序号";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 57;
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column5.DataPropertyName = "IsEffective";
            this.Column5.HeaderText = "Wafer是否有效";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 97;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Column3.DataPropertyName = "WaferNo";
            this.Column3.HeaderText = "Wafer号";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 80;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "WaferPath";
            this.Column2.HeaderText = "路径";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // groupPanel3
            // 
            this.groupPanel3.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel3.Controls.Add(this.txtCurrentOcr);
            this.groupPanel3.Controls.Add(this.labelX5);
            this.groupPanel3.Controls.Add(this.btnChangeRow);
            this.groupPanel3.Controls.Add(this.btnRefreshData);
            this.groupPanel3.Controls.Add(this.txtAlreadyCount);
            this.groupPanel3.Controls.Add(this.labelX4);
            this.groupPanel3.Controls.Add(this.txtCurrentRow);
            this.groupPanel3.Controls.Add(this.labelX8);
            this.groupPanel3.Controls.Add(this.txtGetCount);
            this.groupPanel3.Controls.Add(this.labelX3);
            this.groupPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupPanel3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupPanel3.Location = new System.Drawing.Point(703, 0);
            this.groupPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.groupPanel3.Name = "groupPanel3";
            this.tableLayoutPanel1.SetRowSpan(this.groupPanel3, 2);
            this.groupPanel3.Size = new System.Drawing.Size(297, 297);
            // 
            // 
            // 
            this.groupPanel3.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel3.Style.BackColorGradientAngle = 90;
            this.groupPanel3.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel3.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderBottomWidth = 1;
            this.groupPanel3.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel3.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderLeftWidth = 1;
            this.groupPanel3.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderRightWidth = 1;
            this.groupPanel3.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderTopWidth = 1;
            this.groupPanel3.Style.CornerDiameter = 4;
            this.groupPanel3.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel3.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel3.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupPanel3.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel3.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel3.TabIndex = 16;
            this.groupPanel3.Text = "Map数据显示";
            // 
            // txtCurrentOcr
            // 
            // 
            // 
            // 
            this.txtCurrentOcr.Border.Class = "TextBoxBorder";
            this.txtCurrentOcr.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtCurrentOcr.Location = new System.Drawing.Point(116, 151);
            this.txtCurrentOcr.Name = "txtCurrentOcr";
            this.txtCurrentOcr.Size = new System.Drawing.Size(100, 23);
            this.txtCurrentOcr.TabIndex = 0;
            // 
            // labelX5
            // 
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.Location = new System.Drawing.Point(26, 151);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(97, 23);
            this.labelX5.TabIndex = 1;
            this.labelX5.Text = "当前ocr";
            // 
            // btnChangeRow
            // 
            this.btnChangeRow.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnChangeRow.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnChangeRow.Location = new System.Drawing.Point(141, 199);
            this.btnChangeRow.Name = "btnChangeRow";
            this.btnChangeRow.Size = new System.Drawing.Size(75, 23);
            this.btnChangeRow.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnChangeRow.TabIndex = 4;
            this.btnChangeRow.Text = "手动换排";
            this.btnChangeRow.Click += new System.EventHandler(this.btnChangeRow_Click);
            // 
            // btnRefreshData
            // 
            this.btnRefreshData.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnRefreshData.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnRefreshData.Location = new System.Drawing.Point(26, 199);
            this.btnRefreshData.Name = "btnRefreshData";
            this.btnRefreshData.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshData.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnRefreshData.TabIndex = 4;
            this.btnRefreshData.Text = "刷新数据";
            this.btnRefreshData.Click += new System.EventHandler(this.btnRefreshData_Click);
            // 
            // txtAlreadyCount
            // 
            // 
            // 
            // 
            this.txtAlreadyCount.Border.Class = "TextBoxBorder";
            this.txtAlreadyCount.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtAlreadyCount.Location = new System.Drawing.Point(116, 107);
            this.txtAlreadyCount.Name = "txtAlreadyCount";
            this.txtAlreadyCount.Size = new System.Drawing.Size(100, 23);
            this.txtAlreadyCount.TabIndex = 0;
            // 
            // labelX4
            // 
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Location = new System.Drawing.Point(26, 107);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(97, 23);
            this.labelX4.TabIndex = 1;
            this.labelX4.Text = "已取数量";
            // 
            // txtGetCount
            // 
            // 
            // 
            // 
            this.txtGetCount.Border.Class = "TextBoxBorder";
            this.txtGetCount.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtGetCount.Location = new System.Drawing.Point(116, 63);
            this.txtGetCount.Name = "txtGetCount";
            this.txtGetCount.Size = new System.Drawing.Size(100, 23);
            this.txtGetCount.TabIndex = 0;
            // 
            // labelX3
            // 
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(26, 63);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(97, 23);
            this.labelX3.TabIndex = 1;
            this.labelX3.Text = "bar条取料总数";
            // 
            // groupPanel4
            // 
            this.groupPanel4.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel4.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel4.Controls.Add(this.txtSetBar);
            this.groupPanel4.Controls.Add(this.labelX6);
            this.groupPanel4.Controls.Add(this.btnMapTest);
            this.groupPanel4.Controls.Add(this.btnUpdate);
            this.groupPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupPanel4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupPanel4.Location = new System.Drawing.Point(703, 297);
            this.groupPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.groupPanel4.Name = "groupPanel4";
            this.groupPanel4.Size = new System.Drawing.Size(297, 365);
            // 
            // 
            // 
            this.groupPanel4.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel4.Style.BackColorGradientAngle = 90;
            this.groupPanel4.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel4.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel4.Style.BorderBottomWidth = 1;
            this.groupPanel4.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel4.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel4.Style.BorderLeftWidth = 1;
            this.groupPanel4.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel4.Style.BorderRightWidth = 1;
            this.groupPanel4.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel4.Style.BorderTopWidth = 1;
            this.groupPanel4.Style.CornerDiameter = 4;
            this.groupPanel4.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel4.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel4.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupPanel4.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel4.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel4.TabIndex = 17;
            this.groupPanel4.Text = "Map数据更新";
            // 
            // txtSetBar
            // 
            // 
            // 
            // 
            this.txtSetBar.Border.Class = "TextBoxBorder";
            this.txtSetBar.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSetBar.Location = new System.Drawing.Point(116, 26);
            this.txtSetBar.Name = "txtSetBar";
            this.txtSetBar.Size = new System.Drawing.Size(100, 23);
            this.txtSetBar.TabIndex = 0;
            // 
            // labelX6
            // 
            // 
            // 
            // 
            this.labelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX6.Location = new System.Drawing.Point(26, 26);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(97, 23);
            this.labelX6.TabIndex = 1;
            this.labelX6.Text = "bar条号设置";
            // 
            // btnMapTest
            // 
            this.btnMapTest.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnMapTest.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnMapTest.Location = new System.Drawing.Point(26, 178);
            this.btnMapTest.Name = "btnMapTest";
            this.btnMapTest.Size = new System.Drawing.Size(75, 23);
            this.btnMapTest.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnMapTest.TabIndex = 4;
            this.btnMapTest.Text = "Map测试";
            this.btnMapTest.Click += new System.EventHandler(this.btnMapTest_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnUpdate.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnUpdate.Location = new System.Drawing.Point(26, 99);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnUpdate.TabIndex = 4;
            this.btnUpdate.Text = "更新";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // panelBlow
            // 
            this.panelBlow.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelBlow.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelBlow.Controls.Add(this.tableLayoutPanel1);
            this.panelBlow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBlow.Location = new System.Drawing.Point(0, 0);
            this.panelBlow.Margin = new System.Windows.Forms.Padding(0);
            this.panelBlow.Name = "panelBlow";
            this.panelBlow.Size = new System.Drawing.Size(1000, 662);
            this.panelBlow.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelBlow.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelBlow.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelBlow.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelBlow.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelBlow.Style.GradientAngle = 90;
            this.panelBlow.TabIndex = 1;
            // 
            // labelX8
            // 
            // 
            // 
            // 
            this.labelX8.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX8.Location = new System.Drawing.Point(26, 19);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(97, 23);
            this.labelX8.TabIndex = 1;
            this.labelX8.Text = "当前产品排";
            // 
            // txtCurrentRow
            // 
            // 
            // 
            // 
            this.txtCurrentRow.Border.Class = "TextBoxBorder";
            this.txtCurrentRow.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtCurrentRow.Location = new System.Drawing.Point(116, 19);
            this.txtCurrentRow.Name = "txtCurrentRow";
            this.txtCurrentRow.Size = new System.Drawing.Size(100, 23);
            this.txtCurrentRow.TabIndex = 0;
            // 
            // MapView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelBlow);
            this.Name = "MapView";
            this.Size = new System.Drawing.Size(1000, 662);
            this.Load += new System.EventHandler(this.MapView_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataMapView)).EndInit();
            this.groupPanel3.ResumeLayout(false);
            this.groupPanel4.ResumeLayout(false);
            this.panelBlow.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel3;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel4;
        private DevComponents.DotNetBar.PanelEx panelBlow;
        private DevComponents.DotNetBar.ButtonX btnDel;
        private DevComponents.DotNetBar.ButtonX btnEnter;
        private DevComponents.DotNetBar.ButtonX btnCheckWafer;
        private _3DViews.CommonView.LoadFileView loadMapPath;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbIndex;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtWaferNo;
        private DevComponents.DotNetBar.Controls.DataGridViewX dataMapView;
        private DevComponents.DotNetBar.Controls.TextBoxX txtCurrentOcr;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.ButtonX btnChangeRow;
        private DevComponents.DotNetBar.ButtonX btnRefreshData;
        private DevComponents.DotNetBar.Controls.TextBoxX txtAlreadyCount;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.Controls.TextBoxX txtGetCount;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSetBar;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.ButtonX btnUpdate;
        private DevComponents.Editors.ComboItem comboItem1;
        private DevComponents.Editors.ComboItem comboItem2;
        private DevComponents.Editors.ComboItem comboItem3;
        private DevComponents.Editors.ComboItem comboItem4;
        private DevComponents.Editors.ComboItem comboItem5;
        private DevComponents.Editors.ComboItem comboItem6;
        private DevComponents.Editors.ComboItem comboItem7;
        private DevComponents.Editors.ComboItem comboItem8;
        private DevComponents.Editors.ComboItem comboItem9;
        private DevComponents.Editors.ComboItem comboItem10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private DevComponents.DotNetBar.ButtonX btnMapTest;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbProduct;
        private DevComponents.Editors.ComboItem comboItem21;
        private DevComponents.Editors.ComboItem comboItem22;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.DotNetBar.Controls.TextBoxX txtCurrentRow;
        private DevComponents.DotNetBar.LabelX labelX8;
    }
}
