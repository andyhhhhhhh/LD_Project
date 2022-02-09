using CCWin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagementView.ChartViews
{
    public partial class ChartView : CCSkinMain
    {
        public ChartView()
        {
            InitializeComponent();
        }

        private void ChartView_Load(object sender, EventArgs e)
        {
            m_Dcv.HideDetailData();
            CommHelper.LayoutChildFillView(panel1, m_Dcv);
        }

        DetailChartView m_Dcv = new DetailChartView();

        public void RefershData(List<double> xSeriesValue, List<double> ySeriesValue, string tilte, bool bFirst = false)
        {
            if (xSeriesValue.Count() != ySeriesValue.Count())
            {
                throw new ArgumentException("xSeriesValue和ySeriesValue参数数量个数不匹配");
            }

            m_Dcv.RefershData(xSeriesValue, ySeriesValue, tilte, bFirst);
            //chart1.Series.Clear();
            //Series series = new Series();
            //series.LegendText = tilte;
            //series.ChartType = SeriesChartType.Line;

            //for (int i = 0; i < xSeriesValue.Count(); i++)
            //{
            //    series.Points.AddXY(xSeriesValue[i], ySeriesValue[i]);
            //}

            //chart1.Series.Add(series);

        }
    }
}
