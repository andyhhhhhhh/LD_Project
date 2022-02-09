using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SequenceTestModel
{
    public abstract class BaseMotorModel
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }

        //[XmlIgnore]
        //public abstract string BaseType { get; set; }
    }

    public enum EnumMotor
    {
        控制卡配置,
    }

    public enum EnumIO
    {
        通用输入,
        通用输出,
    }

    public enum EnumMonitorIO
    {
        通用输入,
        通用输出,
        输入输出关联
    }

    public enum EnumIOType
    {
        启动IO,
        复位IO,
        急停IO,
        暂停IO,
        停止IO,
        通用IO,
        刹车IO,
        其他IO,
        扩展IO
    }
}
