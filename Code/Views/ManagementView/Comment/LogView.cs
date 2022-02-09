using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infrastructure.Log;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar;

namespace ManagementView.Comment
{
    public partial class LogView : UserControl
    {
        public LogView()
        {
            InitializeComponent();
        }

        private void LogView_Load(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                 
            }
        }

        public void LogMessage(string message, LogLevel logInfo = LogLevel.Info)
        {
            BeginInvoke(new Action(() =>
            {
                message = message.Replace("HALCON error ", "");
                string strInfo = Enum.GetName(typeof(LogLevel), logInfo);
                string strLog = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff") + ":" + "【" + strInfo + "】" + message + Environment.NewLine;

                if (logInfo != LogLevel.Info)
                {
                    if (logInfo == LogLevel.Debug)
                    {
                        AppendTextColorful(strLog, logInfo, rtbLog_TCP);
                        if (rtbLog_TCP.Text.Length > 1000 * 1024)
                        {
                            rtbLog_TCP.Clear();
                        }
                    }
                    else
                    {
                        AppendTextColorful(strLog, logInfo, rtbLog_Error);
                        if (rtbLog_Error.Text.Length > 1000 * 1024)
                        {
                            rtbLog_Error.Clear();
                        }
                    }
                    Log.WriteLog(logInfo, strLog.Trim());
                }

                //要显示所有信息到Info上面
                if (logInfo == LogLevel.Info)
                {
                    rtbLog_Info.AppendText(strLog);
                }
                else
                {
                    AppendTextColorful(strLog, logInfo, rtbLog_Info);
                }

                rtbLog_Info.ScrollToCaret();
                Log.WriteLog(LogLevel.Info, strLog.Trim());

                if (rtbLog_Info.Text.Length > 1000 * 1024)
                {
                    rtbLog_Info.Clear();
                }
            }));
        }
        private void AppendLog(string message, LogLevel logInfo = LogLevel.Info)
        {
            message = message.Replace("HALCON error ", "");
            string strInfo = Enum.GetName(typeof(LogLevel), logInfo);
            string strLog = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff") + ":" + "【" + strInfo + "】" + message + Environment.NewLine;

            if (logInfo != LogLevel.Info)
            {
                if (logInfo == LogLevel.Debug)
                {
                    AppendTextColorful(strLog, logInfo, rtbLog_TCP);
                }
                else
                {
                    AppendTextColorful(strLog, logInfo, rtbLog_Error);
                }
                Log.WriteLog(logInfo, strLog.Trim());
            }

            //要显示所有信息到Info上面
            if (logInfo == LogLevel.Info)
            {
                rtbLog_Info.AppendText(strLog);
            }
            else
            {
                AppendTextColorful(strLog, logInfo, rtbLog_Info);
            }

            rtbLog_Info.ScrollToCaret();
            Log.WriteLog(LogLevel.Info, strLog.Trim());

            if (rtbLog_Info.Text.Length > 50 * 1024)
            {
                rtbLog_Info.Clear();
            }
        }
          
        //根据Log属性输出不同颜色
        public void AppendTextColorful(string text, LogLevel logInfo = LogLevel.Info, RichTextBoxEx rtbLog = null)
        {
            try
            {
                Color color = Color.Black;
                switch (logInfo)
                {
                    case LogLevel.Debug:
                        color = Color.Gray;
                        break;
                    case LogLevel.Error:
                        color = Color.MediumVioletRed;
                        break;
                    case LogLevel.Exception:
                        color = Color.Red;
                        break;
                    case LogLevel.Warning:
                        color = Color.Orange;
                        break;

                    default:
                        color = Color.Black;
                        break;
                }

                rtbLog.SelectionStart = rtbLog_Info.TextLength;
                rtbLog.SelectionLength = 0;
                rtbLog.SelectionColor = color;
                Font font = new Font("宋体", 9f, FontStyle.Bold);
                rtbLog.SelectionFont = font;
                rtbLog.AppendText(text);
                rtbLog.SelectionColor = rtbLog_Info.ForeColor;
                rtbLog.SelectionFont = rtbLog_Info.Font;
            }
            catch (Exception ex)
            {

            }
        }

        //显示流程信息
        public void LogSysMessage(string message, LogLevel logInfo = LogLevel.Info)
        {
            BeginInvoke(new Action(() =>
            {
                message = message.Replace("HALCON error ", "");
                string strInfo = Enum.GetName(typeof(LogLevel), logInfo);
                string strLog = DateTime.Now + ":" + "【" + strInfo + "】" + message + Environment.NewLine;
                rtbLog_Info.AppendText(strLog);
                rtbLog_Info.ScrollToCaret();
                Log.WriteLog(LogLevel.System, strLog.Trim());
            }));
        }
        public void LogShowMessage(string message)
        {
            BeginInvoke(new Action(() =>
            {
                rtbLog_Info.AppendText(DateTime.Now + ":" + message + Environment.NewLine);
                rtbLog_Info.ScrollToCaret();
                MessageBoxEx.Show(message);
                Log.WriteLog(LogLevel.Debug, message);
            }));
        }
  
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbLog_Info.Clear();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            rtbLog_Error.Clear();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            rtbLog_TCP.Clear();
        }

        public Color BacksColor
        {
            set { rtbLog_Info.BackColor = value; }
        }

    }
}
