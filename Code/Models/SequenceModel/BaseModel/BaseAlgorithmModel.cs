using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceTestModel
{
    /// <summary>
    /// 算法参数基类
    /// </summary>
    public abstract class BaseAlgorithmModel
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
    }
}
