namespace ManagementView.Comment
{
    partial class LogView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogView));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.skinToolTip1 = new CCWin.SkinToolTip(this.components);
            this.navigationPane1 = new DevComponents.DotNetBar.NavigationPane();
            this.navigationPanePanel4 = new DevComponents.DotNetBar.NavigationPanePanel();
            this.panelEx4 = new DevComponents.DotNetBar.PanelEx();
            this.rtbLog_TCP = new DevComponents.DotNetBar.Controls.RichTextBoxEx();
            this.contextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonItem39 = new DevComponents.DotNetBar.ButtonItem();
            this.navigationPanePanel1 = new DevComponents.DotNetBar.NavigationPanePanel();
            this.navigationPanePanel2 = new DevComponents.DotNetBar.NavigationPanePanel();
            this.panelLog = new DevComponents.DotNetBar.PanelEx();
            this.rtbLog_Info = new DevComponents.DotNetBar.Controls.RichTextBoxEx();
            this.buttonItem37 = new DevComponents.DotNetBar.ButtonItem();
            this.navigationPanePanel3 = new DevComponents.DotNetBar.NavigationPanePanel();
            this.panelEx5 = new DevComponents.DotNetBar.PanelEx();
            this.rtbLog_Error = new DevComponents.DotNetBar.Controls.RichTextBoxEx();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonItem38 = new DevComponents.DotNetBar.ButtonItem();
            this.contextMenuStrip1.SuspendLayout();
            this.navigationPane1.SuspendLayout();
            this.navigationPanePanel4.SuspendLayout();
            this.panelEx4.SuspendLayout();
            this.contextMenuStrip3.SuspendLayout();
            this.navigationPanePanel2.SuspendLayout();
            this.panelLog.SuspendLayout();
            this.navigationPanePanel3.SuspendLayout();
            this.panelEx5.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(107, 26);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // skinToolTip1
            // 
            this.skinToolTip1.AutoPopDelay = 5000;
            this.skinToolTip1.InitialDelay = 500;
            this.skinToolTip1.OwnerDraw = true;
            this.skinToolTip1.ReshowDelay = 800;
            // 
            // navigationPane1
            // 
            this.navigationPane1.BackColor = System.Drawing.Color.Transparent;
            this.navigationPane1.Controls.Add(this.navigationPanePanel2);
            this.navigationPane1.Controls.Add(this.navigationPanePanel1);
            this.navigationPane1.Controls.Add(this.navigationPanePanel3);
            this.navigationPane1.Controls.Add(this.navigationPanePanel4);
            this.navigationPane1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navigationPane1.ForeColor = System.Drawing.Color.Black;
            this.navigationPane1.ItemPaddingBottom = -5;
            this.navigationPane1.ItemPaddingTop = 0;
            this.navigationPane1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItem37,
            this.buttonItem38,
            this.buttonItem39});
            this.navigationPane1.Location = new System.Drawing.Point(0, 0);
            this.navigationPane1.Margin = new System.Windows.Forms.Padding(0);
            this.navigationPane1.Name = "navigationPane1";
            this.navigationPane1.NavigationBarHeight = 23;
            this.navigationPane1.Padding = new System.Windows.Forms.Padding(1);
            this.navigationPane1.Size = new System.Drawing.Size(821, 610);
            this.navigationPane1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.navigationPane1.TabIndex = 36;
            this.navigationPane1.TitleButtonAlignment = DevComponents.DotNetBar.eTitleButtonAlignment.Left;
            // 
            // 
            // 
            this.navigationPane1.TitlePanel.CanvasColor = System.Drawing.Color.Transparent;
            this.navigationPane1.TitlePanel.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.navigationPane1.TitlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.navigationPane1.TitlePanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.navigationPane1.TitlePanel.Location = new System.Drawing.Point(1, 1);
            this.navigationPane1.TitlePanel.Margin = new System.Windows.Forms.Padding(0);
            this.navigationPane1.TitlePanel.Name = "panelTitle";
            this.navigationPane1.TitlePanel.Size = new System.Drawing.Size(819, 22);
            this.navigationPane1.TitlePanel.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.navigationPane1.TitlePanel.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.navigationPane1.TitlePanel.Style.Border = DevComponents.DotNetBar.eBorderType.RaisedInner;
            this.navigationPane1.TitlePanel.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(45)))), ((int)(((byte)(150)))));
            this.navigationPane1.TitlePanel.Style.BorderSide = DevComponents.DotNetBar.eBorderSide.Bottom;
            this.navigationPane1.TitlePanel.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.navigationPane1.TitlePanel.Style.GradientAngle = 90;
            this.navigationPane1.TitlePanel.Style.MarginLeft = 4;
            this.navigationPane1.TitlePanel.TabIndex = 0;
            this.navigationPane1.TitlePanel.Text = "提示";
            this.navigationPane1.TitlePanel.Visible = false;
            // 
            // navigationPanePanel4
            // 
            this.navigationPanePanel4.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.navigationPanePanel4.Controls.Add(this.panelEx4);
            this.navigationPanePanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navigationPanePanel4.Location = new System.Drawing.Point(1, 1);
            this.navigationPanePanel4.Name = "navigationPanePanel4";
            this.navigationPanePanel4.ParentItem = this.buttonItem39;
            this.navigationPanePanel4.Size = new System.Drawing.Size(819, 608);
            this.navigationPanePanel4.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.navigationPanePanel4.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.navigationPanePanel4.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.navigationPanePanel4.Style.GradientAngle = 90;
            this.navigationPanePanel4.TabIndex = 5;
            // 
            // panelEx4
            // 
            this.panelEx4.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx4.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx4.Controls.Add(this.rtbLog_TCP);
            this.panelEx4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx4.Location = new System.Drawing.Point(0, 0);
            this.panelEx4.Name = "panelEx4";
            this.panelEx4.Size = new System.Drawing.Size(819, 608);
            this.panelEx4.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx4.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx4.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx4.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx4.Style.GradientAngle = 90;
            this.panelEx4.TabIndex = 0;
            this.panelEx4.Text = "panelEx4";
            // 
            // rtbLog_TCP
            // 
            this.rtbLog_TCP.BackColorRichTextBox = System.Drawing.Color.White;
            // 
            // 
            // 
            this.rtbLog_TCP.BackgroundStyle.Class = "RichTextBoxBorder";
            this.rtbLog_TCP.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.rtbLog_TCP.ContextMenuStrip = this.contextMenuStrip3;
            this.rtbLog_TCP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLog_TCP.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtbLog_TCP.Location = new System.Drawing.Point(0, 0);
            this.rtbLog_TCP.Name = "rtbLog_TCP";
            this.rtbLog_TCP.Size = new System.Drawing.Size(819, 608);
            this.rtbLog_TCP.TabIndex = 1;
            // 
            // contextMenuStrip3
            // 
            this.contextMenuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2});
            this.contextMenuStrip3.Name = "contextMenuStrip1";
            this.contextMenuStrip3.Size = new System.Drawing.Size(107, 26);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(106, 22);
            this.toolStripMenuItem2.Text = "Clear";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // buttonItem39
            // 
            this.buttonItem39.Image = ((System.Drawing.Image)(resources.GetObject("buttonItem39.Image")));
            this.buttonItem39.ImageFixedSize = new System.Drawing.Size(16, 16);
            this.buttonItem39.Name = "buttonItem39";
            this.buttonItem39.OptionGroup = "navBar";
            this.buttonItem39.Text = "信息";
            this.buttonItem39.Tooltip = "信息";
            // 
            // navigationPanePanel1
            // 
            this.navigationPanePanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.navigationPanePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navigationPanePanel1.Location = new System.Drawing.Point(1, 1);
            this.navigationPanePanel1.Name = "navigationPanePanel1";
            this.navigationPanePanel1.ParentItem = null;
            this.navigationPanePanel1.Size = new System.Drawing.Size(819, 585);
            this.navigationPanePanel1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.navigationPanePanel1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.navigationPanePanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.navigationPanePanel1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.navigationPanePanel1.Style.GradientAngle = 90;
            this.navigationPanePanel1.TabIndex = 2;
            // 
            // navigationPanePanel2
            // 
            this.navigationPanePanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.navigationPanePanel2.Controls.Add(this.panelLog);
            this.navigationPanePanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navigationPanePanel2.Location = new System.Drawing.Point(1, 1);
            this.navigationPanePanel2.Name = "navigationPanePanel2";
            this.navigationPanePanel2.ParentItem = this.buttonItem37;
            this.navigationPanePanel2.Size = new System.Drawing.Size(819, 585);
            this.navigationPanePanel2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.navigationPanePanel2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.navigationPanePanel2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.navigationPanePanel2.Style.GradientAngle = 90;
            this.navigationPanePanel2.TabIndex = 3;
            // 
            // panelLog
            // 
            this.panelLog.CanvasColor = System.Drawing.Color.Transparent;
            this.panelLog.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelLog.Controls.Add(this.rtbLog_Info);
            this.panelLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLog.Location = new System.Drawing.Point(0, 0);
            this.panelLog.Name = "panelLog";
            this.panelLog.Size = new System.Drawing.Size(819, 585);
            this.panelLog.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelLog.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelLog.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelLog.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelLog.Style.GradientAngle = 90;
            this.panelLog.TabIndex = 1;
            this.panelLog.Text = "panelEx6";
            // 
            // rtbLog_Info
            // 
            this.rtbLog_Info.BackColorRichTextBox = System.Drawing.Color.White;
            // 
            // 
            // 
            this.rtbLog_Info.BackgroundStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.rtbLog_Info.BackgroundStyle.BackColor2 = System.Drawing.Color.White;
            this.rtbLog_Info.BackgroundStyle.Class = "RichTextBoxBorder";
            this.rtbLog_Info.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.rtbLog_Info.ContextMenuStrip = this.contextMenuStrip1;
            this.rtbLog_Info.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLog_Info.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtbLog_Info.Location = new System.Drawing.Point(0, 0);
            this.rtbLog_Info.Name = "rtbLog_Info";
            this.rtbLog_Info.Size = new System.Drawing.Size(819, 585);
            this.rtbLog_Info.TabIndex = 0;
            // 
            // buttonItem37
            // 
            this.buttonItem37.Checked = true;
            this.buttonItem37.Image = ((System.Drawing.Image)(resources.GetObject("buttonItem37.Image")));
            this.buttonItem37.ImageFixedSize = new System.Drawing.Size(16, 16);
            this.buttonItem37.Name = "buttonItem37";
            this.buttonItem37.OptionGroup = "navBar";
            this.buttonItem37.Text = "提示";
            this.buttonItem37.Tooltip = "提示";
            // 
            // navigationPanePanel3
            // 
            this.navigationPanePanel3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.navigationPanePanel3.Controls.Add(this.panelEx5);
            this.navigationPanePanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navigationPanePanel3.Location = new System.Drawing.Point(1, 1);
            this.navigationPanePanel3.Name = "navigationPanePanel3";
            this.navigationPanePanel3.ParentItem = this.buttonItem38;
            this.navigationPanePanel3.Size = new System.Drawing.Size(819, 585);
            this.navigationPanePanel3.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.navigationPanePanel3.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.navigationPanePanel3.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.navigationPanePanel3.Style.GradientAngle = 90;
            this.navigationPanePanel3.TabIndex = 4;
            // 
            // panelEx5
            // 
            this.panelEx5.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx5.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx5.Controls.Add(this.rtbLog_Error);
            this.panelEx5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx5.Location = new System.Drawing.Point(0, 0);
            this.panelEx5.Name = "panelEx5";
            this.panelEx5.Size = new System.Drawing.Size(819, 585);
            this.panelEx5.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx5.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx5.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx5.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx5.Style.GradientAngle = 90;
            this.panelEx5.TabIndex = 1;
            this.panelEx5.Text = "panelEx5";
            // 
            // rtbLog_Error
            // 
            this.rtbLog_Error.BackColorRichTextBox = System.Drawing.Color.White;
            // 
            // 
            // 
            this.rtbLog_Error.BackgroundStyle.Class = "RichTextBoxBorder";
            this.rtbLog_Error.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.rtbLog_Error.ContextMenuStrip = this.contextMenuStrip2;
            this.rtbLog_Error.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLog_Error.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtbLog_Error.Location = new System.Drawing.Point(0, 0);
            this.rtbLog_Error.Name = "rtbLog_Error";
            this.rtbLog_Error.Size = new System.Drawing.Size(819, 585);
            this.rtbLog_Error.TabIndex = 1;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.contextMenuStrip2.Name = "contextMenuStrip1";
            this.contextMenuStrip2.Size = new System.Drawing.Size(107, 26);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(106, 22);
            this.toolStripMenuItem1.Text = "Clear";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // buttonItem38
            // 
            this.buttonItem38.Image = ((System.Drawing.Image)(resources.GetObject("buttonItem38.Image")));
            this.buttonItem38.ImageFixedSize = new System.Drawing.Size(16, 16);
            this.buttonItem38.Name = "buttonItem38";
            this.buttonItem38.OptionGroup = "navBar";
            this.buttonItem38.Text = "警告";
            this.buttonItem38.Tooltip = "警告";
            // 
            // LogView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.navigationPane1);
            this.Name = "LogView";
            this.Size = new System.Drawing.Size(821, 610);
            this.Load += new System.EventHandler(this.LogView_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.navigationPane1.ResumeLayout(false);
            this.navigationPanePanel4.ResumeLayout(false);
            this.panelEx4.ResumeLayout(false);
            this.contextMenuStrip3.ResumeLayout(false);
            this.navigationPanePanel2.ResumeLayout(false);
            this.panelLog.ResumeLayout(false);
            this.navigationPanePanel3.ResumeLayout(false);
            this.panelEx5.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private CCWin.SkinToolTip skinToolTip1;
        private DevComponents.DotNetBar.NavigationPane navigationPane1;
        private DevComponents.DotNetBar.NavigationPanePanel navigationPanePanel3;
        private DevComponents.DotNetBar.PanelEx panelEx5;
        private DevComponents.DotNetBar.Controls.RichTextBoxEx rtbLog_Error;
        private DevComponents.DotNetBar.ButtonItem buttonItem38;
        private DevComponents.DotNetBar.NavigationPanePanel navigationPanePanel1;
        private DevComponents.DotNetBar.NavigationPanePanel navigationPanePanel4;
        private DevComponents.DotNetBar.PanelEx panelEx4;
        private DevComponents.DotNetBar.Controls.RichTextBoxEx rtbLog_TCP;
        private DevComponents.DotNetBar.ButtonItem buttonItem39;
        private DevComponents.DotNetBar.NavigationPanePanel navigationPanePanel2;
        private DevComponents.DotNetBar.PanelEx panelLog;
        private DevComponents.DotNetBar.Controls.RichTextBoxEx rtbLog_Info;
        private DevComponents.DotNetBar.ButtonItem buttonItem37;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    }
}
