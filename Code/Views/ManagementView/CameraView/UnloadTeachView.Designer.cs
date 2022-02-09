namespace ManagementView
{
    partial class UnloadTeachView
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
            this.btnAlgorithm = new DevComponents.DotNetBar.ButtonX();
            this.btnLoadImage = new DevComponents.DotNetBar.ButtonX();
            this.btnMovePos = new DevComponents.DotNetBar.ButtonX();
            this.cmbSuck = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.panelBtn = new DevComponents.DotNetBar.PanelEx();
            this.btnSnap = new DevComponents.DotNetBar.ButtonX();
            this.cmbCamera = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.logView1 = new ManagementView.Comment.LogView();
            this.panelView = new DevComponents.DotNetBar.PanelEx();
            this.panelParam = new DevComponents.DotNetBar.PanelEx();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.btnGetTeachPos = new DevComponents.DotNetBar.ButtonX();
            this.cmbDownSelect = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.txtUnLoadAng = new ManagementView.Comment.TextInput();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.txtUnLoadY = new ManagementView.Comment.TextInput();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.txtUnLoadX = new ManagementView.Comment.TextInput();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelBtn.SuspendLayout();
            this.panelParam.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAlgorithm
            // 
            this.btnAlgorithm.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAlgorithm.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAlgorithm.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAlgorithm.Location = new System.Drawing.Point(339, 66);
            this.btnAlgorithm.Name = "btnAlgorithm";
            this.btnAlgorithm.Size = new System.Drawing.Size(92, 44);
            this.btnAlgorithm.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnAlgorithm.TabIndex = 0;
            this.btnAlgorithm.Text = "执行算法";
            this.btnAlgorithm.Click += new System.EventHandler(this.btnAlgorithm_Click);
            // 
            // btnLoadImage
            // 
            this.btnLoadImage.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLoadImage.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnLoadImage.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLoadImage.Location = new System.Drawing.Point(202, 66);
            this.btnLoadImage.Name = "btnLoadImage";
            this.btnLoadImage.Size = new System.Drawing.Size(92, 44);
            this.btnLoadImage.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnLoadImage.TabIndex = 0;
            this.btnLoadImage.Text = "加载图片";
            this.btnLoadImage.Click += new System.EventHandler(this.btnLoadImage_Click);
            // 
            // btnMovePos
            // 
            this.btnMovePos.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnMovePos.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnMovePos.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMovePos.Location = new System.Drawing.Point(287, 143);
            this.btnMovePos.Name = "btnMovePos";
            this.btnMovePos.Size = new System.Drawing.Size(113, 36);
            this.btnMovePos.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnMovePos.TabIndex = 0;
            this.btnMovePos.Text = "移动到料盘位置";
            this.btnMovePos.Click += new System.EventHandler(this.btnMovePos_Click);
            // 
            // cmbSuck
            // 
            this.cmbSuck.DisplayMember = "Text";
            this.cmbSuck.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbSuck.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbSuck.FormattingEnabled = true;
            this.cmbSuck.ItemHeight = 17;
            this.cmbSuck.Location = new System.Drawing.Point(112, 113);
            this.cmbSuck.Name = "cmbSuck";
            this.cmbSuck.Size = new System.Drawing.Size(124, 23);
            this.cmbSuck.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbSuck.TabIndex = 75;
            // 
            // panelBtn
            // 
            this.panelBtn.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelBtn.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelBtn.Controls.Add(this.btnAlgorithm);
            this.panelBtn.Controls.Add(this.btnSnap);
            this.panelBtn.Controls.Add(this.btnLoadImage);
            this.panelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBtn.Location = new System.Drawing.Point(657, 484);
            this.panelBtn.Margin = new System.Windows.Forms.Padding(1);
            this.panelBtn.Name = "panelBtn";
            this.panelBtn.Size = new System.Drawing.Size(469, 175);
            this.panelBtn.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelBtn.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelBtn.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelBtn.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelBtn.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelBtn.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelBtn.Style.GradientAngle = 90;
            this.panelBtn.TabIndex = 1;
            // 
            // btnSnap
            // 
            this.btnSnap.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSnap.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSnap.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSnap.Location = new System.Drawing.Point(42, 66);
            this.btnSnap.Name = "btnSnap";
            this.btnSnap.Size = new System.Drawing.Size(92, 44);
            this.btnSnap.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnSnap.TabIndex = 0;
            this.btnSnap.Text = "拍照";
            this.btnSnap.Click += new System.EventHandler(this.btnSnap_Click);
            // 
            // cmbCamera
            // 
            this.cmbCamera.DisplayMember = "Text";
            this.cmbCamera.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbCamera.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbCamera.FormattingEnabled = true;
            this.cmbCamera.ItemHeight = 17;
            this.cmbCamera.Location = new System.Drawing.Point(112, 70);
            this.cmbCamera.Name = "cmbCamera";
            this.cmbCamera.Size = new System.Drawing.Size(124, 23);
            this.cmbCamera.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbCamera.TabIndex = 77;
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX2.Location = new System.Drawing.Point(21, 70);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(80, 23);
            this.labelX2.TabIndex = 76;
            this.labelX2.Text = "相机选择:";
            // 
            // labelX3
            // 
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX3.Location = new System.Drawing.Point(21, 113);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(80, 23);
            this.labelX3.TabIndex = 0;
            this.labelX3.Text = "吸嘴选择：";
            // 
            // logView1
            // 
            this.logView1.BackColor = System.Drawing.Color.Transparent;
            this.logView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logView1.Location = new System.Drawing.Point(1, 484);
            this.logView1.Margin = new System.Windows.Forms.Padding(1);
            this.logView1.Name = "logView1";
            this.logView1.Size = new System.Drawing.Size(654, 175);
            this.logView1.TabIndex = 0;
            // 
            // panelView
            // 
            this.panelView.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelView.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelView.Location = new System.Drawing.Point(1, 1);
            this.panelView.Margin = new System.Windows.Forms.Padding(1);
            this.panelView.Name = "panelView";
            this.panelView.Size = new System.Drawing.Size(654, 481);
            this.panelView.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelView.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelView.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelView.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelView.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelView.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelView.Style.GradientAngle = 90;
            this.panelView.TabIndex = 1;
            // 
            // panelParam
            // 
            this.panelParam.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelParam.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelParam.Controls.Add(this.groupPanel1);
            this.panelParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelParam.Location = new System.Drawing.Point(657, 1);
            this.panelParam.Margin = new System.Windows.Forms.Padding(1);
            this.panelParam.Name = "panelParam";
            this.panelParam.Size = new System.Drawing.Size(469, 481);
            this.panelParam.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelParam.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelParam.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelParam.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelParam.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelParam.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelParam.Style.GradientAngle = 90;
            this.panelParam.TabIndex = 1;
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.txtUnLoadX);
            this.groupPanel1.Controls.Add(this.cmbCamera);
            this.groupPanel1.Controls.Add(this.btnGetTeachPos);
            this.groupPanel1.Controls.Add(this.labelX2);
            this.groupPanel1.Controls.Add(this.cmbDownSelect);
            this.groupPanel1.Controls.Add(this.labelX3);
            this.groupPanel1.Controls.Add(this.cmbSuck);
            this.groupPanel1.Controls.Add(this.labelX4);
            this.groupPanel1.Controls.Add(this.txtUnLoadAng);
            this.groupPanel1.Controls.Add(this.labelX5);
            this.groupPanel1.Controls.Add(this.txtUnLoadY);
            this.groupPanel1.Controls.Add(this.labelX6);
            this.groupPanel1.Controls.Add(this.btnMovePos);
            this.groupPanel1.Controls.Add(this.labelX1);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupPanel1.Location = new System.Drawing.Point(0, 0);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(469, 481);
            // 
            // 
            // 
            this.groupPanel1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel1.Style.BackColorGradientAngle = 90;
            this.groupPanel1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderBottomWidth = 1;
            this.groupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderLeftWidth = 1;
            this.groupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderRightWidth = 1;
            this.groupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderTopWidth = 1;
            this.groupPanel1.Style.CornerDiameter = 4;
            this.groupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel1.TabIndex = 76;
            this.groupPanel1.Text = "下料示教";
            // 
            // btnGetTeachPos
            // 
            this.btnGetTeachPos.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnGetTeachPos.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnGetTeachPos.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGetTeachPos.Location = new System.Drawing.Point(287, 284);
            this.btnGetTeachPos.Name = "btnGetTeachPos";
            this.btnGetTeachPos.Size = new System.Drawing.Size(113, 33);
            this.btnGetTeachPos.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnGetTeachPos.TabIndex = 0;
            this.btnGetTeachPos.Text = "获取示教位置";
            this.btnGetTeachPos.Click += new System.EventHandler(this.btnGetTeachPos_Click);
            // 
            // cmbDownSelect
            // 
            this.cmbDownSelect.DisplayMember = "Text";
            this.cmbDownSelect.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbDownSelect.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbDownSelect.FormattingEnabled = true;
            this.cmbDownSelect.ItemHeight = 17;
            this.cmbDownSelect.Location = new System.Drawing.Point(112, 159);
            this.cmbDownSelect.Name = "cmbDownSelect";
            this.cmbDownSelect.Size = new System.Drawing.Size(124, 23);
            this.cmbDownSelect.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbDownSelect.TabIndex = 75;
            // 
            // labelX4
            // 
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX4.Location = new System.Drawing.Point(21, 159);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(80, 23);
            this.labelX4.TabIndex = 0;
            this.labelX4.Text = "下料盘选择：";
            // 
            // txtUnLoadAng
            // 
            this.txtUnLoadAng.BackColor = System.Drawing.Color.White;
            this.txtUnLoadAng.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUnLoadAng.IsMultiLine = false;
            this.txtUnLoadAng.IsPassword = false;
            this.txtUnLoadAng.Location = new System.Drawing.Point(112, 295);
            this.txtUnLoadAng.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtUnLoadAng.Name = "txtUnLoadAng";
            this.txtUnLoadAng.Size = new System.Drawing.Size(124, 26);
            this.txtUnLoadAng.sText = "";
            this.txtUnLoadAng.TabIndex = 66;
            // 
            // labelX5
            // 
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX5.Location = new System.Drawing.Point(21, 205);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(92, 23);
            this.labelX5.TabIndex = 0;
            this.labelX5.Text = "下料位置X：";
            // 
            // txtUnLoadY
            // 
            this.txtUnLoadY.BackColor = System.Drawing.Color.White;
            this.txtUnLoadY.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUnLoadY.IsMultiLine = false;
            this.txtUnLoadY.IsPassword = false;
            this.txtUnLoadY.Location = new System.Drawing.Point(112, 249);
            this.txtUnLoadY.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtUnLoadY.Name = "txtUnLoadY";
            this.txtUnLoadY.Size = new System.Drawing.Size(124, 26);
            this.txtUnLoadY.sText = "";
            this.txtUnLoadY.TabIndex = 66;
            // 
            // labelX6
            // 
            // 
            // 
            // 
            this.labelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX6.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX6.Location = new System.Drawing.Point(21, 251);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(92, 23);
            this.labelX6.TabIndex = 0;
            this.labelX6.Text = "下料位置Y:";
            // 
            // txtUnLoadX
            // 
            this.txtUnLoadX.BackColor = System.Drawing.Color.White;
            this.txtUnLoadX.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUnLoadX.IsMultiLine = false;
            this.txtUnLoadX.IsPassword = false;
            this.txtUnLoadX.Location = new System.Drawing.Point(112, 203);
            this.txtUnLoadX.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtUnLoadX.Name = "txtUnLoadX";
            this.txtUnLoadX.Size = new System.Drawing.Size(124, 26);
            this.txtUnLoadX.sText = "";
            this.txtUnLoadX.TabIndex = 66;
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX1.Location = new System.Drawing.Point(21, 297);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(92, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "下料位置角度:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58.22034F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.77966F));
            this.tableLayoutPanel1.Controls.Add(this.logView1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panelView, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelParam, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelBtn, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 73.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 26.71428F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1127, 660);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // UnloadTeachView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UnloadTeachView";
            this.Size = new System.Drawing.Size(1127, 660);
            this.Load += new System.EventHandler(this.UnloadTeachView_Load);
            this.panelBtn.ResumeLayout(false);
            this.panelParam.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnAlgorithm;
        private DevComponents.DotNetBar.ButtonX btnLoadImage;
        private DevComponents.DotNetBar.ButtonX btnMovePos;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbSuck;
        private DevComponents.DotNetBar.PanelEx panelBtn;
        private DevComponents.DotNetBar.ButtonX btnSnap;
        private DevComponents.DotNetBar.LabelX labelX3;
        private Comment.LogView logView1;
        private DevComponents.DotNetBar.PanelEx panelView;
        private DevComponents.DotNetBar.PanelEx panelParam;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbDownSelect;
        private DevComponents.DotNetBar.LabelX labelX4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Comment.TextInput txtUnLoadY;
        private Comment.TextInput txtUnLoadX;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.ButtonX btnGetTeachPos;
        private Comment.TextInput txtUnLoadAng;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbCamera;
        private DevComponents.DotNetBar.LabelX labelX2;
    }
}
