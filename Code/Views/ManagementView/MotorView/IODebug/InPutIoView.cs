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
using System.Threading;
using MotionController;
using XMLController;

namespace ManagementView.MotorView
{
    public partial class InPutIoView : UserControl
    {
        public InPutIoView()
        {
            InitializeComponent();
        }

        private IOModel _IoModel;
        public IOModel IoModel
        {
            get { return _IoModel; }
            set
            {
                _IoModel = value;
                if(_IoModel != null)
                {
                    lblIOName.Text = _IoModel.Name;
                }
            }
        }

        private bool _Monitor;
        public bool Monitor
        {
            get { return _Monitor; }
            set
            {
                _Monitor = value;

                if(!_Monitor) 
                {
                    btnIO.Image = Properties.Resources.Red_2;
                }
                StartMonitor(_Monitor);
            }
        }

        IMotorControl m_MotroContorl = MotorInstance.GetInstance();
        public void StartMonitor(bool bstart)
        {
            try
            { 
                if (bstart)
                {
                    timer1.Start();
                }
                else
                {
                    timer1.Stop();
                } 
            }
            catch (Exception ex)
            {

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!Monitor)
                {
                    return;
                }

                //获取实时状态IO 
                var resultModel = m_MotroContorl.Run(IoModel, MotorControlType.QueryDI);
                if (resultModel.RunResult)
                {
                    int value = Int32.Parse(resultModel.ObjectResult.ToString());

                    btnIO.Image = value == 1 ? Properties.Resources.Green_2 : Properties.Resources.Red_2;
                }
            }
            catch (Exception ex)
            {
                 
            } 
        }
    }
}
