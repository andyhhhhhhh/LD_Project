using BaseController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BaseModels;
using SocketModel;
using System.Diagnostics;
using Infrastructure.Log;
using GlobalCore;
using Infrastructure.DBCore;
using SequenceTestModel;

namespace SocketController
{
    public class SocketControl
    {
        public Socket m_Socket;
        private Socket sListener;
        private Socket handler;

        public bool m_IsInit = false;

        private string EndOf = ""; //\r\n   结束字符

        //委托显示消息
        public delegate void GetAccpetMsg(string msg);
        public GetAccpetMsg m_delmsg;

        //单例模式
        public static SocketControl socketControl;
        public SocketControl GetInstance()
        {
            if (socketControl == null)
            {
                lock (this)
                {
                    socketControl = new SocketControl();
                }
            }

            return socketControl;
        }

        public bool Init(object parameter)
        {
            if (m_IsInit)
            {
                return true;
            }
            if (parameter == null)
            {
                throw new ArgumentNullException("传入的控制参数不能为空");
            }
            TCPIPModel socketParamModel = parameter as TCPIPModel;
            if (socketParamModel == null)
            {
                throw new ArgumentNullException("传入的控制参数不是SocketParam参数");
            }
            try
            {
                if (socketParamModel.IsService)
                {
                    if (sListener == null || handler == null)
                    {
                        StartService(socketParamModel);
                    }
                }
                else
                {
                    if (m_Socket == null)
                    {
                        m_Socket = Connect(socketParamModel);
                    }

                }

                m_IsInit = true;
                socketParamModel.IsConnected = true;
                return true;
            }
            catch (Exception ex)
            {
                socketParamModel.IsConnected = false;
                throw ex;
            }
        }

        public bool Dispose(object parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException("传入的控制参数不能为空");
            }
            try
            {
                if (m_Socket != null)
                {
                    m_Socket.Shutdown(SocketShutdown.Both);
                    m_Socket.Close();
                    m_Socket = null;
                    m_IsInit = false;
                }
                if (sListener != null)
                {
                    sListener.Close();
                    sListener = null;
                    m_IsInit = false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BaseResultModel Run(TCPIPModel controlModel, ControlType controlType)
        {
            BaseResultModel resultModel = new BaseResultModel();
            try
            {
                if (controlModel == null)
                {
                    throw new ArgumentNullException("传入的控制参数不能为空");
                }
                TCPIPModel socketParamModel = controlModel as TCPIPModel;
                if (socketParamModel == null)
                {
                    throw new ArgumentNullException("传入的控制参数" + controlModel.Name + "类型错误");
                }
                if (!socketParamModel.IsService && m_Socket == null)
                {
                    throw new ArgumentException("未对参数" + socketParamModel.Name + "进行初始化");
                }
                if (socketParamModel.IsService && sListener == null)
                {
                    throw new ArgumentException("未对参数" + socketParamModel.Name + "进行初始化");
                }
                switch (controlType)
                {
                    case ControlType.SocketSend:
                        if (socketParamModel.IsService)
                        {
                            ServiceSend(socketParamModel);
                        }
                        else
                        {
                            Send(socketParamModel, m_Socket);
                        }
                        break;
                    case ControlType.SocketReceive:
                        if (socketParamModel.IsService)
                        {
                            resultModel.ObjectResult = ReceiveDataFromClient(socketParamModel);
                        }
                        else
                        {
                            resultModel.ObjectResult = ReceiveDataFromServer(socketParamModel, m_Socket);
                        }
                        break;
                    default:
                        throw new InvalidOperationException("无效的操作类型:" + controlType.ToString() + "，不适用于网口操作");
                }
                resultModel.RunResult = true;
            }
            catch (Exception ex)
            {
                resultModel.RunResult = false;
                resultModel.ErrorMessage = ex.Message;
            }
            return resultModel;
        }

        #region 客户端私有方法
        private Socket Connect(TCPIPModel socketParamModel)
        {
            try
            {
                Socket senderSocket;
                IPAddress ip = IPAddress.Parse(socketParamModel.IPAddress);
                senderSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ipEndPoint = new IPEndPoint(ip, socketParamModel.PortNum);
                senderSocket.ReceiveTimeout = socketParamModel.TimeOut;
                senderSocket.Connect(ipEndPoint);
                return senderSocket;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Send(TCPIPModel socketParamModel, Socket socket)
        {
            try
            {
                byte[] msg = Encoding.ASCII.GetBytes(socketParamModel.SendContent);
                int bytesSend = socket.Send(msg);
                //Log.WriteLog(Log.CommunicationFileName, "Send：" + socketParamModel.SendContent);
            }
            catch (Exception ex)
            {
                m_IsInit = false;
                throw ex;
            }
        }

        private void Close(Socket socket)
        {
            try
            {
                if(socket != null)
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
            }
            catch (Exception ex)
            {
                m_IsInit = false;
                throw ex;
            }
        }

        public void Close()
        {
            try
            {
                if(m_Socket != null)
                { 
                    m_Socket.Shutdown(SocketShutdown.Both);
                    m_Socket.Close();

                    m_Socket = null;
                    m_IsInit = false;
                }
            }
            catch (Exception ex)
            {
                m_IsInit = false;
                throw ex;
            }
        }

        private string ReceiveDataFromServer(TCPIPModel socketParamModel, Socket socket)
        {
            try
            {
                byte[] bytes = new byte[2048];
                int bytesRec = socket.Receive(bytes);
                String theMessageToReceive = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                //Log.WriteLog(Log.CommunicationFileName, "Receive：" + theMessageToReceive);
                return theMessageToReceive;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 服务器私有方法

        private Queue<string> m_ClientReceiveQueue = new Queue<string>();

        private void StartService(TCPIPModel socketParamModel)
        {
            try
            {
                IPAddress ip = IPAddress.Parse(socketParamModel.IPAddress);
                sListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                int port = socketParamModel.PortNum;
                sListener.Bind(new IPEndPoint(ip, port));  //绑定IP地址：端口  
                sListener.Listen(10);    //设定最多10个排队连接请求   

                AsyncCallback aCallback = new AsyncCallback(AcceptCallback);
                sListener.BeginAccept(aCallback, sListener);

            }
            catch (Exception ex)
            {
                Log.WriteLog(LogLevel.Exception, ex.Message);
                throw ex;
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            Socket listener = null;

            //Socket handler = null;
            try
            {
                byte[] buffer = new byte[1024];
                listener = (Socket)ar.AsyncState;
                handler = listener.EndAccept(ar);

                handler.NoDelay = false;

                object[] obj = new object[2];
                obj[0] = buffer;
                obj[1] = handler;

                handler.BeginReceive(
                    buffer,
                    0,
                    buffer.Length,
                    SocketFlags.None,
                    new AsyncCallback(ReceiveCallback),
                    obj
                    );

                AsyncCallback aCallback = new AsyncCallback(AcceptCallback);
                listener.BeginAccept(aCallback, listener);

                m_delmsg("Accept:" + listener.LocalEndPoint);
            }
            catch (Exception exc)
            {
                Log.WriteLog(LogLevel.Exception, exc.Message);
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                object[] obj = new object[2];
                obj = (object[])ar.AsyncState;

                byte[] buffer = (byte[])obj[0];

                handler = (Socket)obj[1];

                string content = string.Empty;


                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    content += Encoding.ASCII.GetString(buffer, 0,
                        bytesRead);

                    //if (content.IndexOf("\r\n") > -1)
                    if (content.IndexOf(EndOf) > -1)
                    {
                        string str = content.Substring(0, content.LastIndexOf(EndOf) + 1);
                        m_ClientReceiveQueue.Enqueue(str);
                    }
                    //else
                    //{
                    byte[] buffernew = new byte[1024];
                    obj[0] = buffernew;
                    obj[1] = handler;
                    handler.BeginReceive(buffernew, 0, buffernew.Length,
                        SocketFlags.None,
                        new AsyncCallback(ReceiveCallback), obj);
                    //}
                }
            }
            catch (Exception exc)
            {
                Log.WriteLog(LogLevel.Exception, exc.Message);
            }
        }

        private void ServiceSend(TCPIPModel socketParamModel)
        {
            try
            {
                string message = socketParamModel.SendContent;
                if (!message.Contains("\r\n"))
                {
                    // message += "\r\n";
                }
                byte[] byteData = Encoding.ASCII.GetBytes(message);

                handler.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), handler);
            }
            catch (Exception exc)
            {
                Log.WriteLog(LogLevel.Exception, exc.Message);
            }
        }

        private string ReceiveDataFromClient(TCPIPModel socketParamModel)
        {
            Stopwatch sp = new Stopwatch();
            sp.Start();
            while (true)
            {
                if (Global.Break)
                    return "";
                if (sp.ElapsedMilliseconds > socketParamModel.TimeOut)
                    return "";
                if (m_ClientReceiveQueue.Count > 0)
                    return m_ClientReceiveQueue.Dequeue();
            }
        }

        public void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;

                int bytesSend = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to Client", bytesSend);
            }
            catch (Exception exc)
            {
                Log.WriteLog(LogLevel.Exception, exc.Message);
            }
        }

        #endregion
    }
}
