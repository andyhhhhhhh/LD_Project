namespace ManagementView.Comment
{
    partial class StatusBarView
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblRunStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblLoginUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTcpStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCommunictionStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblRunTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblMemory = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCPU = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCurrentTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelEx1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.statusStrip1);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(853, 34);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 0;
            this.panelEx1.Text = "panelEx1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.Transparent;
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.lblRunStatus,
            this.lblLoginUser,
            this.lblTcpStatus,
            this.lblCommunictionStatus,
            this.lblRunTime,
            this.lblMemory,
            this.lblCPU,
            this.lblCurrentTime});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(853, 34);
            this.statusStrip1.TabIndex = 29;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.lblStatus.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblStatus.Margin = new System.Windows.Forms.Padding(0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(313, 34);
            this.lblStatus.Spring = true;
            this.lblStatus.Text = "当前工作空间:";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRunStatus
            // 
            this.lblRunStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblRunStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblRunStatus.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRunStatus.Name = "lblRunStatus";
            this.lblRunStatus.Size = new System.Drawing.Size(69, 29);
            this.lblRunStatus.Text = "运行状态";
            // 
            // lblLoginUser
            // 
            this.lblLoginUser.BackColor = System.Drawing.Color.Transparent;
            this.lblLoginUser.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblLoginUser.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblLoginUser.Name = "lblLoginUser";
            this.lblLoginUser.Size = new System.Drawing.Size(72, 29);
            this.lblLoginUser.Text = "登录身份:";
            // 
            // lblTcpStatus
            // 
            this.lblTcpStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblTcpStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblTcpStatus.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTcpStatus.Name = "lblTcpStatus";
            this.lblTcpStatus.Size = new System.Drawing.Size(72, 29);
            this.lblTcpStatus.Text = "通讯状态:";
            // 
            // lblCommunictionStatus
            // 
            this.lblCommunictionStatus.ActiveLinkColor = System.Drawing.Color.Pink;
            this.lblCommunictionStatus.BackColor = System.Drawing.Color.Pink;
            this.lblCommunictionStatus.Margin = new System.Windows.Forms.Padding(-5, 7, 1, 4);
            this.lblCommunictionStatus.Name = "lblCommunictionStatus";
            this.lblCommunictionStatus.Size = new System.Drawing.Size(24, 23);
            this.lblCommunictionStatus.Text = "    ";
            // 
            // lblRunTime
            // 
            this.lblRunTime.BackColor = System.Drawing.Color.Transparent;
            this.lblRunTime.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblRunTime.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRunTime.Name = "lblRunTime";
            this.lblRunTime.Size = new System.Drawing.Size(72, 29);
            this.lblRunTime.Text = "运行时间:";
            // 
            // lblMemory
            // 
            this.lblMemory.BackColor = System.Drawing.Color.Transparent;
            this.lblMemory.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblMemory.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMemory.Name = "lblMemory";
            this.lblMemory.Size = new System.Drawing.Size(72, 29);
            this.lblMemory.Text = "内存占用:";
            // 
            // lblCPU
            // 
            this.lblCPU.BackColor = System.Drawing.Color.Transparent;
            this.lblCPU.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblCPU.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCPU.Name = "lblCPU";
            this.lblCPU.Size = new System.Drawing.Size(48, 29);
            this.lblCPU.Text = "CPU: ";
            // 
            // lblCurrentTime
            // 
            this.lblCurrentTime.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrentTime.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblCurrentTime.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCurrentTime.Name = "lblCurrentTime";
            this.lblCurrentTime.Size = new System.Drawing.Size(69, 29);
            this.lblCurrentTime.Text = "北京时间";
            // 
            // StatusBarView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelEx1);
            this.Name = "StatusBarView";
            this.Size = new System.Drawing.Size(853, 34);
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblRunStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblLoginUser;
        private System.Windows.Forms.ToolStripStatusLabel lblTcpStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblCommunictionStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblRunTime;
        private System.Windows.Forms.ToolStripStatusLabel lblMemory;
        private System.Windows.Forms.ToolStripStatusLabel lblCPU;
        private System.Windows.Forms.ToolStripStatusLabel lblCurrentTime;
    }
}
