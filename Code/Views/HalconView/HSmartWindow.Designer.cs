namespace HalconView
{
    partial class HSmartWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HSmartWindow));
            this.panel1 = new System.Windows.Forms.Panel();
            this.hsmartControl = new HalconDotNet.HSmartWindowControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveScreentoolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.addNewObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rectangle1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rectangle2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.circleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ellipseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.circleSectorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ellipseSectorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.yellowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.greenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TestDistance = new System.Windows.Forms.ToolStripMenuItem();
            this.CrossHair = new System.Windows.Forms.ToolStripMenuItem();
            this.setLineStyleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dashedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.continuousToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.dToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jETToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorHotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorHSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorOCEANToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorPINKToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorRAINBOWToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorBONEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colorCOOLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deepLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.showPartViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllObjectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.info = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel_4 = new System.Windows.Forms.Panel();
            this.panel_3 = new System.Windows.Forms.Panel();
            this.panel_2 = new System.Windows.Forms.Panel();
            this.panel_1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.hsmartControl);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(645, 516);
            this.panel1.TabIndex = 2;
            // 
            // hsmartControl
            // 
            this.hsmartControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.hsmartControl.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.hsmartControl.BackColor = System.Drawing.Color.Transparent;
            this.hsmartControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.hsmartControl.ContextMenuStrip = this.contextMenuStrip1;
            this.hsmartControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hsmartControl.ForeColor = System.Drawing.SystemColors.ControlText;
            this.hsmartControl.HDoubleClickToFitContent = true;
            this.hsmartControl.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.hsmartControl.HImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hsmartControl.HKeepAspectRatio = true;
            this.hsmartControl.HMoveContent = true;
            this.hsmartControl.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelForwardZoomsIn;
            this.hsmartControl.Location = new System.Drawing.Point(0, 0);
            this.hsmartControl.Margin = new System.Windows.Forms.Padding(0);
            this.hsmartControl.Name = "hsmartControl";
            this.hsmartControl.Size = new System.Drawing.Size(645, 516);
            this.hsmartControl.TabIndex = 0;
            this.hsmartControl.WindowSize = new System.Drawing.Size(645, 516);
            this.hsmartControl.HMouseMove += new HalconDotNet.HMouseEventHandler(this.hsmartControl_HMouseMove);
            this.hsmartControl.HMouseDown += new HalconDotNet.HMouseEventHandler(this.hsmartControl_HMouseDown);
            this.hsmartControl.HMouseUp += new HalconDotNet.HMouseEventHandler(this.hsmartControl_HMouseUp);
            this.hsmartControl.Load += new System.EventHandler(this.hsmartControl_Load);
            this.hsmartControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.hsmartControl_KeyDown);
            this.hsmartControl.KeyUp += new System.Windows.Forms.KeyEventHandler(this.hsmartControl_KeyUp);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openImageToolStripMenuItem,
            this.SaveScreentoolStripMenuItem1,
            this.saveImageToolStripMenuItem,
            this.toolStripSeparator1,
            this.addNewObjectToolStripMenuItem,
            this.setColorToolStripMenuItem,
            this.TestDistance,
            this.CrossHair,
            this.setLineStyleToolStripMenuItem,
            this.toolStripSeparator3,
            this.dToolStripMenuItem,
            this.colorImageToolStripMenuItem,
            this.deepLineToolStripMenuItem,
            this.toolStripSeparator2,
            this.showPartViewToolStripMenuItem,
            this.clearAllObjectsToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(137, 308);
            // 
            // openImageToolStripMenuItem
            // 
            this.openImageToolStripMenuItem.Name = "openImageToolStripMenuItem";
            this.openImageToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.openImageToolStripMenuItem.Text = "打开图像";
            this.openImageToolStripMenuItem.Click += new System.EventHandler(this.openImageToolStripMenuItem_Click_1);
            // 
            // SaveScreentoolStripMenuItem1
            // 
            this.SaveScreentoolStripMenuItem1.Name = "SaveScreentoolStripMenuItem1";
            this.SaveScreentoolStripMenuItem1.Size = new System.Drawing.Size(136, 22);
            this.SaveScreentoolStripMenuItem1.Text = "保存屏幕";
            this.SaveScreentoolStripMenuItem1.Click += new System.EventHandler(this.SaveScreentoolStripMenuItem1_Click);
            // 
            // saveImageToolStripMenuItem
            // 
            this.saveImageToolStripMenuItem.Name = "saveImageToolStripMenuItem";
            this.saveImageToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.saveImageToolStripMenuItem.Text = "保存图像";
            this.saveImageToolStripMenuItem.Click += new System.EventHandler(this.saveImageToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(133, 6);
            // 
            // addNewObjectToolStripMenuItem
            // 
            this.addNewObjectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rectangle1ToolStripMenuItem,
            this.rectangle2ToolStripMenuItem,
            this.circleToolStripMenuItem,
            this.ellipseToolStripMenuItem,
            this.lineToolStripMenuItem,
            this.circleSectorToolStripMenuItem,
            this.ellipseSectorToolStripMenuItem});
            this.addNewObjectToolStripMenuItem.Name = "addNewObjectToolStripMenuItem";
            this.addNewObjectToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.addNewObjectToolStripMenuItem.Text = "新增图元";
            // 
            // rectangle1ToolStripMenuItem
            // 
            this.rectangle1ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("rectangle1ToolStripMenuItem.Image")));
            this.rectangle1ToolStripMenuItem.Name = "rectangle1ToolStripMenuItem";
            this.rectangle1ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.rectangle1ToolStripMenuItem.Text = "矩形";
            this.rectangle1ToolStripMenuItem.Click += new System.EventHandler(this.rectangle1ToolStripMenuItem_Click);
            // 
            // rectangle2ToolStripMenuItem
            // 
            this.rectangle2ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("rectangle2ToolStripMenuItem.Image")));
            this.rectangle2ToolStripMenuItem.Name = "rectangle2ToolStripMenuItem";
            this.rectangle2ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.rectangle2ToolStripMenuItem.Text = "角度矩形";
            this.rectangle2ToolStripMenuItem.Click += new System.EventHandler(this.rectangle2ToolStripMenuItem_Click);
            // 
            // circleToolStripMenuItem
            // 
            this.circleToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("circleToolStripMenuItem.Image")));
            this.circleToolStripMenuItem.Name = "circleToolStripMenuItem";
            this.circleToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.circleToolStripMenuItem.Text = "圆";
            this.circleToolStripMenuItem.Click += new System.EventHandler(this.circleToolStripMenuItem_Click);
            // 
            // ellipseToolStripMenuItem
            // 
            this.ellipseToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("ellipseToolStripMenuItem.Image")));
            this.ellipseToolStripMenuItem.Name = "ellipseToolStripMenuItem";
            this.ellipseToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.ellipseToolStripMenuItem.Text = "椭圆";
            this.ellipseToolStripMenuItem.Click += new System.EventHandler(this.ellipseToolStripMenuItem_Click);
            // 
            // lineToolStripMenuItem
            // 
            this.lineToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("lineToolStripMenuItem.Image")));
            this.lineToolStripMenuItem.Name = "lineToolStripMenuItem";
            this.lineToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.lineToolStripMenuItem.Text = "直线";
            this.lineToolStripMenuItem.Click += new System.EventHandler(this.lineToolStripMenuItem_Click);
            // 
            // circleSectorToolStripMenuItem
            // 
            this.circleSectorToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("circleSectorToolStripMenuItem.Image")));
            this.circleSectorToolStripMenuItem.Name = "circleSectorToolStripMenuItem";
            this.circleSectorToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.circleSectorToolStripMenuItem.Text = "圆弧";
            this.circleSectorToolStripMenuItem.Click += new System.EventHandler(this.circleSectorToolStripMenuItem_Click);
            // 
            // ellipseSectorToolStripMenuItem
            // 
            this.ellipseSectorToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("ellipseSectorToolStripMenuItem.Image")));
            this.ellipseSectorToolStripMenuItem.Name = "ellipseSectorToolStripMenuItem";
            this.ellipseSectorToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.ellipseSectorToolStripMenuItem.Text = "椭圆弧";
            this.ellipseSectorToolStripMenuItem.Click += new System.EventHandler(this.ellipseSectorToolStripMenuItem_Click);
            // 
            // setColorToolStripMenuItem
            // 
            this.setColorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.redToolStripMenuItem,
            this.yellowToolStripMenuItem,
            this.greenToolStripMenuItem,
            this.blueToolStripMenuItem});
            this.setColorToolStripMenuItem.Name = "setColorToolStripMenuItem";
            this.setColorToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.setColorToolStripMenuItem.Text = "设置颜色";
            this.setColorToolStripMenuItem.Visible = false;
            // 
            // redToolStripMenuItem
            // 
            this.redToolStripMenuItem.Name = "redToolStripMenuItem";
            this.redToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.redToolStripMenuItem.Text = "红色";
            this.redToolStripMenuItem.Click += new System.EventHandler(this.redToolStripMenuItem_Click);
            // 
            // yellowToolStripMenuItem
            // 
            this.yellowToolStripMenuItem.Name = "yellowToolStripMenuItem";
            this.yellowToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.yellowToolStripMenuItem.Text = "黄色";
            this.yellowToolStripMenuItem.Click += new System.EventHandler(this.yellowToolStripMenuItem_Click);
            // 
            // greenToolStripMenuItem
            // 
            this.greenToolStripMenuItem.Name = "greenToolStripMenuItem";
            this.greenToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.greenToolStripMenuItem.Text = "绿色";
            this.greenToolStripMenuItem.Click += new System.EventHandler(this.greenToolStripMenuItem_Click);
            // 
            // blueToolStripMenuItem
            // 
            this.blueToolStripMenuItem.Name = "blueToolStripMenuItem";
            this.blueToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.blueToolStripMenuItem.Text = "蓝色";
            this.blueToolStripMenuItem.Click += new System.EventHandler(this.blueToolStripMenuItem_Click);
            // 
            // TestDistance
            // 
            this.TestDistance.Name = "TestDistance";
            this.TestDistance.Size = new System.Drawing.Size(136, 22);
            this.TestDistance.Text = "两点距离";
            this.TestDistance.Click += new System.EventHandler(this.TestDistance_Click);
            // 
            // CrossHair
            // 
            this.CrossHair.CheckOnClick = true;
            this.CrossHair.Name = "CrossHair";
            this.CrossHair.Size = new System.Drawing.Size(136, 22);
            this.CrossHair.Text = "显示十字线";
            this.CrossHair.CheckedChanged += new System.EventHandler(this.CrossHair_CheckedChanged);
            // 
            // setLineStyleToolStripMenuItem
            // 
            this.setLineStyleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dashedToolStripMenuItem,
            this.continuousToolStripMenuItem});
            this.setLineStyleToolStripMenuItem.Name = "setLineStyleToolStripMenuItem";
            this.setLineStyleToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.setLineStyleToolStripMenuItem.Text = "设置线类型";
            this.setLineStyleToolStripMenuItem.Visible = false;
            // 
            // dashedToolStripMenuItem
            // 
            this.dashedToolStripMenuItem.Name = "dashedToolStripMenuItem";
            this.dashedToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.dashedToolStripMenuItem.Text = "Dashed";
            this.dashedToolStripMenuItem.Click += new System.EventHandler(this.dashedToolStripMenuItem_Click);
            // 
            // continuousToolStripMenuItem
            // 
            this.continuousToolStripMenuItem.Name = "continuousToolStripMenuItem";
            this.continuousToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.continuousToolStripMenuItem.Text = "Continuous";
            this.continuousToolStripMenuItem.Click += new System.EventHandler(this.continuousToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(133, 6);
            // 
            // dToolStripMenuItem
            // 
            this.dToolStripMenuItem.Name = "dToolStripMenuItem";
            this.dToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.dToolStripMenuItem.Text = "3D显示";
            this.dToolStripMenuItem.Click += new System.EventHandler(this.dToolStripMenuItem_Click);
            // 
            // colorImageToolStripMenuItem
            // 
            this.colorImageToolStripMenuItem.CheckOnClick = true;
            this.colorImageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.jETToolStripMenuItem,
            this.colorHotToolStripMenuItem,
            this.colorHSVToolStripMenuItem,
            this.colorOCEANToolStripMenuItem,
            this.colorPINKToolStripMenuItem,
            this.colorRAINBOWToolStripMenuItem,
            this.colorBONEToolStripMenuItem,
            this.colorCOOLToolStripMenuItem});
            this.colorImageToolStripMenuItem.Name = "colorImageToolStripMenuItem";
            this.colorImageToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.colorImageToolStripMenuItem.Text = "彩色显示";
            this.colorImageToolStripMenuItem.CheckedChanged += new System.EventHandler(this.colorImageToolStripMenuItem_CheckedChanged);
            // 
            // jETToolStripMenuItem
            // 
            this.jETToolStripMenuItem.Name = "jETToolStripMenuItem";
            this.jETToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.jETToolStripMenuItem.Text = "颜色_JET";
            this.jETToolStripMenuItem.Click += new System.EventHandler(this.jETToolStripMenuItem_Click);
            // 
            // colorHotToolStripMenuItem
            // 
            this.colorHotToolStripMenuItem.Name = "colorHotToolStripMenuItem";
            this.colorHotToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.colorHotToolStripMenuItem.Text = "颜色_HOT";
            this.colorHotToolStripMenuItem.Click += new System.EventHandler(this.colorHotToolStripMenuItem_Click);
            // 
            // colorHSVToolStripMenuItem
            // 
            this.colorHSVToolStripMenuItem.Name = "colorHSVToolStripMenuItem";
            this.colorHSVToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.colorHSVToolStripMenuItem.Text = "颜色_HSV";
            this.colorHSVToolStripMenuItem.Click += new System.EventHandler(this.colorHSVToolStripMenuItem_Click);
            // 
            // colorOCEANToolStripMenuItem
            // 
            this.colorOCEANToolStripMenuItem.Name = "colorOCEANToolStripMenuItem";
            this.colorOCEANToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.colorOCEANToolStripMenuItem.Text = "颜色_OCEAN";
            this.colorOCEANToolStripMenuItem.Click += new System.EventHandler(this.colorOCEANToolStripMenuItem_Click);
            // 
            // colorPINKToolStripMenuItem
            // 
            this.colorPINKToolStripMenuItem.Name = "colorPINKToolStripMenuItem";
            this.colorPINKToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.colorPINKToolStripMenuItem.Text = "颜色_PINK";
            this.colorPINKToolStripMenuItem.Click += new System.EventHandler(this.colorPINKToolStripMenuItem_Click);
            // 
            // colorRAINBOWToolStripMenuItem
            // 
            this.colorRAINBOWToolStripMenuItem.Name = "colorRAINBOWToolStripMenuItem";
            this.colorRAINBOWToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.colorRAINBOWToolStripMenuItem.Text = "颜色_RAINBOW";
            this.colorRAINBOWToolStripMenuItem.Click += new System.EventHandler(this.colorRAINBOWToolStripMenuItem_Click);
            // 
            // colorBONEToolStripMenuItem
            // 
            this.colorBONEToolStripMenuItem.Name = "colorBONEToolStripMenuItem";
            this.colorBONEToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.colorBONEToolStripMenuItem.Text = "颜色_BONE";
            this.colorBONEToolStripMenuItem.Click += new System.EventHandler(this.colorBONEToolStripMenuItem_Click);
            // 
            // colorCOOLToolStripMenuItem
            // 
            this.colorCOOLToolStripMenuItem.Name = "colorCOOLToolStripMenuItem";
            this.colorCOOLToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.colorCOOLToolStripMenuItem.Text = "颜色_COOL";
            this.colorCOOLToolStripMenuItem.Click += new System.EventHandler(this.colorCOOLToolStripMenuItem_Click);
            // 
            // deepLineToolStripMenuItem
            // 
            this.deepLineToolStripMenuItem.Name = "deepLineToolStripMenuItem";
            this.deepLineToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.deepLineToolStripMenuItem.Text = "深度曲线";
            this.deepLineToolStripMenuItem.Click += new System.EventHandler(this.deepLineToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(133, 6);
            // 
            // showPartViewToolStripMenuItem
            // 
            this.showPartViewToolStripMenuItem.Name = "showPartViewToolStripMenuItem";
            this.showPartViewToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.showPartViewToolStripMenuItem.Text = "局部处理";
            this.showPartViewToolStripMenuItem.Click += new System.EventHandler(this.showPartViewToolStripMenuItem_Click);
            // 
            // clearAllObjectsToolStripMenuItem
            // 
            this.clearAllObjectsToolStripMenuItem.Name = "clearAllObjectsToolStripMenuItem";
            this.clearAllObjectsToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.clearAllObjectsToolStripMenuItem.Text = "清除图元";
            this.clearAllObjectsToolStripMenuItem.Click += new System.EventHandler(this.clearAllObjectsToolStripMenuItem_Click);
            // 
            // info
            // 
            this.info.AutoSize = true;
            this.info.BackColor = System.Drawing.Color.Black;
            this.info.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.info.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.info.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.info.Location = new System.Drawing.Point(3, 520);
            this.info.Margin = new System.Windows.Forms.Padding(0);
            this.info.Name = "info";
            this.info.Size = new System.Drawing.Size(645, 19);
            this.info.TabIndex = 4;
            this.info.Text = "info";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tableLayoutPanel1.Controls.Add(this.info, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel_4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel_3, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel_2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel_1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(651, 542);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // panel_4
            // 
            this.panel_4.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.SetColumnSpan(this.panel_4, 3);
            this.panel_4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_4.Location = new System.Drawing.Point(0, 539);
            this.panel_4.Margin = new System.Windows.Forms.Padding(0);
            this.panel_4.Name = "panel_4";
            this.panel_4.Size = new System.Drawing.Size(651, 3);
            this.panel_4.TabIndex = 6;
            // 
            // panel_3
            // 
            this.panel_3.BackColor = System.Drawing.Color.Transparent;
            this.panel_3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_3.Location = new System.Drawing.Point(648, 3);
            this.panel_3.Margin = new System.Windows.Forms.Padding(0);
            this.panel_3.Name = "panel_3";
            this.tableLayoutPanel1.SetRowSpan(this.panel_3, 2);
            this.panel_3.Size = new System.Drawing.Size(3, 536);
            this.panel_3.TabIndex = 6;
            // 
            // panel_2
            // 
            this.panel_2.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.SetColumnSpan(this.panel_2, 3);
            this.panel_2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_2.Location = new System.Drawing.Point(0, 0);
            this.panel_2.Margin = new System.Windows.Forms.Padding(0);
            this.panel_2.Name = "panel_2";
            this.panel_2.Size = new System.Drawing.Size(651, 3);
            this.panel_2.TabIndex = 7;
            // 
            // panel_1
            // 
            this.panel_1.BackColor = System.Drawing.Color.Transparent;
            this.panel_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_1.Location = new System.Drawing.Point(0, 3);
            this.panel_1.Margin = new System.Windows.Forms.Padding(0);
            this.panel_1.Name = "panel_1";
            this.tableLayoutPanel1.SetRowSpan(this.panel_1, 2);
            this.panel_1.Size = new System.Drawing.Size(3, 536);
            this.panel_1.TabIndex = 8;
            // 
            // HSmartWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "HSmartWindow";
            this.Size = new System.Drawing.Size(651, 542);
            this.Load += new System.EventHandler(this.HSmartWindow_Load);
            this.SizeChanged += new System.EventHandler(this.HSmartWindow_SizeChanged);
            this.panel1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private HalconDotNet.HSmartWindowControl hsmartControl;
        private System.Windows.Forms.Label info;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewObjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rectangle1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rectangle2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem circleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ellipseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setColorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem yellowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem greenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setLineStyleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dashedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem continuousToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearAllObjectsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem colorImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deepLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStripMenuItem CrossHair;
        private System.Windows.Forms.ToolStripMenuItem TestDistance;
        private System.Windows.Forms.Panel panel_4;
        private System.Windows.Forms.Panel panel_3;
        private System.Windows.Forms.Panel panel_2;
        private System.Windows.Forms.Panel panel_1;
        private System.Windows.Forms.ToolStripMenuItem showPartViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem circleSectorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ellipseSectorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveScreentoolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem jETToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem colorHotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem colorHSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem colorOCEANToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem colorPINKToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem colorRAINBOWToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem colorBONEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem colorCOOLToolStripMenuItem;
    }
}
