namespace ManagementView
{
    partial class BoxBtnView
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
            this.btnBoxType = new DevComponents.DotNetBar.ButtonX();
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem2 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem3 = new DevComponents.DotNetBar.ButtonItem();
            this.SuspendLayout();
            // 
            // btnBoxType
            // 
            this.btnBoxType.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnBoxType.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.btnBoxType.ColorTable = DevComponents.DotNetBar.eButtonColor.Flat;
            this.btnBoxType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBoxType.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.btnBoxType.Location = new System.Drawing.Point(0, 0);
            this.btnBoxType.Margin = new System.Windows.Forms.Padding(0);
            this.btnBoxType.Name = "btnBoxType";
            this.btnBoxType.PopupSide = DevComponents.DotNetBar.ePopupSide.Right;
            this.btnBoxType.Size = new System.Drawing.Size(151, 42);
            this.btnBoxType.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnBoxType.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItem1,
            this.buttonItem2,
            this.buttonItem3});
            this.btnBoxType.SubItemsExpandWidth = 15;
            this.btnBoxType.TabIndex = 1;
            this.btnBoxType.Text = "测试OK";
            // 
            // buttonItem1
            // 
            this.buttonItem1.GlobalItem = false;
            this.buttonItem1.Name = "buttonItem1";
            this.buttonItem1.Text = "测试OK";
            this.buttonItem1.Click += new System.EventHandler(this.buttonItem1_Click);
            // 
            // buttonItem2
            // 
            this.buttonItem2.GlobalItem = false;
            this.buttonItem2.Name = "buttonItem2";
            this.buttonItem2.Text = "测试NG";
            this.buttonItem2.Click += new System.EventHandler(this.buttonItem2_Click);
            // 
            // buttonItem3
            // 
            this.buttonItem3.GlobalItem = false;
            this.buttonItem3.Name = "buttonItem3";
            this.buttonItem3.Text = "疑似NG";
            this.buttonItem3.Click += new System.EventHandler(this.buttonItem3_Click);
            // 
            // BoxBtnView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnBoxType);
            this.Name = "BoxBtnView";
            this.Size = new System.Drawing.Size(151, 42);
            this.Load += new System.EventHandler(this.BoxBtnView_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnBoxType;
        private DevComponents.DotNetBar.ButtonItem buttonItem1;
        private DevComponents.DotNetBar.ButtonItem buttonItem2;
        private DevComponents.DotNetBar.ButtonItem buttonItem3;
    }
}
