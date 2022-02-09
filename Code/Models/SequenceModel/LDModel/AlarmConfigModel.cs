using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceTestModel
{
    /// <summary>
    /// 报警配置类
    /// </summary>
    public class AlarmConfigModel
    {
        public List<AlarmValue> ListAlarm { get; set; }

        public AlarmConfigModel()
        {
            ListAlarm = new List<AlarmValue>();
        }
    }

    public class AlarmValue
    {
        /// <summary>
        /// 报警ID
        /// </summary>
        public int AlarmID { get; set; }

        /// <summary>
        /// 报警信息
        /// </summary>
        public string AlarmInfo { get; set; }
    }
}
