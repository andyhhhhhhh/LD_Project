using BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserSetModel
{
    public class UserSetParamModel : BaseModel<UserSetParamModel>
    {
        public virtual string UserName { get; set; }
        public virtual string PassWord { get; set; }
        public virtual string Type { get; set; }

        public override UserSetParamModel Clone()
        {
            UserSetParamModel usersetParamModel = new UserSetParamModel();
            usersetParamModel.UserName = UserName;
            usersetParamModel.PassWord = PassWord;
            usersetParamModel.Type = Type;
            return usersetParamModel;
        }

        public enum User
        { 
            Operator,//操作员
            Debugger,//调试员
            Engineer,//工程师
            SuperAdmin//超级管理员
        }


    }
}