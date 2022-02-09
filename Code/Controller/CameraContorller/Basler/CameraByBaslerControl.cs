using BaseController;
using Basler.Pylon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseModels; 
using Infrastructure.Log;
using System.Drawing;
using System.Drawing.Imaging;
using Infrastructure.DBCore;
using CameraContorller;
using GlobalCore;
using System.Diagnostics;
using System.Threading;
using HalconDotNet;
using SequenceTestModel;

namespace CameraContorller
{
    public class CameraByBaslerControl : BaseControl, ICameraControl
    {
        private static object lockObj = new object();
        private static List<Camera> m_Cameras;
        private List<ICameraInfo> m_AllCameras;
        private PixelDataConverter converter = new PixelDataConverter();
        //private List<BitmapData> m_BitmapDataList = new List<BitmapData>();
        private List<Bitmap> m_BitmapList = new List<Bitmap>();
        private static List<Bitmap> m_BitmapListUp = new List<Bitmap>();
        private Camera iCameraSoft;
        private Camera iCameraHardTirgger;
        private static Camera iCameraContinue;
        private Stopwatch stopWatch = new Stopwatch();

        public event EventHandler<object> GetImageByTrigger;
        public event EventHandler<object> GetImageBySoft;
        public event EventHandler<object> GetImageByContinue;
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

        public bool Init(object parameter)
        {
            try
            {
                if (m_IsInit)
                {
                    return true;
                }
                //检测相机
                m_AllCameras = CameraFinder.Enumerate();
                if (m_Cameras == null || m_Cameras.Count == 0)
                {
                    m_Cameras = new List<Camera>();
                    foreach (var item in m_AllCameras)
                    {
                        Camera camera = new Camera(item);
                        m_Cameras.Add(camera);
                    }
                }
                m_IsInit = true;
                return m_IsInit;
            }
            catch (Exception ex)
            {
                m_IsInit = false;
                throw new Exception("相机初始化失败!" + ex.Message);
            }
        }

        public BaseResultModel Run(Camera2DSetModel controlModel, CameraControlType controlType)
        {
            if (!m_IsInit)
            {
                Init(controlModel);
            }
            CameraResultModel resultModel;
            Camera2DSetModel cameraModel = controlModel as Camera2DSetModel;
            Camera camera;
            FindCamera(controlModel, controlType, cameraModel, out resultModel, out camera);
            lock (lockObj)
            {
                try
                {
                    switch (controlType)
                    {
                        case CameraControlType.CameraDiscover:
                            //检测相机
                            m_AllCameras = CameraFinder.Enumerate();
                            List<string> cameraSerialNums = new List<string>();
                            foreach (var item in m_AllCameras)
                            {
                                cameraSerialNums.Add(item["SerialNumber"]);
                            }
                            resultModel.ObjectResult = cameraSerialNums;
                            resultModel.RunResult = true;
                            break;
                        case CameraControlType.CameraOpenByTirgger:
                            iCameraHardTirgger = CameraOpenTirgger(camera);
                            cameraModel.IsOpen = iCameraHardTirgger.IsOpen;
                            break;
                        case CameraControlType.CameraSetParam:
                            if (camera != null)
                            {
                                if (camera.IsOpen)
                                {
                                    CameraSetParam(camera, cameraModel);
                                }
                                else
                                {
                                    resultModel.RunResult = false;
                                    resultModel.ErrorMessage = "相机未打开";
                                }
                            }

                            break;
                        case CameraControlType.CameraGetImageByTirgger:
                            resultModel.Image = CameraSnap();
                            break;
                        case CameraControlType.CameraClose:
                            if (iCameraContinue != null && camera.CameraInfo[CameraInfoKey.SerialNumber] == iCameraContinue.CameraInfo[CameraInfoKey.SerialNumber])
                                iCameraContinue.StreamGrabber.ImageGrabbed -= OnImageGrabbedByContinue;
                            if (iCameraHardTirgger != null && camera.CameraInfo[CameraInfoKey.SerialNumber] == iCameraHardTirgger.CameraInfo[CameraInfoKey.SerialNumber])
                                iCameraHardTirgger.StreamGrabber.ImageGrabbed -= OnImageGrabbed;
                            if (iCameraSoft != null && camera.CameraInfo[CameraInfoKey.SerialNumber] == iCameraSoft.CameraInfo[CameraInfoKey.SerialNumber])
                                iCameraSoft.StreamGrabber.ImageGrabbed -= OnImageGrabbedBySoft;

                            camera.StreamGrabber.ImageGrabbed -= OnImageGrabbedBySoft;//Add 0718 预防内存泄漏
                            Camera2DSetModel cameraparamModel = m_CameraModelIsOpenList.Find(x => x.DeviceName == cameraModel.DeviceName);
                            if (cameraparamModel != null)
                            {
                                m_CameraModelIsOpenList.Remove(cameraparamModel);
                            }

                            camera.StreamGrabber.Stop();
                            camera.Close();
                            m_BitmapList.Clear();
                            m_BitmapListUp.Clear();
                            cameraModel.IsOpen = false;
                            stopWatch.Reset();
                            break;

                        //普通拍照
                        case CameraControlType.CameraOpenBySoft:
                            camera = CameraOpenSoftSnap(camera);
                            cameraModel.IsOpen = camera.IsOpen;
                            resultModel.RunResult = cameraModel.IsOpen;
                            //if (GetHandleInstance(cameraModel, camera) != null)
                            //{
                            //    resultModel.RunResult = false;
                            //    resultModel.ErrorMessage = "打开相机失败;";
                            //}
                            //else
                            //{
                            //    resultModel.RunResult = true;
                            //}
                            break;
                        case CameraControlType.CameraStartSnapBySoft:
                            if (camera.IsOpen)
                            {
                                CameraStartSnap(camera);
                                string error = "";
                                resultModel.Image = ConvertHImageFromBitmap(CameraGetImage(), out error);
                                resultModel.ObjectResult = resultModel.Image;
                                if (resultModel.Image == null)
                                    resultModel.RunResult = false;
                            }
                            break;
                        case CameraControlType.CameraGetImageBySoft:
                            string errormsg = "";
                            resultModel.Image = ConvertHImageFromBitmap(CameraGetImage(), out errormsg);
                            resultModel.ObjectResult = resultModel.Image;
                            break;
                        case CameraControlType.CameraOpenContinue:
                            iCameraContinue = CameraOpenByContinue(camera);
                            cameraModel.IsOpen = iCameraContinue.IsOpen;
                            break;
                        case CameraControlType.CameraStartContinue:
                            if (iCameraContinue.IsOpen)
                            {
                                CameraStartContinue(cameraModel);
                            }
                            break;
                        case CameraControlType.CameraGetImageByContinue:
                            resultModel.Image = ConvertHImageFromBitmap(CameraGetImageByContinue(), out errormsg);
                            resultModel.ObjectResult = resultModel.Image;
                            break;

                        case CameraControlType.CameraStopGrab:
                            iCameraContinue.StreamGrabber.Stop();
                            iCameraContinue.StreamGrabber.ImageGrabbed -= OnImageGrabbedByContinue;
                            break;
                        default:
                            throw new InvalidOperationException("无效的操作类型:" + controlType.ToString() + "，不适用于相机操作");
                    }
                }
                catch (Exception ex)
                {
                    resultModel.RunResult = false;
                    resultModel.ErrorMessage = ex.Message;
                    if (camera != null)
                    {
                        if (camera.IsOpen)
                        {
                            camera.Close();
                        }
                    }
                    //throw ex;
                }
                return resultModel;
            }
        }

        private void FindCamera(Camera2DSetModel controlModel, CameraControlType controlType, Camera2DSetModel cameraModel, out CameraResultModel resultModel, out Camera camera)
        {
            resultModel = new CameraResultModel();
            cameraModel = controlModel as Camera2DSetModel;
            camera = null;
            if (controlType != CameraControlType.CameraDiscover)
            {
                if (cameraModel == null)
                {
                    throw new ArgumentException("CameraModel参数为空！或类型不匹配！");
                }
                if (!m_IsInit)
                {
                    throw new ArgumentException("没有初始化！");
                }
                if (m_AllCameras == null || m_AllCameras.Count() == 0)
                {
                    //throw new ArgumentException("没有检测到相机！");
                }
                resultModel.RunResult = true;
                camera = m_Cameras.FirstOrDefault(x => x.CameraInfo[CameraInfoKey.SerialNumber] == cameraModel.UniqueName);
            }
        }

        private static List<Camera2DSetModel> m_CameraModelIsOpenList = new List<Camera2DSetModel>();
        private HTuple GetHandleInstance(Camera2DSetModel cameraModel, Camera camera)
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
                    camera = CameraOpenSoftSnap(camera);
                    cameraModel.IsOpen = camera.IsOpen;
                    m_CameraModelIsOpenList.Add(cameraModel);
                }
                else
                {
                    cameraModel.IsOpen = cameraModelTemp.IsOpen;
                    
                }

                return cameraModel.AcqHandle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Camera CameraOpenTirgger(Camera camera)
        {
            try
            {
                //Camera iCamera;
                if (!camera.IsOpen)
                {
                    //默认设置为外触发连续触发
                    camera.CameraOpened += Configuration.AcquireContinuous;

                    iCameraHardTirgger = camera.Open() as Camera;
                    if (iCameraHardTirgger.Parameters.Contains(PLCamera.SensorReadoutMode))
                        iCameraHardTirgger.Parameters[PLCamera.SensorReadoutMode].SetValue(PLCamera.SensorReadoutMode.Fast);

                    //设置为上升沿外触发
                    iCameraHardTirgger.Parameters[PLCamera.TriggerMode].SetValue(PLCamera.TriggerMode.On);
                    iCameraHardTirgger.Parameters[PLUsbCamera.TriggerActivation].SetValue(PLUsbCamera.TriggerActivation.RisingEdge);
                    //设置拍照事件
                    iCameraHardTirgger.StreamGrabber.ImageGrabbed += OnImageGrabbed;
                    iCameraHardTirgger.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
                }
                else
                {
                    iCameraHardTirgger.StreamGrabber.Stop();
                    iCameraHardTirgger.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
                    iCameraHardTirgger = camera;
                }

                return iCameraHardTirgger;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        private void CameraSetParam(Camera camera, Camera2DSetModel cameraModel)
        {
            try
            {
                if (camera.Parameters.Contains(PLCamera.GainAbs))
                {
                    camera.Parameters[PLCamera.GainAbs].SetValue(cameraModel.Gain);
                }
                else if (camera.Parameters.Contains(PLCamera.GainRaw))
                {
                    camera.Parameters[PLCamera.GainRaw].SetValue(cameraModel.Gain);
                }
                else
                {
                    camera.Parameters[PLCamera.Gain].SetValue(cameraModel.Gain);
                }
                if (camera.Parameters.Contains(PLCamera.ExposureTimeAbs))
                {
                    camera.Parameters[PLCamera.ExposureTimeAbs].SetValue(cameraModel.ExposureTime);
                }
                else if (camera.Parameters.Contains(PLCamera.ExposureTimeRaw))
                {
                    camera.Parameters[PLCamera.ExposureTimeRaw].SetValue(cameraModel.ExposureTime);
                }
                else
                {
                    camera.Parameters[PLCamera.ExposureTime].SetValue(cameraModel.ExposureTime);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private Bitmap CameraSnap()
        {
            Bitmap bitmapData = null;
            if (m_BitmapList != null && m_BitmapList.Count() > 0)
            {
                bitmapData = m_BitmapList[0].Clone() as Bitmap;
                m_BitmapList[0].Dispose();
                m_BitmapList.RemoveAt(0);
            }
            //if (m_BitmapDataList.Count > 0)
            //{
            //    m_BitmapList[0].Dispose();
            //    m_BitmapList.RemoveAt(0);
            //}
            //else
            //{
            //    //m_BitmapList.Clear();
            //}
            return bitmapData;
        }
        public static int snapCount = 0;
        private void OnImageGrabbed(Object sender, ImageGrabbedEventArgs e)
        {
            snapCount++;
            IGrabResult grabResult = e.GrabResult;
            if (grabResult.GrabSucceeded)
            {
                byte[] buffer = grabResult.PixelData as byte[];

                if (grabResult.IsValid)
                {
                    Bitmap bitmap = new Bitmap(grabResult.Width, grabResult.Height, PixelFormat.Format32bppRgb);
                    // Lock the bits of the bitmap.
                    BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                    // Place the pointer to the buffer of the bitmap.
                    converter.OutputPixelFormat = PixelType.BGRA8packed;
                    IntPtr ptrBmp = bmpData.Scan0;
                    converter.Convert(ptrBmp, bmpData.Stride * bitmap.Height, grabResult); //Exception handling TODO
                    bitmap.UnlockBits(bmpData);
                    //m_BitmapList.Add(bitmap);
                    OnGetImageByTrigger(bitmap);
                    //
                    //m_imageBufList.Add(buffer);
                }
            }
        }

        private Camera CameraOpenSoftSnap(Camera camera)
        {
            try
            {
                Camera iCameraBySoft;
                if (!camera.IsOpen)
                {
                    iCameraBySoft = camera.Open() as Camera;

                    //if (iCameraBySoft.Parameters.Contains(PLCamera.SensorReadoutMode))
                    iCameraBySoft.Parameters[PLCamera.TriggerMode].SetValue(PLCamera.TriggerMode.Off);
                    //默认设置为单张拍照触发
                    iCameraBySoft.Parameters[PLCamera.AcquisitionMode].SetValue(PLCamera.AcquisitionMode.SingleFrame);
                    //设置拍照事件
                    iCameraBySoft.StreamGrabber.ImageGrabbed += OnImageGrabbedBySoft;
                }
                else
                {
                    iCameraBySoft = camera;
                }

               
                return iCameraBySoft;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        private void CameraStartSnap(Camera camera)
        {
            try
            {
                if (m_BitmapListUp != null)
                {
                    foreach (var item in m_BitmapListUp)
                    {
                        item.Dispose();
                    }
                }
                m_BitmapListUp.Clear();
                camera.StreamGrabber.Start(1, GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        private Bitmap CameraGetImage()
        {
            Bitmap bitmap = null;
            Stopwatch sp = new Stopwatch();
            sp.Start();
            while (/*!Global.Break &&*/ sp.ElapsedMilliseconds < 2000)
            {
                if (m_BitmapListUp != null && m_BitmapListUp.Count() > 0)
                {
                    bitmap = m_BitmapListUp[0].Clone() as Bitmap;
                    foreach (var item in m_BitmapListUp)
                    {
                        item.Dispose();
                    }
                    m_BitmapListUp.Clear();
                    break;
                }
                Thread.Sleep(10);
            }
            return bitmap;
        }
        private void OnImageGrabbedBySoft(Object sender, ImageGrabbedEventArgs e)
        {
            try
            {
                IGrabResult grabResult = e.GrabResult;
                if (grabResult.GrabSucceeded)
                {
                    byte[] buffer = grabResult.PixelData as byte[];

                    if (grabResult.IsValid)
                    {
                        Bitmap bitmap = new Bitmap(grabResult.Width, grabResult.Height, PixelFormat.Format32bppRgb);
                        // Lock the bits of the bitmap.
                        BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                        // Place the pointer to the buffer of the bitmap.
                        converter.OutputPixelFormat = PixelType.BGRA8packed;
                        IntPtr ptrBmp = bmpData.Scan0;
                        converter.Convert(ptrBmp, bmpData.Stride * bitmap.Height, grabResult); //Exception handling TODO
                        bitmap.UnlockBits(bmpData);
                        m_BitmapListUp.Add(bitmap);
                       // bitmap.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                 
            }
           
        }

        private Camera CameraOpenByContinue(Camera camera)
        {
            try
            {
                Camera iCameraBySoft;
                if (!camera.IsOpen)
                {
                    //默认设置为连续拍照触发
                    camera.CameraOpened += Configuration.AcquireContinuous;
                    iCameraBySoft = camera.Open() as Camera;
                }
                else
                {
                    iCameraBySoft = camera;
                }

                if (iCameraBySoft.Parameters.Contains(PLCamera.SensorReadoutMode))
                    iCameraBySoft.Parameters[PLCamera.TriggerMode].SetValue(PLCamera.TriggerMode.Off);
                //设置拍照事件
                iCameraBySoft.StreamGrabber.ImageGrabbed += OnImageGrabbedByContinue;
                return iCameraBySoft;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        private void CameraStartContinue(Camera2DSetModel cameraModel)
        {
            try
            {
                iCameraContinue.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        private Bitmap CameraGetImageByContinue()
        {
            Bitmap bitmap = null;
            Stopwatch sp = new Stopwatch();
            sp.Start();
            while (!Global.Break && sp.ElapsedMilliseconds < 2000)
            {
                if (m_BitmapListUp != null && m_BitmapListUp.Count() > 0)
                {
                    bitmap = m_BitmapListUp[0].Clone() as Bitmap;
                    m_BitmapListUp[0].Dispose();
                    m_BitmapListUp.RemoveAt(0);
                    break;
                }
                Thread.Sleep(10);
            }
            return bitmap;
        }

        Bitmap buffBitmap = null;
        object locker = new object();
        private void OnImageGrabbedByContinue(Object sender, ImageGrabbedEventArgs e)
        {
            lock(locker)
            {
                try
                {
                    IGrabResult grabResult = e.GrabResult;
                    if (!stopWatch.IsRunning || stopWatch.ElapsedMilliseconds > 33)
                    {
                        stopWatch.Restart();
                        if (grabResult.GrabSucceeded)
                        {
                            byte[] buffer = grabResult.PixelData as byte[];

                            if (grabResult.IsValid)
                            {
                                //Bitmap bitmap = new Bitmap(grabResult.Width, grabResult.Height, PixelFormat.Format32bppRgb);

                                Bitmap bitmap = new Bitmap(grabResult.Width, grabResult.Height, PixelFormat.Format32bppArgb);
                                // Lock the bits of the bitmap.
                                BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                                // Place the pointer to the buffer of the bitmap.
                                converter.OutputPixelFormat = PixelType.BGRA8packed;
                                IntPtr ptrBmp = bmpData.Scan0;
                                converter.Convert(ptrBmp, bmpData.Stride * bitmap.Height, grabResult); //Exception handling TODO
                                bitmap.UnlockBits(bmpData);

                                Bitmap bitmapOld = buffBitmap;
                                // Provide the display control with the new bitmap. This action automatically updates the display.
                                buffBitmap = bitmap;
                                if (bitmapOld != null)
                                {
                                    // Dispose the bitmap.
                                    bitmapOld.Dispose();
                                }
                                string error = "";
                                var ho_Image = ConvertHImageFromBitmap(bitmap, out error);
                                List<object> list = new List<object>() { 0, ho_Image };
                                OnGetImageByContinue(list); 
                                e.DisposeGrabResultIfClone();
                                Thread.Sleep(30);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }            
        }

        public HObject ConvertHImageFromBitmap(Bitmap bitmap, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadWrite, bitmap.PixelFormat);
                bitmap.UnlockBits(bitmapData);
                HObject ho_Image;
                HOperatorSet.GenEmptyObj(out ho_Image);
                HOperatorSet.GenImageInterleaved(out ho_Image, bitmapData.Scan0, "bgrx", bitmapData.Width, bitmapData.Height, -1, "byte",
                    bitmapData.Width, bitmapData.Height, 0, 0, -1, 0);
                bitmap.Dispose();
                return ho_Image;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return null;
            }
        }
    }
}
