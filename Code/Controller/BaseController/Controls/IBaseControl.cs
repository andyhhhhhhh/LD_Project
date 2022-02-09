using BaseModels;
using Infrastructure.DBCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BaseController.BaseControl;

namespace BaseController
{
    public interface IBaseControl
    {
        bool Init(object parameter);
        BaseResultModel Run(BaseEntity controlModel, ControlType controlType);
    }
}
