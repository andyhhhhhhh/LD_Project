using BaseController.Services;
using SerialPortModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCollection.Mapping
{
    public class SerialPortMap  : BaseMap<SerialPortParamModel>
    {
        public SerialPortMap()
        {
            ToTable("SerialPortParamModel");
            ////忽略保存到数据库的字段请在此处添加
            //Ignore(x => x.IsFormatRec); 
        }
    }
}
