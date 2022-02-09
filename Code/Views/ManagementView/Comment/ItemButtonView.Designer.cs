namespace ManagementView.Comment
{
    partial class ItemButtonView
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
            this.skinToolTip1 = new CCWin.SkinToolTip(this.components);
            this.btnItem = new DevComponents.DotNetBar.RibbonBar();
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            this.SuspendLayout();
            // 
            // skinToolTip1
            // 
            this.skinToolTip1.AutoPopDelay = 5000;
            this.skinToolTip1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.skinToolTip1.Border = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.skinToolTip1.InitialDelay = 500;
            this.skinToolTip1.OwnerDraw = true;
            this.skinToolTip1.ReshowDelay = 800;
            this.skinToolTip1.TipFore = System.Drawing.Color.SeaGreen;
            // 
            // btnItem
            // 
            this.btnItem.AllowDrop = true;
            this.btnItem.AutoOverflowEnabled = true;
            this.btnItem.BackColor = System.Drawing.Color.LightGray;
            // 
            // 
            // 
            this.btnItem.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.btnItem.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.btnItem.ContainerControlProcessDialogKey = true;
            this.btnItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnItem.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnItem.ForeColor = System.Drawing.Color.Black;
            this.btnItem.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItem1});
            this.btnItem.Location = new System.Drawing.Point(0, 0);
            this.btnItem.Margin = new System.Windows.Forms.Padding(0);
            this.btnItem.Name = "btnItem";
            this.btnItem.Size = new System.Drawing.Size(67, 73);
            this.btnItem.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnItem.TabIndex = 5;
            this.btnItem.Text = "采集图像";
            // 
            // 
            // 
            this.btnItem.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.btnItem.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.btnItem.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnItem_MouseDown);
            // 
            // buttonItem1
            // 
            this.buttonItem1.Image = global::ManagementView.Properties.Resources.多图采集;
            this.buttonItem1.ImageFixedSize = new System.Drawing.Size(50, 42);
            this.buttonItem1.Name = "buttonItem1";
            this.buttonItem1.SubItemsExpandWidth = 14;
            // 
            // ItemButtonView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.btnItem);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ItemButtonView";
            this.Size = new System.Drawing.Size(67, 73);
            this.ResumeLayout(false);

        }

        #endregion
        private CCWin.SkinToolTip skinToolTip1;
        private DevComponents.DotNetBar.RibbonBar btnItem;
        private DevComponents.DotNetBar.ButtonItem buttonItem1;
    }
}
