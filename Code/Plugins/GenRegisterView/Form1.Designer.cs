namespace GenRegisterView
{
    partial class Form1
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnGenRegister = new CCWin.SkinControl.SkinButton();
            this.txtRegisterKey = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtInfoKey = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.btnExit = new CCWin.SkinControl.SkinButton();
            this.lbl1 = new System.Windows.Forms.Label();
            this.numDays = new System.Windows.Forms.NumericUpDown();
            this.chkIsLimitTime = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numDays)).BeginInit();
            this.SuspendLayout();
            // 
            // btnGenRegister
            // 
            this.btnGenRegister.BackColor = System.Drawing.Color.Transparent;
            this.btnGenRegister.BaseColor = System.Drawing.Color.Transparent;
            this.btnGenRegister.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnGenRegister.DownBack = null;
            this.btnGenRegister.Location = new System.Drawing.Point(35, 345);
            this.btnGenRegister.MouseBack = null;
            this.btnGenRegister.Name = "btnGenRegister";
            this.btnGenRegister.NormlBack = null;
            this.btnGenRegister.Size = new System.Drawing.Size(103, 33);
            this.btnGenRegister.TabIndex = 11;
            this.btnGenRegister.Text = "生成注册码";
            this.btnGenRegister.UseVisualStyleBackColor = false;
            this.btnGenRegister.Click += new System.EventHandler(this.btnGenRegister_Click);
            // 
            // txtRegisterKey
            // 
            // 
            // 
            // 
            this.txtRegisterKey.Border.Class = "TextBoxBorder";
            this.txtRegisterKey.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtRegisterKey.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRegisterKey.Location = new System.Drawing.Point(91, 136);
            this.txtRegisterKey.Multiline = true;
            this.txtRegisterKey.Name = "txtRegisterKey";
            this.txtRegisterKey.Size = new System.Drawing.Size(357, 104);
            this.txtRegisterKey.TabIndex = 8;
            // 
            // txtInfoKey
            // 
            // 
            // 
            // 
            this.txtInfoKey.Border.Class = "TextBoxBorder";
            this.txtInfoKey.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtInfoKey.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtInfoKey.Location = new System.Drawing.Point(91, 16);
            this.txtInfoKey.Multiline = true;
            this.txtInfoKey.Name = "txtInfoKey";
            this.txtInfoKey.Size = new System.Drawing.Size(357, 104);
            this.txtInfoKey.TabIndex = 9;
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX2.Location = new System.Drawing.Point(22, 136);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(62, 23);
            this.labelX2.TabIndex = 6;
            this.labelX2.Text = "注册码:";
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX1.Location = new System.Drawing.Point(22, 16);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(62, 23);
            this.labelX1.TabIndex = 7;
            this.labelX1.Text = "机器码:";
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.BaseColor = System.Drawing.Color.Transparent;
            this.btnExit.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.DownBack = null;
            this.btnExit.Location = new System.Drawing.Point(317, 345);
            this.btnExit.MouseBack = null;
            this.btnExit.Name = "btnExit";
            this.btnExit.NormlBack = null;
            this.btnExit.Size = new System.Drawing.Size(103, 33);
            this.btnExit.TabIndex = 12;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl1.Location = new System.Drawing.Point(31, 280);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(32, 17);
            this.lbl1.TabIndex = 13;
            this.lbl1.Text = "天数";
            // 
            // numDays
            // 
            this.numDays.Location = new System.Drawing.Point(91, 278);
            this.numDays.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numDays.Name = "numDays";
            this.numDays.Size = new System.Drawing.Size(120, 21);
            this.numDays.TabIndex = 14;
            // 
            // chkIsLimitTime
            // 
            this.chkIsLimitTime.AutoSize = true;
            this.chkIsLimitTime.Location = new System.Drawing.Point(229, 281);
            this.chkIsLimitTime.Name = "chkIsLimitTime";
            this.chkIsLimitTime.Size = new System.Drawing.Size(96, 16);
            this.chkIsLimitTime.TabIndex = 15;
            this.chkIsLimitTime.Text = "限制使用时间";
            this.chkIsLimitTime.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightBlue;
            this.ClientSize = new System.Drawing.Size(466, 390);
            this.Controls.Add(this.chkIsLimitTime);
            this.Controls.Add(this.numDays);
            this.Controls.Add(this.lbl1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnGenRegister);
            this.Controls.Add(this.txtRegisterKey);
            this.Controls.Add(this.txtInfoKey);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "注册码生成器";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numDays)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CCWin.SkinControl.SkinButton btnGenRegister;
        private DevComponents.DotNetBar.Controls.TextBoxX txtRegisterKey;
        private DevComponents.DotNetBar.Controls.TextBoxX txtInfoKey;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX1;
        private CCWin.SkinControl.SkinButton btnExit;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.NumericUpDown numDays;
        private System.Windows.Forms.CheckBox chkIsLimitTime;
    }
}

