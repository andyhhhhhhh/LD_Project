using SequenceTestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionController
{
    public class MotorControl
    { 
        private Motor m_Motor;

        private static MotorControl m_motorControl;
        public static MotorControl GetInstance()
        {
            if(m_motorControl == null)
            {
                m_motorControl = new MotorControl(); 
            }

            return m_motorControl;
        }
        /// <summary>
        /// 初始化Motor实例
        /// </summary>
        /// <param name="param">控制卡类型</param>
        public bool Init(object param)
        {
            bool bresult = true;
            EnumCard enumCard = (EnumCard)Enum.Parse(typeof(EnumCard), param.ToString());
            switch (enumCard)
            {
                case EnumCard.正运动:
                    if (m_Motor == null)
                    {
                        m_Motor = new ZMotionControl();
                        bresult = ZMotionService.Instance().Init();
                    }
                    break;
                case EnumCard.软PLC:
                    if (m_Motor == null)
                    {
                        m_Motor = new SoftPlcControl();
                        bresult = SoftPlcService.Instance().Init();
                    }
                    break;
                case EnumCard.虚拟卡:
                    if (m_Motor == null)
                    {
                        m_Motor = new VirtualControl();
                    }
                    break;
                default:
                    break;
            }

            return bresult;           
        }

        public AxisStatus GetStatus(ushort cardIndex, ushort axisIndex, out bool bok)
        {
            bok = true;
            return ZMotionService.Instance().GetAxisStatus(cardIndex, axisIndex, out bok);
        }
         
        public int Motor_axis_enable(ushort cardIndex, ushort axisIndex)
        {
            return m_Motor.Motor_axis_enable(cardIndex, axisIndex);
        } 
        public int Motor_axis_disable(ushort cardIndex, ushort axisIndex)
        {
            return m_Motor.Motor_axis_disable(cardIndex, axisIndex);
        }

        /// <summary>
        /// 初始化(每张卡都要进行初始化,逐一按顺序初始化(1~n))
        /// </summary>
        /// <param name="paxis">轴运动参数信息 , 数组大小必须为 axisCnt</param> 
        /// <param name="cardIndex">卡索引，取值范围[0,7]</param> 
        /// <param name="axisCnt">当前卡轴的个数，取值范围[1,8]</param>
        /// <param name="extIOCnt">当前卡扩展io个数,取值范围[0,1,2,3....]</param>
        /// <param name="cntType">卡连接方式</param>
        /// <returns>参考EM_ERR_CODE</returns>
        public int Motor_init_card(AxisParam paxis, ushort cardIndex, ushort axisCnt, int extIOCnt, int cntType)
        {
            return m_Motor.Motor_init_card(paxis, cardIndex, axisCnt, extIOCnt, cntType);
        }

        /// <summary>
        /// 反初始化
        /// </summary>
        /// <returns>参考EM_ERR_CODE</returns>
        public int Motor_deinit()
        {
            return m_Motor.Motor_deinit();
        }

        /// <summary>
        /// 复位控制卡(电机掉电后需要)
        /// </summary>
        /// <returns>参考EM_ERR_CODE</returns>
        public int Motor_reset()
        {
            return m_Motor.Motor_reset();
        }

        /// <summary>
        /// 获取单轴最大运动速度
        /// </summary>
        /// <param name="cardIndex">卡号, 取值范围[0,8)</param>
        /// <param name="axisIndex">轴号  取值范围[0,8)</param>
        /// <param name="speed">用来缓存速度的结构体地址  speed 单位 脉冲/ms</param>
        /// <returns>参考EM_ERR_CODE</returns>
        public int Motor_axis_max_speed(ushort cardIndex, ushort axisIndex, out TSpeed speed)
        {
            return m_Motor.Motor_axis_max_speed(cardIndex, axisIndex, out speed);
        }

        /// <summary>
        /// 重置单轴状态、限位等
        /// </summary>
        /// <param name="cardIndex">卡号, 取值范围[0,8)</param>
        /// <param name="axisIndex">轴号  取值范围[0,8)</param>
        /// <returns></returns>
        public int Motor_axis_reset(ushort cardIndex, ushort axisIndex)
        {
            return m_Motor.Motor_axis_reset(cardIndex, axisIndex);
        }

        /// <summary>
        /// 单轴回原
        /// </summary>
        /// <param name="cardIndex">卡号</param>
        /// <param name="axisIndex">轴号</param>
        /// <returns>参考EM_ERR_CODE</returns>
        public int Motor_axis_home(ushort cardIndex, ushort axisIndex, uint homeIo, float speed, float secondSpeed, int homeType, int limitType)
        {
            return m_Motor.Motor_axis_home(cardIndex, axisIndex, homeIo, speed, secondSpeed, homeType, limitType);
        }

        /// <summary>
        /// 单轴回原完成
        /// </summary>
        /// <param name="cardIndex">卡号,  取值范围[0,8)</param>
        /// <param name="axisIndex">轴号	取值范围[0,8)</param>
        /// <returns>0表示回原完成，否则返回错误码 PS: 返回 ERR_AXIS_MOVING 表示还在回原中</returns>
        public int Motor_axis_home_finished(ushort cardIndex, ushort axisIndex)
        {
            return m_Motor.Motor_axis_home_finished(cardIndex, axisIndex);
        }

        /// <summary>
        /// 判断单轴是否正在运动
        /// </summary>
        /// <param name="cardIndex">卡号,  取值范围[0,8)</param>
        /// <param name="axisIndex">轴号  取值范围[0,8)</param>
        /// <returns>1表示正在运动 0表示不在运动中</returns>
        public int Motor_axis_is_moving(ushort cardIndex, ushort axisIndex)
        {
            return m_Motor.Motor_axis_is_moving(cardIndex, axisIndex);
        }

        /// <summary>
        /// 停止单轴运动
        /// </summary>
        /// <param name="cardIndex">卡号,  取值范围[0,8)</param>
        /// <param name="axisIndex">轴号  取值范围[0,8)</param>
        /// <param name="type">0: 平滑停止  1：紧急停止</param>
        /// <returns>参考EM_ERR_CODE</returns>
        public int Motor_axis_stop(ushort cardIndex, ushort axisIndex, int type)
        {
            return m_Motor.Motor_axis_stop(cardIndex, axisIndex, type);
        }

        /// <summary>
        /// 单轴移动到指定位置(mm)
        /// </summary>
        /// <param name="cardIndex">卡号, 取值范围[0,8)</param>
        /// <param name="axisIndex">轴号 取值范围[0,8)</param>
        /// <param name="pos">点位（mm）</param>
        /// <param name="pspeed">设置速度</param>
        /// <returns>参考EM_ERR_CODE</returns>
        public int Motor_axis_move_pos(ushort cardIndex, ushort axisIndex, double pos, TSpeed pspeed)
        {
            return m_Motor.Motor_axis_move_pos(cardIndex, axisIndex, pos, pspeed);
        }

        /// <summary>
        /// 单轴相对当前位置移动(mm)
        /// </summary>
        /// <param name="cardIndex">卡号,  取值范围[0,7]</param>
        /// <param name="axisIndex">轴号   取值范围[0,7]</param>
        /// <param name="offset">偏移距离 有正负之分</param>
        /// <param name="pspeed">速度设置</param>
        /// <param name="basePos"></param>
        /// <returns>参考EM_ERR_CODE</returns>
        public int Motor_axis_move_offset(ushort cardIndex, ushort axisIndex, double offset, TSpeed pspeed, int basePos)
        {
            return m_Motor.Motor_axis_move_offset(cardIndex, axisIndex, offset, pspeed, basePos);
        }

        /// <summary>
        /// 单轴相对移动(连续)
        /// </summary>
        /// <param name="cardIndex">卡号,  取值范围[0,7]</param>
        /// <param name="axisIndex">轴号   取值范围[0,7]</param>
        /// <param name="dir">0 负方向 1 正方向</param>
        /// <param name="pspeed">速度设置</param>
        /// <returns>参考EM_ERR_CODE</returns>
        public int Motor_axis_vmove(ushort cardIndex, ushort axisIndex, int dir, TSpeed pspeed)
        {
            return m_Motor.Motor_axis_vmove(cardIndex, axisIndex, dir, pspeed);
        }

        /// <summary>
        /// 读取指定卡输入状态
        /// </summary>
        /// <param name="cardIndex">卡索引，取值范围[0,7]</param>
        /// <param name="di_type">EM_GPIO_TYPE</param>
        /// <param name="pval">状态输出</param>
        /// <param name="exindex"></param>
        /// <returns>参考EM_ERR_CODE</returns>
        public int Motor_read_in(ushort cardIndex, short di_type, out long pval, short exindex = 0)
        {
            return m_Motor.Motor_read_in(cardIndex, di_type, out pval, exindex);
        }

        /// <summary>
        /// 读取输入io状态(通用输入)
        /// </summary>
        /// <param name="cardIndex">卡索引，取值范围[0,7]</param>
        /// <param name="ioindex">io索引号  取值范围[0,7]</param>
        /// <param name="exindex">扩展卡索引 , 取值范围[0,1,2,......)  默认值0 ，则为不是扩展卡</param>
        /// <returns>有信号为1，无信号为0，失败返回EM_ERR_CODE</returns>
        public int Motor_read_in_bit(ushort cardIndex, ushort ioindex, short exindex)
        {
            return m_Motor.Motor_read_in_bit(cardIndex, ioindex, exindex);
        }

        /// <summary>
        /// 读取输入电压值
        /// </summary>
        /// <param name="cardIndex">卡索引，取值范围[0,7]</param>
        /// <param name="index">通道，通常读取第一个通道  取值范围根据卡的支持通道数决定</param>
        /// <param name="pvalue">返回的电压值，外部缓存区</param>
        /// <param name="exindex">扩展卡索引 , 取值范围[0,1,2,......)  默认值0 ，则为不是扩展卡</param>
        /// <returns>参考EM_ERR_CODE</returns>
        public int Motor_read_adc(ushort cardIndex, ushort index, out double pvalue, short exindex)
        {
            return m_Motor.Motor_read_adc(cardIndex, index, out pvalue, exindex);
        }

        /// <summary>
        /// 获取指定卡的输出状态
        /// </summary>
        /// <param name="cardIndex">卡号 取值范围[0,7]</param>
        /// <param name="do_type">EM_GPIO_TYPE</param>
        /// <param name="pval">输出状态</param>
        /// <param name="exindex">EM_ERR_CODE</param>
        /// <returns></returns>
        public int Motor_read_out(ushort cardIndex, short do_type, out long pval, short exindex)
        {
            return m_Motor.Motor_read_out(cardIndex, do_type, out pval, exindex);
        }

        /// <summary>
        /// 获取指定IO操作状态
        /// </summary>
        /// <param name="cardIndex">卡索引，取值范围[0,7]</param>
        /// <param name="ioindex">io索引号  取值范围[0,iobit]</param>
        /// <param name="exindex">扩展卡索引 , 取值范围[0,1,2,......)  默认值0 ，则为不是扩展卡</param>
        /// <param name="do_type">EM_GPIO_TYPE</param>
        /// <returns>有信号为1，无信号为0，失败返回EM_ERR_CODE</returns>
        public int Motor_read_out_bit(ushort cardIndex, ushort ioindex, short exindex, short do_type)
        {
            return m_Motor.Motor_read_out_bit(cardIndex, ioindex, exindex, do_type);
        }

        /// <summary>
        /// 对指定out io操作
        /// </summary>
        /// <param name="cardIndex">卡索引，     取值范围[0,7]</param>
        /// <param name="ioindex">io索引号     取值范围[0,iobit]</param>
        /// <param name="val">操作值       取值范围[0,1]</param>
        /// <param name="exindex">扩展卡索引 , 取值范围[0,1,2,......)  值0 .则为不是扩展卡</param>
        /// <param name="do_type">EM_GPIO_TYPE</param>
        /// <returns>参考EM_ERR_CODE</returns>
        public int Motor_write_out_bit(ushort cardIndex, ushort ioindex, ushort val, short exindex, short do_type)
        {
            return m_Motor.Motor_write_out_bit(cardIndex, ioindex, val, exindex, do_type);
        }

        /// <summary>
        /// 获取轴当前位置(mm)
        /// </summary>
        /// <param name="cardIndex">卡号  取值范围[0,7]</param>
        /// <param name="axisIndex">轴号  取值范围[0,7]</param>
        /// <param name="pval">当前点位数据</param>
        /// <param name="type"></param>
        /// <returns>参考EM_ERR_CODE</returns>
        public int Motor_get_current_pos(ushort cardIndex, ushort axisIndex, out double pval, int type = 0)
        {
            return m_Motor.Motor_get_current_pos(cardIndex, axisIndex, out pval, type);
        }

        /// <summary>
        /// 创建一个坐标系
        /// </summary>
        /// <param name="pCardIndex">卡号索引集    取值范围[0,7]</param>
        /// <param name="pAxisIndex">轴号索引集</param>
        /// <param name="axisCnt">轴数量，索引集数量 最少两个轴</param>
        /// <returns>失败返回 INVALID_HANDLE_INT，成功返回坐标系ID</returns>
        public int Motor_create_crd(out ushort pCardIndex, out ushort pAxisIndex, ushort axisCnt)
        {
            return m_Motor.Motor_create_crd(out pCardIndex, out pAxisIndex, axisCnt);
        }

        /// <summary>
        /// 增加一个位置到直线插补运动队列
        /// </summary>
        /// <param name="crdID">坐标系ID</param>
        /// <param name="ppos">位置数组(mm)，大小必须是创建坐标系时的轴数量</param>
        /// <param name="pspeed">设置速度</param>
        /// <returns>参考EM_ERR_CODE</returns>
        public int Motor_add_line_pos(int crdID, double[] ppos, TSpeed pspeed)
        {
            return m_Motor.Motor_add_line_pos(crdID, ppos, pspeed);
        }

        /// <summary>
        /// 增加两个位置到圆弧插补运动队列
        /// </summary>
        /// <param name="crdID">坐标系ID</param>
        /// <param name="ppos1">位置数组(mm)，大小必须是创建坐标系时的轴数量</param>
        /// <param name="ppos2">位置数组(mm)，大小必须是创建坐标系时的轴数量</param>
        /// <param name="ppos3">位置数组(mm)，大小必须是创建坐标系时的轴数量</param>
        /// <param name="pspeed">设置速度</param>
        /// <returns>参考EM_ERR_CODE 
        /// PS: 圆弧插补只能基于两轴进行，通过ppos1，ppos2 ppos3 3个点计算得出
        ///若点位数据超过两轴发生变化，则会运动失败
        ///有4轴的情况下，圆弧插补只能前面3个轴之间任意两轴</returns>
        public int Motor_add_arc_pos(int crdID, double[] ppos1, double[] ppos2, double[] ppos3, TSpeed[] pspeed)
        {
            return m_Motor.Motor_add_arc_pos(crdID, ppos1, ppos2, ppos3, pspeed);
        }

        /// <summary>
        /// 启动插补运动
        /// </summary>
        /// <param name="crdID">坐标系ID</param>
        /// <returns>参考EM_ERR_CODE</returns>
        public int Motor_crd_move(int crdID)
        {
            return m_Motor.Motor_crd_move(crdID);
        }

        /// <summary>
        /// 查询插补运动坐标系状态
        /// </summary>
        /// <param name="crdID">坐标系ID</param>
        /// <param name="pRun">读取插补运动状态，0：该坐标系的该FIFO没有在运动；1：该坐标系的该FIFO正在进行插补运动。 </param>
        /// <param name="pSegment">读取当前已经完成的插补段数，当重新建立坐标系或者调用clear_crd_data指令后，该值会被清零</param>
        /// <returns>参考EM_ERR_CODE</returns>
        public int Motor_get_crd_status(int crdID, out short pRun, out long pSegment)
        {
            return m_Motor.Motor_get_crd_status(crdID, out pRun, out pSegment);
        }

        /// <summary>
        /// 查询轴状态
        /// </summary>
        /// <param name="cardIndex">卡id</param>
        /// <param name="axisIndex">轴id</param>
        /// <param name="type">参考EM_ERR_CODE</param>
        /// <returns></returns>
        public int Motor_get_axis_status(ushort cardIndex, ushort axisIndex, int type)
        {
            return m_Motor.Motor_get_axis_status(cardIndex, axisIndex, type);
        }

        /// <summary>
        /// 关闭一个坐标系
        /// </summary>
        /// <param name="crdID">坐标系ID</param>
        /// <returns>参考EM_ERR_CODE</returns>
        public int Motor_clear_crd_data(int crdID)
        {
            return m_Motor.Motor_clear_crd_data(crdID);
        }

        /// <summary>
        /// 执行一个命令行指令
        /// </summary>
        /// <param name="pCmd">命令行</param>
        /// <returns>参考EM_ERR_CODE</returns>
        public int Motor_cmd(string pCmd)
        {
            return m_Motor.Motor_cmd(pCmd);
        }

        /// <summary>
        /// 获取轴参数
        /// </summary>
        /// <param name="cardIndex">卡id</param>
        /// <param name="axisIndex">轴id</param>
        /// <returns>轴参数</returns>
        public int Motor_get_axis_type(ushort cardIndex, ushort axisIndex)
        {
            return m_Motor.Motor_get_axis_type(cardIndex, axisIndex);
        }

        /// <summary>
        /// 轴位置清零
        /// </summary>
        /// <param name="cardIndex">卡id</param>
        /// <param name="axisIndex">轴id</param>
        /// <returns></returns>
        public bool Motor_set_axis_zero_pos(ushort cardIndex, ushort axisIndex)
        {
            return m_Motor.Motor_set_axis_zero_pos(cardIndex, axisIndex);
        }

        /// <summary>
        /// 读取输入io状态(通用输入)
        /// </summary>
        /// <param name="cardIndex">卡索引，取值范围[0,7]</param>
        /// <param name="node">节点</param>
        /// <param name="index">io索引号  取值范围[0,7]</param>
        /// <returns>有信号为1，无信号为0，失败返回EM_ERR_CODE</returns>
        public int Motor_ext_read_in_bit(ushort cardIndex, ushort node, short index)
        {
            return m_Motor.Motor_ext_read_in_bit(cardIndex, node, index);
        }

        /// <summary>
        /// 对指定out io操作
        /// </summary>
        /// <param name="cardIndex">卡索引，     取值范围[0,7]</param>
        /// <param name="exindex"></param>
        /// <param name="ioindex">io索引号     取值范围[0,iobit]</param>
        /// <param name="val">操作值       取值范围[0,1]</param>
        /// <returns>参考EM_ERR_CODE</returns>
        public int Motor_ext_write_out_bit(ushort cardIndex, short exindex, ushort ioindex, ushort val)
        {
            return m_Motor.Motor_ext_write_out_bit(cardIndex, exindex, ioindex, val);
        }

        /// <summary>
        /// 插补运动
        /// </summary>
        /// <param name="cardIndex"></param>
        /// <param name="listAxisIndex">轴集合</param>
        /// <param name="speed">速度</param>
        /// <returns></returns>
        public int Motor_group_move(List<ushort> cardIndex, List<ushort> listAxisIndex, List<float> listpos, TSpeed speed, int stationIndex)
        {
            return m_Motor.Motor_group_move(cardIndex, listAxisIndex, listpos, speed, stationIndex);
        }
    }
}
