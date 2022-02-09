using Infrastructure.DBCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseModels
{
    public class BaseResultModel
    {
        public BaseResultModel()
        {
            RunResult = false;
            ErrorMessage = "";
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        [DB(IsSerilize = false)]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 运行结果，成功/失败
        /// </summary>
        [DB(IsSerilize = false)]
        public bool RunResult { get; set; }

        /// <summary>
        /// 返回值，用于承载自定义返回值
        /// </summary>
        [DB(IsSerilize = false)]
        public object ObjectResult { get; set; }
    }
}
