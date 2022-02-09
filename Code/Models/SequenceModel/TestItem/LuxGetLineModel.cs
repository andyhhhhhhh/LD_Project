using HalconDotNet;
using SequenceTestModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SequenceTestModel
{
    public class LuxGetLineModel : BaseSeqModel, ICheck3D
    {
        public string ImageForm { get; set; }
        public double MinGray { get; set; }
        public double MaxGray { get; set; }
        public double LineMinGray { get; set; }

        //线交点坐标 

        public string BaseRegionForm { get; set; }
        public string CheckRegionForm { get; set; }
          

        public string ModelForm { get; set; }


        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.取线轮廓.ToString();
            }

            set
            {
                value = FeatureType.取线轮廓.ToString();
            }
        }

        public LuxGetLineModel Clone()
        {
            LuxGetLineModel tModel = new LuxGetLineModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.MinGray = MinGray;
            tModel.MaxGray = MaxGray;
            tModel.LineMinGray = LineMinGray; 
            tModel.BaseRegionForm = BaseRegionForm;
            tModel.CheckRegionForm = CheckRegionForm; 
            tModel.ModelForm = ModelForm;

            return tModel;
        }

        public LuxGetLineModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("线的轮廓")]
            public HObject OutRegion { get; set; }

            [Description("中心线的上点Row")]
            public double UpPointRow { get; set; }
            [Description("中心线的上点Column")]
            public double UpPointCol { get; set; }
            [Description("中心线的中点Row")]
            public double CenterPointRow { get; set; }
            [Description("中心线的中点Column")]
            public double CenterPointCol { get; set; }
            [Description("中心线的下点Row")]
            public double DownPointRow { get; set; }
            [Description("中心线的下点Column")]
            public double DownPointCol { get; set; }

            [Description("线的起始点Row")]
            public double LineBeginRow { get; set; }

            [Description("线的起始点Column")]
            public double LineBeginCol { get; set; }
             
            [Description("线的上终点Row")]
            public double LineUpEndRow { get; set; }

            [Description("线的上终点Column")]
            public double LineUpEndCol { get; set; }

            [Description("线的左终点Row")]
            public double LineLeftEndRow { get; set; }

            [Description("线的左终点Column")]
            public double LineLeftEndCol { get; set; }

        }
    }

    //区域到基准图像的距离
    public class LuxAreaToBaseModel : BaseSeqModel, ICheck3D
    {
        public string ImageForm { get; set; } 

         
        public string CheckRegionForm { get; set; }

        public double MinValue { get; set; }
        public double MaxValue { get; set; }

        public string ModeType { get; set; }
        public string FilterType { get; set; }
        public int IValue { get; set; }
        public int JValue { get; set; }
        public bool IsIPercent { get; set; }
        public bool IsJPercent { get; set; }
         

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.区域高度.ToString();
            }

            set
            {
                value = FeatureType.区域高度.ToString();
            }
        }

        public LuxAreaToBaseModel Clone()
        {
            LuxAreaToBaseModel tModel = new LuxAreaToBaseModel();
            tModel.Name = Name;
            tModel.ImageForm = ImageForm;
            tModel.CheckRegionForm = CheckRegionForm;
            tModel.MinValue = MinValue;
            tModel.MaxValue = MaxValue;
            tModel.ModeType = ModeType;
            tModel.FilterType = FilterType;
            tModel.IValue = IValue;
            tModel.JValue = JValue;
            tModel.IsIPercent = IsIPercent;
            tModel.IsJPercent = IsJPercent;

            return tModel;
        }

        public LuxAreaToBaseModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("区域的高度值")]
            public double HeightValue { get; set; }

        }
    }

    //展示结果到主界面
    public class LuxShowResultModel : BaseSeqModel, IDataOperate
    {
        public List<string> listVar { get; set; }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.结果显示.ToString();
            }

            set
            {
                value = FeatureType.结果显示.ToString();
            }
        }

        public LuxShowResultModel Clone()
        {
            LuxShowResultModel tModel = new LuxShowResultModel();
            tModel.Name = Name;
            tModel.listVar = listVar;

            return tModel;
        }

        public LuxShowResultModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }

    //供主界面显示结果用
    public class ShowResultValue
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public string Result { get; set; }
    }

}
