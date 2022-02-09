using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MotionController
{
    public class DmcMotorControl
    {
        public static bool IsInit = false;
        static int Min_Vel = 3000;//初速度
        static int Max_Vel = 5000;//运行速度
        static double Tac = 0.2;  //加速时间
        static double S_Tac = 0.05;//S段时间
        /// <summary>
        /// 初始化卡
        /// </summary>
        /// <returns></returns>
        public bool InitCard()
        {
            if(IsInit)
            {
                return true;
            }
            //初始化卡
            int n;
            n = Dmc2210.d2210_board_init(); //初始化控制卡
            if (n < 1 || n > 8)
            {
                //MessageBox.Show("未找到控制卡", "出错");
                return false;
            }
            //设置初始默认时间

            //设置轴运动必须使以下状态为有效
            ServoOn(DmcMotion_Utils.AxisNum, DmcMotion_Utils.SERVE_ON);
            Dmc2210.d2210_config_ALM_PIN(0, 1, 0);
            Dmc2210.d2210_config_EZ_PIN(0, 0, 0);
            Dmc2210.d2210_config_EMG_PIN(0, 1, 1);
            IsInit = true;
            return true;
        }

        /// <summary>
        /// 设置速度
        /// </summary>
        /// <param name="min_Vel">初速度</param>
        /// <param name="max_Vel">运行速度</param>
        /// <param name="tac">加速时间</param>
        /// <param name="s_Tac">S段时间</param>
        public void SetParam(int min_Vel, int max_Vel, double tac, double s_Tac)
        {
            Min_Vel = min_Vel;//初速度
            Max_Vel = max_Vel;//运行速度
            Tac = tac;  //加速时间
            S_Tac = s_Tac;//S段时间
        }

        /// <summary>
        /// 关闭卡
        /// </summary>
        public void CloseCard()
        {
            Dmc2210.d2210_board_close(); //关闭控制卡
        }

        /// <summary>
        /// 紧急停止
        /// </summary>
        public void CardEmgStop()
        {
            Dmc2210.d2210_emg_stop();
        }

        /// <summary>
        /// 加速停止
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="tacc"></param>
        public void Decelstop(ushort axis, double tacc)
        {
            Dmc2210.d2210_decel_stop(axis, tacc);
        }

        /// <summary>
        /// 轴位置清0
        /// </summary>
        /// <param name="axis"></param>
        public void ClearPosition(ushort axis)
        {
            Dmc2210.d2210_set_position(axis, 0);
        }

        /// <summary>
        /// 轴运动T或者S型
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="dist">移动位移</param>
        /// <param name="posi_mode">0:相对位移 1:绝对位移</param>
        /// <param name="moveMode">true:T型运动 false:S型运动</param>
        public void SingleMoveLineDim(ushort axis, int dist, ushort posi_mode, bool moveMode)
        {
            DmcMotion_Utils.MotionStrategy nSg;
            nSg.Min_Vel = Min_Vel;//初速度
            nSg.Max_Vel = Max_Vel;//运行速度
            nSg.Tac = Tac;  //加速时间
            nSg.S_Tac = S_Tac;//S段时间

            Dmc2210.d2210_set_pulse_outmode(0, 0);  //设置控制卡轴的脉冲输出模式
            Dmc2210.d2210_set_pulse_outmode(1, 0);

            if (moveMode)
            {
                Dmc2210.d2210_set_profile(0, nSg.Min_Vel, nSg.Max_Vel, nSg.Tac, nSg.Tac);
                Dmc2210.d2210_t_pmove(0, dist, posi_mode);
            }
            else
            {
                Dmc2210.d2210_set_st_profile(0, nSg.Min_Vel, nSg.Max_Vel, nSg.Tac, nSg.Tac, nSg.S_Tac, nSg.S_Tac);
                Dmc2210.d2210_s_pmove(0, dist, posi_mode);
            }

        }

        /// <summary>
        /// 轴连续运动
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="dir">运动方向 1:正向 0:反向</param>
        /// <param name="moveMode">true:T型运动 false:S型运动</param>
        public void CardContinueMove(ushort axis, ushort dir, bool moveMode)
        {
            DmcMotion_Utils.MotionStrategy nSg;
            nSg.Min_Vel = Min_Vel;
            nSg.Max_Vel = Max_Vel;
            nSg.Tac = Tac;
            nSg.S_Tac = S_Tac;

            Dmc2210.d2210_set_pulse_outmode(axis, 0); //脉冲模式选择

            if (moveMode)
            {
                Dmc2210.d2210_set_profile(axis, nSg.Min_Vel, nSg.Max_Vel, nSg.Tac, nSg.Tac); //运行参数
                Dmc2210.d2210_t_vmove(axis, dir);              //T型连续运动
            }
            else
            {
                Dmc2210.d2210_set_st_profile(axis, nSg.Min_Vel, nSg.Max_Vel, nSg.Tac, nSg.Tac, nSg.S_Tac, nSg.S_Tac);
                Dmc2210.d2210_s_vmove(axis, dir);             //S型连续运动
            }

        }

        /// <summary>
        /// 轴使能或者断电
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="onoff"></param>
        public void ServoOn(ushort axis, ushort onoff)
        {
            Dmc2210.d2210_write_SEVON_PIN(axis, onoff);
        }

        /// <summary>
        /// 写DO
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="ioNum"></param>
        /// <param name="onoff"></param>
        public void WriteGeneralDoOutput(ushort axis, int ioNum, ushort onoff)
        {
            Dmc2210.d2210_write_outbit(axis, (ushort)ioNum, onoff);
        }

        /// <summary>
        /// 读取DI
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="ioNum"></param>
        /// <returns></returns>
        public int ReadSingleGeneralDiInput(ushort axis, ushort ioNum)
        {
            int iStatus = Dmc2210.d2210_read_inbit(axis, ioNum);

            return iStatus;
        }

        /// <summary>
        /// 检测是否运动完成
        /// </summary>
        /// <param name="axis"></param>
        /// <returns>1:停止 0:运动</returns>
        public bool CheckDone(ushort axis)
        {
            int nDone = Dmc2210.d2210_check_done(axis);
            bool bDone = Convert.ToBoolean(nDone);

            return bDone;
        }

        /// <summary>
        /// 在线更改速度
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="curr_vel"></param>
        public void ChangeSpeed(ushort axis, double curr_vel)
        {
            Dmc2210.d2210_variety_speed_range(axis, 1, 10000000); //变速使能
            Dmc2210.d2210_change_speed(axis, curr_vel);
        }

        /// <summary>
        /// 获取轴的位置信息
        /// </summary>
        /// <param name="axis"></param>
        /// <returns>运动轴命令脉冲</returns>
        public int GetPositionVal(ushort axis)
        {
            int npos = Dmc2210.d2210_get_position(axis);
            return npos;
        }

        /// <summary>
        /// 获取轴的IO信号
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <returns></returns>
            /*返回值: 8 FU 1：表示正在加速
            9 FD 1：表示正在减速
            10 FC 1：表示在低速运行
            11 ALM 1：表示伺服报警信号 ALM 为 ON
            12 PEL 1：表示正限位信号 +EL 为 ON
            13 MEL 1：表示负限位信号–EL 为 ON
            14 ORG 1：表示原点信号 ORG 为 ON
            15 SD 1：表示 SD 信号为 ON*/
        public int GetAxisIoStatus(ushort axis)
        {
            int iostatus = Dmc2210.d2210_axis_io_status(axis);
            return iostatus;
        }

        /// <summary>
        /// 轴回零
        /// </summary>
        /// <param name="axis">轴号</param>
        /// <param name="home_mode">1:正方向回零 0:负方向回零</param>
        /// <param name="vel_mode">0:低速回原点 1:高速回原点</param>
        public void AxisGoHome(ushort axis, ushort home_mode, ushort vel_mode)
        {
            Dmc2210.d2210_home_move(axis, home_mode, vel_mode);
        }
        
        public int GetAxisStatus(ushort axis)
        {
            uint status = Dmc2210.d2210_get_rsts(axis);
            return (int)status;
        }

    }
}
