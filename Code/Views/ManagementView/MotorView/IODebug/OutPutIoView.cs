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
using MotionController;
using DevComponents.DotNetBar;
using GlobalCore;
using XMLController;

namespace ManagementView.MotorView
{
    public partial class OutPutIoView : UserControl
    {
        public OutPutIoView()
        {
            InitializeComponent();
        }

        private void OutPutIoView_Load(object sender, EventArgs e)
        {
            try
            {
                btnIo.Enabled = !Global.Run || Global.Pause || Global.Stop;

                var resultModel = m_MotroContorl.Run(IoModel, MotorControlType.QueryDO);
                if(resultModel.RunResult)
                {
                    int value = Int32.Parse(resultModel.ObjectResult.ToString());
                    btnIo.BackColor = value == 1 ? Color.LightGreen : Color.Transparent;
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        IMotorControl m_MotroContorl = MotorInstance.GetInstance();
        private void btnIo_Click(object sender, EventArgs e)
        {
            try
            {
                bool bEnable = !Global.Run || Global.Pause || Global.Stop;
                if(!bEnable)
                {
                    MessageBoxEx.Show("设备在运行中！");
                    return;
                }

                if (btnIo.BackColor != Color.LightGreen)
                {
                    IoModel.val = 1;
                    var resultModel = m_MotroContorl.Run(IoModel, MotorControlType.IOTrigger);
                    if(resultModel.RunResult)
                    {
                        btnIo.BackColor = Color.LightGreen;
                    }
                }
                else
                {
                    IoModel.val = 0;
                    var resultModel = m_MotroContorl.Run(IoModel, MotorControlType.IOTrigger);
                    if (resultModel.RunResult)
                    {
                        btnIo.BackColor = Color.Transparent;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        
        private IOModel _IoModel;
        public IOModel IoModel
        {
            get { return _IoModel; }
            set
            {
                _IoModel = value;
                if (_IoModel != null)
                {
                    btnIo.Text = _IoModel.Name;
                    OutPutIoView_Load(null, null);
                }
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
                var resultModel = m_MotroContorl.Run(IoModel, MotorControlType.QueryDO);
                if (resultModel.RunResult)
                {
                    int value = Int32.Parse(resultModel.ObjectResult.ToString());
                    btnIo.BackColor = value == 1 ? Color.LightGreen : Color.Transparent;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private bool _Monitor;
        public bool Monitor
        {
            get { return _Monitor; }
            set
            {
                _Monitor = value; 
                StartMonitor(_Monitor);
            }
        }
         
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
         
        private bool JudgeYZPos()
        {
            try
            {
                StationModel stationModel = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_PrePare);

                var resultModel = m_MotroContorl.Run(stationModel.Axis_Y, MotorControlType.AxisGetPosition);
                double dvalue = double.Parse(resultModel.ObjectResult.ToString());

                if (dvalue < 375)
                {
                    return true;
                }

                stationModel = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_Up);
                PointModel pointModel = stationModel.PointModels.FirstOrDefault(x => x.Name == MotionParam.Pos_Safe);

                resultModel = m_MotroContorl.Run(stationModel.Axis_X, MotorControlType.AxisGetPosition);
                dvalue = double.Parse(resultModel.ObjectResult.ToString());

                if (dvalue < pointModel.Pos_X - 0.5)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            { 
                return false;
            }
        }
        
    }
}
