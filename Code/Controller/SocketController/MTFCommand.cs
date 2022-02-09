using BaseController;
using BaseModels;
using GlobalCore;
using ServiceCollection;
using ServiceCollection.Services;
using SocketModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Log;
using SequenceTestModel;

namespace SocketController
{
    /// <summary>
    /// 富士康螺丝批通讯
    /// </summary>
    public struct TighteningResult
    {
        public string strTime;

        public string strMtfResult;
        public string strMtfErrorInfo;

        public string strStepSetSpeed1;
        public string strStepSetTorque1;
        public string strStepActualTorque1;
        public string strStepSetAngle1;
        public string strStepActualAngle1;

        public string strStepSetSpeed2;
        public string strStepSetTorque2;
        public string strStepActualTorque2;
        public string strStepSetAngle2;
        public string strStepActualAngle2;

        public string strStepSetSpeed3;
        public string strStepSetTorque3;
        public string strStepActualTorque3;
        public string strStepSetAngle3;
        public string strStepActualAngle3;

        public string strPeakTorque;
        public string strTotalAngle;

        //20200518 增加拍照结果
        public string strSnapResult;

        public string ErrorMsg;
    }

    public class MTFCommand
    {
        public SocketControl m_socketControl = new SocketControl();
        SocketService m_SockeService = new SocketService();
        public TCPIPModel m_SocketModel = new TCPIPModel();
        private Mutex m_Mutex = new Mutex();
        
        int m_nDataOffset = 10;
        public  int m_nStation = 1;  // 1,2

        string  m_strRecvData = "";
        public string  m_strRecvLastData = "";

        private string m_csCreateConnect         = "002000010060        ";

        private string m_csKeepConnect           = "002099990010        ";
        private string m_csKeepConnectSuccess    = "002099990010    00  ";
        private string m_csStartSubscribe        = "006000080010        1201001310000000000000000000000000000001";
        private string m_csStartSubscribeSuccess = "002400050010    00  0008";
        private string m_csStopSubscribe         = "002900090010        120100100";
        private string m_csStopSubscribeSuccess  = "002400050010    00  0009";
        //private string m_csStartSubscribe = "00691201001";
        //private string m_csStartSubscribeSuccess = "002400050010    00  1201";
        private string m_csSelectPset            = "002300180010        ";
        private string m_csSelectPsetSuccess     = "002400050010    00  0018";
        private string m_csRecvDataSuccess       = "006912010010    00  00200100";
        private string m_csRecvError = "002600040010    00  000871";

        string[] m_strErrorCodeArray = new string[]{"OK","值偏高","","","","发现重复打紧" };
        string[] m_strErrorValueArray = new string[] { "无", "总时间", "", "扭矩", "", "" };

        public Dictionary<int, TighteningResult> m_DicResult = new Dictionary<int, TighteningResult>();
        public  MTFCommand(string  strName,int nDataOffset,int nStation)
        {
            var listModel = XMLController.XmlControl.sequenceModel.TCPIPModels.FirstOrDefault(x => x.Name == strName);
            if (listModel != null )
            {
                m_SocketModel = listModel;
            }

            
            m_nDataOffset = nDataOffset;
            m_nStation = nStation;

            m_csKeepConnectSuccess += '\0';
            m_csSelectPsetSuccess  += '\0';
            m_csStartSubscribeSuccess += '\0';
            m_csRecvError += '\0';
            m_csStopSubscribeSuccess += '\0';
        }

        ~MTFCommand()
        {
            StopSubscribe();
        }

        public bool Init()
        {           
            if(!m_socketControl.Init(m_SocketModel))
            {
                return false;
            }

            return true;
        }

        public void Close()
        {
            if (null != m_socketControl)
            {
                m_socketControl.Close();
            }
        }

        public bool CreateConnect()
        {
            string csSend = m_csCreateConnect;
            BaseResultModel resultModel;

            csSend += '\0';

            m_Mutex.WaitOne();
            m_SocketModel.SendContent = csSend;
            resultModel = m_socketControl.Run(m_SocketModel, ControlType.SocketSend);
            if (!resultModel.RunResult)
            {
                m_Mutex.ReleaseMutex();
                return false;
            }

            m_Mutex.ReleaseMutex();

            resultModel = m_socketControl.Run(m_SocketModel, ControlType.SocketReceive);
            if (!resultModel.RunResult)
            {
                return false;
            }

            if(222 != resultModel.ObjectResult.ToString().Length)
            {
                return false;
            }

            Thread KeepConnect = new Thread(KeepConnectThread);
            KeepConnect.Start();

            return true;
        }

        public void KeepConnectThread()
        {
            string csSend = m_csKeepConnect;
            BaseResultModel resultModel;
            csSend += '\0';

            TCPIPModel socketModel = m_SocketModel;
            socketModel.SendContent = csSend;

           while (!Global.Break)
           {
                m_Mutex.WaitOne();
                resultModel = m_socketControl.Run(socketModel, ControlType.SocketSend);
                if (!resultModel.RunResult)
                {
                    m_Mutex.ReleaseMutex();
                    return ;
                }

                Thread.Sleep(5);

                string recv = "";
              
                resultModel = m_socketControl.Run(m_SocketModel, ControlType.SocketReceive);
                if (!resultModel.RunResult)
                {
                    m_Mutex.ReleaseMutex();
                    return ;
                }

                recv = resultModel.ObjectResult.ToString();

                bool bFind = recv.Contains(m_csKeepConnectSuccess);
                if (bFind)
                {
                    recv = recv.Replace(m_csKeepConnectSuccess, "");
                }

                bFind = recv.Contains(m_csRecvError);
                if (bFind)
                {
                    recv = recv.Replace(m_csRecvError, "");
                }

                if (recv != string.Empty)
                {
                    m_strRecvData += recv;
                }

                m_Mutex.ReleaseMutex();

                Thread.Sleep(1800);
            }
        }

        public bool StartSubscribe()
        {
            string csSend = m_csStartSubscribe;

            BaseResultModel resultModel;

            csSend += '\0';

            m_Mutex.WaitOne();
            m_SocketModel.SendContent = csSend;
            resultModel = m_socketControl.Run(m_SocketModel, ControlType.SocketSend);
            if (!resultModel.RunResult)
            {
                m_Mutex.ReleaseMutex();
                return false;
            }
           
            m_Mutex.ReleaseMutex();

            int nLength = 1000;

            string csRecv;
            if (!RecvBufferWithLength(nLength, out  csRecv, 2000))
            {
                //m_strRecvLastData = csRecv;
                //return false;
            }

            m_strRecvLastData = csRecv;

            bool bFind = csRecv.Contains(m_csStartSubscribeSuccess);
            if (!bFind)
            {
                return false;
            }

            return true;
        }

        public bool StopSubscribe()
        {
            string csSend = m_csStopSubscribe;

            BaseResultModel resultModel;

            csSend += '\0';



            m_Mutex.WaitOne();
            m_SocketModel.SendContent = csSend;
            resultModel = m_socketControl.Run(m_SocketModel, ControlType.SocketSend);
            if (!resultModel.RunResult)
            {
                m_Mutex.ReleaseMutex();
                return false;
            }

            m_Mutex.ReleaseMutex();

            int nLength = m_csStopSubscribeSuccess.Length;

            string csRecv;
            if (!RecvBufferWithLength(nLength, out csRecv, 2000))
            {
                m_strRecvLastData = csRecv;
                return false;
            }

            m_strRecvLastData = csRecv;



            bool bFind = csRecv.Contains(m_csStopSubscribeSuccess);
            if (!bFind)
            {
                return false;
            }


            return true;
        }

        public bool SelectPset(int nPset)
        {
            string strPset = string.Format("{0:D3}",nPset);
            string csSend = m_csSelectPset + strPset;

            BaseResultModel resultModel;

            csSend += '\0';


            m_Mutex.WaitOne();
            m_SocketModel.SendContent = csSend;
            resultModel = m_socketControl.Run(m_SocketModel, ControlType.SocketSend);
            if (!resultModel.RunResult)
            {
                m_Mutex.ReleaseMutex();
                return false;
            }

            m_Mutex.ReleaseMutex();

            int nLength = m_csSelectPsetSuccess.Length;

            string csRecv;
            if (!RecvBufferWithLength(nLength, out csRecv, 2000))
            {
                m_strRecvLastData = csRecv;
                return false;
            }

            m_strRecvLastData = csRecv;

            if (m_csSelectPsetSuccess != csRecv)
            {
                return false;
            }

            return true;
        }

        public bool RecvData_pre(ref TighteningResult  mtfResult)
        {
            mtfResult.strStepSetSpeed1 = "300";
            mtfResult.strStepSetSpeed2 = "900";
            mtfResult.strStepSetSpeed3 = "200";

            mtfResult.strStepSetTorque1 = "0.10";
            mtfResult.strStepSetTorque2 = "1.2";
            mtfResult.strStepSetTorque3 = "1.2";

            mtfResult.strStepSetAngle1 = "600";
            mtfResult.strStepSetAngle2 = "3000";
            mtfResult.strStepSetAngle3 = "10800";

            try
            {
                int nLength = 1000;

                string csRecv;
                if (!RecvBufferWithLength(nLength, out csRecv, 2000))
                {
                    //m_strRecvLastData = csRecv;
                    //return false;
                }

                m_strRecvLastData = csRecv; 

                if (nLength >= csRecv.Length || csRecv.Length > 5000)
                {
                    mtfResult.ErrorMsg = "接收长度：" + csRecv.Length.ToString() + "不在1000到5000之内";
                    return false;
                }

                bool bFind = csRecv.Contains(m_csRecvDataSuccess);
                if (!bFind)
                {
                    mtfResult.ErrorMsg = "接收信息不包含：" + m_csRecvDataSuccess;
                    return false;
                }

                int nIndexStart =  csRecv.IndexOf(m_csRecvDataSuccess);
                int nIndex = 0;

                ///时间
                nIndex = nIndexStart + 69 + 0x0070 + 12 + 1; 
                //增加判断长度
                if (nIndex + 19 <= csRecv.Length)
                {
                    string strTime = csRecv.Substring(nIndex, 19); 
                    mtfResult.strTime = strTime; 
                    m_strRecvLastData += strTime + "\r\n";
                }
                else
                {
                    mtfResult.strTime = "";
                }               

                ////结果
                nIndex = nIndexStart + 69 + 0x00B2  + 1; 
                if (nIndex + 2 <= csRecv.Length)
                {
                    string strCode = csRecv.Substring(nIndex, 2); 
                    int nErrorCode = Convert.ToInt16(strCode); 
                    mtfResult.strMtfResult = m_strErrorCodeArray[nErrorCode - 1]; 
                    m_strRecvLastData += strCode + "\r\n";
                }
                else
                {
                    mtfResult.strMtfResult = "OK";
                }

                ////错误值
                nIndex = nIndexStart + 69 + 0x00C5 + 1; 
                if(nIndex + 2 <= csRecv.Length)
                {
                    string strValue = csRecv.Substring(nIndex, 2); 
                    int nErrorValue = Convert.ToInt16(strValue);
                    mtfResult.strMtfErrorInfo = m_strErrorValueArray[nErrorValue - 1]; 
                    m_strRecvLastData += strValue + "\r\n";
                }
                else
                {
                    mtfResult.strMtfErrorInfo = "";
                }

                ////扭矩峰值
                //int nIndex = 69 + 0x02D0 + 1;
                //int nIndex = nIndexStart + 69 + 0x02C0 + 11 + 1;
                nIndex = nIndexStart + 69 + 0x02C0 + 10 + m_nDataOffset + 1;
                if(nIndex + 12 <= csRecv.Length)
                { 
                    string strData = csRecv.Substring(nIndex, 12);
                    double dData = Convert.ToDouble(strData) * 0.1 / 9.8;
                    mtfResult.strPeakTorque = dData.ToString();
                    m_strRecvLastData += strData + "\r\n";
                }
                else
                {
                    mtfResult.strPeakTorque = "";
                }

                ////总旋转角度        
                nIndex = nIndexStart + 69 + 0x02E0 + 7 + m_nDataOffset + 1;
                if(nIndex + 12 <= csRecv.Length)
                { 
                    string strTotalAngle = csRecv.Substring(nIndex, 12);
                    mtfResult.strTotalAngle = strTotalAngle;
                    m_strRecvLastData += strTotalAngle + "\r\n";
                }
                else
                {
                    mtfResult.strTotalAngle = "";
                }

                ////第一阶段扭矩峰值                
                nIndex = nIndexStart + 69 + 0x0550 + 6 + m_nDataOffset + 1;
                if(nIndex + 12 <= csRecv.Length)
                { 
                    string strStepData1 = csRecv.Substring(nIndex, 12);
                    double dStepData1 = Convert.ToDouble(strStepData1) * 0.1 / 9.8;
                    mtfResult.strStepActualTorque1 = dStepData1.ToString();
                    m_strRecvLastData += strStepData1 + "\r\n";
                }
                else
                {
                    mtfResult.strStepActualTorque1 = "";
                }

                ////第一阶旋转角度                
                nIndex = nIndexStart + 69 + 0x0570 + 3 + m_nDataOffset + 1;
                if(nIndex + 12 <= csRecv.Length)
                {
                    string strStepAngle1 = csRecv.Substring(nIndex, 12);
                    mtfResult.strStepActualAngle1 = strStepAngle1;
                    m_strRecvLastData += strStepAngle1 + "\r\n";
                }
                else
                {
                    mtfResult.strStepActualAngle1 = "";
                }

                ////第二阶段扭矩峰值                
                nIndex = nIndexStart + 69 + 0x0610 + 4 + m_nDataOffset + 1;
                if(nIndex + 12 <= csRecv.Length)
                {
                    string strStepData2 = csRecv.Substring(nIndex, 12);
                    double dStepData2 = Convert.ToDouble(strStepData2) * 0.1 / 9.8;
                    mtfResult.strStepActualTorque2 = dStepData2.ToString();
                    m_strRecvLastData += strStepData2 + "\r\n";
                }
                else
                {
                    mtfResult.strStepActualTorque2 = "";
                }

                ////第二阶旋转角度                
                nIndex = nIndexStart + 69 + 0x0630 + 1 + m_nDataOffset + 1;
                if(nIndex + 12 <= csRecv.Length)
                {
                    string strStepAngle2 = csRecv.Substring(nIndex, 12);
                    mtfResult.strStepActualAngle2 = strStepAngle2;
                    m_strRecvLastData += strStepAngle2 + "\r\n";
                }
                else
                {
                    mtfResult.strStepActualAngle2 = "";
                }

                ////第三阶段扭矩峰值                
                nIndex = nIndexStart + 69 + 0x06D0 + 2 + m_nDataOffset + 1;
                if(nIndex + 12 <= csRecv.Length)
                {
                    string strStepData3 = csRecv.Substring(nIndex, 12);
                    double dStepData3 = Convert.ToDouble(strStepData3) * 0.1 / 9.8;
                    mtfResult.strStepActualTorque3 = dStepData3.ToString();
                    m_strRecvLastData += strStepData3 + "\r\n";
                }
                else
                {
                    mtfResult.strStepActualTorque3 = "";
                }

                ////第三阶旋转角度                
                nIndex = nIndexStart + 69 + 0x06E0 + 15 + m_nDataOffset + 1;
                if(nIndex + 12 <= csRecv.Length)
                {
                    string strStepAngle3 = csRecv.Substring(nIndex, 12);
                    mtfResult.strStepActualAngle3 = strStepAngle3;
                    m_strRecvLastData += strStepAngle3 + "\r\n";
                }
                else
                {
                    mtfResult.strStepActualAngle3 = "";
                }
                
                //nIndex = nIndexStart + 69 + 0x01A8 + 1;
                //nIndex = nIndexStart + 69 + 0x01A0 + m_nPreDataOffset + 1;

                //string   strPreData = csRecv.Substring(nIndex, 4);

                //dPreData = Convert.ToDouble(strPreData);

                //m_strRecvLastData += strPreData;
            }
            catch(Exception ex)
            {
                mtfResult.ErrorMsg = ex.Message;
               // return  false;
            }

            return true;
        }

        public bool RecvData(ref TighteningResult mtfResult)
        {
            mtfResult.strStepSetSpeed1 = "300";
            mtfResult.strStepSetSpeed2 = "900";
            mtfResult.strStepSetSpeed3 = "200";

            mtfResult.strStepSetTorque1 = "0.10";
            mtfResult.strStepSetTorque2 = "1.2";
            mtfResult.strStepSetTorque3 = "1.2";

            mtfResult.strStepSetAngle1 = "600";
            mtfResult.strStepSetAngle2 = "3000";
            mtfResult.strStepSetAngle3 = "10800";

            try
            {
                int nLength = 8000;

                string csRecv;
                if (!RecvBufferWithLength(nLength, out csRecv, 2000))
                {
                    //m_strRecvLastData = csRecv;
                    //return false;
                }

                m_strRecvLastData = csRecv;
                string str2 = "\r\n" + DateTime.Now.ToString() + ":" + "\r\n";
                Log.WriteLog(Log.CommunicationFileName, str2 + csRecv);

                if (nLength >= csRecv.Length || csRecv.Length > 5000)
                {
                    //mtfResult.ErrorMsg = "接收长度：" + csRecv.Length.ToString() + "不在1000到5000之内";
                    //return false;
                }

                bool bFind = csRecv.Contains(m_csRecvDataSuccess);
                if (!bFind)
                {
                    mtfResult.ErrorMsg = "接收信息不包含：" + m_csRecvDataSuccess;
                    return false;
                }

                int nIndexStart = csRecv.IndexOf(m_csRecvDataSuccess);
                int nIndex = 0;

                ///时间
                nIndex = nIndexStart + 69 + 0x0070 + 12 + 1;
                //增加判断长度
                if (nIndex + 19 <= csRecv.Length)
                {
                    string strTime = csRecv.Substring(nIndex, 19);
                    mtfResult.strTime = strTime;
                    m_strRecvLastData += strTime + "\r\n";
                }
                else
                {
                    mtfResult.strTime = "";
                } 

                ////结果
                nIndex = nIndexStart + 69 + 0x00B2 + 1;
                if (nIndex + 2 <= csRecv.Length)
                {
                    string strCode = csRecv.Substring(nIndex, 2);
                    int nErrorCode = Convert.ToInt16(strCode);
                    mtfResult.strMtfResult = m_strErrorCodeArray[nErrorCode - 1];
                    m_strRecvLastData += strCode + "\r\n";
                }
                else
                {
                    mtfResult.strMtfResult = "OK";
                } 

                ////错误值
                nIndex = nIndexStart + 69 + 0x00C5 + 1;
                if (nIndex + 2 <= csRecv.Length)
                {
                    string strValue = csRecv.Substring(nIndex, 2);
                    int nErrorValue = Convert.ToInt16(strValue);
                    mtfResult.strMtfErrorInfo = m_strErrorValueArray[nErrorValue - 1];
                    m_strRecvLastData += strValue + "\r\n";
                }
                else
                {
                    mtfResult.strMtfErrorInfo = "";
                }

                ////扭矩峰值 30230
                nIndex = csRecv.LastIndexOf("30230");
                if (nIndex != -1)
                {
                    string dData = csRecv.Substring(nIndex + 20, 9);
                    mtfResult.strPeakTorque = dData.ToString();
                }
                else
                {
                    mtfResult.strPeakTorque = "";
                }


                ////总旋转角度 30231
                nIndex = csRecv.LastIndexOf("30231");
                if (nIndex != -1)
                {
                    string strTotalAngle = csRecv.Substring(nIndex + 20, 9);
                    mtfResult.strTotalAngle = strTotalAngle;
                }
                else
                {
                    mtfResult.strTotalAngle = "";
                }

                ////第一阶段扭矩峰值  130302
                nIndex = csRecv.LastIndexOf("130302");
                if (nIndex != -1)
                {
                    string str = csRecv.Substring(nIndex + 21, 9);
                    mtfResult.strStepActualTorque1 = str.ToString();
                }
                else
                { 
                    mtfResult.strStepActualTorque1 = "";
                }

                ////第一阶旋转角度   
                if (nIndex != -1)
                {
                    string str = csRecv.Substring(nIndex + 21 + 21 + 8, 9); 
                    mtfResult.strStepActualAngle1 = str;
                }
                else
                { 
                    mtfResult.strStepActualAngle1 = "";
                }

                ////第二阶段扭矩峰值  230302
                nIndex = csRecv.LastIndexOf("230302");
                if (nIndex != -1)
                {
                    string str = csRecv.Substring(nIndex + 21, 9);
                    mtfResult.strStepActualTorque2 = str.ToString();
                }
                else
                {
                    mtfResult.strStepActualTorque2 = "";
                }

                ////第二阶旋转角度   
                if (nIndex != -1)
                {
                    string str = csRecv.Substring(nIndex + 21 + 21 + 8, 9);
                    mtfResult.strStepActualAngle2 = str;
                }
                else
                {
                    mtfResult.strStepActualAngle2 = "";
                }

                ////第三阶段扭矩峰值  330302
                nIndex = csRecv.LastIndexOf("330302");
                if (nIndex != -1)
                {
                    string str = csRecv.Substring(nIndex + 21, 9);
                    mtfResult.strStepActualTorque3 = str.ToString();
                }
                else
                {
                    mtfResult.strStepActualTorque3 = "";
                }

                ////第三阶旋转角度   
                if (nIndex != -1)
                {
                    string str = csRecv.Substring(nIndex + 21 + 21 + 8, 9);
                    mtfResult.strStepActualAngle3 = str;
                }
                else
                {
                    mtfResult.strStepActualAngle3 = "";
                }

            }
            catch (Exception ex)
            {
                mtfResult.ErrorMsg = ex.Message;
                // return  false;
            }

            return true;
        }

        private bool RecvBufferWithLength(int nLength, out string csRecv, UInt32 dwTimeOut)
        {
            BaseResultModel resultModel;
            csRecv = "";

            long dwOldTime = DateTime.Now.Ticks;
            long dwCurTime = dwOldTime;

            
            while (true)
            {
                dwCurTime = DateTime.Now.Ticks;
                if (dwCurTime - dwOldTime > dwTimeOut)
                {
                    return false;
                }

                Thread.Sleep(20);          

                string recv = "";

                m_Mutex.WaitOne();

                if(!string.IsNullOrEmpty(m_strRecvData))
                {
                    csRecv += m_strRecvData;
                    m_strRecvData = "";
                }
                

                resultModel = m_socketControl.Run(m_SocketModel, ControlType.SocketReceive);
                if (!resultModel.RunResult)
                {
                    m_Mutex.ReleaseMutex();
                    return false;
                }
                
                recv = resultModel.ObjectResult.ToString();

                if (recv != string.Empty)
                {
                    csRecv += recv;
                }

                
                bool bFind = csRecv.Contains(m_csKeepConnectSuccess);
                if (bFind)
                {
                    csRecv = csRecv.Replace(m_csKeepConnectSuccess, "");
                }

                bFind = csRecv.Contains(m_csRecvError);
                if (bFind)
                {
                    csRecv = csRecv.Replace(m_csRecvError, "");
                }

                int len = csRecv.Length;
                //Log.WriteLog(Log.CommunicationFileName, "Length:" + len.ToString() + " " + m_SocketModel.Name);
                if (nLength <= len)
                {
                    m_Mutex.ReleaseMutex();
                    break;
                }

                m_Mutex.ReleaseMutex();

            }

            return true;
        }
    }
}
