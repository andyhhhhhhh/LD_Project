namespace ManagementView.ChartViews
{
    partial class DetailChartView
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dataPoint1 = new System.Windows.Forms.DataVisualization.Charting.DataPoint(1D, 1D);
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.Title title3 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.panelDetailGrid = new System.Windows.Forms.Panel();
            this.cmbSelcetWay = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBigChart = new System.Windows.Forms.Button();
            this.btnSelectData = new System.Windows.Forms.Button();
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.IndexStep = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.XValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.YValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelDetailChart = new System.Windows.Forms.Panel();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panelDetailGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.panelDetailChart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelDetailGrid
            // 
            this.panelDetailGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelDetailGrid.Controls.Add(this.cmbSelcetWay);
            this.panelDetailGrid.Controls.Add(this.label2);
            this.panelDetailGrid.Controls.Add(this.btnBigChart);
            this.panelDetailGrid.Controls.Add(this.btnSelectData);
            this.panelDetailGrid.Controls.Add(this.dgvData);
            this.panelDetailGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetailGrid.Location = new System.Drawing.Point(0, 205);
            this.panelDetailGrid.Name = "panelDetailGrid";
            this.panelDetailGrid.Size = new System.Drawing.Size(221, 158);
            this.panelDetailGrid.TabIndex = 5;
            // 
            // cmbSelcetWay
            // 
            this.cmbSelcetWay.FormattingEnabled = true;
            this.cmbSelcetWay.Items.AddRange(new object[] {
            "X规划",
            "Y规划"});
            this.cmbSelcetWay.Location = new System.Drawing.Point(546, 72);
            this.cmbSelcetWay.Name = "cmbSelcetWay";
            this.cmbSelcetWay.Size = new System.Drawing.Size(133, 20);
            this.cmbSelcetWay.TabIndex = 4;
            this.cmbSelcetWay.SelectedIndexChanged += new System.EventHandler(this.cmbSelcetWay_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(547, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 14);
            this.label2.TabIndex = 3;
            this.label2.Text = "选择规划路径";
            // 
            // btnBigChart
            // 
            this.btnBigChart.Location = new System.Drawing.Point(624, 5);
            this.btnBigChart.Name = "btnBigChart";
            this.btnBigChart.Size = new System.Drawing.Size(75, 23);
            this.btnBigChart.TabIndex = 0;
            this.btnBigChart.Text = "大图模式";
            this.btnBigChart.UseVisualStyleBackColor = true;
            this.btnBigChart.Visible = false;
            this.btnBigChart.Click += new System.EventHandler(this.btnBigChart_Click);
            // 
            // btnSelectData
            // 
            this.btnSelectData.Location = new System.Drawing.Point(547, 5);
            this.btnSelectData.Name = "btnSelectData";
            this.btnSelectData.Size = new System.Drawing.Size(75, 23);
            this.btnSelectData.TabIndex = 0;
            this.btnSelectData.Text = "选择数据";
            this.btnSelectData.UseVisualStyleBackColor = true;
            this.btnSelectData.Click += new System.EventHandler(this.btnSelectData_Click);
            // 
            // dgvData
            // 
            this.dgvData.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IndexStep,
            this.XValue,
            this.YValue});
            this.dgvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvData.Location = new System.Drawing.Point(0, 0);
            this.dgvData.Margin = new System.Windows.Forms.Padding(0);
            this.dgvData.Name = "dgvData";
            this.dgvData.RowTemplate.Height = 23;
            this.dgvData.Size = new System.Drawing.Size(219, 156);
            this.dgvData.TabIndex = 0;
            // 
            // IndexStep
            // 
            this.IndexStep.DataPropertyName = "Index";
            this.IndexStep.HeaderText = "序号";
            this.IndexStep.Name = "IndexStep";
            // 
            // XValue
            // 
            this.XValue.DataPropertyName = "XValue";
            this.XValue.HeaderText = "横轴值";
            this.XValue.Name = "XValue";
            this.XValue.Width = 160;
            // 
            // YValue
            // 
            this.YValue.DataPropertyName = "YValue";
            this.YValue.HeaderText = "纵轴值";
            this.YValue.Name = "YValue";
            this.YValue.Width = 160;
            // 
            // panelDetailChart
            // 
            this.panelDetailChart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelDetailChart.Controls.Add(this.chart1);
            this.panelDetailChart.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDetailChart.Location = new System.Drawing.Point(0, 0);
            this.panelDetailChart.Margin = new System.Windows.Forms.Padding(0);
            this.panelDetailChart.Name = "panelDetailChart";
            this.panelDetailChart.Size = new System.Drawing.Size(221, 205);
            this.panelDetailChart.TabIndex = 4;
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.Color.Transparent;
            this.chart1.BackSecondaryColor = System.Drawing.Color.White;
            chartArea1.AxisX.ScrollBar.Enabled = false;
            chartArea1.AxisX2.ScrollBar.Enabled = false;
            chartArea1.AxisY.ScrollBar.Enabled = false;
            chartArea1.AxisY2.ScrollBar.Enabled = false;
            chartArea1.Name = "ChartArea1";
            chartArea1.Position.Auto = false;
            chartArea1.Position.Height = 87F;
            chartArea1.Position.Width = 93F;
            chartArea1.Position.X = 3F;
            chartArea1.Position.Y = 3F;
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Enabled = false;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Margin = new System.Windows.Forms.Padding(0);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.IsVisibleInLegend = false;
            series1.Legend = "Legend1";
            series1.MarkerColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            series1.Name = "Series1";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Legend = "Legend1";
            series2.Name = "Series2";
            series2.Points.Add(dataPoint1);
            series2.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(219, 203);
            this.chart1.TabIndex = 1;
            this.chart1.Text = "chart1";
            title1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            title1.DockingOffset = -3;
            title1.Name = "Title1";
            title1.Position.Auto = false;
            title1.Position.Height = 6.6F;
            title1.Position.Width = 94F;
            title1.Position.X = 3F;
            title1.Position.Y = 90F;
            title1.Text = "电流(Amp)";
            title2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Left;
            title2.DockingOffset = 4;
            title2.Name = "Title2";
            title2.Position.Auto = false;
            title2.Position.Height = 84.36324F;
            title2.Position.Width = 5.854257F;
            title2.Position.X = 1F;
            title2.Text = "电压(Volt)";
            title2.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Rotated90;
            title3.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Right;
            title3.DockingOffset = -3;
            title3.Name = "Title3";
            title3.Position.Auto = false;
            title3.Position.Height = 94F;
            title3.Position.Width = 5F;
            title3.Position.X = 94F;
            title3.Position.Y = 3F;
            title3.Text = "光功率(W)";
            title3.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Rotated90;
            this.chart1.Titles.Add(title1);
            this.chart1.Titles.Add(title2);
            this.chart1.Titles.Add(title3);
            this.toolTip1.SetToolTip(this.chart1, "X:0 Y:0");
            this.chart1.GetToolTipText += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs>(this.chart1_GetToolTipText);
            this.chart1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseClick);
            this.chart1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseDoubleClick);
            this.chart1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseMove);
            // 
            // DetailChartView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelDetailGrid);
            this.Controls.Add(this.panelDetailChart);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "DetailChartView";
            this.Size = new System.Drawing.Size(221, 363);
            this.Load += new System.EventHandler(this.DetailChartView_Load);
            this.panelDetailGrid.ResumeLayout(false);
            this.panelDetailGrid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.panelDetailChart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelDetailGrid;
        private System.Windows.Forms.Panel panelDetailChart;
        private System.Windows.Forms.Button btnBigChart;
        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.Button btnSelectData;
        private System.Windows.Forms.DataGridViewTextBoxColumn IndexStep;
        private System.Windows.Forms.DataGridViewTextBoxColumn XValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn YValue;
        private System.Windows.Forms.ComboBox cmbSelcetWay;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
