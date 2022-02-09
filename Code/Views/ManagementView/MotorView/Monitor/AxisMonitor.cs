using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SequenceTestModel;

namespace ManagementView.MotorView
{
    public partial class AxisMonitor : UserControl
    {
        public AxisMonitor()
        {
            InitializeComponent();
        }

        private void AxisMonitor_Load(object sender, EventArgs e)
        {

        }

        public void SetStatus(AxisStatus axisStatus)
        {
            try
            {
                BeginInvoke(new Action(() =>
                {
                    btn_homed.Image = axisStatus.homed ? Properties.Resources.Green_2 : Properties.Resources.Red_2; 
                    btn_inited.Image = axisStatus.inited ? Properties.Resources.Green_2 : Properties.Resources.Red_2;
                    btn_enabled.Image = axisStatus.enabled ? Properties.Resources.Green_2 : Properties.Resources.Red_2;
                    btn_warning.Image = axisStatus.warning ? Properties.Resources.Green_2 : Properties.Resources.Red_2;
                    btn_pLimited.Image = axisStatus.pLimited ? Properties.Resources.Green_2 : Properties.Resources.Red_2;
                    btn_nLimited.Image = axisStatus.nLimited ? Properties.Resources.Green_2 : Properties.Resources.Red_2;
                    btn_planning.Image = axisStatus.planning ? Properties.Resources.Green_2 : Properties.Resources.Red_2;
                    btn_reached.Image = axisStatus.reached ? Properties.Resources.Green_2 : Properties.Resources.Red_2;
                }));
            }
            catch (Exception ex)
            {
                 
            }
        }
    }
}
