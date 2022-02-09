using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SequenceTestModel
{
    /// <summary>
    /// Model流程的基类
    /// </summary>
    public abstract class BaseSeqModel
    { 
        public virtual int Id { get; set; } 
        public virtual string Name { get; set; }

        /// <summary>
        /// 单个模块的FeatureType
        /// </summary>
        [XmlIgnore]
        public abstract string BaseType { get; set; }

        //[XmlIgnore]
        //public abstract BaseSeqResultModel itemResult { get; set; }
    }

    /// <summary>
    /// Model结果的基类
    /// </summary>
    public abstract class BaseSeqResultModel
    {
        /// <summary>
        /// 模块测试的结果 true/false
        /// </summary> 
        [Description("测试结果true or false")]
        public virtual bool TestResult { get; set; }

        /// <summary>
        /// 模块输出图片
        /// </summary>
        [Description("结果图片")]
        public virtual HObject ResultImage { get; set; }

        /// <summary>
        /// 模块输出的ROI结果
        /// </summary>
        [Description("结果ROI")]
        public virtual HObject ResultObj { get; set; }

        /// <summary>
        /// 模块输出结果字符串
        /// </summary>
        [Description("结果字符串")]
        public virtual string ResultStr { get; set; }
    }

    /// <summary>
    /// 标定工具 接口
    /// </summary>
    public interface ICalibration
    {

    }

    /// <summary>
    /// 检测3D 接口
    /// </summary>
    public interface ICheck3D
    {

    }

    /// <summary>
    /// 检测识别 接口
    /// </summary>
    public interface ICheckDetect
    {

    }

    /// <summary>
    /// 数据操作 接口
    /// </summary>
    public interface IDataOperate
    {

    }

    /// <summary>
    /// 系统流程 接口
    /// </summary>
    public interface IFlowControl
    {

    }

    /// <summary>
    /// 图像处理 接口
    /// </summary>
    public interface IImageControl
    {

    }

    /// <summary>
    /// 几何测量 接口
    /// </summary>
    public interface IMeasure
    {

    }

    /// <summary>
    /// 变量工具 接口
    /// </summary>
    public interface IVariableSet
    {

    }

    /// <summary>
    /// 定位工具 接口
    /// </summary>
    public interface ICreateModel
    {

    }

    /// <summary>
    /// 区域处理 接口
    /// </summary>
    public interface IRegionModel
    {

    }

    /// <summary>
    /// 轮廓处理 接口
    /// </summary>
    public interface IContourModel
    {

    }

    /// <summary>
    /// 通讯 接口
    /// </summary>
    public interface IReport
    {

    }

    /// <summary>
    /// 运动控制 接口
    /// </summary>
    public interface IMotorModel
    {

    }


}
