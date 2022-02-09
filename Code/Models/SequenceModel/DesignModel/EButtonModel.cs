using GlobalCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceTestModel
{
    public class EButtonModel : BaseDesignModel
    {
        public Global.EnumEButtonRun EText { get; set; }

        public float FontSize { get; set; }

    }
    public class EButtonProModel : BaseDesignModel
    {
        public Global.EnumEProcess EText { get; set; }
        public string SText { get; set; }
        public float FontSize { get; set; }
    }


    public class EDataOutputModel : BaseDesignModel
    {

    }
    public class EHSmartWindowModel : BaseDesignModel
    {
        public string LText { get; set; }
        public int LayoutWindow { get; set; }
    }
    public class ELblResultModel : BaseDesignModel
    {
        public string sResult { get; set; }
    }
    public class ELblStatusModel : BaseDesignModel
    {

    }
    public class ELogModel : BaseDesignModel
    {

    }
    public class ETextBoxModel : BaseDesignModel
    {
        public string LText { get; set; }

        public string LinkValue { get; set; }

        public int TextLength { get; set; }
    }
    public class ESetTextModel : BaseDesignModel
    {
        public string LText { get; set; }

        public string LinkValue { get; set; }

        public int TextLength { get; set; }
    }

    public class EComboProductModel:BaseDesignModel
    {
        
    }

    public class EProductSelModel : BaseDesignModel
    {

    }


    public class EItemResultModel : BaseDesignModel
    {
        public string LText { get; set; }
        public string SText { get; set; }
        public string LinkValue { get; set; }
    }

    public class EErrorItemModel : BaseDesignModel
    {
        public Global.EnumEProcess EText { get; set; }
        public string SText { get; set; }
    }

    public class ECheckModel : BaseDesignModel
    {
        public string LText { get; set; }

        public string LinkValue { get; set; }
    }
    public class ELightModel : BaseDesignModel
    {
        public string LText { get; set; }

        public string ComName { get; set; }

        public string OpenText { get; set; }

        public string CloseText { get; set; }

    }

    public class EGroupBoxModel : BaseDesignModel
    {
        public string LText { get; set; }
    }


    public class EComboModel : BaseDesignModel
    {
        public string LText { get; set; }
        public string LinkValue { get; set; }
        public string ListValue { get; set; }
    }
}
