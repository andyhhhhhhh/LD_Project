using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using XMLController;
using GlobalCore;
using SequenceTestModel;
using DevComponents.DotNetBar;
using HalconView;
using Smartray.Sample;
using HalconDotNet;
using System.Threading;
using Smartray;

namespace ManagementView._3DViews
{
    public partial class ImageSet : UserControl
    {
        HSmartWindow m_hSmartWindow = new HSmartWindow();
        public ImageSet()
        {
            InitializeComponent(); 
        }

        private void ImageSet_Load(object sender, EventArgs e)
        {
            try
            {
                numExposureTime.TxtValueEvent += NumExposureTime_TxtValueEvent;
                numGain.TxtValueEvent += NumGain_TxtValueEvent;
                numBrightness.TxtValueEvent += NumBrightness_TxtValueEvent; 

                CommHelper.LayoutChildFillView(panelView, m_hSmartWindow);
                this.ParentForm.FormClosing += ParentForm_FormClosing;

                if (File.Exists(Global.SequencePath))
                { 
                    SequenceModel squence = XmlControl.sequenceModelNew;
                    Camera3DSetModel cameraSet = squence.Camera3DSet;
                    if (cameraSet != null)
                    {
                        txtIP.sText = cameraSet.IPAddress;
                        txtPort.sText = cameraSet.Port.ToString();
                        txtProfile.sText = cameraSet.Profile.ToString();
                        numExposureTime.sText = cameraSet.ExposureTime.ToString();
                        trackBarExposure.Value = Int32.Parse(numExposureTime.sText);
                        numBrightness.sText = cameraSet.Brightness.ToString();
                        trackBarBrightness.Value = Int32.Parse(numBrightness.sText);
                        numGain.sText = cameraSet.Gain.ToString();
                        trackbarGain.Value = Int32.Parse(numGain.sText);
                        chkEnableGain.Checked = cameraSet.EnableGain;
                        numXScale.sText = cameraSet.XScale.ToString();
                        numYScale.sText = cameraSet.YScale.ToString();
                        numXYResolution.sText = cameraSet.XYResolution.ToString();
                        numZResolution.sText = cameraSet.ZResolution.ToString();
                        numLaserThreshold.sText = cameraSet.LaserThreshold.ToString();
                    }
                }
            }
            catch (Exception ex)
            { 

            }            
        }

        private void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                DisConnect();
            }
            catch (Exception ex)
            {
                 
            }
        }

        //保存参数
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SequenceModel sequence = new SequenceModel();
                if (!File.Exists(Global.SequencePath)) 
                { 
                    XmlControl.SetObject();
                    XmlControl.SaveToXml(Global.SequencePath, sequence, sequence.GetType());
                }
                else
                {
                    sequence = XmlControl.sequenceModelNew;
                } 

                sequence.Camera3DSet.IPAddress = txtIP.sText;
                sequence.Camera3DSet.Port = Int32.Parse(txtPort.sText);
                sequence.Camera3DSet.Profile = Int32.Parse(txtProfile.sText);
                sequence.Camera3DSet.ExposureTime = Int32.Parse(numExposureTime.sText);
                sequence.Camera3DSet.Brightness = Int32.Parse(numBrightness.sText);
                sequence.Camera3DSet.Gain = Int32.Parse(numGain.sText);
                sequence.Camera3DSet.EnableGain = chkEnableGain.Checked;
                sequence.Camera3DSet.XScale = double.Parse(numXScale.sText);
                sequence.Camera3DSet.YScale = double.Parse(numYScale.sText);
                sequence.Camera3DSet.XYResolution = double.Parse(numXYResolution.sText);
                sequence.Camera3DSet.ZResolution = double.Parse(numZResolution.sText);
                sequence.Camera3DSet.LaserThreshold = Int32.Parse(numLaserThreshold.sText);

                Global.XYResolution = sequence.Camera3DSet.XYResolution;
                Global.ZResolution = sequence.Camera3DSet.ZResolution;

                //XmlControl.SaveToXml(Global.SequencePath, sequence, sequence.GetType());

                MessageBoxEx.Show("保存到配置文件成功");
            }
            catch (Exception ex)
            { 

            }
          
        }

        private void trackBarExposure_Scroll(object sender, EventArgs e)
        {
            try
            {
                numExposureTime.sText = trackBarExposure.Value.ToString();
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void trackBarBrightness_Scroll(object sender, EventArgs e)
        {
            try
            { 
                numBrightness.sText = trackBarBrightness.Value.ToString();
            }
            catch (Exception ex)
            {

            }
        }

        private void trackbarGain_Scroll(object sender, EventArgs e)
        {
            try
            {
                numGain.sText = trackbarGain.Value.ToString(); 
            }
            catch (Exception ex)
            {

            }
        }

        private void btnLiveImage_Click(object sender, EventArgs e)
        {
            try
            {
                if(btnLiveImage.Text == "Live On")
                {
                    btnLiveImage.Text = "Live Off";
                    btnLiveImage.BackColor = Color.White;
                    m_bLiveImage = true;
                    Thread t = new Thread(new ThreadStart(() =>
                    {
                        LiveImageSample(); 
                    }));
                    t.Start();
                }
                else
                {
                    btnLiveImage.Text = "Live On";
                    btnLiveImage.BackColor = Color.LightGray;
                    m_bLiveImage = false; 
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        Sensor sensor = null;
        SensorManager sensorManager = null;
        bool m_bLiveImage = false;
        public void LiveImageSample()
        {
            //Register Acquisition One image Complete Event
            sensor.AcqusitionCompletedEvent += new Sensor.delegateAcqusitionCompleted(OnCompletedAcqEvent);
            m_bCompleted = true;
            while (m_bLiveImage)
            {
                if(!m_bCompleted)
                {
                    Thread.Sleep(100);
                    continue;
                }
                m_bCompleted = false;
                SetParam(); 
                sensor.StartAcquisition();
                sensor.WaitForImage(1);
                Thread.Sleep(50);
            }
            //sensor.Disconnect(); 
        }

        public void Connect()
        {
            if(sensor == null || sensor.GetSensorState().SensorConnection != SensorState.ConnectionState.Connected)
            {
                sensorManager = new SensorManager();
                sensor = sensorManager.CreateSensor("sensor 0");
                sensor.Connect();
                sensor.LoadParameterSet(Sensor.ParameterSet.LiveImage);
                sensor.SendParameterSet();
            } 
        }

        public void DisConnect()
        {
            m_bLiveImage = false;
            if (sensor != null && sensor.GetSensorState().SensorConnection == SensorState.ConnectionState.Connected)
            {
                sensor.StopAcquisition();
                sensor.Disconnect();
            }

            if(sensorManager != null)
            {
                sensorManager.Dispose();
                sensorManager = null;
            }
        }

        bool m_bCompleted = false;
        private void OnCompletedAcqEvent(object Sender, EventArgs e)
        {
            try
            {
                SensorImageData imageDatas = sensor.GetLastImageData();

                if (imageDatas != null)
                {
                    byte[] dataArray = imageDatas.LiveImage.GetImage();

                    if (dataArray != null)
                    {
                        HImage b = ConvertuShortToHimage(dataArray, imageDatas.Width, dataArray.Length / imageDatas.Width);
                        if (b != null)
                        {
                            m_hSmartWindow.DispImage(b);
                        }
                    }
                }

                //imageDatas.SaveLiveImage("LiveImageSample_LiveImage");

                sensor.StopAcquisition();
                m_bCompleted = true;
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
            
        }

        private HImage ConvertuShortToHimage(byte[] datas, int ImageWidth, int ImageHeight)
        {
            HImage hImage = new HImage();
            try
            {
                unsafe
                {
                    fixed (byte* m = datas)
                    {
                        hImage.GenImage1("byte", ImageWidth, ImageHeight, new IntPtr(m)); 
                    }
                }
                GC.Collect();
            }
            catch (Exception ce)
            {
                hImage = null; 
            }
            return hImage;
        }
        
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnConnect.Text == "Connect")
                {
                    Connect();
                    btnConnect.Text = "DisConnect";
                    btnConnect.BackColor = Color.PaleGreen;
                    btnLiveImage.Enabled = true;
                }
                else
                {
                    DisConnect();
                    btnConnect.Text = "Connect";
                    btnConnect.BackColor = Color.LightCoral;
                    btnLiveImage.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message);
            }
        }

        private void NumExposureTime_TxtValueEvent(object sender, string e)
        {
            try
            {
                trackBarExposure.Value = Int32.Parse(e);
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void NumBrightness_TxtValueEvent(object sender, string e)
        {
            try
            {
                trackBarBrightness.Value = Int32.Parse(e);
            }
            catch (Exception ex)
            {

            }
        }

        private void NumGain_TxtValueEvent(object sender, string e)
        {
            try
            {
                trackbarGain.Value = Int32.Parse(e);
            }
            catch (Exception ex)
            {

            }
        }
         
        private void chkEnableGain_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (sensor == null)
                {
                    return;
                }
                //Api.SetGain(sensor._sensorObject, chkEnableGain.Checked, (int)numGain.Value);

            }
            catch (Exception ex)
            {

            }
        }
        
        private void SetParam()
        {
            if (sensor == null)
            {
                return;
            }

            Api.SetExposureTime(sensor._sensorObject, 0, Int32.Parse(numExposureTime.sText));
            Api.SetLaserBrightness(sensor._sensorObject, Int32.Parse(numBrightness.sText));
            Api.SetGain(sensor._sensorObject, chkEnableGain.Checked, Int32.Parse(numGain.sText));


        }
    }
}
