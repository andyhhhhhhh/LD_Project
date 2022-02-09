using BaseController;
using BaseModels;
using Infrastructure.DBCore;
using Infrastructure.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using HalconDotNet;
using System.Threading;
using SequenceTestModel;

namespace CameraContorller
{
    public class CameraByHalconControl : BaseControl, ICameraControl
    {
        public CameraByHalconControl()
        {
        }

        #region 事件
        public event EventHandler<object> GetImageByContinue;
        public event EventHandler<object> GetImageBySoft;
        public event EventHandler<object> GetImageByTrigger;
        protected void OnGetImageByTrigger(object e)
        {
            EventHandler<object> handler = GetImageByTrigger;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected void OnGetImageBySoft(object e)
        {
            EventHandler<object> handler = GetImageBySoft;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected void OnGetImageByContinue(object e)
        {
            EventHandler<object> handler = GetImageByContinue;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        #endregion

        public bool Init(object parameter)
        {
            return true;
        }

        public BaseResultModel Run(Camera2DSetModel controlModel, CameraControlType type)
        {
            CameraResultModel resultModel = new CameraResultModel();
            resultModel.RunResult = true;
            Camera2DSetModel cameraModel = controlModel as Camera2DSetModel;
            if (type != CameraControlType.CameraDiscover && cameraModel == null)
            {
                throw new ArgumentException("CameraModel参数为空！或类型不匹配！");
            }
            string errorMessage = "";
            //Log.WriteLog(LogLevel.Debug, "【" + controlModel.Name + "】执行【" + type.ToString() + "】方法");
            switch (type)
            {
                case CameraControlType.CameraDiscover:
                    resultModel.ObjectResult = GetCameras(cameraModel.InterfaceName);
                    break;
                case CameraControlType.CameraStartSnapBySoft: 
                    HObject image = Snap(cameraModel, ref errorMessage);
                    if (image != null && !cameraModel.IsExternTrig)
                    {
                        resultModel.Image = image;
                    }
                    else
                    {
                        if(!cameraModel.IsExternTrig)
                        {
                            resultModel.ErrorMessage = errorMessage;
                            resultModel.RunResult = false;
                        }
                    }
                    break;

                case CameraControlType.CameraGetImageByTirgger:
                    HObject imageTrig = CameraGetTrigger(cameraModel, ref errorMessage);
                    if (imageTrig != null)
                    {
                        resultModel.Image = imageTrig;
                    }
                    else
                    {
                        resultModel.ErrorMessage = errorMessage;
                        resultModel.RunResult = false;
                    }
                    break;

                case CameraControlType.CameraOpenBySoft:
                case CameraControlType.CameraOpenContinue: 
                    if(GetHandleInstance(cameraModel, ref errorMessage) == null)
                    {
                        resultModel.RunResult = false;
                        resultModel.ErrorMessage = "打开相机失败;" + errorMessage;
                    }
                    else
                    {
                        resultModel.RunResult = true;
                    }
                    break;
                case CameraControlType.CameraStartContinue:
                    {
                        CameraOpenByContinue(cameraModel, ref errorMessage);
                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            resultModel.RunResult = false;
                            resultModel.ErrorMessage = "启动连续采集失败;" + errorMessage;
                        }
                    }
                    break;
                case CameraControlType.CameraSetParam:
                    if (!RefreshExposureAndGain(cameraModel, ref errorMessage))
                    {
                        resultModel.RunResult = false;
                        resultModel.ErrorMessage = "设置曝光失败;" + errorMessage;
                    }
                    break;
                case CameraControlType.CameraClose:
                    if (!CloseDevice(cameraModel))
                    {
                        resultModel.RunResult = false;
                        resultModel.ErrorMessage = "关闭相机失败;" + errorMessage;
                    }
                    break;
                case CameraControlType.CloseCamera:
                    if (!CloseCamera(cameraModel))
                    {
                        resultModel.RunResult = false;
                        resultModel.ErrorMessage = "关闭相机失败;" + errorMessage;
                    }
                    break;
                default:
                    throw new InvalidOperationException("无效的操作类型:" + type.ToString() + "，不适用于相机操作");
            }
            return resultModel;
        }

        #region 私有成员函数
        private static List<Camera2DSetModel> m_CameraModelIsOpenList = new List<Camera2DSetModel>();
        private HTuple GetHandleInstance(Camera2DSetModel cameraModel, ref string errorMsg)
        {
            try
            {
                if (cameraModel == null)
                {
                    throw new ArgumentNullException("输入参数SerialPortParamModel为空");
                }
                if (m_CameraModelIsOpenList == null)
                {
                    m_CameraModelIsOpenList = new List<Camera2DSetModel>();
                }
                var cameraModelTemp = m_CameraModelIsOpenList.FirstOrDefault(x => x.DeviceName == cameraModel.DeviceName);
                if (cameraModelTemp == null)
                {
                    if (OpenDevice(cameraModel))
                    {
                        m_CameraModelIsOpenList.Add(cameraModel);
                    }
                }
                else
                {
                    cameraModel.IsOpen = cameraModelTemp.IsOpen;
                    cameraModel.AcqHandle = cameraModelTemp.AcqHandle;
                }

                return cameraModel.AcqHandle;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return null;
            }
        } 

        private List<string> GetCameras(string selectName/*, out string expParamName, out string gainParamName*/)
        {
            List<string> list = new List<string>();
            HTuple information = null, valueList = null;
            try
            {
                if (string.IsNullOrEmpty(selectName))
                {
                    throw new Exception("未选择接口类型");
                } 
                if(selectName == "MVision" || selectName == "HMV3rdParty")
                { 
                    HOperatorSet.InfoFramegrabber(selectName, "device", out information, out valueList);
                }
                else
                {
                    HOperatorSet.InfoFramegrabber(selectName, "info_boards", out information, out valueList);
                }
                if (valueList.Length == 0)
                {
                    throw new Exception("未查找到【" + selectName + "】接口类型相关设备");
                }
                if (selectName == "MVision" || selectName == "HMV3rdParty")
                {
                    for (int i = 0; i < valueList.Length; i++)
                    {
                        list.Add(valueList[i].S);
                    }
                }
                else
                {
                    for (int i = 0; i < valueList.Length; i++)
                    {
                        string[] array = valueList[i].S.Split('|');
                        if (array != null && array.Length > 2)
                        {
                            for (int j = 0; j < array.Length; j++)
                            {
                                if (array[j].Contains("device:"))
                                {
                                    string device = array[j].Trim().Replace("device:", "");
                                    list.Add(device);
                                }
                            }
                        }
                    }
                }
              
                return list;
            }
            catch (Exception ex)
            {
                return list;
            }
        }
        private bool OpenDevice(Camera2DSetModel cameraModel)
        {
            bool result = false;
            if (cameraModel.IsOpen)
            {
                result = true;
            }
            else
            {
                try
                {
                    if (cameraModel.bLocalImage)
                    {
                        cameraModel.IsOpen = true;
                        result = true;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(cameraModel.UniqueName) && cameraModel.UniqueName != "NA")
                        {
                            HTuple acqHandle;
                            if (cameraModel.InterfaceName == null)
                            {
                               // GigeVision2
                                HOperatorSet.OpenFramegrabber("GigEVision2", 0, 0, 0, 0, 0, 0, "default", -1, "default", -1, "false", "default", cameraModel.UniqueName, 0, -1, out acqHandle);
                            }
                            else
                            { 
                                switch (cameraModel.InterfaceName)
                                {
                                    case "USB3Vision":
                                        HOperatorSet.OpenFramegrabber(cameraModel.InterfaceName, 0, 0, 0, 0, 0, 0, "progressive",
                                            -1, "default", -1, "false", "default", cameraModel.UniqueName,
                                            0, -1, out acqHandle);
                                        break;

                                    case "GigEVision2":
                                        HOperatorSet.OpenFramegrabber(cameraModel.InterfaceName, 0, 0, 0, 0, 0, 0, "default", -1, "default", -1, "false", "default", cameraModel.UniqueName, 0, -1, out acqHandle);
                                        break;

                                    case "MVision"://海康相机
                                    case "HMV3rdParty"://大华相机
                                        HOperatorSet.OpenFramegrabber(cameraModel.InterfaceName, 0, 0, 0, 0, 0, 0, "default", -1, "default", -1, "false", "default", cameraModel.UniqueName, 0, -1, out acqHandle);
                                        break;

                                    default:
                                        throw new ArgumentNullException("没有匹配的相机接口");
                                }
                            }
                            if (cameraModel.InterfaceName =="1394IIDC")
                            {
                                HOperatorSet.SetFramegrabberParam(acqHandle, "camera_type", "7:0:0");
                            }

                            //是否为外部触发
                            if(cameraModel.IsExternTrig)
                            { 
                                HOperatorSet.SetFramegrabberParam(acqHandle, "TriggerMode", "On");
                                HOperatorSet.SetFramegrabberParam(acqHandle, "TriggerSource", "Line0");
                                HOperatorSet.SetFramegrabberParam(acqHandle, "TriggerSelector", "FrameBurstStart");
                                HOperatorSet.SetFramegrabberParam(acqHandle, "TriggerActivation", "RisingEdge");

                                HOperatorSet.GrabImageStart(acqHandle, -1);
                            }

                            cameraModel.AcqHandle = acqHandle;
                            cameraModel.IsOpen = true;
                            result = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return result;
        }

        private bool CloseDevice(Camera2DSetModel cameraModel)
        {
            bool result = false;
            if (cameraModel.bLocalImage)
            {
                return true;
            }
            if (!cameraModel.IsOpen)
            {
                result = (true);
            }
            else if (cameraModel.AcqHandle == null)
            {
                result = false;
            }
            else
            {
                try
                {
                    if (cameraModel.bLocalImage)
                    {
                        result = true;
                    }
                    else
                    {
                        Thread.Sleep(1);
                        HOperatorSet.CloseFramegrabber(cameraModel.AcqHandle);
                    }
                    Camera2DSetModel cameraparamModel = m_CameraModelIsOpenList.Find(x => x.DeviceName == cameraModel.DeviceName);
                    if (cameraparamModel != null)
                    {
                        m_CameraModelIsOpenList.Remove(cameraparamModel);
                    }
                    cameraModel.IsContinue = false;
                    cameraModel.IsOpen = false;
                    result = true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return result;
        }

        private bool CloseCamera(Camera2DSetModel cameraModel)
        {
            try
            {
                Camera2DSetModel cameraparamModel = m_CameraModelIsOpenList.Find(x => x.DeviceName == cameraModel.DeviceName);
                if (cameraparamModel != null)
                {
                    HOperatorSet.CloseFramegrabber(cameraparamModel.AcqHandle);
                    m_CameraModelIsOpenList.Remove(cameraparamModel);
                }
                cameraModel.IsContinue = false;
                cameraModel.IsOpen = false;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
           
        }

        private HObject Snap(Camera2DSetModel cameraModel, ref string errorMsg)
        {
            try
            {
                HObject imageObj = null;
                if (cameraModel.bLocalImage)
                {
                    HOperatorSet.ReadImage(out imageObj, cameraModel.LocalPath);
                }
                else
                {
                    if (!cameraModel.IsOpen)
                    {
                        throw new InvalidOperationException("相机没有被打开！");
                    }

                    if(cameraModel.IsExternTrig)
                    {
                        return imageObj;
                    }
                    else
                    {
                        HOperatorSet.GrabImage(out imageObj, cameraModel.AcqHandle);
                    }
                }
                return imageObj;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return null;
            }
        }

        private HObject CameraGetTrigger(Camera2DSetModel cameraModel, ref string errorMsg)
        {
            try
            {
                HObject imageObj = null;
                if (!cameraModel.IsOpen)
                {
                    throw new InvalidOperationException("相机没有被打开！");
                }

                HOperatorSet.GrabImageAsync(out imageObj, cameraModel.AcqHandle, -1);
                
                return imageObj;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return null;
            }
        }

        private void CameraOpenByContinue(Camera2DSetModel cameraModel, ref string errorMsg)
        {
            if (!cameraModel.IsOpen)
            {
                errorMsg = "相机没有打开";
                return;
            }
            if (cameraModel.IsContinue)
            {
                errorMsg = "相机正在执行连续采集";
                return;
            }
            cameraModel.IsContinue = true;
            Thread t = new Thread(new ThreadStart(() =>
            {
                while (cameraModel.IsContinue)
                {
                    string error = "";
                    var hImage = Snap(cameraModel, ref error);
                    if (!string.IsNullOrEmpty(error))
                    {
                        break;
                    }
                    List<object> list = new List<object>() { cameraModel.Id, hImage };
                    OnGetImageByContinue(list);
                    Thread.Sleep(100);
                }
            }));
            t.Start();
        }

        private bool SetDevieceExposureTime(Camera2DSetModel cameraModel, HTuple value, int nSleepTimeMM, ref string errorMsg)
        {
            bool result = false;
            try
            {
                if (value.Length == 0)
                {
                    errorMsg = "曝光参数为空!";
                    return false;
                }
                if (!cameraModel.IsOpen)
                {
                    result = false;
                    errorMsg = "Device is not open!";
                }
                else
                {
                    if (cameraModel.bLocalImage)
                    {
                        result = true;
                    }
                    else
                    {
                        cameraModel.ExposureTime = value.I;
                        double dvalue = cameraModel.ExposureTime + 0.1;
                        HOperatorSet.SetFramegrabberParam(cameraModel.AcqHandle, cameraModel.ExposureParamName, dvalue);
                        Thread.Sleep(nSleepTimeMM);

                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                throw ex;
            }
            return result;
        }

        private bool GetDevieceExposureTime(Camera2DSetModel cameraModel, ref HTuple value, ref string errorMsg)
        {
            bool result = false;
            try
            {
                if (!cameraModel.IsOpen)
                {
                    result = false;
                    errorMsg = "Device is not open!";
                }
                else
                {
                    if (cameraModel.bLocalImage)
                    {
                        result = true;
                    }
                    else
                    {
                        HOperatorSet.GetFramegrabberParam(cameraModel.AcqHandle, cameraModel.ExposureParamName, out value);
                    }
                    result = true;
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                throw ex;
            }

            return result;
        }

        private bool SetDevieceGainRaw(Camera2DSetModel cameraModel, HTuple value, int nSleepTimeMM, ref string errorMsg)
        {
            bool result = false;
            try
            {
                if (value.Length == 0)
                {
                    errorMsg = "曝光参数为空!";
                    return false;
                }
                if (!cameraModel.IsOpen)
                {
                    result = false;
                    errorMsg = "Device is not open!";
                }
                else
                {
                    if (cameraModel.bLocalImage)
                    {
                        result = true;
                    }
                    else
                    {
                        cameraModel.Gain = value.I;
                        double dvalue = value.I + 0.1;
                        HOperatorSet.SetFramegrabberParam(cameraModel.AcqHandle, cameraModel.GainParamName, dvalue);
                        Thread.Sleep(nSleepTimeMM);

                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                throw ex;
            }
            return result;
        }

        private bool GetDevieceGainRaw(Camera2DSetModel cameraModel, ref HTuple value, ref string errorMsg)
        {
            bool result = false;
            try
            {
                if (!cameraModel.IsOpen)
                {
                    result = false;
                    errorMsg = "Device is not open!";
                }
                else
                {
                    if (cameraModel.bLocalImage)
                    {
                        result = true;
                    }
                    else
                    {
                        HOperatorSet.GetFramegrabberParam(cameraModel.AcqHandle, cameraModel.GainParamName, out value);
                    }
                    result = true;
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                throw ex;
            }

            return result;
        }
        
        private bool SetDevieceTriggerMode(Camera2DSetModel cameraModel, int nSleepTimeMM, ref string errorMsg)
        {
            bool result = false;
            try
            {
                if (!cameraModel.IsOpen)
                {
                    result = false;
                    errorMsg = "Device is not open!";
                }
                else
                {
                    if (cameraModel.bLocalImage)
                    {
                        result = true;
                    }
                    else
                    { 
                        if(!cameraModel.IsExternTrig)
                        {
                            HOperatorSet.SetFramegrabberParam(cameraModel.AcqHandle, "TriggerMode", "Off");
                            Thread.Sleep(nSleepTimeMM);
                        }

                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
            return result;
        }

        private bool RefreshExposureAndGain(Camera2DSetModel cameraModel, ref string errorMsg)
        {
            bool result = false;
            try
            {
                if (!cameraModel.IsOpen)
                {
                    result = false;
                    errorMsg = "Device is not open!";
                }
                else
                {
                    int dSleepTime = 0;
                    if (cameraModel.bLocalImage)
                    {
                        result = true;
                    }
                    else
                    { 
                        result = SetDevieceExposureTime(cameraModel, new HTuple(cameraModel.ExposureTime), dSleepTime, ref errorMsg);
                        if (result)
                        {
                            result = SetDevieceGainRaw(cameraModel, new HTuple(cameraModel.Gain), dSleepTime, ref errorMsg);
                        }

                        if(result)
                        {
                            result = SetDevieceTriggerMode(cameraModel, dSleepTime, ref errorMsg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                throw ex;
            }
            return result;
        }
        
        #endregion
    }
}
