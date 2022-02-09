using Infrastructure.Log;
using System;
using System.Collections.Generic;

namespace Smartray.Sample
{
    internal class SensorManager : IDisposable
    {
        public delegate void ShowLog(string strLog, LogLevel loglevel = LogLevel.Info);
        public static ShowLog m_showLog;
        #region constants

        // constants !!! rename & use
        public const string DEFAULT_IP_ADR = "192.168.178.200";

        public const ushort DEFAULT_PORT_NUM = 40;

        #endregion constants

        #region members

        /// <summary>
        /// enable disable trace messages to the console output
        /// </summary>
        public static bool TraceOutput = true;

        // list of managed sensors
        private static List<Sensor> _sensors = new List<Sensor>();

        #endregion members

        #region helper functions

        // message and status callback, called by the api
        private static int ApiStatusCallback(Api.Sensor sensorObject, Api.MessageType msgType, Api.SubMessageType subMsgType, int msgData, string msg)
        {
            // checkk for message data available
            if (msg == null)
                return -1;

            // try to match the sensor object
            Sensor sensor;
            if (!TryGetSensor(sensorObject, out sensor))
                return -1;

            // handle ethernet connection messages
            if (msgType == Api.MessageType.Connection)
            {
                sensor._sensorState.LastConnectionMessage = msg;

                // TCP/IP connection established
                if (subMsgType == Api.SubMessageType.Connection_SensorConnected)
                    sensor._sensorState.SensorConnection = SensorState.ConnectionState.Connected;
                else
                    sensor._sensorState.SensorConnection = SensorState.ConnectionState.Disconnected;
            }

            // handle info messages
            else if (msgType == Api.MessageType.Info)
            {
                sensor._sensorState.LastInfoMessage = msg;
            }

            // handle error messages
            else if (msgType == Api.MessageType.Error)
            {
                sensor._sensorState.LastErrorMessage = msg;
            }

            // sensor IO and general data (sensor sends this every 200ms)
            else if (msgType == Api.MessageType.Data && subMsgType == Api.SubMessageType.Data_Io)
            {
                // can be used to track the alive status of the sensor
            }

            return 0;
        }

        // default callback when a live image is retrieved 8 Bit gray scale
        private static int LiveImageCallback(Api.Sensor sensorObject, Api.ImageDataType imageType, int originX, int height, int width, byte[] liveImage)
        {
            // try to match the sensor object
            Sensor sensor;
            if (!TryGetSensor(sensorObject, out sensor))
                return -1;

            Trace("*** Live image package " + ++(sensor._sensorState.ImagePackageCounter)
                + " received for sensor: " + sensorObject.name
                + " width: " + width
                + " height: " + height);

            // collect image data by adding the provided data package
            int size = width * height;
            var imageData = sensor.AddImageData(width, height, imageType, originX);
            imageData.LiveImage.AddPackageData(liveImage, size);

            // signal completed images
            sensor.SignalImageDataCompleted(imageData);

            Trace("\n");
            return 0;
        }

        public struct MetaDataStruct
        {
            public UInt32 StartTriggerNb;
            public UInt32 DataTriggerNb;
            public UInt32 ProfileNb;
            public UInt64 TimeStamp;
        }

        // default callback when a PIL image is retrieved 16 Bit image data (profile image, intensity
        // image, laser line thickness image)
        private static int PilImageCallback(Api.Sensor sensorObject, Api.ImageDataType imageType, int originX, int height, int width, ushort[] profileImage, ushort[] intensityImage, ushort[] lltImage, int numExtData, Byte[] extData)
        {
            // try to match the sensor object
            Sensor sensor;
            if (!TryGetSensor(sensorObject, out sensor))
                return -1;

            Trace("*** Profile image package " + ++(sensor._sensorState.ImagePackageCounter)
                + " received for sensor: " + sensorObject.name
                + " width: " + width
                + " height: " + height);

            // collect image data by adding the provided data package
            int size = width * height;
            SensorImageData imageData = sensor.AddImageData(width, height, imageType, originX);
            switch (imageType)
            {
                case Api.ImageDataType.Profile:
                    Trace("(profile image only)");
                    imageData.ProfileImage.AddPackageData(profileImage, size);
                    break;

                case Api.ImageDataType.Intensity:
                    Trace("(intensity image only)");
                    imageData.IntensityImage.AddPackageData(intensityImage, size);
                    break;

                case Api.ImageDataType.ProfileIntensity:
                    Trace("(profile & intensity image)");
                    imageData.ProfileImage.AddPackageData(profileImage, size);
                    imageData.IntensityImage.AddPackageData(intensityImage, size);
                    break;

                case Api.ImageDataType.ProfileIntensityLaserLineThickness:
                    Trace("(profile, intensity and laser line thickness image)");
                    imageData.ProfileImage.AddPackageData(profileImage, size);
                    imageData.IntensityImage.AddPackageData(intensityImage, size);
                    imageData.LaserLineThicknessImage.AddPackageData(lltImage, size);
                    break;

                default:
                    break;
            }

            // signal completed images
            sensor.SignalImageDataCompleted(imageData);

            Trace("\n");

            unsafe
            {
                if (sensor.GetMetaDataExportEnable() && numExtData == sizeof(MetaDataStruct))
                {
                    List<Api.MetaData> metaData = new List<Api.MetaData>();
                    
                    // By using the fixed keyword, we fix the array in a static memory location.
                    // Otherwise, the garbage collector might move it while we are still using it!

                    fixed (Byte* source = extData)
                    {
                        Byte* source_it = source;
                        Byte* source_end = source_it + extData.Length;

                        while (source_it < source_end)
                        {
                            MetaDataStruct* dest_it = (MetaDataStruct*)source_it;
                            source_it += sizeof(MetaDataStruct);

                            Api.MetaData apiMetaData = new Api.MetaData();
                            apiMetaData.DataTriggerNb = dest_it->DataTriggerNb;
                            apiMetaData.ProfileNb = dest_it->ProfileNb;
                            apiMetaData.StartTriggerNb = dest_it->StartTriggerNb;
                            apiMetaData.TimeStamp = dest_it->TimeStamp;

                            metaData.Add(apiMetaData);
                        }
                    }

                    sensor.AddMetaData((UInt32)height, metaData.ToArray());
                }
            }
            return 0;
        }

        private static int PointCloudCallback(Api.Sensor sensorObject, Api.ImageDataType imageType, int numPoints, int numProfiles, Api.Point3d[] pointCloud, ushort[] intensity, ushort[] laserlinethickness, uint[] porfileIndex, uint[] columnIndex, int numExtData, ushort[] extData)
        {
            Sensor sensor;
            if (!TryGetSensor(sensorObject, out sensor))
                return -1;

            sensor.AddPointCloudData(numPoints, numProfiles, pointCloud, intensity, laserlinethickness, porfileIndex, columnIndex);
            return 0;
        }

        // default callback when a ZIL (Zmap) image is retrieved 16 Bit image data (profile zmap
        // image, intensity zmap image, laser line thickness zmap image)
        private static int ZilImageCallback(Api.Sensor sensorObject, Api.ImageDataType imageType, int height, int width, float verticalRes, float horizontalRes, ushort[] zMap, ushort[] intensityZmap, ushort[] lltZmap, float originYMillimeters, int numExtData, ushort[] extdata)
        {
            // try to match the sensor object
            Sensor sensor;
            if (!TryGetSensor(sensorObject, out sensor))
                return -1;

            //Trace("*** ZMap image package " + ++(sensor._sensorState.ImagePackageCounter)
            //    + " received for sensor: " + sensorObject.name
            //    + " width: " + width
            //    + " height: " + height);
           // Trace("vertical Resolution [mm]: " + verticalRes
           //     + " horizontal Resolution [mm]: " + horizontalRes);

            // collect image data by adding the provided data package
            int size = width * height;
            SensorImageData imageData = sensor.AddImageData(width, height, imageType, 0, originYMillimeters);
            switch (imageType)
            {
                case Api.ImageDataType.ZMap:
                    //Trace("(zmap image only)");
                    imageData.ZMapImage.AddPackageData(zMap, size);
                    break;

                case Api.ImageDataType.ZMapIntensity:
                    //Trace("(intensity zmap image only)");
                    imageData.ZMapImage.AddPackageData(zMap, size);
                    imageData.ZMapIntensityImage.AddPackageData(intensityZmap, size);
                    break;

                case Api.ImageDataType.ZMapIntensityLaserLineThickness:
                    //Trace("(zmap, intensity zmap and laser line thickness zmap image)");
                    imageData.ZMapImage.AddPackageData(zMap, size);
                    imageData.ZMapIntensityImage.AddPackageData(intensityZmap, size);
                    imageData.ZMapLaserLineThicknessImage.AddPackageData(lltZmap, size);
                    break;

                default:
                    break;
            }

            // signal completed images
            sensor.SignalImageDataCompleted(imageData);

            //Trace("\n");
            return 0;
        }

        // handle error codes after calling an API function returns true when the api function succeeded
        public static bool HandleReturnCode(int apiReturnCode, bool fatal = true)
        {
            if (apiReturnCode == 0)
                return true;

            // try to get error string from api
            string apiErrorString;
            Api.GetErrorMsg(apiReturnCode, out apiErrorString);

            string errorText;
            if (apiErrorString != null)
                errorText = apiErrorString + " Error code:" + apiReturnCode;
            else
                errorText = " Smartray API call failed with error code: " + apiReturnCode;

            HandleError(errorText, fatal);
            return false;
        }

        // handle & report errors when fatal is true, the application is terminated
        internal static void HandleError(string text, bool fatal = true)
        {
            if (fatal)
            {
                foreach (Sensor sensor in SensorManager._sensors)
                {
                    if (sensor._sensorObject.connectionstate == (int)Api.SensorConnectionStatus.SensorConnected)
                    {
                        sensor.StopAcquisition();
                        sensor.Disconnect();
                    }
                }
                SensorManager._sensors.Clear();

                //Api.Exit(); will be called on throw, throw => calls SensorManager.Dispose()
                throw new FatalErrorException(text);
            }
            else
            {
                string prefix = fatal ? "FATAL: " : "ERROR: ";
                Trace(prefix + text);
            }
        }

        /// <summary>
        /// trace the message to the command window, if tracing is on
        /// </summary>
        internal static void Trace(string text)
        {
            if (TraceOutput)
                Console.WriteLine(text);
            if(m_showLog != null)
            {
                m_showLog(text);
            }
        }

        // try to get a sensor by it's internal sensor descriptor object otherwise NULL is returned
        private static bool TryGetSensor(Api.Sensor sensorObject, out Sensor sensor)
        {
            foreach (var sensorCandidate in _sensors)
            {
                // match the sensor object by its index
                if (sensorCandidate._sensorObject.cam_index == sensorObject.cam_index)
                {
                    sensor = sensorCandidate;
                    return true;
                }
            }
            sensor = null;
            return false;
        }

        // inits the Smartray API
        private void InitApi()
        {
            int ret;

            //=======================================
            // get API version
            //=======================================
            string apiVersion;
            ret = Api.GetApiVersion(out apiVersion);
            HandleReturnCode(ret);
            Trace("using Smartray API " + apiVersion);

            //=======================================
            // API Init & register status callback
            //=======================================
            ret = Api.Initialize(ApiStatusCallback);
            HandleReturnCode(ret);

            //=======================================
            // register image callbacks
            //=======================================
            // register live image callback
            Api.RegisterLiveImageCB(LiveImageCallback);
            // register PIL image callback
            Api.RegisterPilImageCB(PilImageCallback);
            // register ZIL callback
            Api.RegisterZilImageCB(ZilImageCallback);

            Api.RegisterPointCloudCB(PointCloudCallback);
        }

        // teardown the Smartray API
        private void DeinitApi()
        {
            //=======================================
            // teardown API
            //=======================================
            Trace("teardown API");
            int ret = Api.Exit();
            HandleReturnCode(ret);
        }

        #endregion helper functions

        // Ctor
        public SensorManager()
        {
            InitApi();
        }

        // Dispose
        public void Dispose()
        {
            DeinitApi();

            foreach (var sensor in _sensors)
                sensor.Dispose();
            _sensors.Clear();
        }

        // create sensor object
        public Sensor CreateSensor(string name, int multiSensorIndex = 0, string ipAddress = "192.168.178.200", ushort port = 40)
        {
            var sensor = new Sensor(name, multiSensorIndex, ipAddress, port);
            _sensors.Add(sensor);

            return sensor;
        }
    }

    #region FatalErrorException

    // fire when a fatal error occurs which disallows test execution
    internal class FatalErrorException : Exception
    {
        // ctor
        public FatalErrorException(string message)
            : base(message)
        { }
    }

    #endregion FatalErrorException
}