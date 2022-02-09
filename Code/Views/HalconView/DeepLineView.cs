using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace HalconView
{
    public partial class DeepLineView : Form
    {
        public DeepLineView()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            //this.Text += " 分辨率:" + GlobalCore.Global.ZResolution;
            //chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            //chart1.ChartAreas[0].CursorY.IsUserEnabled = true;

            //chart1.ChartAreas[0].CursorX.SelectionStart = 0;
            //chart1.ChartAreas[0].CursorX.SelectionEnd = 100;

            chart1.ChartAreas[0].CursorX.Interval = 0D;
            chart1.ChartAreas[0].CursorX.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Minutes;

            //设置鼠标可以选中
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;

            //X轴显示小数位数
            this.chart1.ChartAreas[0].AxisX.LabelStyle.Format = "N1";
            
        }

        public void RefershData(List<double> xSeriesValue, List<double> ySeriesValue, string tilte, bool bfirst = false)
        {
            try
            {
                if (xSeriesValue.Count() != ySeriesValue.Count())
                {
                    throw new ArgumentException("xSeriesValue和ySeriesValue参数数量个数不匹配");
                } 

                //第一次会清除掉图中数据
                if (bfirst)
                {
                    if (chart1.Series.Count() > 0)
                    {
                        chart1.Series[0].Points.Clear();
                    }
                    chart1.Series.Clear();
                }

                Series series = new Series();
                SetParam(tilte, series);
                
                for (int i = 0; i < xSeriesValue.Count(); i++)
                {
                    series.Points.AddXY(xSeriesValue[i], ySeriesValue[i]);
                }
                chart1.Series.Add(series);
                
            }
            catch (Exception ex)
            {

            }

        }

        private void SetParam(string tilte, Series series)
        {
            series.MarkerBorderColor = Color.White;
            series.MarkerColor = Color.White;
            // series.MarkerStyle = MarkerStyle.Square;
            series.MarkerSize = 2;
            // series.IsValueShownAsLabel = true;
            series.Color = Color.Yellow;
            series.BorderWidth = 2;

            series.LabelBackColor = Color.White;
            series.LabelForeColor = Color.Black;

            series.LegendText = tilte;
            series.ChartType = SeriesChartType.Line; 
        }

        private void chart1_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            if (e.HitTestResult.ChartElementType == ChartElementType.DataPoint)
            {
                int i = e.HitTestResult.PointIndex;
                DataPoint dp = e.HitTestResult.Series.Points[i];
                e.Text = string.Format("X:{0} Y:{1:F1}", dp.XValue, dp.YValues[0]/**GlobalCore.Global.ZResolution*/);
            }
        }

        private void chart1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            chart1.ChartAreas[0].AxisX.ScaleView.ZoomReset();
            chart1.ChartAreas[0].AxisY.ScaleView.ZoomReset();
        }
    }
}
