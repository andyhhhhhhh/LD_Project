using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using BaseController;
using BaseModels;
using System.Diagnostics;
using SerialPortModel;
using GlobalCore;
using Infrastructure.DBCore;
using SequenceTestModel;

namespace SerialPortController
{
    public class SerialPortControl
    {
        // private const string ByEnd = "\r\n";
        private const string ByEnd = "\r";
        private const string SpiltChar = "";
        private List<SerialPort> m_SerialPortLst = new List<SerialPort>();
        private Queue<string> RecevieCollection = new Queue<string>();

        private SerialPort m_SerialPort = null;
        private SerialPortControl m_SerialPortControl = null;
        private bool m_IsInit = false;
        private bool m_IsFormatRec = false;
        //public bool ReceiveAndClear = false;
        private string m_Rest = "";
        private bool m_IsStatus = false;

        public SerialPortControl()
        {

        }

        //public static SerialPortControl GetInstance()
        //{
        //    if (m_SerialPortControl == null)
        //    {
        //        m_SerialPortControl = new SerialPortControl();
        //    }
        //    return m_SerialPortControl;
        //}

        public void SetFormatRec(bool bTrue)
        {
            m_IsFormatRec = bTrue;
        }

        public bool Init(object parameter)
        {
            if (m_IsInit)
            {
                return true;
            }
            try
            {
                var ComModel = parameter as ComModel;
                if (ComModel == null)
                {
                    //throw new ArgumentNullException("传入的控制参数不能为空");
                    return false;
                }

                //握手成功 
                //SerialPortControl serialPortControl = new SerialPortControl();

                if (!Open(ComModel))
                {
                    return false;
                }


                return true;
            }
            catch (Exception ex)
            {
                //throw ex;
                return false;
            }
        }

        public BaseResultModel Run(ComModel controlModel, ControlType controlType)
        {
            if (controlModel == null)
            {
                throw new ArgumentNullException("传入的控制参数为空");
            }
            ComModel m_SerialPortModel = controlModel as ComModel;
            if (m_SerialPortModel == null)
            {
                throw new ArgumentException("SerialPortModel参数为空！或类型不匹配！");
            }
            SerialPortResultModel resultModel = new SerialPortResultModel();
            resultModel.RunResult = true;
            //var control = GetInstance();
            switch (controlType)
            {
                case ControlType.SerialPortSend:
                    Open(m_SerialPortModel);
                    Send(m_SerialPortModel);
                    Thread.Sleep(20);
                    break;
                case ControlType.SerialPortReceive:
                    m_IsStatus = false;
                    Open(m_SerialPortModel);
                    resultModel.ReceiveMessage = Receive();
                    resultModel.ObjectResult = resultModel.ReceiveMessage;
                    resultModel.RunResult = resultModel.ReceiveMessage != "Time Out";
                    break;
                case ControlType.SerialPortSendAndReceive:
                    Open(m_SerialPortModel);
                    resultModel.ReceiveMessage = SendAndReceive(m_SerialPortModel);
                    break;
                case ControlType.SerialPortReceiveStatus:
                    m_IsStatus = true;
                    Open(m_SerialPortModel);
                    resultModel.ReceiveMessage = Receive();
                    break;
                case ControlType.SerialPortOpen:
                    Open(m_SerialPortModel);
                    break;
                case ControlType.SerialPortClose:
                    Close(m_SerialPortModel);
                    break;
                default:
                    throw new InvalidOperationException("无效的操作类型:" + controlType.ToString() + "，不适用于串口操作");
            }
            return resultModel;
        }

        #region COM 通信


        public SerialPort GetSerialPort()
        {
            return m_SerialPort;
        }

        protected bool Open(ComModel serialPortModel)
        {
            SerialPort serialPort = GetPortInstance(serialPortModel);
            if (serialPort == null)
            {
                //throw new ArgumentNullException("串口为空，初始化失败");
                return false;
            }
            if (IsOpen(serialPortModel))
            {
                return false;
            }

            serialPort.ReadTimeout = serialPortModel.TimeOut;
            serialPort.WriteTimeout = serialPortModel.TimeOut;

            //打开串口后开启接收事件            
            serialPort.DataReceived += SerialPort_DataReceived;
            serialPort.Open();

            return true;
        }

        protected bool IsOpen(ComModel serialPortModel)
        {
            SerialPort serialPort = GetPortInstance(serialPortModel);
            return serialPort.IsOpen;
        }

        protected void Close(ComModel serialPortModel)
        {
            SerialPort serialPort = GetPortInstance(serialPortModel);
            if (null != serialPort)
            {
                m_SerialPortLst.Remove(serialPort);
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }
                //释放串口的事件资源
                serialPort.DataReceived -= SerialPort_DataReceived;
                serialPort.Dispose();
                serialPort = null;
            }
        }
        protected void Send(ComModel serialPortModel)
        {
            SerialPort serialPort = GetPortInstance(serialPortModel);
            if (!serialPort.IsOpen)
            {
                throw new InvalidOperationException("串口" + serialPort.PortName + "未打开！");
            }
            else
            {
                if (serialPortModel.IsHex)
                {
                    //var bytes = CommConvert.GetHexBytes(serialPortModel.SendContent); 
                    //byte[] btes = Encoding.Default.GetBytes(serialPortModel.SendContent); 
                    string input = serialPortModel.SendContent;
                    string str = HexStringToString(input, Encoding.Default);

                    serialPort.Write(str);
                }
                else
                {
                    serialPort.Write(serialPortModel.SendContent + "\r");
                }
            }
        }
        private string HexStringToString(string hs, Encoding encode)
        {
            string strTemp = "";
            byte[] b = new byte[hs.Length / 2];
            for (int i = 0; i < hs.Length / 2; i++)
            {
                strTemp = hs.Substring(i * 2, 2);
                b[i] = Convert.ToByte(strTemp, 16);
            }
            //按照指定编码将字节数组变为字符串
            return encode.GetString(b);
        }

        public string SendAndReceive(ComModel serialPortModel)
        {
            SerialPort serialPort = GetPortInstance(serialPortModel);
            if (!serialPort.IsOpen)
            {
                throw new InvalidOperationException("串口" + serialPort.PortName + "未打开！");
            }
            else
            {
                Send(serialPortModel);
                string strReceived = serialPort.ReadTo(ByEnd);
                return strReceived;
            }
        }

        public string Receive()
        {
            string recevieMessage = string.Empty;
            Stopwatch sp = new Stopwatch();
            sp.Start();
            while (true)
            {
                if (Global.Break)
                {
                    return "";
                }
                if (sp.ElapsedMilliseconds > 5000)
                {
                    recevieMessage = "Time Out";
                    break;
                }
                Thread.Sleep(10);
                if (RecevieCollection.Count > 0)
                {
                    recevieMessage = RecevieCollection.Dequeue();
                    return recevieMessage;
                }
            }
            return recevieMessage;
        }

        #endregion

        #region 私有函数 

        private SerialPort GetPortInstance(ComModel serialPortModel)
        {
            try
            {
                if (serialPortModel == null)
                {
                    throw new ArgumentNullException("输入参数ComModel为空");
                }
                if (m_SerialPortLst.Any(x => x.PortName == serialPortModel.ComPortName))
                {
                    m_SerialPort = m_SerialPortLst.FirstOrDefault(x => x.PortName == serialPortModel.ComPortName);
                }
                else
                {
                    m_SerialPort = new SerialPort(serialPortModel.ComPortName, serialPortModel.BaudRate, serialPortModel.Parity, serialPortModel.DataBits, serialPortModel.StopBits);
                    if (serialPortModel.TimeOut > 0 && serialPortModel.TimeOut < 1000000)
                    {
                        m_SerialPort.WriteTimeout = serialPortModel.TimeOut;
                        m_SerialPort.ReadTimeout = serialPortModel.TimeOut;
                    }
                    m_SerialPortLst.Add(m_SerialPort);
                }
            }
            catch (Exception ex)
            {
                m_SerialPort = null;
                throw ex;
            }
            return m_SerialPort;
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort serialPort = sender as SerialPort;
                byte[] buffer = new byte[1024];
                string indata = "";
                if (m_IsFormatRec)
                {
                    //indata = serialPort.ReadLine(); 
                    //以'/r'为截止符
                    indata = GetReceiveWithCR(serialPort, buffer, indata);
                }
                else
                {
                    //int count = serialPort.Read(buffer, 0, buffer.Length);
                    //if (count > 0)
                    //{
                    //indata = Encoding.ASCII.GetString(buffer);
                    //if(m_IsStatus)
                    //{
                    //    //add 20180122
                    //    indata = "";
                    //    for (int i = 0; i < count; i++)
                    //    {
                    //        indata += ("0x" + buffer[i].ToString("X2") + " ");
                    //    }
                    //} 
                    //}

                    indata = serialPort.ReadExisting();
                }
                //RecevieCollection.Clear();
                RecevieCollection.Enqueue(indata);
            }
            catch (TimeoutException timeEx)
            {
                //throw timeEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetReceiveWithCR(SerialPort serialPort, byte[] buffer, string indata)
        {
            Stopwatch sp = new Stopwatch();
            sp.Start();
            while (true)
            {
                if (sp.ElapsedMilliseconds > serialPort.ReadTimeout)
                {
                    sp.Stop();
                    return "";
                }
                Thread.Sleep(20);
                string temp = serialPort.ReadExisting();
                if (temp.Contains('\r'))
                {
                    indata = temp;
                    break;
                }
            }
            //截断第一个'\r'，包含截止符的存入队列，剩余部分存到m_Rest中
            int index = indata.IndexOf('\r');
            int totalLength = indata.Length;
            string originalString = indata;
            indata = indata.Substring(0, index);
            //如果m_Rest不为空要把前端补齐到下一次接收的内容中
            if (!string.IsNullOrEmpty(m_Rest))
            {
                //不包含截止符
                if (!m_Rest.Contains('\r'))
                {
                    indata = m_Rest + indata;
                    m_Rest = "";
                }
                else if (m_Rest.Equals("\r"))
                {
                    m_Rest = "";
                }
                else
                {
                    //包含截止符后半段不能清空
                    int restIndex = m_Rest.IndexOf('\r');
                    string restIndata = m_Rest.Substring(0, restIndex);
                    indata = m_Rest + restIndata;
                    //剩余部分再存到m_Rest中
                    m_Rest = m_Rest.Substring(restIndex, m_Rest.Length - restIndex);
                }
            }
            else
            {
                if (totalLength > index + 1)
                {
                    m_Rest = originalString.Substring(index, originalString.Length - index).TrimStart('\r');
                    m_Rest = m_Rest.TrimStart('\n');
                }
            }

            return indata;
        }

        #endregion
    }
}
