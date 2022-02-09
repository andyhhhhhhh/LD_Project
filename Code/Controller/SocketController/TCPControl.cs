using CSockets;
using Infrastructure.Log;
using SequenceTestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XMLController;

namespace SocketController
{
    public class TCPControl
    {
        object m_lock = new object();

        public delegate void Del_OutPutLog(string log, LogLevel loglevel = LogLevel.Info);
        /// <summary>
        /// 委托 -- 主界面输出Log信息
        /// </summary>
        public static Del_OutPutLog m_DelOutPutLog;

        public delegate void Del_OutExLog(Exception ex);
        /// <summary>
        /// 委托 -- 主界面输出Exception信息
        /// </summary>
        public static Del_OutExLog m_DelOutExLog;

        private List<System.Net.Sockets.Socket> SocketClientList = new List<System.Net.Sockets.Socket>();
        private CServerSocket[] m_SocketServerControl;
        private CClientSocket[] m_SocketClientControl;
        private List<string>[] m_CommReceivedArray;
        public bool InitialCommunication(TCPIPModel m_TcpipModel)
        {
            if (m_SocketServerControl == null)
            {
                m_SocketServerControl = new CServerSocket[6];
                InitialCommunicationParam(6);
            }

            if (m_SocketClientControl == null)
            {
                m_SocketClientControl = new CClientSocket[6];
                InitialCommunicationParam(6);
            }

            lock (m_lock)
            {
                string ipAddress;
                string strPort;

                #region 打开服务器  

                if (m_TcpipModel != null && m_TcpipModel.IsService)
                {
                    ipAddress = m_TcpipModel.IPAddress;
                    strPort = m_TcpipModel.PortNum.ToString();
                    if (m_SocketServerControl[m_TcpipModel.Id] != null)
                    {
                        return true;
                    }
                    try
                    {
                        IPAddress ip;
                        int port;
                        if (IPAddress.TryParse(ipAddress, out ip))
                        {
                            if (int.TryParse(strPort, out port) && (port < 65536))
                            {
                                m_SocketServerControl[m_TcpipModel.Id] = new CServerSocket(port);
                                if (!m_SocketServerControl[m_TcpipModel.Id].Active())
                                {
                                    return false;
                                }
                                m_SocketServerControl[m_TcpipModel.Id].OnConnect += new CServerSocket.ConnectionDelegate(Comuncation_OnConnect);
                                m_SocketServerControl[m_TcpipModel.Id].OnDisconnect += new CServerSocket.ConnectionDelegate(Comuncation_OnDisconnect);
                                m_SocketServerControl[m_TcpipModel.Id].OnError += new CServerSocket.ErrorDelegate(Comuncation_OnError);
                                m_SocketServerControl[m_TcpipModel.Id].OnRead += new CServerSocket.ConnectionDelegate(Comuncation_OnRead);
                                m_SocketServerControl[m_TcpipModel.Id].OnWrite += new CServerSocket.ConnectionDelegate(Comuncation_OnWrite);

                                return true;
                            }
                            else
                            {
                                throw new InvalidOperationException("端口开启不成功");
                            }
                        }
                        else
                        {
                            throw new InvalidOperationException("IP地址不正确");
                        }
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }

                #endregion

                #region 打开客户端 

                if (m_TcpipModel != null && !m_TcpipModel.IsService)
                {
                    ipAddress = m_TcpipModel.IPAddress;
                    strPort = m_TcpipModel.PortNum.ToString();
                    if (m_SocketClientControl[m_TcpipModel.Id] != null && m_SocketClientControl[m_TcpipModel.Id].Connected)
                    {
                        return true;
                    }
                    try
                    {
                        IPAddress ip;
                        int port;
                        if (IPAddress.TryParse(ipAddress, out ip))
                        {
                            if (int.TryParse(strPort, out port) && (port < 65536))
                            {
                                m_SocketClientControl[m_TcpipModel.Id] = new CClientSocket(ipAddress, port);
                                if (m_SocketClientControl[m_TcpipModel.Id].Connect())
                                {
                                    m_TcpipModel.IsConnected = true;
                                }
                                //if (!m_SocketClientControl.Active())
                                //{
                                //    return false;
                                //}
                                m_SocketClientControl[m_TcpipModel.Id].OnConnect += M_SocketClientControl_OnConnect;
                                m_SocketClientControl[m_TcpipModel.Id].OnDisconnect += M_SocketClientControl_OnDisconnect;
                                m_SocketClientControl[m_TcpipModel.Id].OnError += M_SocketClientControl_OnError;
                                m_SocketClientControl[m_TcpipModel.Id].OnRead += M_SocketClientControl_OnRead;
                                m_SocketClientControl[m_TcpipModel.Id].OnWrite += M_SocketClientControl_OnWrite;

                                return true;
                            }
                            else
                            {
                                throw new InvalidOperationException("端口开启不成功");
                            }
                        }
                        else
                        {
                            throw new InvalidOperationException("IP地址不正确");
                        }
                    }
                    catch (System.Exception ex)
                    {
                        throw new InvalidOperationException(ex.ToString());
                    }
                }

                #endregion

                throw new InvalidOperationException("没有任何设置为启用的服务器或客户端");
            }
        }

        private void InitialCommunicationParam(int count)
        {
            m_CommReceivedArray = new List<string>[6];
            for (int i = 0; i < count; i++)
            {
                m_CommReceivedArray[i] = new List<string>();
                //m_CommReceivedArray[i] = "";
            }
            //m_IsComunicationReceived = false;
            //m_AllCommunicationModelList = m_CommunicationParamService.QueryAll();
        }

        private void CommunicationSetCompleted(string commString, int index = 0)
        {
            m_CommReceivedArray[index].Add(commString.Trim('\0').Trim());
            //m_IsComunicationReceived = true;
        }

        private void CommunicationReset(int index, string str)
        {
            m_CommReceivedArray[index].Remove(str);
            //m_IsComunicationReceived = false;
        }

        private void TcpIPSend(TCPIPModel tcpipModel, string content, int clientIndex, int index = 0)
        {
            try
            {
                if (tcpipModel.IsService)
                {
                    WriteCommunicationToClient(tcpipModel, content, clientIndex, index);
                }
                else
                {
                    WriteCommunicationToServer(content, index);
                }

                m_DelOutPutLog("[" + tcpipModel.Name + "]发送消息:" + content, LogLevel.Debug);
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
            }
        }

        private string TcpListen(TCPIPModel tcpipModel)
        {
            try
            {
                string str = m_CommReceivedArray[tcpipModel.Id].Last();
                CommunicationReset(tcpipModel.Id, str);
                return str;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private void CloseTcp(TCPIPModel tcpipModel)
        {
            try
            {
                if (tcpipModel.IsService)
                {
                    for (int i = 0; i < SocketClientList.Count; i++)
                    {
                        m_SocketServerControl[tcpipModel.Id].CloseConnection(i);
                    }
                }
                else
                {
                    m_SocketClientControl[tcpipModel.Id].Disconnect();
                }
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
            }
        }

        #region 客户端控制

        private void M_SocketClientControl_OnWrite(System.Net.Sockets.Socket soc)
        {
        }

        private void WriteCommunicationToServer(string content, int index = 0)
        {
            try
            {
                m_SocketClientControl[index].SendText(content);
                //m_DelOutPutLog("发送消息：" + content);
            }
            catch (Exception ex)
            {
                m_SocketClientControl = null;
            }
        }

        private void M_SocketClientControl_OnRead(System.Net.Sockets.Socket soc)
        {
            try
            {
                string[] strIp = soc.RemoteEndPoint.ToString().Split(':');
                var tcpipModel = XmlControl.sequenceModelNew.TCPIPModels.FirstOrDefault(x => x.IPAddress == strIp[0] && x.PortNum.ToString() == strIp[1]);

                if (tcpipModel != null)
                {
                    string commString = m_SocketClientControl[tcpipModel.Id].ReceivedText;
                    if (commString != null)
                    {
                        CommunicationSetCompleted(commString, tcpipModel.Id);
                    }
                    m_DelOutPutLog("[" + tcpipModel.Name + "]收到消息：" + commString.Trim('\0').Trim(), LogLevel.Debug);
                }
                else
                {
                    m_DelOutPutLog(string.Format("未找到IP:{0} Port:{1}的信息", strIp[0], strIp[1]), LogLevel.Error);
                }
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
            }
        }

        private void M_SocketClientControl_OnError(string ErroMessage, System.Net.Sockets.Socket soc, int ErroCode)
        {
            m_DelOutPutLog("连接错误：" + ErroCode + "," + ErroMessage);
        }

        private void M_SocketClientControl_OnDisconnect(System.Net.Sockets.Socket soc)
        {
            try
            {
                m_DelOutPutLog("断开连接："/*+ soc.AddressFamily.ToString()*/ + soc.RemoteEndPoint.ToString());

                string[] strIp = soc.RemoteEndPoint.ToString().Split(':');
                var tcpipModel = XmlControl.sequenceModelNew.TCPIPModels.FirstOrDefault(x => x.IPAddress == strIp[0] && x.PortNum.ToString() == strIp[1]);

                tcpipModel.IsConnected = false;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
            }
        }

        private void M_SocketClientControl_OnConnect(System.Net.Sockets.Socket soc)
        {
            try
            {
                m_DelOutPutLog("连接成功：" /*+ soc.AddressFamily.ToString()*/ + soc.RemoteEndPoint.ToString());

                string[] strIp = soc.RemoteEndPoint.ToString().Split(':');
                var tcpipModel = XmlControl.sequenceModelNew.TCPIPModels.FirstOrDefault(x => x.IPAddress == strIp[0] && x.PortNum.ToString() == strIp[1]);

                tcpipModel.IsConnected = true;
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
            }
        }

        #endregion 

        #region 服务器控制

        private void Comuncation_OnRead(System.Net.Sockets.Socket soc)
        {
            try
            {
                //只接收一次信息，直到信息被处理完成
                //if (!m_IsComunicationReceived)
                //{
                string[] strIp = soc.LocalEndPoint.ToString().Split(':');
                var tcpipModel = XmlControl.sequenceModelNew.TCPIPModels.FirstOrDefault(x => x.IPAddress == strIp[0] && x.PortNum.ToString() == strIp[1]);

                if (tcpipModel != null)
                {
                    string commString = m_SocketServerControl[tcpipModel.Id].ReceivedText;
                    if (commString != null)
                    {
                        CommunicationSetCompleted(commString, tcpipModel.Id);
                    }
                    m_DelOutPutLog("[" + tcpipModel.Name + "]收到消息：" + commString.Trim('\0').Trim(), LogLevel.Debug);
                }
                else
                {
                    m_DelOutPutLog(string.Format("未找到IP:{0} Port:{1}的信息", strIp[0], strIp[1]), LogLevel.Error);
                }
                //}
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
            }
        }
        private void Comuncation_OnWrite(System.Net.Sockets.Socket soc)
        {
        }
        private void Comuncation_OnConnect(System.Net.Sockets.Socket soc)
        {
            try
            {
                //if (SocketClientList.Count >= 1)
                //{
                //    SocketClientList.RemoveAt(0);
                //}
                SocketClientList.Add(soc);
                m_DelOutPutLog("客户端连接成功："  /*+ soc.AddressFamily.ToString()*/ + soc.LocalEndPoint.ToString());

                string[] strIp = soc.LocalEndPoint.ToString().Split(':');
                var tcpipModel = XmlControl.sequenceModelNew.TCPIPModels.FirstOrDefault(x => x.IPAddress == strIp[0] && x.PortNum.ToString() == strIp[1]);

                tcpipModel.IsConnected = true;
                tcpipModel.Handler.Add(soc.Handle);
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
            }
        }

        private void Comuncation_OnDisconnect(System.Net.Sockets.Socket soc)
        {
            try
            {
                SocketClientList.Remove(soc);
                m_DelOutPutLog("客户端断开连接：" + soc.AddressFamily.ToString());

                var tcpipModel = XmlControl.sequenceModelNew.TCPIPModels.FirstOrDefault(x => x.Handler.Contains(soc.Handle));
                if (tcpipModel != null)
                {
                    tcpipModel.Handler.Remove(soc.Handle);
                    if (tcpipModel.Handler.Count == 0)
                    {
                        tcpipModel.IsConnected = false;
                    }
                }
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
            }
        }

        private void Comuncation_OnError(string ErroMessage, System.Net.Sockets.Socket soc, int ErroCode)
        {
            if (soc != null)
            {
                m_DelOutPutLog("Comuncation_OnError ErrorSocket: " + ErroMessage + " ::: " + ErroCode.ToString());
            }
            else
            {
                m_DelOutPutLog("Comuncation_OnError Error:" + ErroMessage);
            }
        }

        private void WriteCommunicationToClient(TCPIPModel tcpipModel, string content, int clientIndex, int index = 0)
        {
            try
            {
                if (clientIndex == 0)
                {
                    //找出连接此IP的所有客户端
                    List<System.Net.Sockets.Socket> _sockClientList = new List<System.Net.Sockets.Socket>();
                    for (int i = 0; i < SocketClientList.Count; i++)
                    {
                        string[] strIp = SocketClientList[i].LocalEndPoint.ToString().Split(':');

                        if (strIp != null && strIp.Length == 2)
                        {
                            if (tcpipModel.IPAddress == strIp[0] && tcpipModel.PortNum.ToString() == strIp[1])
                            {
                                _sockClientList.Add(SocketClientList[i]);
                            }
                        }
                    }

                    if (_sockClientList.Count > 0)
                    {
                        for (int i = 0; i < _sockClientList.Count; i++)
                        {
                            m_SocketServerControl[index].SendText(content, i);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < SocketClientList.Count; i++)
                        {
                            m_SocketServerControl[index].SendText(content, i);
                            //m_DelOutPutLog("发送消息给客户端" + i + ":" + content);
                        }
                    }
                }
                else
                {
                    m_SocketServerControl[index].SendText(content, clientIndex - 1);
                    //m_DelOutPutLog("发送消息给客户端" + (clientIndex - 1) + ":" + content);
                }
            }
            catch (Exception ex)
            {
                m_DelOutExLog(ex);
            }

        }//向外写数据

        #endregion
          

    }
}
