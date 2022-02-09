using ExternDll;
using System;
using System.Text;

namespace EngineController.VBAEngine
{
    /// <summary>
    /// vba引擎共用方法
    /// </summary>
    [Microsoft.VisualBasic.CompilerServices.StandardModuleAttribute()]
    public class GlobalObject
    {
        public static VbaFunction h = new VbaFunction();
        public static ExternClass t = new ExternClass();
        /// <summary>
        /// 全局的 HMeasureSYS 对象
        /// </summary>
        public static VbaFunction HVba
        {
            get
            {
                return h;
            }
        }

    }
}
