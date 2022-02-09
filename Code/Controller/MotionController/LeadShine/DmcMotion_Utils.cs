using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionController
{
    public class DmcMotion_Utils
    {
        public const ushort AxisNum = 0;
        public const UInt16 SERVE_ON = 0;       //伺服使能
        public const UInt16 SERVE_OFF = 1;       //伺服OFF
        public const bool TMode = true;
        public const bool SMode = false;
        public const int ON = 0;
        public const int OFF = 1;
        public const int SENSORID = 3;
        public const int BLOW1 = 3;
        public const int BLOW2 = 4;
        public const int BLOW3 = 5;
        public const int BLOW4 = 6;
        public const int BLOW5 = 7;

        public const int STARTLIGHT = 1;
        public const int STOPLIGHT = 2;

        public const int Mode_Releative = 0;//相对位移
        public const int Mode_Absolute = 1;//绝对位移


        public struct MotionStrategy
        {
            public double Min_Vel;       //初速度
            public double Max_Vel;       //最大速度
            public double EndVel;       //停止速度
            public double Tac;         //加速度
            public double S_Tac;         //减速度
            public double Curve;        //加速曲线
        }; //运动参数
    }
}
