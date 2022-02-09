using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseModels; 
using Infrastructure.DBCore;
using BaseController;
using MvCamCtrl.NET;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using HalconDotNet;
using AlgorithmController;
using SequenceTestModel;

namespace CameraContorller
{
    public class CameraByMVSControl : BaseControl, ICameraControl
    {
        public event EventHandler<object> GetImageByContinue;
        public event EventHandler<object> GetImageBySoft;
        public event EventHandler<object> GetImageByTrigger;

        private static object lockObj = new object();

        MyCamera.MV_CC_DEVICE_INFO_LIST m_pDeviceList;
        static CameraOperator[] m_pOperator  = new CameraOperator[9];
        static bool m_bInit = false;

        //500万相机
        //UInt32 m_nBufSizeForDriver = 2592 * 1944 * 3;
        //byte[] m_pBufForDriver = new byte[2592 * 1944 * 3];            // 用于从驱动获取图像的缓存 

        //2000万相机
        static UInt32 m_nBufSizeForDriver;
        static byte[] m_pBufForDriver;            // 用于从驱动获取图像的缓存 

        //UInt32 m_nBufSizeForSaveImage = 4024 * 3036 * 3 * 3 + 3036;
        //byte[] m_pBufForSaveImage = new byte[4024 * 3036 * 3 * 3 + 3036];         // 用于保存图像的缓存

        public delegate void GetContinueImage1(HObject e);
        public GetContinueImage1 m_GetContinueImage1;

        public static IntPtr m_handle;

        Queue<HObject> m_QImage = new Queue<HObject>();

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
            m_pDeviceList = new MyCamera.MV_CC_DEVICE_INFO_LIST();
            for(int i = 0; i< 9;i++)
            { 
                m_pOperator[i] = new CameraOperator();
            }

            m_nBufSizeForDriver = 5472 * 3648 * 3;
            m_pBufForDriver = new byte[5472 * 3648 * 3];

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
            string errorMsg = "";
            lock (lockObj)
            {
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
                            resultModel.RunResult = CameraSetParamter(cameraModel, ref errorMsg);
                            resultModel.ErrorMessage = errorMsg;
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
                            resultModel.RunResult = GetHandleInstance(cameraModel, ref errorMsg);
                            resultModel.ErrorMessage = errorMsg;
                            cameraModel.IsOpen = resultModel.RunResult;
                            break;
                        case CameraControlType.CameraStartSnapBySoft:
                            if(!cameraModel.IsExternTrig)
                            {
                                resultModel.RunResult = CameraSnap(cameraModel);
                                //resultModel.Image = CameraGetImage(cameraModel); 
                                //resultModel.ObjectResult = resultModel.Image;
                                //if (resultModel.Image != null)
                                //{
                                //    resultModel.RunResult = true;
                                //}
                                //string errormsg = "";
                                //HObject ho_image = AlgorithmCommHelper.ConvertHImageFromBitmap(resultModel.Image as Bitmap, out errormsg);
                                //resultModel.Image = ho_image;
                                resultModel.Image =  CameraGetImage2(cameraModel);
                                resultModel.ObjectResult = resultModel.Image;
                                if (resultModel.Image != null)
                                {
                                    resultModel.RunResult = true;
                                }
                            }
                            else
                            {
                                resultModel.RunResult = true;
                            }
                           
                            break;
                        case CameraControlType.CameraGetImageBySoft:
                            resultModel.Image = CameraGetImage2(cameraModel);
                            resultModel.ObjectResult = resultModel.Image;
                            if(resultModel.Image != null)
                            {
                                resultModel.RunResult = true;
                            } 
                            break;
                        case CameraControlType.CameraStopGrab:
                            resultModel.RunResult = CameraStopGrab(cameraModel);
                            break;
                             
                        case CameraControlType.CameraStartContinue:
                            // resultModel.RunResult = CameraSnapContinue(cameraModel);
                            string errorMessage = "";
                            CameraOpenByContinue(cameraModel, ref errorMessage);
                            resultModel.RunResult = true;
                            if (!string.IsNullOrEmpty(errorMessage))
                            {
                                resultModel.RunResult = false;
                                resultModel.ErrorMessage = "启动连续采集失败;" + errorMessage;
                            }
                            
                            break;
                        case CameraControlType.CameraGetImageByContinue:
                            resultModel.RunResult = ShowImage(m_handle);
                            break;

                        case CameraControlType.CameraSetTriggerMode:
                            resultModel.RunResult = CameraSetTriggerMode(cameraModel);
                            break;

                        default:
                            throw new InvalidOperationException("无效的操作类型:" + controlType.ToString() + "，不适用于相机操作");
                    }
                }
                catch (Exception ex)
                {
                    resultModel.RunResult = false;
                    resultModel.ErrorMessage = ex.Message;
                    CameraClose(cameraModel);
                    //throw ex;
                }
                return resultModel;
            }
        }

        #region 相机操作
        private static List<Camera2DSetModel> m_CameraModelIsOpenList = new List<Camera2DSetModel>();
        private bool GetHandleInstance(Camera2DSetModel cameraModel, ref string errorMsg)
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
                    string errorMessage = "";
                    if (CameraOpen(cameraModel, ref errorMessage))
                    {
                        m_CameraModelIsOpenList.Add(cameraModel);
                    }
                    else
                    {
                        errorMsg = errorMessage;
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
                errorMsg = ex.Message;
                return false;
            }

        }
        public List<string> DisCoverDevice()
        {
            List<string> cameraSerialNums = new List<string>();

            int nRet;
            /*创建设备列表*/
            System.GC.Collect(); 
            nRet = CameraOperator.EnumDevices(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE, ref m_pDeviceList);
            if (0 != nRet)
            {
                //MessageBox.Show("枚举设备失败!");
                return null;
            }

            //在窗体列表中显示设备名
            for (int i = 0; i < m_pDeviceList.nDeviceNum; i++)
            {
                MyCamera.MV_CC_DEVICE_INFO device = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_pDeviceList.pDeviceInfo[i], typeof(MyCamera.MV_CC_DEVICE_INFO));
                if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                {
                    IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stGigEInfo, 0);
                    MyCamera.MV_GIGE_DEVICE_INFO gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_GIGE_DEVICE_INFO));
                    if (gigeInfo.chUserDefinedName != "")
                    {
                        //cbDeviceList.Items.Add("GigE: " + gigeInfo.chUserDefinedName + " (" + gigeInfo.chSerialNumber + ")");
                        cameraSerialNums.Add(gigeInfo.chSerialNumber);
                    }
                    else
                    {
                        //    cbDeviceList.Items.Add("GigE: " + gigeInfo.chManufacturerName + " " + gigeInfo.chModelName + " (" + gigeInfo.chSerialNumber + ")");
                        cameraSerialNums.Add(gigeInfo.chSerialNumber);
                    }
                }
                else if (device.nTLayerType == MyCamera.MV_USB_DEVICE)
                {
                    IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stUsb3VInfo, 0);
                    MyCamera.MV_USB3_DEVICE_INFO usbInfo = (MyCamera.MV_USB3_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_USB3_DEVICE_INFO));
                    if (usbInfo.chUserDefinedName != "")
                    {
                        cameraSerialNums.Add(usbInfo.chSerialNumber);
                        //cbDeviceList.Items.Add("USB: " + usbInfo.chUserDefinedName + " (" + usbInfo.chSerialNumber + ")");
                    }
                    else
                    {
                        cameraSerialNums.Add(usbInfo.chSerialNumber);
                        //cbDeviceList.Items.Add("USB: " + usbInfo.chManufacturerName + " " + usbInfo.chModelName + " (" + usbInfo.chSerialNumber + ")");
                    }
                }
            }

            //选择第一项
            if (m_pDeviceList.nDeviceNum != 0)
            {
                //cbDeviceList.SelectedIndex = 0;
            }


            return cameraSerialNums;
        }

        public bool CameraOpen(Camera2DSetModel cameraModel, ref string errorMsg)
        {
            if (!cameraModel.IsOpen)
            {
                int nRet = CameraOperator.EnumDevices(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE, ref m_pDeviceList);
                if (0 != nRet)
                {
                    errorMsg = "枚举设备失败!";
                    return false;
                }
                if (m_pDeviceList.nDeviceNum == 0)
                {
                    errorMsg = "枚举设备个数为0!";
                    return false;
                }
                nRet = -1;

                //获取选择的设备信息
                MyCamera.MV_CC_DEVICE_INFO device =
                    (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_pDeviceList.pDeviceInfo[cameraModel.Index],
                                                                  typeof(MyCamera.MV_CC_DEVICE_INFO));

                //打开设备
                if (!cameraModel.IsOpen)
                {
                    nRet = m_pOperator[cameraModel.Index].Open(ref device);
                    if (MyCamera.MV_OK != nRet)
                    {
                        errorMsg = "打开相机失败!";
                        return false;
                    }
                }

                //设置为触发模式
                nRet = m_pOperator[cameraModel.Index].SetEnumValue("TriggerMode", 1);
                if (MyCamera.MV_OK != nRet)
                {
                    errorMsg = "设置触发失败!";
                    return false;
                }
                
                //是否为外部触发
                if (cameraModel.IsExternTrig)
                {
                    nRet = m_pOperator[cameraModel.Index].SetEnumValue("AcquisitionMode", 2);
                    if (MyCamera.MV_OK != nRet)
                    {
                        errorMsg = "设置AcquisitionMode失败!";
                        return false;
                    }

                    ImageCallback0 = new MyCamera.cbOutputdelegate(GetImageCamera0);
                    nRet = m_pOperator[cameraModel.Index].RegisterImageCallBack(ImageCallback0, IntPtr.Zero);
                    if (CameraOperator.CO_OK != nRet)
                    {
                        errorMsg = "RegisterImageCallBack失败!";
                        return false;
                    }
                    nRet = m_pOperator[cameraModel.Index].StartGrabbing();
                    if (MyCamera.MV_OK != nRet)
                    {
                        return false;
                    }
                }
                else
                {
                    ////设置心跳时间
                    //nRet = m_pOperator[cameraModel.Index].SetIntValue("GevHeartbeatTimeout", 10000);
                    //if (MyCamera.MV_OK != nRet)
                    //{
                    //    errorMsg = "设置GevHeartbeatTimeout失败!";
                    //    return false;
                    //}

                    //触发源设为软触发
                    nRet = m_pOperator[cameraModel.Index].SetEnumValue("TriggerSource", 7);
                    if (MyCamera.MV_OK != nRet)
                    {
                        errorMsg = "设置TriggerSource失败!";
                        return false;
                    }

                    //打开相机时开始采集图像
                    nRet = m_pOperator[cameraModel.Index].StartGrabbing();
                    if (CameraOperator.CO_OK != nRet)
                    {
                        errorMsg = "StartGrabbing失败!";
                        return false;
                    }
                }

                m_QImage.Clear();
            }

            return true;
        }

        public bool CameraSnap(Camera2DSetModel cameraModel)
        {
            //判断是否为外部触发
            if(!cameraModel.IsExternTrig)
            {
                int nRet;

                //每次不开始
                //nRet = m_pOperator[cameraModel.Index].StartGrabbing(); 

                //触发命令
                nRet = m_pOperator[cameraModel.Index].CommandExecute("TriggerSoftware");
                if (CameraOperator.CO_OK != nRet)
                {
                    return false;
                }
            }

            return true;
        }

        public bool CameraSnapContinue(Camera2DSetModel cameraModel)
        { 
            int nRet;
            nRet = m_pOperator[cameraModel.Index].SetEnumValue("AcquisitionMode", 2);// 工作在连续模式
            nRet = m_pOperator[cameraModel.Index].SetEnumValue("TriggerMode", 0);//设置相机是连续模式 
            //开始采集
            nRet = m_pOperator[cameraModel.Index].StartGrabbing();
            if (MyCamera.MV_OK != nRet)
            {
                return false;
            }
            return true;
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
                string errorMessage = "";
                bool bresult = CameraSetParamter(cameraModel, ref errorMessage);
                while (cameraModel.IsContinue)
                {
                    string error = "";
                    HObject hImage = null;
                    if(!cameraModel.IsExternTrig)
                    {
                        hImage = Snap(cameraModel, ref error);
                        if (!string.IsNullOrEmpty(error))
                        {
                            break;
                        }
                    }
                    else
                    {
                        hImage = CameaGetImageTrig();
                    } 

                    if (hImage != null && hImage.IsInitialized() && hImage.Key != (IntPtr)0)
                    {
                        List<object> list = new List<object>() { cameraModel.Id, hImage };
                        OnGetImageByContinue(list);
                    }

                    Thread.Sleep(50);
               }
            }));
            t.Start();
        }

        public bool ShowImage(object param)
        {
            //显示
            int nRet = m_pOperator[0].Display((IntPtr)param);
            if (MyCamera.MV_OK != nRet)
            {
                return false;
            }

            return true;
        }

        public bool CameraStopGrab(Camera2DSetModel cameraModel)
        {
            int nret; 
            nret = m_pOperator[cameraModel.Index].StopGrabbing();
            if (MyCamera.MV_OK != nret)
            { 
                return false;
            }
            return true;
        }

        public bool CameraClose(Camera2DSetModel cameraModel)
        {
            //关闭设备
            int nRet;
            if(!cameraModel.IsExternTrig)
            {
                nRet = m_pOperator[cameraModel.Index].SetEnumValue("TriggerMode", 0);//设置相机的触发模式
            }

            nRet = m_pOperator[cameraModel.Index].Close();
            if (CameraOperator.CO_OK != nRet)
            {
                return false;
            }
            cameraModel.IsOpen = false;
            Camera2DSetModel cameraparamModel = m_CameraModelIsOpenList.Find(x => x.DeviceName == cameraModel.DeviceName);
            if (cameraparamModel != null)
            {
                m_CameraModelIsOpenList.Remove(cameraparamModel);
            }

            m_QImage.Clear();
            return true;
        }

        public bool CameraSetParamter(Camera2DSetModel cameraModel, ref string errorMsg)
        {
            try
            {
                int nRet;
                m_pOperator[cameraModel.Index].SetEnumValue("ExposureAuto", 0);
                nRet = m_pOperator[cameraModel.Index].SetFloatValue("ExposureTime", (float)cameraModel.ExposureTime);
                if (nRet != CameraOperator.CO_OK)
                {
                    errorMsg = "设置曝光失败";
                    return false;
                }

                m_pOperator[cameraModel.Index].SetEnumValue("GainAuto", 0);
                nRet = m_pOperator[cameraModel.Index].SetFloatValue("Gain", (float)cameraModel.Gain);
                if (nRet != CameraOperator.CO_OK)
                {
                    errorMsg = "设置增益失败";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public bool CameraSetTriggerMode(Camera2DSetModel cameraModel)
        {
            int nRet; 
            nRet = m_pOperator[cameraModel.Index].SetEnumValue("TriggerMode", 0); 
            return true;
        }

        private Bitmap CameraGetImage(Camera2DSetModel cameraModel)
        {
            int nRet;
            UInt32 nPayloadSize = 0;
            nRet = m_pOperator[cameraModel.Index].GetIntValue("PayloadSize", ref nPayloadSize);
            if (MyCamera.MV_OK != nRet)
            {
                //MessageBox.Show("Get PayloadSize failed");
                return null;
            }

            //if (nPayloadSize + 2048 > m_nBufSizeForDriver)
            //{
            //    m_nBufSizeForDriver = nPayloadSize + 2048;
            //    m_pBufForDriver = new byte[m_nBufSizeForDriver];

            //    // 同时对保存图像的缓存做大小判断处理
            //    // BMP图片大小：width * height * 3 + 2048(预留BMP头大小)
            //    m_nBufSizeForSaveImage = m_nBufSizeForDriver * 3 + 2048;
            //    m_pBufForSaveImage = new byte[m_nBufSizeForSaveImage];
            //}

            IntPtr pData = Marshal.UnsafeAddrOfPinnedArrayElement(m_pBufForDriver, 0);
            UInt32 nDataLen = 0;
            MyCamera.MV_FRAME_OUT_INFO_EX stFrameInfo = new MyCamera.MV_FRAME_OUT_INFO_EX();

            //超时获取一帧，超时时间为1秒
            nRet = m_pOperator[cameraModel.Index].GetOneFrameTimeout(pData, ref nDataLen, m_nBufSizeForDriver, ref stFrameInfo, 1000);
            if (MyCamera.MV_OK != nRet)
            {
                //MessageBox.Show("无数据！");
                return null;
            }

            /************************Mono8 转 Bitmap*******************************/
            Bitmap bmp = new Bitmap(stFrameInfo.nWidth, stFrameInfo.nHeight, stFrameInfo.nWidth * 1, PixelFormat.Format8bppIndexed, pData);

            ColorPalette cp = bmp.Palette;
            // init palette
            for (int i = 0; i < 256; i++)
            {
                cp.Entries[i] = Color.FromArgb(i, i, i);
            }
            // set palette back
            bmp.Palette = cp;

            //bmp.Save("D:\\test.bmp", ImageFormat.Bmp);
            return bmp;

        }
        
        object locker = new object(); 
        private HObject Snap(Camera2DSetModel cameraModel, ref string errorMsg)
        {
            lock (locker)
            {
                try
                {
                    HObject imageObj = null;
                    if (!CameraSnap(cameraModel))
                    {
                        return null;
                    }

                    imageObj = CameraGetImage2(cameraModel);

                    //imageObj = AlgorithmCommHelper.ConvertHImageFromBitmap(CameraGetImage(cameraModel), out errorMsg);

                    return imageObj;
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    return null;
                }
            }

        }
        #endregion

        //外触发
        public event EventHandler<object> ExternalTrigger0;
        MyCamera.cbOutputdelegate ImageCallback0;
        protected void OnExternalTrigger0(object e)
        {
            EventHandler<object> handler = ExternalTrigger0;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void GetImageCamera0(IntPtr pData, ref MyCamera.MV_FRAME_OUT_INFO pFrameInfo, IntPtr pUser)
        {
            try
            {
                int nRet; 
                IntPtr pImage = Marshal.UnsafeAddrOfPinnedArrayElement(m_pBufForDriver, 0);
                UInt32 nDataLen = 0;
                MyCamera.MV_FRAME_OUT_INFO_EX stFrameInfo = new MyCamera.MV_FRAME_OUT_INFO_EX();

                //超时获取一帧，超时时间为1秒
                //nRet = m_pOperator[0].GetOneFrameTimeout(pImage, ref nDataLen, m_nBufSizeForSaveImage, ref stFrameInfo, 1000);
                //if (MyCamera.MV_OK != nRet)
                //{
                //    return ;
                //}

                //Bitmap bmp = RGBTOBITMAP(pData, stFrameInfo.nWidth, stFrameInfo.nHeight);

                //***********************Mono8 转 Bitmap*******************************/
                Bitmap bmp = new Bitmap(pFrameInfo.nWidth, pFrameInfo.nHeight, pFrameInfo.nWidth, PixelFormat.Format8bppIndexed, pData);

                ColorPalette cp = bmp.Palette;
                //init palette
                for (int i = 0; i < 256; i++)
                {
                    cp.Entries[i] = Color.FromArgb(i, i, i);
                }

                //set palette back
                bmp.Palette = cp;
                
                string errormsg;
                HObject ImageSnap = AlgorithmCommHelper.ConvertHImageFromBitmap(bmp, out errormsg);
                m_QImage.Enqueue(ImageSnap);
                OnExternalTrigger0(ImageSnap);
                 
            }
            catch (Exception ex)
            {
                 
            }
        }

        private HObject CameaGetImageTrig()
        {
            try
            {
                if(m_QImage.Count > 0)
                {
                    return m_QImage.Dequeue();
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;  
            }
        }
        
        private HObject CameraGetImage2(Camera2DSetModel cameraModel)
        {
            int nRet = MyCamera.MV_OK;
            //MyCamera.MV_FRAME_OUT stFrameOut = new MyCamera.MV_FRAME_OUT();

            IntPtr pImageBuf = IntPtr.Zero;
            int nImageBufSize = 0;

            HObject Hobj = new HObject();
            IntPtr pTemp = IntPtr.Zero;

            //while (m_bGrabbing)
            {
                IntPtr pData = Marshal.UnsafeAddrOfPinnedArrayElement(m_pBufForDriver, 0);
                UInt32 nDataLen = 0;
                MyCamera.MV_FRAME_OUT_INFO_EX stFrameInfo = new MyCamera.MV_FRAME_OUT_INFO_EX();

                //超时获取一帧，超时时间为1秒
                nRet = m_pOperator[cameraModel.Index].GetOneFrameTimeout(pData, ref nDataLen, m_nBufSizeForDriver, ref stFrameInfo, 2000);
                if (MyCamera.MV_OK != nRet)
                { 
                    return null;
                }

                //nRet = m_pOperator[cameraModel.Index].GetOneFrameInfo(ref stFrameOut, 1500);
                if (MyCamera.MV_OK == nRet)
                {
                    if (IsColorPixelFormat(stFrameInfo.enPixelType))
                    {
                        if (stFrameInfo.enPixelType == MyCamera.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed)
                        {
                            pTemp = pData;
                        }
                        else
                        {
                            if (IntPtr.Zero == pImageBuf || nImageBufSize < (stFrameInfo.nWidth * stFrameInfo.nHeight * 3))
                            {
                                if (pImageBuf != IntPtr.Zero)
                                {
                                    Marshal.FreeHGlobal(pImageBuf);
                                    pImageBuf = IntPtr.Zero;
                                }

                                pImageBuf = Marshal.AllocHGlobal((int)stFrameInfo.nWidth * stFrameInfo.nHeight * 3);
                                if (IntPtr.Zero == pImageBuf)
                                {
                                    return null;
                                }
                                nImageBufSize = stFrameInfo.nWidth * stFrameInfo.nHeight * 3;
                            }

                            MyCamera.MV_PIXEL_CONVERT_PARAM stPixelConvertParam = new MyCamera.MV_PIXEL_CONVERT_PARAM();

                            stPixelConvertParam.pSrcData = pData;//源数据
                            stPixelConvertParam.nWidth = stFrameInfo.nWidth;//图像宽度
                            stPixelConvertParam.nHeight = stFrameInfo.nHeight;//图像高度
                            stPixelConvertParam.enSrcPixelType = stFrameInfo.enPixelType;//源数据的格式
                            stPixelConvertParam.nSrcDataLen = stFrameInfo.nFrameLen;

                            stPixelConvertParam.nDstBufferSize = (uint)nImageBufSize;
                            stPixelConvertParam.pDstBuffer = pImageBuf;//转换后的数据
                            stPixelConvertParam.enDstPixelType = MyCamera.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed;
                            nRet = m_pOperator[cameraModel.Index].m_pCSI.MV_CC_ConvertPixelType_NET(ref stPixelConvertParam);//格式转换
                            if (MyCamera.MV_OK != nRet)
                            {
                                return null;
                            }
                            pTemp = pImageBuf;
                        }

                        try
                        {
                            HOperatorSet.GenImageInterleaved(out Hobj, (HTuple)pTemp, (HTuple)"rgb", (HTuple)stFrameInfo.nWidth, (HTuple)stFrameInfo.nHeight, -1, "byte", 0, 0, 0, 0, -1, 0);
                        }
                        catch (System.Exception ex)
                        {
                            return null;
                        }
                    }
                    else if (IsMonoPixelFormat(stFrameInfo.enPixelType))
                    {
                        if (stFrameInfo.enPixelType == MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8)
                        {
                            pTemp = pData;
                        }
                        else
                        {
                            if (IntPtr.Zero == pImageBuf || nImageBufSize < (stFrameInfo.nWidth * stFrameInfo.nHeight))
                            {
                                if (pImageBuf != IntPtr.Zero)
                                {
                                    Marshal.FreeHGlobal(pImageBuf);
                                    pImageBuf = IntPtr.Zero;
                                }

                                pImageBuf = Marshal.AllocHGlobal((int)stFrameInfo.nWidth * stFrameInfo.nHeight);
                                if (IntPtr.Zero == pImageBuf)
                                {
                                    return null;
                                }
                                nImageBufSize = stFrameInfo.nWidth * stFrameInfo.nHeight;
                            }

                            MyCamera.MV_PIXEL_CONVERT_PARAM stPixelConvertParam = new MyCamera.MV_PIXEL_CONVERT_PARAM();

                            stPixelConvertParam.pSrcData = pData;//源数据
                            stPixelConvertParam.nWidth = stFrameInfo.nWidth;//图像宽度
                            stPixelConvertParam.nHeight = stFrameInfo.nHeight;//图像高度
                            stPixelConvertParam.enSrcPixelType = stFrameInfo.enPixelType;//源数据的格式
                            stPixelConvertParam.nSrcDataLen = stFrameInfo.nFrameLen;

                            stPixelConvertParam.nDstBufferSize = (uint)nImageBufSize;
                            stPixelConvertParam.pDstBuffer = pImageBuf;//转换后的数据
                            stPixelConvertParam.enDstPixelType = MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8;
                            nRet = m_pOperator[cameraModel.Index].m_pCSI.MV_CC_ConvertPixelType_NET(ref stPixelConvertParam);//格式转换
                            if (MyCamera.MV_OK != nRet)
                            {
                               return null;
                            }
                            pTemp = pImageBuf;
                        }
                        try
                        {
                            HOperatorSet.GenImage1Extern(out Hobj, "byte", stFrameInfo.nWidth, stFrameInfo.nHeight, pTemp, IntPtr.Zero);
                        }
                        catch (System.Exception ex)
                        {
                            return null;
                        }
                    }
                    else
                    {
                        //m_pOperator[cameraModel.Index].m_pCSI.MV_CC_FreeImageBuffer_NET(ref stFrameOut); 
                    }
                    // HalconDisplay(m_Window, Hobj, stFrameOut.stFrameInfo.nHeight, stFrameOut.stFrameInfo.nWidth);

                   // m_pOperator[cameraModel.Index].m_pCSI.MV_CC_FreeImageBuffer_NET(ref stFrameOut);
                }
                else
                {
                    return null;
                }
            } 
            if (pImageBuf != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(pImageBuf);
                pImageBuf = IntPtr.Zero;
            }

            ////设置是否镜像
            //if (Hobj != null && Hobj.IsInitialized() && Hobj.Key != (IntPtr)0)
            //{
            //    if (cameraModel.IsReverseX)
            //    {
            //        HOperatorSet.MirrorImage(Hobj, out Hobj, "column");
            //    }
            //    if (cameraModel.IsReverseY)
            //    {
            //        HOperatorSet.MirrorImage(Hobj, out Hobj, "row");
            //    }
            //}

            return Hobj;
        }

        private bool IsMonoPixelFormat(MyCamera.MvGvspPixelType enType)
        {
            switch (enType)
            {
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono12_Packed:
                    return true;
                default:
                    return false;
            }
        }

        private bool IsColorPixelFormat(MyCamera.MvGvspPixelType enType)
        {
            switch (enType)
            {
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BGR8_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_RGBA8_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BGRA8_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_YUV422_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_YUV422_YUYV_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR12_Packed:
                    return true;
                default:
                    return false;
            }
        }

    }
}
