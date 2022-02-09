using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SequenceTestModel
{
    /// <summary>
    /// PLC通讯Model
    /// </summary>
    public class PLCSetModel : BaseSeqModel
    {
        public PLCTYPE plcType { get; set; }
        public string ConnType { get; set; }
        public string ConnObj { get; set; }

        public int StationNum { get; set; }

        [XmlIgnore]
        public bool IsConnected { get; set; }

        public PLCSetModel Clone()
        {
            PLCSetModel tModel = new PLCSetModel();
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
                return FeatureType.PLC通讯.ToString();
            }

            set
            {
                value = FeatureType.PLC通讯.ToString();
            }
        }

        public PLCSetModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        { 

        }
    }

    public enum PLCTYPE
    {
        欧姆龙,
        三菱,
        西门子,
        ABB,
        松下,
        汇川,
        台达,
    }

    /// <summary>
    /// PLC开始Model
    /// </summary>
    public class PLCStartModel : BaseSeqModel, IReport
    {
        public string PLCForm { get; set; }
        public PLCStartModel Clone()
        {
            PLCStartModel tModel = new PLCStartModel();
            tModel.Name = Name;
            tModel.PLCForm = PLCForm;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.PLC通讯.ToString();
            }

            set
            {
                value = FeatureType.PLC通讯.ToString();
            }
        }

        public PLCStartModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }

    /// <summary>
    /// PLC写入Model
    /// </summary>
    public class PLCWriteModel : BaseSeqModel, IReport
    {
        public string PLCForm { get; set; }
        public string WriteDevice { get; set; }
        public string OKValue { get; set; }
        public string NGValue { get; set; }
        public string Block { get; set; }//块号

        public bool IsJudgeNG { get; set; }
        public string StartItem { get; set; }
        public string EndItem { get; set; }

        public bool IsNoLog { get; set; }

        public int Length { get; set; }
        public PLCWriteModel Clone()
        {
            PLCWriteModel tModel = new PLCWriteModel();
            tModel.Name = Name;
            tModel.PLCForm = PLCForm;
            tModel.WriteDevice = WriteDevice;
            tModel.OKValue = OKValue;
            tModel.NGValue = NGValue;
            tModel.IsJudgeNG = IsJudgeNG;
            tModel.StartItem = StartItem;
            tModel.EndItem = EndItem;
            tModel.Block = Block;
            tModel.Length = Length;
            tModel.IsNoLog = IsNoLog;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.PLC写入.ToString();
            }

            set
            {
                value = FeatureType.PLC写入.ToString();
            }
        }

        public PLCWriteModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }

    /// <summary>
    /// PLC读取Model
    /// </summary>
    public class PLCReadModel : BaseSeqModel, IReport
    {
        public string PLCForm { get; set; }
        public string ReadDevice { get; set; }
        public string Block { get; set; }//块号
        public bool IsWaitVar { get; set; }
        public string WaitVar { get; set; }
        public int Length { get; set; }
        
        public bool IsRealValue { get; set; }
        public string ScaleValue { get; set; }

        public PLCReadModel Clone()
        {
            PLCReadModel tModel = new PLCReadModel();
            tModel.Name = Name;
            tModel.PLCForm = PLCForm;
            tModel.Block = Block;
            tModel.ReadDevice = ReadDevice;
            tModel.IsWaitVar = IsWaitVar;
            tModel.WaitVar = WaitVar;
            tModel.Length = Length;
            tModel.IsRealValue = IsRealValue;
            tModel.ScaleValue = ScaleValue;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.PLC读取.ToString();
            }

            set
            {
                value = FeatureType.PLC读取.ToString();
            }
        }

        public PLCReadModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("PLC读取的寄存器值")]
            public double ReadValue { get; set; }
        }
    }


    /// <summary>
    /// 接收文件Model
    /// </summary>
    public class RecieveFileModel : BaseSeqModel, IReport
    {
        public RecieveFileModel Clone()
        {
            RecieveFileModel tModel = new RecieveFileModel();
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
                return FeatureType.接收文本.ToString();
            }

            set
            {
                value = FeatureType.接收文本.ToString();
            }
        }

        public RecieveFileModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }


    /// <summary>
    /// 发送文本Model
    /// </summary>
    public class SendFileModel : BaseSeqModel, IReport
    { 
        public bool IsJudgeResult { get; set; }
        public List<string> ListValue { get; set; }
        public SendFileModel Clone()
        {
            SendFileModel tModel = new SendFileModel();
            tModel.Name = Name; 
            tModel.ListValue = ListValue;
            tModel.IsJudgeResult = IsJudgeResult;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.发送文本.ToString();
            }

            set
            {
                value = FeatureType.发送文本.ToString();
            }
        }

        public SendFileModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }

    /// <summary>
    /// 读取文本Model
    /// </summary>
    public class ReadFileModel : BaseSeqModel, IReport
    {
        public string LoadPath { get; set; }

        public ReadFileModel Clone()
        {
            ReadFileModel tModel = new ReadFileModel();
            tModel.Name = Name;
            tModel.LoadPath = LoadPath;
            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; } 

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.读取文本.ToString();
            }

            set
            {
                value = FeatureType.读取文本.ToString();
            }
        }

        public ReadFileModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("文本的字符串结果数组")]
            public string[] ReadTexts { get; set; }
        }
    }
    
    /// <summary>
    /// 写入文本Model
    /// </summary>
    public class WriteFileModel : BaseSeqModel, IReport
    {
        public string SavePath { get; set; }

        public List<string> ListValue { get; set; }

        public bool IsCheckSame { get; set; }

        public WriteFileModel Clone()
        {
            WriteFileModel tModel = new WriteFileModel();
            tModel.Name = Name;
            tModel.SavePath = SavePath;
            tModel.ListValue = ListValue;
            tModel.IsCheckSame = IsCheckSame;
            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }
        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.写入文本.ToString();
            }

            set
            {
                value = FeatureType.写入文本.ToString();
            }
        }

        public WriteFileModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }

    /// <summary>
    /// Ftp操作 Model
    /// </summary>
    public class FtpUDLoadModel:BaseSeqModel, IReport
    {
        public string FtpName { get; set; }
        public bool IsUpLoad { get; set; }
        public string LoadPath { get; set; }

        public string FileName { get; set; }

        public string SavePath { get; set; }

        public bool IsAddTime { get; set; }

        public bool IsOKNG { get; set; }

        public bool IsOnlyNG { get; set; }
        public FtpUDLoadModel Clone()
        {
            FtpUDLoadModel tModel = new FtpUDLoadModel();
            tModel.Name = Name;
            tModel.FtpName = FtpName;
            tModel.IsUpLoad = IsUpLoad;
            tModel.LoadPath = LoadPath;
            tModel.FileName = FileName;
            tModel.SavePath = SavePath;
            tModel.IsAddTime = IsAddTime;
            tModel.IsOKNG = IsOKNG;
            tModel.IsOnlyNG = IsOnlyNG;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.Ftp操作.ToString();
            }

            set
            {
                value = FeatureType.Ftp操作.ToString();
            }
        }

        public FtpUDLoadModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }


    /// <summary>
    /// 光源控制Model
    /// </summary>
    public class LightControlModel : BaseSeqModel, IReport
    { 
        public string MsgName { get; set; }
        public string SendValue { get; set; }
        public string RetureValue { get; set; }
        public bool IsNgRetry { get; set; }
        public int RetryTimes { get; set; }

        public bool IsJudgeOK { get; set; }
        public LightControlModel Clone()
        {
            LightControlModel tModel = new LightControlModel();
            tModel.Name = Name;
            tModel.MsgName = MsgName;
            tModel.SendValue = SendValue;
            tModel.RetureValue = RetureValue;
            tModel.IsNgRetry = IsNgRetry;
            tModel.RetryTimes = RetryTimes;
            tModel.IsJudgeOK = IsJudgeOK;
            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.光源控制.ToString();
            }

            set
            {
                value = FeatureType.光源控制.ToString();
            }
        }

        public LightControlModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }


    /// <summary>
    /// 批处理Model
    /// </summary>
    public class RunBatchModel : BaseSeqModel, IReport
    {
        public string BatPath { get; set; }

        public bool IsWaitDone { get; set; }

        public RunBatchModel Clone()
        {
            RunBatchModel tModel = new RunBatchModel();
            tModel.Name = Name;
            tModel.BatPath = BatPath;
            tModel.IsWaitDone = IsWaitDone;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.批处理.ToString();
            }

            set
            {
                value = FeatureType.批处理.ToString();
            }
        }

        public RunBatchModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }
    /// <summary>
    /// 连接螺批Model
    /// </summary>
    public class ScrewInitModel : BaseSeqModel, IReport
    {
        public string MsgName { get; set; } 

        public int OffSetNum { get; set; }

        public ScrewInitModel Clone()
        {
            ScrewInitModel tModel = new ScrewInitModel();
            tModel.Name = Name;
            tModel.MsgName = MsgName;
            tModel.OffSetNum = OffSetNum;
            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.连接螺批.ToString();
            }

            set
            {
                value = FeatureType.连接螺批.ToString();
            }
        }

        public ScrewInitModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }
    /// <summary>
    /// 读螺丝批Model
    /// </summary>
    public class ScrewReadModel : BaseSeqModel, IReport
    {
        public string MsgName { get; set; }
        public string ScrewIndex { get; set; }
        public string SnapResult { get; set; }

        public string SetSpeed1 { get; set; }
        public string SetTorque1 { get; set; }
        public string SetAngle1 { get; set; }
        public string SetSpeed2 { get; set; }
        public string SetTorque2 { get; set; }
        public string SetAngle2 { get; set; }
        public string SetSpeed3 { get; set; }
        public string SetTorque3 { get; set; }
        public string SetAngle3 { get; set; }
         
        public ScrewReadModel Clone()
        {
            ScrewReadModel tModel = new ScrewReadModel();
            tModel.Name = Name;
            tModel.MsgName = MsgName;
            tModel.ScrewIndex = ScrewIndex;
            tModel.SnapResult = SnapResult;
            tModel.SetSpeed1 = SetSpeed1;
            tModel.SetTorque1 = SetTorque1;
            tModel.SetAngle1 = SetAngle1;
            tModel.SetSpeed2 = SetSpeed2;
            tModel.SetTorque2 = SetTorque2;
            tModel.SetAngle2 = SetAngle2;
            tModel.SetSpeed3 = SetSpeed3;
            tModel.SetTorque3 = SetTorque3;
            tModel.SetAngle3 = SetAngle3;
            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.读螺丝批.ToString();
            }

            set
            {
                value = FeatureType.读螺丝批.ToString();
            }
        }

        public ScrewReadModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            [Description("结果")]
            public string MtfResult { get; set; }
            [Description("错误信息")]
            public string ErrorInfo { get; set; }
            [Description("扭矩峰值")]
            public double PeakTorque { get; set; }
            
            [Description("总旋转角度")]
            public double TotalAngle { get; set; }
            
            [Description("第一阶段扭矩峰值")]
            public double StepActualTorque1 { get; set; }
            
            [Description("第一阶段旋转角度")]
            public double StepActualAngle1 { get; set; }
            
            [Description("第二阶段扭矩峰值")]
            public double StepActualTorque2 { get; set; }
            
            [Description("第二阶段旋转角度")]
            public double StepActualAngle2 { get; set; }
            
            [Description("第三阶段扭矩峰值")]
            public double StepActualTorque3 { get; set; }
            
            [Description("第三阶段旋转角度")]
            public double StepActualAngle3 { get; set; }


            [Description("第一阶段设置扭矩")]
            public string SetTorque1 { get; set; }
            [Description("第一阶段设置速度")]
            public string SetSpeed1 { get; set; }
            [Description("第一阶段设置角度")]
            public string SetAngle1 { get; set; }
            [Description("第二阶段设置扭矩")]
            public string SetTorque2 { get; set; }
            [Description("第二阶段设置速度")]
            public string SetSpeed2 { get; set; }
            [Description("第二阶段设置角度")]
            public string SetAngle2 { get; set; }
            [Description("第三阶段设置扭矩")]
            public string SetTorque3 { get; set; }
            [Description("第三阶段设置速度")]
            public string SetSpeed3 { get; set; }
            [Description("第三阶段设置角度")]
            public string SetAngle3 { get; set; }
        }
    }

    /// <summary>
    /// 批量写入Model
    /// </summary>
    public class PLCLotsWriteModel : BaseSeqModel, IReport
    {
        public string PLCForm { get; set; }
        public List<WriteClass> ListWrite { get; set; }
      
        public PLCLotsWriteModel Clone()
        {
            PLCLotsWriteModel tModel = new PLCLotsWriteModel();
            tModel.Name = Name;
            tModel.PLCForm = PLCForm;
            tModel.ListWrite = ListWrite;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.批量写入.ToString();
            }

            set
            {
                value = FeatureType.批量写入.ToString();
            }
        }

        public PLCLotsWriteModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }

        public class WriteClass
        {
            public string WriteDevice { get; set; }
            public string Block { get; set; }//块号
            public string OKValue { get; set; }
            public string NGValue { get; set; } 
            public string StartItem { get; set; }
            public string EndItem { get; set; }
            public bool IsJudgeNG { get; set; }
        }
    }

    /// <summary>
    /// 上传系统Model
    /// </summary>
    public class UploadSFCModel : BaseSeqModel, IReport
    {
        public string Pid { get; set; }
        public string Fixture { get; set; }
        public bool IsOnlyConfig { get; set; }
        public bool IsUploadOk { get; set; }
        //public string UserName { get; set; }
        //public string PassWord { get; set; }
        //public string StationID { get; set; }
        //public string Lang { get; set; }
        //public string Site { get; set; }
        //public string Bu { get; set; }
        //public string AccType { get; set; }

        //上传序号
        public int Index { get; set; }

        

        public UploadSFCModel Clone()
        {
            UploadSFCModel tModel = new UploadSFCModel();
            tModel.Name = Name;
            tModel.Pid = Pid;
            tModel.Fixture = Fixture;
            tModel.IsOnlyConfig = IsOnlyConfig;
            tModel.IsUploadOk = IsUploadOk;
            //tModel.UserName = UserName;
            //tModel.PassWord = PassWord;
            //tModel.StationID = StationID;
            //tModel.Lang = Lang;
            //tModel.Site = Site;
            //tModel.Bu = Bu;
            //tModel.AccType = AccType;
            tModel.Index = Index;
            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.上传SFC.ToString();
            }

            set
            {
                value = FeatureType.上传SFC.ToString();
            }
        }

        public UploadSFCModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            public string DSN { get; set; }
        }
    }

    /// <summary>
    /// 上传SAPModel
    /// </summary>
    public class SAPControlModel : BaseSeqModel, IReport
    {
        public string Address { get; set; }
        public string Lang { get; set; }
        public string Port { get; set; }
        public string SystemName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SystemNum { get; set; }
        public string QRCode { get; set; }
        public string TaxAB { get; set; }
        public string JobNum { get; set; } 

        public SAPControlModel Clone()
        {
            SAPControlModel tModel = new SAPControlModel();
            tModel.Name = Name;
            tModel.Address = Address;
            tModel.Lang = Lang;
            tModel.Port = Port;
            tModel.SystemName = SystemName;
            tModel.UserName = UserName;
            tModel.Password = Password;
            tModel.SystemNum = SystemNum;
            tModel.QRCode = QRCode;
            tModel.TaxAB = TaxAB;
            tModel.JobNum = JobNum;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.上传SAP.ToString();
            }

            set
            {
                value = FeatureType.上传SAP.ToString();
            }
        }

        public SAPControlModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            public string Status { get; set; }
            public string Batch { get; set; }
            public string Return { get; set; }
        }
    }

    /// <summary>
    /// 斑马打印Model
    /// </summary>
    public class ZebraPrintModel : BaseSeqModel, IReport
    {
        /// <summary>
        /// 打印机名称
        /// </summary>
        public string PrintName { get; set; }
         
        /// <summary>
        /// 品名
        /// </summary>
        public string PartName { get; set; }
        /// <summary>
        /// 批次
        /// </summary>
        public string Batch { get; set; }
        /// <summary>
        /// 料号
        /// </summary>
        public string PartNumber { get; set; }
        /// <summary>
        /// 制造日期
        /// </summary>
        public string ManuDate { get; set; }


        public ZebraPrintModel Clone()
        {
            ZebraPrintModel tModel = new ZebraPrintModel();
            tModel.Name = Name;
            tModel.PrintName = PrintName;
            tModel.PartName = PartName;
            tModel.Batch = Batch;
            tModel.PartNumber = PartNumber;
            tModel.ManuDate = ManuDate;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.斑马打印.ToString();
            }

            set
            {
                value = FeatureType.斑马打印.ToString();
            }
        }

        public ZebraPrintModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }
}
