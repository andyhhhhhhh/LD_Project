using BaseModels;
using Infrastructure.DBCore;
using SequenceTestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionController
{
    public interface IMotorControl
    {
        bool Init(object parameter);
        BaseResultModel Run(BaseMotorModel controlModel, MotorControlType controlType);
    }

    public enum MotorControlType
    {
        /// <summary>
        /// 使能
        /// </summary>
        AxisEnable,
        /// <summary>
        /// 下电
        /// </summary>
        AxisDisable,
        /// <summary>
        /// 回原点
        /// </summary>
        AxisGoHome,
        /// <summary>
        /// 回原点
        /// </summary>
        AxisAllGoHome,
        /// <summary>
        /// 绝对运动 参数：Coord:多轴运动 Axis:单轴运动
        /// </summary>
        AxisMove,
        /// <summary>
        /// 插补运动 参数：Coord:多轴运动 Axis:单轴运动
        /// </summary>
        AxisMoveGroup,
        /// <summary>
        /// 插补运动不等待完成 参数：Coord:多轴运动 Axis:单轴运动
        /// </summary>
        AxisMoveGroupNotWait,
        /// <summary>
        /// 相对对运动 参数：Coord:多轴运动 Axis:单轴运动
        /// </summary>
        AxisMoveRel,
        /// <summary>
        /// Jog不停运动 参数：Coord:多轴运动 Axis:单轴运动
        /// </summary>
        AxisMoveJog,
        /// <summary>
        /// 运动 参数：Coord:多轴运动 排除Z轴的运动
        /// </summary> 
        AxisMoveWithOutZ,
        /// <summary>
        /// XY不等待运动完成（非阻塞的方法） 参数：Coord:多轴运动 Axis:单轴运动
        /// </summary>
        AxisMoveNotWait,
        /// <summary>
        /// 不等待运动完成（非阻塞的方法） 参数：Coord:多轴运动 排除Z轴的运动
        /// </summary>
        AxisMoveWithOutZNotWait,
        /// <summary>
        /// 轴等待运动完成
        /// </summary>
        AxisWaitMoveDone,
        /// <summary>
        /// 暂停
        /// </summary>
        AxisStop,
        /// <summary>
        /// 复位
        /// </summary>
        AxisReset,
        /// <summary>
        /// 设置零点
        /// </summary>
        AxisSetHome,
        /// <summary>
        /// 轴状态
        /// </summary>
        AxisStatus,
        /// <summary>
        /// 急停
        /// </summary>
        AxisEmergency,
        /// <summary>
        /// 获取轴实际位置
        /// </summary>
        AxisGetPosition,
        /// <summary>
        /// 获取轴编码器位置
        /// </summary>
        AxisGetEncodePosition,
        /// <summary>
        /// IO不设置超时触发
        /// </summary>
        IOTrigger,
        /// <summary>
        /// IO设置超时触发
        /// </summary>
        IOTriggerTime,
        /// <summary>
        /// 查询DO
        /// </summary>
        QueryDO,
        /// <summary>
        /// 查询DI
        /// </summary>
        QueryDI,
        /// <summary>
        /// 查询DI超时
        /// </summary>
        QueryDITime,
        /// <summary>
        /// 关闭控制卡
        /// </summary>
        Close,
        /// <summary>
        /// 所有轴停止运动
        /// </summary>
        AllStop,
    }

    public enum EM_ERRCODE
    {
        ERR_INPUT_PARAM = -1000,        //入参为空或者不合法
        //motor err
        ERR_MOTOR_API,                  //控制卡接口命令返回错误
        ERR_NO_CARD,                    //控制卡不存在，或者无卡
        ERR_CARD_EXIST,                 //控制卡已经存在
        ERR_LOAD_CFG,                   //加载配置文件失败
        ERR_INIT_MOTOR,                 //初始化控制卡失败
        ERR_INVALID_AXIS,               //无效轴
        ERR_INVALID_EXT,                //无效的IO模块(卡自带或扩展模块)
        ERR_NOINIT_MOTOR,               //未初始化控制卡
        ERR_AXIS_HOME,                  //轴回原失败
        ERR_SET_HOMEPOS,                //设置原点位置失败
        ERR_AXIS_MOVING,                //轴正在运动中
        ERR_GET_CURPOS,                 //获取当前位置失败
        ERR_CARD_RESET,                 //卡复位失败
        ERR_AXIS_RESET,                 //轴复位失败
        ERR_AXIS_TRAP,                  //轴点位运动失败
        ERR_AXIS_CAPTURE,               //硬件捕获模式失败
        ERR_AXIS_GEAR,                  //跟随模式运动失败
        ERR_SET_CRD,                    //建立坐标系失败
        ERR_CRD_OVERLIMIT,              //坐标系超过2个
        ERR_GET_CRD,                    //获取坐标系失败
        ERR_ARC_MOVE,                   //圆弧插补不合法，数据错误
        ERR_CRD_MOVE,                   //坐标系运动失败
        ERR_IO_TYPE,                    //io类型错误
        ERR_WRITE_IO,                   //写入io失败

        RETURN_OK = 0,                  //正常返回
    }
}
