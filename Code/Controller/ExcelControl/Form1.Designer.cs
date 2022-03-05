namespace ExcelController
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
            this.btnMapPath = new System.Windows.Forms.Button();
            this.txtMapPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtWaferNum = new System.Windows.Forms.TextBox();
            this.btnGetData = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnReadExcel = new System.Windows.Forms.Button();
            this.btnSetBackColor = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtbar = new System.Windows.Forms.TextBox();
            this.btnSetBar = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBoxCol = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBoxRow = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnParseBar = new System.Windows.Forms.Button();
            this.btnGetBarData = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtProductType = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnUnMerge = new System.Windows.Forms.Button();
            this.txtMapType = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnMapPath
            // 
            this.btnMapPath.Location = new System.Drawing.Point(500, 25);
            this.btnMapPath.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnMapPath.Name = "btnMapPath";
            this.btnMapPath.Size = new System.Drawing.Size(98, 22);
            this.btnMapPath.TabIndex = 1;
            this.btnMapPath.Text = "选择MAP路径";
            this.btnMapPath.UseVisualStyleBackColor = true;
            this.btnMapPath.Click += new System.EventHandler(this.btnMapPath_Click);
            // 
            // txtMapPath
            // 
            this.txtMapPath.Location = new System.Drawing.Point(75, 26);
            this.txtMapPath.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtMapPath.Name = "txtMapPath";
            this.txtMapPath.Size = new System.Drawing.Size(422, 21);
            this.txtMapPath.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 31);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "Map路径：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 58);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "wafer：";
            // 
            // txtWaferNum
            // 
            this.txtWaferNum.Location = new System.Drawing.Point(75, 54);
            this.txtWaferNum.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtWaferNum.Name = "txtWaferNum";
            this.txtWaferNum.Size = new System.Drawing.Size(103, 21);
            this.txtWaferNum.TabIndex = 6;
            this.txtWaferNum.Text = "A7587-04";
            // 
            // btnGetData
            // 
            this.btnGetData.Location = new System.Drawing.Point(88, 128);
            this.btnGetData.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(68, 33);
            this.btnGetData.TabIndex = 9;
            this.btnGetData.Text = "读取W数据";
            this.btnGetData.UseVisualStyleBackColor = true;
            this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(16, 166);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.Size = new System.Drawing.Size(939, 349);
            this.dataGridView1.TabIndex = 10;
            // 
            // btnReadExcel
            // 
            this.btnReadExcel.Location = new System.Drawing.Point(16, 128);
            this.btnReadExcel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnReadExcel.Name = "btnReadExcel";
            this.btnReadExcel.Size = new System.Drawing.Size(68, 33);
            this.btnReadExcel.TabIndex = 11;
            this.btnReadExcel.Text = "读取Excel";
            this.btnReadExcel.UseVisualStyleBackColor = true;
            this.btnReadExcel.Click += new System.EventHandler(this.btnReadExcel_Click);
            // 
            // btnSetBackColor
            // 
            this.btnSetBackColor.Location = new System.Drawing.Point(307, 128);
            this.btnSetBackColor.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSetBackColor.Name = "btnSetBackColor";
            this.btnSetBackColor.Size = new System.Drawing.Size(68, 33);
            this.btnSetBackColor.TabIndex = 12;
            this.btnSetBackColor.Text = "设置底色";
            this.btnSetBackColor.UseVisualStyleBackColor = true;
            this.btnSetBackColor.Click += new System.EventHandler(this.btnSetBackColor_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 83);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 14;
            this.label3.Text = "Bar：";
            // 
            // txtbar
            // 
            this.txtbar.Location = new System.Drawing.Point(75, 78);
            this.txtbar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtbar.Name = "txtbar";
            this.txtbar.Size = new System.Drawing.Size(103, 21);
            this.txtbar.TabIndex = 13;
            this.txtbar.Text = "4";
            // 
            // btnSetBar
            // 
            this.btnSetBar.Location = new System.Drawing.Point(192, 78);
            this.btnSetBar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSetBar.Name = "btnSetBar";
            this.btnSetBar.Size = new System.Drawing.Size(98, 22);
            this.btnSetBar.TabIndex = 15;
            this.btnSetBar.Text = "设置Bar条号";
            this.btnSetBar.UseVisualStyleBackColor = true;
            this.btnSetBar.Click += new System.EventHandler(this.btnSetBar_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(434, 86);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 19;
            this.label4.Text = "列：";
            // 
            // txtBoxCol
            // 
            this.txtBoxCol.Location = new System.Drawing.Point(496, 83);
            this.txtBoxCol.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtBoxCol.Name = "txtBoxCol";
            this.txtBoxCol.Size = new System.Drawing.Size(103, 21);
            this.txtBoxCol.TabIndex = 18;
            this.txtBoxCol.Text = "6";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(434, 61);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "行：";
            // 
            // txtBoxRow
            // 
            this.txtBoxRow.Location = new System.Drawing.Point(496, 56);
            this.txtBoxRow.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtBoxRow.Name = "txtBoxRow";
            this.txtBoxRow.Size = new System.Drawing.Size(103, 21);
            this.txtBoxRow.TabIndex = 16;
            this.txtBoxRow.Text = "5";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(618, 54);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(227, 21);
            this.textBox1.TabIndex = 20;
            this.textBox1.Text = "09135,09134,09133,09132,09131,04160,04159";
            // 
            // btnParseBar
            // 
            this.btnParseBar.Location = new System.Drawing.Point(161, 128);
            this.btnParseBar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnParseBar.Name = "btnParseBar";
            this.btnParseBar.Size = new System.Drawing.Size(68, 33);
            this.btnParseBar.TabIndex = 21;
            this.btnParseBar.Text = "解析bar条";
            this.btnParseBar.UseVisualStyleBackColor = true;
            this.btnParseBar.Click += new System.EventHandler(this.btnParseBar_Click);
            // 
            // btnGetBarData
            // 
            this.btnGetBarData.Location = new System.Drawing.Point(234, 128);
            this.btnGetBarData.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnGetBarData.Name = "btnGetBarData";
            this.btnGetBarData.Size = new System.Drawing.Size(68, 33);
            this.btnGetBarData.TabIndex = 22;
            this.btnGetBarData.Text = "获取B数据";
            this.btnGetBarData.UseVisualStyleBackColor = true;
            this.btnGetBarData.Click += new System.EventHandler(this.btnGetBarData_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(380, 128);
            this.btnTest.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(68, 33);
            this.btnTest.TabIndex = 24;
            this.btnTest.Text = "测试";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 108);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 26;
            this.label6.Text = "产品：";
            // 
            // txtProductType
            // 
            this.txtProductType.Location = new System.Drawing.Point(75, 103);
            this.txtProductType.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtProductType.Name = "txtProductType";
            this.txtProductType.Size = new System.Drawing.Size(103, 21);
            this.txtProductType.TabIndex = 25;
            this.txtProductType.Text = "25W";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(434, 113);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 28;
            this.label7.Text = "表：";
            // 
            // btnUnMerge
            // 
            this.btnUnMerge.Location = new System.Drawing.Point(618, 108);
            this.btnUnMerge.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnUnMerge.Name = "btnUnMerge";
            this.btnUnMerge.Size = new System.Drawing.Size(98, 22);
            this.btnUnMerge.TabIndex = 29;
            this.btnUnMerge.Text = "取消单元格";
            this.btnUnMerge.UseVisualStyleBackColor = true;
            this.btnUnMerge.Click += new System.EventHandler(this.btnUnMerge_Click);
            // 
            // txtMapType
            // 
            this.txtMapType.FormattingEnabled = true;
            this.txtMapType.Items.AddRange(new object[] {
            "国内",
            "国外"});
            this.txtMapType.Location = new System.Drawing.Point(495, 110);
            this.txtMapType.Name = "txtMapType";
            this.txtMapType.Size = new System.Drawing.Size(103, 20);
            this.txtMapType.TabIndex = 30;
            this.txtMapType.Text = "国内";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(973, 524);
            this.Controls.Add(this.txtMapType);
            this.Controls.Add(this.btnUnMerge);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtProductType);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnGetBarData);
            this.Controls.Add(this.btnParseBar);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtBoxCol);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtBoxRow);
            this.Controls.Add(this.btnSetBar);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtbar);
            this.Controls.Add(this.btnSetBackColor);
            this.Controls.Add(this.btnReadExcel);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnGetData);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtWaferNum);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMapPath);
            this.Controls.Add(this.btnMapPath);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnMapPath;
        private System.Windows.Forms.TextBox txtMapPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtWaferNum;
        private System.Windows.Forms.Button btnGetData;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnReadExcel;
        private System.Windows.Forms.Button btnSetBackColor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtbar;
        private System.Windows.Forms.Button btnSetBar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBoxCol;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBoxRow;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnParseBar;
        private System.Windows.Forms.Button btnGetBarData;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtProductType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnUnMerge;
        private System.Windows.Forms.ComboBox txtMapType;
    }
}

