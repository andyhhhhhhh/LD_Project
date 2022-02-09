namespace ManagementView.MotorView
{
    partial class OutInPutView
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
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.inPutIoView2 = new ManagementView.MotorView.InPutIoView();
            this.inPutIoView1 = new ManagementView.MotorView.InPutIoView();
            this.outPutIoView1 = new ManagementView.MotorView.OutPutIoView();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.outPutIoView1);
            this.panelEx1.Controls.Add(this.inPutIoView2);
            this.panelEx1.Controls.Add(this.inPutIoView1);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(437, 31);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 0;
            // 
            // inPutIoView2
            // 
            this.inPutIoView2.IoModel = null;
            this.inPutIoView2.Location = new System.Drawing.Point(288, 0);
            this.inPutIoView2.Margin = new System.Windows.Forms.Padding(0);
            this.inPutIoView2.Monitor = false;
            this.inPutIoView2.Name = "inPutIoView2";
            this.inPutIoView2.Size = new System.Drawing.Size(148, 31);
            this.inPutIoView2.TabIndex = 1;
            // 
            // inPutIoView1
            // 
            this.inPutIoView1.IoModel = null;
            this.inPutIoView1.Location = new System.Drawing.Point(139, 0);
            this.inPutIoView1.Margin = new System.Windows.Forms.Padding(0);
            this.inPutIoView1.Monitor = false;
            this.inPutIoView1.Name = "inPutIoView1";
            this.inPutIoView1.Size = new System.Drawing.Size(149, 31);
            this.inPutIoView1.TabIndex = 1;
            // 
            // outPutIoView1
            // 
            this.outPutIoView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.outPutIoView1.IoModel = null;
            this.outPutIoView1.Location = new System.Drawing.Point(0, 0);
            this.outPutIoView1.Name = "outPutIoView1";
            this.outPutIoView1.Size = new System.Drawing.Size(139, 31);
            this.outPutIoView1.TabIndex = 2;
            // 
            // OutInPutView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelEx1);
            this.Name = "OutInPutView";
            this.Size = new System.Drawing.Size(437, 31);
            this.Load += new System.EventHandler(this.OutInPutView_Load);
            this.panelEx1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private InPutIoView inPutIoView1;
        private InPutIoView inPutIoView2;
        private OutPutIoView outPutIoView1;
    }
}
