namespace ManagementView._3DViews.CommonView
{
    partial class ModelFollowView
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
            this.skinGroupBox1 = new CCWin.SkinControl.SkinGroupBox();
            this.btnDisplayPic = new CCWin.SkinControl.SkinButton();
            this.cmbModelForm = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.chkIsModelFollow = new CCWin.SkinControl.SkinCheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.skinGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // skinGroupBox1
            // 
            this.skinGroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.skinGroupBox1.BorderColor = System.Drawing.Color.SteelBlue;
            this.skinGroupBox1.Controls.Add(this.btnDisplayPic);
            this.skinGroupBox1.Controls.Add(this.cmbModelForm);
            this.skinGroupBox1.Controls.Add(this.chkIsModelFollow);
            this.skinGroupBox1.Controls.Add(this.label1);
            this.skinGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinGroupBox1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinGroupBox1.ForeColor = System.Drawing.Color.Black;
            this.skinGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.skinGroupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.skinGroupBox1.Name = "skinGroupBox1";
            this.skinGroupBox1.Padding = new System.Windows.Forms.Padding(0);
            this.skinGroupBox1.RectBackColor = System.Drawing.Color.Transparent;
            this.skinGroupBox1.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinGroupBox1.Size = new System.Drawing.Size(242, 96);
            this.skinGroupBox1.TabIndex = 16;
            this.skinGroupBox1.TabStop = false;
            this.skinGroupBox1.Text = "模板跟随";
            this.skinGroupBox1.TitleBorderColor = System.Drawing.Color.Transparent;
            this.skinGroupBox1.TitleRectBackColor = System.Drawing.Color.White;
            this.skinGroupBox1.TitleRoundStyle = CCWin.SkinClass.RoundStyle.None;
            // 
            // btnDisplayPic
            // 
            this.btnDisplayPic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDisplayPic.BackColor = System.Drawing.Color.Transparent;
            this.btnDisplayPic.BaseColor = System.Drawing.Color.Transparent;
            this.btnDisplayPic.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnDisplayPic.DownBack = null;
            this.btnDisplayPic.GlowColor = System.Drawing.Color.Blue;
            this.btnDisplayPic.Location = new System.Drawing.Point(142, 58);
            this.btnDisplayPic.MouseBack = null;
            this.btnDisplayPic.Name = "btnDisplayPic";
            this.btnDisplayPic.NormlBack = null;
            this.btnDisplayPic.Size = new System.Drawing.Size(95, 25);
            this.btnDisplayPic.TabIndex = 137;
            this.btnDisplayPic.Text = "显示模板图片";
            this.btnDisplayPic.UseVisualStyleBackColor = false;
            this.btnDisplayPic.Visible = false;
            this.btnDisplayPic.Click += new System.EventHandler(this.btnDisplayPic_Click);
            // 
            // cmbModelForm
            // 
            this.cmbModelForm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbModelForm.DisplayMember = "Text";
            this.cmbModelForm.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbModelForm.FormattingEnabled = true;
            this.cmbModelForm.ItemHeight = 17;
            this.cmbModelForm.Location = new System.Drawing.Point(64, 27);
            this.cmbModelForm.Name = "cmbModelForm";
            this.cmbModelForm.Size = new System.Drawing.Size(173, 23);
            this.cmbModelForm.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbModelForm.TabIndex = 15;
            this.cmbModelForm.SelectedIndexChanged += new System.EventHandler(this.cmbModelForm_SelectedIndexChanged);
            // 
            // chkIsModelFollow
            // 
            this.chkIsModelFollow.AutoSize = true;
            this.chkIsModelFollow.BackColor = System.Drawing.Color.Transparent;
            this.chkIsModelFollow.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.chkIsModelFollow.DownBack = null;
            this.chkIsModelFollow.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkIsModelFollow.Location = new System.Drawing.Point(61, 60);
            this.chkIsModelFollow.MouseBack = null;
            this.chkIsModelFollow.Name = "chkIsModelFollow";
            this.chkIsModelFollow.NormlBack = null;
            this.chkIsModelFollow.SelectedDownBack = null;
            this.chkIsModelFollow.SelectedMouseBack = null;
            this.chkIsModelFollow.SelectedNormlBack = null;
            this.chkIsModelFollow.Size = new System.Drawing.Size(75, 21);
            this.chkIsModelFollow.TabIndex = 13;
            this.chkIsModelFollow.Text = "是否启用";
            this.chkIsModelFollow.UseVisualStyleBackColor = false;
            this.chkIsModelFollow.CheckedChanged += new System.EventHandler(this.chkIsModelFollow_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(4, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 17);
            this.label1.TabIndex = 12;
            this.label1.Text = "模板来源:";
            // 
            // ModelFollowView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.skinGroupBox1);
            this.Name = "ModelFollowView";
            this.Size = new System.Drawing.Size(242, 96);
            this.Load += new System.EventHandler(this.ModelFollowView_Load);
            this.skinGroupBox1.ResumeLayout(false);
            this.skinGroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CCWin.SkinControl.SkinGroupBox skinGroupBox1;
        private System.Windows.Forms.Label label1;
        private CCWin.SkinControl.SkinCheckBox chkIsModelFollow;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbModelForm;
        private CCWin.SkinControl.SkinButton btnDisplayPic;
    }
}
