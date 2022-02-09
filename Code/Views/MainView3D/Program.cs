using ManagementView._3DViews;
using ManagementView.Popup;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainView3D
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //增加重复启动导致多个程序运行ManagementView._3DViews
             
            if (!MainForm.m_jsonControl.SystemPara.OpenMany)
            {
                Process instance = RunningInstance();
                if (instance == null)
                {
                    MainForm.m_ShowStarStatus += SplashView.ShowStartStaus;
                    //LoadingView.LoadAndRun(new NewViewForm());
                    SplashView.LoadAndRun(new MainForm());//增加欢迎界面
                }
                else
                {
                    HandleRunningInstance(instance);
                }
            }
            else
            {
                MainForm.m_ShowStarStatus += SplashView.ShowStartStaus;
                SplashView.LoadAndRun(new MainForm());//增加欢迎界面
            }
        }
        
        #region 确保程序只运行一个实例
        //在进程中查找是否已经有实例在运行
        public static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            //遍历与当前进程名称相同的进程列表 
            foreach (Process process in processes)
            {
                //如果实例已经存在则忽略当前进程 
                if (process.Id != current.Id)
                {
                    //保证要打开的进程同已经存在的进程来自同一文件路径
                    if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName)
                    {
                        //返回已经存在的进程
                        return process;
                    }
                }
            }
            return null;
        }

        //已经有了就把它激活，并将其窗口放置最前端
        public static void HandleRunningInstance(Process instance)
        {
            ShowWindowAsync(instance.MainWindowHandle, 1); //调用api函数，正常显示窗口
            SetForegroundWindow(instance.MainWindowHandle); //将窗口放置最前端
        }
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(System.IntPtr hWnd, int cmdShow);
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(System.IntPtr hWnd);
        #endregion
    }
}
