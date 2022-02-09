using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Threading;
using System.Runtime.InteropServices;

using IKapC.NET;
using HalconDotNet;

namespace CameraContorller
{
    class GrabOnce
    {
        // Frame Grabber and camera parameters
        public IntPtr m_hCamera = new IntPtr(-1);                           // Camera device handle
        public IntPtr m_hStream = new IntPtr(-1);                            // Frame grabber device handle 
        public IntPtr m_hBuffer = new IntPtr(-1);                              // Image buffer
        public string m_strFileName = "C:\\CSharpImage.tif";       // Save image name
        public IntPtr m_bufferData = new IntPtr(-1);                        // Read buffer data

        public Queue<HObject> m_QImage = new Queue<HObject>();
        int m_width = 0;
        int m_height = 0;
        // Simple IKapC error handling
        public bool CheckIKapC(uint res)
        {
            if (res != (uint)ItkStatusErrorId.ITKSTATUS_OK)
            {
                Console.Error.WriteLine("Error Code: {0}.\n", res.ToString("x8"));
                IKapCLib.ItkManTerminate();
                //Console.ReadLine();
                //Environment.Exit(1);
                return false;
            }

            return true;
        }

        #region Callback
        public IKapCLib.PITKSTREAMCALLBACK cbOnStartOfStreamProc = null;
        public IKapCLib.PITKSTREAMCALLBACK cbOnEndOfFrameProc = null;
        public IKapCLib.PITKSTREAMCALLBACK cbOnTimeOutProc = null;
        public IKapCLib.PITKSTREAMCALLBACK cbOnFrameLostProc = null;
        public IKapCLib.PITKSTREAMCALLBACK cbOnEndOfStreamProc = null;
        #endregion

        #region Callback
        // This callback function will be called on grab start
        public void cbOnStartOfStreamFunc(uint eventType, IntPtr pContext)
        {
            Console.WriteLine("Start of stream");
        }
        // This callback function will be called at the frame ready
        public void cbOnEndOfFrameFunc(uint eventType, IntPtr pContext)
        {
            Console.WriteLine("End of frame");
            // Get image data and save
            uint res = (uint)ItkStatusErrorId.ITKSTATUS_OK;
            IntPtr bufferStatus = Marshal.AllocHGlobal(4);
            IntPtr nImageSize = Marshal.AllocHGlobal(8);
            IntPtr hBuffer = m_hBuffer;
            uint nStatus = 0;
            uint nBufferSize = 0;

            res = IKapCLib.ItkBufferGetPrm(hBuffer, (uint)ItkBufferPrm.ITKBUFFER_PRM_STATE, (IntPtr)(bufferStatus));
            CheckIKapC(res);
            nStatus = (uint)Marshal.ReadInt32(bufferStatus);
            if (nStatus == (uint)ItkBufferState.ITKBUFFER_VAL_STATE_FULL || nStatus == (uint)ItkBufferState.ITKBUFFER_VAL_STATE_UNCOMPLETED)
            {
                // Save buffer 
                /*
                res = IKapCLib.ItkBufferSave(hBuffer, m_strFileName, (uint)ItkBufferSaveType.ITKBUFFER_VAL_TIFF);
                CheckIKapC(res);
                 */

                // Read buffer
                res = IKapCLib.ItkBufferGetPrm(hBuffer, (uint)ItkBufferPrm.ITKBUFFER_PRM_SIZE, nImageSize);
                CheckIKapC(res);
                nBufferSize = (uint)Marshal.ReadInt64(nImageSize);
                res = IKapCLib.ItkBufferRead(hBuffer, 0, m_bufferData, (uint)nBufferSize);
                CheckIKapC(res);


                HObject ho_Image;
                HOperatorSet.GenImage1(out ho_Image, "byte", m_width, m_height, m_bufferData);
                m_QImage.Enqueue(ho_Image);
            }
        }
        // This callback function will be called on grabbing timeout
        public void cbOnTimeOutFunc(uint eventType, IntPtr pContext)
        {
            Console.WriteLine("Grab timeout");
        }
        // This callback function will be called on frame lost
        public void cbOnFrameLostFunc(uint eventType, IntPtr pContext)
        {
            Console.WriteLine("Grab Frame lost");
        }
        // This callback function will be called on grab stop
        public void cbOnEndOfStreamFunc(uint eventType, IntPtr pContext)
        {
            Console.WriteLine("End of stream");
        }
        #endregion

        #region member function
        // Initialize environment
        public void InitEnvironment()
        {
            uint res = (uint)ItkStatusErrorId.ITKSTATUS_OK;		                        // Return value of IKapC methods
            res = IKapCLib.ItkManInitialize();
            CheckIKapC(res);
        }

        // Release environment
        public void ReleaseEnvironment()
        {
            // ItkManTerminate
            IKapCLib.ItkManTerminate();
        }
        public List<string> DisCoverDevice()
        {
            List<string> cameraSerialNums = new List<string>();

            uint res = (uint)ItkStatusErrorId.ITKSTATUS_OK;
            uint numDevices = 0;

            //// Before using any IKapC methods, the IKapC runtime must be initialized
            //res = IKapCLib.ItkManInitialize();
            //CheckIKapC(res);

            // Enumerate all camera devices. You must call ItkManGetDeviceCount() before creating a device
            res = IKapCLib.ItkManGetDeviceCount(ref numDevices);
            CheckIKapC(res);
            if (numDevices == 0)
            {
                Console.WriteLine("No devices found");
                IKapCLib.ItkManTerminate();
                return cameraSerialNums;
            }

            // Print out the information of the camera connected to PC now
            for (uint i = 0; i < numDevices; i++)
            {
                IKapCLib.ITKDEV_INFO di = new IKapCLib.ITKDEV_INFO();
                IKapCLib.ItkManGetDeviceInfo(i, ref di);
                Console.WriteLine("Device Full Name:{0}\nFriendly Name:{1}\nVendor Name:{2}\nModel Name:{3}\nSerial Name:{4}\nDevice Class:{5}\nDevice Version:{6}\nUser Defined Name:{7}\n",
                                di.FullName,
                                di.FriendlyName,
                                di.VendorName,
                                di.ModelName,
                                di.SerialNumber,
                                di.DeviceClass,
                                di.DeviceVersion,
                                di.UserDefinedName);

                cameraSerialNums.Add(di.SerialNumber);
            }

            // Shut down the IKapC runtime system. Don't call any IKapC method afte calling ItkManTerminate()
            // IKapCLib.ItkManTerminate();

            return cameraSerialNums;
        }

        //  This function can be used to configure camera, form more examples please see IKapC usage
        public void ConfigureCamera()
        {
            uint res = (uint)ItkStatusErrorId.ITKSTATUS_OK;		                        // Return value of IKapC methods
            uint numCameras = 0;

            // Enumerate all camera devices. You must call ItkManGetDeviceCount() before creating a device. 
            res = IKapCLib.ItkManGetDeviceCount(ref numCameras);
            CheckIKapC(res);

            if (numCameras == 0)
            {
                Console.Write("No cameras found.\n");
                IKapCLib.ItkManTerminate();
                Console.ReadLine();
                Environment.Exit(1);
            }

            // Open first CameraLink Camera.
            for (uint i = 0; i < numCameras; i++)
            {
                IKapCLib.ITKDEV_INFO di = new IKapCLib.ITKDEV_INFO();

                res = IKapCLib.ItkManGetDeviceInfo(i, ref di);
                Console.Write("Using camera: serial: {0}, name: {1}, interface: {2}.\n", di.SerialNumber, di.FullName, di.DeviceClass);

                // Only use CameraLink camera with proper serial number
                if (di.DeviceClass == "GigEVision" && di.SerialNumber != "")
                {
                    IKapCLib.ITKGIGEDEV_INFO gv_board_info = new IKapCLib.ITKGIGEDEV_INFO();

                    /* Open camera. */
                    res = IKapCLib.ItkDevOpen(i, (int)ItkDeviceAccessMode.ITKDEV_VAL_ACCESS_MODE_EXCLUSIVE, ref m_hCamera);
                    CheckIKapC(res);

                    res = IKapCLib.ItkManGetGigEDeviceInfo(i, ref gv_board_info);
                    CheckIKapC(res);

                    break;
                }
            }
        }
        public bool ConfigureCamera(int index)
        {
            uint res = (uint)ItkStatusErrorId.ITKSTATUS_OK;// Return value of IKapC methods
            uint numDevices = 0;

            res = IKapCLib.ItkManGetDeviceCount(ref numDevices);
            if (!CheckIKapC(res))
            {
                return false;
            }

            // Print out the information of the camera connected to PC now 
            //IKapCLib.ITKDEV_INFO di = new IKapCLib.ITKDEV_INFO();
            //IKapCLib.ItkManGetDeviceInfo((uint)index, ref di);

            /* Open camera. */
            res = IKapCLib.ItkDevOpen((uint)index, (int)ItkDeviceAccessMode.ITKDEV_VAL_ACCESS_MODE_EXCLUSIVE, ref m_hCamera);
            return CheckIKapC(res);
        }

        //  This function can be used to create stream and add buffer
        public bool CreateStreamAndBuffer()
        {
            uint res = (uint)ItkStatusErrorId.ITKSTATUS_OK;		                                                    // Return value of IKapC methods
            uint streamCount = 0;															                                        // Stream count
            Int64 nWidth = 0;																	                                        // Image width
            Int64 nHeight = 0;																	                                        // Image height
            uint nFormat = (uint)ItkBufferFormat.ITKBUFFER_VAL_FORMAT_MONO8;		// Image format
            IntPtr nImageSize = Marshal.AllocHGlobal(8);															    // Image size
            StringBuilder pixelFormat = new StringBuilder(128);	                                                     // Image pixel format
            uint pixelFormatSize = 128;
            uint nBufferSize = 0;

            // Get stream count
            res = IKapCLib.ItkDevGetStreamCount(m_hCamera, ref streamCount);
            if (!CheckIKapC(res))
            {
                return false;
            }
            if (streamCount == 0)
            {
                Console.WriteLine("Camera does not have image stream channel.");
                /* Before exiting a program, ItkManTerminate() should be called to release
                all pylon related resources. */
                IKapCLib.ItkManTerminate();
                return false;
            }

            res = IKapCLib.ItkDevGetInt64(m_hCamera, "Width", ref nWidth);
            m_width = (int)nWidth;
            if (!CheckIKapC(res))
            {
                return false;
            }

            res = IKapCLib.ItkDevGetInt64(m_hCamera, "Height", ref nHeight);
            m_height = (int)nHeight;
            if (!CheckIKapC(res))
            {
                return false;
            }

            res = IKapCLib.ItkDevToString(m_hCamera, "PixelFormat", pixelFormat, ref pixelFormatSize);
            CheckIKapC(res);
            if (pixelFormat.ToString() == "Mono8")
                nFormat = (uint)ItkBufferFormat.ITKBUFFER_VAL_FORMAT_MONO8;
            else if (pixelFormat.ToString() == "Mono10")
                nFormat = (uint)ItkBufferFormat.ITKBUFFER_VAL_FORMAT_MONO10;
            else if (pixelFormat.ToString() == "Mono10Packed")
                nFormat = (uint)ItkBufferFormat.ITKBUFFER_VAL_FORMAT_MONO10PACKED;
            else if (pixelFormat.ToString() == "BayerGR8")
                nFormat = (uint)ItkBufferFormat.ITKBUFFER_VAL_FORMAT_BAYER_GR8;
            else if (pixelFormat.ToString() == "BayerRG8")
                nFormat = (uint)ItkBufferFormat.ITKBUFFER_VAL_FORMAT_BAYER_RG8;
            else if (pixelFormat.ToString() == "BayerGB8")
                nFormat = (uint)ItkBufferFormat.ITKBUFFER_VAL_FORMAT_BAYER_GB8;
            else if (pixelFormat.ToString() == "BayerBG8")
                nFormat = (uint)ItkBufferFormat.ITKBUFFER_VAL_FORMAT_BAYER_BG8;
            else
            {
                Console.WriteLine("Camera does not support pixel format %s.", pixelFormat);
                /* Before exiting a program, ItkManTerminate() should be called to release
                all pylon related resources. */
                IKapCLib.ItkManTerminate();
                return false;
            }

            IntPtr hBuffer = new IntPtr();
            res = IKapCLib.ItkBufferNew(nWidth, nHeight, nFormat, ref hBuffer);
            if (!CheckIKapC(res))
            {
                return false;
            }
            m_hBuffer = hBuffer;

            // Get buffer size
            res = IKapCLib.ItkBufferGetPrm(hBuffer, (uint)ItkBufferPrm.ITKBUFFER_PRM_SIZE, nImageSize);
            if (!CheckIKapC(res))
            {
                return false;
            }
            nBufferSize = (uint)Marshal.ReadInt64(nImageSize);

            // apply buffer data
            m_bufferData = Marshal.AllocHGlobal((int)nBufferSize);
            if (m_bufferData.Equals(-1))
            {
                Console.WriteLine("Apply buffer data failure");
                IKapCLib.ItkManTerminate();
                return false;
            }

            /* Allocate stream handle for image grab. */
            res = IKapCLib.ItkDevAllocStream(m_hCamera, 0, hBuffer, ref m_hStream);
            if (!CheckIKapC(res))
            {
                return false;
            }

            return true;
        }

        // This function can be used to configure stream, form more examples please see IKapC usage
        public bool ConfigureStream()
        {
            uint res = (uint)ItkStatusErrorId.ITKSTATUS_OK;
            IntPtr xferMode = Marshal.AllocHGlobal(4);	                            // Transfer image in asynchronous mode    
            Marshal.WriteInt32(xferMode, 0, (int)ItkStreamTransferMode.ITKSTREAM_VAL_TRANSFER_MODE_SYNCHRONOUS_WITH_PROTECT);
            IntPtr startMode = Marshal.AllocHGlobal(4);		                    // Start mode
            Marshal.WriteInt32(startMode, 0, (int)ItkStreamStartMode.ITKSTREAM_VAL_START_MODE_NON_BLOCK);
            IntPtr timeOut = Marshal.AllocHGlobal(4);		                            // Image transfer timeout
            Marshal.WriteInt32(timeOut, 0, (int)IKapCLib.ITKSTREAM_CONTINUOUS);

            /* Set block mode which means the grab will not be stopped before an entire image
            come into buffer. */
            res = IKapCLib.ItkStreamSetPrm(m_hStream, (uint)ItkStreamPrm.ITKSTREAM_PRM_START_MODE, startMode);
            if (!CheckIKapC(res))
            {
                return false;
            }

            res = IKapCLib.ItkStreamSetPrm(m_hStream, (uint)ItkStreamPrm.ITKSTREAM_PRM_TRANSFER_MODE, xferMode);
            if (!CheckIKapC(res))
            {
                return false;
            }

            res = IKapCLib.ItkStreamSetPrm(m_hStream, (uint)ItkStreamPrm.ITKSTREAM_PRM_TIME_OUT, timeOut);
            if (!CheckIKapC(res))
            {
                return false;
            }

            // Register callback which will be called at the begin of stream
            cbOnStartOfStreamProc = new IKapCLib.PITKSTREAMCALLBACK(cbOnStartOfStreamFunc);
            res = IKapCLib.ItkStreamRegisterCallback(m_hStream, (uint)ItkStreamEventType.ITKSTREAM_VAL_EVENT_TYPE_START_OF_STREAM, cbOnStartOfStreamProc, m_hStream);
            if (!CheckIKapC(res))
            {
                return false;
            }

            // Register callback which will be called at the end of stream
            cbOnEndOfStreamProc = new IKapCLib.PITKSTREAMCALLBACK(cbOnEndOfStreamFunc);
            res = IKapCLib.ItkStreamRegisterCallback(m_hStream, (uint)ItkStreamEventType.ITKSTREAM_VAL_EVENT_TYPE_END_OF_STREAM, cbOnEndOfStreamProc, m_hStream);
            if (!CheckIKapC(res))
            {
                return false;
            }

            // Register callback which will be called at the end of one image completely
            cbOnEndOfFrameProc = new IKapCLib.PITKSTREAMCALLBACK(cbOnEndOfFrameFunc);
            res = IKapCLib.ItkStreamRegisterCallback(m_hStream, (uint)ItkStreamEventType.ITKSTREAM_VAL_EVENT_TYPE_END_OF_FRAME, cbOnEndOfFrameProc, m_hStream);
            if (!CheckIKapC(res))
            {
                return false;
            }

            // Register callback which will be called at the time out
            cbOnTimeOutProc = new IKapCLib.PITKSTREAMCALLBACK(cbOnTimeOutFunc);
            res = IKapCLib.ItkStreamRegisterCallback(m_hStream, (uint)ItkStreamEventType.ITKSTREAM_VAL_EVENT_TYPE_TIME_OUT, cbOnTimeOutProc, m_hStream);
            if (!CheckIKapC(res))
            {
                return false;
            }

            // Register callback which will be called at the frame lost
            cbOnFrameLostProc = new IKapCLib.PITKSTREAMCALLBACK(cbOnFrameLostFunc);
            res = IKapCLib.ItkStreamRegisterCallback(m_hStream, (uint)ItkStreamEventType.ITKSTREAM_VAL_EVENT_TYPE_FRAME_LOST, cbOnFrameLostProc, m_hStream);
            if (!CheckIKapC(res))
            {
                return false;
            }

            return true;
        }


        // This function will be line trigger
        public bool SetLineTrigger()
        {
            uint res = (uint)ItkStatusErrorId.ITKSTATUS_OK;

            // Turn off frame trigger
            res = IKapCLib.ItkDevFromString(m_hCamera, "TriggerSelector", "FrameStart");
            if (!CheckIKapC(res))
            {
                return false;
            }

            res = IKapCLib.ItkDevFromString(m_hCamera, "TriggerMode", "Off");
            if (!CheckIKapC(res))
            {
                return false;
            }

            // Trigger selector
            res = IKapCLib.ItkDevFromString(m_hCamera, "TriggerSelector", "LineStart");
            if (!CheckIKapC(res))
            {
                return false;
            }

            // Set trigger mode
            res = IKapCLib.ItkDevFromString(m_hCamera, "TriggerMode", "On");
            if (!CheckIKapC(res))
            {
                return false;
            }

            // Select trigger source(Line1、Line2、RotaryEncoder1)
            res = IKapCLib.ItkDevFromString(m_hCamera, "TriggerSource", "Line1");
            if (!CheckIKapC(res))
            {
                return false;
            }

            return true;
        }

        // This function will be unregister callback
        public void UnRegisterCallback()
        {
            uint res = (uint)ItkStatusErrorId.ITKSTATUS_OK;
            res = IKapCLib.ItkStreamUnregisterCallback(m_hStream, (uint)ItkStreamEventType.ITKSTREAM_VAL_EVENT_TYPE_START_OF_STREAM);
            res = IKapCLib.ItkStreamUnregisterCallback(m_hStream, (uint)ItkStreamEventType.ITKSTREAM_VAL_EVENT_TYPE_END_OF_STREAM);
            res = IKapCLib.ItkStreamUnregisterCallback(m_hStream, (uint)ItkStreamEventType.ITKSTREAM_VAL_EVENT_TYPE_END_OF_FRAME);
            res = IKapCLib.ItkStreamUnregisterCallback(m_hStream, (uint)ItkStreamEventType.ITKSTREAM_VAL_EVENT_TYPE_TIME_OUT);
            res = IKapCLib.ItkStreamUnregisterCallback(m_hStream, (uint)ItkStreamEventType.ITKSTREAM_VAL_EVENT_TYPE_FRAME_LOST);
        }

        // This function will be close device and release buffer
        public void CloseDevice()
        {
            // Free stream and buffer
            IKapCLib.ItkStreamRemoveBuffer(m_hStream, m_hBuffer);
            IKapCLib.ItkBufferFree(m_hBuffer);
            IKapCLib.ItkDevFreeStream(m_hStream);

            // Close camera
            if (!m_hCamera.Equals(-1))
            {
                IKapCLib.ItkDevClose(m_hCamera);
                m_hCamera = (IntPtr)(-1);
            }

            if (!m_bufferData.Equals(-1))
            {
                Marshal.FreeHGlobal(m_bufferData);
            }
        }
        #endregion

        [STAThread]
        static void Main(string[] args)
        {
            uint res = (uint)ItkStatusErrorId.ITKSTATUS_OK;
            GrabOnce grab = new GrabOnce();

            // Initialize Environment
            grab.InitEnvironment();

            // Configure camera
            grab.ConfigureCamera();

            // Create stream and add buffer
            grab.CreateStreamAndBuffer();

            // Configure stream
            grab.ConfigureStream();

            // Start capturing image
            res = IKapCLib.ItkStreamStart(grab.m_hStream, 1);
            //CheckIKapC(res);

            // wait
            res = IKapCLib.ItkStreamWait(grab.m_hStream);
           // CheckIKapC(res);

            // Stop capturing image
            res = IKapCLib.ItkStreamStop(grab.m_hStream);
           // CheckIKapC(res);

            // UnRegister callback
            grab.UnRegisterCallback();

            // Close device
            grab.CloseDevice();

            Console.ReadLine();

            // Release environment
            grab.ReleaseEnvironment();
        }
    }
}
