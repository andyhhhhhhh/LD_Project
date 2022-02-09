using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonController
{
    public class Systemparameters
    {
        //保存路径
        private string _SavePath;

        public string SavePath
        {
            get { return _SavePath; }
            set { _SavePath = value; }
        }

        //设定保存天数
        private int _SaveDays;

        public int SaveDays
        {
            get { return _SaveDays; }
            set { _SaveDays = value; }
        }

        //保存大小
        private int _MaxCapity;

        public int MaxCapity
        {
            get { return _MaxCapity; }
            set { _MaxCapity = value; }
        }

        //操作员密码
        private string _OpPassword;
        public string OpPassword
        {
            get { return _OpPassword; }
            set { _OpPassword = value; }
        }

        //管理员密码
        private string _MgPassword;
        public string MgPassword
        {
            get { return _MgPassword; }
            set { _MgPassword = value; }
        }

        //管理员密码
        private string _EnPassword;
        public string EnPassword
        {
            get { return _EnPassword; }
            set { _EnPassword = value; }
        }

        //超级密码
        private string _SuperPassword;
        public string SuperPassword
        {
            get { return _SuperPassword; }
            set { _SuperPassword = value; }
        }

        //是否开机启动
        private bool _OpenAutonRun;

        public bool OpenAutonRun
        {
            get { return _OpenAutonRun; }
            set { _OpenAutonRun = value; }
        }
        
        private bool _MinToPallet;
        /// <summary>
        /// 是否最小化到托盘
        /// </summary>
        public bool MinToPallet
        {
            get { return _MinToPallet; }
            set { _MinToPallet = value; }
        } 
        
        private bool _OpenMany;
        /// <summary>
        /// 是否允许打开多个程序
        /// </summary>
        public bool OpenMany
        {
            get { return _OpenMany; }
            set { _OpenMany = value; }
        }

        private bool _EnableOsk;
        /// <summary>
        /// 是否显示软键盘
        /// </summary>
        public bool EnableOsk
        {
            get { return _EnableOsk; }
            set { _EnableOsk = value; }
        }

        private bool _EnableRunView;
        /// <summary>
        /// 是否启动运行界面
        /// </summary>
        public bool EnableRunView
        {
            get { return _EnableRunView; }
            set { _EnableRunView = value; }
        }

        private bool _IsRunOtherSoft;
        /// <summary>
        /// 是否启动另外一个程序
        /// </summary>
        public bool IsRunOtherSoft
        {
            get { return _IsRunOtherSoft; }
            set { _IsRunOtherSoft = value; }
        }

        private string _OtherSoftAddr;
        /// <summary>
        /// 第三方程序地址
        /// </summary>
        public string OtherSoftAddr
        {
            get { return _OtherSoftAddr; }
            set { _OtherSoftAddr = value; }
        }

        private bool _IsPrintLog;
        /// <summary>
        /// 是否只打印重要Log
        /// </summary>
        public bool IsPrintLog
        {
            get { return _IsPrintLog; }
            set { _IsPrintLog = value; }
        }

        private bool _IsInitDevice;
        /// <summary>
        /// 启动是否初始化设备
        /// </summary>
        public bool IsInitDevice
        {
            get { return _IsInitDevice; }
            set { _IsInitDevice = value; }
        } 

        private int _ViewNum;
        /// <summary>
        /// 窗体数量
        /// </summary>
        public int ViewNum
        {
            get { return _ViewNum; }
            set { _ViewNum = value; }
        }

        private bool _IsItemVisible;
        /// <summary>
        /// 测试项目是否折叠
        /// </summary>
        public bool IsItemVisible
        {
            get { return _IsItemVisible; }
            set { _IsItemVisible = value; }
        }

        private bool _IsProVisible;
        /// <summary>
        /// 测试模块是否课件
        /// </summary>
        public bool IsProVisible
        {
            get { return _IsProVisible; }
            set { _IsProVisible = value; }
        }

        private bool _IsRealDisplay;
        /// <summary>
        /// 是否相机实时显示
        /// </summary>
        public bool IsRealDisplay
        {
            get { return _IsRealDisplay; }
            set { _IsRealDisplay = value; }
        }

        private bool _IsShowCross;
        /// <summary>
        /// 图像窗口是否显示十字线
        /// </summary>
        public bool IsShowCross
        {
            get { return _IsShowCross; }
            set { _IsShowCross = value; }
        }

        private bool _IsStopShowMsg;
        /// <summary>
        /// 点击停止按钮先提示
        /// </summary>
        public bool IsStopShowMsg
        {
            get { return _IsStopShowMsg; }
            set { _IsStopShowMsg = value; }
        }

        public string _ResetProcess;
        /// <summary>
        /// 复位流程
        /// </summary>
        public string ResetProcess
        {
            get { return _ResetProcess; }
            set { _ResetProcess = value; }
        }

        public string _MainProcess;
        /// <summary>
        /// 复位流程
        /// </summary>
        public string MainProcess
        {
            get { return _MainProcess; }
            set { _MainProcess = value; }
        }

        public string _MotionCard;
        /// <summary>
        /// 运动控制卡分类
        /// </summary>
        public string MotionCard
        { 
            get { return _MotionCard; }
            set { _MotionCard = value; }
        }
        
        private bool _IsSaveImg;
        /// <summary>
        /// 是否保存图片
        /// </summary>
        public bool IsSaveImg
        {
            get { return _IsSaveImg; }
            set { _IsSaveImg = value; }
        }

        private bool _IsSaveNGImg;
        /// <summary>
        /// 是否只保存NG图片
        /// </summary>
        public bool IsSaveNGImg
        {
            get { return _IsSaveNGImg; }
            set { _IsSaveNGImg = value; }
        }

        private string _ImagePath;
        /// <summary>
        /// 保存图片路径
        /// </summary>
        public string ImagePath
        {
            get { return _ImagePath; }
            set { _ImagePath = value; }
        }
        
        private bool _IsFullScreen;
        /// <summary>
        /// 是否全屏显示软件
        /// </summary>
        public bool IsFullScreen
        {
            get { return _IsFullScreen; }
            set { _IsFullScreen = value; }
        }

        public Systemparameters()
        {
            this.IsSaveImg = false;
            this.IsSaveNGImg = false;
            this.ImagePath = "";
            this.SaveDays = 3;
            this.SavePath = "";
            this.MaxCapity = 5000;
            this.SuperPassword = "daschen8888";
            this.EnableOsk = false;
            this.EnableRunView = false;
            this.IsPrintLog = false;
            this.IsInitDevice = true;
            this.IsItemVisible = true;
            this.IsProVisible = true;
            this.IsRealDisplay = false;
            this.IsShowCross = false;
            this.IsStopShowMsg = false;
            this.IsFullScreen = false;
            this.ViewNum = 1;
        }

    }
}
