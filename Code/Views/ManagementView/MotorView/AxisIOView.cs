using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagementView.MotorView
{
    public partial class AxisIOView : UserControl
    {
        public AxisIOView()
        {
            InitializeComponent();
        }

        private void AxisIOView_Load(object sender, EventArgs e)
        {
            try
            {
                CommHelper.LayoutChildFillView(panelAxis, new PointView());
                CommHelper.LayoutChildFillView(panelIO, new IoView());

                tabControl1.SelectedTabIndex = 0;
            }
            catch (Exception ex)
            {
                 
            }
        }
    }
}
