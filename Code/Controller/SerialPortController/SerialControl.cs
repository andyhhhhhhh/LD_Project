using BaseController;
using BaseModels;
using GlobalCore;
using Infrastructure.DBCore;
using SequenceTestModel;
using SerialPortModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XMLController;

namespace SerialPortController
{
    /// <summary>
    /// 串口Control方法
    /// </summary>   
    public class SerialControl
    {
        private const string ByEnd = "\r";
        private const string SpiltChar = "";
        private static List<SerialPort> m_SerialPortLst = new List<SerialPort>();
        private static Queue<string> RecevieCollection = new Queue<string>();

        private static SerialPort m_SerialPort = null;
        private static SerialControl m_SerialControl = null;
        private static bool m_IsInit = false;
        private bool m_IsFormatRec = false;
        //public bool ReceiveAndClear = false;
        private static string m_Rest = "";
        private static bool m_IsStatus = false;

        public bool Init(object parameter)
        {
            if (m_IsInit)
            {
                return true;
            }
            try
            {
                var comModel = parameter as ComModel;
                if (comModel == null)
                {
                    throw new ArgumentNullException("传入的控制参数不能为空");
                }

                //握手成功 
                SerialControl serialPortControl = GetInstance();
                serialPortControl.Open(comModel);
                return true;
            }
            catch (Exception ex)
            {
                //throw ex;
                return false;
            }
        }

        public BaseResultModel Run(BaseSeqModel controlModel, ControlType controlType)
        {
            SerialPortResultModel resultModel = new SerialPortResultModel();
            resultModel.RunResult = true;
            try
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
                var control = GetInstance();
                switch (controlType)
                {
                    case ControlType.SerialPortSend:
                        Open(m_SerialPortModel);
                        Send(m_SerialPortModel);
                        Thread.Sleep(10);
                        break;
                    case ControlType.SerialPortReceive:
                        m_IsStatus = false;
                        Open(m_SerialPortModel);
                        resultModel.ReceiveMessage = Receive();
                        resultModel.ObjectResult = resultModel.ReceiveMessage;
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
            catch (Exception ex)
            {
                resultModel.ErrorMessage = ex.Message;
                resultModel.RunResult = false;
                return resultModel; 
            }
        }

        #region COM 通信
        public SerialControl()
        {
        }
        public static SerialControl GetInstance()
        {
            if (m_SerialControl == null)
            {
                m_SerialControl = new SerialControl();
            }
            return m_SerialControl;
        }

        public static SerialPort GetSerialPort()
        {
            return m_SerialPort;
        }

        protected void Open(ComModel comModel)
        {
            SerialPort serialPort = GetPortInstance(comModel);
            if (serialPort == null)
            {
                throw new ArgumentNullException("串口为空，初始化失败");
            }
            if (IsOpen(comModel))
                return;
            serialPort.ReadTimeout = comModel.TimeOut;
            serialPort.WriteTimeout = comModel.TimeOut;

            //打开串口后开启接收事件
            serialPort.DataReceived += SerialPort_DataReceived;
            serialPort.Open();
            comModel.IsConnected = true;
        }

        protected bool IsOpen(ComModel comModel)
        {
            SerialPort serialPort = GetPortInstance(comModel);
            return serialPort.IsOpen;
        }

        protected void Close(ComModel comModel)
        {
            SerialPort serialPort = GetPortInstance(comModel);
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

                comModel.IsConnected = false;
            }
        }

        protected void Send(ComModel comModel)
        {
            SerialPort serialPort = GetPortInstance(comModel);
            if (!serialPort.IsOpen)
            {
                throw new InvalidOperationException("串口" + serialPort.PortName + "未打开！");
            }
            else
            {
                if (comModel.IsHex)
                {
                    var strHex = CommConvert.GetHexStr(comModel.SendContent + ByEnd);
                    //byte[] bytes = Encoding.Default.GetBytes(strHex);  
                    byte[] bytes = CommConvert.strToHexByte(strHex);
                    serialPort.Write(bytes, 0, bytes.Length);



                    //string input = comModel.SendContent;
                    //string str = HexStringToString(input, Encoding.Default);

                    //serialPort.Write(str);
                }
                else
                {
                    serialPort.Write(comModel.SendContent + (comModel.IsByEnd ? ByEnd : ""));
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

        protected string SendAndReceive(ComModel comModel)
        {
            SerialPort serialPort = GetPortInstance(comModel);
            if (!serialPort.IsOpen)
            {
                throw new InvalidOperationException("串口" + serialPort.PortName + "未打开！");
            }
            else
            {
                Send(comModel);
                string strReceived = serialPort.ReadTo((comModel.IsByEnd ? ByEnd : ""));
                return strReceived;
            }
        }

        protected string Receive()
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
                if (sp.ElapsedMilliseconds > 1000)
                {
                    //recevieMessage = "Time Out";
                    break;
                }
                Thread.Sleep(100);
                if (RecevieCollection.Count > 0)
                {
                    recevieMessage = RecevieCollection.Dequeue();
                    return recevieMessage;
                }
            }
            return recevieMessage;
        }

        public void ClearQueue()
        {
            try
            {
                RecevieCollection.Clear();
            }
            catch (Exception ex)
            {
                 
            }
        }

        #endregion

        #region 私有函数 

        private SerialPort GetPortInstance(ComModel comModel)
        {
            try
            {
                if (comModel == null)
                {
                    throw new ArgumentNullException("输入参数ComModel为空");
                }
                if (m_SerialPortLst.Any(x => x.PortName == comModel.ComPortName))
                {
                    m_SerialPort = m_SerialPortLst.FirstOrDefault(x => x.PortName == comModel.ComPortName);
                }
                else
                {
                    m_SerialPort = new SerialPort(comModel.ComPortName, comModel.BaudRate, comModel.Parity, comModel.DataBits, comModel.StopBits);
                    if (comModel.TimeOut > 0 && comModel.TimeOut < 1000000)
                    {
                        m_SerialPort.WriteTimeout = comModel.TimeOut;
                        m_SerialPort.ReadTimeout = comModel.TimeOut;
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
                    Thread.Sleep(100);
                    int count = serialPort.Read(buffer, 0, buffer.Length);
                    if (count > 0)
                    {
                        indata = Encoding.ASCII.GetString(buffer);
                        if (m_IsStatus)
                        {
                            //add 20180122
                            indata = "";
                            for (int i = 0; i < count; i++)
                            {
                                indata += ("0x" + buffer[i].ToString("X2") + " ");
                            }
                        }
                    }

                    //indata = serialPort.ReadExisting();
                }
                RecevieCollection.Clear();
                if(string.IsNullOrEmpty(indata))
                {
                    return;
                }
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
                Thread.Sleep(200);
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
