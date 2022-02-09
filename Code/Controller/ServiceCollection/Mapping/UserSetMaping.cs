using BaseController.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSetModel;
using static UserSetModel.UserSetParamModel;

namespace ServiceCollection.Mapping
{
    class UserSetMap : BaseMap<UserSetParamModel>
    {
        public UserSetMap()
        {
            // Table & Column Mappings
            this.ToTable("UserSetParamModel");

            //this.Ignore(x => x.xStartPos);
            //this.Ignore(x => x.yStartPos);
        }
    }
}
