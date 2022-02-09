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
using DevComponents.DotNetBar;
using GlobalCore;

namespace ManagementView.MotorView
{
    public partial class OutInPutView : UserControl
    {
        public OutInPutView()
        {
            InitializeComponent();
        }

        private void OutInPutView_Load(object sender, EventArgs e)
        {
            try
            {
                if (OutIoModel_1 != null)
                {
                    outPutIoView1.IoModel = OutIoModel_1;
                }

                outPutIoView1.Enabled = !Global.Run || Global.Pause || Global.Stop;
            }
            catch (Exception ecx)
            {

            }
        }

        IMotorControl m_MotroContorl = MotorInstance.GetInstance();
       
        //输出的两个IOModel
        private IOModel _outModel1;
        public IOModel OutIoModel_1
        {
            get { return _outModel1; }
            set
            {
                _outModel1 = value;
            }
        }         

        //输入的两个IoModel
        private IOModel _InModel1;
        public IOModel InIoModel_1
        {
            get { return _InModel1; }
            set
            {
                _InModel1 = value;
                inPutIoView1.IoModel = _InModel1;
            }
        }

        private IOModel _InModel2;
        public IOModel InIoModel_2
        {
            get { return _InModel2; }
            set
            {
                _InModel2 = value;
                inPutIoView2.IoModel = _InModel2;
            }
        }

        //监视IO
        private bool _Monitor;
        public bool Monitor
        {
            get { return _Monitor; }
            set
            {
                _Monitor = value;
                StartMonitor();
            }
        }

        public void StartMonitor()
        {
            try
            {
                inPutIoView1.Monitor = Monitor;
                inPutIoView2.Monitor = Monitor;
            }
            catch (Exception ex)
            {

            }
        }

        public void SetVisible()
        {
            try
            {
                inPutIoView2.Visible = false;
                this.Width = 289;
            }
            catch (Exception ex)
            {
                 
            }
        }
 
    }
}
