namespace ManagementView.UserView
{
    partial class SuperUserManage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SuperUserManage));
            this.chkDisplayDeb = new System.Windows.Forms.CheckBox();
            this.chkDisplayEng = new System.Windows.Forms.CheckBox();
            this.chkDisplayOp = new System.Windows.Forms.CheckBox();
            this.txtDebugValue = new System.Windows.Forms.TextBox();
            this.labelDebugger = new System.Windows.Forms.Label();
            this.txtEngValue = new System.Windows.Forms.TextBox();
            this.labelEngineer = new System.Windows.Forms.Label();
            this.txtOperValue = new System.Windows.Forms.TextBox();
            this.labelOperator = new System.Windows.Forms.Label();
            this.btnCancel = new CCWin.SkinControl.SkinButton();
            this.btnModify = new CCWin.SkinControl.SkinButton();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkDisplayDeb
            // 
            this.chkDisplayDeb.AutoSize = true;
            this.chkDisplayDeb.BackColor = System.Drawing.Color.Transparent;
            this.chkDisplayDeb.Location = new System.Drawing.Point(260, 66);
            this.chkDisplayDeb.Name = "chkDisplayDeb";
            this.chkDisplayDeb.Size = new System.Drawing.Size(48, 16);
            this.chkDisplayDeb.TabIndex = 6;
            this.chkDisplayDeb.Text = "显示";
            this.chkDisplayDeb.UseVisualStyleBackColor = false;
            this.chkDisplayDeb.CheckedChanged += new System.EventHandler(this.chkDisplayDeb_CheckedChanged);
            // 
            // chkDisplayEng
            // 
            this.chkDisplayEng.AutoSize = true;
            this.chkDisplayEng.BackColor = System.Drawing.Color.Transparent;
            this.chkDisplayEng.Location = new System.Drawing.Point(260, 108);
            this.chkDisplayEng.Name = "chkDisplayEng";
            this.chkDisplayEng.Size = new System.Drawing.Size(48, 16);
            this.chkDisplayEng.TabIndex = 4;
            this.chkDisplayEng.Text = "显示";
            this.chkDisplayEng.UseVisualStyleBackColor = false;
            this.chkDisplayEng.CheckedChanged += new System.EventHandler(this.chkDisplayEng_CheckedChanged);
            // 
            // chkDisplayOp
            // 
            this.chkDisplayOp.AutoSize = true;
            this.chkDisplayOp.BackColor = System.Drawing.Color.Transparent;
            this.chkDisplayOp.Location = new System.Drawing.Point(260, 18);
            this.chkDisplayOp.Name = "chkDisplayOp";
            this.chkDisplayOp.Size = new System.Drawing.Size(48, 16);
            this.chkDisplayOp.TabIndex = 2;
            this.chkDisplayOp.Text = "显示";
            this.chkDisplayOp.UseVisualStyleBackColor = false;
            this.chkDisplayOp.CheckedChanged += new System.EventHandler(this.chkDisplayOp_CheckedChanged);
            // 
            // txtDebugValue
            // 
            this.txtDebugValue.Location = new System.Drawing.Point(103, 64);
            this.txtDebugValue.Name = "txtDebugValue";
            this.txtDebugValue.PasswordChar = '*';
            this.txtDebugValue.Size = new System.Drawing.Size(145, 21);
            this.txtDebugValue.TabIndex = 5;
            // 
            // labelDebugger
            // 
            this.labelDebugger.AutoSize = true;
            this.labelDebugger.BackColor = System.Drawing.Color.Transparent;
            this.labelDebugger.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelDebugger.Location = new System.Drawing.Point(21, 67);
            this.labelDebugger.Name = "labelDebugger";
            this.labelDebugger.Size = new System.Drawing.Size(44, 17);
            this.labelDebugger.TabIndex = 6;
            this.labelDebugger.Text = "管理员";
            // 
            // txtEngValue
            // 
            this.txtEngValue.Location = new System.Drawing.Point(103, 106);
            this.txtEngValue.Name = "txtEngValue";
            this.txtEngValue.PasswordChar = '*';
            this.txtEngValue.Size = new System.Drawing.Size(145, 21);
            this.txtEngValue.TabIndex = 3;
            // 
            // labelEngineer
            // 
            this.labelEngineer.AutoSize = true;
            this.labelEngineer.BackColor = System.Drawing.Color.Transparent;
            this.labelEngineer.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelEngineer.Location = new System.Drawing.Point(21, 109);
            this.labelEngineer.Name = "labelEngineer";
            this.labelEngineer.Size = new System.Drawing.Size(44, 17);
            this.labelEngineer.TabIndex = 7;
            this.labelEngineer.Text = "工程师";
            // 
            // txtOperValue
            // 
            this.txtOperValue.Location = new System.Drawing.Point(103, 16);
            this.txtOperValue.Name = "txtOperValue";
            this.txtOperValue.PasswordChar = '*';
            this.txtOperValue.Size = new System.Drawing.Size(145, 21);
            this.txtOperValue.TabIndex = 1;
            // 
            // labelOperator
            // 
            this.labelOperator.AutoSize = true;
            this.labelOperator.BackColor = System.Drawing.Color.Transparent;
            this.labelOperator.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelOperator.Location = new System.Drawing.Point(21, 19);
            this.labelOperator.Name = "labelOperator";
            this.labelOperator.Size = new System.Drawing.Size(44, 17);
            this.labelOperator.TabIndex = 8;
            this.labelOperator.Text = "操作员";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BaseColor = System.Drawing.Color.Transparent;
            this.btnCancel.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnCancel.DownBack = null;
            this.btnCancel.GlowColor = System.Drawing.Color.Blue;
            this.btnCancel.Location = new System.Drawing.Point(205, 152);
            this.btnCancel.MouseBack = null;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.NormlBack = null;
            this.btnCancel.Size = new System.Drawing.Size(75, 33);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnModify
            // 
            this.btnModify.BackColor = System.Drawing.Color.Transparent;
            this.btnModify.BaseColor = System.Drawing.Color.Transparent;
            this.btnModify.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnModify.DownBack = null;
            this.btnModify.GlowColor = System.Drawing.Color.Blue;
            this.btnModify.Location = new System.Drawing.Point(43, 151);
            this.btnModify.MouseBack = null;
            this.btnModify.Name = "btnModify";
            this.btnModify.NormlBack = null;
            this.btnModify.Size = new System.Drawing.Size(75, 33);
            this.btnModify.TabIndex = 7;
            this.btnModify.Text = "密码修改";
            this.btnModify.UseVisualStyleBackColor = false;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.btnCancel);
            this.panelEx1.Controls.Add(this.btnModify);
            this.panelEx1.Controls.Add(this.labelOperator);
            this.panelEx1.Controls.Add(this.txtOperValue);
            this.panelEx1.Controls.Add(this.chkDisplayDeb);
            this.panelEx1.Controls.Add(this.labelEngineer);
            this.panelEx1.Controls.Add(this.chkDisplayEng);
            this.panelEx1.Controls.Add(this.txtEngValue);
            this.panelEx1.Controls.Add(this.chkDisplayOp);
            this.panelEx1.Controls.Add(this.labelDebugger);
            this.panelEx1.Controls.Add(this.txtDebugValue);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(333, 216);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 17;
            // 
            // SuperUserManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Beige;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(333, 216);
            this.Controls.Add(this.panelEx1);
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SuperUserManage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "用户管理";
            this.Load += new System.EventHandler(this.SuperUserManage_Load);
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkDisplayDeb;
        private System.Windows.Forms.CheckBox chkDisplayEng;
        private System.Windows.Forms.CheckBox chkDisplayOp;
        private System.Windows.Forms.TextBox txtDebugValue;
        private System.Windows.Forms.Label labelDebugger;
        private System.Windows.Forms.TextBox txtEngValue;
        private System.Windows.Forms.Label labelEngineer;
        private System.Windows.Forms.TextBox txtOperValue;
        private System.Windows.Forms.Label labelOperator;
        private CCWin.SkinControl.SkinButton btnCancel;
        private CCWin.SkinControl.SkinButton btnModify;
        private DevComponents.DotNetBar.PanelEx panelEx1;
    }
}