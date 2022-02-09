using BaseController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseModels;
using SequenceTestModel;
using HalconDotNet;
using AlgorithmController;
using System.Drawing;
using System.Diagnostics;
using System.Threading;
using System.Reflection;
using System.IO;

namespace CameraContorller
{
    public class CameraByIDSControl : BaseControl, ICameraControl
    {
        // our camera class
        static uEye.Camera m_Camera;
        static bool m_bInit = false;
        Int32 m_s32FrameCount; 
        private const int m_cnNumberOfSeqBuffers = 3; 
        private static List<Bitmap> m_BitmapList = new List<Bitmap>(); 

        public event EventHandler<object> GetImageByContinue;
        public event EventHandler<object> GetImageBySoft;
        public event EventHandler<object> GetImageByTrigger;

        protected void OnGetImageByContinue(object e)
        {
            EventHandler<object> handler = GetImageByContinue;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public bool Init(object parameter)
        {
            m_Camera = new uEye.Camera();
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
                        List<string> cameraSerialNums = DisCoverDevice();
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
                        //resultModel.Image = CameaGetImageTrig(); 
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
                        if (CameraSnap(cameraModel))
                        {
                            resultModel.Image = CameraGetImage();
                            if (resultModel.Image != null)
                            {
                                resultModel.RunResult = true;

                                string errormsg = "";
                                HObject ho_image = AlgorithmCommHelper.ConvertHImageFromBitmap2(resultModel.Image as Bitmap, out errormsg);
                                resultModel.Image = ho_image;
                                resultModel.ObjectResult = resultModel.Image;
                            }
                        } 
                       
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
                        resultModel.RunResult = CameraSnapContinue(cameraModel);
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

        private bool HasValueChangedEventHandler(uEye.Camera b)
        {
            FieldInfo f1 = typeof(uEye.Camera).GetField("onFrameEvent", BindingFlags.Instance | BindingFlags.NonPublic);
            var handler = (EventHandler)f1.GetValue(b);
            return handler != null;
        }

        public List<string> DisCoverDevice()
        {
            List<string> cameraSerialNums = new List<string>();

            uEye.Types.CameraInformation[] cameraList;
            uEye.Info.Camera.GetCameraList(out cameraList);

            foreach (uEye.Types.CameraInformation info in cameraList)
            {
                //ListViewItem item = new ListViewItem();
                //item.Text = info.InUse ? "No" : "Yes";
                //item.ImageIndex = info.InUse ? 1 : 0;

                //item.SubItems.Add(info.CameraID.ToString());
                //item.SubItems.Add(info.DeviceID.ToString());
                //item.SubItems.Add(info.Model);
                //item.SubItems.Add(info.SerialNumber);

                //listViewCamera.Items.Add(item); 

                cameraSerialNums.Add(info.DeviceID.ToString());
            }

            return cameraSerialNums;
        }

        private bool CameraClose(Camera2DSetModel cameraModel)
        {
            try
            {
                m_Camera.EventFrame -= onFrameEvent;
                m_Camera.Exit();

                cameraModel.IsOpen = false;
                Camera2DSetModel cameraparamModel = m_CameraModelIsOpenList.Find(x => x.DeviceName == cameraModel.DeviceName);
                if (cameraparamModel != null)
                {
                    m_CameraModelIsOpenList.Remove(cameraparamModel);
                }
            }
            catch (Exception ex)
            {
                 
            }
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
            uEye.Defines.Status statusRet;
            statusRet = initCamera(cameraModel);

            if (statusRet == uEye.Defines.Status.SUCCESS)
            {
                // start capture
                //statusRet = m_Camera.Acquisition.Freeze();
                //if (statusRet != uEye.Defines.Status.SUCCESS)
                //{
                //    MessageBox.Show("Starting live video failed");
                //}
                //else
                //{
                //    // everything is ok
                //    m_IsLive = false;
                //    UpdateToolbar();
                //}

                //uEye.Defines.ColorMode colorMode = uEye.Defines.ColorMode.Mono8;
                //uEye.Defines.ColorConvertMode convertMode = uEye.Defines.ColorConvertMode.Software5X5;

                //statusRet = m_Camera.PixelFormat.Set(colorMode);
                //statusRet = m_Camera.Color.Converter.Set(colorMode, convertMode); 
            }

            // cleanup on any camera error
            if (statusRet != uEye.Defines.Status.SUCCESS && m_Camera.IsOpened)
            {
                m_Camera.Exit();
            }

            return statusRet == uEye.Defines.Status.SUCCESS;
        }

        private bool CameraSetParamter(Camera2DSetModel cameraModel)
        {
            uEye.Defines.Status statusRet;
            uEye.Types.Range<Double> range;

            statusRet = m_Camera.Timing.Exposure.GetRange(out range);


            if (cameraModel.ExposureTime > range.Maximum)
            {
                cameraModel.ExposureTime = (int)range.Maximum;
            }
             
            statusRet = m_Camera.Timing.Exposure.Set(cameraModel.ExposureTime);
            Thread.Sleep(150);

            //设置对焦AOI区域
            //m_Camera.Size.AOI.Set(0, 0, 100, 200);

            return statusRet == uEye.Defines.Status.SUCCESS;
        }

        bool m_bLoad = false;
        bool m_bfirst = true;
        private bool CameraSnap(Camera2DSetModel cameraModel)
        {
            uEye.Defines.Status statusRet;

            ///加载不同的配置文件
            //if (cameraModel.IsLoadConfig != m_bLoad || m_bfirst)
            if (false)
            {
                m_Camera.Acquisition.Stop();
                MemoryHelper.ClearSequence(m_Camera);
                MemoryHelper.FreeImageMems(m_Camera);
                //string strPath = cameraModel.IsLoadConfig ? "C://2.ini" : "C://1.ini";
                //if (File.Exists(strPath))
                //{
                //    m_Camera.Acquisition.Stop();
                //    m_Camera.Parameter.Load(strPath);
                //}
                uEye.Defines.ColorMode colorMode;
                m_Camera.PixelFormat.Get(out colorMode);
                // allocate new standard memory
                MemoryHelper.AllocImageMems(m_Camera, m_cnNumberOfSeqBuffers);
                MemoryHelper.InitSequence(m_Camera);
                //m_bLoad = cameraModel.IsLoadConfig;
                m_bfirst = false;
            }
            ///

            // start capture
            statusRet = m_Camera.Acquisition.Freeze();

            return statusRet == uEye.Defines.Status.SUCCESS;
        }

        private uEye.Defines.Status initCamera(Camera2DSetModel cameraModel)
        {
            uEye.Defines.Status statusRet = uEye.Defines.Status.NO_SUCCESS;

            statusRet = m_Camera.Init(Int32.Parse(cameraModel.DeviceName) | (Int32)uEye.Defines.DeviceEnumeration.UseDeviceID, 0);
            if (statusRet != uEye.Defines.Status.SUCCESS)
            {
                return statusRet;
            }

            //加载相机内参数
            MemoryHelper.ClearSequence(m_Camera);
            MemoryHelper.FreeImageMems(m_Camera); 
            m_Camera.Parameter.Load();
            //m_Camera.Parameter.Load("");

            statusRet = MemoryHelper.AllocImageMems(m_Camera, m_cnNumberOfSeqBuffers);
            if (statusRet != uEye.Defines.Status.SUCCESS)
            {
                return statusRet;
            }

            statusRet = MemoryHelper.InitSequence(m_Camera);
            if (statusRet != uEye.Defines.Status.SUCCESS)
            {
                return statusRet;
            }

            // set event
            m_Camera.EventFrame += onFrameEvent;

            // reset framecount
            m_s32FrameCount = 0; 

            uEye.Types.SensorInfo sensorInfo;
            m_Camera.Information.GetSensorInfo(out sensorInfo);

            return statusRet;

        }

        private void onFrameEvent(object sender, EventArgs e)
        {
            // convert sender object to our camera object
            uEye.Camera camera = sender as uEye.Camera;

            if (camera.IsOpened)
            {
                uEye.Defines.DisplayMode mode;
                camera.Display.Mode.Get(out mode);

                // only display in dib mode
                if (mode == uEye.Defines.DisplayMode.DiB)
                {
                    Int32 s32MemID;
                    uEye.Defines.Status statusRet = camera.Memory.GetLast(out s32MemID);

                    if ((uEye.Defines.Status.SUCCESS == statusRet) && (0 < s32MemID))
                    {
                        if (uEye.Defines.Status.SUCCESS == camera.Memory.Lock(s32MemID))
                        { 
                            Bitmap bitmap;
                            m_Camera.Memory.ToBitmap(s32MemID, out bitmap);
                            if(m_BitmapList.Count > 0)
                            {
                                m_BitmapList.Clear();
                            }
                            m_BitmapList.Add(bitmap);
                            
                            camera.Memory.Unlock(s32MemID);
                        }
                    }
                }

                ++m_s32FrameCount;
            }
        }

        private Bitmap CameraGetImage()
        {
            Bitmap bitmap = null;
            Stopwatch sp = new Stopwatch();
            sp.Start();
            while (sp.ElapsedMilliseconds < 2000)
            {
                if (m_BitmapList != null && m_BitmapList.Count() > 0)
                {
                    bitmap = m_BitmapList[0].Clone() as Bitmap;
                    foreach (var item in m_BitmapList)
                    {
                        item.Dispose();
                    }
                    m_BitmapList.Clear();
                    break;
                }
                Thread.Sleep(20);
            }
            return bitmap;
        }
        
        private bool CameraSnapContinue(Camera2DSetModel cameraModel)
        {
            if (!cameraModel.IsOpen)
            { 
                return false;
            }
            if (cameraModel.IsContinue)
            { 
                return false;
            }
            cameraModel.IsContinue = true;
            Thread t = new Thread(new ThreadStart(() =>
            {
                bool bresult = CameraSetParamter(cameraModel);
                while (cameraModel.IsContinue)
                {
                    string error = "";
                    HObject hImage = null;
                    if (!cameraModel.IsExternTrig)
                    {
                        if (!CameraSnap(cameraModel))
                        {
                            break;
                        }
                        Bitmap bitmap = CameraGetImage();
                        if (bitmap != null)
                        { 
                            string errormsg = "";
                            hImage = AlgorithmCommHelper.ConvertHImageFromBitmap2(bitmap, out errormsg);

                            if (hImage != null && hImage.IsInitialized() && hImage.Key != (IntPtr)0)
                            {
                                List<object> list = new List<object>() { cameraModel.Id, hImage };
                                OnGetImageByContinue(list);
                            }
                        }
                    }

                    Thread.Sleep(50);
                }
            }));
            t.Start();
            return true;
        }

    }
}
