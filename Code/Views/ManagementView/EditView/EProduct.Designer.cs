namespace ManagementView.EditView
{
    partial class EProduct
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
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.txtBoxCode = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.cmbSevenTitle = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX1.Location = new System.Drawing.Point(3, 35);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(52, 28);
            this.labelX1.TabIndex = 3;
            this.labelX1.Text = "彩盒码:";
            // 
            // txtBoxCode
            // 
            // 
            // 
            // 
            this.txtBoxCode.Border.Class = "TextBoxBorder";
            this.txtBoxCode.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtBoxCode.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBoxCode.Location = new System.Drawing.Point(64, 37);
            this.txtBoxCode.Multiline = true;
            this.txtBoxCode.Name = "txtBoxCode";
            this.txtBoxCode.Size = new System.Drawing.Size(150, 26);
            this.txtBoxCode.TabIndex = 2;
            // 
            // cmbSevenTitle
            // 
            this.cmbSevenTitle.DisplayMember = "Text";
            this.cmbSevenTitle.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbSevenTitle.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbSevenTitle.FormattingEnabled = true;
            this.cmbSevenTitle.ItemHeight = 20;
            this.cmbSevenTitle.Location = new System.Drawing.Point(64, 4);
            this.cmbSevenTitle.Name = "cmbSevenTitle";
            this.cmbSevenTitle.Size = new System.Drawing.Size(150, 26);
            this.cmbSevenTitle.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbSevenTitle.TabIndex = 4;
            this.cmbSevenTitle.SelectedIndexChanged += new System.EventHandler(this.cmbSevenTitle_SelectedIndexChanged);
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX2.Location = new System.Drawing.Point(3, 4);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(60, 28);
            this.labelX2.TabIndex = 3;
            this.labelX2.Text = "七字头:";
            // 
            // EComboProduct
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbSevenTitle);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.txtBoxCode);
            this.Name = "EComboProduct";
            this.Size = new System.Drawing.Size(218, 66);
            this.Load += new System.EventHandler(this.EComboProduct_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtBoxCode;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbSevenTitle;
        private DevComponents.DotNetBar.LabelX labelX2;
    }
}
