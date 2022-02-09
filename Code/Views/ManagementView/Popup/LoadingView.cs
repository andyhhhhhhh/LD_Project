using ManagementView._3DViews;
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
    public partial class LoadingView : Form
    {
        public LoadingView()
        {
            InitializeComponent();
        }

        private void LoadingView_Load(object sender, EventArgs e)
        { 
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
                form.HandleCreated += delegate
                {
                    //启动新线程来显示Splash窗体
                    new Thread(new ThreadStart(delegate
                    {
                        LoadingView splash = new LoadingView();
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
                //Application.Run(form);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                CommFuncView.ShowErrMsg("LoadAndRun", ex.Message);
            } 
        }

       
    }
}
