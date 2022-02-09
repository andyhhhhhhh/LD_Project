using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Infrastructure.Log
{
    public enum LogLevel
    {
        Debug = 0,     /* 调试信息 */
        Info = 1,    /* 提示信息 */
        Warning = 2,   /* 警告信息 */
        Error = 3,     /* 错误信息 */
        Exception = 4,  /* 异常信息 */
        System = 5 /*系统日志*/
    };

    public class Log
    {
        private static object m_LockObj = new object();
        private static int m_LogSize = 1024;
        private static Stopwatch m_Sp = new Stopwatch();
        #region 变量
        public static String LogDirectory = "Log/";
        private static LogLevel LogLevelToWrite = LogLevel.Debug;
        private static String LogFileNamePath = "DebugLog";

        public const string ErrorLogFileName = "ErrorLog";
        public const string WarningLogFileName = "WarningLog";
        public const string ExceptionLogFileName = "ExceptionLog";
        public const string DebugLogFileName = "DebugLog";
        public const string NoticeLogFileName = "NoticeLog";
        public const string CalibrationFileName = "CalibrationLog";
        public const string SystemFileName = "SystemLog";
        public const string CommunicationFileName = "CommunicationLog";
        public const string ProcessLogFileName = "ProcessLog";
        public const string MeasureAlgorithmLogFileName = "MeasureAlgorithmLog";
        public const string OffsetLogFileName = "OffsetLog";
        #endregion

        public static bool WriteLog(LogLevel LogLevel, String strLogDesc)
        {
            string fileName = LogFileNamePath;

            if (!File.Exists(fileName))
            {
                if (fileName.LastIndexOf("\\") > 0)
                {
                    string dir = fileName.Substring(0, fileName.LastIndexOf("\\"));
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                }
            }

            if (LogLevel == LogLevel.Error)
            {
                fileName = ErrorLogFileName;
            }
            else if (LogLevel == LogLevel.Exception)
            {
                fileName = ExceptionLogFileName;
            }
            else if (LogLevel == LogLevel.Warning)
            {
                fileName = WarningLogFileName;
            }
            else if (LogLevel == LogLevel.Debug)
            {
                fileName = DebugLogFileName;
            }
            else if (LogLevel == LogLevel.Info)
            {
                fileName = NoticeLogFileName;
            }
            else if (LogLevel == LogLevel.System)
            {
                fileName = SystemFileName;
            }

            return WriteLog(fileName, strLogDesc);
        }

        public static bool WriteLog(String strFileName, String strLogDesc)
        {
            //strFileName = LogDirectory + strFileName;

            if (!File.Exists(strFileName))
            {
                if (!Directory.Exists(LogDirectory))
                {
                    Directory.CreateDirectory(LogDirectory);
                }
            }
            try
            {
                bool bReturn = false;

                if (null == LogFileNamePath)
                {
                    LogFileNamePath = strFileName;
                }
                string dt = "_" + DateTime.Now.ToString("yyyy_MM_dd");//加上日期

                Task task = new Task(new Action(() =>
                {
                    lock (m_LockObj)
                    {
                        try
                        {
                            if(!strFileName.Contains(".log"))
                            {
                                strFileName += dt + ".log";
                            }
                            StreamWriter writer = File.AppendText(LogDirectory + strFileName);//文件中添加文件流   
                            //if (!strLogDesc.Contains(DateTime.Now.ToString("yyyy-MM-dd")))
                            //{
                            //    strLogDesc = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + strLogDesc;
                            //}
                            
                            writer.WriteLine(strLogDesc);
                            writer.Flush();
                            writer.Close();

                            //GlobalCore.Global.LogBuffer += strLogDesc + Environment.NewLine;

                            //if (m_Sp.ElapsedMilliseconds > 10 * 60 * 1000)
                            //{
                            //    FileInfo fileInfo = new FileInfo(strFileName);
                            //    var length = fileInfo.Length;
                            //    if (length > m_LogSize)
                            //    {
                            //        fileInfo.MoveTo(strFileName + DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                            //    }
                            //    m_Sp.Restart();
                            //}
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }));
                task.Start();

                return bReturn;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //public static bool WriteLog(String directory, String strFileName, LogLevel LogLevel, String strLogDesc)
        //{
        //    if (!Directory.Exists(directory))
        //    {
        //        Directory.CreateDirectory(directory);
        //    }

        //    strFileName = directory + "\\" + strFileName;
        //    try
        //    {
        //        bool bReturn = false;

        //        if ((int)LogLevelToWrite > (int)LogLevel)
        //        {
        //            return true;
        //        }

        //        if (null == LogFileNamePath)
        //        {
        //            LogFileNamePath = strFileName;
        //        }

        //        lock (m_LockObj)
        //        {
        //            Task task = new Task(new Action(() =>
        //            {
        //                File.WriteAllText(strFileName, strLogDesc);
        //            }));
        //            task.Start();
        //        }

        //        return bReturn;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        public static void SetLogSize(int Size)
        {
            try
            {
                m_LogSize = (Size * 1024 * 1024);
                m_Sp.Start();
                //SetLogSizeCpp((UInt64)(Size * 1024 * 1024));
            }
            catch (Exception ex)
            {
                Log.WriteLog(LogLevel.Exception, ex.Message);
            }
        }

        public static void SetWriteLogLevel(LogLevel LogLevelToWriteSet)
        {
            LogLevelToWrite = LogLevelToWriteSet;
        }
        
    }
}
