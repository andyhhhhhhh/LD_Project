namespace ManagementView.Popup
{
    partial class TestLogView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestLogView));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.richLog = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnNowNote = new CCWin.SkinControl.SkinButton();
            this.btnLoad = new CCWin.SkinControl.SkinButton();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLogPath = new System.Windows.Forms.TextBox();
            this.panelEx2 = new DevComponents.DotNetBar.PanelEx();
            this.btnException = new DevComponents.DotNetBar.ButtonX();
            this.btnError = new DevComponents.DotNetBar.ButtonX();
            this.btnDebug = new DevComponents.DotNetBar.ButtonX();
            this.btnInfo = new DevComponents.DotNetBar.ButtonX();
            this.btnAll = new DevComponents.DotNetBar.ButtonX();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panelEx2.SuspendLayout();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.richLog, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelEx2, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18.05556F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 81.94444F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(884, 664);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // richLog
            // 
            this.richLog.BackColor = System.Drawing.Color.AliceBlue;
            this.richLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richLog.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richLog.Location = new System.Drawing.Point(0, 112);
            this.richLog.Margin = new System.Windows.Forms.Padding(0);
            this.richLog.Name = "richLog";
            this.richLog.Size = new System.Drawing.Size(884, 512);
            this.richLog.TabIndex = 0;
            this.richLog.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.btnNowNote);
            this.groupBox1.Controls.Add(this.btnLoad);
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtLogPath);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(884, 112);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "操作";
            // 
            // btnNowNote
            // 
            this.btnNowNote.BackColor = System.Drawing.Color.Transparent;
            this.btnNowNote.BaseColor = System.Drawing.Color.Transparent;
            this.btnNowNote.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnNowNote.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnNowNote.DownBack = null;
            this.btnNowNote.GlowColor = System.Drawing.Color.Blue;
            this.btnNowNote.Location = new System.Drawing.Point(42, 68);
            this.btnNowNote.MouseBack = null;
            this.btnNowNote.Name = "btnNowNote";
            this.btnNowNote.NormlBack = null;
            this.btnNowNote.Size = new System.Drawing.Size(87, 27);
            this.btnNowNote.TabIndex = 5;
            this.btnNowNote.Text = "当天日志";
            this.btnNowNote.UseVisualStyleBackColor = false;
            this.btnNowNote.Click += new System.EventHandler(this.btnNowNote_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.BackColor = System.Drawing.Color.Transparent;
            this.btnLoad.BaseColor = System.Drawing.Color.Transparent;
            this.btnLoad.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnLoad.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnLoad.DownBack = null;
            this.btnLoad.GlowColor = System.Drawing.Color.Blue;
            this.btnLoad.Location = new System.Drawing.Point(42, 27);
            this.btnLoad.MouseBack = null;
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.NormlBack = null;
            this.btnLoad.Size = new System.Drawing.Size(87, 27);
            this.btnLoad.TabIndex = 5;
            this.btnLoad.Text = "加载文件";
            this.btnLoad.UseVisualStyleBackColor = false;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(505, 70);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(138, 23);
            this.dateTimePicker1.TabIndex = 4;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(437, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "选择日期:";
            // 
            // txtLogPath
            // 
            this.txtLogPath.Location = new System.Drawing.Point(146, 29);
            this.txtLogPath.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtLogPath.Name = "txtLogPath";
            this.txtLogPath.Size = new System.Drawing.Size(497, 23);
            this.txtLogPath.TabIndex = 1;
            // 
            // panelEx2
            // 
            this.panelEx2.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx2.Controls.Add(this.btnException);
            this.panelEx2.Controls.Add(this.btnError);
            this.panelEx2.Controls.Add(this.btnDebug);
            this.panelEx2.Controls.Add(this.btnInfo);
            this.panelEx2.Controls.Add(this.btnAll);
            this.panelEx2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx2.Location = new System.Drawing.Point(0, 624);
            this.panelEx2.Margin = new System.Windows.Forms.Padding(0);
            this.panelEx2.Name = "panelEx2";
            this.panelEx2.Size = new System.Drawing.Size(884, 40);
            this.panelEx2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx2.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx2.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx2.Style.GradientAngle = 90;
            this.panelEx2.TabIndex = 2;
            // 
            // btnException
            // 
            this.btnException.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnException.ColorTable = DevComponents.DotNetBar.eButtonColor.Blue;
            this.btnException.Location = new System.Drawing.Point(771, 5);
            this.btnException.Name = "btnException";
            this.btnException.Size = new System.Drawing.Size(95, 30);
            this.btnException.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnException.TabIndex = 0;
            this.btnException.Text = "异常";
            this.btnException.Click += new System.EventHandler(this.btnException_Click);
            // 
            // btnError
            // 
            this.btnError.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnError.ColorTable = DevComponents.DotNetBar.eButtonColor.Blue;
            this.btnError.Location = new System.Drawing.Point(582, 5);
            this.btnError.Name = "btnError";
            this.btnError.Size = new System.Drawing.Size(95, 30);
            this.btnError.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnError.TabIndex = 0;
            this.btnError.Text = "错误";
            this.btnError.Click += new System.EventHandler(this.btnError_Click);
            // 
            // btnDebug
            // 
            this.btnDebug.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDebug.ColorTable = DevComponents.DotNetBar.eButtonColor.Blue;
            this.btnDebug.Location = new System.Drawing.Point(393, 5);
            this.btnDebug.Name = "btnDebug";
            this.btnDebug.Size = new System.Drawing.Size(95, 30);
            this.btnDebug.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnDebug.TabIndex = 0;
            this.btnDebug.Text = "调试";
            this.btnDebug.Click += new System.EventHandler(this.btnDebug_Click);
            // 
            // btnInfo
            // 
            this.btnInfo.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.btnInfo.ColorTable = DevComponents.DotNetBar.eButtonColor.Blue;
            this.btnInfo.Location = new System.Drawing.Point(204, 5);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(95, 30);
            this.btnInfo.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnInfo.TabIndex = 0;
            this.btnInfo.Text = "信息";
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
            // 
            // btnAll
            // 
            this.btnAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAll.BackColor = System.Drawing.Color.Turquoise;
            this.btnAll.ColorTable = DevComponents.DotNetBar.eButtonColor.Blue;
            this.btnAll.Location = new System.Drawing.Point(15, 5);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(95, 30);
            this.btnAll.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnAll.TabIndex = 0;
            this.btnAll.Text = "全部";
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.tableLayoutPanel1);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(884, 664);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 1;
            this.panelEx1.Text = "panelEx1";
            // 
            // TestLogView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(884, 664);
            this.Controls.Add(this.panelEx1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TestLogView";
            this.ShowInTaskbar = false;
            this.Text = "日志界面";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_Popup2_FormClosing);
            this.Load += new System.EventHandler(this.Frm_Popup2_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panelEx2.ResumeLayout(false);
            this.panelEx1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RichTextBox richLog;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtLogPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private CCWin.SkinControl.SkinButton btnNowNote;
        private CCWin.SkinControl.SkinButton btnLoad;
        private DevComponents.DotNetBar.PanelEx panelEx2;
        private DevComponents.DotNetBar.ButtonX btnException;
        private DevComponents.DotNetBar.ButtonX btnError;
        private DevComponents.DotNetBar.ButtonX btnDebug;
        private DevComponents.DotNetBar.ButtonX btnInfo;
        private DevComponents.DotNetBar.ButtonX btnAll;
    }
}