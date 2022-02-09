namespace ManagementView.EditView
{
    partial class EErrorItem
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
            this.txtTitle = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.richTextItem = new DevComponents.DotNetBar.Controls.RichTextBoxEx();
            this.SuspendLayout();
            // 
            // txtTitle
            // 
            this.txtTitle.BackColor = System.Drawing.Color.LightPink;
            // 
            // 
            // 
            this.txtTitle.Border.Class = "TextBoxBorder";
            this.txtTitle.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtTitle.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtTitle.HideSelection = false;
            this.txtTitle.Location = new System.Drawing.Point(0, 0);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.ReadOnly = true;
            this.txtTitle.Size = new System.Drawing.Size(150, 26);
            this.txtTitle.TabIndex = 1;
            this.txtTitle.WatermarkColor = System.Drawing.SystemColors.ButtonShadow;
            // 
            // richTextItem
            // 
            // 
            // 
            // 
            this.richTextItem.BackgroundStyle.Class = "RichTextBoxBorder";
            this.richTextItem.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.richTextItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextItem.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richTextItem.Location = new System.Drawing.Point(0, 26);
            this.richTextItem.Name = "richTextItem";
            this.richTextItem.ReadOnly = true;
            this.richTextItem.Size = new System.Drawing.Size(150, 124);
            this.richTextItem.TabIndex = 2;
            // 
            // EErrorItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.richTextItem);
            this.Controls.Add(this.txtTitle);
            this.Name = "EErrorItem";
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX txtTitle;
        private DevComponents.DotNetBar.Controls.RichTextBoxEx richTextItem;
    }
}
