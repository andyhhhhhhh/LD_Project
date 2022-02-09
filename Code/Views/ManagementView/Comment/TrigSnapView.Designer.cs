namespace ManagementView
{
    partial class TrigSnapView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnReg = new System.Windows.Forms.Button();
            this.btnGetPara = new System.Windows.Forms.Button();
            this.btnLoop = new System.Windows.Forms.Button();
            this.btnExitLoop = new System.Windows.Forms.Button();
            this.btnCloseCamera = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(497, 622);
            this.panel1.TabIndex = 0;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(503, 26);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(269, 160);
            this.listBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(503, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "相机支持回调列表";
            // 
            // btnReg
            // 
            this.btnReg.Location = new System.Drawing.Point(505, 220);
            this.btnReg.Name = "btnReg";
            this.btnReg.Size = new System.Drawing.Size(75, 45);
            this.btnReg.TabIndex = 3;
            this.btnReg.Text = "注册回调";
            this.btnReg.UseVisualStyleBackColor = true;
            this.btnReg.Click += new System.EventHandler(this.btnReg_Click);
            // 
            // btnGetPara
            // 
            this.btnGetPara.Location = new System.Drawing.Point(671, 220);
            this.btnGetPara.Name = "btnGetPara";
            this.btnGetPara.Size = new System.Drawing.Size(75, 45);
            this.btnGetPara.TabIndex = 3;
            this.btnGetPara.Text = "获取参数";
            this.btnGetPara.UseVisualStyleBackColor = true;
            this.btnGetPara.Click += new System.EventHandler(this.btnGetPara_Click);
            // 
            // btnLoop
            // 
            this.btnLoop.Location = new System.Drawing.Point(505, 397);
            this.btnLoop.Name = "btnLoop";
            this.btnLoop.Size = new System.Drawing.Size(75, 45);
            this.btnLoop.TabIndex = 3;
            this.btnLoop.Text = "循环开始";
            this.btnLoop.UseVisualStyleBackColor = true;
            this.btnLoop.Click += new System.EventHandler(this.btnLoop_Click);
            // 
            // btnExitLoop
            // 
            this.btnExitLoop.Location = new System.Drawing.Point(671, 397);
            this.btnExitLoop.Name = "btnExitLoop";
            this.btnExitLoop.Size = new System.Drawing.Size(75, 45);
            this.btnExitLoop.TabIndex = 3;
            this.btnExitLoop.Text = "循环结束";
            this.btnExitLoop.UseVisualStyleBackColor = true;
            this.btnExitLoop.Click += new System.EventHandler(this.btnExitLoop_Click);
            // 
            // btnCloseCamera
            // 
            this.btnCloseCamera.Location = new System.Drawing.Point(505, 468);
            this.btnCloseCamera.Name = "btnCloseCamera";
            this.btnCloseCamera.Size = new System.Drawing.Size(75, 45);
            this.btnCloseCamera.TabIndex = 3;
            this.btnCloseCamera.Text = "关闭相机";
            this.btnCloseCamera.UseVisualStyleBackColor = true;
            this.btnCloseCamera.Click += new System.EventHandler(this.btnCloseCamera_Click);
            // 
            // TrigSnapView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 622);
            this.Controls.Add(this.btnExitLoop);
            this.Controls.Add(this.btnGetPara);
            this.Controls.Add(this.btnCloseCamera);
            this.Controls.Add(this.btnLoop);
            this.Controls.Add(this.btnReg);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.panel1);
            this.Name = "TrigSnapView";
            this.Text = "硬触发采集测试";
            this.Load += new System.EventHandler(this.TrigSnapView_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnReg;
        private System.Windows.Forms.Button btnGetPara;
        private System.Windows.Forms.Button btnLoop;
        private System.Windows.Forms.Button btnExitLoop;
        private System.Windows.Forms.Button btnCloseCamera;
    }
}