using BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceTestModel
{
    /// <summary>
    /// 运动参数 定义
    /// </summary>
    public class MotionParam
    {
        //模组命名
        public const string Station_PrePare = "备料模组";
        public const string Station_Up = "顶升模组";
        public const string Station_LoadX = "上料模组X";
        public const string Station_LoadZ = "上料模组Z";
        public const string Station_Camera = "双目相机模组";
        public const string Station_Check = "检测模组";
        public const string Station_UnLoad = "下料模组";
        public const string Station_UnLoadZ = "下料模组Z";
        public const string Station_EmptyTray = "空料盘模组";
        public const string Station_TakeTray = "取料盘模组";

        public const int AxisZ1No = 0;

        //位置命名
        //备料模组
        public const string Pos_Safe = "安全位";
        public const string Pos_GetTray = "送料盘位";
        public const string Pos_BigCamera = "大视野相机拍照位";

        //双目相机
        public const string Pos_SmallCamera = "小视野相机拍照位";

        //生成位置
        public const string Pos_BigCameraOffSet = "大视野相机偏移位";
        public const string Pos_SmallCameraCenter = "小视野相机中心位";
        public const string Pos_SmallGet = "小视野产品位";
        public const string Pos_ProFixed = "产品定位位置";

        //顶升模组
        public const string Pos_SuckFilm = "吸膜位";
        public const string Pos_ThimbleRise = "顶针上升位";
        public const string Pos_ThimbleDown = "顶针下降位";

        //上料模组
        public const string Pos_Load = "上料位";
        public const string Pos_NSnap = "N面拍照位";
        public const string Pos_UnLoad = "下料位";
        public const string Pos_UnLoadWait = "下料等待位";
         
        //检测模组
        public const string Pos_Correct1 = "上料位置校正1";
        public const string Pos_Correct2 = "上料位置校正2";
        public const string Pos_ARCheck = "AR检测位";
        public const string Pos_HRCheck = "HR检测位";

        public const string Pos_ARFixed = "AR定位";
        public const string Pos_HRFixed = "HR定位";
        public const string Pos_LoadFixed = "上料纠正位置";
        public const string Pos_UnLoadFixed = "下料纠正位置";

        //下料模组
        public const string Pos_Take = "取料位";
        public const string Pos_Put_1 = "放料位1";
        public const string Pos_Put_2 = "放料位2";
        public const string Pos_Put_3 = "放料位3";
        public const string Pos_Put_4 = "放料位4";
        public const string Pos_Put_5 = "放料位5";
        public const string Pos_Put_6 = "放料位6";
        public const string Pos_Put_7 = "放料位7";
        public const string Pos_Put_8 = "放料位8";
        public const string Pos_Put_9 = "放料位9";
        public const string Pos_Put_10 = "放料位10";
        public const string Pos_Put_11 = "放料位11";
        public const string Pos_Put_12 = "放料位12";
        public const string Pos_Put_13 = "放料位13";
        public const string Pos_Put_14 = "放料位14";
        public const string Pos_Put_15 = "放料位15";
        public const string Pos_Put_16 = "放料位16";


        //DI
        public const string DI_Emergey = "急停";
        public const string DI_Start = "启动按钮";
        public const string DI_Stop = "停止按钮";
        public const string DI_Door = "安全门";
        public const string DI_Load1CylinderUnClamp = "上料平台1夹紧气缸张开到位";
        public const string DI_Load1CylinderClamp = "上料平台1夹紧气缸夹紧到位";
        public const string DI_Load2CylinderUnClamp = "上料平台2夹紧气缸张开到位";
        public const string DI_Load2CylinderClamp = "上料平台2夹紧气缸夹紧到位";
        public const string DI_LoadVaccum = "上料机械手真空检测";
        public const string DI_UnLoadVaccum = "下料机械手真空检测";
        public const string DI_TakeMotorReach = "取料步进到位光电"; 
        public const string DI_EmptyMotorReach = "空盘步进到位光电";

        public const string DI_LoadInComing = "上料平台来料检测";
        public const string DI_Get1CylinderUnClamp = "取产品1夹紧气缸张开到位";
        public const string DI_Get1CylinderClamp = "取产品1夹紧气缸夹紧到位";
        public const string DI_Get2CylinderUnClamp = "取产品2夹紧气缸张开到位";
        public const string DI_Get2CylinderClamp = "取产品2夹紧气缸夹紧到位";

        //DO
        public const string DO_StartLight = "启动-灯";
        public const string DO_StopLight = "停止-灯";
        public const string DO_Beep = "蜂鸣器";
        public const string DO_RedLight = "警示灯-红灯";
        public const string DO_YellowLight = "警示灯-黄灯";
        public const string DO_GreenLight = "警示灯-绿灯";
        public const string DO_LoadBreakVacuum = "上料机械手吸嘴破真空";
        public const string DO_UnLoadBreakVacuum = "下料机械手吸嘴破真空";
        public const string DO_GetSuctionVacuum = "取料位吸蓝膜真空发生器";
        public const string DO_CheckCCDVacuum = "CCD检测真空发生器";
        public const string DO_HighGas= "测高传感器气源";
        public const string DO_LoadSuctionVacuum = "上料机械手吸嘴真空";
        public const string DO_UnLoadSuctionVacuum = "下料机械手吸嘴真空";
        public const string DO_LoadCylinderUnClamp = "上料平台夹紧气缸张开";
        public const string DO_GetCylinderClamp = "取产品位夹紧气缸夹紧";
        public const string DO_LoadCylinderClamp = "上料平台夹紧气缸夹紧";
        public const string DO_GetCylinderUnClamp = "取产品位夹紧气缸张开";
    }

    /// <summary>
    /// 轴状态
    /// </summary>
    public class AxisStatus
    {
        /// <summary>
        /// 回零状态
        /// </summary>
        public bool homed { get; set; }
        /// <summary>
        /// 是否已经初始化
        /// </summary>
        public bool inited { get; set; }
        /// <summary>
        /// 是否已经使能
        /// </summary>
        public bool enabled { get; set; }
        /// <summary>
        /// 是否已经报警
        /// </summary>
        public bool warning { get; set; }
        /// <summary>
        /// 触发正限位
        /// </summary>
        public bool pLimited { get; set; }
        /// <summary>
        /// 触发负限位
        /// </summary>
        public bool nLimited { get; set; }
        /// <summary>
        /// 运动规划中
        /// </summary>
        public bool planning { get; set; }
        /// <summary>
        /// 运动到位
        /// </summary>
        public bool reached { get; set; }
        /// <summary>
        /// 编码器值
        /// </summary>
        public float Acs { get; set; }
        /// <summary>
        /// 实际值
        /// </summary>
        public float Mcs { get; set; } 
        /// <summary>
        /// 实际位置
        /// </summary>
        public float ActVel { get; set; }
        /// <summary>
        /// 实际扭矩
        /// </summary>
        public float ActTorque { get; set; }
        /// <summary>
        /// 跟随误差
        /// </summary>
        public float FollowingErr { get; set; }
    }

    /// <summary>
    /// 吸嘴
    /// </summary>
    public enum ESuck
    {
        吸嘴1,
        吸嘴2,
        吸嘴3,
    }

    public enum EThreeLight
    {
        运行,
        暂停,
        报警
    }
}
