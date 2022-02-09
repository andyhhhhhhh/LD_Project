namespace ManagementView.EditView
{
    partial class ELblResult
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblResult = new DevComponents.DotNetBar.LabelX();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            // 
            // lblResult
            // 
            // 
            // 
            // 
            this.lblResult.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblResult.Font = new System.Drawing.Font("宋体", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblResult.ForeColor = System.Drawing.Color.Green;
            this.lblResult.Location = new System.Drawing.Point(0, 0);
            this.lblResult.Margin = new System.Windows.Forms.Padding(0);
            this.lblResult.Name = "lblResult";
            this.lblResult.PaddingTop = 8;
            this.lblResult.Size = new System.Drawing.Size(136, 112);
            this.lblResult.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.lblResult.TabIndex = 1;
            this.lblResult.Text = "OK";
            this.lblResult.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // ELblResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblResult);
            this.Name = "ELblResult";
            this.Size = new System.Drawing.Size(136, 112);
            this.Load += new System.EventHandler(this.ELblResult_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private DevComponents.DotNetBar.LabelX lblResult;
    }
}
