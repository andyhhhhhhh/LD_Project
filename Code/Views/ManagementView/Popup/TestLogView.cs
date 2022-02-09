using AlgorithmController;
using DevComponents.DotNetBar;
using GlobalCore;
using Infrastructure.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserSetController;

namespace ManagementView.Popup
{
    public partial class TestLogView : Form
    {
        int rectRight = 0;
        int rectBottom = 0;
        public TestLogView(int RectRight, int RectBottom)
        {
            InitializeComponent();
            rectRight = RectRight;
            rectBottom = RectBottom;
        } 
        
        private void Frm_Popup2_Load(object sender, EventArgs e)
        {
            //int x = Screen.PrimaryScreen.WorkingArea.Right - this.Width;
            //int y = Screen.PrimaryScreen.WorkingArea.Bottom - this.Height;
            int x = rectRight - this.Width;
            int y = rectBottom - this.Height;
            this.Location = new Point(x, y);//设置窗体在屏幕右下角显示 

            //选择语言
            //string strLang = Global.GetNodeValue("Language");
            //MultiLanguage.SetLanguage(this.Controls, strLang);
            CommFunc.OpenAnimateWindow(this.Handle);
        }

        private void Frm_Popup2_FormClosing(object sender, FormClosingEventArgs e)
        {
            CommFunc.CloseAnimateWindow(this.Handle);
        }

        OpenFileDialog OpenlogDialog = new OpenFileDialog();
        private void btnLoad_Click(object sender, EventArgs e)
        {
            Stream inputStream = null; 
            this.OpenlogDialog.Filter = "All files (*.*)|*.*";
            if (this.OpenlogDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((inputStream = this.OpenlogDialog.OpenFile()) != null)
                    {
                        String strLogFile = this.OpenlogDialog.FileName;
                        this.OpenlogDialog.InitialDirectory = strLogFile;
                        txtLogPath.Text = strLogFile;

                        ReadFile(strLogFile);
                    }
                }
                catch (Exception ex)
                {
                    MessageBoxEx.Show(this, "打开文件失败!" + ex.Message, "提示");
                }
            }
        }
        
        public void ReadFile(string filepath)
        {
            try
            {
                Thread t = new Thread(new ThreadStart(() =>
                {
                    FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);//初始化文件流
                    byte[] array = new byte[fs.Length];//初始化字节数组
                    fs.Read(array, 0, array.Length);//读取流中数据到字节数组中
                    fs.Close();//关闭流
                    string str = Encoding.UTF8.GetString(array);//将字节数组转化为字符串
                    BeginInvoke(new Action(() =>
                    {
                        richLog.Clear();
                        richLog.AppendText(str);
                    }));
                }));
                t.Start();
            }
            catch (Exception e)
            {
                richLog.AppendText(e.ToString());
            }
        }

        //加载当天日志
        private void btnNowNote_Click(object sender, EventArgs e)
        {
            try
            {
                Thread t = new Thread(new ThreadStart(() =>
                {
                    string dt = DateTime.Now.ToString("yyyy_MM_dd");
                    string logPath = Log.LogDirectory + "\\NoticeLog" + "_" + dt + ".log"; 

                    FileStream fs = new FileStream(logPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    StreamReader streamReader = new StreamReader(fs);
                    string line = "";

                    BeginInvoke(new Action(() =>
                    {
                        richLog.Clear();
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            //string strtime = DateTime.Now.ToString("yyyy-MM-dd");
                            //if (line.Contains(strtime))
                            //{
                            string strline = line + "\r\n";
                            richLog.AppendText(strline);
                            //}
                        }
                    }));

                }));
                t.Start();
            }
            catch (Exception ex)
            {
                richLog.AppendText(ex.ToString());
            }
        }

        //加载缓存区日志
        private void btnBuffer_Click(object sender, EventArgs e)
        {
            try
            {
                richLog.Clear();
                richLog.AppendText(GlobalCore.Global.LogBuffer);
            }
            catch(Exception ex)
            { 
                richLog.AppendText(ex.ToString());
            }
        } 
       
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                LogShowInfo("");
            }
            catch (Exception ex)
            {

            }
            
        }

        private void btnError_Click(object sender, EventArgs e)
        {
            LogShowInfo("【Error】");
            btnError.BackColor = Color.Turquoise;
            btnInfo.BackColor = Color.Transparent;
            btnDebug.BackColor = Color.Transparent;
            btnAll.BackColor = Color.Transparent;
            btnException.BackColor = Color.Transparent;
        }

        private void btnInfo_Click(object sender, EventArgs e)
        { 
            LogShowInfo("【Info】");
            btnError.BackColor = Color.Transparent;
            btnInfo.BackColor = Color.Turquoise;
            btnDebug.BackColor = Color.Transparent;
            btnAll.BackColor = Color.Transparent;
            btnException.BackColor = Color.Transparent;
        }

        private void btnDebug_Click(object sender, EventArgs e)
        {
            LogShowInfo("【Debug】");
            btnError.BackColor = Color.Transparent;
            btnInfo.BackColor = Color.Transparent;
            btnDebug.BackColor = Color.Turquoise;
            btnAll.BackColor = Color.Transparent;
            btnException.BackColor = Color.Transparent;
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            LogShowInfo("");
            btnError.BackColor = Color.Transparent;
            btnInfo.BackColor = Color.Transparent;
            btnDebug.BackColor = Color.Transparent;
            btnAll.BackColor = Color.Turquoise;
            btnException.BackColor = Color.Transparent;
        }

        private void btnException_Click(object sender, EventArgs e)
        {
            LogShowInfo("【Exception】");
            btnError.BackColor = Color.Transparent;
            btnInfo.BackColor = Color.Transparent;
            btnDebug.BackColor = Color.Transparent;
            btnAll.BackColor = Color.Transparent;
            btnException.BackColor = Color.Turquoise;
        }

        private void LogShowInfo(string value)
        {
            try
            {
                Thread t = new Thread(new ThreadStart(() =>
                 {
                     string dt = DateTime.Now.ToString("yyyy_MM_dd");
                     string logPath = txtLogPath.Text;
                     if (logPath == "")
                     {
                         logPath = Log.LogDirectory + "\\NoticeLog" + "_" + dt + ".log";
                     }
                     FileStream fs = new FileStream(logPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                     StreamReader streamReader = new StreamReader(fs);
                     string line = "";

                     BeginInvoke(new Action(() =>
                     {
                         richLog.Clear();
                         while ((line = streamReader.ReadLine()) != null)
                         {
                             if(line.Contains(value))
                             { 
                                 string strline = line + "\r\n";
                                 richLog.AppendText(strline);
                             }
                         }
                     }));

                 }));
                t.Start();
            }
            catch (Exception ex)
            {

            }

        }

    }
}
