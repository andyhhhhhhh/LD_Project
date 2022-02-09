namespace ManagementView.UserView
{
    partial class UserLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserLogin));
            this.btnLogin = new CCWin.SkinControl.SkinButton();
            this.labelUser = new CCWin.SkinControl.SkinLabel();
            this.labelPassword = new CCWin.SkinControl.SkinLabel();
            this.labelUserSelect = new CCWin.SkinControl.SkinLabel();
            this.txtUser = new CCWin.SkinControl.SkinTextBox();
            this.btnCancel = new CCWin.SkinControl.SkinButton();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.txtPassword = new ManagementView.Comment.TextInput();
            this.cmbUserSelect = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.Transparent;
            this.btnLogin.BaseColor = System.Drawing.Color.Transparent;
            this.btnLogin.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnLogin.DownBack = null;
            this.btnLogin.GlowColor = System.Drawing.Color.Blue;
            this.btnLogin.Location = new System.Drawing.Point(40, 135);
            this.btnLogin.MouseBack = null;
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.NormlBack = null;
            this.btnLogin.Size = new System.Drawing.Size(75, 33);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Text = "登录";
            this.btnLogin.UseMnemonic = false;
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // labelUser
            // 
            this.labelUser.AutoSize = true;
            this.labelUser.BackColor = System.Drawing.Color.Transparent;
            this.labelUser.BorderColor = System.Drawing.Color.White;
            this.labelUser.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelUser.Location = new System.Drawing.Point(37, 207);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(47, 17);
            this.labelUser.TabIndex = 1;
            this.labelUser.Text = "用户名:";
            this.labelUser.Visible = false;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.BackColor = System.Drawing.Color.Transparent;
            this.labelPassword.BorderColor = System.Drawing.Color.White;
            this.labelPassword.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelPassword.Location = new System.Drawing.Point(37, 83);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(35, 17);
            this.labelPassword.TabIndex = 1;
            this.labelPassword.Text = "密码:";
            // 
            // labelUserSelect
            // 
            this.labelUserSelect.AutoSize = true;
            this.labelUserSelect.BackColor = System.Drawing.Color.Transparent;
            this.labelUserSelect.BorderColor = System.Drawing.Color.White;
            this.labelUserSelect.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelUserSelect.Location = new System.Drawing.Point(37, 26);
            this.labelUserSelect.Name = "labelUserSelect";
            this.labelUserSelect.Size = new System.Drawing.Size(59, 17);
            this.labelUserSelect.TabIndex = 1;
            this.labelUserSelect.Text = "用户选择:";
            // 
            // txtUser
            // 
            this.txtUser.BackColor = System.Drawing.Color.Transparent;
            this.txtUser.DownBack = null;
            this.txtUser.Icon = null;
            this.txtUser.IconIsButton = false;
            this.txtUser.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtUser.IsPasswordChat = '\0';
            this.txtUser.IsSystemPasswordChar = false;
            this.txtUser.Lines = new string[] {
        "user"};
            this.txtUser.Location = new System.Drawing.Point(127, 201);
            this.txtUser.Margin = new System.Windows.Forms.Padding(0);
            this.txtUser.MaxLength = 32767;
            this.txtUser.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtUser.MouseBack = null;
            this.txtUser.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtUser.Multiline = false;
            this.txtUser.Name = "txtUser";
            this.txtUser.NormlBack = null;
            this.txtUser.Padding = new System.Windows.Forms.Padding(5);
            this.txtUser.ReadOnly = false;
            this.txtUser.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtUser.Size = new System.Drawing.Size(148, 28);
            // 
            // 
            // 
            this.txtUser.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUser.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUser.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.txtUser.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtUser.SkinTxt.Name = "BaseText";
            this.txtUser.SkinTxt.Size = new System.Drawing.Size(138, 18);
            this.txtUser.SkinTxt.TabIndex = 0;
            this.txtUser.SkinTxt.Text = "user";
            this.txtUser.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtUser.SkinTxt.WaterText = "";
            this.txtUser.TabIndex = 2;
            this.txtUser.Text = "user";
            this.txtUser.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtUser.Visible = false;
            this.txtUser.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtUser.WaterText = "";
            this.txtUser.WordWrap = true;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BaseColor = System.Drawing.Color.Transparent;
            this.btnCancel.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnCancel.DownBack = null;
            this.btnCancel.GlowColor = System.Drawing.Color.Blue;
            this.btnCancel.Location = new System.Drawing.Point(200, 135);
            this.btnCancel.MouseBack = null;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.NormlBack = null;
            this.btnCancel.Size = new System.Drawing.Size(75, 33);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.txtPassword);
            this.panelEx1.Controls.Add(this.cmbUserSelect);
            this.panelEx1.Controls.Add(this.btnLogin);
            this.panelEx1.Controls.Add(this.btnCancel);
            this.panelEx1.Controls.Add(this.txtUser);
            this.panelEx1.Controls.Add(this.labelUser);
            this.panelEx1.Controls.Add(this.labelUserSelect);
            this.panelEx1.Controls.Add(this.labelPassword);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(321, 191);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 4;
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.White;
            this.txtPassword.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPassword.IsMultiLine = false;
            this.txtPassword.IsPassword = true;
            this.txtPassword.Location = new System.Drawing.Point(127, 78);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(148, 26);
            this.txtPassword.sText = "";
            this.txtPassword.TabIndex = 5;
            // 
            // cmbUserSelect
            // 
            this.cmbUserSelect.DisplayMember = "Text";
            this.cmbUserSelect.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbUserSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUserSelect.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbUserSelect.FormattingEnabled = true;
            this.cmbUserSelect.ItemHeight = 20;
            this.cmbUserSelect.Location = new System.Drawing.Point(127, 22);
            this.cmbUserSelect.Name = "cmbUserSelect";
            this.cmbUserSelect.Size = new System.Drawing.Size(148, 26);
            this.cmbUserSelect.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbUserSelect.TabIndex = 1;
            this.cmbUserSelect.SelectedIndexChanged += new System.EventHandler(this.cmbUserSelect_SelectedIndexChanged);
            // 
            // UserLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FloralWhite;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(321, 191);
            this.Controls.Add(this.panelEx1);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UserLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "登录";
            this.Load += new System.EventHandler(this.UserLogin_Load);
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CCWin.SkinControl.SkinButton btnLogin;
        private CCWin.SkinControl.SkinLabel labelUser;
        private CCWin.SkinControl.SkinLabel labelPassword;
        private CCWin.SkinControl.SkinLabel labelUserSelect;
        private CCWin.SkinControl.SkinTextBox txtUser;
        private CCWin.SkinControl.SkinButton btnCancel;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbUserSelect;
        private Comment.TextInput txtPassword;
    }
}