using BaseModels;
using SequenceTestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionController
{
    public interface IAlgorithmControl
    {
        bool Init(object parameter);
        BaseResultModel Run(BaseAlgorithmModel controlModel, AlgorithmControlType controlType);
    }

    public enum AlgorithmControlType
    {
        /// <summary>
        /// 算法初始化
        /// </summary>
        AlgorithmInit,

        /// <summary>
        /// 算法执行
        /// </summary>
        AlgorithmRun,
    }
}
