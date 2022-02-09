using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using Smartray.Sample;
using SequenceTestModel;
using Smartray;
using SmartRay;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace LaserController
{
    public class SRControl : ISensor
    { 
        SensorManager m_SensorManager;
        Sensor[] m_Sensors; 

        public event EventHandler<ImageClass> CompletedAcqEvent;
        public event EventHandler<string> MessageEvent; 

        public SRControl()
        {
            SensorManager.m_showLog += ShowLog;
            if(m_Sensors == null)
            {
                m_Sensors = new Sensor[2];
            }
        }

        public void ClearObject()
        {
            throw new NotImplementedException();
        }

        public void CloseSensor()
        {
            try
            {
                for (int i = 0; i < 2; i++)
                {
                    if (m_Sensors[i] != null)
                    {
                        if (m_Sensors[i].GetSensorState().SensorConnection != SensorState.ConnectionState.Disconnected)
                        {
                            m_Sensors[i].StopAcquisition();
                            m_Sensors[i].Disconnect();
                        }
                    }
                }

                if (m_SensorManager != null)
                {
                    m_SensorManager.Dispose();
                    m_SensorManager = null;
                }

            }
            catch (Exception ex)
            {
                 
            }
            
        }

        public object GetScanImage(int boardIndex)
        {
            throw new NotImplementedException();
        }

        public object GetSensor()
        {
            throw new NotImplementedException();
        }

        object m_lock = new object();

        public bool InitialSensor(SnapImageModel snapModel, string strIp, int port)
        {
            try
            {
                lock(m_lock)
                {
                    //Connect To Sensor0 
                    if (m_SensorManager == null)
                    {
                        m_SensorManager = new SensorManager();
                    }

                    if (m_Sensors[snapModel.LaserId] == null || m_Sensors[snapModel.LaserId]._sensorState.SensorConnection == SensorState.ConnectionState.Disconnected)
                    {
                        m_Sensors[snapModel.LaserId] = m_SensorManager.CreateSensor("Sensor" + snapModel.LaserId.ToString(), snapModel.LaserId, strIp, (ushort)port);
                        m_Sensors[snapModel.LaserId].Connect();

                        //Load Calibration File form Sensor0
                        m_Sensors[snapModel.LaserId].LoadCalibrationDataFromSensor();

                        string configName = Path.GetFileName(snapModel.ConfigPath);

                        string path = Application.StartupPath + @"\SoftwareConfig\" + configName;
                        //Loading Parameter set for Sensor0
                        Api.LoadParameterSetFromFile(m_Sensors[snapModel.LaserId]._sensorObject, path);

                        // define image acquiring Type
                        Api.SetImageAcquisitionType(m_Sensors[snapModel.LaserId]._sensorObject, Api.ImageAcquisitionType.ZMap);

                        //Setting Acquisition Mode as Continue Mode
                        Api.SetAcquisitionMode(m_Sensors[snapModel.LaserId]._sensorObject, Api.AcquisitionMode.RepeatSnapshot);

                        //Register Acquisition One image Complete Event
                        if (snapModel.LaserId == 0)
                        {
                            m_Sensors[snapModel.LaserId].AcqusitionCompletedEvent += new Sensor.delegateAcqusitionCompleted(OnCompletedAcqEvent); //new Sensor.AcqusitionCompleted(OnCompletedAcqEvent);
                        }
                        else
                        {
                            m_Sensors[snapModel.LaserId].AcqusitionCompletedEvent += new Sensor.delegateAcqusitionCompleted(OnCompletedAcqEvent2); //new Sensor.AcqusitionCompleted(OnCompletedAcqEvent);
                        }
                        m_Sensors[snapModel.LaserId].SendParameterSet();

                        int originX, width, originY, height;
                        Api.GetROI(m_Sensors[snapModel.LaserId]._sensorObject, out originX, out width, out originY, out height);
                        OnMessageEvent(string.Format("originX:{0} width:{1} originY:{2} height:{3}", originX, width, originY, height));
                    }

                    //重新加载曝光和增益
                    Api.SetExposureTime(m_Sensors[snapModel.LaserId]._sensorObject, snapModel.LaserId, snapModel.ExposureTime);
                    Api.SetGain(m_Sensors[snapModel.LaserId]._sensorObject, snapModel.EnableGain, snapModel.Gain);

                    return true;
                }                
            }
            catch (Exception ex)
            {
                OnMessageEvent("InitialSensor Error:" + ex.Message);
                if (m_Sensors[snapModel.LaserId] != null)
                {
                    m_Sensors[snapModel.LaserId].Disconnect();
                    m_Sensors[snapModel.LaserId] = null;
                }

                return false;
            }
        } 

        public Task AcqusitionFromSensor(bool isRun, uint numberofprofile, int index)
        {

            Task mtask = new Task(() =>
            { 
                try
                {
                    if (isRun)
                    {
                        Api.SetNumberOfProfilesToCapture(m_Sensors[index]._sensorObject, numberofprofile);
                        Api.SetPacketSize(m_Sensors[index]._sensorObject, 0);
                        Api.SetPacketTimeOut(m_Sensors[index]._sensorObject, 0);
                        m_Sensors[index].StartAcquisition();
                        //WriteCommunicationToClient("RGC,1,1;", 0, 0); 
                        //sensor0.WaitForImage(1);
                        Thread.Sleep(200);
                    }
                    else
                    {
                        HiPerfTimer timer = new HiPerfTimer();
                        timer.Start();
                        m_Sensors[index].StopAcquisition();
                        timer.Stop();
                        OnMessageEvent("Stop Acquisition function execute Time:" + timer.Duration);
                    }
                }
                catch (Exception ce)
                {
                    OnMessageEvent("AcqusitionFromSensor Error:" + ce.Message);
                    //WriteCommunicationToClient("RGC,1,0;", 0, 0);
                    //ShowLog("发送消息：" + "RGC,1,0;", LogLevel.Debug);
                    if (m_Sensors[index] != null)
                    {
                        m_Sensors[index].Disconnect();
                        m_Sensors[index] = null;
                    }
                }
            });
            mtask.Start();
            return mtask;

        }

        public void OnCompletedAcqEvent(ImageClass e)
        {
            CompletedAcqEvent?.Invoke(this, e);
        }

        public void StopAcquistion(int index)
        {
            try
            {
                if (m_Sensors[index] != null)
                {
                    m_Sensors[index].StopAcquisition();
                    m_Sensors[index].ClearImageData();
                }
            }
            catch (Exception ex)
            {
                 
            }
        }

        //...
        public void ShowLog(string str)
        {
            OnMessageEvent(str);
        }
        public void OnMessageEvent(string e)
        {
            MessageEvent?.Invoke(this, e);
        }
        public async void OnCompletedAcqEvent(object sender, EventArgs e)
        {
            try
            {
                Sensor sensor = sender as Sensor;
                int index = 0;
                for (int i = 0; i < 2; i++)
                {
                    if(sensor == m_Sensors[i])
                    {
                        index = i;
                        break;
                    }
                }

                Console.WriteLine("Acquisition Completed!");
                SensorImageData imageDatas = sensor.GetLastImageData();
                ushort[] dataArray = imageDatas.ZMapImage.GetImage();
                float a = 0f;
                SensorTemperature.GetSensorTemperatures(sensor._sensorObject, 4, ref a);
                HImage convertImage = ConvertuShortToHimage(dataArray, imageDatas.Width, dataArray.Length / imageDatas.Width);

                await AcqusitionFromSensor(false, 0, index);

                OnCompletedAcqEvent(new ImageClass
                {
                    index = index,
                    Image = convertImage,
                });
                convertImage.Dispose();

                sensor.ClearImageData();
                GC.Collect();
            }
            catch (Exception ex)
            {
                OnMessageEvent("OnCompletedAcqEvent Error:" + ex.ToString());
            }
        }

        public async void OnCompletedAcqEvent2(object sender, EventArgs e)
        {
            try
            {
                Sensor sensor = sender as Sensor;
                int index = 0;
                for (int i = 0; i < 2; i++)
                {
                    if (sensor == m_Sensors[i])
                    {
                        index = i;
                        break;
                    }
                }

                Console.WriteLine("Acquisition Completed!");
                SensorImageData imageDatas = sensor.GetLastImageData();
                ushort[] dataArray = imageDatas.ZMapImage.GetImage();
                float a = 0f;
                SensorTemperature.GetSensorTemperatures(sensor._sensorObject, 4, ref a);
                HImage convertImage = ConvertuShortToHimage(dataArray, imageDatas.Width, dataArray.Length / imageDatas.Width);

                await AcqusitionFromSensor(false, 0, index);

                OnCompletedAcqEvent(new ImageClass
                {
                    index = index,
                    Image = convertImage,
                });
                convertImage.Dispose();

                sensor.ClearImageData();
                GC.Collect();
            }
            catch (Exception ex)
            {
                OnMessageEvent("OnCompletedAcqEvent Error:" + ex.ToString());
            }
        }

        public HImage ConvertuShortToHimage(ushort[] datas, int ImageWidth, int ImageHeight)
        {
            HImage hImage = new HImage();
            try
            {
                unsafe
                {
                    fixed (ushort* m = datas)
                    {
                        hImage.GenImage1("uint2", ImageWidth, ImageHeight, new IntPtr(m));
                    }
                }
                GC.Collect();
            }
            catch (Exception ce)
            {
                hImage = null;
                OnMessageEvent("ConvertuShortToHimage Error:" + ce.Message);
            }
            return hImage;
        }

    }
}
