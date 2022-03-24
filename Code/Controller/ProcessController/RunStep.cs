using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessController
{
    public class RunStep
    {

    }

    /// <summary>
    /// 自动流程步骤枚举
    /// </summary>
    public enum AutoRunStep
    {
        
        [Description("上料Z移动到安全位置")]
        StepLoadZMoveSafePos,

        [Description("上料X移动到安全位置")]
        StepLoadXMoveSafePos,

        [Description("吸膜破真空")]
        StepSuckFilmOff,

        [Description("顶升模组到安全位")]
        StepUpMoveSafePos,

        [Description("移XYR动到接料盘位置")]
        StepXYRMoveGetTrayPos,
        
        [Description("取产品位气缸松开")]
        StepGetCylinderUnClamp, 

        [Description("检查Map表是否录入")]
        StepCheckMap,

        [Description("等待员工上料完成")]
        StepWaitLoadDone,

        [Description("取产品位气缸夹紧")]
        StepGetCylinderClamp,

        [Description("检测气缸夹紧到位")]
        StepCheckClamp,

        [Description("XY移动到大视野拍照位置")]
        StepXYMoveBigCameraPos,

        [Description("双目相机移动到大视野拍照位置")]
        StepMoveBigCameraPos,
        
        [Description("大视野相机拍照执行算法")]
        StepBigCameraSnap,

        [Description("XYR移动大视野补偿位")]
        StepXYMoveBigOffSetPos,

        [Description("大视野相机第二次拍照")]
        StepBigCameraSnapSecond,

        [Description("等待小视野完成")]
        StepWaitSmallDone,
    }

    /// <summary>
    /// 上料模组执行步骤
    /// </summary>
    public enum LoadRunStep
    {
        [Description("上料模组Z移动到安全位置")]
        StepLoadZMoveSafePos,

        [Description("上料模组移动到等待位置")]
        StepLoadXMoveWaitPos,
        
        [Description("等待是否可以取料")]
        StepWaitReadLoad,

        [Description("上料模组X移动到上料位置")]
        StepLoadXMoveTakePos,

        [Description("上料模组Z移动到上料位置")]
        StepLoadZMoveTakePos,

        [Description("顶针上升")]
        StepThimbleRisePos,

        [Description("上料吸嘴吸真空")]
        StepLoadSuctionVaccum,

        [Description("顶针下降")]
        StepThimbleDownPos,

        [Description("上料模组Z移动到安全位置1")]
        StepLoadZMoveSafePos_1,

        [Description("上料模组X移动N面拍照位")]
        StepLoadXMoveNSnapPos,

        [Description("上料模组Z移动N面拍照位")]
        StepLoadZMoveNSnapPos,

        [Description("N面拍照执行算法")]
        StepNSnap,

        [Description("移动到下料等待位")]
        StepMoveUnLoadWaitPos,
         
        [Description("等待是否放产品到检测位")]
        StepWaitReadyCheck,

        [Description("上料模组X移动到DDR马达")]
        StepLoadXMoveDDR,

        [Description("上料模组Z移动到DDR马达")]
        StepLoadZMoveDDR,

        [Description("上料吸嘴破真空")]
        StepLoadBreakVaccum,

        [Description("上料Z移动到上料等待位2")]
        StepLoadZMoveSafePos_2,

        [Description("上料X移动到上料等待位2")]
        StepLoadXMoveWaitPos_2,

    }
    
    /// <summary>
    /// 工位3检测及下料步骤枚举
    /// </summary>
    public enum CheckRunStep
    {
        [Description("下料Z移动到下料安全位")]
        StepUnLoadZMoveSafePos,

        [Description("移动到下料安全位")]
        StepUnLoadXYMoveSafePos,

        [Description("检测模组移动到上料位")]
        StepCheckMoveLoadPos,

        [Description("等待DDR开始检测产品")]
        StepWaitReadyDDRCheck,
        
        [Description("检测模组移动到上料校正位置1")]
        StepMoveCorrect1Pos,

        //[Description("下料模组移动到AR定位位置")]
        //StepUnLoadMoveARPos, 

        [Description("校正位置1拍照执行算法")]
        StepCorrect1Snap,

        [Description("检测模组移动到AR检测位")]
        StepMoveARCheckPos, 

        [Description("AR检测拍照执行算法")]
        StepARCheckSnap,

        [Description("检测模组移动到上料校正位置2")]
        StepMoveCorrect2Pos,

        //[Description("下料模组移动到HR定位位置")]
        //StepCheckMoveHRPos,

        [Description("校正位置2拍照执行算法")]
        StepCorrect2Snap,

        [Description("检测模组移动到HR检测位")]
        StepMoveHRCheckPos,

        [Description("HR检测拍照执行算法")]
        StepHRCheckSnap, 

        [Description("检测模组移动到下料位")]
        StepCheckMoveUnLoadPos,

        [Description("下料模组移动到上料位")]
        StepUnLoadMoveLoadPos,

        [Description("下料模组Z移动到上料位")]
        StepUnLoadZMoveLoadPos,

        [Description("下料吸嘴吸真空")]
        StepUnLoadSuctionVaccum,

        [Description("下料Z移动到下料安全位1")]
        StepUnLoadZMoveSafePos1,

        [Description("下料模组移动到放料位")]
        StepUnLoadMovePutPos,

        [Description("下料模组Z移动到放料位")]
        StepUnLoadZMovePutPos,

        [Description("下料模组移动到换料位")]
        StepChangeUnLoadTray,

        [Description("下料吸嘴破真空")]
        StepUnLoadBreakVaccum,
    }
    
    /// <summary>
    /// 小视野模组
    /// </summary>
    public enum SmallRunStep
    {
        [Description("等待大视野相机拍完")]
        StepWaitBigDone,

        [Description("吸膜破真空")]
        StepSuckFilmOff,

        [Description("顶升模组到安全位")]
        StepMoveSuckSafePos,

        [Description("XYR移动小视野中心位置")]
        StepXYMoveSmallCenterPos,

        [Description("双目相机移动到小视野拍照位置")]
        StepMoveSmallCameraPos,

        [Description("顶升模组移到吸蓝膜位置")]
        StepMoveSuckFilmPos,

        [Description("吸蓝膜")]
        StepSuckFilm,
         
        [Description("小视野入料判断")]
        StepSmallJudge,

        [Description("XYR移动到Bar条判定位置")]
        StepXYMoveBarJudgePos,

        [Description("小视野相机拍照推算Bar条号")]
        StepSmallCameraSnapBar,

        [Description("推算Bar条号重新移动位置")]
        StepBarNGMovePos,

        [Description("等待上料取产品完成")]
        StepWaitLoadDone,

        [Description("XYR移动到拍照抓取位置")]
        StepXYMoveSmallGetPos,

        [Description("小视野入料判断2")]
        StepSmallJudge_2,

        [Description("小视野相机定位产品OCR")]
        StepSmallCameraSnapOCR, 

        [Description("XYR移动到顶针上方位置")]
        StepXYRMoveSuckPos,

        [Description("小视野拍照检测缺陷")]
        StepAlgorithmSmall,

        [Description("小视野拍照取完判断")]
        StepSmallGetDone,

    }

}
