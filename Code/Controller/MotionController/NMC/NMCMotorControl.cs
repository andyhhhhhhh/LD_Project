using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseModels;
using Infrastructure.DBCore;
using SequenceTestModel;

namespace MotionController
{
    //高川控制卡运动控制
    public class NMCMotorControl : IMotorControl
    {
        public bool Init(object parameter)
        {
            throw new NotImplementedException();
        }

        public BaseResultModel Run(BaseMotorModel controlModel, MotorControlType controlType)
        {
            throw new NotImplementedException();
        } 

    }
}
