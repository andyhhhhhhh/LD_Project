namespace ManagementView.Popup
{
    partial class SplashView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashView));
            this.labelCompany = new System.Windows.Forms.Label();
            this.lblLoadTxt = new CCWin.SkinControl.SkinLabel();
            this.skinprogress = new CCWin.SkinControl.SkinProgressIndicator();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelCompany
            // 
            this.labelCompany.AutoSize = true;
            this.labelCompany.BackColor = System.Drawing.Color.Transparent;
            this.labelCompany.Font = new System.Drawing.Font("新宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelCompany.ForeColor = System.Drawing.Color.Khaki;
            this.labelCompany.Location = new System.Drawing.Point(178, 144);
            this.labelCompany.Name = "labelCompany";
            this.labelCompany.Size = new System.Drawing.Size(89, 20);
            this.labelCompany.TabIndex = 5;
            this.labelCompany.Text = "加载数据";
            // 
            // lblLoadTxt
            // 
            this.lblLoadTxt.AutoSize = true;
            this.lblLoadTxt.BackColor = System.Drawing.Color.Transparent;
            this.lblLoadTxt.BorderColor = System.Drawing.Color.White;
            this.lblLoadTxt.Font = new System.Drawing.Font("隶书", 42F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblLoadTxt.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.lblLoadTxt.Location = new System.Drawing.Point(178, 46);
            this.lblLoadTxt.Name = "lblLoadTxt";
            this.lblLoadTxt.Size = new System.Drawing.Size(444, 56);
            this.lblLoadTxt.TabIndex = 4;
            this.lblLoadTxt.Text = "正在加载中.....";
            // 
            // skinprogress
            // 
            this.skinprogress.BackColor = System.Drawing.Color.Transparent;
            this.skinprogress.CircleColor = System.Drawing.Color.Yellow;
            this.skinprogress.Dock = System.Windows.Forms.DockStyle.Left;
            this.skinprogress.Location = new System.Drawing.Point(0, 0);
            this.skinprogress.Name = "skinprogress";
            this.skinprogress.Percentage = 0F;
            this.skinprogress.Size = new System.Drawing.Size(167, 167);
            this.skinprogress.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("新宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.PaleGoldenrod;
            this.label1.Location = new System.Drawing.Point(412, 144);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(269, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "深圳市维柯智能科技有限公司";
            // 
            // SplashView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(678, 167);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelCompany);
            this.Controls.Add(this.lblLoadTxt);
            this.Controls.Add(this.skinprogress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SplashView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "欢迎界面";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SplashView_FormClosing);
            this.Load += new System.EventHandler(this.SplashView_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelCompany;
        private CCWin.SkinControl.SkinLabel lblLoadTxt;
        private CCWin.SkinControl.SkinProgressIndicator skinprogress;
        private System.Windows.Forms.Label label1;
    }
}