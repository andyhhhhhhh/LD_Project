using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseModels;
using Infrastructure.DBCore;
using SequenceTestModel;
using XMLController;
using System.Diagnostics;
using System.Threading;

namespace MotionController
{
    public class MotorInstance : IMotorControl
    {
        private static MotorInstance m_pInstance;
        /// <summary>
        /// 单例模式
        /// </summary>
        /// <returns></returns>
        public static MotorInstance GetInstance()
        {
            try
            {
                if (m_pInstance == null)
                {
                    m_pInstance = new MotorInstance();
                }
                return m_pInstance;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 运动中间类
        /// </summary>
        MotorControl m_motorControl;

        /// <summary>
        /// 初始化Motor实例
        /// </summary>
        public bool Init(object parameter)
        {
            try
            {
                m_motorControl = MotorControl.GetInstance();
                m_motorControl.Init(parameter);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 运动执行
        /// </summary>
        /// <param name="controlModel">输入参数</param>
        /// <param name="controlType">控制类型</param>
        /// <returns></returns>
        public BaseResultModel Run(BaseMotorModel controlModel, MotorControlType controlType)
        {
            BaseResultModel resultModel = new BaseResultModel();
            int ireturn = 0;
            try
            {
                #region 输入参数判定 

                IOModel ioModel = null;
                AxisParamModel axisModel = null;
                PointModel pointModel = null;
                StationModel stationModel = null;
                RelatIoModel relatIoModel = null;
                int axisNum = 0;
                if (controlModel != null)
                {
                    ioModel = controlModel as IOModel;
                    if (ioModel == null)
                    {
                        relatIoModel = controlModel as RelatIoModel;
                        if (relatIoModel == null)
                        {
                            axisModel = controlModel as AxisParamModel;
                            if (axisModel == null)
                            {
                                pointModel = controlModel as PointModel;
                                if (pointModel != null)
                                {
                                    stationModel = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == pointModel.StationName);
                                    SetStationPos(pointModel, stationModel);
                                    axisNum = stationModel.AxisNum;
                                }
                                else
                                {
                                    stationModel = controlModel as StationModel;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if(controlType != MotorControlType.AllStop && controlType != MotorControlType.AxisAllGoHome && controlType != MotorControlType.Close)
                    {
                        resultModel.RunResult = false;
                        resultModel.ErrorMessage = "输入参数controlModel为null..";
                        return resultModel;
                    } 
                }

                #endregion
                 
                resultModel.RunResult = true;
                string strerrorMsg = "";
                switch (controlType)
                {
                    case MotorControlType.AxisEnable:
                        {
                            if (axisModel != null)//单轴上电
                            {
                                ireturn = m_motorControl.Motor_axis_enable(axisModel.cardIndex, axisModel.axisIndex);
                            }
                            else if (stationModel != null)//模组上电
                            {
                                foreach (var item in GetStationAxis(stationModel))
                                {
                                    ireturn = m_motorControl.Motor_axis_enable(item.cardIndex, item.axisIndex);
                                };
                            }
                        }
                        break;
                    case MotorControlType.AxisDisable:
                        {
                            if (axisModel != null)//单轴下电
                            {
                                ireturn = m_motorControl.Motor_axis_disable(axisModel.cardIndex, axisModel.axisIndex);
                            }
                            else if (stationModel != null)//模组下电
                            {
                                foreach (var item in GetStationAxis(stationModel))
                                {
                                    ireturn = m_motorControl.Motor_axis_disable(item.cardIndex, item.axisIndex);
                                };
                            }
                        }
                        break;
                    case MotorControlType.AxisGoHome:
                        {
                            if (axisModel != null)//单轴回零
                            {
                                ireturn = m_motorControl.Motor_axis_home(axisModel.cardIndex, axisModel.axisIndex, axisModel.homeIo, (float)axisModel.homeVel, (float)axisModel.homeSecondVel);
                            }
                            else if (stationModel != null)//多轴回零
                            {
                                resultModel = StationGoHome(stationModel);
                                if (!resultModel.RunResult)
                                {
                                    ireturn = -1;
                                }
                            }
                        }
                        break;
                    case MotorControlType.AxisAllGoHome:
                        {
                            resultModel = AxisAllGoHome(strerrorMsg);
                            if(!resultModel.RunResult)
                            {
                                ireturn = -1;
                            }
                        }
                        break;
                    case MotorControlType.AxisMove:
                        {
                            if (axisModel != null)//单轴移动
                            {
                                TSpeed pspeed = new TSpeed()
                                {
                                    vel = axisModel.vel,
                                    acc = axisModel.maxAcc,
                                    dec = axisModel.maxDec
                                };
                                ireturn = m_motorControl.Motor_axis_move_pos(axisModel.cardIndex, axisModel.axisIndex, axisModel.pos, pspeed);
                                string str = WaitMoveFinish(axisModel);
                                if(!string.IsNullOrEmpty(str))
                                {
                                    resultModel.ErrorMessage = str;
                                    ireturn = -1;
                                }
                            }
                            else if (stationModel != null)//模组移动
                            {
                               string str = StationMove(stationModel, true);
                                if (!string.IsNullOrEmpty(str))
                                {
                                    resultModel.ErrorMessage = str;
                                    ireturn = -1;
                                }
                            }
                        }
                        break;
                    case MotorControlType.AxisMoveRel:
                        {
                            if (axisModel != null)//单轴移动
                            {
                                TSpeed pspeed = new TSpeed()
                                {
                                    vel = axisModel.vel,
                                    acc = axisModel.maxAcc,
                                    dec = axisModel.maxDec
                                };
                                ireturn = m_motorControl.Motor_axis_move_offset(axisModel.cardIndex, axisModel.axisIndex, axisModel.relPos, pspeed, 0);
                            } 
                        }
                        break;
                    case MotorControlType.AxisMoveJog:
                        {
                            if (axisModel != null)//Jog移动
                            {
                                TSpeed pspeed = new TSpeed()
                                {
                                    vel = axisModel.vel,
                                    acc = axisModel.maxAcc,
                                    dec = axisModel.maxDec
                                };

                                ireturn = m_motorControl.Motor_axis_vmove(axisModel.cardIndex, axisModel.axisIndex, axisModel.dir, pspeed);
                            }
                        }
                        break;
                    case MotorControlType.AxisMoveWithOutZ:
                        {

                        }
                        break;
                    case MotorControlType.AxisWaitMoveDone:
                        {
                            if(axisModel != null)//单轴等待停止
                            {
                                string str = WaitMoveFinish(axisModel);
                                if (!string.IsNullOrEmpty(str))
                                {
                                    resultModel.ErrorMessage = str;
                                    ireturn = -1;
                                }
                            }
                            else if(stationModel != null)//工站等待停止
                            {
                               var listAxis = GetStationAxis(stationModel);
                                foreach (var axis in listAxis)
                                {
                                    string str = WaitMoveFinish(axis);
                                    if (!string.IsNullOrEmpty(str))
                                    {
                                        resultModel.ErrorMessage += str;
                                        ireturn = -1;
                                    }
                                }
                            }
                        }
                        break;
                    case MotorControlType.AxisMoveNotWait:
                        {
                            if (axisModel != null)//单轴移动
                            {
                                TSpeed pspeed = new TSpeed()
                                {
                                    vel = axisModel.vel,
                                    acc = axisModel.maxAcc,
                                    dec = axisModel.maxDec
                                };
                                ireturn = m_motorControl.Motor_axis_move_pos(axisModel.cardIndex, axisModel.axisIndex, axisModel.pos, pspeed);
                            }
                            else if (stationModel != null)//模组移动
                            {
                                string str = StationMove(stationModel, false);
                                if (!string.IsNullOrEmpty(str))
                                {
                                    resultModel.ErrorMessage = str;
                                    ireturn = -1;
                                }
                            }
                        }
                        break;
                    case MotorControlType.AxisMoveWithOutZNotWait:
                        {

                        }
                        break;
                    case MotorControlType.AxisStop:
                        {
                            if (axisModel != null)//单轴停止
                            {
                                ireturn = m_motorControl.Motor_axis_stop(axisModel.cardIndex, axisModel.axisIndex, 0);
                            }
                            else if (stationModel != null)//多轴停止
                            {
                                foreach (var item in GetStationAxis(stationModel))
                                {
                                    ireturn = m_motorControl.Motor_axis_stop(item.cardIndex, item.axisIndex, 0);
                                }
                            }
                        }
                        break;
                    case MotorControlType.AllStop:
                        {
                            foreach (var station in XmlControl.controlCardModel.StationModels)
                            {
                                foreach (var item in GetStationAxis(station))
                                {
                                    ireturn = m_motorControl.Motor_axis_stop(item.cardIndex, item.axisIndex, 0);
                                }
                            }
                        }
                        break;
                    case MotorControlType.AxisReset:
                        {
                            if (axisModel != null)//单轴复位
                            {
                                ireturn = m_motorControl.Motor_axis_reset(axisModel.cardIndex, axisModel.axisIndex);
                            }
                            else if (stationModel != null)//多轴复位
                            {
                                foreach (var item in GetStationAxis(stationModel))
                                {
                                    ireturn = m_motorControl.Motor_axis_reset(item.cardIndex, item.axisIndex);
                                }
                            }
                        }
                        break;
                    case MotorControlType.AxisSetHome:
                        {
                            if (axisModel != null)//单轴复位
                            {
                                bool bvalue = m_motorControl.Motor_set_axis_zero_pos(axisModel.cardIndex, axisModel.axisIndex);
                                ireturn = bvalue ? 0 : -1;
                            }
                            else if (stationModel != null)//多轴复位
                            {
                                foreach (var item in GetStationAxis(stationModel))
                                {
                                    bool bvalue = m_motorControl.Motor_set_axis_zero_pos(item.cardIndex, item.axisIndex);
                                    ireturn = bvalue ? 0 : -1;
                                }
                            }
                        }
                        break;
                    case MotorControlType.AxisEmergency:
                        {
                            if (axisModel != null)//单轴停止
                            {
                                ireturn = m_motorControl.Motor_axis_stop(axisModel.cardIndex, axisModel.axisIndex, 1);
                            }
                            else if (stationModel != null)//多轴停止
                            {
                                foreach (var item in GetStationAxis(stationModel))
                                {
                                    ireturn = m_motorControl.Motor_axis_stop(item.cardIndex, item.axisIndex, 1);
                                }
                            }
                        }
                        break;
                    case MotorControlType.AxisStatus:
                        {
                            if (axisModel != null)
                            {
                                bool bok = true;
                                resultModel.ObjectResult = m_motorControl.GetStatus(axisModel.axisIndex, out bok);
                                ireturn = bok ? 0 : -1;
                            }
                        }
                        break;
                    case MotorControlType.AxisGetPosition:
                        {
                            if (axisModel != null)
                            {
                                double dpval = 0;
                                ireturn = m_motorControl.Motor_get_current_pos(axisModel.cardIndex, axisModel.axisIndex, out dpval, 0);
                                resultModel.ObjectResult = dpval;
                            }
                        }
                        break;
                    case MotorControlType.AxisGetEncodePosition:
                        {
                            if (axisModel != null)
                            {
                                double dpval = 0;
                                ireturn = m_motorControl.Motor_get_current_pos(axisModel.cardIndex, axisModel.axisIndex, out dpval, 1);
                                resultModel.ObjectResult = dpval;
                            }
                        }
                        break;
                    case MotorControlType.IOTrigger:
                        {
                            if (ioModel != null)
                            {
                                ireturn = m_motorControl.Motor_write_out_bit((ushort)ioModel.cardIndex, (ushort)ioModel.index, ioModel.val, (short)ioModel.extIndex, 1);
                            }
                        }
                        break;
                    case MotorControlType.IOTriggerTime:
                        {
                            if (relatIoModel != null)
                            {
                                string str = RelatedIOTrig(relatIoModel);
                                if (str != "")
                                {
                                    ireturn = -1;
                                }
                            }
                        }
                        break;
                    case MotorControlType.QueryDI:
                        {
                            if (ioModel != null)
                            {
                                int value = m_motorControl.Motor_read_in_bit((ushort)ioModel.cardIndex, (ushort)ioModel.index, (short)ioModel.extIndex);
                                resultModel.ObjectResult = value;
                                if(value != -1)
                                {
                                    ireturn = 0;
                                }
                            }
                        }
                        break;
                    case MotorControlType.QueryDITime:
                        {
                            if (ioModel != null)
                            {
                                int value = ReadDITimeOut(ioModel);
                                resultModel.ObjectResult = value; 
                                if (value != -1)
                                {
                                    ireturn = 0;
                                }
                            }
                        }
                        break;
                    case MotorControlType.QueryDO:
                        {
                            if (ioModel != null)
                            {
                                int value = m_motorControl.Motor_read_out_bit((ushort)ioModel.cardIndex, (ushort)ioModel.index, (short)ioModel.extIndex, 0);
                                resultModel.ObjectResult = value;
                                if (value != -1)
                                {
                                    ireturn = 0;
                                }
                            }
                        }
                        break;
                    case MotorControlType.Close:
                        {

                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                resultModel.ErrorMessage = ex.Message;
                resultModel.RunResult = false;
            }

            resultModel.RunResult = ireturn == 0;
            return resultModel;
        }
        
        #region 私有运动处理方法
        /// <summary>
        /// 获取工站中的所有轴
        /// </summary>
        /// <param name="stationModel">输入工站</param>
        /// <returns>返回所有轴</returns>
        private List<AxisParamModel> GetStationAxis(StationModel stationModel)
        {
            List<AxisParamModel> listAxis = new List<AxisParamModel>();
            try
            {
                switch (stationModel.AxisNum)
                {
                    case 1:
                        listAxis.Add(stationModel.Axis_X);
                        break;
                    case 2:
                        listAxis.Add(stationModel.Axis_X);
                        listAxis.Add(stationModel.Axis_Y);
                        break;
                    case 3:
                        listAxis.Add(stationModel.Axis_X);
                        listAxis.Add(stationModel.Axis_Y);
                        listAxis.Add(stationModel.Axis_Z);
                        break;
                    case 4:
                        listAxis.Add(stationModel.Axis_X);
                        listAxis.Add(stationModel.Axis_Y);
                        listAxis.Add(stationModel.Axis_Z);
                        listAxis.Add(stationModel.Axis_U);
                        break;

                    default:
                        listAxis = null;
                        break;
                }

                return listAxis;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取多个工站的所有轴
        /// </summary>
        /// <param name="listModel">工站List</param>
        /// <returns>返回所有轴</returns>
        private List<AxisParamModel> GetStationAxis(List<StationModel> listModel)
        {
            List<AxisParamModel> listAxis = new List<AxisParamModel>();
            try
            {
                foreach (var stationModel in listModel)
                {
                    switch (stationModel.AxisNum)
                    {
                        case 1:
                            listAxis.Add(stationModel.Axis_X);
                            break;
                        case 2:
                            listAxis.Add(stationModel.Axis_X);
                            listAxis.Add(stationModel.Axis_Y);
                            break;
                        case 3:
                            listAxis.Add(stationModel.Axis_X);
                            listAxis.Add(stationModel.Axis_Y);
                            listAxis.Add(stationModel.Axis_Z);
                            break;
                        case 4:
                            listAxis.Add(stationModel.Axis_X);
                            listAxis.Add(stationModel.Axis_Y);
                            listAxis.Add(stationModel.Axis_Z);
                            listAxis.Add(stationModel.Axis_U);
                            break;

                        default:
                            listAxis = null;
                            break;
                    }
                }

                return listAxis;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 轴移动除了Z轴
        /// </summary>
        /// <param name="stationModel">输入工站</param>
        /// <returns></returns>
        private List<AxisParamModel> GetStationAxisNoZ(StationModel stationModel)
        {
            List<AxisParamModel> listAxis = new List<AxisParamModel>();
            try
            {
                switch (stationModel.AxisNum)
                {
                    case 1:
                        if (stationModel.Axis_X.Id != MotionParam.AxisZ1No)
                        {
                            listAxis.Add(stationModel.Axis_X);
                        }
                        break;
                    case 2:
                        if (stationModel.Axis_X.Id != MotionParam.AxisZ1No)
                        {
                            listAxis.Add(stationModel.Axis_X);
                        }
                        if (stationModel.Axis_Y.Id != MotionParam.AxisZ1No)
                        {
                            listAxis.Add(stationModel.Axis_Y);
                        }
                        break;
                    case 3:
                        if (stationModel.Axis_X.Id != MotionParam.AxisZ1No)
                        {
                            listAxis.Add(stationModel.Axis_X);
                        }
                        if (stationModel.Axis_Y.Id != MotionParam.AxisZ1No)
                        {
                            listAxis.Add(stationModel.Axis_Y);
                        }
                        if (stationModel.Axis_Z.Id != MotionParam.AxisZ1No)
                        {
                            listAxis.Add(stationModel.Axis_Z);
                        }
                        break;
                    case 4:
                        if (stationModel.Axis_X.Id != MotionParam.AxisZ1No)
                        {
                            listAxis.Add(stationModel.Axis_X);
                        }
                        if (stationModel.Axis_Y.Id != MotionParam.AxisZ1No)
                        {
                            listAxis.Add(stationModel.Axis_Y);
                        }
                        if (stationModel.Axis_Z.Id != MotionParam.AxisZ1No)
                        {
                            listAxis.Add(stationModel.Axis_Z);
                        }
                        if (stationModel.Axis_U.Id != MotionParam.AxisZ1No)
                        {
                            listAxis.Add(stationModel.Axis_U);
                        }
                        break;

                    default:
                        listAxis = null;
                        break;
                }

                return listAxis;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 所有轴回零运动
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private BaseResultModel AxisAllGoHome(string errorMessage)
        {
            BaseResultModel resultModel = new BaseResultModel();
            try
            {
                List<AxisParamModel> listAxis = new List<AxisParamModel>();

                //先回零带Z轴的模组
                //List<StationModel> listModel = new List<StationModel>();
                //StationModel stationZ = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_Load);
                //StationModel stationSphere1 = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_UnLoad);
                //listModel.Add(stationZ);
                //listModel.Add(stationSphere1);
                //listModel.Add(stationSphere1);

                //List<AxisParamModel> listAxis = GetStationAxis(listModel);

                //resultModel = AllAxisGoHome(listAxis);
                //if(resultModel.RunResult)
                //{
                //    return resultModel;
                //}

                //再回零不带Z轴的模组
                //StationModel stationXY = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_PrePare);
                ////StationModel stationChange = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_Change);
                //StationModel stationR1 = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_R1);
                //StationModel stationR2 = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_R2);
                //StationModel stationR3 = XmlControl.controlCardModel.StationModels.FirstOrDefault(x => x.Name == MotionParam.Station_R3);

                List<StationModel> listModel2 = new List<StationModel>();
                //listModel2.Add(stationXY);
                ////listModel2.Add(stationChange);
                //listModel2.Add(stationR1);
                //listModel2.Add(stationR2);
                //listModel2.Add(stationR3);

                listAxis = GetStationAxis(listModel2);

                resultModel = AllAxisGoHome(listAxis); 

                return resultModel;
            }
            catch (Exception ex)
            {
                resultModel.ErrorMessage = ex.Message;
                resultModel.RunResult = false;
                return resultModel;
            }
        }

        /// <summary>
        /// 工站回零
        /// </summary>
        /// <param name="stationModel">输入工站</param>
        private BaseResultModel StationGoHome(StationModel stationModel)
        {
            BaseResultModel resultModel = new BaseResultModel();
            try
            { 
                List<AxisParamModel> listAxis = GetStationAxisNoZ(stationModel); 

                resultModel = AllAxisGoHome(listAxis);

                return resultModel;
            }
            catch (Exception ex)
            {
                resultModel.RunResult = false;
                resultModel.ErrorMessage = ex.Message;
                return resultModel;
            }
        }

        /// <summary>
        /// 多轴回零
        /// </summary>
        /// <param name="listAxis">轴List</param>
        /// <returns>返回结果</returns>
        private BaseResultModel AllAxisGoHome(List<AxisParamModel> listAxis)
        {
            BaseResultModel resultModel = new BaseResultModel();
            try
            {
                string error = "";
                int ivalue = 0;
                Parallel.ForEach(listAxis, new Action<AxisParamModel>(axis =>
                { 
                    int ireturn = m_motorControl.Motor_axis_home(axis.cardIndex, (ushort)axis.Id, axis.homeIo, (float)axis.homeVel, (float)axis.homeSecondVel);
                    if (ireturn != 0)
                    {
                        ivalue = ireturn;
                    }

                    if (ireturn != 0)
                        error += "轴" + axis.Id + "回零失败";

                    //查询轴是否在忙
                    error += WaitMoveFinish(axis);
                    if (!string.IsNullOrEmpty(error))
                    { 
                        ivalue = -1;
                    }
                }));

                resultModel.ErrorMessage = error;
                if(string.IsNullOrEmpty(error))
                {
                    resultModel.RunResult = true;
                }

                return resultModel;
            }
            catch (Exception ex)
            {
                resultModel.RunResult = false;
                resultModel.ErrorMessage = ex.Message;
                return resultModel;
            }
        }

        /// <summary>
        /// 工站绝对移动
        /// </summary>
        /// <param name="stationModel">输入工站</param>
        private string StationMove(StationModel stationModel, bool bwait)
        {
            try
            { 
                string errorMessage = "";
                string errorMessageTotal = "";

                List<AxisParamModel> axisList = GetStationAxis(stationModel);

                Parallel.ForEach(axisList, new Action<AxisParamModel>((o) =>
                {
                    string error = "";
                    try
                    {
                        AxisParamModel p = o as AxisParamModel;
                        if (p == null)
                        {
                            errorMessageTotal = "对象为空"; 
                            return;
                        }

                        TSpeed tspeed = new TSpeed()
                        {
                            dec = p.maxDec,
                            acc = p.maxAcc,
                            vel = p.maxVel * stationModel.PercentSpeed,
                        };
                        if (m_motorControl.Motor_axis_move_pos(p.cardIndex, p.axisIndex, p.pos, tspeed) != 0)
                        {
                            //移动失败再移动一次
                            if (m_motorControl.Motor_axis_move_pos(p.cardIndex, p.axisIndex, p.pos, tspeed) != 0)
                            {
                                error += p.Name + "运动失败"; 
                            }
                        }
                        
                        //查询轴是否在忙
                        if(bwait)
                        {
                            error += WaitMoveFinish(p);
                            Thread.Sleep(80);
                            double pcurrentpos = 0;
                            int ivalue = m_motorControl.Motor_get_current_pos(p.cardIndex, p.axisIndex, out pcurrentpos);
                            if (ivalue == 0)
                            {
                                if(Math.Abs(pcurrentpos - p.pos) > 0.2)
                                {
                                    error += p.Name + ":未走到位";
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    { 
                        error += ex.Message;
                    }
                    finally
                    {
                        errorMessageTotal += error;
                    }
                }));
                 

                errorMessage = errorMessageTotal;

                return errorMessage;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 根据PointModel设置轴位置
        /// </summary>
        /// <param name="pointModel">点位Model</param>
        /// <param name="stationModel">工站Model</param>
        private void SetStationPos(PointModel pointModel, StationModel stationModel)
        {
            try
            {
                if(stationModel.Axis_X != null)
                {
                    stationModel.Axis_X.pos = pointModel.Pos_X;
                }
                if (stationModel.Axis_Y != null)
                {
                    stationModel.Axis_Y.pos = pointModel.Pos_Y;
                }
                if (stationModel.Axis_Z != null)
                {
                    stationModel.Axis_Z.pos = pointModel.Pos_Z;
                }
                if (stationModel.Axis_U != null)
                {
                    stationModel.Axis_U.pos = pointModel.Pos_U;
                }
            }
            catch (Exception ex)
            {
                 
            }
        }

        /// <summary>
        /// 等待运动完成
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string WaitMoveFinish(AxisParamModel p)
        {
            string error = "";
            Stopwatch sp = new Stopwatch();
            sp.Start();
            while (true)
            {
                if (sp.ElapsedMilliseconds > 20000)
                {
                    error = "等待轴到位超时";
                    break;
                }
                Thread.Sleep(30);
                if (m_motorControl.Motor_axis_is_moving((ushort)p.cardIndex, (ushort)p.axisIndex) == 0)
                {
                    break;
                }
            }
            sp.Stop();
            return error;
        }

        private string RelatedIOTrig(RelatIoModel relatIo)
        {
            try
            {
                string error = "";
                var outIo = relatIo.OutIoModel;
                var inIo = relatIo.InIoModel1;
                var ireturn = m_motorControl.Motor_write_out_bit((ushort)outIo.cardIndex, (ushort)outIo.index, outIo.val, (short)outIo.extIndex, 1);
                if (ireturn != 0)
                {
                    error += outIo.Name + "写入值失败";
                }
                if (relatIo.TimeOut == 0)
                {
                    relatIo.TimeOut = 1000;
                }

                Stopwatch sp = new Stopwatch();
                sp.Start();
                while (relatIo.IsWaitTimeOut)
                {
                    if (sp.ElapsedMilliseconds > relatIo.TimeOut)
                    {
                        error += inIo.Name + " 等待1超时";
                        break;
                    }
                    int value = m_motorControl.Motor_read_in_bit((ushort)inIo.cardIndex, (ushort)inIo.index, (short)inIo.extIndex);
                    if (value == -1)
                    {
                        error += inIo.Name + " 读失败";
                        break;
                    }
                    else if(value == 1)
                    {
                        break;
                    }

                    Thread.Sleep(20);
                }
                sp.Stop();

                return error;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private int ReadDITimeOut(IOModel inIo)
        {
            try
            {
                Stopwatch sp = new Stopwatch();
                sp.Start();
                int value = 0;
                while (true)
                {
                    if (sp.ElapsedMilliseconds > 2000)
                    {
                        return value;
                    }
                    value = m_motorControl.Motor_read_in_bit((ushort)inIo.cardIndex, (ushort)inIo.index, (short)inIo.extIndex);
                    if (value == -1)
                    {
                        break;
                    }
                    else if (value == 1)
                    {
                        break;
                    }

                    Thread.Sleep(10);
                }
                sp.Stop();

                return value;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        
        #endregion
        
    }
}
