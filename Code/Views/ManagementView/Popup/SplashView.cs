using AlgorithmController;
using GlobalCore;
using ManagementView._3DViews;
using ManagementView.EncyptView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagementView.Popup
{
    public partial class SplashView : Form
    {
        static bool m_bOpen = false;
        public static SplashView m_view1;
        public SplashView()
        {
            InitializeComponent();
            m_view1 = this;
            BaseModels.BaseResultModel baseModel = new BaseModels.BaseResultModel();
        }
        
        private void SplashView_Load(object sender, EventArgs e)
        {
            try
            {
                //设置欢迎界面只被打开一次
                if(m_bOpen)
                {
                    this.Close();
                    return;
                }
                m_bOpen = true;
                //设置启动窗体
                skinprogress.Start();
                skinprogress.AnimationSpeed = 2; 
                this.FormBorderStyle = FormBorderStyle.None;

                //labelCompany.Text = "深圳市大晟博视科技有限公司 0755-21038106";

                labelCompany.Text = "";
                this.BackgroundImage = Image.FromFile("loading.jpg"); 
                this.BackgroundImageLayout = ImageLayout.Stretch;
                ShowLoadTxt();
            }
            catch (Exception ex)
            {

            }
        }

        bool m_bShow = true;
        private void ShowLoadTxt()
        {
            try
            {
                Task task = new Task(new Action(() =>
                {
                    while (m_bShow)
                    {
                        string strText = lblLoadTxt.Text;
                        if (strText.Length > 5)
                        {
                            string strPoint = strText.Substring(5);
                            BeginInvoke(new Action(() =>
                            {
                                if (strPoint.Length == 5)
                                {
                                    lblLoadTxt.Text = "正在加载中" + strPoint.Replace(".....", ".");
                                }
                                if (strPoint.Length == 4)
                                {
                                    lblLoadTxt.Text = "正在加载中" + strPoint.Replace("....", ".....");
                                }
                                if (strPoint.Length == 3)
                                {
                                    lblLoadTxt.Text = "正在加载中" + strPoint.Replace("...", "....");
                                }
                                if (strPoint.Length == 2)
                                {
                                    lblLoadTxt.Text = "正在加载中" + strPoint.Replace("..", "...");
                                }
                                if (strPoint.Length == 1)
                                {
                                    lblLoadTxt.Text = "正在加载中" + strPoint.Replace(".", "..");
                                }
                            }));
                        }
                       
                        Thread.Sleep(400);
                        Application.DoEvents();
                    }
                }));
                task.Start();
                //Thread t = new Thread(new ThreadStart(() =>
                //{
                //    while (m_bShow)
                //    {
                //        string strText = lblLoadTxt.Text;
                //        string strPoint = strText.Substring(5); 
                        
                //        if (strPoint.Length == 4)
                //        {
                //            lblLoadTxt.Text = strPoint.Replace("....", ".");
                //        }
                //        if (strPoint.Length == 3)
                //        {
                //            lblLoadTxt.Text = strPoint.Replace("...", "....");
                //        }
                //        if (strPoint.Length == 2)
                //        {
                //            lblLoadTxt.Text = strPoint.Replace("..", "...");
                //        }
                //        if (strPoint.Length == 1)
                //        {
                //            lblLoadTxt.Text = strPoint.Replace(".", "..");
                //        }
                //        Thread.Sleep(50);
                //        Application.DoEvents();
                //    }
                //}));
                //t.Start();
            }
            catch (Exception ex)
            {
                 
            }
            
        }
        
        /// <summary>
        /// 关闭自身
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public void KillMe(object o, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 加载并显示主窗体
        /// </summary>
        /// <param name="form">主窗体</param>
        public static void LoadAndRun(Form form)
        {
            try
            {
                //订阅主窗体的句柄创建事件
                form.BindingContextChanged += delegate
                {
                    //启动新线程来显示Splash窗体
                    new Thread(new ThreadStart(delegate
                    {
                        SplashView splash = new SplashView();
                        //订阅主窗体的Shown事件
                        form.Shown += delegate
                        {
                            //通知Splash窗体关闭自身
                            splash.Invoke(new EventHandler(splash.KillMe));
                            splash.Dispose();
                        };
                        //显示Splash窗体
                        Application.Run(splash);
                    })).Start();
                };
                //显示主窗体
                Application.Run(form);
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("LoadAndRun", ex.Message);
            }
           
        }

        public static void ShowStartStaus(string strLog)
        {
            try
            {
                if(m_view1 != null)
                {
                    m_view1.BeginInvoke(new Action(() =>
                    {
                        m_view1.labelCompany.Text = strLog;
                    }));
                }
            }
            catch (Exception ex)
            {
                 
            }
            
        }

        private void SplashView_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_bShow = false;
        }




    }
}

