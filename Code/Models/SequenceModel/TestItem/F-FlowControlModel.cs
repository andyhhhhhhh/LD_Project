using SequenceTestModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

/// <summary>
/// 系统流程 集合
/// </summary>
namespace SequenceTestModel
{
    /// <summary>
    /// 流程触发Model
    /// </summary>
    public class ProcessTrigModel : BaseSeqModel, IFlowControl
    {
        public string TrigValue { get; set; }

        //通讯方式
        public string MsgFunc { get; set; }
        public string MsgName { get; set; } 

        public bool IsParamExist { get; set; }

        public string SendMsg { get; set; }

        public bool IsSendMsg { get; set; }

        public bool IsEqual { get; set; }

        public List<string> ListVar { get; set; }

        public ProcessTrigModel Clone()
        {
            ProcessTrigModel tModel = new ProcessTrigModel();
            tModel.Name = Name;
            tModel.Id = Id;
            tModel.TrigValue = TrigValue;
            tModel.MsgFunc = MsgFunc;
            tModel.MsgName = MsgName;
            tModel.IsParamExist = IsParamExist;
            tModel.ListVar = ListVar;
            tModel.SendMsg = SendMsg;
            tModel.IsSendMsg = IsSendMsg;
            tModel.IsEqual = IsEqual;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.流程触发.ToString();
            }

            set
            {
                value = FeatureType.流程触发.ToString();
            }
        }

        public ProcessTrigModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            //add 20200113新增收SN
            [Description("接收SN")]
            public string SN { get; set; }
        }
    }


    /// <summary>
    /// 延时操作(单位:ms) Model
    /// </summary>
    public class DelaySetModel : BaseSeqModel, IFlowControl
    {
        public int DelayTime { get; set; }
        public DelaySetModel Clone()
        {
            DelaySetModel tModel = new DelaySetModel();
            tModel.Name = Name;
            tModel.DelayTime = DelayTime;

            return tModel;
        }


        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.延时等待.ToString();
            }

            set
            {
                value = FeatureType.延时等待.ToString();
            }
        }

        public DelaySetModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }


    /// <summary>
    /// 发送消息Model
    /// </summary> 
    public class SendMessageModel : BaseSeqModel, IFlowControl
    {
        public string OKValue { get; set; }
        public string NGValue { get; set; }

        //通讯方式
        public string MsgFunc { get; set; }
        public string MsgName { get; set; }

        public bool IsBoxValue { get; set; }

        public string BoxValue { get; set; }

        public List<string> ListVar { get; set; }

        public bool IsJudgeNG { get; set; }
        public string StartItem { get; set; }
        public string EndItem { get; set; }

        public SendMessageModel Clone()
        {
            SendMessageModel tModel = new SendMessageModel();
            tModel.Name = Name;
            tModel.Id = Id;
            tModel.OKValue = OKValue;
            tModel.NGValue = NGValue;
            tModel.MsgFunc = MsgFunc;
            tModel.MsgName = MsgName;
            tModel.ListVar = ListVar;
            tModel.IsBoxValue = IsBoxValue;
            tModel.BoxValue = BoxValue;
            tModel.IsJudgeNG = IsJudgeNG;
            tModel.StartItem = StartItem;
            tModel.EndItem = EndItem;

            return tModel;
        }


        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.发送消息.ToString();
            }

            set
            {
                value = FeatureType.发送消息.ToString();
            }
        }

        public SendMessageModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
        }
    }

    /// <summary>
    /// 接收消息Model
    /// </summary> 
    public class ReciveMsgModel : BaseSeqModel, IFlowControl
    {
        //通讯方式
        public string MsgFunc { get; set; }
        public string MsgName { get; set; }

        public bool IsTimeOut { get; set; }

        public int TimeOut { get; set; }

        //是否比对彩盒码
        public bool IsJudegCode { get; set; }

        public ReciveMsgModel Clone()
        {
            ReciveMsgModel tModel = new ReciveMsgModel();
            tModel.Name = Name;
            tModel.Id = Id;
            tModel.IsTimeOut = IsTimeOut;
            tModel.TimeOut = TimeOut;
            tModel.IsJudegCode = IsJudegCode;

            return tModel;
        }


        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.接收消息.ToString();
            }

            set
            {
                value = FeatureType.接收消息.ToString();
            }
        }

        public ReciveMsgModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("接收的字符串内容")]
            public string ReciveText { get; set; }
        }
    }


    /// <summary>
    /// 调用流程Model
    /// </summary> 
    public class CallSubProModel : BaseSeqModel, IFlowControl
    {
        public string SubProName { get; set; }
        public bool bAsync { get; set; }
        public CallSubProModel Clone()
        {
            CallSubProModel tModel = new CallSubProModel();
            tModel.Name = Name;
            tModel.SubProName = SubProName;
            tModel.bAsync = bAsync;

            return tModel;
        }


        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.调用流程.ToString();
            }

            set
            {
                value = FeatureType.调用流程.ToString();
            }
        }

        public CallSubProModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }


    /// <summary>
    /// 循环开始Model
    /// </summary>
    public class LoopStartModel : BaseSeqModel, IFlowControl
    {
        public int LoopTimes { get; set; }
        public bool bAlwaysLoop { get; set; }
        public LoopStartModel Clone()
        {
            LoopStartModel tModel = new LoopStartModel();
            tModel.Name = Name;
            tModel.LoopTimes = LoopTimes;
            tModel.bAlwaysLoop = bAlwaysLoop;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.循环开始.ToString();
            }

            set
            {
                value = FeatureType.循环开始.ToString();
            }
        }

        public LoopStartModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }


    /// <summary>
    /// 循环结束Model
    /// </summary>
    public class LoopEndModel : BaseSeqModel, IFlowControl
    {
        public string LoopStartName { get; set; }
        public LoopEndModel Clone()
        {
            LoopEndModel tModel = new LoopEndModel();
            tModel.Name = Name;
            tModel.LoopStartName = LoopStartName;
            return tModel;
        }


        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.循环结束.ToString();
            }

            set
            {
                value = FeatureType.循环结束.ToString();
            }
        }

        public LoopEndModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }


    /// <summary>
    /// 执行片段Model
    /// </summary>
    public class StartPartModel : BaseSeqModel, IFlowControl
    { 
        public StartPartModel Clone()
        {
            StartPartModel tModel = new StartPartModel();
            tModel.Name = Name;
            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.执行片段.ToString();
            }

            set
            {
                value = FeatureType.执行片段.ToString();
            }
        }

        public StartPartModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }


    /// <summary>
    /// 停止片段Model
    /// </summary>
    public class StopPartModel : BaseSeqModel, IFlowControl
    {
        public string PartStartName { get; set; }
        public StopPartModel Clone()
        {
            StopPartModel tModel = new StopPartModel();
            tModel.Name = Name;
            tModel.PartStartName = PartStartName;
            return tModel;
        }


        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.停止片段.ToString();
            }

            set
            {
                value = FeatureType.停止片段.ToString();
            }
        }

        public StopPartModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }


    /// <summary>
    /// 条件分支Model
    /// </summary>
    public class IfElseModel : BaseSeqModel, IFlowControl
    {        
        public bool bExpressValue { get; set; }
        public string ExpressVale { get; set; }
        public IfElseModel Clone()
        {
            IfElseModel tModel = new IfElseModel();
            tModel.Name = Name;
            tModel.bExpressValue = bExpressValue;
            tModel.ExpressVale = ExpressVale;
            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.条件分支.ToString();
            }

            set
            {
                value = FeatureType.条件分支.ToString();
            }
        }

        public IfElseModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }

    /// <summary>
    /// 判断跳转Model
    /// </summary>
    public class IfGoModel : BaseSeqModel, IFlowControl
    {
        public bool bExpressValue { get; set; }
        public string ExpressVale { get; set; }
        public string BgoFrom { get; set; }
        public string PassGo { get; set; } 
        public string FailGo { get; set; }
        public IfGoModel Clone()
        {
            IfGoModel tModel = new IfGoModel();
            tModel.Name = Name;
            tModel.bExpressValue = bExpressValue;
            tModel.ExpressVale = ExpressVale;
            tModel.BgoFrom = BgoFrom;
            tModel.PassGo = PassGo;
            tModel.FailGo = FailGo; 
            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.判断跳转.ToString();
            }

            set
            {
                value = FeatureType.判断跳转.ToString();
            }
        }

        public IfGoModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }

    /// <summary>
    /// 弹框提示Model
    /// </summary>
    public class ShowBoxModel : BaseSeqModel, IFlowControl
    {
        public string BoxValue { get; set; }
        public ShowBoxModel Clone()
        {
            ShowBoxModel tModel = new ShowBoxModel();
            tModel.Name = Name;
            tModel.BoxValue = BoxValue;
            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.弹框提示.ToString();
            }

            set
            {
                value = FeatureType.弹框提示.ToString();
            }
        }

        public ShowBoxModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }


    /// <summary>
    /// 条件选择Model
    /// </summary>
    public class SwitchCaseModel : BaseSeqModel, IFlowControl
    {
        public string SwitchValue { get; set; }
        public string CaseValue { get; set; }
        public string SwitchForm { get; set; }
        public SwitchCaseModel Clone()
        {
            SwitchCaseModel tModel = new SwitchCaseModel();
            tModel.Name = Name;
            tModel.SwitchValue = SwitchValue;
            tModel.CaseValue = CaseValue;
            tModel.SwitchForm = SwitchForm;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.条件选择.ToString();
            }

            set
            {
                value = FeatureType.条件选择.ToString();
            }
        }

        public SwitchCaseModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("条件选择的结果")]
            public string SwitchResult { get; set; }
        }
    }


    /// <summary>
    /// 切换方案Model
    /// </summary>
    public class ChangeProjectModel : BaseSeqModel, IFlowControl
    {
        public string ProjectPath { get; set; }
        public ChangeProjectModel Clone()
        {
            ChangeProjectModel tModel = new ChangeProjectModel();
            tModel.Name = Name;
            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.切换方案.ToString();
            }

            set
            {
                value = FeatureType.切换方案.ToString();
            }
        }

        public ChangeProjectModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }

    /// <summary>
    /// 流程结果Model
    /// </summary>
    public class SeqResultModel : BaseSeqModel, IFlowControl
    {
        public string ValueForm { get; set; }
        public bool IsJudgeNG { get; set; }
        public string StartItem { get; set; }
        public string EndItem { get; set; }
        public SeqResultModel Clone()
        {
            SeqResultModel tModel = new SeqResultModel();
            tModel.Name = Name;
            tModel.ValueForm = ValueForm;
            tModel.IsJudgeNG = IsJudgeNG;
            tModel.StartItem = StartItem;
            tModel.EndItem = EndItem;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.流程结果.ToString();
            }

            set
            {
                value = FeatureType.流程结果.ToString();
            }
        }

        public SeqResultModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }
}
