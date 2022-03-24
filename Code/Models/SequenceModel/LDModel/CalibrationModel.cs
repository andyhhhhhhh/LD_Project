using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceTestModel
{
    /// <summary>
    /// 标定参数
    /// </summary>
    public class CalibrationModel
    {

        [CategoryAttribute("模板参数"), Description("开始角度")]
        public double StartingAngle { get; set; }

        [CategoryAttribute("模板参数"), Description("结束角度")]
        public double AngleExtent { get; set; }

        [CategoryAttribute("模板参数"), Description("最小分数")]
        public double MinScore { get; set; } 

        [CategoryAttribute("模板参数"), Description("")]
        public double PyramidLevel { get; set; }

        [CategoryAttribute("模板参数"), Description("")]
        public double LastPyramidLevel { get; set; }

        [CategoryAttribute("模板参数"), Description("")]
        public double Greediness { get; set; }


        [CategoryAttribute("步距参数"), Description("X方向步距")]
        public double XOffSet { get; set; }

        [CategoryAttribute("步距参数"), Description("Y方向步距")]
        public double YOffSet { get; set; }

        [CategoryAttribute("步距参数"), Description("U方向步距")]
        public double UOffSet { get; set; }

    }


    /// <summary>
    /// 标定参数
    /// </summary>
    public class FixtureTeachModel
    {
        public int Id { get; set; }

        /// <summary>
        /// 示教图像中心Row
        /// </summary>
        public double TeachImageRow { get; set; }
        /// <summary>
        /// 示教图像中心Col
        /// </summary>
        public double TeachImageCol { get; set; }
        /// <summary>
        /// 示教图像中心Angle
        /// </summary>
        public double TeachImageAng { get; set; }
    }
}
