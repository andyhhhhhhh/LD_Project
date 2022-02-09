namespace ManagementView.Comment
{
    partial class SetColorView
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
            this.btnColor = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.chkIsFill = new CCWin.SkinControl.SkinCheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.skinGroupBox1 = new CCWin.SkinControl.SkinGroupBox();
            this.numContourWidth = new System.Windows.Forms.NumericUpDown();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.skinGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numContourWidth)).BeginInit();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnColor
            // 
            this.btnColor.BackColor = System.Drawing.Color.Red;
            this.btnColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnColor.Location = new System.Drawing.Point(73, 20);
            this.btnColor.Name = "btnColor";
            this.btnColor.Size = new System.Drawing.Size(87, 23);
            this.btnColor.TabIndex = 12;
            this.btnColor.UseVisualStyleBackColor = false;
            this.btnColor.Click += new System.EventHandler(this.btnColor_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.ForeColor = System.Drawing.Color.Blue;
            this.label13.Location = new System.Drawing.Point(12, 23);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(35, 17);
            this.label13.TabIndex = 11;
            this.label13.Text = "颜色:";
            // 
            // chkIsFill
            // 
            this.chkIsFill.AutoSize = true;
            this.chkIsFill.BackColor = System.Drawing.Color.Transparent;
            this.chkIsFill.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.chkIsFill.DownBack = null;
            this.chkIsFill.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkIsFill.Location = new System.Drawing.Point(71, 85);
            this.chkIsFill.MouseBack = null;
            this.chkIsFill.Name = "chkIsFill";
            this.chkIsFill.NormlBack = null;
            this.chkIsFill.SelectedDownBack = null;
            this.chkIsFill.SelectedMouseBack = null;
            this.chkIsFill.SelectedNormlBack = null;
            this.chkIsFill.Size = new System.Drawing.Size(99, 21);
            this.chkIsFill.TabIndex = 13;
            this.chkIsFill.Text = "区域填充显示";
            this.chkIsFill.UseVisualStyleBackColor = false;
            this.chkIsFill.CheckedChanged += new System.EventHandler(this.chkIsFill_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(12, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 17);
            this.label1.TabIndex = 11;
            this.label1.Text = "轮廓宽度:";
            // 
            // skinGroupBox1
            // 
            this.skinGroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.skinGroupBox1.BorderColor = System.Drawing.Color.SteelBlue;
            this.skinGroupBox1.Controls.Add(this.numContourWidth);
            this.skinGroupBox1.Controls.Add(this.btnColor);
            this.skinGroupBox1.Controls.Add(this.label13);
            this.skinGroupBox1.Controls.Add(this.chkIsFill);
            this.skinGroupBox1.Controls.Add(this.label1);
            this.skinGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinGroupBox1.ForeColor = System.Drawing.Color.Black;
            this.skinGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.skinGroupBox1.Name = "skinGroupBox1";
            this.skinGroupBox1.RectBackColor = System.Drawing.Color.Transparent;
            this.skinGroupBox1.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinGroupBox1.Size = new System.Drawing.Size(201, 126);
            this.skinGroupBox1.TabIndex = 15;
            this.skinGroupBox1.TabStop = false;
            this.skinGroupBox1.Text = "显示设置";
            this.skinGroupBox1.TitleBorderColor = System.Drawing.Color.Transparent;
            this.skinGroupBox1.TitleRectBackColor = System.Drawing.Color.White;
            this.skinGroupBox1.TitleRoundStyle = CCWin.SkinClass.RoundStyle.None;
            // 
            // numContourWidth
            // 
            this.numContourWidth.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numContourWidth.Location = new System.Drawing.Point(73, 52);
            this.numContourWidth.Name = "numContourWidth";
            this.numContourWidth.Size = new System.Drawing.Size(87, 23);
            this.numContourWidth.TabIndex = 14;
            this.numContourWidth.ValueChanged += new System.EventHandler(this.numContourWidth_ValueChanged);
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.skinGroupBox1);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(201, 126);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 16;
            // 
            // SetColorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Ivory;
            this.Controls.Add(this.panelEx1);
            this.Name = "SetColorView";
            this.Size = new System.Drawing.Size(201, 126);
            this.Load += new System.EventHandler(this.SetColorView_Load);
            this.skinGroupBox1.ResumeLayout(false);
            this.skinGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numContourWidth)).EndInit();
            this.panelEx1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnColor;
        private System.Windows.Forms.Label label13;
        private CCWin.SkinControl.SkinCheckBox chkIsFill;
        private System.Windows.Forms.Label label1;
        private CCWin.SkinControl.SkinGroupBox skinGroupBox1;
        private System.Windows.Forms.NumericUpDown numContourWidth;
        private DevComponents.DotNetBar.PanelEx panelEx1;
    }
}
