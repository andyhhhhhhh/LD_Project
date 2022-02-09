
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using System.Management;
using System.Windows.Forms;
using System.Net;

namespace ServiceController
{
    public struct LoginParam
    {
        public string userName;//登錄 NewFuse 系統賬戶（通常為工號）
        public string passWord;//登錄 NewFuse 系統賬戶對應密碼
        public string stationID;//工站 ID，由 IE 人員配置

        public string lang;//系統使用語言
        public string site; //Site 代碼
        public string bu; //Bu 代碼
    }

    public  class SFCMethod
    {
        private string m_serverPage = "http://10.162.80.77:8099/FuseService/Services/ServiceFacade.asmx";
        public string  m_Token = "";
        public string  m_JobID = "";

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        public  SFCMethod()
        {
            this.m_Token = "";
        }

        /// <summary>
        /// 用戶驗證取得 Token 值，後續操作需要用到此Token 值
        /// </summary>
        /// <param name="loginParam"></param>
        /// <returns>Token 或者 FALSE^消息（獲取 Token 失敗）</returns>
        public bool GetUserToken( LoginParam loginParam, out string errorMsg)
        {
            try
            {
                errorMsg = "";
                if (!m_Token.Contains("FALSE") && !string.IsNullOrEmpty(m_Token))
                {
                    return true;
                }

                ServiceReference1.ServiceFacadeSoapClient aa = new ServiceReference1.ServiceFacadeSoapClient();
                m_Token = aa.GetUserToken(loginParam.userName, loginParam.passWord, loginParam.stationID,
                               loginParam.lang, loginParam.site, loginParam.bu);

                if(m_Token.Contains("FALSE"))
                {
                    errorMsg = m_Token;
                    return false;
                }

            }
            catch (Exception  ex)
            {
                errorMsg = ex.Message;
                return false;//连接服务器失败
            }

            return true;
            
        }

        /// <summary>
        /// 檢查產品路由。
        /// </summary>
        /// <param name="pid">產品條碼，如 PID,IMEI 等</param>
        /// <returns>結果^消息</returns>
        public bool CheckRoute(string pid, out string errorMsg)
        {
            try
            {
                errorMsg = "";
                ServiceReference1.ServiceFacadeSoapClient aa = new ServiceReference1.ServiceFacadeSoapClient();
                string  result = aa.CheckRoute(pid, m_Token);

                if (result.Contains("FALSE"))
                {
                    errorMsg = result;
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;//连接服务器失败
            }

            return true;
        }

        /// <summary>
        /// 獲取測試事務流水號    在 CheckRoute 返回 TRUE 時再調用此接口
        /// </summary>
        /// <returns>JobID 或者 FALSE^……</returns>
        public bool GetNextJobID(out string errorMsg)
        {
            try
            {
                errorMsg = "";
                ServiceReference1.ServiceFacadeSoapClient aa = new ServiceReference1.ServiceFacadeSoapClient();
                m_JobID = aa.GetNextJobID(m_Token);

                if (m_JobID.Contains("FALSE"))
                {
                    errorMsg = m_JobID;
                    return false;
                }

            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;//连接服务器失败
            }
            return true;
        }
        
        public bool CheckAcc(string pid, string accName, string accValue, out string errorMsg)
        {
            try
            {
                errorMsg = "";
                ServiceReference1.ServiceFacadeSoapClient aa = new ServiceReference1.ServiceFacadeSoapClient();
                string str = aa.CheckACC(pid, accName, accValue);

                if (str.Contains("FALSE"))
                {
                    errorMsg = str;
                    return false;
                }

            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;//连接服务器失败
            }
            return true;
        }

        public bool GetAccDsnData(string pid, string accType, out string dsn, out string errorMsg)
        {
            dsn = "";
            try
            {
                errorMsg = "";
                ServiceReference1.ServiceFacadeSoapClient aa = new ServiceReference1.ServiceFacadeSoapClient();
                string str = aa.GetAccDsnData(pid, accType);

                errorMsg = str;
                if (str.Contains("FALSE"))
                {
                    return false;
                }
                else
                {
                    dsn = str.Replace("TRUE^", "");
                }

            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;//连接服务器失败
            }
            return true;
        }

        public bool SaveAcc(string pid, string accName, string accValue, out string errorMsg)
        {
            try
            {
                errorMsg = "";
                ServiceReference1.ServiceFacadeSoapClient aa = new ServiceReference1.ServiceFacadeSoapClient();
                string str = aa.SaveAcc(pid, accName, accValue);

                if (str.Contains("FALSE"))
                {
                    errorMsg = str;
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;//连接服务器失败
            }
            return true;
        }

        /// <summary>
        /// 記錄測試結果，推動路由到下一站
        /// </summary>
        /// <param name="pid"></param>  產品條碼，如 pid, IMEI 等
        /// <param name="result"></param> 检测结果P或者F
        /// <param name="errCode"></param> 若 result=“F”，可以上傳錯誤代碼，此參數一般為空
        /// <param name="fixture"></param>  治具编号
        /// <returns></returns>
        public bool ForwardRoute(string pid, string result, string errCode, string fixture, out string errorMsg)
        {
            try
            {
                errorMsg = "";
                ServiceReference1.ServiceFacadeSoapClient aa = new ServiceReference1.ServiceFacadeSoapClient();
                string strRet = aa.ForwardRoute(pid,result, errCode, fixture,m_JobID,"",m_Token);
                if (strRet.Contains("FALSE"))
                {
                    errorMsg = strRet;
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;//连接服务器失败
            }

            return true;
        }
        
        private void Sleep(int delay)
        {
            DateTime dt = DateTime.Now;
            while ((DateTime.Now - dt).TotalMilliseconds < delay)
            {
                Application.DoEvents();
            }
        }

        public void WriteToFile(string path, string inputStr)
        {
            try
            {
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.BaseStream.Seek(0, SeekOrigin.End);
                sw.WriteLine(inputStr);
                sw.Flush();
                sw.Close();
                fs.Close();
            }
            catch (Exception)
            {

            }

        }

        /// <summary>
        /// 上傳测试工站測試 Log 至 SFC 系统
        /// </summary>
        /// <param name="logInfo">測試 log 使用 json 字符串格式上傳</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool UploadTestLogFile(string logInfo, out string errorMsg)
        {
            errorMsg = "";
            ServiceReference1.ServiceFacadeSoapClient aa = new ServiceReference1.ServiceFacadeSoapClient();
             
            string str = aa.UploadTestLogFile(logInfo, m_Token);
            if (str.Contains("FALSE"))
            {
                errorMsg = str;
                return false;
            }
            return str.Contains("TRUE");
        } 

    }

    public class ResultInfo
    {
        public string Name;
        public string Result;
        public string MinValue;
        public string MaxValue;
        public string PassFail;
    }
}
