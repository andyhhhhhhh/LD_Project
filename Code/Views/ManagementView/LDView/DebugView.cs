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
using XMLController;

namespace ManagementView
{
    public partial class DebugView : UserControl
    {
        private IMotorControl m_MotroContorl;
        public DebugView()
        {
            InitializeComponent();
        }

        private void DebugView_Load(object sender, EventArgs e)
        {
            try
            {
                var comModel = XMLController.XmlControl.sequenceModelNew.ComModels[0];
                eLight1.ComName = comModel.Name;
                eLight2.ComName = comModel.Name;
                eLight3.ComName = comModel.Name;
                eLight4.ComName = comModel.Name;
                eLight5.ComName = comModel.Name;

                //初始化控制卡
                m_MotroContorl = MotorInstance.GetInstance();
            }
            catch (Exception ex)
            {
                 
            }
        }

        /// <summary>
        /// 下料盘下料位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUnLoadPos_Click(object sender, EventArgs e)
        {
            try
            {
                if(!JudgeUnLoadZSafe())
                {
                    MessageBox.Show("下料Z轴未在安全位");
                    return;
                }
                StationModel stationModel = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_UnLoad);
                PointModel pointModel = stationModel.PointModels.FirstOrDefault(x => x.Name == MotionParam.Pos_Safe);
                stationModel.Axis_Y.pos = pointModel.Pos_Y;

                var resultModel = m_MotroContorl.Run(stationModel.Axis_Y, MotorControlType.AxisMove);
                if (!resultModel.RunResult)
                {

                }
            }
            catch (Exception ex)
            {
                
            }
        }


        /// <summary>
        /// 判断下料模组Z是否在安全位置
        /// </summary>
        /// <param name="bLeft">是否在左边</param>
        /// <returns></returns>
        public bool JudgeUnLoadZSafe()
        {
            try
            {
                StationModel stationModel = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_UnLoadZ);
                PointModel pointModel = stationModel.PointModels.FirstOrDefault(x => x.Name == MotionParam.Pos_Safe);

                var resultModel = m_MotroContorl.Run(stationModel.Axis_X, MotorControlType.AxisGetPosition);
                if (!resultModel.RunResult)
                {
                    return false;
                }
                double dvalue = double.Parse(resultModel.ObjectResult.ToString());

                if (dvalue > pointModel.Pos_X + 0.2)
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
