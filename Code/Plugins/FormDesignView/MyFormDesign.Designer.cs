namespace MyFormDesinger
{
    partial class MyFormDesign
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MyFormDesign));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.cmbSelect = new System.Windows.Forms.ToolStripComboBox();
            this.toolEButton = new System.Windows.Forms.ToolStripButton();
            this.toolEButtonPro = new System.Windows.Forms.ToolStripButton();
            this.toolEDataOutput = new System.Windows.Forms.ToolStripButton();
            this.toolEHWindow = new System.Windows.Forms.ToolStripButton();
            this.toolELblResult = new System.Windows.Forms.ToolStripButton();
            this.toolELblStatus = new System.Windows.Forms.ToolStripButton();
            this.toolELog = new System.Windows.Forms.ToolStripButton();
            this.toolETextBox = new System.Windows.Forms.ToolStripButton();
            this.toolESetText = new System.Windows.Forms.ToolStripButton();
            this.toolEProduct = new System.Windows.Forms.ToolStripButton();
            this.toolEItemResult = new System.Windows.Forms.ToolStripButton();
            this.toolECheck = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolELight = new System.Windows.Forms.ToolStripButton();
            this.toolGroup = new System.Windows.Forms.ToolStripButton();
            this.ECombo = new System.Windows.Forms.ToolStripButton();
            this.toolAdd = new System.Windows.Forms.ToolStripButton();
            this.toolDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolLeft = new System.Windows.Forms.ToolStripButton();
            this.toolRight = new System.Windows.Forms.ToolStripButton();
            this.toolTop = new System.Windows.Forms.ToolStripButton();
            this.toolBottom = new System.Windows.Forms.ToolStripButton();
            this.toolVertically = new System.Windows.Forms.ToolStripButton();
            this.toolHorizontally = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolWidthEqual = new System.Windows.Forms.ToolStripButton();
            this.toolHeightEqual = new System.Windows.Forms.ToolStripButton();
            this.toolSameSize = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolSaveConfig = new System.Windows.Forms.ToolStripButton();
            this.toolLoadConfig = new System.Windows.Forms.ToolStripButton();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.skinSplitContainer1 = new CCWin.SkinControl.SkinSplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.designerControl1 = new MyFormDesinger.DesignerControl();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.skinSplitContainer1)).BeginInit();
            this.skinSplitContainer1.Panel1.SuspendLayout();
            this.skinSplitContainer1.Panel2.SuspendLayout();
            this.skinSplitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.AllowDrop = true;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmbSelect,
            this.toolEButton,
            this.toolEButtonPro,
            this.toolEDataOutput,
            this.toolEHWindow,
            this.toolELblResult,
            this.toolELblStatus,
            this.toolELog,
            this.toolETextBox,
            this.toolESetText,
            this.toolEProduct,
            this.toolEItemResult,
            this.toolECheck,
            this.toolStripButton1,
            this.toolELight,
            this.toolGroup,
            this.ECombo,
            this.toolAdd,
            this.toolDelete,
            this.toolStripSeparator1,
            this.toolLeft,
            this.toolRight,
            this.toolTop,
            this.toolBottom,
            this.toolVertically,
            this.toolHorizontally,
            this.toolStripSeparator3,
            this.toolWidthEqual,
            this.toolHeightEqual,
            this.toolSameSize,
            this.toolStripSeparator2,
            this.toolSaveConfig,
            this.toolLoadConfig});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1350, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // cmbSelect
            // 
            this.cmbSelect.DropDownWidth = 75;
            this.cmbSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbSelect.Items.AddRange(new object[] {
            "主界面",
            "调试界面"});
            this.cmbSelect.Margin = new System.Windows.Forms.Padding(0);
            this.cmbSelect.Name = "cmbSelect";
            this.cmbSelect.Size = new System.Drawing.Size(75, 25);
            this.cmbSelect.Text = "主界面";
            this.cmbSelect.ToolTipText = "编辑界面选择";
            this.cmbSelect.SelectedIndexChanged += new System.EventHandler(this.cmbSelect_SelectedIndexChanged);
            // 
            // toolEButton
            // 
            this.toolEButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolEButton.Name = "toolEButton";
            this.toolEButton.Size = new System.Drawing.Size(57, 22);
            this.toolEButton.Text = "EButton";
            this.toolEButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolStrip_MouseDown);
            // 
            // toolEButtonPro
            // 
            this.toolEButtonPro.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolEButtonPro.Image = ((System.Drawing.Image)(resources.GetObject("toolEButtonPro.Image")));
            this.toolEButtonPro.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolEButtonPro.Name = "toolEButtonPro";
            this.toolEButtonPro.Size = new System.Drawing.Size(77, 22);
            this.toolEButtonPro.Text = "EButtonPro";
            this.toolEButtonPro.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolStrip_MouseDown);
            // 
            // toolEDataOutput
            // 
            this.toolEDataOutput.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolEDataOutput.Image = ((System.Drawing.Image)(resources.GetObject("toolEDataOutput.Image")));
            this.toolEDataOutput.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolEDataOutput.Name = "toolEDataOutput";
            this.toolEDataOutput.Size = new System.Drawing.Size(46, 22);
            this.toolEDataOutput.Text = "EData";
            this.toolEDataOutput.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolStrip_MouseDown);
            // 
            // toolEHWindow
            // 
            this.toolEHWindow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolEHWindow.Image = ((System.Drawing.Image)(resources.GetObject("toolEHWindow.Image")));
            this.toolEHWindow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolEHWindow.Name = "toolEHWindow";
            this.toolEHWindow.Size = new System.Drawing.Size(75, 22);
            this.toolEHWindow.Text = "EHWindow";
            this.toolEHWindow.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolStrip_MouseDown);
            // 
            // toolELblResult
            // 
            this.toolELblResult.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolELblResult.Image = ((System.Drawing.Image)(resources.GetObject("toolELblResult.Image")));
            this.toolELblResult.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolELblResult.Name = "toolELblResult";
            this.toolELblResult.Size = new System.Drawing.Size(71, 22);
            this.toolELblResult.Text = "ELblResult";
            this.toolELblResult.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolStrip_MouseDown);
            // 
            // toolELblStatus
            // 
            this.toolELblStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolELblStatus.Image = ((System.Drawing.Image)(resources.GetObject("toolELblStatus.Image")));
            this.toolELblStatus.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolELblStatus.Name = "toolELblStatus";
            this.toolELblStatus.Size = new System.Drawing.Size(71, 22);
            this.toolELblStatus.Text = "ELblStatus";
            this.toolELblStatus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolStrip_MouseDown);
            // 
            // toolELog
            // 
            this.toolELog.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolELog.Image = ((System.Drawing.Image)(resources.GetObject("toolELog.Image")));
            this.toolELog.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolELog.Name = "toolELog";
            this.toolELog.Size = new System.Drawing.Size(41, 22);
            this.toolELog.Text = "ELog";
            this.toolELog.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolStrip_MouseDown);
            // 
            // toolETextBox
            // 
            this.toolETextBox.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolETextBox.Image = ((System.Drawing.Image)(resources.GetObject("toolETextBox.Image")));
            this.toolETextBox.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolETextBox.Name = "toolETextBox";
            this.toolETextBox.Size = new System.Drawing.Size(65, 22);
            this.toolETextBox.Text = "ETextBox";
            this.toolETextBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolStrip_MouseDown);
            // 
            // toolESetText
            // 
            this.toolESetText.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolESetText.Image = ((System.Drawing.Image)(resources.GetObject("toolESetText.Image")));
            this.toolESetText.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolESetText.Name = "toolESetText";
            this.toolESetText.Size = new System.Drawing.Size(61, 22);
            this.toolESetText.Text = "ESetText";
            this.toolESetText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolStrip_MouseDown);
            // 
            // toolEProduct
            // 
            this.toolEProduct.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolEProduct.Image = ((System.Drawing.Image)(resources.GetObject("toolEProduct.Image")));
            this.toolEProduct.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolEProduct.Name = "toolEProduct";
            this.toolEProduct.Size = new System.Drawing.Size(64, 22);
            this.toolEProduct.Text = "EProduct";
            this.toolEProduct.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolStrip_MouseDown);
            // 
            // toolEItemResult
            // 
            this.toolEItemResult.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolEItemResult.Image = ((System.Drawing.Image)(resources.GetObject("toolEItemResult.Image")));
            this.toolEItemResult.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolEItemResult.Name = "toolEItemResult";
            this.toolEItemResult.Size = new System.Drawing.Size(80, 22);
            this.toolEItemResult.Text = "EItemResult";
            this.toolEItemResult.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolStrip_MouseDown);
            // 
            // toolECheck
            // 
            this.toolECheck.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolECheck.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolECheck.Name = "toolECheck";
            this.toolECheck.Size = new System.Drawing.Size(54, 22);
            this.toolECheck.Text = "ECheck";
            this.toolECheck.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolStrip_MouseDown);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(49, 22);
            this.toolStripButton1.Text = "EError";
            this.toolStripButton1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolStrip_MouseDown);
            // 
            // toolELight
            // 
            this.toolELight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolELight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolELight.Name = "toolELight";
            this.toolELight.Size = new System.Drawing.Size(47, 22);
            this.toolELight.Text = "ELight";
            this.toolELight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolStrip_MouseDown);
            // 
            // toolGroup
            // 
            this.toolGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolGroup.Name = "toolGroup";
            this.toolGroup.Size = new System.Drawing.Size(56, 22);
            this.toolGroup.Text = "EGroup";
            this.toolGroup.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolStrip_MouseDown);
            // 
            // ECombo
            // 
            this.ECombo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ECombo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ECombo.Name = "ECombo";
            this.ECombo.Size = new System.Drawing.Size(62, 22);
            this.ECombo.Text = "ECombo";
            this.ECombo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolStrip_MouseDown);
            // 
            // toolAdd
            // 
            this.toolAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolAdd.Name = "toolAdd";
            this.toolAdd.Size = new System.Drawing.Size(36, 22);
            this.toolAdd.Text = "添加";
            this.toolAdd.Visible = false;
            this.toolAdd.Click += new System.EventHandler(this.toolAdd_Click);
            // 
            // toolDelete
            // 
            this.toolDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolDelete.Image = ((System.Drawing.Image)(resources.GetObject("toolDelete.Image")));
            this.toolDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolDelete.Name = "toolDelete";
            this.toolDelete.Size = new System.Drawing.Size(36, 22);
            this.toolDelete.Text = "删除";
            this.toolDelete.Click += new System.EventHandler(this.toolDelete_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolLeft
            // 
            this.toolLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolLeft.Image = ((System.Drawing.Image)(resources.GetObject("toolLeft.Image")));
            this.toolLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolLeft.Name = "toolLeft";
            this.toolLeft.Size = new System.Drawing.Size(23, 22);
            this.toolLeft.Text = "toolStripButton1";
            this.toolLeft.ToolTipText = "左对齐";
            this.toolLeft.Click += new System.EventHandler(this.toolLeft_Click);
            // 
            // toolRight
            // 
            this.toolRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolRight.Image = ((System.Drawing.Image)(resources.GetObject("toolRight.Image")));
            this.toolRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolRight.Name = "toolRight";
            this.toolRight.Size = new System.Drawing.Size(23, 22);
            this.toolRight.Text = "toolStripButton2";
            this.toolRight.ToolTipText = "右对齐";
            this.toolRight.Click += new System.EventHandler(this.toolRight_Click);
            // 
            // toolTop
            // 
            this.toolTop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolTop.Image = global::FormDesignView.Properties.Resources.block_align_top_1;
            this.toolTop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolTop.Name = "toolTop";
            this.toolTop.Size = new System.Drawing.Size(23, 22);
            this.toolTop.Text = "toolStripButton2";
            this.toolTop.ToolTipText = "上对齐";
            this.toolTop.Click += new System.EventHandler(this.toolTop_Click);
            // 
            // toolBottom
            // 
            this.toolBottom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBottom.Image = ((System.Drawing.Image)(resources.GetObject("toolBottom.Image")));
            this.toolBottom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBottom.Name = "toolBottom";
            this.toolBottom.Size = new System.Drawing.Size(23, 22);
            this.toolBottom.Text = "toolStripButton2";
            this.toolBottom.ToolTipText = "下对齐";
            this.toolBottom.Click += new System.EventHandler(this.toolBottom_Click);
            // 
            // toolVertically
            // 
            this.toolVertically.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolVertically.Image = ((System.Drawing.Image)(resources.GetObject("toolVertically.Image")));
            this.toolVertically.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolVertically.Name = "toolVertically";
            this.toolVertically.Size = new System.Drawing.Size(23, 22);
            this.toolVertically.Text = "toolStripButton3";
            this.toolVertically.ToolTipText = "使水平间距相等";
            this.toolVertically.Click += new System.EventHandler(this.toolVertically_Click);
            // 
            // toolHorizontally
            // 
            this.toolHorizontally.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolHorizontally.Image = ((System.Drawing.Image)(resources.GetObject("toolHorizontally.Image")));
            this.toolHorizontally.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolHorizontally.Name = "toolHorizontally";
            this.toolHorizontally.Size = new System.Drawing.Size(23, 22);
            this.toolHorizontally.Text = "toolStripButton4";
            this.toolHorizontally.ToolTipText = "使垂直间距相等";
            this.toolHorizontally.Click += new System.EventHandler(this.toolHorizontally_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolWidthEqual
            // 
            this.toolWidthEqual.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolWidthEqual.Image = ((System.Drawing.Image)(resources.GetObject("toolWidthEqual.Image")));
            this.toolWidthEqual.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolWidthEqual.Name = "toolWidthEqual";
            this.toolWidthEqual.Size = new System.Drawing.Size(23, 22);
            this.toolWidthEqual.Text = "toolStripButton4";
            this.toolWidthEqual.ToolTipText = "使宽度方向相等";
            this.toolWidthEqual.Click += new System.EventHandler(this.toolWidthEqual_Click);
            // 
            // toolHeightEqual
            // 
            this.toolHeightEqual.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolHeightEqual.Image = ((System.Drawing.Image)(resources.GetObject("toolHeightEqual.Image")));
            this.toolHeightEqual.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolHeightEqual.Name = "toolHeightEqual";
            this.toolHeightEqual.Size = new System.Drawing.Size(23, 22);
            this.toolHeightEqual.Text = "toolStripButton4";
            this.toolHeightEqual.ToolTipText = "使高度方向相等";
            this.toolHeightEqual.Click += new System.EventHandler(this.toolHeightEqual_Click);
            // 
            // toolSameSize
            // 
            this.toolSameSize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolSameSize.Image = ((System.Drawing.Image)(resources.GetObject("toolSameSize.Image")));
            this.toolSameSize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSameSize.Name = "toolSameSize";
            this.toolSameSize.Size = new System.Drawing.Size(23, 22);
            this.toolSameSize.Text = "toolStripButton5";
            this.toolSameSize.ToolTipText = "使大小相同";
            this.toolSameSize.Click += new System.EventHandler(this.toolSameSize_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolSaveConfig
            // 
            this.toolSaveConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolSaveConfig.Image = ((System.Drawing.Image)(resources.GetObject("toolSaveConfig.Image")));
            this.toolSaveConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSaveConfig.Name = "toolSaveConfig";
            this.toolSaveConfig.Size = new System.Drawing.Size(36, 21);
            this.toolSaveConfig.Text = "保存";
            this.toolSaveConfig.Click += new System.EventHandler(this.toolSaveConfig_Click);
            // 
            // toolLoadConfig
            // 
            this.toolLoadConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolLoadConfig.Image = ((System.Drawing.Image)(resources.GetObject("toolLoadConfig.Image")));
            this.toolLoadConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolLoadConfig.Name = "toolLoadConfig";
            this.toolLoadConfig.Size = new System.Drawing.Size(36, 21);
            this.toolLoadConfig.Text = "加载";
            this.toolLoadConfig.Click += new System.EventHandler(this.toolLoadConfig_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(215, 705);
            this.propertyGrid1.TabIndex = 2;
            this.propertyGrid1.SelectedObjectsChanged += new System.EventHandler(this.propertyGrid1_SelectedObjectsChanged);
            // 
            // skinSplitContainer1
            // 
            this.skinSplitContainer1.CollapsePanel = CCWin.SkinControl.CollapsePanel.Panel2;
            this.skinSplitContainer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.skinSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinSplitContainer1.Location = new System.Drawing.Point(0, 25);
            this.skinSplitContainer1.Name = "skinSplitContainer1";
            // 
            // skinSplitContainer1.Panel1
            // 
            this.skinSplitContainer1.Panel1.Controls.Add(this.panel1);
            this.skinSplitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // skinSplitContainer1.Panel2
            // 
            this.skinSplitContainer1.Panel2.Controls.Add(this.propertyGrid1);
            this.skinSplitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.skinSplitContainer1.Size = new System.Drawing.Size(1350, 705);
            this.skinSplitContainer1.SplitterDistance = 1131;
            this.skinSplitContainer1.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.designerControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1131, 705);
            this.panel1.TabIndex = 4;
            // 
            // designerControl1
            // 
            this.designerControl1.AutoScroll = true;
            this.designerControl1.BackColor = System.Drawing.Color.White;
            this.designerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.designerControl1.Location = new System.Drawing.Point(0, 0);
            this.designerControl1.Name = "designerControl1";
            this.designerControl1.Size = new System.Drawing.Size(1131, 705);
            this.designerControl1.TabIndex = 3;
            // 
            // MyFormDesign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1350, 730);
            this.Controls.Add(this.skinSplitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MyFormDesign";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "界面设计";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MyFormDesign_FormClosing);
            this.Load += new System.EventHandler(this.MyFormDesigner_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.skinSplitContainer1.Panel1.ResumeLayout(false);
            this.skinSplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.skinSplitContainer1)).EndInit();
            this.skinSplitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolAdd;
        private DesignerControl designerControl1;
        public System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.ToolStripButton toolEButton;
        private System.Windows.Forms.ToolStripButton toolEButtonPro;
        private System.Windows.Forms.ToolStripButton toolEDataOutput;
        private System.Windows.Forms.ToolStripButton toolEHWindow;
        private System.Windows.Forms.ToolStripButton toolELblResult;
        private System.Windows.Forms.ToolStripButton toolELblStatus;
        private System.Windows.Forms.ToolStripButton toolELog;
        private System.Windows.Forms.ToolStripButton toolETextBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolSaveConfig;
        private System.Windows.Forms.ToolStripButton toolLoadConfig;
        private System.Windows.Forms.ToolStripButton toolDelete;
        private System.Windows.Forms.ToolStripButton toolESetText;
        private System.Windows.Forms.ToolStripButton toolEProduct;
        private System.Windows.Forms.ToolStripButton toolEItemResult;
        private CCWin.SkinControl.SkinSplitContainer skinSplitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripButton toolELight;
        private System.Windows.Forms.ToolStripButton toolLeft;
        private System.Windows.Forms.ToolStripButton toolTop;
        private System.Windows.Forms.ToolStripButton toolVertically;
        private System.Windows.Forms.ToolStripButton toolHorizontally;
        private System.Windows.Forms.ToolStripButton toolSameSize;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolRight;
        private System.Windows.Forms.ToolStripButton toolBottom;
        private System.Windows.Forms.ToolStripButton toolECheck;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolWidthEqual;
        private System.Windows.Forms.ToolStripButton toolHeightEqual;
        private System.Windows.Forms.ToolStripComboBox cmbSelect;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolGroup;
        private System.Windows.Forms.ToolStripButton ECombo;
    }
}

