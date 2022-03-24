using Huxi;
using Huxi.Move;
using SequenceTestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MotionController
{
    public class ZMotionService
    {
        IMotor[] m_motor = new IMotor[15];
        IDigtal[] m_io = new IDigtal[2];
        IContext context;
        //步进电机
        IMotor[] m_motorStep = new IMotor[8];

        public static ZMotionService zMotionService;
        public static ZMotionService Instance()
        {
            if (zMotionService == null)
            {
                zMotionService = new ZMotionService();
            }

            return zMotionService;
        }

        /// <summary>
        /// 控制卡初始化
        /// </summary>
        public bool Init()
        {
            //得到上下文对象
            context = ContextFactory.instance().createContext("zmotion");
            ContextFactory.EnableLog(false);
            long value = context.getHeartbeat();
            string configPath = Environment.CurrentDirectory + "\\Config\\config.xml";
            bool bvalue = context.initialize(configPath);
            if(!bvalue)
            {
                Thread.Sleep(3000);
                bvalue = context.initialize(configPath);
            }
            for (uint i = 0; i < m_motor.Length; i++)
            {
                //创建一个电机
                m_motor[i] = context.createMotor(i);
            }

            for (uint i = 0; i < m_io.Length; i++)
            {
                //创建一个IO对象
                m_io[i] = context.createDigtal(i);
            }

            //设置急停IO
            var emgIo = XMLController.XmlControl.controlCardModel.IOModels.FirstOrDefault(x => x.enumIoType == EnumIOType.急停IO);
            if (emgIo != null)
            {
                //bvalue = context.setStopIo(m_io[0], emgIo.index + 1, true);
            }
            return bvalue;
        }

        /// <summary>
        /// 轴回零参数
        /// </summary>
        /// <param name="cardIndex">卡号</param>
        /// <param name="axis">轴号</param>
        /// <param name="homeIo">回零IO</param>
        /// <param name="speed">寻零速度</param>
        /// <param name="secondSpeed">二段速度</param>
        /// <param name="homeType">回零类型 0--普通回零 1--捕获回零</param>
        /// <param name="limitType">回限位类型 0--正限位 1--负限位 2--无限位</param>
        /// <returns></returns>
        public bool AxisHome(ushort cardIndex, int axis, uint homeIo, float speed, float secondSpeed, int homeType, int limitType)
        {
            if(limitType == 2)
            {
                return GetMotor(cardIndex, axis).findHomeByDriver(0, speed, secondSpeed, speed * 10, homeType);
            }
            else
            {
                return GetMotor(cardIndex, axis).findHomeByIO(2, speed, secondSpeed, homeType);
            }
        }

        /// <summary>
        /// 轴上使能
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <returns>返回结果</returns>
        public bool AxisEnable(ushort cardIndex, int axis)
        {
            return GetMotor(cardIndex, axis).enable();
        }

        /// <summary>
        /// 轴断使能
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <returns>返回结果</returns>
        public bool AxisDisable(ushort cardIndex, int axis)
        {
            return GetMotor(cardIndex, axis).disable();
        }

        /// <summary>
        /// 轴移动绝对位置
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="pos">位置</param>
        /// <param name="pspeed">速度</param>
        /// <returns>返回结果</returns>
        public bool AxisMovePos(ushort cardIndex, ushort axis, double pos, TSpeed pspeed)
        {
            return GetMotor(cardIndex, axis).moveAbs((float)pos, (ulong)pspeed.vel, (ulong)pspeed.acc, (ulong)pspeed.dec, (float)pspeed.aacc);
        }

        /// <summary>
        /// 插补运动
        /// </summary>
        /// <param name="listaxis"></param>
        /// <param name="listpos"></param>
        /// <param name="Tspeed"></param>
        /// <returns></returns>
        public bool GroupMovePos(List<ushort> listaxis, List<float> listpos, TSpeed Tspeed, int stationIndex)
        {
            //step1:创建轴
            List<IMotor> motors = new List<IMotor>();
            foreach (var axisIndex in listaxis)
            {
                motors.Add(m_motor[axisIndex]);
            }

            //step3:移动
            LinearMotorGroup motorGroup = new LinearMotorGroup(motors, stationIndex);
            //LinearMotorGroup motorGroup = new LinearMotorGroup(motors);
            LinearMoveGroup moveGroup = context.createLinearMoveGroup(motorGroup);
            //移动到1000，1000位置
            bool bresult = moveGroup.move(listpos, (ulong)Tspeed.vel, (ulong)Tspeed.acc, (ulong)Tspeed.dec, (float)Tspeed.aacc);

            //step4：判断移动结束
            if (moveGroup.finished())
            {

            }

            return bresult;
        }

        /// <summary>
        /// 轴移动相对位置
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="pos">位置</param>
        /// <param name="pspeed">速度</param>
        /// <returns>返回结果</returns>
        public bool AxisMoveRelPos(ushort cardIndex, ushort axis, double pos, TSpeed pspeed)
        {
            return GetMotor(cardIndex, axis).move((float)pos, (ulong)pspeed.vel, (ulong)pspeed.acc, (ulong)pspeed.dec, (float)pspeed.aacc);
        }

        /// <summary>
        /// 轴连续运动
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="dir">0 正向 1 负向</param>
        /// <param name="pspeed">速度</param>
        /// <returns>返回结果</returns>
        public bool AxisMoveJog(ushort cardIndex, ushort axis, int dir, TSpeed pspeed)
        {
            float vel = dir == 0 ? (float)pspeed.vel : ((float)pspeed.vel * -1);

            return GetMotor(cardIndex, axis).jog(vel, (float)pspeed.acc, (float)pspeed.dec, (float)pspeed.aacc);
        }

        /// <summary>
        /// 轴停止运动
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <returns>返回结果</returns>
        public bool AxisStop(ushort cardIndex, int axis)
        {
            return GetMotor(cardIndex, axis).stop();
        }

        /// <summary>
        /// 轴复位
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <returns>返回结果</returns>
        public bool AxisReset(ushort cardIndex, int axis)
        {
            return GetMotor(cardIndex, axis).reset();
        }

        /// <summary>
        /// 设置零点
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <returns>返回结果</returns>
        public bool AxisSetHome(ushort cardIndex, int axis)
        {
            return GetMotor(cardIndex, axis).setMcs(0);
        }

        /// <summary>
        /// 获取轴实际位置
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="pval">输出位置</param>
        /// <returns>返回结果</returns>
        public bool AxisGetPos(ushort cardIndex, int axis, out double pval)
        {
            bool bok;

            var stat = GetMotor(cardIndex, axis).stat(out bok);
            pval = Math.Round(stat.Mcs, 2);
            return bok;
        }

        /// <summary>
        /// 获取轴编码器位置
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="pval">输出位置</param>
        /// <returns>返回结果</returns>
        public bool AxisGetEncodePos(ushort cardIndex, int axis, out double pval)
        {
            bool bok;

            var stat = GetMotor(cardIndex, axis).stat(out bok);
            pval = Math.Round(stat.Acs, 2);
            return bok;
        }

        /// <summary>
        /// 输出IO
        /// </summary>
        /// <param name="cardIndex">卡号</param>
        /// <param name="ch">通道</param>
        /// <param name="value">值 1/0</param>
        /// <returns></returns>
        public bool IOWriteOutBit(int cardIndex, uint ch, int value)
        {
            return m_io[cardIndex].setOutBit(ch, value == 1);
        }

        /// <summary>
        /// 读取输入IO
        /// </summary>
        /// <param name="cardIndex">卡号</param>
        /// <param name="ch">通道</param>
        /// <returns></returns>
        public int IOReadInBit(int cardIndex, uint ch)
        {
            bool bok;
            if(m_io[cardIndex] == null)
            {
                return -1;
            }
            bool breturn = m_io[cardIndex].getInBit(ch, out bok);
            if (!bok)
            {
                return -1;
            }

            if (breturn)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 读取输出IO
        /// </summary>
        /// <param name="cardIndex">卡号</param>
        /// <param name="ch">通道</param>
        /// <returns></returns>
        public int IOReadOutBit(int cardIndex, uint ch)
        {
            bool bok;
            if(m_io[cardIndex] == null)
            {
                return -1;
            }

            bool breturn = m_io[cardIndex].getOutBit(ch, out bok);
            if (!bok)
            {
                return -1;
            }

            if (breturn)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取轴状态
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="bok">是否OK</param>
        /// <returns></returns>
        public AxisStatus GetAxisStatus(ushort cardIndex, int axis, out bool bok)
        {
            bok = false;
            AxisStatus axisStatus = new AxisStatus();

            if(GetMotor(cardIndex, axis) == null)
            {
                return axisStatus;
            }

            MoterStat axisstat = GetMotor(cardIndex, axis).stat(out bok);

            if (bok)
            {
                axisStatus.homed = axisstat.homed;
                axisStatus.inited = axisstat.inited;
                axisStatus.enabled = axisstat.enabled;
                axisStatus.warning = axisstat.warning;
                axisStatus.pLimited = axisstat.pLimited;
                axisStatus.nLimited = axisstat.nLimited;
                axisStatus.planning = axisstat.planning;
                axisStatus.reached = axisstat.reached;
                axisStatus.Acs = (float)Math.Round(axisstat.Acs, 2);
                axisStatus.Mcs = (float)Math.Round(axisstat.Mcs, 2);
                axisStatus.ActVel = axisstat.ActVel;
                axisStatus.ActTorque = axisstat.ActTorque;
                //axisStatus.FollowingErr = axisstat.FollowingErr;
            }

            return axisStatus;
        }

        private IMotor GetMotor(ushort cardIndex, int axisIndex)
        {
            try
            {
                if (cardIndex == 0)
                {
                    return m_motor[axisIndex];
                }
                else
                {
                    return m_motorStep[axisIndex];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
