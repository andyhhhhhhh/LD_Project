namespace ManagementView._3DViews
{
    partial class SetView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetView));
            this.btnCancel = new CCWin.SkinControl.SkinButton();
            this.btnConfirm = new CCWin.SkinControl.SkinButton();
            this.btnLoadPath = new CCWin.SkinControl.SkinButton();
            this.txtMaxCapaity = new CCWin.SkinControl.SkinTextBox();
            this.txtFileDay = new CCWin.SkinControl.SkinTextBox();
            this.skinLabel5 = new CCWin.SkinControl.SkinLabel();
            this.txtPath = new CCWin.SkinControl.SkinTextBox();
            this.skinLabel4 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel2 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel3 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel1 = new CCWin.SkinControl.SkinLabel();
            this.chkMinToPallet = new CCWin.SkinControl.SkinCheckBox();
            this.chkOpenMany = new CCWin.SkinControl.SkinCheckBox();
            this.chkOpenAutoRun = new CCWin.SkinControl.SkinCheckBox();
            this.chkIsSaveImg = new CCWin.SkinControl.SkinCheckBox();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.panel_OtherSet = new DevComponents.DotNetBar.PanelEx();
            this.skinGroupBox1 = new CCWin.SkinControl.SkinGroupBox();
            this.radioVirtual = new CCWin.SkinControl.SkinRadioButton();
            this.radioSoftPLC = new CCWin.SkinControl.SkinRadioButton();
            this.radioZMotion = new CCWin.SkinControl.SkinRadioButton();
            this.chkRunOtherSoft = new CCWin.SkinControl.SkinCheckBox();
            this.txtSoftAddr = new CCWin.SkinControl.SkinTextBox();
            this.lblSoftAddr = new CCWin.SkinControl.SkinLabel();
            this.btnLoadAddr = new CCWin.SkinControl.SkinButton();
            this.chkIsFullScreen = new CCWin.SkinControl.SkinCheckBox();
            this.chkIsStopShowMsg = new CCWin.SkinControl.SkinCheckBox();
            this.panel_LogSet = new DevComponents.DotNetBar.PanelEx();
            this.btnLoadImagePath = new CCWin.SkinControl.SkinButton();
            this.txtImagePath = new CCWin.SkinControl.SkinTextBox();
            this.btnDelImage = new CCWin.SkinControl.SkinButton();
            this.chkIsSaveNGImage = new CCWin.SkinControl.SkinCheckBox();
            this.chkIsPrintLog = new CCWin.SkinControl.SkinCheckBox();
            this.panel_BaseSet = new DevComponents.DotNetBar.PanelEx();
            this.cmbMainProcess = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cmbResetProcess = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.btnSelectSeqence = new CCWin.SkinControl.SkinButton();
            this.txtViewName = new CCWin.SkinControl.SkinTextBox();
            this.skinLabel8 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel7 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel6 = new CCWin.SkinControl.SkinLabel();
            this.chkIsInitDevice = new CCWin.SkinControl.SkinCheckBox();
            this.chkEnableOsk = new CCWin.SkinControl.SkinCheckBox();
            this.chkEnableRunView = new CCWin.SkinControl.SkinCheckBox();
            this.chkIsProVisible = new CCWin.SkinControl.SkinCheckBox();
            this.chkIsRealDisplay = new CCWin.SkinControl.SkinCheckBox();
            this.chkIsItemVisible = new CCWin.SkinControl.SkinCheckBox();
            this.chkIsShowCross = new CCWin.SkinControl.SkinCheckBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.panelEx1.SuspendLayout();
            this.panel_OtherSet.SuspendLayout();
            this.skinGroupBox1.SuspendLayout();
            this.panel_LogSet.SuspendLayout();
            this.panel_BaseSet.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BaseColor = System.Drawing.Color.Transparent;
            this.btnCancel.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnCancel.DownBack = null;
            this.btnCancel.GlowColor = System.Drawing.Color.Blue;
            this.btnCancel.Location = new System.Drawing.Point(512, 400);
            this.btnCancel.MouseBack = null;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.NormlBack = null;
            this.btnCancel.Size = new System.Drawing.Size(94, 39);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.BaseColor = System.Drawing.Color.Transparent;
            this.btnConfirm.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnConfirm.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnConfirm.DownBack = null;
            this.btnConfirm.GlowColor = System.Drawing.Color.Blue;
            this.btnConfirm.Location = new System.Drawing.Point(238, 400);
            this.btnConfirm.MouseBack = null;
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.NormlBack = null;
            this.btnConfirm.Size = new System.Drawing.Size(94, 39);
            this.btnConfirm.TabIndex = 3;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnLoadPath
            // 
            this.btnLoadPath.BackColor = System.Drawing.Color.Transparent;
            this.btnLoadPath.BaseColor = System.Drawing.Color.Transparent;
            this.btnLoadPath.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnLoadPath.DownBack = null;
            this.btnLoadPath.GlowColor = System.Drawing.Color.Blue;
            this.btnLoadPath.Location = new System.Drawing.Point(428, 32);
            this.btnLoadPath.MouseBack = null;
            this.btnLoadPath.Name = "btnLoadPath";
            this.btnLoadPath.NormlBack = null;
            this.btnLoadPath.Size = new System.Drawing.Size(83, 28);
            this.btnLoadPath.TabIndex = 3;
            this.btnLoadPath.Text = "加载路径";
            this.btnLoadPath.UseVisualStyleBackColor = false;
            this.btnLoadPath.Click += new System.EventHandler(this.btnLoadPath_Click);
            // 
            // txtMaxCapaity
            // 
            this.txtMaxCapaity.BackColor = System.Drawing.Color.Transparent;
            this.txtMaxCapaity.DownBack = null;
            this.txtMaxCapaity.Icon = null;
            this.txtMaxCapaity.IconIsButton = false;
            this.txtMaxCapaity.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtMaxCapaity.IsPasswordChat = '\0';
            this.txtMaxCapaity.IsSystemPasswordChar = false;
            this.txtMaxCapaity.Lines = new string[0];
            this.txtMaxCapaity.Location = new System.Drawing.Point(68, 111);
            this.txtMaxCapaity.Margin = new System.Windows.Forms.Padding(0);
            this.txtMaxCapaity.MaxLength = 32767;
            this.txtMaxCapaity.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtMaxCapaity.MouseBack = null;
            this.txtMaxCapaity.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtMaxCapaity.Multiline = false;
            this.txtMaxCapaity.Name = "txtMaxCapaity";
            this.txtMaxCapaity.NormlBack = null;
            this.txtMaxCapaity.Padding = new System.Windows.Forms.Padding(5);
            this.txtMaxCapaity.ReadOnly = false;
            this.txtMaxCapaity.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtMaxCapaity.Size = new System.Drawing.Size(95, 28);
            // 
            // 
            // 
            this.txtMaxCapaity.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMaxCapaity.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMaxCapaity.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtMaxCapaity.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtMaxCapaity.SkinTxt.Name = "BaseText";
            this.txtMaxCapaity.SkinTxt.Size = new System.Drawing.Size(85, 16);
            this.txtMaxCapaity.SkinTxt.TabIndex = 0;
            this.txtMaxCapaity.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtMaxCapaity.SkinTxt.WaterText = "允许容量";
            this.txtMaxCapaity.TabIndex = 2;
            this.txtMaxCapaity.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtMaxCapaity.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtMaxCapaity.WaterText = "允许容量";
            this.txtMaxCapaity.WordWrap = true;
            // 
            // txtFileDay
            // 
            this.txtFileDay.BackColor = System.Drawing.Color.Transparent;
            this.txtFileDay.DownBack = null;
            this.txtFileDay.Icon = null;
            this.txtFileDay.IconIsButton = false;
            this.txtFileDay.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtFileDay.IsPasswordChat = '\0';
            this.txtFileDay.IsSystemPasswordChar = false;
            this.txtFileDay.Lines = new string[0];
            this.txtFileDay.Location = new System.Drawing.Point(68, 73);
            this.txtFileDay.Margin = new System.Windows.Forms.Padding(0);
            this.txtFileDay.MaxLength = 32767;
            this.txtFileDay.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtFileDay.MouseBack = null;
            this.txtFileDay.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtFileDay.Multiline = false;
            this.txtFileDay.Name = "txtFileDay";
            this.txtFileDay.NormlBack = null;
            this.txtFileDay.Padding = new System.Windows.Forms.Padding(5);
            this.txtFileDay.ReadOnly = false;
            this.txtFileDay.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtFileDay.Size = new System.Drawing.Size(95, 28);
            // 
            // 
            // 
            this.txtFileDay.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFileDay.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFileDay.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtFileDay.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtFileDay.SkinTxt.Name = "BaseText";
            this.txtFileDay.SkinTxt.Size = new System.Drawing.Size(85, 16);
            this.txtFileDay.SkinTxt.TabIndex = 0;
            this.txtFileDay.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtFileDay.SkinTxt.WaterText = "保存天数";
            this.txtFileDay.TabIndex = 2;
            this.txtFileDay.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtFileDay.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtFileDay.WaterText = "保存天数";
            this.txtFileDay.WordWrap = true;
            // 
            // skinLabel5
            // 
            this.skinLabel5.AutoSize = true;
            this.skinLabel5.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel5.BorderColor = System.Drawing.Color.White;
            this.skinLabel5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel5.Location = new System.Drawing.Point(6, 117);
            this.skinLabel5.Name = "skinLabel5";
            this.skinLabel5.Size = new System.Drawing.Size(59, 17);
            this.skinLabel5.TabIndex = 1;
            this.skinLabel5.Text = "允许大小:";
            // 
            // txtPath
            // 
            this.txtPath.BackColor = System.Drawing.Color.Transparent;
            this.txtPath.DownBack = null;
            this.txtPath.Enabled = false;
            this.txtPath.Icon = null;
            this.txtPath.IconIsButton = false;
            this.txtPath.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtPath.IsPasswordChat = '\0';
            this.txtPath.IsSystemPasswordChar = false;
            this.txtPath.Lines = new string[0];
            this.txtPath.Location = new System.Drawing.Point(6, 32);
            this.txtPath.Margin = new System.Windows.Forms.Padding(0);
            this.txtPath.MaxLength = 32767;
            this.txtPath.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtPath.MouseBack = null;
            this.txtPath.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtPath.Multiline = false;
            this.txtPath.Name = "txtPath";
            this.txtPath.NormlBack = null;
            this.txtPath.Padding = new System.Windows.Forms.Padding(5);
            this.txtPath.ReadOnly = false;
            this.txtPath.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtPath.Size = new System.Drawing.Size(419, 28);
            // 
            // 
            // 
            this.txtPath.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPath.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPath.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPath.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtPath.SkinTxt.Name = "BaseText";
            this.txtPath.SkinTxt.Size = new System.Drawing.Size(409, 16);
            this.txtPath.SkinTxt.TabIndex = 0;
            this.txtPath.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtPath.SkinTxt.WaterText = "Log保存路径";
            this.txtPath.TabIndex = 2;
            this.txtPath.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtPath.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtPath.WaterText = "Log保存路径";
            this.txtPath.WordWrap = true;
            // 
            // skinLabel4
            // 
            this.skinLabel4.AutoSize = true;
            this.skinLabel4.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel4.BorderColor = System.Drawing.Color.White;
            this.skinLabel4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel4.Location = new System.Drawing.Point(166, 117);
            this.skinLabel4.Name = "skinLabel4";
            this.skinLabel4.Size = new System.Drawing.Size(20, 17);
            this.skinLabel4.TabIndex = 1;
            this.skinLabel4.Text = "M";
            // 
            // skinLabel2
            // 
            this.skinLabel2.AutoSize = true;
            this.skinLabel2.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel2.BorderColor = System.Drawing.Color.White;
            this.skinLabel2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel2.Location = new System.Drawing.Point(6, 79);
            this.skinLabel2.Name = "skinLabel2";
            this.skinLabel2.Size = new System.Drawing.Size(59, 17);
            this.skinLabel2.TabIndex = 1;
            this.skinLabel2.Text = "保存时间:";
            // 
            // skinLabel3
            // 
            this.skinLabel3.AutoSize = true;
            this.skinLabel3.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel3.BorderColor = System.Drawing.Color.White;
            this.skinLabel3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel3.Location = new System.Drawing.Point(166, 79);
            this.skinLabel3.Name = "skinLabel3";
            this.skinLabel3.Size = new System.Drawing.Size(20, 17);
            this.skinLabel3.TabIndex = 1;
            this.skinLabel3.Text = "天";
            // 
            // skinLabel1
            // 
            this.skinLabel1.AutoSize = true;
            this.skinLabel1.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel1.BorderColor = System.Drawing.Color.White;
            this.skinLabel1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel1.Location = new System.Drawing.Point(6, 7);
            this.skinLabel1.Name = "skinLabel1";
            this.skinLabel1.Size = new System.Drawing.Size(83, 17);
            this.skinLabel1.TabIndex = 1;
            this.skinLabel1.Text = "日志保存路径:";
            // 
            // chkMinToPallet
            // 
            this.chkMinToPallet.AutoSize = true;
            this.chkMinToPallet.BackColor = System.Drawing.Color.Transparent;
            this.chkMinToPallet.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.chkMinToPallet.DownBack = null;
            this.chkMinToPallet.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkMinToPallet.Location = new System.Drawing.Point(7, 199);
            this.chkMinToPallet.MouseBack = null;
            this.chkMinToPallet.Name = "chkMinToPallet";
            this.chkMinToPallet.NormlBack = null;
            this.chkMinToPallet.SelectedDownBack = null;
            this.chkMinToPallet.SelectedMouseBack = null;
            this.chkMinToPallet.SelectedNormlBack = null;
            this.chkMinToPallet.Size = new System.Drawing.Size(123, 21);
            this.chkMinToPallet.TabIndex = 0;
            this.chkMinToPallet.Text = "最小化隐藏到托盘";
            this.chkMinToPallet.UseVisualStyleBackColor = false;
            // 
            // chkOpenMany
            // 
            this.chkOpenMany.AutoSize = true;
            this.chkOpenMany.BackColor = System.Drawing.Color.Transparent;
            this.chkOpenMany.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.chkOpenMany.DownBack = null;
            this.chkOpenMany.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkOpenMany.Location = new System.Drawing.Point(5, 96);
            this.chkOpenMany.MouseBack = null;
            this.chkOpenMany.Name = "chkOpenMany";
            this.chkOpenMany.NormlBack = null;
            this.chkOpenMany.SelectedDownBack = null;
            this.chkOpenMany.SelectedMouseBack = null;
            this.chkOpenMany.SelectedNormlBack = null;
            this.chkOpenMany.Size = new System.Drawing.Size(123, 21);
            this.chkOpenMany.TabIndex = 0;
            this.chkOpenMany.Text = "允许打开多个程序";
            this.chkOpenMany.UseVisualStyleBackColor = false;
            // 
            // chkOpenAutoRun
            // 
            this.chkOpenAutoRun.AutoSize = true;
            this.chkOpenAutoRun.BackColor = System.Drawing.Color.Transparent;
            this.chkOpenAutoRun.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.chkOpenAutoRun.DownBack = null;
            this.chkOpenAutoRun.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkOpenAutoRun.Location = new System.Drawing.Point(7, 133);
            this.chkOpenAutoRun.MouseBack = null;
            this.chkOpenAutoRun.Name = "chkOpenAutoRun";
            this.chkOpenAutoRun.NormlBack = null;
            this.chkOpenAutoRun.SelectedDownBack = null;
            this.chkOpenAutoRun.SelectedMouseBack = null;
            this.chkOpenAutoRun.SelectedNormlBack = null;
            this.chkOpenAutoRun.Size = new System.Drawing.Size(99, 21);
            this.chkOpenAutoRun.TabIndex = 0;
            this.chkOpenAutoRun.Text = "开机自动启动";
            this.chkOpenAutoRun.UseVisualStyleBackColor = false;
            // 
            // chkIsSaveImg
            // 
            this.chkIsSaveImg.AutoSize = true;
            this.chkIsSaveImg.BackColor = System.Drawing.Color.Transparent;
            this.chkIsSaveImg.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.chkIsSaveImg.DownBack = null;
            this.chkIsSaveImg.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkIsSaveImg.Location = new System.Drawing.Point(6, 225);
            this.chkIsSaveImg.MouseBack = null;
            this.chkIsSaveImg.Name = "chkIsSaveImg";
            this.chkIsSaveImg.NormlBack = null;
            this.chkIsSaveImg.SelectedDownBack = null;
            this.chkIsSaveImg.SelectedMouseBack = null;
            this.chkIsSaveImg.SelectedNormlBack = null;
            this.chkIsSaveImg.Size = new System.Drawing.Size(99, 21);
            this.chkIsSaveImg.TabIndex = 0;
            this.chkIsSaveImg.Text = "自动保存图像";
            this.chkIsSaveImg.UseVisualStyleBackColor = false;
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.panel_OtherSet);
            this.panelEx1.Controls.Add(this.panel_LogSet);
            this.panelEx1.Controls.Add(this.panel_BaseSet);
            this.panelEx1.Controls.Add(this.listBox1);
            this.panelEx1.Controls.Add(this.btnCancel);
            this.panelEx1.Controls.Add(this.btnConfirm);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(703, 444);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 1;
            // 
            // panel_OtherSet
            // 
            this.panel_OtherSet.CanvasColor = System.Drawing.SystemColors.Control;
            this.panel_OtherSet.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panel_OtherSet.Controls.Add(this.skinGroupBox1);
            this.panel_OtherSet.Controls.Add(this.chkRunOtherSoft);
            this.panel_OtherSet.Controls.Add(this.txtSoftAddr);
            this.panel_OtherSet.Controls.Add(this.lblSoftAddr);
            this.panel_OtherSet.Controls.Add(this.btnLoadAddr);
            this.panel_OtherSet.Controls.Add(this.chkIsFullScreen);
            this.panel_OtherSet.Controls.Add(this.chkIsStopShowMsg);
            this.panel_OtherSet.Controls.Add(this.chkOpenMany);
            this.panel_OtherSet.Location = new System.Drawing.Point(159, 75);
            this.panel_OtherSet.Name = "panel_OtherSet";
            this.panel_OtherSet.Size = new System.Drawing.Size(532, 51);
            this.panel_OtherSet.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panel_OtherSet.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panel_OtherSet.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panel_OtherSet.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panel_OtherSet.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panel_OtherSet.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panel_OtherSet.Style.GradientAngle = 90;
            this.panel_OtherSet.TabIndex = 7;
            // 
            // skinGroupBox1
            // 
            this.skinGroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.skinGroupBox1.BorderColor = System.Drawing.Color.White;
            this.skinGroupBox1.Controls.Add(this.radioVirtual);
            this.skinGroupBox1.Controls.Add(this.radioSoftPLC);
            this.skinGroupBox1.Controls.Add(this.radioZMotion);
            this.skinGroupBox1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinGroupBox1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.skinGroupBox1.Location = new System.Drawing.Point(9, 201);
            this.skinGroupBox1.Name = "skinGroupBox1";
            this.skinGroupBox1.RectBackColor = System.Drawing.Color.Transparent;
            this.skinGroupBox1.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinGroupBox1.Size = new System.Drawing.Size(502, 65);
            this.skinGroupBox1.TabIndex = 4;
            this.skinGroupBox1.TabStop = false;
            this.skinGroupBox1.Text = "控制卡选择";
            this.skinGroupBox1.TitleBorderColor = System.Drawing.Color.White;
            this.skinGroupBox1.TitleRectBackColor = System.Drawing.Color.LightGray;
            this.skinGroupBox1.TitleRoundStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // radioVirtual
            // 
            this.radioVirtual.AutoSize = true;
            this.radioVirtual.BackColor = System.Drawing.Color.Transparent;
            this.radioVirtual.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.radioVirtual.DownBack = null;
            this.radioVirtual.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioVirtual.Location = new System.Drawing.Point(232, 32);
            this.radioVirtual.MouseBack = null;
            this.radioVirtual.Name = "radioVirtual";
            this.radioVirtual.NormlBack = null;
            this.radioVirtual.SelectedDownBack = null;
            this.radioVirtual.SelectedMouseBack = null;
            this.radioVirtual.SelectedNormlBack = null;
            this.radioVirtual.Size = new System.Drawing.Size(62, 21);
            this.radioVirtual.TabIndex = 0;
            this.radioVirtual.TabStop = true;
            this.radioVirtual.Text = "虚拟卡";
            this.radioVirtual.UseVisualStyleBackColor = false;
            this.radioVirtual.CheckedChanged += new System.EventHandler(this.radioVirtual_CheckedChanged);
            // 
            // radioSoftPLC
            // 
            this.radioSoftPLC.AutoSize = true;
            this.radioSoftPLC.BackColor = System.Drawing.Color.Transparent;
            this.radioSoftPLC.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.radioSoftPLC.DownBack = null;
            this.radioSoftPLC.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioSoftPLC.Location = new System.Drawing.Point(127, 32);
            this.radioSoftPLC.MouseBack = null;
            this.radioSoftPLC.Name = "radioSoftPLC";
            this.radioSoftPLC.NormlBack = null;
            this.radioSoftPLC.SelectedDownBack = null;
            this.radioSoftPLC.SelectedMouseBack = null;
            this.radioSoftPLC.SelectedNormlBack = null;
            this.radioSoftPLC.Size = new System.Drawing.Size(59, 21);
            this.radioSoftPLC.TabIndex = 0;
            this.radioSoftPLC.TabStop = true;
            this.radioSoftPLC.Text = "软PLC";
            this.radioSoftPLC.UseVisualStyleBackColor = false;
            this.radioSoftPLC.CheckedChanged += new System.EventHandler(this.radioSoftPLC_CheckedChanged);
            // 
            // radioZMotion
            // 
            this.radioZMotion.AutoSize = true;
            this.radioZMotion.BackColor = System.Drawing.Color.Transparent;
            this.radioZMotion.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.radioZMotion.DownBack = null;
            this.radioZMotion.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioZMotion.Location = new System.Drawing.Point(22, 32);
            this.radioZMotion.MouseBack = null;
            this.radioZMotion.Name = "radioZMotion";
            this.radioZMotion.NormlBack = null;
            this.radioZMotion.SelectedDownBack = null;
            this.radioZMotion.SelectedMouseBack = null;
            this.radioZMotion.SelectedNormlBack = null;
            this.radioZMotion.Size = new System.Drawing.Size(74, 21);
            this.radioZMotion.TabIndex = 0;
            this.radioZMotion.TabStop = true;
            this.radioZMotion.Text = "正运动卡";
            this.radioZMotion.UseVisualStyleBackColor = false;
            this.radioZMotion.CheckedChanged += new System.EventHandler(this.radioZMotion_CheckedChanged);
            // 
            // chkRunOtherSoft
            // 
            this.chkRunOtherSoft.AutoSize = true;
            this.chkRunOtherSoft.BackColor = System.Drawing.Color.Transparent;
            this.chkRunOtherSoft.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.chkRunOtherSoft.DownBack = null;
            this.chkRunOtherSoft.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkRunOtherSoft.Location = new System.Drawing.Point(5, 67);
            this.chkRunOtherSoft.MouseBack = null;
            this.chkRunOtherSoft.Name = "chkRunOtherSoft";
            this.chkRunOtherSoft.NormlBack = null;
            this.chkRunOtherSoft.SelectedDownBack = null;
            this.chkRunOtherSoft.SelectedMouseBack = null;
            this.chkRunOtherSoft.SelectedNormlBack = null;
            this.chkRunOtherSoft.Size = new System.Drawing.Size(198, 21);
            this.chkRunOtherSoft.TabIndex = 0;
            this.chkRunOtherSoft.Text = "启动软件时,同时打开第三方软件";
            this.chkRunOtherSoft.UseVisualStyleBackColor = false;
            this.chkRunOtherSoft.CheckedChanged += new System.EventHandler(this.chkRunOtherSoft_CheckedChanged);
            // 
            // txtSoftAddr
            // 
            this.txtSoftAddr.BackColor = System.Drawing.Color.Transparent;
            this.txtSoftAddr.DownBack = null;
            this.txtSoftAddr.Enabled = false;
            this.txtSoftAddr.Icon = null;
            this.txtSoftAddr.IconIsButton = false;
            this.txtSoftAddr.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtSoftAddr.IsPasswordChat = '\0';
            this.txtSoftAddr.IsSystemPasswordChar = false;
            this.txtSoftAddr.Lines = new string[0];
            this.txtSoftAddr.Location = new System.Drawing.Point(5, 29);
            this.txtSoftAddr.Margin = new System.Windows.Forms.Padding(0);
            this.txtSoftAddr.MaxLength = 32767;
            this.txtSoftAddr.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtSoftAddr.MouseBack = null;
            this.txtSoftAddr.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtSoftAddr.Multiline = false;
            this.txtSoftAddr.Name = "txtSoftAddr";
            this.txtSoftAddr.NormlBack = null;
            this.txtSoftAddr.Padding = new System.Windows.Forms.Padding(5);
            this.txtSoftAddr.ReadOnly = false;
            this.txtSoftAddr.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtSoftAddr.Size = new System.Drawing.Size(419, 28);
            // 
            // 
            // 
            this.txtSoftAddr.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSoftAddr.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSoftAddr.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSoftAddr.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtSoftAddr.SkinTxt.Name = "BaseText";
            this.txtSoftAddr.SkinTxt.Size = new System.Drawing.Size(409, 16);
            this.txtSoftAddr.SkinTxt.TabIndex = 0;
            this.txtSoftAddr.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtSoftAddr.SkinTxt.WaterText = "第三方软件目录";
            this.txtSoftAddr.TabIndex = 2;
            this.txtSoftAddr.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtSoftAddr.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtSoftAddr.WaterText = "第三方软件目录";
            this.txtSoftAddr.WordWrap = true;
            // 
            // lblSoftAddr
            // 
            this.lblSoftAddr.AutoSize = true;
            this.lblSoftAddr.BackColor = System.Drawing.Color.Transparent;
            this.lblSoftAddr.BorderColor = System.Drawing.Color.White;
            this.lblSoftAddr.Enabled = false;
            this.lblSoftAddr.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSoftAddr.Location = new System.Drawing.Point(5, 7);
            this.lblSoftAddr.Name = "lblSoftAddr";
            this.lblSoftAddr.Size = new System.Drawing.Size(59, 17);
            this.lblSoftAddr.TabIndex = 1;
            this.lblSoftAddr.Text = "软件路径:";
            // 
            // btnLoadAddr
            // 
            this.btnLoadAddr.BackColor = System.Drawing.Color.Transparent;
            this.btnLoadAddr.BaseColor = System.Drawing.Color.Transparent;
            this.btnLoadAddr.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnLoadAddr.DownBack = null;
            this.btnLoadAddr.Enabled = false;
            this.btnLoadAddr.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLoadAddr.GlowColor = System.Drawing.Color.Blue;
            this.btnLoadAddr.Location = new System.Drawing.Point(428, 29);
            this.btnLoadAddr.MouseBack = null;
            this.btnLoadAddr.Name = "btnLoadAddr";
            this.btnLoadAddr.NormlBack = null;
            this.btnLoadAddr.Size = new System.Drawing.Size(83, 28);
            this.btnLoadAddr.TabIndex = 3;
            this.btnLoadAddr.Text = "加载路径";
            this.btnLoadAddr.UseVisualStyleBackColor = false;
            this.btnLoadAddr.Click += new System.EventHandler(this.btnLoadAddr_Click);
            // 
            // chkIsFullScreen
            // 
            this.chkIsFullScreen.AutoSize = true;
            this.chkIsFullScreen.BackColor = System.Drawing.Color.Transparent;
            this.chkIsFullScreen.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.chkIsFullScreen.DownBack = null;
            this.chkIsFullScreen.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkIsFullScreen.Location = new System.Drawing.Point(5, 154);
            this.chkIsFullScreen.MouseBack = null;
            this.chkIsFullScreen.Name = "chkIsFullScreen";
            this.chkIsFullScreen.NormlBack = null;
            this.chkIsFullScreen.SelectedDownBack = null;
            this.chkIsFullScreen.SelectedMouseBack = null;
            this.chkIsFullScreen.SelectedNormlBack = null;
            this.chkIsFullScreen.Size = new System.Drawing.Size(123, 21);
            this.chkIsFullScreen.TabIndex = 0;
            this.chkIsFullScreen.Text = "软件界面全屏显示";
            this.chkIsFullScreen.UseVisualStyleBackColor = false;
            // 
            // chkIsStopShowMsg
            // 
            this.chkIsStopShowMsg.AutoSize = true;
            this.chkIsStopShowMsg.BackColor = System.Drawing.Color.Transparent;
            this.chkIsStopShowMsg.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.chkIsStopShowMsg.DownBack = null;
            this.chkIsStopShowMsg.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkIsStopShowMsg.Location = new System.Drawing.Point(5, 125);
            this.chkIsStopShowMsg.MouseBack = null;
            this.chkIsStopShowMsg.Name = "chkIsStopShowMsg";
            this.chkIsStopShowMsg.NormlBack = null;
            this.chkIsStopShowMsg.SelectedDownBack = null;
            this.chkIsStopShowMsg.SelectedMouseBack = null;
            this.chkIsStopShowMsg.SelectedNormlBack = null;
            this.chkIsStopShowMsg.Size = new System.Drawing.Size(135, 21);
            this.chkIsStopShowMsg.TabIndex = 0;
            this.chkIsStopShowMsg.Text = "点击停止按钮先提示";
            this.chkIsStopShowMsg.UseVisualStyleBackColor = false;
            // 
            // panel_LogSet
            // 
            this.panel_LogSet.CanvasColor = System.Drawing.SystemColors.Control;
            this.panel_LogSet.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panel_LogSet.Controls.Add(this.btnLoadImagePath);
            this.panel_LogSet.Controls.Add(this.btnLoadPath);
            this.panel_LogSet.Controls.Add(this.txtImagePath);
            this.panel_LogSet.Controls.Add(this.txtPath);
            this.panel_LogSet.Controls.Add(this.skinLabel1);
            this.panel_LogSet.Controls.Add(this.btnDelImage);
            this.panel_LogSet.Controls.Add(this.skinLabel2);
            this.panel_LogSet.Controls.Add(this.chkIsSaveNGImage);
            this.panel_LogSet.Controls.Add(this.chkIsSaveImg);
            this.panel_LogSet.Controls.Add(this.skinLabel4);
            this.panel_LogSet.Controls.Add(this.skinLabel3);
            this.panel_LogSet.Controls.Add(this.skinLabel5);
            this.panel_LogSet.Controls.Add(this.chkIsPrintLog);
            this.panel_LogSet.Controls.Add(this.txtMaxCapaity);
            this.panel_LogSet.Controls.Add(this.txtFileDay);
            this.panel_LogSet.Location = new System.Drawing.Point(159, 12);
            this.panel_LogSet.Name = "panel_LogSet";
            this.panel_LogSet.Size = new System.Drawing.Size(532, 40);
            this.panel_LogSet.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panel_LogSet.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panel_LogSet.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panel_LogSet.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panel_LogSet.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panel_LogSet.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panel_LogSet.Style.GradientAngle = 90;
            this.panel_LogSet.TabIndex = 6;
            // 
            // btnLoadImagePath
            // 
            this.btnLoadImagePath.BackColor = System.Drawing.Color.Transparent;
            this.btnLoadImagePath.BaseColor = System.Drawing.Color.Transparent;
            this.btnLoadImagePath.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnLoadImagePath.DownBack = null;
            this.btnLoadImagePath.GlowColor = System.Drawing.Color.Blue;
            this.btnLoadImagePath.Location = new System.Drawing.Point(428, 186);
            this.btnLoadImagePath.MouseBack = null;
            this.btnLoadImagePath.Name = "btnLoadImagePath";
            this.btnLoadImagePath.NormlBack = null;
            this.btnLoadImagePath.Size = new System.Drawing.Size(83, 28);
            this.btnLoadImagePath.TabIndex = 3;
            this.btnLoadImagePath.Text = "加载路径";
            this.btnLoadImagePath.UseVisualStyleBackColor = false;
            this.btnLoadImagePath.Click += new System.EventHandler(this.btnLoadImagePath_Click);
            // 
            // txtImagePath
            // 
            this.txtImagePath.BackColor = System.Drawing.Color.Transparent;
            this.txtImagePath.DownBack = null;
            this.txtImagePath.Enabled = false;
            this.txtImagePath.Icon = null;
            this.txtImagePath.IconIsButton = false;
            this.txtImagePath.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtImagePath.IsPasswordChat = '\0';
            this.txtImagePath.IsSystemPasswordChar = false;
            this.txtImagePath.Lines = new string[0];
            this.txtImagePath.Location = new System.Drawing.Point(6, 186);
            this.txtImagePath.Margin = new System.Windows.Forms.Padding(0);
            this.txtImagePath.MaxLength = 32767;
            this.txtImagePath.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtImagePath.MouseBack = null;
            this.txtImagePath.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtImagePath.Multiline = false;
            this.txtImagePath.Name = "txtImagePath";
            this.txtImagePath.NormlBack = null;
            this.txtImagePath.Padding = new System.Windows.Forms.Padding(5);
            this.txtImagePath.ReadOnly = false;
            this.txtImagePath.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtImagePath.Size = new System.Drawing.Size(419, 28);
            // 
            // 
            // 
            this.txtImagePath.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtImagePath.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtImagePath.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtImagePath.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtImagePath.SkinTxt.Name = "BaseText";
            this.txtImagePath.SkinTxt.Size = new System.Drawing.Size(409, 16);
            this.txtImagePath.SkinTxt.TabIndex = 0;
            this.txtImagePath.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtImagePath.SkinTxt.WaterText = "图片保存路径";
            this.txtImagePath.TabIndex = 2;
            this.txtImagePath.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtImagePath.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtImagePath.WaterText = "图片保存路径";
            this.txtImagePath.WordWrap = true;
            // 
            // btnDelImage
            // 
            this.btnDelImage.BackColor = System.Drawing.Color.Transparent;
            this.btnDelImage.BaseColor = System.Drawing.Color.Transparent;
            this.btnDelImage.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnDelImage.DownBack = null;
            this.btnDelImage.Location = new System.Drawing.Point(192, 79);
            this.btnDelImage.MouseBack = null;
            this.btnDelImage.Name = "btnDelImage";
            this.btnDelImage.NormlBack = null;
            this.btnDelImage.Size = new System.Drawing.Size(55, 54);
            this.btnDelImage.TabIndex = 3;
            this.btnDelImage.Text = "删除";
            this.btnDelImage.UseVisualStyleBackColor = false;
            this.btnDelImage.Click += new System.EventHandler(this.btnDelImage_Click);
            // 
            // chkIsSaveNGImage
            // 
            this.chkIsSaveNGImage.AutoSize = true;
            this.chkIsSaveNGImage.BackColor = System.Drawing.Color.Transparent;
            this.chkIsSaveNGImage.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.chkIsSaveNGImage.DownBack = null;
            this.chkIsSaveNGImage.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkIsSaveNGImage.Location = new System.Drawing.Point(6, 254);
            this.chkIsSaveNGImage.MouseBack = null;
            this.chkIsSaveNGImage.Name = "chkIsSaveNGImage";
            this.chkIsSaveNGImage.NormlBack = null;
            this.chkIsSaveNGImage.SelectedDownBack = null;
            this.chkIsSaveNGImage.SelectedMouseBack = null;
            this.chkIsSaveNGImage.SelectedNormlBack = null;
            this.chkIsSaveNGImage.Size = new System.Drawing.Size(106, 21);
            this.chkIsSaveNGImage.TabIndex = 0;
            this.chkIsSaveNGImage.Text = "只保存NG图像";
            this.chkIsSaveNGImage.UseVisualStyleBackColor = false;
            // 
            // chkIsPrintLog
            // 
            this.chkIsPrintLog.AutoSize = true;
            this.chkIsPrintLog.BackColor = System.Drawing.Color.Transparent;
            this.chkIsPrintLog.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.chkIsPrintLog.DownBack = null;
            this.chkIsPrintLog.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkIsPrintLog.Location = new System.Drawing.Point(6, 152);
            this.chkIsPrintLog.MouseBack = null;
            this.chkIsPrintLog.Name = "chkIsPrintLog";
            this.chkIsPrintLog.NormlBack = null;
            this.chkIsPrintLog.SelectedDownBack = null;
            this.chkIsPrintLog.SelectedMouseBack = null;
            this.chkIsPrintLog.SelectedNormlBack = null;
            this.chkIsPrintLog.Size = new System.Drawing.Size(133, 21);
            this.chkIsPrintLog.TabIndex = 0;
            this.chkIsPrintLog.Text = "是否只打印重要Log";
            this.chkIsPrintLog.UseVisualStyleBackColor = false;
            // 
            // panel_BaseSet
            // 
            this.panel_BaseSet.CanvasColor = System.Drawing.SystemColors.Control;
            this.panel_BaseSet.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panel_BaseSet.Controls.Add(this.cmbMainProcess);
            this.panel_BaseSet.Controls.Add(this.cmbResetProcess);
            this.panel_BaseSet.Controls.Add(this.btnSelectSeqence);
            this.panel_BaseSet.Controls.Add(this.txtViewName);
            this.panel_BaseSet.Controls.Add(this.skinLabel8);
            this.panel_BaseSet.Controls.Add(this.skinLabel7);
            this.panel_BaseSet.Controls.Add(this.skinLabel6);
            this.panel_BaseSet.Controls.Add(this.chkIsInitDevice);
            this.panel_BaseSet.Controls.Add(this.chkEnableOsk);
            this.panel_BaseSet.Controls.Add(this.chkOpenAutoRun);
            this.panel_BaseSet.Controls.Add(this.chkEnableRunView);
            this.panel_BaseSet.Controls.Add(this.chkMinToPallet);
            this.panel_BaseSet.Controls.Add(this.chkIsProVisible);
            this.panel_BaseSet.Controls.Add(this.chkIsRealDisplay);
            this.panel_BaseSet.Controls.Add(this.chkIsItemVisible);
            this.panel_BaseSet.Controls.Add(this.chkIsShowCross);
            this.panel_BaseSet.Location = new System.Drawing.Point(159, 308);
            this.panel_BaseSet.Name = "panel_BaseSet";
            this.panel_BaseSet.Size = new System.Drawing.Size(532, 73);
            this.panel_BaseSet.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panel_BaseSet.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panel_BaseSet.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panel_BaseSet.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panel_BaseSet.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panel_BaseSet.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panel_BaseSet.Style.GradientAngle = 90;
            this.panel_BaseSet.TabIndex = 5;
            // 
            // cmbMainProcess
            // 
            this.cmbMainProcess.DisplayMember = "Text";
            this.cmbMainProcess.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbMainProcess.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbMainProcess.FormattingEnabled = true;
            this.cmbMainProcess.ItemHeight = 17;
            this.cmbMainProcess.Location = new System.Drawing.Point(255, 99);
            this.cmbMainProcess.Name = "cmbMainProcess";
            this.cmbMainProcess.Size = new System.Drawing.Size(121, 23);
            this.cmbMainProcess.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbMainProcess.TabIndex = 4;
            // 
            // cmbResetProcess
            // 
            this.cmbResetProcess.DisplayMember = "Text";
            this.cmbResetProcess.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbResetProcess.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbResetProcess.FormattingEnabled = true;
            this.cmbResetProcess.ItemHeight = 17;
            this.cmbResetProcess.Location = new System.Drawing.Point(255, 66);
            this.cmbResetProcess.Name = "cmbResetProcess";
            this.cmbResetProcess.Size = new System.Drawing.Size(121, 23);
            this.cmbResetProcess.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbResetProcess.TabIndex = 4;
            // 
            // btnSelectSeqence
            // 
            this.btnSelectSeqence.BackColor = System.Drawing.Color.Transparent;
            this.btnSelectSeqence.BaseColor = System.Drawing.Color.Transparent;
            this.btnSelectSeqence.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnSelectSeqence.DownBack = null;
            this.btnSelectSeqence.GlowColor = System.Drawing.Color.Blue;
            this.btnSelectSeqence.Location = new System.Drawing.Point(428, 28);
            this.btnSelectSeqence.MouseBack = null;
            this.btnSelectSeqence.Name = "btnSelectSeqence";
            this.btnSelectSeqence.NormlBack = null;
            this.btnSelectSeqence.Size = new System.Drawing.Size(83, 28);
            this.btnSelectSeqence.TabIndex = 3;
            this.btnSelectSeqence.Text = "工作空间";
            this.btnSelectSeqence.UseVisualStyleBackColor = false;
            this.btnSelectSeqence.Click += new System.EventHandler(this.btnSelectSeqence_Click);
            // 
            // txtViewName
            // 
            this.txtViewName.BackColor = System.Drawing.Color.Transparent;
            this.txtViewName.DownBack = null;
            this.txtViewName.Icon = null;
            this.txtViewName.IconIsButton = false;
            this.txtViewName.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtViewName.IsPasswordChat = '\0';
            this.txtViewName.IsSystemPasswordChar = false;
            this.txtViewName.Lines = new string[0];
            this.txtViewName.Location = new System.Drawing.Point(7, 28);
            this.txtViewName.Margin = new System.Windows.Forms.Padding(0);
            this.txtViewName.MaxLength = 32767;
            this.txtViewName.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtViewName.MouseBack = null;
            this.txtViewName.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtViewName.Multiline = false;
            this.txtViewName.Name = "txtViewName";
            this.txtViewName.NormlBack = null;
            this.txtViewName.Padding = new System.Windows.Forms.Padding(5);
            this.txtViewName.ReadOnly = false;
            this.txtViewName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtViewName.Size = new System.Drawing.Size(419, 28);
            // 
            // 
            // 
            this.txtViewName.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtViewName.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtViewName.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtViewName.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtViewName.SkinTxt.Name = "BaseText";
            this.txtViewName.SkinTxt.Size = new System.Drawing.Size(409, 16);
            this.txtViewName.SkinTxt.TabIndex = 0;
            this.txtViewName.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtViewName.SkinTxt.WaterText = "窗口名称";
            this.txtViewName.TabIndex = 2;
            this.txtViewName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtViewName.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtViewName.WaterText = "窗口名称";
            this.txtViewName.WordWrap = true;
            // 
            // skinLabel8
            // 
            this.skinLabel8.AutoSize = true;
            this.skinLabel8.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel8.BorderColor = System.Drawing.Color.White;
            this.skinLabel8.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel8.Location = new System.Drawing.Point(189, 102);
            this.skinLabel8.Name = "skinLabel8";
            this.skinLabel8.Size = new System.Drawing.Size(47, 17);
            this.skinLabel8.TabIndex = 1;
            this.skinLabel8.Text = "主流程:";
            // 
            // skinLabel7
            // 
            this.skinLabel7.AutoSize = true;
            this.skinLabel7.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel7.BorderColor = System.Drawing.Color.White;
            this.skinLabel7.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel7.Location = new System.Drawing.Point(189, 69);
            this.skinLabel7.Name = "skinLabel7";
            this.skinLabel7.Size = new System.Drawing.Size(59, 17);
            this.skinLabel7.TabIndex = 1;
            this.skinLabel7.Text = "复位流程:";
            // 
            // skinLabel6
            // 
            this.skinLabel6.AutoSize = true;
            this.skinLabel6.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel6.BorderColor = System.Drawing.Color.White;
            this.skinLabel6.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel6.Location = new System.Drawing.Point(6, 6);
            this.skinLabel6.Name = "skinLabel6";
            this.skinLabel6.Size = new System.Drawing.Size(71, 17);
            this.skinLabel6.TabIndex = 1;
            this.skinLabel6.Text = "主界面名称:";
            // 
            // chkIsInitDevice
            // 
            this.chkIsInitDevice.AutoSize = true;
            this.chkIsInitDevice.BackColor = System.Drawing.Color.Transparent;
            this.chkIsInitDevice.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.chkIsInitDevice.DownBack = null;
            this.chkIsInitDevice.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkIsInitDevice.Location = new System.Drawing.Point(7, 67);
            this.chkIsInitDevice.MouseBack = null;
            this.chkIsInitDevice.Name = "chkIsInitDevice";
            this.chkIsInitDevice.NormlBack = null;
            this.chkIsInitDevice.SelectedDownBack = null;
            this.chkIsInitDevice.SelectedMouseBack = null;
            this.chkIsInitDevice.SelectedNormlBack = null;
            this.chkIsInitDevice.Size = new System.Drawing.Size(135, 21);
            this.chkIsInitDevice.TabIndex = 0;
            this.chkIsInitDevice.Text = "是否启动初始化设备";
            this.chkIsInitDevice.UseVisualStyleBackColor = false;
            // 
            // chkEnableOsk
            // 
            this.chkEnableOsk.AutoSize = true;
            this.chkEnableOsk.BackColor = System.Drawing.Color.Transparent;
            this.chkEnableOsk.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.chkEnableOsk.DownBack = null;
            this.chkEnableOsk.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkEnableOsk.Location = new System.Drawing.Point(7, 100);
            this.chkEnableOsk.MouseBack = null;
            this.chkEnableOsk.Name = "chkEnableOsk";
            this.chkEnableOsk.NormlBack = null;
            this.chkEnableOsk.SelectedDownBack = null;
            this.chkEnableOsk.SelectedMouseBack = null;
            this.chkEnableOsk.SelectedNormlBack = null;
            this.chkEnableOsk.Size = new System.Drawing.Size(111, 21);
            this.chkEnableOsk.TabIndex = 0;
            this.chkEnableOsk.Text = "是否弹出软键盘";
            this.chkEnableOsk.UseVisualStyleBackColor = false;
            // 
            // chkEnableRunView
            // 
            this.chkEnableRunView.AutoSize = true;
            this.chkEnableRunView.BackColor = System.Drawing.Color.Transparent;
            this.chkEnableRunView.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.chkEnableRunView.DownBack = null;
            this.chkEnableRunView.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkEnableRunView.Location = new System.Drawing.Point(7, 166);
            this.chkEnableRunView.MouseBack = null;
            this.chkEnableRunView.Name = "chkEnableRunView";
            this.chkEnableRunView.NormlBack = null;
            this.chkEnableRunView.SelectedDownBack = null;
            this.chkEnableRunView.SelectedMouseBack = null;
            this.chkEnableRunView.SelectedNormlBack = null;
            this.chkEnableRunView.Size = new System.Drawing.Size(123, 21);
            this.chkEnableRunView.TabIndex = 0;
            this.chkEnableRunView.Text = "是否弹出运行界面";
            this.chkEnableRunView.UseVisualStyleBackColor = false;
            // 
            // chkIsProVisible
            // 
            this.chkIsProVisible.AutoSize = true;
            this.chkIsProVisible.BackColor = System.Drawing.Color.Transparent;
            this.chkIsProVisible.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.chkIsProVisible.DownBack = null;
            this.chkIsProVisible.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkIsProVisible.Location = new System.Drawing.Point(7, 232);
            this.chkIsProVisible.MouseBack = null;
            this.chkIsProVisible.Name = "chkIsProVisible";
            this.chkIsProVisible.NormlBack = null;
            this.chkIsProVisible.SelectedDownBack = null;
            this.chkIsProVisible.SelectedMouseBack = null;
            this.chkIsProVisible.SelectedNormlBack = null;
            this.chkIsProVisible.Size = new System.Drawing.Size(99, 21);
            this.chkIsProVisible.TabIndex = 0;
            this.chkIsProVisible.Text = "是否隐藏流程";
            this.chkIsProVisible.UseVisualStyleBackColor = false;
            // 
            // chkIsRealDisplay
            // 
            this.chkIsRealDisplay.AutoSize = true;
            this.chkIsRealDisplay.BackColor = System.Drawing.Color.Transparent;
            this.chkIsRealDisplay.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.chkIsRealDisplay.DownBack = null;
            this.chkIsRealDisplay.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkIsRealDisplay.Location = new System.Drawing.Point(7, 331);
            this.chkIsRealDisplay.MouseBack = null;
            this.chkIsRealDisplay.Name = "chkIsRealDisplay";
            this.chkIsRealDisplay.NormlBack = null;
            this.chkIsRealDisplay.SelectedDownBack = null;
            this.chkIsRealDisplay.SelectedMouseBack = null;
            this.chkIsRealDisplay.SelectedNormlBack = null;
            this.chkIsRealDisplay.Size = new System.Drawing.Size(123, 21);
            this.chkIsRealDisplay.TabIndex = 0;
            this.chkIsRealDisplay.Text = "相机是否实时显示";
            this.chkIsRealDisplay.UseVisualStyleBackColor = false;
            // 
            // chkIsItemVisible
            // 
            this.chkIsItemVisible.AutoSize = true;
            this.chkIsItemVisible.BackColor = System.Drawing.Color.Transparent;
            this.chkIsItemVisible.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.chkIsItemVisible.DownBack = null;
            this.chkIsItemVisible.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkIsItemVisible.Location = new System.Drawing.Point(7, 265);
            this.chkIsItemVisible.MouseBack = null;
            this.chkIsItemVisible.Name = "chkIsItemVisible";
            this.chkIsItemVisible.NormlBack = null;
            this.chkIsItemVisible.SelectedDownBack = null;
            this.chkIsItemVisible.SelectedMouseBack = null;
            this.chkIsItemVisible.SelectedNormlBack = null;
            this.chkIsItemVisible.Size = new System.Drawing.Size(111, 21);
            this.chkIsItemVisible.TabIndex = 0;
            this.chkIsItemVisible.Text = "可视化测试项目";
            this.chkIsItemVisible.UseVisualStyleBackColor = false;
            // 
            // chkIsShowCross
            // 
            this.chkIsShowCross.AutoSize = true;
            this.chkIsShowCross.BackColor = System.Drawing.Color.Transparent;
            this.chkIsShowCross.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.chkIsShowCross.DownBack = null;
            this.chkIsShowCross.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkIsShowCross.Location = new System.Drawing.Point(7, 298);
            this.chkIsShowCross.MouseBack = null;
            this.chkIsShowCross.Name = "chkIsShowCross";
            this.chkIsShowCross.NormlBack = null;
            this.chkIsShowCross.SelectedDownBack = null;
            this.chkIsShowCross.SelectedMouseBack = null;
            this.chkIsShowCross.SelectedNormlBack = null;
            this.chkIsShowCross.Size = new System.Drawing.Size(135, 21);
            this.chkIsShowCross.TabIndex = 0;
            this.chkIsShowCross.Text = "图像窗口显示中心线";
            this.chkIsShowCross.UseVisualStyleBackColor = false;
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.listBox1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 20;
            this.listBox1.Items.AddRange(new object[] {
            "基本设置",
            "日志设置",
            "其他设置"});
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(147, 444);
            this.listBox1.TabIndex = 4;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // SetView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.OldLace;
            this.ClientSize = new System.Drawing.Size(703, 444);
            this.Controls.Add(this.panelEx1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SetView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "系统设置";
            this.Load += new System.EventHandler(this.SetView_Load);
            this.panelEx1.ResumeLayout(false);
            this.panel_OtherSet.ResumeLayout(false);
            this.panel_OtherSet.PerformLayout();
            this.skinGroupBox1.ResumeLayout(false);
            this.skinGroupBox1.PerformLayout();
            this.panel_LogSet.ResumeLayout(false);
            this.panel_LogSet.PerformLayout();
            this.panel_BaseSet.ResumeLayout(false);
            this.panel_BaseSet.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private CCWin.SkinControl.SkinButton btnCancel;
        private CCWin.SkinControl.SkinButton btnConfirm;
        private CCWin.SkinControl.SkinButton btnLoadPath;
        private CCWin.SkinControl.SkinTextBox txtFileDay;
        private CCWin.SkinControl.SkinTextBox txtPath;
        private CCWin.SkinControl.SkinLabel skinLabel3;
        private CCWin.SkinControl.SkinLabel skinLabel1;
        private CCWin.SkinControl.SkinCheckBox chkIsSaveImg;
        private CCWin.SkinControl.SkinLabel skinLabel2;
        private CCWin.SkinControl.SkinTextBox txtMaxCapaity;
        private CCWin.SkinControl.SkinLabel skinLabel5;
        private CCWin.SkinControl.SkinLabel skinLabel4;
        private CCWin.SkinControl.SkinCheckBox chkMinToPallet;
        private CCWin.SkinControl.SkinCheckBox chkOpenMany;
        private CCWin.SkinControl.SkinCheckBox chkOpenAutoRun;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private CCWin.SkinControl.SkinCheckBox chkEnableOsk;
        private CCWin.SkinControl.SkinCheckBox chkEnableRunView;
        private CCWin.SkinControl.SkinButton btnSelectSeqence;
        private CCWin.SkinControl.SkinLabel skinLabel6;
        private CCWin.SkinControl.SkinTextBox txtViewName;
        private CCWin.SkinControl.SkinButton btnLoadAddr;
        private CCWin.SkinControl.SkinCheckBox chkRunOtherSoft;
        private CCWin.SkinControl.SkinLabel lblSoftAddr;
        private CCWin.SkinControl.SkinTextBox txtSoftAddr;
        private CCWin.SkinControl.SkinButton btnDelImage;
        private CCWin.SkinControl.SkinCheckBox chkIsPrintLog;
        private CCWin.SkinControl.SkinCheckBox chkIsInitDevice;
        private CCWin.SkinControl.SkinCheckBox chkIsProVisible;
        private CCWin.SkinControl.SkinCheckBox chkIsItemVisible;
        private CCWin.SkinControl.SkinCheckBox chkIsRealDisplay;
        private CCWin.SkinControl.SkinCheckBox chkIsShowCross;
        private System.Windows.Forms.ListBox listBox1;
        private DevComponents.DotNetBar.PanelEx panel_LogSet;
        private DevComponents.DotNetBar.PanelEx panel_BaseSet;
        private DevComponents.DotNetBar.PanelEx panel_OtherSet;
        private CCWin.SkinControl.SkinCheckBox chkIsStopShowMsg;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbMainProcess;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbResetProcess;
        private CCWin.SkinControl.SkinLabel skinLabel8;
        private CCWin.SkinControl.SkinLabel skinLabel7;
        private CCWin.SkinControl.SkinGroupBox skinGroupBox1;
        private CCWin.SkinControl.SkinRadioButton radioZMotion;
        private CCWin.SkinControl.SkinButton btnLoadImagePath;
        private CCWin.SkinControl.SkinTextBox txtImagePath;
        private CCWin.SkinControl.SkinCheckBox chkIsSaveNGImage;
        private CCWin.SkinControl.SkinRadioButton radioVirtual;
        private CCWin.SkinControl.SkinRadioButton radioSoftPLC;
        private CCWin.SkinControl.SkinCheckBox chkIsFullScreen;
    }
}