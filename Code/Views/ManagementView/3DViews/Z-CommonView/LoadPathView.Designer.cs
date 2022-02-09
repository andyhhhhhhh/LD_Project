namespace ManagementView._3DViews.CommonView
{
    partial class LoadPathView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadPathView));
            this.txtFolderPath = new CCWin.SkinControl.SkinTextBox();
            this.btnLoadFolder = new CCWin.SkinControl.SkinButton();
            this.lblPath = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtFolderPath
            // 
            this.txtFolderPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFolderPath.BackColor = System.Drawing.Color.Transparent;
            this.txtFolderPath.DownBack = null;
            this.txtFolderPath.Icon = null;
            this.txtFolderPath.IconIsButton = false;
            this.txtFolderPath.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtFolderPath.IsPasswordChat = '\0';
            this.txtFolderPath.IsSystemPasswordChar = false;
            this.txtFolderPath.Lines = new string[0];
            this.txtFolderPath.Location = new System.Drawing.Point(76, 0);
            this.txtFolderPath.Margin = new System.Windows.Forms.Padding(0);
            this.txtFolderPath.MaxLength = 32767;
            this.txtFolderPath.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtFolderPath.MouseBack = null;
            this.txtFolderPath.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtFolderPath.Multiline = false;
            this.txtFolderPath.Name = "txtFolderPath";
            this.txtFolderPath.NormlBack = null;
            this.txtFolderPath.Padding = new System.Windows.Forms.Padding(5);
            this.txtFolderPath.ReadOnly = false;
            this.txtFolderPath.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtFolderPath.Size = new System.Drawing.Size(291, 28);
            // 
            // 
            // 
            this.txtFolderPath.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFolderPath.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFolderPath.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.txtFolderPath.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtFolderPath.SkinTxt.Name = "BaseText";
            this.txtFolderPath.SkinTxt.Size = new System.Drawing.Size(281, 18);
            this.txtFolderPath.SkinTxt.TabIndex = 0;
            this.txtFolderPath.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtFolderPath.SkinTxt.WaterText = "打开文件夹路径";
            this.txtFolderPath.TabIndex = 1;
            this.txtFolderPath.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtFolderPath.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtFolderPath.WaterText = "打开文件夹路径";
            this.txtFolderPath.WordWrap = true;
            // 
            // btnLoadFolder
            // 
            this.btnLoadFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadFolder.BackColor = System.Drawing.Color.Transparent;
            this.btnLoadFolder.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLoadFolder.BackgroundImage")));
            this.btnLoadFolder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLoadFolder.BaseColor = System.Drawing.Color.Transparent;
            this.btnLoadFolder.BorderColor = System.Drawing.Color.Transparent;
            this.btnLoadFolder.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnLoadFolder.DownBack = null;
            this.btnLoadFolder.Location = new System.Drawing.Point(366, 0);
            this.btnLoadFolder.MouseBack = null;
            this.btnLoadFolder.Name = "btnLoadFolder";
            this.btnLoadFolder.NormlBack = null;
            this.btnLoadFolder.Size = new System.Drawing.Size(30, 28);
            this.btnLoadFolder.TabIndex = 2;
            this.btnLoadFolder.UseVisualStyleBackColor = false;
            this.btnLoadFolder.Click += new System.EventHandler(this.btnLoadFolder_Click);
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPath.Location = new System.Drawing.Point(3, 6);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(71, 17);
            this.lblPath.TabIndex = 0;
            this.lblPath.Text = "文件夹路径:";
            // 
            // LoadPathView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnLoadFolder);
            this.Controls.Add(this.txtFolderPath);
            this.Controls.Add(this.lblPath);
            this.Name = "LoadPathView";
            this.Size = new System.Drawing.Size(397, 29);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CCWin.SkinControl.SkinTextBox txtFolderPath;
        private CCWin.SkinControl.SkinButton btnLoadFolder;
        private System.Windows.Forms.Label lblPath;
    }
}
