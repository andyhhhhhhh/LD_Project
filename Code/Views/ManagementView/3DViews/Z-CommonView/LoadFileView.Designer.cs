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
            this.lblFileName = new DevComponents.DotNetBar.LabelX();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLoadFile
            // 
            this.btnLoadFile.BackColor = System.Drawing.Color.Transparent;
            this.btnLoadFile.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLoadFile.BackgroundImage")));
            this.btnLoadFile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLoadFile.BaseColor = System.Drawing.Color.Transparent;
            this.btnLoadFile.BorderColor = System.Drawing.Color.Transparent;
            this.btnLoadFile.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnLoadFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLoadFile.DownBack = null;
            this.btnLoadFile.Location = new System.Drawing.Point(471, 1);
            this.btnLoadFile.Margin = new System.Windows.Forms.Padding(1);
            this.btnLoadFile.MouseBack = null;
            this.btnLoadFile.Name = "btnLoadFile";
            this.btnLoadFile.NormlBack = null;
            this.btnLoadFile.Size = new System.Drawing.Size(38, 27);
            this.btnLoadFile.TabIndex = 2;
            this.btnLoadFile.UseVisualStyleBackColor = false;
            this.btnLoadFile.Click += new System.EventHandler(this.btnLoadFile_Click);
            // 
            // txtFilePath
            // 
            this.txtFilePath.BackColor = System.Drawing.Color.Transparent;
            this.txtFilePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFilePath.DownBack = null;
            this.txtFilePath.Icon = null;
            this.txtFilePath.IconIsButton = false;
            this.txtFilePath.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtFilePath.IsPasswordChat = '\0';
            this.txtFilePath.IsSystemPasswordChar = false;
            this.txtFilePath.Lines = new string[0];
            this.txtFilePath.Location = new System.Drawing.Point(80, 0);
            this.txtFilePath.Margin = new System.Windows.Forms.Padding(0);
            this.txtFilePath.MaxLength = 32767;
            this.txtFilePath.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtFilePath.MouseBack = null;
            this.txtFilePath.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtFilePath.Multiline = true;
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.NormlBack = null;
            this.txtFilePath.Padding = new System.Windows.Forms.Padding(5);
            this.txtFilePath.ReadOnly = false;
            this.txtFilePath.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtFilePath.Size = new System.Drawing.Size(390, 29);
            // 
            // 
            // 
            this.txtFilePath.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFilePath.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFilePath.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.txtFilePath.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtFilePath.SkinTxt.Multiline = true;
            this.txtFilePath.SkinTxt.Name = "BaseText";
            this.txtFilePath.SkinTxt.Size = new System.Drawing.Size(380, 19);
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
            // 
            // 
            // 
            this.lblFileName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblFileName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFileName.Font = new System.Drawing.Font("微软雅黑", 9.5F);
            this.lblFileName.Location = new System.Drawing.Point(3, 3);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(74, 23);
            this.lblFileName.TabIndex = 3;
            this.lblFileName.Text = "文件路径:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Controls.Add(this.lblFileName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnLoadFile, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtFilePath, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(510, 29);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // LoadFileView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "LoadFileView";
            this.Size = new System.Drawing.Size(510, 29);
            this.Load += new System.EventHandler(this.LoadFileView_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private CCWin.SkinControl.SkinTextBox txtFilePath;
        private CCWin.SkinControl.SkinButton btnLoadFile;
        private DevComponents.DotNetBar.LabelX lblFileName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
