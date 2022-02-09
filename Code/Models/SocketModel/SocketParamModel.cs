using BaseModels;
using Infrastructure.DBCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketModel
{
    public class SocketParamModel : BaseModel<SocketParamModel>
    {
        public virtual string IPAddress { get; set; }

        public virtual int PortNum { get; set; }

        public virtual int TimeOut { get; set; }

        public virtual string SendContent { get; set; }

        public virtual bool IsWork { get; set; }

        [DB(IsSerilize = false)]
        public virtual bool IsService { get; set; }

        public override SocketParamModel Clone()
        {
            SocketParamModel socketParamModel = new SocketParamModel();
            socketParamModel.IPAddress = IPAddress;
            socketParamModel.PortNum = PortNum;
            socketParamModel.TimeOut = TimeOut;
            socketParamModel.SendContent = SendContent;
            socketParamModel.IsWork = IsWork;
            socketParamModel.IsService = IsService;
            return socketParamModel;
        }

        //public override bool IsEuqalEntity(SocketParamModel compare)
        //{
        //    if (compare.Name == Name)
        //        if (compare.IPAddress == IPAddress)
        //            if (compare.PortNum == PortNum)
        //                if (compare.IsWork == IsWork)
        //                    return true;
        //    return false;
        //}
    }
}
