namespace FormDesignView
{
    partial class RunEditView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RunEditView));
            this.ribbonControl1 = new DevComponents.DotNetBar.RibbonControl();
            this.btnUserManage = new DevComponents.DotNetBar.ButtonItem();
            this.btnDebugView = new DevComponents.DotNetBar.ButtonItem();
            this.btnEditView = new DevComponents.DotNetBar.ButtonItem();
            this.btnStyle = new DevComponents.DotNetBar.ButtonItem();
            this.btnViewStyle1 = new DevComponents.DotNetBar.ButtonItem();
            this.btnViewStyle2 = new DevComponents.DotNetBar.ButtonItem();
            this.btnViewStyle3 = new DevComponents.DotNetBar.ButtonItem();
            this.btnViewStyle4 = new DevComponents.DotNetBar.ButtonItem();
            this.btnViewStyle5 = new DevComponents.DotNetBar.ButtonItem();
            this.btnViewStyle6 = new DevComponents.DotNetBar.ButtonItem();
            this.btnViewStyle7 = new DevComponents.DotNetBar.ButtonItem();
            this.btnViewStyle8 = new DevComponents.DotNetBar.ButtonItem();
            this.btnViewStyle9 = new DevComponents.DotNetBar.ButtonItem();
            this.btnViewStyle10 = new DevComponents.DotNetBar.ButtonItem();
            this.panel1 = new DevComponents.DotNetBar.PanelEx();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblRunStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblLoginUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblRunTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblMemory = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCPU = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCurrentTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.styleManager1 = new DevComponents.DotNetBar.StyleManager(this.components);
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            // 
            // 
            // 
            this.ribbonControl1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ribbonControl1.CaptionVisible = true;
            this.ribbonControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ribbonControl1.KeyTipsFont = new System.Drawing.Font("Tahoma", 7F);
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.ribbonControl1.QuickToolbarItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnUserManage,
            this.btnDebugView,
            this.btnEditView,
            this.btnStyle});
            this.ribbonControl1.Size = new System.Drawing.Size(1008, 26);
            this.ribbonControl1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ribbonControl1.SystemText.MaximizeRibbonText = "&Maximize the Ribbon";
            this.ribbonControl1.SystemText.MinimizeRibbonText = "Mi&nimize the Ribbon";
            this.ribbonControl1.SystemText.QatAddItemText = "&Add to Quick Access Toolbar";
            this.ribbonControl1.SystemText.QatCustomizeMenuLabel = "<b>Customize Quick Access Toolbar</b>";
            this.ribbonControl1.SystemText.QatCustomizeText = "&Customize Quick Access Toolbar...";
            this.ribbonControl1.SystemText.QatDialogAddButton = "&Add >>";
            this.ribbonControl1.SystemText.QatDialogCancelButton = "Cancel";
            this.ribbonControl1.SystemText.QatDialogCaption = "Customize Quick Access Toolbar";
            this.ribbonControl1.SystemText.QatDialogCategoriesLabel = "&Choose commands from:";
            this.ribbonControl1.SystemText.QatDialogOkButton = "OK";
            this.ribbonControl1.SystemText.QatDialogPlacementCheckbox = "&Place Quick Access Toolbar below the Ribbon";
            this.ribbonControl1.SystemText.QatDialogRemoveButton = "&Remove";
            this.ribbonControl1.SystemText.QatPlaceAboveRibbonText = "&Place Quick Access Toolbar above the Ribbon";
            this.ribbonControl1.SystemText.QatPlaceBelowRibbonText = "&Place Quick Access Toolbar below the Ribbon";
            this.ribbonControl1.SystemText.QatRemoveItemText = "&Remove from Quick Access Toolbar";
            this.ribbonControl1.TabGroupHeight = 14;
            this.ribbonControl1.TabIndex = 5;
            this.ribbonControl1.Text = "ribbonControl1";
            // 
            // btnUserManage
            // 
            this.btnUserManage.Name = "btnUserManage";
            this.btnUserManage.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlQ);
            this.btnUserManage.Text = "用户管理";
            this.btnUserManage.Click += new System.EventHandler(this.btnUserManage_Click);
            // 
            // btnDebugView
            // 
            this.btnDebugView.Name = "btnDebugView";
            this.btnDebugView.Text = "参数界面";
            this.btnDebugView.Click += new System.EventHandler(this.btnDebugView_Click);
            // 
            // btnEditView
            // 
            this.btnEditView.Name = "btnEditView";
            this.btnEditView.Text = "编辑界面";
            this.btnEditView.Click += new System.EventHandler(this.btnEditView_Click);
            // 
            // btnStyle
            // 
            this.btnStyle.Name = "btnStyle";
            this.btnStyle.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnViewStyle1,
            this.btnViewStyle2,
            this.btnViewStyle3,
            this.btnViewStyle4,
            this.btnViewStyle5,
            this.btnViewStyle6,
            this.btnViewStyle7,
            this.btnViewStyle8,
            this.btnViewStyle9,
            this.btnViewStyle10});
            this.btnStyle.Text = "样式";
            this.btnStyle.ExpandChange += new System.EventHandler(this.btnStyle_ExpandChange);
            // 
            // btnViewStyle1
            // 
            this.btnViewStyle1.Name = "btnViewStyle1";
            this.btnViewStyle1.Text = "Office2007Blue";
            this.btnViewStyle1.Click += new System.EventHandler(this.buttonItem1_Click);
            // 
            // btnViewStyle2
            // 
            this.btnViewStyle2.Name = "btnViewStyle2";
            this.btnViewStyle2.Text = "Office2007Silver";
            this.btnViewStyle2.Click += new System.EventHandler(this.buttonItem1_Click);
            // 
            // btnViewStyle3
            // 
            this.btnViewStyle3.Name = "btnViewStyle3";
            this.btnViewStyle3.Text = "Office2007Black";
            this.btnViewStyle3.Click += new System.EventHandler(this.buttonItem1_Click);
            // 
            // btnViewStyle4
            // 
            this.btnViewStyle4.Name = "btnViewStyle4";
            this.btnViewStyle4.Text = "Office2007VistaGlass";
            this.btnViewStyle4.Click += new System.EventHandler(this.buttonItem1_Click);
            // 
            // btnViewStyle5
            // 
            this.btnViewStyle5.Name = "btnViewStyle5";
            this.btnViewStyle5.Text = "Office2010Silver";
            this.btnViewStyle5.Click += new System.EventHandler(this.buttonItem1_Click);
            // 
            // btnViewStyle6
            // 
            this.btnViewStyle6.Name = "btnViewStyle6";
            this.btnViewStyle6.Text = "Office2010Blue";
            this.btnViewStyle6.Click += new System.EventHandler(this.buttonItem1_Click);
            // 
            // btnViewStyle7
            // 
            this.btnViewStyle7.Name = "btnViewStyle7";
            this.btnViewStyle7.Text = "Office2010Black";
            this.btnViewStyle7.Click += new System.EventHandler(this.buttonItem1_Click);
            // 
            // btnViewStyle8
            // 
            this.btnViewStyle8.Name = "btnViewStyle8";
            this.btnViewStyle8.Text = "Windows7Blue";
            this.btnViewStyle8.Click += new System.EventHandler(this.buttonItem1_Click);
            // 
            // btnViewStyle9
            // 
            this.btnViewStyle9.Name = "btnViewStyle9";
            this.btnViewStyle9.Text = "VisualStudio2010Blue";
            this.btnViewStyle9.Click += new System.EventHandler(this.buttonItem1_Click);
            // 
            // btnViewStyle10
            // 
            this.btnViewStyle10.Name = "btnViewStyle10";
            this.btnViewStyle10.Text = "Metro";
            this.btnViewStyle10.Click += new System.EventHandler(this.buttonItem1_Click);
            // 
            // panel1
            // 
            this.panel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panel1.Controls.Add(this.statusStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 704);
            this.panel1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panel1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panel1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panel1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panel1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panel1.Style.GradientAngle = 90;
            this.panel1.TabIndex = 31;
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.Transparent;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.lblRunStatus,
            this.lblLoginUser,
            this.lblRunTime,
            this.lblMemory,
            this.lblCPU,
            this.lblCurrentTime});
            this.statusStrip1.Location = new System.Drawing.Point(0, 675);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1008, 29);
            this.statusStrip1.TabIndex = 31;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.lblStatus.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblStatus.Margin = new System.Windows.Forms.Padding(0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(591, 29);
            this.lblStatus.Spring = true;
            this.lblStatus.Text = "当前工作空间:";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRunStatus
            // 
            this.lblRunStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblRunStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblRunStatus.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRunStatus.Name = "lblRunStatus";
            this.lblRunStatus.Size = new System.Drawing.Size(69, 24);
            this.lblRunStatus.Text = "运行状态";
            // 
            // lblLoginUser
            // 
            this.lblLoginUser.BackColor = System.Drawing.Color.Transparent;
            this.lblLoginUser.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblLoginUser.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblLoginUser.Name = "lblLoginUser";
            this.lblLoginUser.Size = new System.Drawing.Size(72, 24);
            this.lblLoginUser.Text = "登录身份:";
            // 
            // lblRunTime
            // 
            this.lblRunTime.BackColor = System.Drawing.Color.Transparent;
            this.lblRunTime.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblRunTime.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRunTime.Name = "lblRunTime";
            this.lblRunTime.Size = new System.Drawing.Size(72, 24);
            this.lblRunTime.Text = "运行时间:";
            // 
            // lblMemory
            // 
            this.lblMemory.BackColor = System.Drawing.Color.Transparent;
            this.lblMemory.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblMemory.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMemory.Name = "lblMemory";
            this.lblMemory.Size = new System.Drawing.Size(72, 24);
            this.lblMemory.Text = "内存占用:";
            // 
            // lblCPU
            // 
            this.lblCPU.BackColor = System.Drawing.Color.Transparent;
            this.lblCPU.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblCPU.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCPU.Name = "lblCPU";
            this.lblCPU.Size = new System.Drawing.Size(48, 24);
            this.lblCPU.Text = "CPU: ";
            // 
            // lblCurrentTime
            // 
            this.lblCurrentTime.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrentTime.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblCurrentTime.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCurrentTime.Name = "lblCurrentTime";
            this.lblCurrentTime.Size = new System.Drawing.Size(69, 24);
            this.lblCurrentTime.Text = "北京时间";
            // 
            // styleManager1
            // 
            this.styleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.Office2007Blue;
            this.styleManager1.MetroColorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(163)))), ((int)(((byte)(26))))));
            // 
            // RunEditView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ribbonControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "RunEditView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RunEditView";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RunEditView_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.RibbonControl ribbonControl1;
        private DevComponents.DotNetBar.ButtonItem btnStyle;
        private DevComponents.DotNetBar.ButtonItem btnViewStyle1;
        private DevComponents.DotNetBar.ButtonItem btnViewStyle2;
        private DevComponents.DotNetBar.ButtonItem btnViewStyle3;
        private DevComponents.DotNetBar.ButtonItem btnViewStyle4;
        private DevComponents.DotNetBar.ButtonItem btnViewStyle5;
        private DevComponents.DotNetBar.ButtonItem btnViewStyle6;
        private DevComponents.DotNetBar.ButtonItem btnViewStyle7;
        private DevComponents.DotNetBar.ButtonItem btnViewStyle8;
        private DevComponents.DotNetBar.ButtonItem btnViewStyle9;
        private DevComponents.DotNetBar.ButtonItem btnViewStyle10;
        private DevComponents.DotNetBar.PanelEx panel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblRunStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblLoginUser;
        private System.Windows.Forms.ToolStripStatusLabel lblRunTime;
        private System.Windows.Forms.ToolStripStatusLabel lblMemory;
        private System.Windows.Forms.ToolStripStatusLabel lblCPU;
        private System.Windows.Forms.ToolStripStatusLabel lblCurrentTime;
        private DevComponents.DotNetBar.StyleManager styleManager1;
        private DevComponents.DotNetBar.ButtonItem btnUserManage;
        private DevComponents.DotNetBar.ButtonItem btnEditView;
        private DevComponents.DotNetBar.ButtonItem btnDebugView;
    }
}