namespace ManagementView._3DViews.CommonView
{
    partial class LoadFileView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadFileView));
            this.btnLoadFile = new CCWin.SkinControl.SkinButton();
            this.txtFilePath = new CCWin.SkinControl.SkinTextBox();
            this.lblFileName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnLoadFile
            // 
            this.btnLoadFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadFile.BackColor = System.Drawing.Color.Transparent;
            this.btnLoadFile.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLoadFile.BackgroundImage")));
            this.btnLoadFile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnLoadFile.BaseColor = System.Drawing.Color.Transparent;
            this.btnLoadFile.BorderColor = System.Drawing.Color.Transparent;
            this.btnLoadFile.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnLoadFile.DownBack = null;
            this.btnLoadFile.Location = new System.Drawing.Point(308, 0);
            this.btnLoadFile.MouseBack = null;
            this.btnLoadFile.Name = "btnLoadFile";
            this.btnLoadFile.NormlBack = null;
            this.btnLoadFile.Size = new System.Drawing.Size(30, 28);
            this.btnLoadFile.TabIndex = 2;
            this.btnLoadFile.UseVisualStyleBackColor = false;
            this.btnLoadFile.Click += new System.EventHandler(this.btnLoadFile_Click);
            // 
            // txtFilePath
            // 
            this.txtFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilePath.BackColor = System.Drawing.Color.Transparent;
            this.txtFilePath.DownBack = null;
            this.txtFilePath.Icon = null;
            this.txtFilePath.IconIsButton = false;
            this.txtFilePath.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtFilePath.IsPasswordChat = '\0';
            this.txtFilePath.IsSystemPasswordChar = false;
            this.txtFilePath.Lines = new string[0];
            this.txtFilePath.Location = new System.Drawing.Point(61, 0);
            this.txtFilePath.Margin = new System.Windows.Forms.Padding(0);
            this.txtFilePath.MaxLength = 32767;
            this.txtFilePath.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtFilePath.MouseBack = null;
            this.txtFilePath.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtFilePath.Multiline = false;
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.NormlBack = null;
            this.txtFilePath.Padding = new System.Windows.Forms.Padding(5);
            this.txtFilePath.ReadOnly = false;
            this.txtFilePath.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtFilePath.Size = new System.Drawing.Size(244, 28);
            // 
            // 
            // 
            this.txtFilePath.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFilePath.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFilePath.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtFilePath.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtFilePath.SkinTxt.Name = "BaseText";
            this.txtFilePath.SkinTxt.Size = new System.Drawing.Size(234, 16);
            this.txtFilePath.SkinTxt.TabIndex = 0;
            this.txtFilePath.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtFilePath.SkinTxt.WaterText = "打开保存的文件路径";
            this.txtFilePath.TabIndex = 1;
            this.txtFilePath.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtFilePath.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtFilePath.WaterText = "打开保存的文件路径";
            this.txtFilePath.WordWrap = true;
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblFileName.Location = new System.Drawing.Point(3, 6);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(59, 17);
            this.lblFileName.TabIndex = 0;
            this.lblFileName.Text = "文件路径:";
            // 
            // LoadFileView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnLoadFile);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.lblFileName);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "LoadFileView";
            this.Size = new System.Drawing.Size(341, 28);
            this.Load += new System.EventHandler(this.LoadFileView_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private CCWin.SkinControl.SkinTextBox txtFilePath;
        private System.Windows.Forms.Label lblFileName;
        private CCWin.SkinControl.SkinButton btnLoadFile;
    }
}
