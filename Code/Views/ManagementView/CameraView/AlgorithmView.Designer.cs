namespace ManagementView
{
    partial class AlgorithmView
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.logView1 = new ManagementView.Comment.LogView();
            this.panelView1 = new DevComponents.DotNetBar.PanelEx();
            this.panelView2 = new DevComponents.DotNetBar.PanelEx();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ParamNameBoxCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParamValueBoxCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelEx2 = new DevComponents.DotNetBar.PanelEx();
            this.chkIsPathTest = new System.Windows.Forms.CheckBox();
            this.loadPath = new ManagementView._3DViews.CommonView.LoadPathView();
            this.cmbCamera = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.label7 = new System.Windows.Forms.Label();
            this.btn_DrawSearchRegion = new System.Windows.Forms.Button();
            this.btn_SaveParam = new System.Windows.Forms.Button();
            this.btnCycleRun = new System.Windows.Forms.Button();
            this.btn_AutoRun = new System.Windows.Forms.Button();
            this.btn_DrawModelRegion = new System.Windows.Forms.Button();
            this.btn_ReadImage = new System.Windows.Forms.Button();
            this.btn_DrawLine4 = new System.Windows.Forms.Button();
            this.btn_DrawOCRRegion = new System.Windows.Forms.Button();
            this.btnSnap = new System.Windows.Forms.Button();
            this.btnFindModel = new System.Windows.Forms.Button();
            this.btn_CreateModel = new System.Windows.Forms.Button();
            this.btn_CleanReDraw = new System.Windows.Forms.Button();
            this.panelEx1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panelEx2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.tableLayoutPanel1);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(1180, 700);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 480F));
            this.tableLayoutPanel1.Controls.Add(this.logView1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panelView1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelView2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelEx2, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1180, 700);
            this.tableLayoutPanel1.TabIndex = 45;
            // 
            // logView1
            // 
            this.logView1.BackColor = System.Drawing.Color.Transparent;
            this.logView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logView1.Location = new System.Drawing.Point(1, 541);
            this.logView1.Margin = new System.Windows.Forms.Padding(1);
            this.logView1.Name = "logView1";
            this.logView1.Size = new System.Drawing.Size(698, 158);
            this.logView1.TabIndex = 46;
            // 
            // panelView1
            // 
            this.panelView1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelView1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelView1.Location = new System.Drawing.Point(1, 1);
            this.panelView1.Margin = new System.Windows.Forms.Padding(1);
            this.panelView1.Name = "panelView1";
            this.panelView1.Size = new System.Drawing.Size(698, 368);
            this.panelView1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelView1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelView1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelView1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelView1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelView1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelView1.Style.GradientAngle = 90;
            this.panelView1.TabIndex = 44;
            // 
            // panelView2
            // 
            this.panelView2.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelView2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelView2.Location = new System.Drawing.Point(1, 371);
            this.panelView2.Margin = new System.Windows.Forms.Padding(1);
            this.panelView2.Name = "panelView2";
            this.panelView2.Size = new System.Drawing.Size(698, 168);
            this.panelView2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelView2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelView2.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelView2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelView2.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelView2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelView2.Style.GradientAngle = 90;
            this.panelView2.TabIndex = 44;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ParamNameBoxCol,
            this.ParamValueBoxCol});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(701, 1);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(1);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(478, 368);
            this.dataGridView1.TabIndex = 32;
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
            // 
            // ParamNameBoxCol
            // 
            this.ParamNameBoxCol.HeaderText = "参数名称";
            this.ParamNameBoxCol.Name = "ParamNameBoxCol";
            // 
            // ParamValueBoxCol
            // 
            this.ParamValueBoxCol.HeaderText = "参数值";
            this.ParamValueBoxCol.Name = "ParamValueBoxCol";
            this.ParamValueBoxCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // panelEx2
            // 
            this.panelEx2.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx2.Controls.Add(this.chkIsPathTest);
            this.panelEx2.Controls.Add(this.loadPath);
            this.panelEx2.Controls.Add(this.cmbCamera);
            this.panelEx2.Controls.Add(this.label7);
            this.panelEx2.Controls.Add(this.btn_DrawSearchRegion);
            this.panelEx2.Controls.Add(this.btn_SaveParam);
            this.panelEx2.Controls.Add(this.btnCycleRun);
            this.panelEx2.Controls.Add(this.btn_AutoRun);
            this.panelEx2.Controls.Add(this.btn_DrawModelRegion);
            this.panelEx2.Controls.Add(this.btn_ReadImage);
            this.panelEx2.Controls.Add(this.btn_DrawLine4);
            this.panelEx2.Controls.Add(this.btn_DrawOCRRegion);
            this.panelEx2.Controls.Add(this.btnSnap);
            this.panelEx2.Controls.Add(this.btnFindModel);
            this.panelEx2.Controls.Add(this.btn_CreateModel);
            this.panelEx2.Controls.Add(this.btn_CleanReDraw);
            this.panelEx2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx2.Location = new System.Drawing.Point(701, 371);
            this.panelEx2.Margin = new System.Windows.Forms.Padding(1);
            this.panelEx2.Name = "panelEx2";
            this.tableLayoutPanel1.SetRowSpan(this.panelEx2, 2);
            this.panelEx2.Size = new System.Drawing.Size(478, 328);
            this.panelEx2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx2.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx2.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx2.Style.GradientAngle = 90;
            this.panelEx2.TabIndex = 45;
            // 
            // chkIsPathTest
            // 
            this.chkIsPathTest.AutoSize = true;
            this.chkIsPathTest.Location = new System.Drawing.Point(380, 297);
            this.chkIsPathTest.Name = "chkIsPathTest";
            this.chkIsPathTest.Size = new System.Drawing.Size(84, 16);
            this.chkIsPathTest.TabIndex = 76;
            this.chkIsPathTest.Text = "文件夹测试";
            this.chkIsPathTest.UseVisualStyleBackColor = true;
            // 
            // loadPath
            // 
            this.loadPath.FolderName = null;
            this.loadPath.FolderPath = null;
            this.loadPath.Location = new System.Drawing.Point(11, 291);
            this.loadPath.Name = "loadPath";
            this.loadPath.Size = new System.Drawing.Size(358, 29);
            this.loadPath.TabIndex = 75;
            // 
            // cmbCamera
            // 
            this.cmbCamera.DisplayMember = "Text";
            this.cmbCamera.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbCamera.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbCamera.FormattingEnabled = true;
            this.cmbCamera.ItemHeight = 17;
            this.cmbCamera.Location = new System.Drawing.Point(78, 14);
            this.cmbCamera.Name = "cmbCamera";
            this.cmbCamera.Size = new System.Drawing.Size(133, 23);
            this.cmbCamera.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbCamera.TabIndex = 73;
            this.cmbCamera.SelectedIndexChanged += new System.EventHandler(this.cmbCamera_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(11, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 74;
            this.label7.Text = "相机选择:";
            // 
            // btn_DrawSearchRegion
            // 
            this.btn_DrawSearchRegion.Location = new System.Drawing.Point(257, 143);
            this.btn_DrawSearchRegion.Name = "btn_DrawSearchRegion";
            this.btn_DrawSearchRegion.Size = new System.Drawing.Size(116, 50);
            this.btn_DrawSearchRegion.TabIndex = 34;
            this.btn_DrawSearchRegion.Text = "绘制搜索区域";
            this.btn_DrawSearchRegion.UseVisualStyleBackColor = true;
            this.btn_DrawSearchRegion.Click += new System.EventHandler(this.btn_DrawSearchRegion_Click);
            // 
            // btn_SaveParam
            // 
            this.btn_SaveParam.Location = new System.Drawing.Point(257, 231);
            this.btn_SaveParam.Name = "btn_SaveParam";
            this.btn_SaveParam.Size = new System.Drawing.Size(116, 50);
            this.btn_SaveParam.TabIndex = 31;
            this.btn_SaveParam.Text = "保存参数";
            this.btn_SaveParam.UseVisualStyleBackColor = true;
            this.btn_SaveParam.Click += new System.EventHandler(this.btn_SaveParam_Click);
            // 
            // btnCycleRun
            // 
            this.btnCycleRun.Location = new System.Drawing.Point(380, 231);
            this.btnCycleRun.Name = "btnCycleRun";
            this.btnCycleRun.Size = new System.Drawing.Size(86, 48);
            this.btnCycleRun.TabIndex = 41;
            this.btnCycleRun.Text = "循环测试";
            this.btnCycleRun.UseVisualStyleBackColor = true;
            this.btnCycleRun.Click += new System.EventHandler(this.btnCycleRun_Click);
            // 
            // btn_AutoRun
            // 
            this.btn_AutoRun.Location = new System.Drawing.Point(135, 231);
            this.btn_AutoRun.Name = "btn_AutoRun";
            this.btn_AutoRun.Size = new System.Drawing.Size(116, 50);
            this.btn_AutoRun.TabIndex = 41;
            this.btn_AutoRun.Text = "自动测试";
            this.btn_AutoRun.UseVisualStyleBackColor = true;
            this.btn_AutoRun.Click += new System.EventHandler(this.btn_AutoRun_Click);
            // 
            // btn_DrawModelRegion
            // 
            this.btn_DrawModelRegion.Location = new System.Drawing.Point(133, 57);
            this.btn_DrawModelRegion.Name = "btn_DrawModelRegion";
            this.btn_DrawModelRegion.Size = new System.Drawing.Size(116, 50);
            this.btn_DrawModelRegion.TabIndex = 33;
            this.btn_DrawModelRegion.Text = "绘制创建模板区域";
            this.btn_DrawModelRegion.UseVisualStyleBackColor = true;
            this.btn_DrawModelRegion.Click += new System.EventHandler(this.btn_DrawModelRegion_Click);
            // 
            // btn_ReadImage
            // 
            this.btn_ReadImage.Location = new System.Drawing.Point(11, 57);
            this.btn_ReadImage.Name = "btn_ReadImage";
            this.btn_ReadImage.Size = new System.Drawing.Size(116, 50);
            this.btn_ReadImage.TabIndex = 40;
            this.btn_ReadImage.Text = "读取图像";
            this.btn_ReadImage.UseVisualStyleBackColor = true;
            this.btn_ReadImage.Click += new System.EventHandler(this.btn_ReadImage_Click);
            // 
            // btn_DrawLine4
            // 
            this.btn_DrawLine4.Location = new System.Drawing.Point(135, 143);
            this.btn_DrawLine4.Name = "btn_DrawLine4";
            this.btn_DrawLine4.Size = new System.Drawing.Size(116, 50);
            this.btn_DrawLine4.TabIndex = 35;
            this.btn_DrawLine4.Text = "绘制四条边线位置";
            this.btn_DrawLine4.UseVisualStyleBackColor = true;
            this.btn_DrawLine4.Click += new System.EventHandler(this.btn_DrawLine4_Click);
            // 
            // btn_DrawOCRRegion
            // 
            this.btn_DrawOCRRegion.Location = new System.Drawing.Point(13, 143);
            this.btn_DrawOCRRegion.Name = "btn_DrawOCRRegion";
            this.btn_DrawOCRRegion.Size = new System.Drawing.Size(116, 50);
            this.btn_DrawOCRRegion.TabIndex = 39;
            this.btn_DrawOCRRegion.Text = "绘制字符区域";
            this.btn_DrawOCRRegion.UseVisualStyleBackColor = true;
            this.btn_DrawOCRRegion.Click += new System.EventHandler(this.btn_DrawOCRRegion_Click);
            // 
            // btnSnap
            // 
            this.btnSnap.Location = new System.Drawing.Point(13, 231);
            this.btnSnap.Name = "btnSnap";
            this.btnSnap.Size = new System.Drawing.Size(116, 50);
            this.btnSnap.TabIndex = 36;
            this.btnSnap.Text = "拍照";
            this.btnSnap.UseVisualStyleBackColor = true;
            this.btnSnap.Click += new System.EventHandler(this.btnSnap_Click);
            // 
            // btnFindModel
            // 
            this.btnFindModel.Location = new System.Drawing.Point(380, 57);
            this.btnFindModel.Name = "btnFindModel";
            this.btnFindModel.Size = new System.Drawing.Size(86, 48);
            this.btnFindModel.TabIndex = 36;
            this.btnFindModel.Text = "查找模板";
            this.btnFindModel.UseVisualStyleBackColor = true;
            this.btnFindModel.Click += new System.EventHandler(this.btnFindModel_Click);
            // 
            // btn_CreateModel
            // 
            this.btn_CreateModel.Location = new System.Drawing.Point(255, 57);
            this.btn_CreateModel.Name = "btn_CreateModel";
            this.btn_CreateModel.Size = new System.Drawing.Size(116, 50);
            this.btn_CreateModel.TabIndex = 36;
            this.btn_CreateModel.Text = " 创建模板";
            this.btn_CreateModel.UseVisualStyleBackColor = true;
            this.btn_CreateModel.Click += new System.EventHandler(this.btn_CreateModel_Click);
            // 
            // btn_CleanReDraw
            // 
            this.btn_CleanReDraw.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btn_CleanReDraw.Location = new System.Drawing.Point(380, 143);
            this.btn_CleanReDraw.Name = "btn_CleanReDraw";
            this.btn_CleanReDraw.Size = new System.Drawing.Size(86, 48);
            this.btn_CleanReDraw.TabIndex = 38;
            this.btn_CleanReDraw.Text = "清除重绘";
            this.btn_CleanReDraw.UseVisualStyleBackColor = false;
            this.btn_CleanReDraw.Click += new System.EventHandler(this.btn_CleanReDraw_Click);
            // 
            // AlgorithmView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelEx1);
            this.Name = "AlgorithmView";
            this.Size = new System.Drawing.Size(1180, 700);
            this.Load += new System.EventHandler(this.AlgorithmView_Load);
            this.panelEx1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panelEx2.ResumeLayout(false);
            this.panelEx2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.PanelEx panelView2;
        private DevComponents.DotNetBar.PanelEx panelView1;
        private System.Windows.Forms.Button btn_AutoRun;
        private System.Windows.Forms.Button btn_ReadImage;
        private System.Windows.Forms.Button btn_DrawOCRRegion;
        private System.Windows.Forms.Button btn_CleanReDraw;
        private System.Windows.Forms.Button btn_CreateModel;
        private System.Windows.Forms.Button btn_DrawLine4;
        private System.Windows.Forms.Button btn_DrawSearchRegion;
        private System.Windows.Forms.Button btn_DrawModelRegion;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn_SaveParam;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParamNameBoxCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParamValueBoxCol;
        private DevComponents.DotNetBar.PanelEx panelEx2;
        private System.Windows.Forms.Button btnFindModel;
        private Comment.LogView logView1;
        private System.Windows.Forms.Button btnSnap;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbCamera;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkIsPathTest;
        private _3DViews.CommonView.LoadPathView loadPath;
        private System.Windows.Forms.Button btnCycleRun;
    }
}
