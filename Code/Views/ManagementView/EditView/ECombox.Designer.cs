namespace ManagementView.EditView
{
    partial class ECombox
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
            this.cmbValue = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.lblSetName = new DevComponents.DotNetBar.LabelX();
            this.SuspendLayout();
            // 
            // cmbValue
            // 
            this.cmbValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbValue.DisplayMember = "Text";
            this.cmbValue.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbValue.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbValue.FormattingEnabled = true;
            this.cmbValue.ItemHeight = 20;
            this.cmbValue.Location = new System.Drawing.Point(73, 2);
            this.cmbValue.Name = "cmbValue";
            this.cmbValue.Size = new System.Drawing.Size(131, 26);
            this.cmbValue.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbValue.TabIndex = 8;
            this.cmbValue.SelectedIndexChanged += new System.EventHandler(this.cmbValue_SelectedIndexChanged);
            // 
            // lblSetName
            // 
            // 
            // 
            // 
            this.lblSetName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblSetName.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSetName.Location = new System.Drawing.Point(1, 2);
            this.lblSetName.Name = "lblSetName";
            this.lblSetName.Size = new System.Drawing.Size(79, 27);
            this.lblSetName.TabIndex = 7;
            this.lblSetName.Text = "名称:";
            // 
            // ECombox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbValue);
            this.Controls.Add(this.lblSetName);
            this.Name = "ECombox";
            this.Size = new System.Drawing.Size(207, 29);
            this.Load += new System.EventHandler(this.ECombox_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbValue;
        private DevComponents.DotNetBar.LabelX lblSetName;
    }
}
