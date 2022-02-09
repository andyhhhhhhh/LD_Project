using BaseModels;
using Infrastructure.DBCore;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortModel
{
    public class SerialPortParamModel : BaseModel<SerialPortParamModel>
    {
        public bool IsHex { get; set; }
        public string ComPortName { get; set; }
        public Parity Parity { get; set; }

        public int BaudRate { get; set; }

        public int DataBits { get; set; }
        public StopBits StopBits { get; set; }

        public string SendContent { get; set; }

        [DB(IsSerilize = false)]
        public bool IsFormatRec { get; set; }

        /// <summary>
        /// 等待时间ms
        /// </summary>
        public int TimeOut { get; set; }

        public override SerialPortParamModel Clone()
        {
            SerialPortParamModel SerialPortParamModel = new SerialPortParamModel();
            SerialPortParamModel.Name = Name;
            SerialPortParamModel.BaudRate = BaudRate;
            SerialPortParamModel.ComPortName = ComPortName;
            SerialPortParamModel.DataBits = DataBits;
            SerialPortParamModel.Parity = Parity;
            SerialPortParamModel.SendContent = SendContent;
            SerialPortParamModel.StopBits = StopBits;
            SerialPortParamModel.TimeOut = TimeOut;
            SerialPortParamModel.IsFormatRec = IsFormatRec;
            return SerialPortParamModel;
        }

        //public override bool IsEuqalEntity(SerialPortParamModel compare)
        //{
        //    if (compare.Name == Name)
        //        if (compare.BaudRate == BaudRate)
        //            if (compare.ComPortName == ComPortName)
        //                if (compare.DataBits == DataBits)
        //                    if (compare.Parity == Parity)
        //                        if (compare.StopBits == StopBits)
        //                            return true;
        //    return false;
        //}
    }
}
