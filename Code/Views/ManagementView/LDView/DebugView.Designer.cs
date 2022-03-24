namespace ManagementView
{
    partial class DebugView
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
            this.eLight5 = new ManagementView.EditView.ELight();
            this.eLight4 = new ManagementView.EditView.ELight();
            this.eLight3 = new ManagementView.EditView.ELight();
            this.eLight2 = new ManagementView.EditView.ELight();
            this.eLight1 = new ManagementView.EditView.ELight();
            this.btnUnLoadPos = new DevComponents.DotNetBar.ButtonX();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.btnUnLoadPos);
            this.panelEx1.Controls.Add(this.eLight5);
            this.panelEx1.Controls.Add(this.eLight4);
            this.panelEx1.Controls.Add(this.eLight3);
            this.panelEx1.Controls.Add(this.eLight2);
            this.panelEx1.Controls.Add(this.eLight1);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(915, 479);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 0;
            // 
            // eLight5
            // 
            this.eLight5.BackColor = System.Drawing.SystemColors.Control;
            this.eLight5.CloseText = "SD0000#";
            this.eLight5.ComName = null;
            this.eLight5.Location = new System.Drawing.Point(25, 253);
            this.eLight5.LText = "HR检测相机光源";
            this.eLight5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.eLight5.Name = "eLight5";
            this.eLight5.OpenText = "SD0255#";
            this.eLight5.Size = new System.Drawing.Size(288, 38);
            this.eLight5.TabIndex = 0;
            // 
            // eLight4
            // 
            this.eLight4.BackColor = System.Drawing.SystemColors.Control;
            this.eLight4.CloseText = "SE0000#";
            this.eLight4.ComName = null;
            this.eLight4.Location = new System.Drawing.Point(25, 196);
            this.eLight4.LText = "AR检测相机光源";
            this.eLight4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.eLight4.Name = "eLight4";
            this.eLight4.OpenText = "SE0255#";
            this.eLight4.Size = new System.Drawing.Size(288, 38);
            this.eLight4.TabIndex = 0;
            // 
            // eLight3
            // 
            this.eLight3.BackColor = System.Drawing.SystemColors.Control;
            this.eLight3.CloseText = "SB0000#";
            this.eLight3.ComName = null;
            this.eLight3.Location = new System.Drawing.Point(25, 139);
            this.eLight3.LText = "检测上相机光源";
            this.eLight3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.eLight3.Name = "eLight3";
            this.eLight3.OpenText = "SB0255#";
            this.eLight3.Size = new System.Drawing.Size(288, 38);
            this.eLight3.TabIndex = 0;
            // 
            // eLight2
            // 
            this.eLight2.BackColor = System.Drawing.SystemColors.Control;
            this.eLight2.CloseText = "SC0000#";
            this.eLight2.ComName = null;
            this.eLight2.Location = new System.Drawing.Point(25, 82);
            this.eLight2.LText = "N面下相机光源";
            this.eLight2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.eLight2.Name = "eLight2";
            this.eLight2.OpenText = "SC0255#";
            this.eLight2.Size = new System.Drawing.Size(288, 38);
            this.eLight2.TabIndex = 0;
            // 
            // eLight1
            // 
            this.eLight1.BackColor = System.Drawing.SystemColors.Control;
            this.eLight1.CloseText = "SA0000#";
            this.eLight1.ComName = null;
            this.eLight1.Location = new System.Drawing.Point(25, 25);
            this.eLight1.LText = "小视野相机光源";
            this.eLight1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.eLight1.Name = "eLight1";
            this.eLight1.OpenText = "SA0255#";
            this.eLight1.Size = new System.Drawing.Size(288, 38);
            this.eLight1.TabIndex = 0;
            // 
            // btnUnLoadPos
            // 
            this.btnUnLoadPos.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnUnLoadPos.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnUnLoadPos.Location = new System.Drawing.Point(524, 25);
            this.btnUnLoadPos.Name = "btnUnLoadPos";
            this.btnUnLoadPos.Size = new System.Drawing.Size(264, 38);
            this.btnUnLoadPos.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnUnLoadPos.TabIndex = 1;
            this.btnUnLoadPos.Text = "下料盘下料位置";
            this.btnUnLoadPos.Click += new System.EventHandler(this.btnUnLoadPos_Click);
            // 
            // DebugView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelEx1);
            this.Name = "DebugView";
            this.Size = new System.Drawing.Size(915, 479);
            this.Load += new System.EventHandler(this.DebugView_Load);
            this.panelEx1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private EditView.ELight eLight1;
        private EditView.ELight eLight5;
        private EditView.ELight eLight4;
        private EditView.ELight eLight3;
        private EditView.ELight eLight2;
        private DevComponents.DotNetBar.ButtonX btnUnLoadPos;
    }
}
