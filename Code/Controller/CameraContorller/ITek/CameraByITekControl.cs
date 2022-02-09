using BaseController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseModels;
using SequenceTestModel;
using IKapC.NET;
using HalconDotNet;
using AlgorithmController;
using System.Drawing;
using System.Threading;

namespace CameraContorller
{
    public class CameraByITekControl : BaseControl, ICameraControl
    {
        public event EventHandler<object> GetImageByContinue;
        public event EventHandler<object> GetImageBySoft;
        public event EventHandler<object> GetImageByTrigger;
        bool m_bInit = false;

        //行触发
        GrabOnce m_lineTrigger = new GrabOnce();

        //帧触发
        FrameTrigger m_frameTrigger = new FrameTrigger();

        public bool Init(object parameter)
        {
            m_lineTrigger.InitEnvironment();
            m_bInit = true;
            return true;
        }

        public BaseResultModel Run(Camera2DSetModel controlModel, CameraControlType controlType)
        {
            if (!m_bInit)
            {
                Init(controlModel);
            }
            CameraResultModel resultModel = new CameraResultModel();
            Camera2DSetModel cameraModel = controlModel as Camera2DSetModel;
            try
            {
                switch (controlType)
                {
                    case CameraControlType.CameraDiscover:
                        //检测相机 
                        List<string> cameraSerialNums = m_lineTrigger.DisCoverDevice();
                        if (cameraSerialNums != null && cameraSerialNums.Count() > 0)
                        {
                            resultModel.ObjectResult = cameraSerialNums;
                            resultModel.RunResult = true;
                        }
                        break;
                    case CameraControlType.CameraOpenByTirgger:
                        break;
                    case CameraControlType.CameraSetParam:
                        resultModel.RunResult = CameraSetParamter(cameraModel);
                        break;
                    case CameraControlType.CameraGetImageByTirgger:
                        resultModel.Image = CameaGetImageTrig();
                        resultModel.ObjectResult = resultModel.Image;
                        if (resultModel.Image != null)
                        {
                            resultModel.RunResult = true;
                        }
                        else
                        {
                            resultModel.ErrorMessage = "未采集到图像";
                        }
                        break;
                    case CameraControlType.CameraClose:
                        resultModel.RunResult = CameraClose(cameraModel);
                        break;

                    //普通拍照
                    case CameraControlType.CameraOpenBySoft:
                    case CameraControlType.CameraOpenContinue:
                        resultModel.RunResult = GetHandleInstance(cameraModel);
                        cameraModel.IsOpen = resultModel.RunResult;
                        break;
                    case CameraControlType.CameraStartSnapBySoft:
                        resultModel.RunResult = CameraSnap(cameraModel);
                        //resultModel.Image = CameraGetImage(cameraModel); 
                        resultModel.ObjectResult = resultModel.Image;
                        if (resultModel.Image != null)
                        {
                            resultModel.RunResult = true;
                        }
                        string errormsg = "";
                        HObject ho_image = AlgorithmCommHelper.ConvertHImageFromBitmap(resultModel.Image as Bitmap, out errormsg);
                        resultModel.Image = ho_image;
                        break;
                    case CameraControlType.CameraGetImageBySoft:
                        //resultModel.Image = CameraGetImage(cameraModel);
                        resultModel.ObjectResult = resultModel.Image;
                        if (resultModel.Image != null)
                        {
                            resultModel.RunResult = true;
                        }
                        break;
                    case CameraControlType.CameraStopGrab:
                       // resultModel.RunResult = CameraStopGrab(cameraModel);
                        break;

                    case CameraControlType.CameraStartContinue:
                        // resultModel.RunResult = CameraSnapContinue(cameraModel);
                        string errorMessage = "";
                        //CameraOpenByContinue(cameraModel, ref errorMessage);
                        resultModel.RunResult = true;
                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            resultModel.RunResult = false;
                            resultModel.ErrorMessage = "启动连续采集失败;" + errorMessage;
                        }

                        break;
                    case CameraControlType.CameraGetImageByContinue:
                       // resultModel.RunResult = ShowImage(m_handle);
                        break;

                    case CameraControlType.CameraSetTriggerMode:
                       // resultModel.RunResult = CameraSetTriggerMode(cameraModel);
                        break;

                    default:
                        throw new InvalidOperationException("无效的操作类型:" + controlType.ToString() + "，不适用于相机操作");
                }
            }
            catch (Exception ex)
            {

            }
            return resultModel;
        }

 

        private bool CameraClose(Camera2DSetModel cameraModel)
        {
            if(cameraModel.IsOpen)
            {
                // Stop capturing image
                IKapCLib.ItkStreamStop(m_lineTrigger.m_hStream);

                // UnRegister callback
                m_lineTrigger.UnRegisterCallback();

                // Close device
                m_lineTrigger.CloseDevice();

                // Release environment
                m_lineTrigger.ReleaseEnvironment();

            }
            cameraModel.IsOpen = false;
            Camera2DSetModel cameraparamModel = m_CameraModelIsOpenList.Find(x => x.DeviceName == cameraModel.DeviceName);
            if (cameraparamModel != null)
            {
                m_CameraModelIsOpenList.Remove(cameraparamModel);
            }

            m_bInit = false;

            return true;
        }
        
        private static List<Camera2DSetModel> m_CameraModelIsOpenList = new List<Camera2DSetModel>();
        private bool GetHandleInstance(Camera2DSetModel cameraModel)
        {
            try
            {
                if (cameraModel == null)
                {
                    throw new ArgumentNullException("输入参数Camera2DSetModel为空");
                }
                if (m_CameraModelIsOpenList == null)
                {
                    m_CameraModelIsOpenList = new List<Camera2DSetModel>();
                }
                var cameraModelTemp = m_CameraModelIsOpenList.FirstOrDefault(x => x.DeviceName == cameraModel.DeviceName);
                if (cameraModelTemp == null)
                {
                    if (CameraOpen(cameraModel))
                    {
                        m_CameraModelIsOpenList.Add(cameraModel);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    cameraModel.IsOpen = cameraModelTemp.IsOpen;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            } 
        }

        private bool CameraOpen(Camera2DSetModel cameraModel)
        {
            //  This function can be used to configure camera, form more examples please see IKapC usage
            if (!m_lineTrigger.ConfigureCamera(cameraModel.Index))
            {
                return false;
            }

            // Create stream and add buffer
            if(!m_lineTrigger.CreateStreamAndBuffer())
            {
                return false;
            }

            // Configure stream
            if(!m_lineTrigger.ConfigureStream())
            {
                return false;
            }

            cameraModel.IsOpen = true;
            return true;
        }

        private bool CameraSetParamter(Camera2DSetModel cameraModel)
        {
            if(cameraModel.IsOpen)
            {
                return true;
            }
            return m_lineTrigger.SetLineTrigger();
        }

        private bool CameraSnap(Camera2DSetModel cameraModel)
        {
            m_lineTrigger.m_QImage.Clear();
            // Stop capturing image
            //IKapCLib.ItkStreamStop(m_lineTrigger.m_hStream);
            //// Start capturing image
            //uint res = IKapCLib.ItkStreamStart(m_lineTrigger.m_hStream, (uint)IKapCLib.ITKSTREAM_CONTINUOUS); 
            //if (!m_lineTrigger.CheckIKapC(res))
            //{
            //    return false;
            //}

            // Start capturing image
            uint res = IKapCLib.ItkStreamStart(m_lineTrigger.m_hStream, 1);
            if (!m_lineTrigger.CheckIKapC(res))
            {
                return false;
            }

            // wait
            res = IKapCLib.ItkStreamWait(m_lineTrigger.m_hStream);
            if (!m_lineTrigger.CheckIKapC(res))
            {
                return false;
            }

            // Stop capturing image
            res = IKapCLib.ItkStreamStop(m_lineTrigger.m_hStream);
            if (!m_lineTrigger.CheckIKapC(res))
            {
                return false;
            }

            return true;
        }

        private HObject CameaGetImageTrig()
        {
            try
            {
                if (m_lineTrigger.m_QImage.Count > 0)
                { 
                    // Stop capturing image
                   // IKapCLib.ItkStreamStop(m_lineTrigger.m_hStream); 

                    return m_lineTrigger.m_QImage.Dequeue();
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
    }

}
