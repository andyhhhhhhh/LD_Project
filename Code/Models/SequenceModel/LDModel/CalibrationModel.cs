using System;
using System.Collections.Generic;
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
        /// <summary>
        /// 示教上料图像中心Row
        /// </summary>
        public double TeachImageRow { get; set; }
        /// <summary>
        /// 示教上料图像中心Col
        /// </summary>
        public double TeachImageCol { get; set; }
        /// <summary>
        /// 示教上料图像中心Angle
        /// </summary>
        public double TeachImageAng { get; set; }

        /// <summary>
        /// 示教上料图像中心Row _2
        /// </summary>
        public double TeachImageRow_2 { get; set; }
        /// <summary>
        /// 示教上料图像中心Col _2
        /// </summary>
        public double TeachImageCol_2 { get; set; }
        /// <summary>
        /// 示教上料图像中心Angle _2
        /// </summary>
        public double TeachImageAng_2 { get; set; }


        /// <summary>
        /// 示教上料图像中心Row _3
        /// </summary>
        public double TeachImageRow_3 { get; set; }
        /// <summary>
        /// 示教上料图像中心Col _2
        /// </summary>
        public double TeachImageCol_3 { get; set; }
        /// <summary>
        /// 示教上料图像中心Angle _3
        /// </summary>
        public double TeachImageAng_3 { get; set; }
         
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
