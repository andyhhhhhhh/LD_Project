namespace ManagementView._3DViews
{
    partial class CalculteView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalculteView));
            this.calculator1 = new DevComponents.Editors.Calculator();
            this.SuspendLayout();
            // 
            // calculator1
            // 
            this.calculator1.AutoSize = true;
            this.calculator1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.calculator1.Location = new System.Drawing.Point(0, 0);
            this.calculator1.Name = "calculator1";
            this.calculator1.Size = new System.Drawing.Size(190, 211);
            this.calculator1.Text = "calculator1";
            // 
            // CalculteView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(190, 211);
            this.Controls.Add(this.calculator1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CalculteView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "计算器";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.Editors.Calculator calculator1;
    }
}