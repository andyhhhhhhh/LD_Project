using Huxi;
using SequenceTestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionController
{
    public class SoftPlcService
    {
        IMotor[] m_motor = new IMotor[12];
        IDigtal[] m_io = new IDigtal[2];

        public static SoftPlcService softPlcService;
        public static SoftPlcService Instance()
        {
            if(softPlcService == null)
            {
                softPlcService = new SoftPlcService();
            }

            return softPlcService;
        }

        /// <summary>
        /// 控制卡初始化
        /// </summary>
        public bool Init()
        {
            //得到上下文对象
            IContext context = ContextFactory.instance().createContext("rt");
            long value = context.getHeartbeat();
            bool bvalue = context.initialize();
            for (uint i = 0; i < m_motor.Length; i++)
            {
                //创建一个电机
                m_motor[i] = context.createMotor(i + 1);
            }

            for (uint i = 0; i < m_io.Length; i++)
            {
                //创建一个IO对象
                m_io[i] = context.createDigtal(i + 1); 
            }

            //设置急停IO
            var emgIo = XMLController.XmlControl.controlCardModel.IOModels.FirstOrDefault(x => x.enumIoType == EnumIOType.急停IO);
            if(emgIo != null)
            { 
                bvalue = context.setStopIo(m_io[0], emgIo.index + 1, true);
            }
            return bvalue;
        }

        /// <summary>
        /// 轴回零
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <returns>返回结果</returns>
        public bool AxisHome(int axis, uint homeIo, float speed, float secondSpeed)
        {
            //m_motor[axis].setLimit();
            return m_motor[axis].findHomeByIO((uint)homeIo, speed, secondSpeed, 4); 
        }

        /// <summary>
        /// 轴上使能
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <returns>返回结果</returns>
        public bool AxisEnable(int axis)
        {
            return m_motor[axis].enable();
        }

        /// <summary>
        /// 轴断使能
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <returns>返回结果</returns>
        public bool AxisDisable(int axis)
        {
            return m_motor[axis].disable();
        }

        /// <summary>
        /// 轴移动绝对位置
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="pos">位置</param>
        /// <param name="pspeed">速度</param>
        /// <returns>返回结果</returns>
        public bool AxisMovePos(ushort axisIndex, double pos, TSpeed pspeed)
        {
            return m_motor[axisIndex].moveAbs((float)pos, (ulong)pspeed.vel, (ulong)pspeed.acc, (ulong)pspeed.dec, 1000);
        }

        /// <summary>
        /// 轴移动相对位置
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="pos">位置</param>
        /// <param name="pspeed">速度</param>
        /// <returns>返回结果</returns>
        public bool AxisMoveRelPos(ushort axisIndex, double pos, TSpeed pspeed)
        {
            return m_motor[axisIndex].move((float)pos, (ulong)pspeed.vel, (ulong)pspeed.acc, (ulong)pspeed.dec, 1000);
        }

        /// <summary>
        /// 轴连续运动
        /// </summary>
        /// <param name="axisIndex">轴号</param>
        /// <param name="dir">0 正向 1 负向</param>
        /// <param name="pspeed">速度</param>
        /// <returns>返回结果</returns>
        public bool AxisMoveJog(ushort axisIndex, int dir, TSpeed pspeed)
        {
            float vel = dir == 0 ? (float)pspeed.vel : ((float)pspeed.vel * -1);
            return m_motor[axisIndex].jog(vel, (float)pspeed.acc, (float)pspeed.dec, 1000);
        }

        /// <summary>
        /// 轴停止运动
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <returns>返回结果</returns>
        public bool AxisStop(int axis)
        {
            return m_motor[axis].stop(); 
        }

        /// <summary>
        /// 轴复位
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <returns>返回结果</returns>
        public bool AxisReset(int axis)
        {
            return m_motor[axis].reset();
        }

        /// <summary>
        /// 设置零点
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <returns>返回结果</returns>
        public bool AxisSetHome(int axis)
        {
            return m_motor[axis].setMcs(0);
        }

        /// <summary>
        /// 获取轴实际位置
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="pval">输出位置</param>
        /// <returns>返回结果</returns>
        public bool AxisGetPos(int axis, out double pval)
        {
            bool bok;
            var stat = m_motor[axis].stat(out bok);
            pval = Math.Round(stat.Mcs, 3);
            return bok;
        }

        /// <summary>
        /// 获取轴编码器位置
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="pval">输出位置</param>
        /// <returns>返回结果</returns>
        public bool AxisGetEncodePos(int axis, out double pval)
        {
            bool bok;
            var stat = m_motor[axis].stat(out bok);
            pval = Math.Round(stat.Acs, 3);
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
            return m_io[cardIndex].setOutBit(ch + 1, value == 1);
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
            bool breturn = m_io[cardIndex].getInBit(ch + 1, out bok);
            if(!bok)
            {
                return -1;
            }

            if(breturn)
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
            bool breturn = m_io[cardIndex].getOutBit(ch + 1, out bok);
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
        public AxisStatus GetAxisStatus(int axis, out bool bok)
        {
            AxisStatus axisStatus = new AxisStatus();
            MoterStat axisstat =  m_motor[axis].stat(out bok);
            if(bok)
            {
                axisStatus.homed = axisstat.homed;
                axisStatus.inited = axisstat.inited;
                axisStatus.enabled = axisstat.enabled;
                axisStatus.warning = axisstat.warning;
                axisStatus.pLimited = axisstat.pLimited;
                axisStatus.nLimited = axisstat.nLimited;
                axisStatus.planning = axisstat.planning;
                axisStatus.reached = axisstat.reached;
                axisStatus.Acs = (float)Math.Round(axisstat.Acs, 3);
                axisStatus.Mcs = (float)Math.Round(axisstat.Mcs, 3);
                axisStatus.ActVel = axisstat.ActVel;
                axisStatus.ActTorque = axisstat.ActTorque;
                axisStatus.FollowingErr = axisstat.FollowingErr;
            }

            return axisStatus;
        }

    }
}
