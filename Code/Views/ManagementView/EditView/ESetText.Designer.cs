namespace ManagementView.EditView
{
    partial class ESetText
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
            this.lblSetName = new DevComponents.DotNetBar.LabelX();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.txtSetValue = new ManagementView.Comment.TextInput();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSetName
            // 
            // 
            // 
            // 
            this.lblSetName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lblSetName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSetName.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSetName.Location = new System.Drawing.Point(0, 0);
            this.lblSetName.Margin = new System.Windows.Forms.Padding(0);
            this.lblSetName.Name = "lblSetName";
            this.lblSetName.Size = new System.Drawing.Size(70, 27);
            this.lblSetName.TabIndex = 1;
            this.lblSetName.Text = "设置项:";
            // 
            // timer1
            // 
            this.timer1.Interval = 300;
            // 
            // txtSetValue
            // 
            this.txtSetValue.BackColor = System.Drawing.Color.AliceBlue;
            this.txtSetValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSetValue.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSetValue.Location = new System.Drawing.Point(70, 0);
            this.txtSetValue.Margin = new System.Windows.Forms.Padding(0);
            this.txtSetValue.Name = "txtSetValue";
            this.txtSetValue.Size = new System.Drawing.Size(166, 27);
            this.txtSetValue.sText = "";
            this.txtSetValue.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.txtSetValue, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblSetName, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(236, 27);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // ESetText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ESetText";
            this.Size = new System.Drawing.Size(236, 27);
            this.Load += new System.EventHandler(this.ESetText_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevComponents.DotNetBar.LabelX lblSetName;
        private System.Windows.Forms.Timer timer1;
        private Comment.TextInput txtSetValue;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
