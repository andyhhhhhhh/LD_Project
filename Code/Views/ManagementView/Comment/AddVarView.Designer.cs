namespace ManagementView.Comment
{
    partial class AddVarView
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
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.skinGroupBox1 = new CCWin.SkinControl.SkinGroupBox();
            this.btnDownMove = new CCWin.SkinControl.SkinButton();
            this.btnUpMove = new CCWin.SkinControl.SkinButton();
            this.btnClear = new CCWin.SkinControl.SkinButton();
            this.btnAddVar = new CCWin.SkinControl.SkinButton();
            this.btnDel = new CCWin.SkinControl.SkinButton();
            this.listVar = new System.Windows.Forms.ListBox();
            this.linkVarForm = new ManagementView._3DViews.CommonView.LinkVariableView();
            this.panelEx1.SuspendLayout();
            this.skinGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.skinGroupBox1);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(566, 416);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 48;
            this.panelEx1.Text = "panelEx1";
            // 
            // skinGroupBox1
            // 
            this.skinGroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.skinGroupBox1.BorderColor = System.Drawing.Color.SteelBlue;
            this.skinGroupBox1.Controls.Add(this.listVar);
            this.skinGroupBox1.Controls.Add(this.btnDownMove);
            this.skinGroupBox1.Controls.Add(this.btnUpMove);
            this.skinGroupBox1.Controls.Add(this.btnClear);
            this.skinGroupBox1.Controls.Add(this.btnAddVar);
            this.skinGroupBox1.Controls.Add(this.btnDel);
            this.skinGroupBox1.Controls.Add(this.linkVarForm);
            this.skinGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinGroupBox1.ForeColor = System.Drawing.Color.Black;
            this.skinGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.skinGroupBox1.Name = "skinGroupBox1";
            this.skinGroupBox1.RectBackColor = System.Drawing.Color.Transparent;
            this.skinGroupBox1.RoundStyle = CCWin.SkinClass.RoundStyle.Top;
            this.skinGroupBox1.Size = new System.Drawing.Size(566, 416);
            this.skinGroupBox1.TabIndex = 41;
            this.skinGroupBox1.TabStop = false;
            this.skinGroupBox1.Text = "文本值配置";
            this.skinGroupBox1.TitleBorderColor = System.Drawing.Color.Transparent;
            this.skinGroupBox1.TitleRadius = 6;
            this.skinGroupBox1.TitleRectBackColor = System.Drawing.Color.White;
            this.skinGroupBox1.TitleRoundStyle = CCWin.SkinClass.RoundStyle.None;
            // 
            // btnDownMove
            // 
            this.btnDownMove.BackColor = System.Drawing.Color.Transparent;
            this.btnDownMove.BaseColor = System.Drawing.Color.Transparent;
            this.btnDownMove.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnDownMove.DownBack = null;
            this.btnDownMove.Location = new System.Drawing.Point(263, 256);
            this.btnDownMove.MouseBack = null;
            this.btnDownMove.Name = "btnDownMove";
            this.btnDownMove.NormlBack = null;
            this.btnDownMove.Size = new System.Drawing.Size(75, 31);
            this.btnDownMove.TabIndex = 28;
            this.btnDownMove.Text = "下移";
            this.btnDownMove.UseVisualStyleBackColor = false;
            this.btnDownMove.Click += new System.EventHandler(this.btnDownMove_Click);
            // 
            // btnUpMove
            // 
            this.btnUpMove.BackColor = System.Drawing.Color.Transparent;
            this.btnUpMove.BaseColor = System.Drawing.Color.Transparent;
            this.btnUpMove.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnUpMove.DownBack = null;
            this.btnUpMove.Location = new System.Drawing.Point(263, 194);
            this.btnUpMove.MouseBack = null;
            this.btnUpMove.Name = "btnUpMove";
            this.btnUpMove.NormlBack = null;
            this.btnUpMove.Size = new System.Drawing.Size(75, 31);
            this.btnUpMove.TabIndex = 29;
            this.btnUpMove.Text = "上移";
            this.btnUpMove.UseVisualStyleBackColor = false;
            this.btnUpMove.Click += new System.EventHandler(this.btnUpMove_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.Transparent;
            this.btnClear.BaseColor = System.Drawing.Color.Transparent;
            this.btnClear.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnClear.DownBack = null;
            this.btnClear.Location = new System.Drawing.Point(263, 318);
            this.btnClear.MouseBack = null;
            this.btnClear.Name = "btnClear";
            this.btnClear.NormlBack = null;
            this.btnClear.Size = new System.Drawing.Size(75, 28);
            this.btnClear.TabIndex = 24;
            this.btnClear.Text = "清空";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnAddVar
            // 
            this.btnAddVar.BackColor = System.Drawing.Color.Transparent;
            this.btnAddVar.BaseColor = System.Drawing.Color.Transparent;
            this.btnAddVar.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnAddVar.DownBack = null;
            this.btnAddVar.Location = new System.Drawing.Point(263, 76);
            this.btnAddVar.MouseBack = null;
            this.btnAddVar.Name = "btnAddVar";
            this.btnAddVar.NormlBack = null;
            this.btnAddVar.Size = new System.Drawing.Size(75, 28);
            this.btnAddVar.TabIndex = 26;
            this.btnAddVar.Text = "添加";
            this.btnAddVar.UseVisualStyleBackColor = false;
            this.btnAddVar.Click += new System.EventHandler(this.btnAddVar_Click);
            // 
            // btnDel
            // 
            this.btnDel.BackColor = System.Drawing.Color.Transparent;
            this.btnDel.BaseColor = System.Drawing.Color.Transparent;
            this.btnDel.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnDel.DownBack = null;
            this.btnDel.Location = new System.Drawing.Point(263, 135);
            this.btnDel.MouseBack = null;
            this.btnDel.Name = "btnDel";
            this.btnDel.NormlBack = null;
            this.btnDel.Size = new System.Drawing.Size(75, 28);
            this.btnDel.TabIndex = 25;
            this.btnDel.Text = "删除";
            this.btnDel.UseVisualStyleBackColor = false;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // listVar
            // 
            this.listVar.Dock = System.Windows.Forms.DockStyle.Left;
            this.listVar.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listVar.FormattingEnabled = true;
            this.listVar.ItemHeight = 17;
            this.listVar.Location = new System.Drawing.Point(3, 17);
            this.listVar.Name = "listVar";
            this.listVar.Size = new System.Drawing.Size(252, 396);
            this.listVar.TabIndex = 30;
            // 
            // linkVarForm
            // 
            this.linkVarForm.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.linkVarForm.LinkText = "";
            this.linkVarForm.Location = new System.Drawing.Point(261, 19);
            this.linkVarForm.ModelType = "";
            this.linkVarForm.Name = "linkVarForm";
            this.linkVarForm.OutValueType = "";
            this.linkVarForm.ShowParam = true;
            this.linkVarForm.Size = new System.Drawing.Size(296, 32);
            this.linkVarForm.TabIndex = 27;
            this.linkVarForm.ValueType = "";
            // 
            // AddVarView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelEx1);
            this.Name = "AddVarView";
            this.Size = new System.Drawing.Size(566, 416);
            this.Load += new System.EventHandler(this.AddVarView_Load);
            this.panelEx1.ResumeLayout(false);
            this.skinGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private CCWin.SkinControl.SkinGroupBox skinGroupBox1;
        private CCWin.SkinControl.SkinButton btnDownMove;
        private CCWin.SkinControl.SkinButton btnUpMove;
        private CCWin.SkinControl.SkinButton btnClear;
        private CCWin.SkinControl.SkinButton btnAddVar;
        private CCWin.SkinControl.SkinButton btnDel;
        private _3DViews.CommonView.LinkVariableView linkVarForm;
        private System.Windows.Forms.ListBox listVar;
    }
}
