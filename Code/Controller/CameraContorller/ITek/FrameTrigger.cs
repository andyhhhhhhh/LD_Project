using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Threading;
using System.Runtime.InteropServices;

using IKapC.NET;

namespace CameraContorller
{
    class FrameTrigger
    {
        // Frame Grabber and camera parameters
        public IntPtr m_hCamera = new IntPtr(-1);                           // Camera device handle
        public IntPtr m_hStream = new IntPtr(-1);                            // Frame grabber device handle 
        List<IntPtr> m_hBufferList = new List<IntPtr>();                 // Image buffer list
        public string m_strFileName = "C:\\CSharpImage.tif";       // Save image name
        public int m_nCurFrameIndex = 0;                                          // Current grab frame index
        public int m_nBufferCountOfStream = 5;                              // Total frame count
        public IntPtr m_bufferData = new IntPtr(-1);                        // Read buffer data

        // Simple IKapC error handling
        static void CheckIKapC(uint res)
        {
            if (res != (uint)ItkStatusErrorId.ITKSTATUS_OK)
            {
                Console.Error.WriteLine("Error Code: {0}.\n", res.ToString("x8"));
                IKapCLib.ItkManTerminate();
                //Console.ReadLine();
                //Environment.Exit(1);
            }
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
            IntPtr hBuffer = m_hBufferList.ElementAt(m_nCurFrameIndex);
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
            }
            m_nCurFrameIndex++;
            m_nCurFrameIndex = m_nCurFrameIndex % m_nBufferCountOfStream;

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
        private void InitEnvironment()
        {
            uint res = (uint)ItkStatusErrorId.ITKSTATUS_OK;		                        // Return value of IKapC methods
            res = IKapCLib.ItkManInitialize();
            CheckIKapC(res);
        }

        // Release environment
        private void ReleaseEnvironment()
        {
            // ItkManTerminate
            IKapCLib.ItkManTerminate();
        }

        //  This function can be used to configure camera, form more examples please see IKapC usage
        private void ConfigureCamera()
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
                //Console.ReadLine();
                //Environment.Exit(1);
                return;
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

        //  This function can be used to create stream and add buffer
        private void CreateStreamAndBuffer()
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
            CheckIKapC(res);
            if (streamCount == 0)
            {
                Console.WriteLine("Camera does not have image stream channel.");
                /* Before exiting a program, ItkManTerminate() should be called to release
                all pylon related resources. */
                IKapCLib.ItkManTerminate();
                //Console.ReadLine();
                //Environment.Exit(1);
                return;
            }

            res = IKapCLib.ItkDevGetInt64(m_hCamera, "Width", ref nWidth);
            CheckIKapC(res);

            res = IKapCLib.ItkDevGetInt64(m_hCamera, "Height", ref nHeight);
            CheckIKapC(res);

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
                //Console.ReadLine();
                //Environment.Exit(1);
                return;
            }

            IntPtr hBuffer = new IntPtr();
            res = IKapCLib.ItkBufferNew(nWidth, nHeight, nFormat, ref hBuffer);
            CheckIKapC(res);
            m_hBufferList.Add(hBuffer);

            // Get buffer size
            res = IKapCLib.ItkBufferGetPrm(hBuffer, (uint)ItkBufferPrm.ITKBUFFER_PRM_SIZE, nImageSize);
            CheckIKapC(res);
            nBufferSize = (uint)Marshal.ReadInt64(nImageSize);

            // apply buffer data
            m_bufferData = Marshal.AllocHGlobal((int)nBufferSize);
            if (m_bufferData.Equals(-1))
            {
                Console.WriteLine("Apply buffer data failure");
                IKapCLib.ItkManTerminate();
                //Console.ReadLine();
                //Environment.Exit(1);
                return;
            }

            /* Allocate stream handle for image grab. */
            res = IKapCLib.ItkDevAllocStream(m_hCamera, 0, hBuffer, ref m_hStream);
            CheckIKapC(res);

            for (int i = 1; i < m_nBufferCountOfStream; i++)
            {
                res = IKapCLib.ItkBufferNew(nWidth, nHeight, nFormat, ref hBuffer);
                CheckIKapC(res);
                res = IKapCLib.ItkStreamAddBuffer(m_hStream, hBuffer);
                CheckIKapC(res);
                m_hBufferList.Add(hBuffer);
            }
        }

        // This function can be used to configure stream, form more examples please see IKapC usage
        private void ConfigureStream()
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
            CheckIKapC(res);

            res = IKapCLib.ItkStreamSetPrm(m_hStream, (uint)ItkStreamPrm.ITKSTREAM_PRM_TRANSFER_MODE, xferMode);
            CheckIKapC(res);

            res = IKapCLib.ItkStreamSetPrm(m_hStream, (uint)ItkStreamPrm.ITKSTREAM_PRM_TIME_OUT, timeOut);
            CheckIKapC(res);

            // Register callback which will be called at the begin of stream
            cbOnStartOfStreamProc = new IKapCLib.PITKSTREAMCALLBACK(cbOnStartOfStreamFunc);
            res = IKapCLib.ItkStreamRegisterCallback(m_hStream, (uint)ItkStreamEventType.ITKSTREAM_VAL_EVENT_TYPE_START_OF_STREAM, cbOnStartOfStreamProc, m_hStream);
            CheckIKapC(res);

            // Register callback which will be called at the end of stream
            cbOnEndOfStreamProc = new IKapCLib.PITKSTREAMCALLBACK(cbOnEndOfStreamFunc);
            res = IKapCLib.ItkStreamRegisterCallback(m_hStream, (uint)ItkStreamEventType.ITKSTREAM_VAL_EVENT_TYPE_END_OF_STREAM, cbOnEndOfStreamProc, m_hStream);
            CheckIKapC(res);

            // Register callback which will be called at the end of one image completely
            cbOnEndOfFrameProc = new IKapCLib.PITKSTREAMCALLBACK(cbOnEndOfFrameFunc);
            res = IKapCLib.ItkStreamRegisterCallback(m_hStream, (uint)ItkStreamEventType.ITKSTREAM_VAL_EVENT_TYPE_END_OF_FRAME, cbOnEndOfFrameProc, m_hStream);
            CheckIKapC(res);

            // Register callback which will be called at the time out
            cbOnTimeOutProc = new IKapCLib.PITKSTREAMCALLBACK(cbOnTimeOutFunc);
            res = IKapCLib.ItkStreamRegisterCallback(m_hStream, (uint)ItkStreamEventType.ITKSTREAM_VAL_EVENT_TYPE_TIME_OUT, cbOnTimeOutProc, m_hStream);
            CheckIKapC(res);

            // Register callback which will be called at the frame lost
            cbOnFrameLostProc = new IKapCLib.PITKSTREAMCALLBACK(cbOnFrameLostFunc);
            res = IKapCLib.ItkStreamRegisterCallback(m_hStream, (uint)ItkStreamEventType.ITKSTREAM_VAL_EVENT_TYPE_FRAME_LOST, cbOnFrameLostProc, m_hStream);
            CheckIKapC(res);
        }

        // This function will be frame trigger
        void SetFrameTrigger()
        {
            uint res = (uint)ItkStatusErrorId.ITKSTATUS_OK;

            // Turn off line trigger
            res = IKapCLib.ItkDevFromString(m_hCamera, "TriggerSelector", "LineStart");
            CheckIKapC(res);
            res = IKapCLib.ItkDevFromString(m_hCamera, "TriggerMode", "Off");
            CheckIKapC(res);

            // Trigger selector
            res = IKapCLib.ItkDevFromString(m_hCamera, "TriggerSelector", "FrameStart");
            CheckIKapC(res);

            // Set trigger mode
            res = IKapCLib.ItkDevFromString(m_hCamera, "TriggerMode", "On");
            CheckIKapC(res);

            // Select trigger source(Line3、Software)
            res = IKapCLib.ItkDevFromString(m_hCamera, "TriggerSource", "Line3");
            CheckIKapC(res);
        }

        // This function will be unregister callback
        private void UnRegisterCallback()
        {
            uint res = (uint)ItkStatusErrorId.ITKSTATUS_OK;
            res = IKapCLib.ItkStreamUnregisterCallback(m_hStream, (uint)ItkStreamEventType.ITKSTREAM_VAL_EVENT_TYPE_START_OF_STREAM);
            res = IKapCLib.ItkStreamUnregisterCallback(m_hStream, (uint)ItkStreamEventType.ITKSTREAM_VAL_EVENT_TYPE_END_OF_STREAM);
            res = IKapCLib.ItkStreamUnregisterCallback(m_hStream, (uint)ItkStreamEventType.ITKSTREAM_VAL_EVENT_TYPE_END_OF_FRAME);
            res = IKapCLib.ItkStreamUnregisterCallback(m_hStream, (uint)ItkStreamEventType.ITKSTREAM_VAL_EVENT_TYPE_TIME_OUT);
            res = IKapCLib.ItkStreamUnregisterCallback(m_hStream, (uint)ItkStreamEventType.ITKSTREAM_VAL_EVENT_TYPE_FRAME_LOST);
        }

        // This function will be close device and release buffer
        private void CloseDevice()
        {
            // Free stream and buffer
            foreach (var it in m_hBufferList)
            {
                IKapCLib.ItkStreamRemoveBuffer(m_hStream, it);
                IKapCLib.ItkBufferFree(it);
            }
            m_hBufferList.Clear();
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
            FrameTrigger grab = new FrameTrigger();

            // Initialize Environment
            grab.InitEnvironment();

            // Configure camera
            grab.ConfigureCamera();

            // Set frame Trigger
            grab.SetFrameTrigger();

            // Create stream and add buffer
            grab.CreateStreamAndBuffer();

            // Configure stream
            grab.ConfigureStream();

            // Start capturing image
            res = IKapCLib.ItkStreamStart(grab.m_hStream, (uint)IKapCLib.ITKSTREAM_CONTINUOUS);
            CheckIKapC(res);

            // wait
            Console.ReadLine();

            // Stop capturing image
            res = IKapCLib.ItkStreamStop(grab.m_hStream);
            CheckIKapC(res);

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
