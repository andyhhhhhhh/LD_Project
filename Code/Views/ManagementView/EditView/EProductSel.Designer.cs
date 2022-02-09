namespace ManagementView.EditView
{
    partial class EProductSel
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
            this.cmbProduct = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.lblProduct = new DevComponents.DotNetBar.LabelX();
            this.SuspendLayout();
            // 
            // cmbProduct
            // 
            this.cmbProduct.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbProduct.DisplayMember = "Text";
            this.cmbProduct.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbProduct.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbProduct.FormattingEnabled = true;
            this.cmbProduct.ItemHeight = 20;
            this.cmbProduct.Location = new System.Drawing.Point(78, 1);
            this.cmbProduct.Name = "cmbProduct";
            this.cmbProduct.Size = new System.Drawing.Size(158, 26);
            this.cmbProduct.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbProduct.TabIndex = 6;
            this.cmbProduct.SelectedIndexChanged += new System.EventHandler(this.cmbProduct_SelectedIndexChanged);
            // 
            // lblProduct
            // 
            // 
            // 
            // 
            this.lblProduct.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblProduct.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblProduct.Location = new System.Drawing.Point(2, 0);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(70, 28);
            this.lblProduct.TabIndex = 5;
            this.lblProduct.Text = "产品选择:";
            // 
            // EProductSel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbProduct);
            this.Controls.Add(this.lblProduct);
            this.Name = "EProductSel";
            this.Size = new System.Drawing.Size(239, 28);
            this.Load += new System.EventHandler(this.EProductSel_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbProduct;
        private DevComponents.DotNetBar.LabelX lblProduct;
    }
}
