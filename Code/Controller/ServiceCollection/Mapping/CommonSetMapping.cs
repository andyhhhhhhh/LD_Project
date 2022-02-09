
using BaseController.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSetModel;

namespace ServiceCollection.Mapping
{
    public class CommonSetMapping : BaseMap<CommonSetModel>
    {
        public CommonSetMapping()
        {
            // Table & Column Mappings
            ToTable("CommonSetModel");
        }


    }

}
