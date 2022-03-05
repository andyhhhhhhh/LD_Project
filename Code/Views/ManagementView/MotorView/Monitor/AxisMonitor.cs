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
        private int index;
        /// <summary>
        /// 设置轴号
        /// </summary>
        public int Index
        {
            get
            {
                return index;
            }
            set
            {
                index = value;
            }
        }

        public delegate void Del_AxisServo(int index, bool bservo);
        public Del_AxisServo m_DelAxisServo;

        //是否已使能
        bool m_bservo = false;

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
                    m_bservo = axisStatus.enabled;
                }));
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void btn_enabled_Click(object sender, EventArgs e)
        {
            try
            {
                m_DelAxisServo(Index, !m_bservo);
            }
            catch (Exception ex)
            {
                 
            }
        }

    }
}
