using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SequenceTestModel
{
    /// <summary>
    /// Sequence根节点
    /// </summary>
    [XmlRoot("Sequence")]
    public class SequenceModel
    {
        [XmlAttribute("Id")]
        public int Id { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 指向程序的一些配置文件保存路径
        /// </summary>
        [XmlAttribute("BasePath")]        
        public string BasePath { get; set; }

        /// <summary>
        /// 3D产品的型号 --- 暂时没用到
        /// </summary>
        [XmlAttribute("ProductVersion")]
        public string ProductVersion { get; set; }

        /// <summary>
        /// 产品的型号
        /// </summary>
        [XmlAttribute("ProductInfo")]
        public string ProductInfo { get; set; }

        /// <summary>
        /// 当前路径调试用 --- 旧版本用到
        /// </summary>
        [XmlAttribute("LocalPath")]
        public string LocalPath { get; set; }

        /// <summary>
        /// 窗口图形的数量
        /// </summary>
        [XmlAttribute("LayOut")]
        public int LayOutNum { get; set; }

        /// <summary>
        /// 3D相机配置信息
        /// </summary>
        [XmlElement("Camera3DSet")]
        public List<Camera3DSetModel> Camera3DSet { get; set; }

        /// <summary>
        /// 2D相机配置信息
        /// </summary>
        [XmlElement("Camera2DSetModels")]
        public List<Camera2DSetModel> Camera2DSetModels { get; set; }

        /// <summary>
        /// 流程集合List
        /// </summary>
        [XmlArrayItem("ChildSequenceModels")]
        public List<ChildSequenceModel> ChildSequenceModels { get;set;}

        /// <summary>
        /// 全局变量集合List
        /// </summary>
        [XmlArrayItem("VariableSetModels")]
        public List<VariableSetModel> VariableSetModels { get; set; }

        /// <summary>
        /// TCPIP 通讯集合List
        /// </summary>
        [XmlArrayItem("TCPIPModels")]
        public List<TCPIPModel> TCPIPModels { get; set; }

        /// <summary>
        /// 串口 通讯集合List
        /// </summary>
        [XmlArrayItem("ComModels")]
        public List<ComModel> ComModels { get; set; }

        /// <summary>
        /// PLC 通讯集合List
        /// </summary>
        [XmlArrayItem("PLCSetModels")]
        public List<PLCSetModel> PLCSetModels { get; set; }

        /// <summary>
        /// Ftp客户端 集合List
        /// </summary>
        [XmlArrayItem("FtpClients")]
        public List<FtpClientModel> FtpClientModels { get; set; }

        /// <summary>
        /// 产品信息 集合List
        /// </summary>
        [XmlArrayItem("ProductInfoModels")]
        public List<ProductInfoModel> ProductInfoModels { get; set; }
         
        /// <summary>
        /// 产品信息 选择List
        /// </summary>
        [XmlArrayItem("ProductSelModels")]
        public List<ProductSelModel> ProductSelModels { get; set; }
         
        /// <summary>
        /// SFC 集合
        /// </summary>
        [XmlElement("SfcModel")]
        public SFCModel SfcModel { get; set; }
        
        /// <summary>
        /// LD 集合
        /// </summary>
        [XmlElement("LDModel")]
        public LDModel LDModel { get; set; }

        /// <summary>
        /// 生产数据 集合
        /// </summary>
        [XmlElement("productDataModel")]
        public ProductDataModel productDataModel { get; set; }

        /// <summary>
        /// 算法 
        /// </summary>
        [XmlElement("algorithmModel")]
        public  List<AlgorithmModel> algorithmModels { get; set; }

        /// <summary>
        /// 标定参数 
        /// </summary>
        [XmlElement("calibrationModel")]
        public CalibrationModel calibrationModel { get; set; }

        /// <summary>
        /// 报警配置Model
        /// </summary>
        [XmlElement("alarmConfigModel")]
        public AlarmConfigModel alarmConfigModel { get; set; }

        /// <summary>
        /// 算法 
        /// </summary>
        [XmlElement("fixtureAlgorithmModel")]
        public List<FixtureAlgorithmModel> fixtureAlgorithmModels { get; set; }

        /// <summary>
        /// 治具示教参数 
        /// </summary>
        [XmlElement("fxitureTeachModels")]
        public List<FixtureTeachModel> fixtureTeachModels { get; set; }

        public SequenceModel()
        {
            Id = 1;
            Name = "";
            Camera3DSet = new List<Camera3DSetModel>();
            Camera2DSetModels = new List<Camera2DSetModel>();
            ChildSequenceModels = new List<ChildSequenceModel>();
            VariableSetModels = new List<VariableSetModel>();
            TCPIPModels = new List<TCPIPModel>();
            ComModels = new List<ComModel>();
            PLCSetModels = new List<PLCSetModel>();
            FtpClientModels = new List<FtpClientModel>();
            ProductInfoModels = new List<ProductInfoModel>();
            ProductSelModels = new List<ProductSelModel>();
            SfcModel = new SFCModel();
            LDModel = new LDModel();
            productDataModel = new ProductDataModel();
            algorithmModels = new List<AlgorithmModel>();
            alarmConfigModel = new AlarmConfigModel();
            calibrationModel = new CalibrationModel();
            fixtureAlgorithmModels = new List<FixtureAlgorithmModel>();
            fixtureTeachModels = new List<FixtureTeachModel>();
        }

        public SequenceModel Clone()
        {
            SequenceModel tModel = new SequenceModel();
            tModel.Id = Id;
            tModel.Name = Name;
            tModel.BasePath = BasePath;
            tModel.ProductVersion = ProductVersion;
            tModel.ProductInfo = ProductInfo;
            tModel.LocalPath = LocalPath;
            tModel.LayOutNum = LayOutNum;
            tModel.Camera3DSet = Camera3DSet;
            tModel.Camera2DSetModels = Camera2DSetModels;
            tModel.ChildSequenceModels = ChildSequenceModels;
            tModel.VariableSetModels = VariableSetModels;
            tModel.TCPIPModels = TCPIPModels;
            tModel.ComModels = ComModels;
            tModel.PLCSetModels = PLCSetModels;
            tModel.FtpClientModels = FtpClientModels;
            tModel.ProductInfoModels = ProductInfoModels;
            tModel.ProductSelModels = ProductSelModels;
            tModel.SfcModel = SfcModel;
            tModel.algorithmModels = algorithmModels;
            tModel.fixtureAlgorithmModels = fixtureAlgorithmModels;
            tModel.fixtureTeachModels = fixtureTeachModels;

            return tModel;
        }
    }

    public class ChildSequenceModel
    {
        [XmlAttribute("Id")]
        public int Id { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 流程是否启用
        /// </summary>
        [XmlAttribute("bEnable")]
        public bool bEnable { get; set; }

        /// <summary>
        /// 是否为子流程 子流程不主动运行 true:子流程 false:非子流程
        /// </summary>
        [XmlAttribute("SubProcess")]
        public bool SubProcess { get; set; }

        /// <summary>
        /// 流程别名
        /// </summary>
        [XmlAttribute("AnotherName")]
        public string AnotherName { get; set; }

        /// <summary>
        /// 当前运行测试项目--不保存
        /// </summary>
        [XmlIgnore]
        public string RunningName { get; set; }

        /// <summary>
        /// 是否是单次运行流程
        /// </summary>
        [XmlAttribute("IsOnceProcess")]
        public bool IsOnceProcess { get; set; }

        /// <summary>
        /// 是否是单线程单元
        /// </summary>
        [XmlAttribute("IsSTAThread")]
        public bool IsSTAThread { get; set; }

        /// <summary>
        /// 流程间隔时间 ms
        /// </summary>
        [XmlAttribute("SeqSleep")]
        public int SeqSleep { get; set; }

        /// <summary>
        /// 模块间隔时间 ms
        /// </summary>
        [XmlAttribute("ModelSleep")]
        public int ModelSleep { get; set; }

        /// <summary>
        /// 流程集合List
        /// </summary>
        [XmlArrayItem("SingleSequenceModel")]
        public List<SingleSequenceModel> SingleSequenceModels { get; set; }

        public ChildSequenceModel()
        {
            Id = 1;
            Name = "流程" + Id.ToString();
            AnotherName = "流程" + Id.ToString();
            IsOnceProcess = false;
            SubProcess = false;
            bEnable = true;
            SeqSleep = 300;
            ModelSleep = 5;

            SingleSequenceModels = new List<SingleSequenceModel>();
        }

        public List<CheckFeatureModel> GetCheckFeatureList()
        {
            try
            {
                List<CheckFeatureModel> listModel = new List<CheckFeatureModel>();
                foreach (var item in SingleSequenceModels)
                {
                    listModel.AddRange(item.CheckFeatureModels);
                }
                return listModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    /// <summary>
    /// 单个流程空间Model
    /// </summary>
    public class SingleSequenceModel
    {
        [XmlAttribute("Id")]
        public int Id { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 流程是否启用
        /// </summary>
        [XmlAttribute("bEnable")]
        public bool bEnable { get; set; } 

        /// <summary>
        /// 是否为子流程 子流程不主动运行 true:子流程 false:非子流程
        /// </summary>
        //[XmlAttribute("SubProcess")]
        //public bool SubProcess { get; set; } 

        /// <summary>
        /// 流程别名
        /// </summary>
        [XmlAttribute("AnotherName")]
        public string AnotherName { get; set; }

        /// <summary>
        /// 当前运行测试项目--不保存
        /// </summary>
        [XmlIgnore] 
        public string RunningName { get; set; }

        /// <summary>
        /// 上级Sequence名称
        /// </summary>
        [XmlAttribute("BaseSeqName")]
        public string BaseSeqName { get; set; }

        /// <summary>
        /// 是否是单次运行流程
        /// </summary>
        //[XmlAttribute("IsOnceProcess")]
        //public bool IsOnceProcess { get; set; }

        /// <summary>
        /// 是否是单线程单元
        /// </summary>
        //[XmlAttribute("IsSTAThread")]
        //public bool IsSTAThread { get; set; }

        /// <summary>
        /// 流程间隔时间 ms
        /// </summary>
        //[XmlAttribute("SeqSleep")]
        //public int SeqSleep { get; set; }

        /// <summary>
        /// 模块间隔时间 ms
        /// </summary>
        //[XmlAttribute("ModelSleep")]
        //public int ModelSleep { get; set; }


        /// <summary>
        /// 流程集合
        /// </summary>
        [XmlArrayItem("CheckFeatureModels")]
        public List<CheckFeatureModel> CheckFeatureModels { get; set; }

        /****************** 以下为所有单个模块的集合******************/

        //[XmlArrayItem("CommunicationModels")]
        //public List<CommunicationModel> CommunicationModels { get; set; }      
        
        #region 图像处理
        //
        [XmlArrayItem("SnapImageModels")]
        public List<SnapImageModel> SnapImageModels { get; set; }

        [XmlArrayItem("ImageSaveModels")]
        public List<ImageSaveModel> ImageSaveModels { get; set; }

        [XmlArrayItem("Camera2DSnapModels")]
        public List<Camera2DSnapModel> Camera2DSnapModels { get; set; }

        [XmlArrayItem("PretreatImageModels")]
        public List<PretreatImageModel> PretreatImageModels { get; set; }

        [XmlArrayItem("DisplayImgModels")]
        public List<DisplayImgModel> DisplayImgModels { get; set; }

        [XmlArrayItem("ImageCutModels")]
        public List<ImageCutModel> ImageCutModels { get; set; }

        [XmlArrayItem("ThreshodSegModels")]
        public List<ThreshodSegModel> ThreshodSegModels { get; set; }

        [XmlArrayItem("ThresholdImgModels")]
        public List<ThresholdImgModel> ThresholdImgModels { get; set; }

        [XmlArrayItem("PolarImageModels")]
        public List<PolarImageModel> PolarImageModels { get; set; }

        [XmlArrayItem("ImageSubModels")]
        public List<ImageSubModel> ImageSubModels { get; set; }

        [XmlArrayItem("TransRGBModels")]
        public List<TransRGBModel> TransRGBModels { get; set; }

        [XmlArrayItem("ImageSplitModels")]
        public List<ImageSplitModel> ImageSplitModels { get; set; }

        [XmlArrayItem("MulitCameraSnapModels")]
        public List<MultiCameraSnapModel> MultiCameraSnapModels { get; set; }
        //
        #endregion

        #region 检测识别
        //
        [XmlArrayItem("ReadCode2DModels")]
        public List<ReadCode2DModel> ReadCode2DModels { get; set; }

        [XmlArrayItem("ReadBarCodeModels")]
        public List<ReadBarCodeModel> ReadBarCodeModels { get; set; }

        [XmlArrayItem("CheckAreaModels")]
        public List<CheckAreaModel> CheckAreaModels { get; set; }

        [XmlArrayItem("BaseLevelModels")]
        public List<BaseLevelModel> BaseLevelModels { get; set; }

        [XmlArrayItem("CheckLightModels")]
        public List<CheckLightModel> CheckLightModels { get; set; }

        [XmlArrayItem("CreateRegionModels")]
        public List<CreateRegionModel> CreateRegionModels { get; set; }

        [XmlArrayItem("CountPixModels")]
        public List<CountPixModel> CountPixModels { get; set; }

        [XmlArrayItem("OcrModels")]
        public List<OcrModel> OcrModels { get; set; }

        [XmlArrayItem("DefinitionModels")]
        public List<DefinitionModel> DefinitionModels { get; set; }

        [XmlArrayItem("RectArrayModels")]
        public List<RectArrayModel> RectArrayModels { get; set; }

        [XmlArrayItem("CircleArrayModels")]
        public List<CircleArrayModel> CircleArrayModels { get; set; }

        [XmlArrayItem("CheckReverseModels")]
        public List<CheckReverseModel> CheckReverseModels { get; set; }

        [XmlArrayItem("SpotCheckModels")]
        public List<SpotCheckModel> SpotCheckModels { get; set; }

        [XmlArrayItem("OcrTrainModels")]
        public List<OcrTrainModel> OcrTrainModels { get; set; }

        [XmlArrayItem("ColorExtractModels")]
        public List<ColorExtractModel> ColorExtractModels { get; set; }
        //
        #endregion

        #region 变量工具
        //
        [XmlArrayItem("VariableValueModels")]
        public List<VariableValueModel> VariableValueModels { get; set; }

        [XmlArrayItem("WaitVariableModels")]
        public List<WaitVariableModel> WaitVariableModels { get; set; }

        [XmlArrayItem("LocalVariableModels")]
        public List<LocalVariableModel> LocalVariableModels { get; set; }

        [XmlArrayItem("LocalArrayModels")]
        public List<LocalArrayModel> LocalArrayModels { get; set; }

        [XmlArrayItem("ArraySetModels")]
        public List<ArraySetModel> ArraySetModels { get; set; }

        [XmlArrayItem("SpiltTextModels")]
        public List<SpiltTextModel> SpiltTextModels { get; set; }

        [XmlArrayItem("CreateTextModels")]
        public List<CreateTextModel> CreateTextModels { get; set; }
        [XmlArrayItem("EnQueueModels")]
        public List<EnQueueModel> EnQueueModels { get; set; }

        [XmlArrayItem("DeQueueModels")]
        public List<DeQueueModel> DeQueueModels { get; set; }

        [XmlArrayItem("ClearQueueModels")]
        public List<ClearQueueModel> ClearQueueModels { get; set; }

        [XmlArrayItem("SaveVarModels")]
        public List<SaveVarModel> SaveVarModels { get; set; }

        [XmlArrayItem("ReadVarModels")]
        public List<ReadVarModel> ReadVarModels { get; set; }
        //
        #endregion

        #region 系统流程
        //
        [XmlArrayItem("ProcessTrigModels")]
        public List<ProcessTrigModel> ProcessTrigModels { get; set; }

        [XmlArrayItem("SendMessageModels")]
        public List<SendMessageModel> SendMessageModels { get; set; }

        [XmlArrayItem("DelaySetModels")]
        public List<DelaySetModel> DelaySetModels { get; set; }

        [XmlArrayItem("CallSubProModels")]
        public List<CallSubProModel> CallSubProModels { get; set; }

        [XmlArrayItem("LoopStartModels")]
        public List<LoopStartModel> LoopStartModels { get; set; }

        [XmlArrayItem("LoopEndModels")]
        public List<LoopEndModel> LoopEndModels { get; set; }

        [XmlArrayItem("StartPartModels")]
        public List<StartPartModel> StartPartModels { get; set; }

        [XmlArrayItem("StopPartModels")]
        public List<StopPartModel> StopPartModels { get; set; }

        [XmlArrayItem("IfElseModels")]
        public List<IfElseModel> IfElseModels { get; set; }

        [XmlArrayItem("ReciveMsgModels")]
        public List<ReciveMsgModel> ReciveMsgModels { get; set; }

        [XmlArrayItem("IfGoModels")]
        public List<IfGoModel> IfGoModels { get; set; }

        [XmlArrayItem("ShowBoxModels")]
        public List<ShowBoxModel> ShowBoxModels { get; set; }

        [XmlArrayItem("SwitchCaseModels")]
        public List<SwitchCaseModel> SwitchCaseModels { get; set; }

        [XmlArrayItem("ChangeProjectModels")]
        public List<ChangeProjectModel> ChangeProjectModels { get; set; }

        [XmlArrayItem("SeqResultModels")]
        public List<SeqResultModel> SeqResultModels { get; set; }
        //
        #endregion

        #region 定位工具
        //
        [XmlArrayItem("FixedItemModels")]
        public List<FixedItemModel> FixedItemModels { get; set; }

        [XmlArrayItem("MatchingModels")]
        public List<MatchingModel> MatchingModels { get; set; }

        [XmlArrayItem("NccMatchingModels")]
        public List<NccMatchingModel> NccMatchingModels { get; set; }

        [XmlArrayItem("LinePMatchModels")]
        public List<LinePMatchModel> LinePMatchModels { get; set; }

        [XmlArrayItem("LineMatchModels")]
        public List<LineMatchModel> LineMatchModels { get; set; }

        [XmlArrayItem("DrawMatchModels")]
        public List<DrawMatchModel> DrawMatchModels { get; set; }
        //
        #endregion

        #region 几何测量
        //
        [XmlArrayItem("FindLineModels")]
        public List<FindLineModel> FindLineModels { get; set; }

        [XmlArrayItem("FindCircleModels")]
        public List<FindCircleModel> FindCircleModels { get; set; }

        [XmlArrayItem("CircleDDModels")]
        public List<CircleDDModel> CircleDDModels { get; set; }

        [XmlArrayItem("LineDDModels")]
        public List<LineDDModel> LineDDModels { get; set; }

        [XmlArrayItem("LCPointCrossModels")]
        public List<LCPointCrossModel> LCPointCrossModels { get; set; }

        [XmlArrayItem("LLPointCrossModels")]
        public List<LLPointCrossModel> LLPointCrossModels { get; set; }

        [XmlArrayItem("LLAngleModels")]
        public List<LLAngleModel> LLAngleModels { get; set; }

        [XmlArrayItem("CCDistanceModels")]
        public List<CCDistanceModel> CCDistanceModels { get; set; }

        [XmlArrayItem("OneMeasureModels")]
        public List<OneMeasureModel> OneMeasureModels { get; set; }

        [XmlArrayItem("PLDistanceModels")]
        public List<PLDistanceModel> PLDistanceModels { get; set; }

        [XmlArrayItem("PCDistanceModels")]
        public List<PCDistanceModel> PCDistanceModels { get; set; }

        [XmlArrayItem("CCPointCrossModels")]
        public List<CCPointCrossModel> CCPointCrossModels { get; set; }

        [XmlArrayItem("FindCircleArcModels")]
        public List<FindCircleArcModel> FindCircleArcModels { get; set; } 

        [XmlArrayItem("FindRegionCircleModels")]
        public List<FindRegionCircleModel> FindRegionCircleModels { get; set; }

        [XmlArrayItem("MeasureRectModels")]
        public List<MeasureRectModel> MeasureRectModels { get; set; }

        //
        #endregion

        #region 数据操作
        //
        [XmlArrayItem("DataSaveModels")]
        public List<DataSaveModel> DataSaveModels { get; set; }

        [XmlArrayItem("DataOperateModels")]
        public List<DataOperateModel> DataOperateModels { get; set; }

        [XmlArrayItem("ExpressModels")]
        public List<ExpressModel> ExpressModels { get; set; }

        [XmlArrayItem("HADevEngineModels")]
        public List<HADevEngineModel> HADevEngineModels { get; set; }

        [XmlArrayItem("TimeComputeModels")]
        public List<TimeComputeModel> TimeComputeModels { get; set; }

        [XmlArrayItem("ScriptModels")]
        public List<ScriptModel> ScriptModels { get; set; }

        [XmlArrayItem("SelectProductModels")]
        public List<SelectProductModel> SelectProductModels { get; set; }

        [XmlArrayItem("SharpScriptModels")]
        public List<SharpScriptModel> SharpScriptModels { get; set; } 

        [XmlArrayItem("DisplayDataModels")]
        public List<DisplayDataModel> DisplayDataModels { get; set; }

        [XmlArrayItem("SystemTimeModels")]
        public List<SystemTimeModel> SystemTimeModels { get; set; }
        //
        #endregion

        #region 检测3D
        //
        [XmlArrayItem("FlatnessModels")]
        public List<FlatnessModel> FlatnessModels { get; set; }

        [XmlArrayItem("PointToAreaModels")]
        public List<PointToAreaModel> PointToAreaModels { get; set; }

        [XmlArrayItem("ProcessImageModels")]
        public List<ProcessImageModel> ProcessImageModels { get; set; }

        [XmlArrayItem("GenFittingImageModels")]
        public List<GenFittingImageModel> GenFittingImageModels { get; set; }

        [XmlArrayItem("GetSubImageModels")]
        public List<GetSubImageModel> GetSubImageModels { get; set; }

        [XmlArrayItem("CheckKEdgeContourModels")]
        public List<CheckKEdgeContourModel> CheckKEdgeContourModels { get; set; }

        [XmlArrayItem("CheckGlueModels")]
        public List<CheckGlueModel> CheckGlueModels { get; set; }

        [XmlArrayItem("CheckGlueDefectModels")]
        public List<CheckGlueDefectModel> CheckGlueDefectModels { get; set; }

        [XmlArrayItem("CheckGlueFourthModels")]
        public List<CheckGlueFourthModel> CheckGlueFourthModels { get; set; }
        //
        #endregion

        #region 标定工具
        //
        [XmlArrayItem("MeasureCaliModels")]
        public List<MeasureCaliModel> MeasureCaliModels { get; set; }

        [XmlArrayItem("NineCaliModels")]
        public List<NineCaliModel> NineCaliModels { get; set; }

        [XmlArrayItem("CoordMappingModels")]
        public List<CoordMappingModel> CoordMappingModels { get; set; }

        [XmlArrayItem("AffineTransModels")]
        public List<AffineTransModel> AffineTransModels { get; set; }

        [XmlArrayItem("DistrotionCaliModels")]
        public List<DistrotionCaliModel> DistrotionCaliModels { get; set; }

        [XmlArrayItem("MatTransModels")]
        public List<MatTransModel> MatTransModels { get; set; }

        [XmlArrayItem("RegionMapModels")]
        public List<RegionMapModel> RegionMapModels { get; set; }

        [XmlArrayItem("RotateCenterModels")]
        public List<RotateCenterModel> RotateCenterModels { get; set; }
        //
        #endregion

        #region 区域处理     
        //
        [XmlArrayItem("RegionShapeModels")]
        public List<RegionShapeModel> RegionShapeModels { get; set; }

        [XmlArrayItem("RegionFilterModels")]
        public List<RegionFilterModel> RegionFilterModels { get; set; }

        [XmlArrayItem("RegionToContourModels")]
        public List<RegionToContourModel> RegionToContourModels { get; set; }

        [XmlArrayItem("RegionFillModels")]
        public List<RegionFillModel> RegionFillModels { get; set; }

        [XmlArrayItem("RegionComputeModels")]
        public List<RegionComputeModel> RegionComputeModels { get; set; }

        [XmlArrayItem("OpenCloseModels")]
        public List<OpenCloseModel> OpenCloseModels { get; set; }

        [XmlArrayItem("DErosionModels")]
        public List<DErosionModel> DErosionModels { get; set; }

        [XmlArrayItem("AreaCenterModels")]
        public List<AreaCenterModel> AreaCenterModels { get; set; }

        [XmlArrayItem("FeatureDetectModels")]
        public List<FeatureDetectModel> FeatureDetectModels { get; set; }

        [XmlArrayItem("SortRegionModels")]
        public List<SortRegionModel> SortRegionModels { get; set; }

        [XmlArrayItem("PointCreateModels")]
        public List<PointCreateModel> PointCreateModels { get; set; }

        [XmlArrayItem("PointGenCircleModels")]
        public List<PointGenCircleModel> PointGenCircleModels { get; set; }

        [XmlArrayItem("TwoPLineModels")]
        public List<TwoPLineModel> TwoPLineModels { get; set; }
         
        [XmlArrayItem("PartitionRegionModels")]
        public List<PartitionRegionModel> PartitionRegionModels { get; set; }

        [XmlArrayItem("GrayTophatModels")]
        public List<GrayTophatModel> GrayTophatModels { get; set; }
        //
        #endregion

        #region 轮廓处理
        //
        [XmlArrayItem("EdgeExtractModels")]
        public List<EdgeExtractModel> EdgeExtractModels { get; set; }

        [XmlArrayItem("ContourFilterModels")]
        public List<ContourFilterModel> ContourFilterModels { get; set; }

        [XmlArrayItem("ContourIntersectModels")]
        public List<ContourIntersectModel> ContourIntersectModels { get; set; }

        [XmlArrayItem("ContourToRegionModels")]
        public List<ContourToRegionModel> ContourToRegionModels { get; set; }

        [XmlArrayItem("GenCrossTenModels")]
        public List<GenCrossTenModel> GenCrossTenModels { get; set; }
        [XmlArrayItem("SortPointModels")]
        public List<SortPointModel> SortPointModels { get; set; }
        //
        #endregion

        #region 通讯工具
        //
        [XmlArrayItem("PLCStartModels")]
        public List<PLCStartModel> PLCStartModels { get; set; }

        [XmlArrayItem("PLCWriteModels")]
        public List<PLCWriteModel> PLCWriteModels { get; set; }

        [XmlArrayItem("PLCLotsWriteModels")]
        public List<PLCLotsWriteModel> PLCLotsWriteModels { get; set; }

        [XmlArrayItem("PLCReadModels")]
        public List<PLCReadModel> PLCReadModels { get; set; }

        [XmlArrayItem("RecieveFileModels")]
        public List<RecieveFileModel> RecieveFileModels { get; set; }

        [XmlArrayItem("SendFileModels")]
        public List<SendFileModel> SendFileModels { get; set; }

        [XmlArrayItem("ReadFileModels")]
        public List<ReadFileModel> ReadFileModels { get; set; }

        [XmlArrayItem("WriteFileModels")]
        public List<WriteFileModel> WriteFileModels { get; set; }

        [XmlArrayItem("FtpUDLoadModels")]
        public List<FtpUDLoadModel> FtpUDLoadModels { get; set; }

        [XmlArrayItem("LightControlModels")]
        public List<LightControlModel> LightControlModels { get; set; }

        [XmlArrayItem("RunBatchModels")]
        public List<RunBatchModel> RunBatchModels { get; set; }

        [XmlArrayItem("ScrewInitModels")]
        public List<ScrewInitModel> ScrewInitModels { get; set; }

        [XmlArrayItem("ScrewReadModels")]
        public List<ScrewReadModel> ScrewReadModels { get; set; }

        [XmlArrayItem("UploadSFCModels")]
        public List<UploadSFCModel> UploadSFCModels { get; set; }

        [XmlArrayItem("SAPControlModels")]
        public List<SAPControlModel> SAPControlModels { get; set; }

        [XmlArrayItem("ZebraPrintModels")]
        public List<ZebraPrintModel> ZebraPrintModels { get; set; }

        //
        #endregion

        #region 立讯算法
        //
        [XmlArrayItem("LuxGetLineModels")]
        public List<LuxGetLineModel> LuxGetLineModels { get; set; }

        [XmlArrayItem("LuxAreaToBaseModels")]
        public List<LuxAreaToBaseModel> LuxAreaToBaseModels { get; set; }

        [XmlArrayItem("LuxShowResultModels")]
        public List<LuxShowResultModel> LuxShowResultModels { get; set; }
        //
        #endregion

        #region 运动控制

        [XmlArrayItem("IOControlModels")]
        public List<IOControlModel> IOControlModels { get; set; }
        [XmlArrayItem("IOCheckModels")]
        public List<IOCheckModel> IOCheckModels { get; set; }
        [XmlArrayItem("AxisHomeModels")]
        public List<AxisHomeModel> AxisHomeModels { get; set; }
        [XmlArrayItem("CoordMoveModels")]
        public List<CoordMoveModel> CoordMoveModels { get; set; }
        [XmlArrayItem("MoveOffSetModels")]
        public List<MoveOffSetModel> MoveOffSetModels { get; set; }
        [XmlArrayItem("SetSpeedModels")]
        public List<SetSpeedModel> SetSpeedModels { get; set; }
        [XmlArrayItem("StopMoveModels")]
        public List<StopMoveModel> StopMoveModels { get; set; }
        [XmlArrayItem("WaitDoneModels")]
        public List<WaitDoneModel> WaitDoneModels { get; set; }
        [XmlArrayItem("ModifyPosModels")]
        public List<ModifyPosModel> ModifyPosModels { get; set; }
        [XmlArrayItem("GetPosModels")]
        public List<GetPosModel> GetPosModels { get; set; }
        #endregion

        public SingleSequenceModel()
        {
            Id = 1;
            Name = "分流程"+ Id.ToString();
            AnotherName = "分流程" + Id.ToString();
            //IsOnceProcess = false;
            //SubProcess = false;
            bEnable = true;
            //SeqSleep = 100;
            //ModelSleep = 5;

            CheckFeatureModels = new List<CheckFeatureModel>();
            //CommunicationModels = new List<CommunicationModel>();
            //PreImageModels = new List<PreImageModel>();

            
            #region 图像处理
            //
            SnapImageModels = new List<SnapImageModel>();
            ImageSaveModels = new List<ImageSaveModel>();
            Camera2DSnapModels = new List<Camera2DSnapModel>();
            PretreatImageModels = new List<PretreatImageModel>();
            DisplayImgModels = new List<DisplayImgModel>();
            ImageCutModels = new List<ImageCutModel>();
            ThreshodSegModels = new List<ThreshodSegModel>();
            ThresholdImgModels = new List<ThresholdImgModel>();
            PolarImageModels = new List<PolarImageModel>();
            ImageSubModels = new List<ImageSubModel>();
            TransRGBModels = new List<TransRGBModel>();
            ImageSplitModels = new List<ImageSplitModel>();
            MultiCameraSnapModels = new List<MultiCameraSnapModel>();
            //
            #endregion

            #region 检测识别            
            //
            ReadCode2DModels = new List<ReadCode2DModel>();
            ReadBarCodeModels = new List<ReadBarCodeModel>();
            CheckAreaModels = new List<CheckAreaModel>();
            BaseLevelModels = new List<BaseLevelModel>();
            CheckLightModels = new List<CheckLightModel>();
            CreateRegionModels = new List<CreateRegionModel>();
            CountPixModels = new List<CountPixModel>();
            OcrModels = new List<OcrModel>();
            DefinitionModels = new List<DefinitionModel>();
            RectArrayModels = new List<RectArrayModel>();
            CircleArrayModels = new List<CircleArrayModel>();
            CheckReverseModels = new List<CheckReverseModel>();
            SpotCheckModels = new List<SpotCheckModel>();
            OcrTrainModels = new List<OcrTrainModel>();
            ColorExtractModels = new List<ColorExtractModel>();
            //
            #endregion

            #region 变量工具
            //
            VariableValueModels = new List<VariableValueModel>();
            WaitVariableModels = new List<WaitVariableModel>();
            LocalVariableModels = new List<LocalVariableModel>();
            LocalArrayModels = new List<LocalArrayModel>();
            ArraySetModels = new List<ArraySetModel>();
            SpiltTextModels = new List<SpiltTextModel>();
            CreateTextModels = new List<CreateTextModel>();
            EnQueueModels = new List<EnQueueModel>();
            DeQueueModels = new List<DeQueueModel>();
            ClearQueueModels = new List<ClearQueueModel>();
            SaveVarModels = new List<SaveVarModel>();
            ReadVarModels = new List<ReadVarModel>();
            //
            #endregion

            #region 系统流程
            //
            ProcessTrigModels = new List<ProcessTrigModel>();
            SendMessageModels = new List<SendMessageModel>();
            DelaySetModels = new List<DelaySetModel>();
            CallSubProModels = new List<CallSubProModel>();
            LoopStartModels = new List<LoopStartModel>();
            LoopEndModels = new List<LoopEndModel>();
            StartPartModels = new List<StartPartModel>();
            StopPartModels = new List<StopPartModel>();
            IfElseModels = new List<IfElseModel>();
            ReciveMsgModels = new List<ReciveMsgModel>();
            IfGoModels = new List<IfGoModel>();
            ShowBoxModels = new List<ShowBoxModel>();
            SwitchCaseModels = new List<SwitchCaseModel>();
            ChangeProjectModels = new List<ChangeProjectModel>();
            SeqResultModels = new List<SeqResultModel>();
            //
            #endregion

            #region 定位工具
            //
            FixedItemModels = new List<FixedItemModel>();
            MatchingModels = new List<MatchingModel>();
            NccMatchingModels = new List<NccMatchingModel>();
            LinePMatchModels = new List<LinePMatchModel>();
            LineMatchModels = new List<LineMatchModel>();
            DrawMatchModels = new List<DrawMatchModel>();
            //
            #endregion

            #region 几何测量
            //
            FindLineModels = new List<FindLineModel>();
            FindCircleModels = new List<FindCircleModel>();
            CircleDDModels = new List<CircleDDModel>();
            LCPointCrossModels = new List<LCPointCrossModel>();
            LLPointCrossModels = new List<LLPointCrossModel>();
            LLAngleModels = new List<LLAngleModel>();
            CCDistanceModels = new List<CCDistanceModel>();
            PLDistanceModels = new List<PLDistanceModel>();
            PCDistanceModels = new List<PCDistanceModel>();
            TwoPLineModels = new List<TwoPLineModel>();
            FindCircleArcModels = new List<FindCircleArcModel>();
            OneMeasureModels = new List<OneMeasureModel>();
            CCPointCrossModels = new List<CCPointCrossModel>();
            FindRegionCircleModels = new List<FindRegionCircleModel>();
            MeasureRectModels = new List<MeasureRectModel>();
            //
            #endregion

            #region 数据操作
            //
            DataSaveModels = new List<DataSaveModel>();
            DataOperateModels = new List<DataOperateModel>();
            ExpressModels = new List<ExpressModel>();
            HADevEngineModels = new List<HADevEngineModel>();
            TimeComputeModels = new List<TimeComputeModel>();
            ScriptModels = new List<ScriptModel>();
            SelectProductModels = new List<SelectProductModel>();
            SharpScriptModels = new List<SharpScriptModel>();
            DisplayDataModels = new List<DisplayDataModel>();
            SystemTimeModels = new List<SystemTimeModel>();
            //
            #endregion

            #region 检测3D
            //
            FlatnessModels = new List<FlatnessModel>();
            PointToAreaModels = new List<PointToAreaModel>();
            GenFittingImageModels = new List<GenFittingImageModel>();
            ProcessImageModels = new List<ProcessImageModel>();
            GetSubImageModels = new List<GetSubImageModel>();
            CheckGlueDefectModels = new List<CheckGlueDefectModel>();
            CheckGlueFourthModels = new List<CheckGlueFourthModel>();
            CheckKEdgeContourModels = new List<CheckKEdgeContourModel>();
            CheckGlueModels = new List<CheckGlueModel>();
            //
            #endregion

            #region 标定工具
            //
            MeasureCaliModels = new List<MeasureCaliModel>();
            DistrotionCaliModels = new List<DistrotionCaliModel>();
            NineCaliModels = new List<NineCaliModel>();
            CoordMappingModels = new List<CoordMappingModel>();
            AffineTransModels = new List<AffineTransModel>();
            MatTransModels = new List<MatTransModel>();
            RegionMapModels = new List<RegionMapModel>();
            RotateCenterModels = new List<RotateCenterModel>();
            //
            #endregion

            #region 区域处理
            //
            RegionShapeModels = new List<RegionShapeModel>();
            RegionFilterModels = new List<RegionFilterModel>();
            RegionToContourModels = new List<RegionToContourModel>();
            RegionFillModels = new List<RegionFillModel>();
            RegionComputeModels = new List<RegionComputeModel>();
            OpenCloseModels = new List<OpenCloseModel>();
            DErosionModels = new List<DErosionModel>();
            AreaCenterModels = new List<AreaCenterModel>();
            FeatureDetectModels = new List<FeatureDetectModel>();
            SortRegionModels = new List<SortRegionModel>();
            PointCreateModels = new List<PointCreateModel>();
            PointGenCircleModels = new List<PointGenCircleModel>();
            PartitionRegionModels = new List<PartitionRegionModel>();
            GrayTophatModels = new List<GrayTophatModel>();
            //
            #endregion

            #region 轮廓处理
            //
            EdgeExtractModels = new List<EdgeExtractModel>();
            ContourFilterModels = new List<ContourFilterModel>();
            ContourIntersectModels = new List<ContourIntersectModel>();
            GenCrossTenModels = new List<GenCrossTenModel>();
            ContourToRegionModels = new List<ContourToRegionModel>();
            SortPointModels = new List<SortPointModel>();
            //
            #endregion

            #region 通讯工具
            //
            PLCStartModels = new List<PLCStartModel>();
            PLCWriteModels = new List<PLCWriteModel>();
            PLCReadModels = new List<PLCReadModel>();
            PLCLotsWriteModels = new List<PLCLotsWriteModel>();
            RecieveFileModels = new List<RecieveFileModel>();
            SendFileModels = new List<SendFileModel>();
            FtpUDLoadModels = new List<FtpUDLoadModel>();
            ReadFileModels = new List<ReadFileModel>();
            WriteFileModels = new List<WriteFileModel>();
            LightControlModels = new List<LightControlModel>();
            RunBatchModels = new List<RunBatchModel>();
            ScrewInitModels = new List<ScrewInitModel>();
            ScrewReadModels = new List<ScrewReadModel>();
            UploadSFCModels = new List<UploadSFCModel>();
            SAPControlModels = new List<SAPControlModel>();
            ZebraPrintModels = new List<ZebraPrintModel>();
            //
            #endregion

            #region 立讯算法
            //
            LuxGetLineModels = new List<LuxGetLineModel>();
            LuxAreaToBaseModels = new List<LuxAreaToBaseModel>();
            LuxShowResultModels = new List<LuxShowResultModel>();
            //
            #endregion

            #region 运动控制
            IOControlModels = new List<IOControlModel>();
            IOCheckModels = new List<IOCheckModel>();
            AxisHomeModels = new List<AxisHomeModel>();
            CoordMoveModels = new List<CoordMoveModel>();
            MoveOffSetModels = new List<MoveOffSetModel>();
            SetSpeedModels = new List<SetSpeedModel>();
            StopMoveModels = new List<StopMoveModel>();
            WaitDoneModels = new List<WaitDoneModel>();
            ModifyPosModels = new List<ModifyPosModel>();
            GetPosModels = new List<GetPosModel>();
            #endregion
            
        }
    }

}
