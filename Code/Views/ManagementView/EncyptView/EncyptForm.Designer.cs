namespace ManagementView.EncyptView
{
    partial class EncyptForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EncyptForm));
            this.lbRegistInfo = new System.Windows.Forms.Label();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.txtInfoKey = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtRegisterKey = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnRegist = new CCWin.SkinControl.SkinButton();
            this.btnExit = new CCWin.SkinControl.SkinButton();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.lblUseTime = new System.Windows.Forms.Label();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbRegistInfo
            // 
            this.lbRegistInfo.AutoSize = true;
            this.lbRegistInfo.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbRegistInfo.ForeColor = System.Drawing.Color.Green;
            this.lbRegistInfo.Location = new System.Drawing.Point(82, 268);
            this.lbRegistInfo.Name = "lbRegistInfo";
            this.lbRegistInfo.Size = new System.Drawing.Size(74, 20);
            this.lbRegistInfo.TabIndex = 0;
            this.lbRegistInfo.Text = "正在注册...";
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(16, 15);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(62, 23);
            this.labelX1.TabIndex = 2;
            this.labelX1.Text = "机器码:";
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(16, 144);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(62, 23);
            this.labelX2.TabIndex = 2;
            this.labelX2.Text = "注册码:";
            // 
            // txtInfoKey
            // 
            // 
            // 
            // 
            this.txtInfoKey.Border.Class = "TextBoxBorder";
            this.txtInfoKey.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtInfoKey.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtInfoKey.Location = new System.Drawing.Point(85, 15);
            this.txtInfoKey.Multiline = true;
            this.txtInfoKey.Name = "txtInfoKey";
            this.txtInfoKey.Size = new System.Drawing.Size(357, 104);
            this.txtInfoKey.TabIndex = 3;
            // 
            // txtRegisterKey
            // 
            // 
            // 
            // 
            this.txtRegisterKey.Border.Class = "TextBoxBorder";
            this.txtRegisterKey.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtRegisterKey.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRegisterKey.Location = new System.Drawing.Point(85, 144);
            this.txtRegisterKey.Multiline = true;
            this.txtRegisterKey.Name = "txtRegisterKey";
            this.txtRegisterKey.Size = new System.Drawing.Size(357, 104);
            this.txtRegisterKey.TabIndex = 3;
            // 
            // btnRegist
            // 
            this.btnRegist.BackColor = System.Drawing.Color.Transparent;
            this.btnRegist.BaseColor = System.Drawing.Color.Transparent;
            this.btnRegist.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnRegist.DownBack = null;
            this.btnRegist.Location = new System.Drawing.Point(44, 309);
            this.btnRegist.MouseBack = null;
            this.btnRegist.Name = "btnRegist";
            this.btnRegist.NormlBack = null;
            this.btnRegist.Size = new System.Drawing.Size(103, 33);
            this.btnRegist.TabIndex = 4;
            this.btnRegist.Text = "注册";
            this.btnRegist.UseVisualStyleBackColor = false;
            this.btnRegist.Click += new System.EventHandler(this.btnRegist_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.BaseColor = System.Drawing.Color.Transparent;
            this.btnExit.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.DownBack = null;
            this.btnExit.Location = new System.Drawing.Point(309, 309);
            this.btnExit.MouseBack = null;
            this.btnExit.Name = "btnExit";
            this.btnExit.NormlBack = null;
            this.btnExit.Size = new System.Drawing.Size(103, 33);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.btnRegist);
            this.panelEx1.Controls.Add(this.btnExit);
            this.panelEx1.Controls.Add(this.lblUseTime);
            this.panelEx1.Controls.Add(this.lbRegistInfo);
            this.panelEx1.Controls.Add(this.labelX1);
            this.panelEx1.Controls.Add(this.txtRegisterKey);
            this.panelEx1.Controls.Add(this.labelX2);
            this.panelEx1.Controls.Add(this.txtInfoKey);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(463, 357);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 5;
            // 
            // lblUseTime
            // 
            this.lblUseTime.AutoSize = true;
            this.lblUseTime.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblUseTime.ForeColor = System.Drawing.Color.Green;
            this.lblUseTime.Location = new System.Drawing.Point(270, 268);
            this.lblUseTime.Name = "lblUseTime";
            this.lblUseTime.Size = new System.Drawing.Size(93, 20);
            this.lblUseTime.TabIndex = 0;
            this.lblUseTime.Text = "软件永久权限";
            // 
            // EncyptForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(463, 357);
            this.Controls.Add(this.panelEx1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "EncyptForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "注册窗口";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbRegistInfo;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.TextBoxX txtInfoKey;
        private DevComponents.DotNetBar.Controls.TextBoxX txtRegisterKey;
        private CCWin.SkinControl.SkinButton btnRegist;
        private CCWin.SkinControl.SkinButton btnExit;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private System.Windows.Forms.Label lblUseTime;
    }
}