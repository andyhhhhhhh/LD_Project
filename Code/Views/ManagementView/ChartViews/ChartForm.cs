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
    public partial class ChartForm : Form
    {
        DetailChartView[] m_Dcv = new DetailChartView[4];
        public ChartForm()
        {
            InitializeComponent();
        }

        private void ChartForm_Load(object sender, EventArgs e)
        {

            List<double> listX = new List<double> { 0, 1, 2, 3, 4, 5, 6, 7 };
            List<double> listY = new List<double> { 10, 40, 30, 60, 50, 80, 20, 100 };
            for (int i = 0; i < 4; i++)
            {
                m_Dcv[i] = new DetailChartView();
                m_Dcv[i].HideDetailData();


                m_Dcv[i].RefershData(listX, listY, "测试1", true, i);  
            }


            CommHelper.LayoutChildFillView(panel1, m_Dcv[0]);
            CommHelper.LayoutChildFillView(panel2, m_Dcv[1]);
            CommHelper.LayoutChildFillView(panel3, m_Dcv[2]);
            CommHelper.LayoutChildFillView(panel4, m_Dcv[3]);
        }
    }
}
