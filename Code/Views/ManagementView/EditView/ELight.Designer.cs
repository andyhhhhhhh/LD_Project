namespace ManagementView.EditView
{
    partial class ELight
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
            this.btnLight = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // btnLight
            // 
            this.btnLight.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLight.BackColor = System.Drawing.Color.Transparent;
            this.btnLight.ColorTable = DevComponents.DotNetBar.eButtonColor.Blue;
            this.btnLight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLight.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLight.Image = global::ManagementView.Properties.Resources.光源控制;
            this.btnLight.ImageFixedSize = new System.Drawing.Size(25, 28);
            this.btnLight.Location = new System.Drawing.Point(0, 0);
            this.btnLight.Name = "btnLight";
            this.btnLight.Size = new System.Drawing.Size(119, 27);
            this.btnLight.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnLight.TabIndex = 4;
            this.btnLight.Text = "光源名称";
            this.btnLight.Click += new System.EventHandler(this.btnLight_Click);
            // 
            // ELight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.btnLight);
            this.Name = "ELight";
            this.Size = new System.Drawing.Size(119, 27);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnLight;
    }
}
