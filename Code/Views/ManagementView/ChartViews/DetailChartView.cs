using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using BaseModels.Comm;
using CCWin;

namespace ManagementView.ChartViews
{
    public partial class DetailChartView : UserControl
    {
        public DetailChartView()
        {
            InitializeComponent();
        } 

        private void DetailChartView_Load(object sender, EventArgs e)
        {
            try
            {
                chart1.ChartAreas[0].CursorX.Interval = 0D;
                chart1.ChartAreas[0].CursorX.IntervalOffsetType = DateTimeIntervalType.Minutes;

                //设置鼠标可以选中
                chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
                chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
                //X轴显示小数位数
                this.chart1.ChartAreas[0].AxisX.LabelStyle.Format = "N1";

                chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gray;
                chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gray;
                chart1.ChartAreas[0].AxisY2.MajorGrid.LineColor = Color.Gray;
            }
            catch (Exception ex)
            {
                 
            }
        }

        public void HideDetailData()
        {
            panelDetailChart.Dock = DockStyle.Fill;
            panelDetailGrid.Visible = false;
        }
        public void ShowDetailData()
        {
            panelDetailChart.Dock = DockStyle.Top;
            panelDetailChart.Size = new Size(954, 204);
            panelDetailGrid.Dock = DockStyle.Fill;
            panelDetailGrid.Visible = true;
        }

        public void SetXYAxisMin(double xvalue, double yvalue, double xmaxvalue, double ymaxvalue, double ymaxvalue2, double yminvalue2)
        {
            try
            { 
                this.chart1.ChartAreas[0].AxisX.Minimum = xvalue;
                if(xmaxvalue > xvalue)
                {
                    this.chart1.ChartAreas[0].AxisX.Maximum = xmaxvalue;
                }
                this.chart1.ChartAreas[0].AxisY.Minimum = yvalue;
                if (ymaxvalue > yvalue)
                {
                    this.chart1.ChartAreas[0].AxisY.Maximum = ymaxvalue; 
                }
                if(ymaxvalue2 != 0)
                {
                    this.chart1.ChartAreas[0].AxisY2.Minimum = yminvalue2;
                    if(ymaxvalue2 > yminvalue2)
                    {
                        this.chart1.ChartAreas[0].AxisY2.Maximum = ymaxvalue2;
                    }
                }
            }
            catch (Exception ex)
            {
                 
            }
        }


        public void SetXYAxisMin(List<double> xvalue, List<double> yvalue, List<double> yvalue1, List<double> yminvalue1)
        {
            try
            {
                double xmin = xvalue.Min();
                double xmax = xvalue.Max();
                if (xmax > xmin)
                {
                    this.chart1.ChartAreas[0].AxisX.Minimum = xmin;
                    this.chart1.ChartAreas[0].AxisX.Maximum = xmax;
                }

                double ymin = (double)yvalue.Min();
                double ymax = (double)yvalue.Max();
                if (ymax > ymin)
                {
                    this.chart1.ChartAreas[0].AxisY.Minimum = ymin;
                    this.chart1.ChartAreas[0].AxisY.Maximum = ymax;
                }
                double y1min = (double)yminvalue1.Min();
                double y2max = (double)yvalue1.Max();
                if (y2max > y1min)
                {
                    this.chart1.ChartAreas[0].AxisY2.Minimum = y1min;
                    this.chart1.ChartAreas[0].AxisY2.Maximum = y2max;
                }
            }
            catch (Exception ex)
            {

            }
        }

        static List<double> m_listXSeriesValue = new List<double>();
        static List<double> m_listYSeriesValueRow = new List<double>();
        static List<double> m_listYSeriesValueCol = new List<double>();

        List<double>[] m_listYSeriesRows = new List<double>[50];
        List<double>[] m_listYSeriesCols = new List<double>[50];
        public int numValuesRow = 0;
        public int numValuesCol = 0;
        public void SetSeriesValueX(List<double> xSeriesValue, List<double> ySeriesValueRow, bool bFirst = false)
        {
            if(bFirst)
            {
                numValuesRow = 0;
            }
            m_listXSeriesValue = xSeriesValue;
            m_listYSeriesValueRow = ySeriesValueRow;
            m_listYSeriesRows[numValuesRow++] = ySeriesValueRow;
        }

        public void SetSeriesValueY(List<double> xSeriesValue,List<double> ySeriesValueCol, bool bFirst = false)
        {
            if (bFirst)
            {
                numValuesCol = 0;
            }
            m_listXSeriesValue = xSeriesValue;
            m_listYSeriesValueCol = ySeriesValueCol;
            m_listYSeriesCols[numValuesCol++] = ySeriesValueCol;
        }
          
        public void RefershData(List<double> xSeriesValue, List<double> ySeriesValue, string tilte, bool bfirst = false, int index = 0)
        {
            try
            {
                if (xSeriesValue.Count() != ySeriesValue.Count())
                {
                    throw new ArgumentException("xSeriesValue和ySeriesValue参数数量个数不匹配");
                }

                //第一次会清除掉图中数据
                if(bfirst)
                {
                    if (chart1.Series.Count() > 0)
                    {
                        chart1.Series[0].Points.Clear();
                    }
                    chart1.Series.Clear(); 
                }
                
                Series series = new Series();
                SetParam(tilte, series, index);

                List<ChartData> chartData = new List<ChartData>();
                for (int i = 0; i < xSeriesValue.Count(); i++)
                {
                    ChartData chartData1 = new ChartData();
                    chartData1.Index = i;
                    chartData1.XValue = xSeriesValue[i];
                    chartData1.YValue = ySeriesValue[i];
                    chartData.Add(chartData1);

                    series.Points.AddXY(xSeriesValue[i], ySeriesValue[i]);
                }
                chart1.Series.Add(series);
                dgvData.DataSource = null;
                dgvData.DataSource = chartData;
                int count = dgvData.ColumnCount;
                dgvData.Columns[count-1].Visible = false;
            }
            catch(Exception ex)
            {

            }

        }

        private void SetParam(string tilte, Series series, int index = 0)
        {
            series.MarkerBorderColor = Color.Black;
            series.MarkerColor = Color.Black;
           // series.MarkerStyle = MarkerStyle.Square;
            series.MarkerSize = 2;
           // series.IsValueShownAsLabel = true;


            series.LabelBackColor = Color.White;
            series.LabelForeColor = Color.Black;

            series.LegendText = tilte;
            if(index == 0)
            { 
                series.ChartType = SeriesChartType.Line;
                series.Color = Color.Blue;
                series.YAxisType = AxisType.Primary;
            }
            else if (index == 1)
            {
                //series.ChartType = SeriesChartType.Column;
                series.ChartType = SeriesChartType.Line;
                series.Color = Color.Brown;
                series.YAxisType = AxisType.Secondary;
            }
            else if (index == 2)
            {
                //series.ChartType = SeriesChartType.Bar;
                series.ChartType = SeriesChartType.Line;
                series.Color = Color.Red;
                series.YAxisType = AxisType.Secondary; 
            }
            else if (index == 3)
            {
                //series.ChartType = SeriesChartType.StepLine;
                series.ChartType = SeriesChartType.Line;
                series.Color = Color.Green;
                series.YAxisType = AxisType.Secondary; 
            }
        }

        public void SetChartColor()
        {
            try
            { 
                chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Transparent;
            }
            catch (Exception ex)
            {
                 
            }
        }

        public void RefershData(List<ChartData> chartData, string tilte)
        {
            chart1.Series.Clear();
            Series series = new Series();
            SetParam(tilte, series);

            for (int i = 0; i < chartData.Count(); i++)
            {
                series.Points.AddXY(chartData[i].XValue, chartData[i].YValue);
            }
            chart1.Series.Add(series);
        }

        private void btnSelectData_Click(object sender, EventArgs e)
        {
            List<ChartData> dataDisp = new List<ChartData>();
            DataGridViewSelectedCellCollection cells = dgvData.SelectedCells;
            if (cells.Count > 1)
            {
                List<ChartData> coords = new List<ChartData>();
                ChartData data = new ChartData();
                foreach (DataGridViewCell cell in cells)
                {
                    data = dgvData.Rows[cell.RowIndex].DataBoundItem as ChartData;
                    if (!dataDisp.Contains(data))
                    {
                        dataDisp.Add(data);
                    }
                }
            }
            else
            {
                MessageBoxEx.Show("选择绘制的点位数量太少，没有办法形成曲线");
            }
            if (dataDisp.Count < 2)
                MessageBoxEx.Show("选择绘制的点位数量太少，没有办法形成曲线");
            RefershData(dataDisp, "数据A");
        }

        private void btnBigChart_Click(object sender, EventArgs e)
        {

        } 

        private void cmbSelcetWay_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSelcetWay.Text.Contains("X"))
                {
                    for(int i = 0; i < numValuesRow; i++)
                    { 
                        RefershData(m_listXSeriesValue, m_listYSeriesRows[i], string.Format("X方向数据{0}", i), i==0);
                    }
                }
                else if (cmbSelcetWay.Text.Contains("Y"))
                {
                    for (int i = 0; i < numValuesCol; i++)
                    {
                        RefershData(m_listXSeriesValue, m_listYSeriesCols[i], string.Format("Y方向数据{0}", i), i == 0);
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }

        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                HitTestResult hit = chart1.HitTest(e.X, e.Y);
                if (hit.Series != null)
                {
                    var xValue = hit.Series.Points[hit.PointIndex].XValue;
                    var yValue = hit.Series.Points[hit.PointIndex].YValues.First();
                    string value = string.Format("{0:F0},{1:F0}", "x:" + xValue, "y:" + yValue);//textbox1也是自己建的一个专门用来显示的内容框，也可以用messagebox直接弹出内容
                                                                                                // MessageBoxEx.Show(value);
                }
                else
                {
                    string value = "未点击到波形曲线";
                    // MessageBoxEx.Show(value);
                }
            }
            catch (Exception ex)
            {
                 
            } 
        }

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                var area = chart1.ChartAreas[0];

                double xValue = area.AxisX.PixelPositionToValue(e.X);
                double yValue = area.AxisY.PixelPositionToValue(e.Y);
                string value = string.Format("{0:F0},{1:F0}", xValue, yValue);
                //MessageBoxEx.Show(value);
            }
            catch (Exception ex)
            {
                 
            }
          
        }

        private void chart1_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            try
            {
                if (e.HitTestResult.ChartElementType == ChartElementType.DataPoint)
                {
                    int i = e.HitTestResult.PointIndex;
                    DataPoint dp = e.HitTestResult.Series.Points[i];
                    e.Text = string.Format("X:{0} Y:{1:F1}", dp.XValue, dp.YValues[0]);
                }
            }
            catch (Exception ex)
            {
                 
            } 
        }

        public void SetTitle(string title1, string title2, string title3 = "")
        {
            try
            {
                chart1.Titles[0].Text = title1;
                chart1.Titles[1].Text = title2;
                chart1.Titles[2].Text = title3;
                if(string.IsNullOrEmpty(title3))
                {
                    chart1.Titles[2].Visible = false;
                }
                else
                {
                    chart1.Titles[2].Visible = true;
                }
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void chart1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            { 
                chart1.ChartAreas[0].AxisX.ScaleView.ZoomReset();
                chart1.ChartAreas[0].AxisY.ScaleView.ZoomReset();
            }
            catch (Exception ex)
            { 
            }
        }
    }
}
