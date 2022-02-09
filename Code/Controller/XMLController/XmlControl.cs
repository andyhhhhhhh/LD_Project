using GlobalCore;
using SequenceTestModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XMLController
{
    public class XmlControl
    {
        /// <summary>
        /// 保存到xml文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="sourceObj"></param>
        /// <param name="type"></param>
        public static void SaveToXml(string filePath, object sourceObj, Type type)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(filePath) && sourceObj != null)
                {
                    type = type != null ? type : sourceObj.GetType();

                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(type);
                        XmlSerializerNamespaces nameSpace = new XmlSerializerNamespaces();

                        nameSpace.Add("", ""); //not ot output the default namespace
                        xmlSerializer.Serialize(writer, sourceObj, nameSpace);
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        /// <summary>
        /// 加载xml文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object LoadFromXml(string filePath, Type type)
        {
            object result = null;

            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(type);
                    result = xmlSerializer.Deserialize(reader);
                    reader.Close();
                }
            }
            return result;
        }

        private static ControlCardModel _ControlCardModel;
        /// <summary>
        /// 运动控制卡属性
        /// </summary>
        public static ControlCardModel controlCardModel
        {
            get
            {
                return _ControlCardModel;
            }
            set
            {
                _ControlCardModel = value;
            }
        }

        private static SequenceModel _SequenceModel = new SequenceModel();
        /// <summary>
        /// 无用--获取XML内容赋值
        /// </summary>
        public static SequenceModel sequenceModel
        {
            get
            {
                _SequenceModel = LoadFromXml(Global.SequencePath, _SequenceModel.GetType()) as SequenceModel;
                return _SequenceModel;
            }
            set
            {
                SaveToXml(Global.SequencePath, _SequenceModel, _SequenceModel.GetType());
            }
        }

        private static SingleSequenceModel _sequenceSingle;
        /// <summary>
        /// 单个工作空间流程属性
        /// </summary>
        public static SingleSequenceModel sequenceSingle
        {
            get
            {
                return _sequenceSingle;
            }
            set
            {
                _sequenceSingle = value;
            }
        }

        private static SequenceModel _SequenceModelNew;
        /// <summary>
        /// 总的工作空间属性
        /// </summary>
        public static SequenceModel sequenceModelNew
        {
            get
            {
                return _SequenceModelNew;
            }
            set
            {
                _SequenceModelNew = value;
            }
        }

        public static List<ProductInfoModel> listProductModel
        {
            get
            {
                return _SequenceModelNew.ProductInfoModels;
            }
        }

        public static List<ProductSelModel> listProductSelModel
        {
            get
            {
                return _SequenceModelNew.ProductSelModels;
            }
        }

        /// <summary>
        /// 把returnValue设置为空 预防有不能保存的类型
        /// </summary>
        public static void SetObject()
        {
            if (sequenceModelNew != null && sequenceModelNew.ChildSequenceModels != null)
            {
                foreach (var item in sequenceModelNew.ChildSequenceModels)
                {
                    if (item != null && item.SingleSequenceModels != null)
                    {
                        foreach (var item2 in item.SingleSequenceModels)
                        {
                            foreach (var item3 in item2.CheckFeatureModels)
                            {
                                item3.Result = "";
                                item3.ReturnValue = "";
                                item3.ExecuteTime = 0;
                            } 
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 根据Id获取单个流程中所有工具内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public static List<CheckFeatureModel> GetSequenceList(int id)
        //{
        //    string strName = "流程" + id.ToString();
        //    var singleModel = sequenceModelNew.SingleSequenceModels.FirstOrDefault(x => x.Name == strName);
        //    //return sequenceModelNew.SingleSequenceModels[id-1].CheckFeatureModels.OrderBy(x=>x.Id).ToList();
        //    return singleModel.CheckFeatureModels.OrderBy(x => x.Id).ToList();
        //}

        /// <summary>
        /// 根据名称类型设置对应的工具名称
        /// </summary>
        /// <param name="sequence">流程</param>
        /// <param name="strType">工具类型名称</param>
        /// <returns></returns>
        public static string SetName(SingleSequenceModel sequence, string strType)
        {
            int index = -1;
            int ivalue = 1;
            string strName = string.Format("{0}-{1} ", sequence.BaseSeqName, strType);

            List<CheckFeatureModel> listModel = new List<CheckFeatureModel>();
            ChildSequenceModel childModel = XmlControl.sequenceModelNew.ChildSequenceModels.FirstOrDefault(x => x.Name == sequence.BaseSeqName);
            if(childModel == null)
            {
                return "";
            }
            foreach (var item in childModel.SingleSequenceModels)
            {
                listModel.AddRange(item.CheckFeatureModels);
            }

            index = listModel.FindIndex(x => x.Name == strName + ivalue.ToString());
            while (index != -1)
            {
                ivalue++;
                index = listModel.FindIndex(x => x.Name == strName + ivalue.ToString());
            }

            strName += ivalue.ToString();

            return strName;
        }

        public static string SetSeqName(ChildSequenceModel sequence, out int id)
        {
            int index = -1;
            int ivalue = 1;
            string strName = "分流程";
            index = sequence.SingleSequenceModels.FindIndex(x => x.Name == strName + ivalue.ToString());
            while (index != -1)
            {
                ivalue++;
                index = sequence.SingleSequenceModels.FindIndex(x => x.Name == strName + ivalue.ToString());
            }

            strName += ivalue.ToString();
            id = ivalue;

            return strName;
        }

        /// <summary>
        /// 获取ItemResult中的所有属性
        /// </summary>
        /// <param name="checkFeatureModel">具体工具单元</param>
        /// <returns></returns>
        public static PropertyInfo[] GetPropertyInfo(CheckFeatureModel checkFeatureModel)
        {
            PropertyInfo[] listpro = null;

            switch (checkFeatureModel.featureType)
            {
                case FeatureType.镭射采集:
                    SnapImageModel snapModel = new SnapImageModel();
                    listpro = GetPropertyInfoArray(snapModel.itemResult);
                    break;

                case FeatureType.找圆:
                    FindCircleModel findCircleModel = new FindCircleModel();
                    listpro = GetPropertyInfoArray(findCircleModel.itemResult);
                    break;

                case FeatureType.找线:
                    FindLineModel findLineModel = new FindLineModel();
                    listpro = GetPropertyInfoArray(findLineModel.itemResult);
                    break;

                case FeatureType.基准面:
                    BaseLevelModel baseLevelModel = new BaseLevelModel();
                    listpro = GetPropertyInfoArray(baseLevelModel.itemResult);
                    break;

                case FeatureType.检测点:
                    CheckAreaModel checkAreaModel = new CheckAreaModel();
                    listpro = GetPropertyInfoArray(checkAreaModel.itemResult);
                    break;

                case FeatureType.圆心距:
                    CircleDDModel circleDDModel = new CircleDDModel();
                    listpro = GetPropertyInfoArray(circleDDModel.itemResult);
                    break;

                case FeatureType.两线距离:
                    LineDDModel lineDDModel = new LineDDModel();
                    listpro = GetPropertyInfoArray(lineDDModel.itemResult);
                    break;

                case FeatureType.检测高度:
                    PointToAreaModel pointToAreaModel = new PointToAreaModel();
                    listpro = GetPropertyInfoArray(pointToAreaModel.itemResult);
                    break;

                case FeatureType.特征匹配:
                    FixedItemModel fixedItemModel = new FixedItemModel();
                    listpro = GetPropertyInfoArray(fixedItemModel.itemResult);
                    break;

                case FeatureType.平面度:
                    FlatnessModel flatnessModel = new FlatnessModel();
                    listpro = GetPropertyInfoArray(flatnessModel.itemResult);
                    break;

                case FeatureType.图片处理:
                    ProcessImageModel processImageModel = new ProcessImageModel();
                    listpro = GetPropertyInfoArray(processImageModel.itemResult);
                    break;

                case FeatureType.基准图像:
                    GenFittingImageModel fittModel = new GenFittingImageModel();
                    listpro = GetPropertyInfoArray(fittModel.itemResult);
                    break;

                case FeatureType.胶路缺陷:
                    CheckGlueDefectModel checkGlueModel = new CheckGlueDefectModel();
                    listpro = GetPropertyInfoArray(checkGlueModel.itemResult);
                    break;

                case FeatureType.流程触发:
                    ProcessTrigModel processTrigModel = new ProcessTrigModel();
                    listpro = GetPropertyInfoArray(processTrigModel.itemResult);
                    break;

                case FeatureType.图像保存:
                    ImageSaveModel imageSaveModel = new ImageSaveModel();
                    listpro = GetPropertyInfoArray(imageSaveModel.itemResult);
                    break;

                case FeatureType.发送消息:
                    SendMessageModel sendMessageModel = new SendMessageModel();
                    listpro = GetPropertyInfoArray(sendMessageModel.itemResult);
                    break;

                case FeatureType.数据运算:
                    DataOperateModel dataOperateModel = new DataOperateModel();
                    listpro = GetPropertyInfoArray(dataOperateModel.itemResult);
                    break;

                case FeatureType.数据保存:
                    DataSaveModel dataSaveModel = new DataSaveModel();
                    listpro = GetPropertyInfoArray(dataSaveModel.itemResult);
                    break;

                case FeatureType.延时等待:
                    DelaySetModel delaySetModel = new DelaySetModel();
                    listpro = GetPropertyInfoArray(delaySetModel.itemResult);
                    break;

                case FeatureType.循环开始:
                    LoopStartModel loopStartModel = new LoopStartModel();
                    listpro = GetPropertyInfoArray(loopStartModel.itemResult);
                    break;

                case FeatureType.循环结束:
                    LoopEndModel loopEndModel = new LoopEndModel();
                    listpro = GetPropertyInfoArray(loopEndModel.itemResult);
                    break;

                case FeatureType.相机采集:
                    Camera2DSnapModel camera2DSnapModel = new Camera2DSnapModel();
                    listpro = GetPropertyInfoArray(camera2DSnapModel.itemResult);
                    break;

                case FeatureType.测量标定:
                    MeasureCaliModel measureCaliModel = new MeasureCaliModel();
                    listpro = GetPropertyInfoArray(measureCaliModel.itemResult);
                    break;

                case FeatureType.N点标定:
                    NineCaliModel nineCaliModel = new NineCaliModel();
                    listpro = GetPropertyInfoArray(nineCaliModel.itemResult);
                    break;

                case FeatureType.坐标映射:
                    CoordMappingModel coordMappingModel = new CoordMappingModel();
                    listpro = GetPropertyInfoArray(coordMappingModel.itemResult);
                    break;

                case FeatureType.仿射变换:
                    AffineTransModel AffineTransModel = new AffineTransModel();
                    listpro = GetPropertyInfoArray(AffineTransModel.itemResult);
                    break;

                case FeatureType.预处理图:
                    PretreatImageModel pretreatImageModel = new PretreatImageModel();
                    listpro = GetPropertyInfoArray(pretreatImageModel.itemResult);
                    break;

                case FeatureType.算表达式:
                    ExpressModel ExpressModel = new ExpressModel();
                    listpro = GetPropertyInfoArray(ExpressModel.itemResult);
                    break;

                case FeatureType.检测亮度:
                    CheckLightModel CheckLightModel = new CheckLightModel();
                    listpro = GetPropertyInfoArray(CheckLightModel.itemResult);
                    break;

                case FeatureType.畸变标定:
                    DistrotionCaliModel DistrotionCaliModel = new DistrotionCaliModel();
                    listpro = GetPropertyInfoArray(DistrotionCaliModel.itemResult);
                    break;

                case FeatureType.创建ROI:
                    CreateRegionModel CreateRegionModel = new CreateRegionModel();
                    listpro = GetPropertyInfoArray(CreateRegionModel.itemResult);
                    break;

                case FeatureType.统计像素:
                    CountPixModel CountPixModel = new CountPixModel();
                    listpro = GetPropertyInfoArray(CountPixModel.itemResult);
                    break;

                case FeatureType.算法引擎:
                    HADevEngineModel HADevEngineModel = new HADevEngineModel();
                    listpro = GetPropertyInfoArray(HADevEngineModel.itemResult);
                    break;

                case FeatureType.局部变量:
                    LocalVariableModel LocalVariableModel = new LocalVariableModel();
                    listpro = GetPropertyInfoArray(LocalVariableModel.itemResult);
                    break;

                case FeatureType.数组定义:
                    LocalArrayModel LocalArrayModel = new LocalArrayModel();
                    listpro = GetPropertyInfoArray(LocalArrayModel.itemResult);
                    break;

                case FeatureType.显示图像:
                    DisplayImgModel DisplayImgModel = new DisplayImgModel();
                    listpro = GetPropertyInfoArray(DisplayImgModel.itemResult);
                    break;

                //case FeatureType.轮廓匹配:
                //    MatchingModel MatchingModel = new MatchingModel();
                //    listpro = GetPropertyInfoArray(MatchingModel.itemResult);
                //    break;

                //case FeatureType.时间统计:
                //    TimeComputeModel TimeComputeModel = new TimeComputeModel();
                //    listpro = GetPropertyInfoArray(TimeComputeModel.itemResult);
                //    break;

                default:

                    BaseSeqModel baseSeqModel = null;
                    var aType = typeof(BaseSeqModel);
                    var types = typeof(BaseSeqModel).Assembly.GetTypes().ToList().FindAll(x => x.BaseType != null && x.BaseType.FullName == aType.FullName);
                    //var types = Assembly.GetCallingAssembly().GetTypes();  //获取所有类型 
                    foreach (var t in types)
                    {
                        baseSeqModel = Activator.CreateInstance(t) as BaseSeqModel;
                        if (baseSeqModel.BaseType == null)
                        {
                            continue;
                        }
                        if (baseSeqModel.BaseType == checkFeatureModel.featureType.ToString())
                        {
                            break;
                        }
                    }

                    listpro = FromatDitits(baseSeqModel) as PropertyInfo[];
                    break;
            }

            return listpro;
        }

        /// <summary>
        /// 获取ItemResult中的所有属性 -- 用泛型实现
        /// </summary>
        /// <typeparam name="T">泛型类</typeparam>
        /// <param name="model">工具单元</param>
        /// <returns></returns>
        public static object FromatDitits<T>(T model)
        {
            var newType = model.GetType();
            foreach (var item in newType.GetRuntimeProperties())
            {
                var type = item.PropertyType.Name;
                var IsGenericType = item.PropertyType.IsClass;
                if (IsGenericType && type == "ItemResult")
                {
                    object obj = Activator.CreateInstance(item.PropertyType);
                    var pro = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    return pro;
                }
            }
            return null;
        }
        public static PropertyInfo[] GetPropertyInfoArray(BaseSeqResultModel itemResult)
        {
            PropertyInfo[] props = null;
            try
            {
                Type type = itemResult.GetType();
                object obj = Activator.CreateInstance(type);
                props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            }
            catch (Exception ex)
            {
            }
            return props;
        }

        /// <summary>
        /// 获取链接的变量值
        /// </summary>
        /// <param name="strvalue"></param>
        /// <returns></returns>
        public static object GetLinkValue(string strvalue)
        {
            try
            {
                //SingleSequenceModel sequence = XmlControl.sequenceSingle;
                double dvalue;
                if (!strvalue.Contains(".") || Double.TryParse(strvalue, out dvalue))
                {
                    return strvalue;
                }
                object ovalue = null;

                string[] valueArr = strvalue.Split('.');
                string name = valueArr[0];
                //获取全局变量
                if (name == "Global")
                {
                    var varModel = sequenceModelNew.VariableSetModels.FirstOrDefault(x => x.Name == valueArr[1]);
                    if (varModel != null)
                    {
                        if (varModel.Expression == "")
                        {
                            if (varModel.variableType == VariableType.Object)
                            {
                                ovalue = varModel.ObjectValue;
                            }
                            else
                            {
                                ovalue = varModel.TestResult;
                            }
                        }
                        else
                        {
                            ovalue = CalcFunc.GetResult(CalcFunc.GetValue(varModel.Expression));
                        }
                    }
                }
                else if (name == "Local")//获取局部变量
                {
                    string strName = valueArr[1];
                    string localName = valueArr[2];

                    int index = strName.IndexOf('-');
                    string strChild = strName.Substring(0, index);
                    ChildSequenceModel childModel = sequenceModelNew.ChildSequenceModels.FirstOrDefault(x => x.Name == strChild);
                     
                    foreach (var sequence in childModel.SingleSequenceModels)
                    {
                        var checkModel = sequence.CheckFeatureModels.FirstOrDefault(x => x.Name == strName);
                        if (checkModel != null)
                        {
                            var LocalVarModel = sequence.LocalVariableModels.FirstOrDefault(x => x.Name == strName);
                            LocalVariableModel.Variable var = LocalVarModel.ListVariable.FirstOrDefault(x => x.Name == localName);

                            if (var.variableType == VariableType.Object)
                            {
                                ovalue = var.ObjectValue;
                            }
                            else
                            {
                                ovalue = var.TestResult;
                            }
                            break;
                        }
                    }
                }
                else if (name == "Array")//获取数组
                {
                    string strName = valueArr[1];
                    string ArrayName = valueArr[2];

                    int index = strName.IndexOf('-');
                    string strChild = strName.Substring(0, index);
                    ChildSequenceModel childModel = sequenceModelNew.ChildSequenceModels.FirstOrDefault(x => x.Name == strChild);
                    foreach (var sequence in childModel.SingleSequenceModels)
                    {
                        var checkModel = sequence.CheckFeatureModels.FirstOrDefault(x => x.Name == strName);
                        if (checkModel != null)
                        {
                            var LocalArrModel = sequence.LocalArrayModels.FirstOrDefault(x => x.Name == strName);

                            //如果只是取数组中的某一个数值
                            if (ArrayName.Contains("[") && ArrayName.Contains("]"))
                            {
                                int startindex = ArrayName.IndexOf("[");
                                int endindex = ArrayName.IndexOf("]");
                                string aName = ArrayName.Substring(0, startindex);
                                int index2 = Int32.Parse(ArrayName.Substring(startindex + 1, endindex - startindex - 1));
                                var var = LocalArrModel.ListArray.FirstOrDefault(x => x.Name == aName);
                                if (var.ObjectValue is Array)
                                {
                                    Array arrValue = (Array)var.ObjectValue;
                                    int start = 0;
                                    foreach (var item in arrValue)
                                    {
                                        if (index2 == start)
                                        {
                                            ovalue = item;
                                            break;
                                        }
                                        start++;
                                    }
                                }
                            }
                            else
                            {
                                var var = LocalArrModel.ListArray.FirstOrDefault(x => x.Name == ArrayName);
                                ovalue = var.ObjectValue;
                            }

                            break;
                        }
                    }
                }
                else
                {
                    //要具体确定是哪个Single里面的 20191104 chb
                    int index = name.IndexOf('-');
                    string strChild = name.Substring(0, index);
                    ChildSequenceModel childModel = sequenceModelNew.ChildSequenceModels.FirstOrDefault(x => x.Name == strChild);

                    foreach (var sequence in childModel.SingleSequenceModels)
                    {
                        var checkModel = sequence.CheckFeatureModels.FirstOrDefault(x => x.Name == name);

                        if (checkModel != null)
                        {
                            ovalue = XmlControl.Get_LinkObj(checkModel, valueArr[1], sequence);
                            //是否为数组类型的数据
                            if(valueArr.Length > 2)
                            {
                                if (ovalue is Array && valueArr[2].Contains("[") && valueArr[2].Contains("]"))
                                {
                                    int startindex = valueArr[2].IndexOf("[");
                                    int endindex = valueArr[2].IndexOf("]"); 
                                    int index2 = Int32.Parse(valueArr[2].Substring(startindex + 1, endindex - startindex - 1));
                                    Array arrValue = (Array)ovalue;
                                    int start = 0;
                                    foreach (var item in arrValue)
                                    {
                                        if (index2 == start)
                                        {
                                            ovalue = item;
                                            break;
                                        }
                                        start++;
                                    }
                                }
                            }
                            break;
                        }
                    }
                }

                return ovalue;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static object GetLinkValue(SingleSequenceModel sequence, string strvalue)
        {
            try
            {
                double dvalue;
                if (!strvalue.Contains(".") || Double.TryParse(strvalue, out dvalue))
                {
                    return strvalue;
                }
                object ovalue = null;

                string[] valueArr = strvalue.Split('.');
                string name = valueArr[0];
                //获取全局变量
                if (name == "Global")
                {
                    var varModel = sequenceModelNew.VariableSetModels.FirstOrDefault(x => x.Name == valueArr[1]);
                    if (varModel != null)
                    {
                        if (varModel.Expression == "")
                        {
                            if (varModel.variableType == VariableType.Object)
                            {
                                ovalue = varModel.ObjectValue;
                            }
                            else
                            {
                                ovalue = varModel.TestResult;
                            }
                        }
                        else
                        {
                            ovalue = CalcFunc.GetResult(CalcFunc.GetValue(varModel.Expression));
                        }
                    }
                }
                else if (name == "Local")//获取局部变量
                {
                    string strName = valueArr[1];
                    string localName = valueArr[2];

                    var checkModel = sequence.CheckFeatureModels.FirstOrDefault(x => x.Name == strName);
                    if (checkModel != null)
                    {
                        var LocalVarModel = sequence.LocalVariableModels.FirstOrDefault(x => x.Name == strName);
                        LocalVariableModel.Variable var = LocalVarModel.ListVariable.FirstOrDefault(x => x.Name == localName);

                        if (var.variableType == VariableType.Object)
                        {
                            ovalue = var.ObjectValue;
                        }
                        else
                        {
                            ovalue = var.TestResult;
                        }
                    }
                }
                else if (name == "Array")//获取数组
                {
                    string strName = valueArr[1];
                    string ArrayName = valueArr[2]; 

                    var checkModel = sequence.CheckFeatureModels.FirstOrDefault(x => x.Name == strName);
                    if (checkModel != null)
                    {
                        var LocalArrModel = sequence.LocalArrayModels.FirstOrDefault(x => x.Name == strName);

                        //如果只是取数组中的某一个数值
                        if (ArrayName.Contains("[") && ArrayName.Contains("]"))
                        {
                            int startindex = ArrayName.IndexOf("[");
                            int endindex = ArrayName.IndexOf("]");
                            string aName = ArrayName.Substring(0, startindex);
                            int index2 = Int32.Parse(ArrayName.Substring(startindex + 1, endindex - startindex - 1));
                            var var = LocalArrModel.ListArray.FirstOrDefault(x => x.Name == aName);
                            if (var.ObjectValue is Array)
                            {
                                Array arrValue = (Array)var.ObjectValue;
                                int start = 0;
                                foreach (var item in arrValue)
                                {
                                    if (index2 == start)
                                    {
                                        ovalue = item;
                                        break;
                                    }
                                    start++;
                                }
                            }
                        }
                        else
                        {
                            var var = LocalArrModel.ListArray.FirstOrDefault(x => x.Name == ArrayName);
                            ovalue = var.ObjectValue;
                        }
                    }
                }
                else
                {
                    //要具体确定是哪个Single里面的 20191104 chb 
                    var checkModel = sequence.CheckFeatureModels.FirstOrDefault(x => x.Name == name);

                    if (checkModel != null)
                    {
                        ovalue = XmlControl.Get_LinkObj(checkModel, valueArr[1], sequence);
                        //是否为数组类型的数据
                        if (valueArr.Length > 2)
                        {
                            if (ovalue is Array && valueArr[2].Contains("[") && valueArr[2].Contains("]"))
                            {
                                int startindex = valueArr[2].IndexOf("[");
                                int endindex = valueArr[2].IndexOf("]");
                                int index2 = Int32.Parse(valueArr[2].Substring(startindex + 1, endindex - startindex - 1));
                                Array arrValue = (Array)ovalue;
                                int start = 0;
                                foreach (var item in arrValue)
                                {
                                    if (index2 == start)
                                    {
                                        ovalue = item;
                                        break;
                                    }
                                    start++;
                                }
                            }
                        }
                    }
                }

                return ovalue;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取对应的属性值
        /// </summary>
        /// <param name="checkFeatureModel"></param>
        /// <param name="strvalue"></param>
        /// <param name="sequence"></param>
        /// <returns></returns> 
        public static object Get_LinkObj(CheckFeatureModel checkFeatureModel, string strvalue, SingleSequenceModel sequence)
        {
            try
            {
                object ovalue = null;
                BaseSeqModel baseModel = GetModel(checkFeatureModel, sequence);
                if (baseModel != null)
                {
                    PropertyInfo[] infos = baseModel.GetType().GetProperties();
                    object result = baseModel.GetType().GetProperty("itemResult").GetValue(baseModel, null);
                    ovalue = GetValue(result as BaseSeqResultModel, strvalue);
                }

                return ovalue;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public static object GetLinkObject(CheckFeatureModel checkFeatureModel, string strvalue, SingleSequenceModel sequence)
        {
            //SingleSequenceModel sequence = XmlControl.sequenceSingle;
            try
            {
                object ovalue = null;
                switch (checkFeatureModel.featureType)
                {
                    case FeatureType.镭射采集:
                        SnapImageModel snapModel = sequence.SnapImageModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(snapModel.itemResult, strvalue);
                        break;

                    case FeatureType.找圆:
                        FindCircleModel findCircleModel = sequence.FindCircleModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(findCircleModel.itemResult, strvalue);
                        break;

                    case FeatureType.找线:
                        FindLineModel findLineModel = sequence.FindLineModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(findLineModel.itemResult, strvalue);
                        break;

                    case FeatureType.基准面:
                        BaseLevelModel baseLevelModel = sequence.BaseLevelModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(baseLevelModel.itemResult, strvalue);
                        break;

                    case FeatureType.检测点:
                        CheckAreaModel CheckAreaModel = sequence.CheckAreaModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(CheckAreaModel.itemResult, strvalue);
                        break;

                    case FeatureType.圆心距:
                        CircleDDModel circleDDModel = sequence.CircleDDModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(circleDDModel.itemResult, strvalue);
                        break;

                    case FeatureType.两线距离:
                        LineDDModel lineDDModel = sequence.LineDDModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(lineDDModel.itemResult, strvalue);
                        break;

                    case FeatureType.检测高度:
                        PointToAreaModel pointToAreaModel = sequence.PointToAreaModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(pointToAreaModel.itemResult, strvalue);
                        break;

                    case FeatureType.特征匹配:
                        FixedItemModel fixedItemModel = sequence.FixedItemModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(fixedItemModel.itemResult, strvalue);
                        break;

                    case FeatureType.轮廓匹配:
                        MatchingModel MatchingModel = sequence.MatchingModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(MatchingModel.itemResult, strvalue);
                        break;

                    case FeatureType.灰度匹配:
                        NccMatchingModel NccMatchingModel = sequence.NccMatchingModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(NccMatchingModel.itemResult, strvalue);
                        break;

                    case FeatureType.平面度:
                        FlatnessModel flatnessModel = sequence.FlatnessModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(flatnessModel.itemResult, strvalue);
                        break;

                    case FeatureType.图片处理:
                        ProcessImageModel processImageModel = sequence.ProcessImageModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(processImageModel.itemResult, strvalue);
                        break;

                    case FeatureType.基准图像:
                        GenFittingImageModel fittModel = sequence.GenFittingImageModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(fittModel.itemResult, strvalue);
                        break;

                    case FeatureType.胶路缺陷:
                        CheckGlueDefectModel checkGlueModel = sequence.CheckGlueDefectModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(checkGlueModel.itemResult, strvalue);
                        break;

                    case FeatureType.流程触发:
                        ProcessTrigModel processTrigModel = sequence.ProcessTrigModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(processTrigModel.itemResult, strvalue);
                        break;

                    case FeatureType.图像保存:
                        ImageSaveModel imageSaveModel = sequence.ImageSaveModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(imageSaveModel.itemResult, strvalue);
                        break;

                    case FeatureType.发送消息:
                        SendMessageModel sendMessageModel = sequence.SendMessageModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(sendMessageModel.itemResult, strvalue);
                        break;

                    case FeatureType.数据运算:
                        DataOperateModel dataOperateModel = sequence.DataOperateModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(dataOperateModel.itemResult, strvalue);
                        break;

                    case FeatureType.数据保存:
                        DataSaveModel dataSaveModel = sequence.DataSaveModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(dataSaveModel.itemResult, strvalue);
                        break;

                    case FeatureType.延时等待:
                        DelaySetModel delaySetModel = sequence.DelaySetModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(delaySetModel.itemResult, strvalue);
                        break;

                    case FeatureType.循环开始:
                        LoopStartModel loopStartModel = sequence.LoopStartModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(loopStartModel.itemResult, strvalue);
                        break;

                    case FeatureType.循环结束:
                        LoopEndModel loopEndModel = sequence.LoopEndModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(loopEndModel.itemResult, strvalue);
                        break;

                    case FeatureType.相机采集:
                        Camera2DSnapModel camera2DSnapModel = sequence.Camera2DSnapModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(camera2DSnapModel.itemResult, strvalue);
                        break;

                    case FeatureType.测量标定:
                        MeasureCaliModel measureCaliModel = sequence.MeasureCaliModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(measureCaliModel.itemResult, strvalue);
                        break;

                    case FeatureType.N点标定:
                        NineCaliModel nineCaliModel = sequence.NineCaliModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(nineCaliModel.itemResult, strvalue);
                        break;

                    case FeatureType.坐标映射:
                        CoordMappingModel coordMappingModel = sequence.CoordMappingModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(coordMappingModel.itemResult, strvalue);
                        break;

                    case FeatureType.仿射变换:
                        AffineTransModel AffineTransModel = sequence.AffineTransModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(AffineTransModel.itemResult, strvalue);
                        break;

                    case FeatureType.预处理图:
                        PretreatImageModel pretreatImageModel = sequence.PretreatImageModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(pretreatImageModel.itemResult, strvalue);
                        break;

                    case FeatureType.算表达式:
                        ExpressModel ExpressModel = sequence.ExpressModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(ExpressModel.itemResult, strvalue);
                        break;

                    case FeatureType.检测亮度:
                        CheckLightModel CheckLightModel = sequence.CheckLightModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(CheckLightModel.itemResult, strvalue);
                        break;

                    case FeatureType.畸变标定:
                        DistrotionCaliModel DistrotionCaliModel = sequence.DistrotionCaliModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(DistrotionCaliModel.itemResult, strvalue);
                        break;

                    case FeatureType.创建ROI:
                        CreateRegionModel CreateRegionModel = sequence.CreateRegionModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(CreateRegionModel.itemResult, strvalue);
                        break;

                    case FeatureType.统计像素:
                        CountPixModel CountPixModel = sequence.CountPixModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(CountPixModel.itemResult, strvalue);
                        break;

                    case FeatureType.算法引擎:
                        HADevEngineModel HADevEngineModel = sequence.HADevEngineModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(HADevEngineModel.itemResult, strvalue);
                        break;

                    case FeatureType.局部变量:
                        LocalVariableModel LocalVariableModel = sequence.LocalVariableModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(LocalVariableModel.itemResult, strvalue);
                        break;

                    case FeatureType.数组定义:
                        LocalArrayModel LocalArrayModel = sequence.LocalArrayModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(LocalArrayModel.itemResult, strvalue);
                        break;

                    case FeatureType.显示图像:
                        DisplayImgModel DisplayImgModel = sequence.DisplayImgModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(DisplayImgModel.itemResult, strvalue);
                        break;

                    case FeatureType.时间统计:
                        TimeComputeModel TimeComputeModel = sequence.TimeComputeModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(TimeComputeModel.itemResult, strvalue);
                        break;

                    case FeatureType.执行片段:
                        StartPartModel StartPartModel = sequence.StartPartModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(StartPartModel.itemResult, strvalue);
                        break;

                    case FeatureType.停止片段:
                        StopPartModel StopPartModel = sequence.StopPartModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(StopPartModel.itemResult, strvalue);
                        break;

                    case FeatureType.条件分支:
                        IfElseModel IfElseModel = sequence.IfElseModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(IfElseModel.itemResult, strvalue);
                        break;

                    case FeatureType.线圆交点:
                        LCPointCrossModel LCPointCrossModel = sequence.LCPointCrossModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(LCPointCrossModel.itemResult, strvalue);
                        break;

                    case FeatureType.两线交点:
                        LLPointCrossModel LLPointCrossModel = sequence.LLPointCrossModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(LLPointCrossModel.itemResult, strvalue);
                        break;

                    case FeatureType.两线角度:
                        LLAngleModel LLAngleModel = sequence.LLAngleModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(LLAngleModel.itemResult, strvalue);
                        break;

                    case FeatureType.两圆距离:
                        CCDistanceModel CCDistanceModel = sequence.CCDistanceModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(CCDistanceModel.itemResult, strvalue);
                        break;

                    case FeatureType.点线距离:
                        PLDistanceModel PLDistanceModel = sequence.PLDistanceModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(PLDistanceModel.itemResult, strvalue);
                        break;

                    case FeatureType.点圆距离:
                        PCDistanceModel PCDistanceModel = sequence.PCDistanceModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(PCDistanceModel.itemResult, strvalue);
                        break;

                    case FeatureType.PLC通讯:
                        PLCStartModel PLCStartModel = sequence.PLCStartModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(PLCStartModel.itemResult, strvalue);
                        break;

                    case FeatureType.PLC写入:
                        PLCWriteModel PLCWriteModel = sequence.PLCWriteModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(PLCWriteModel.itemResult, strvalue);
                        break;

                    case FeatureType.PLC读取:
                        PLCReadModel PLCReadModel = sequence.PLCReadModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(PLCReadModel.itemResult, strvalue);
                        break;

                    case FeatureType.接收文本:
                        RecieveFileModel RecieveFileModel = sequence.RecieveFileModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(RecieveFileModel.itemResult, strvalue);
                        break;

                    case FeatureType.发送文本:
                        SendFileModel SendFileModel = sequence.SendFileModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(SendFileModel.itemResult, strvalue);
                        break;

                    case FeatureType.Ftp操作:
                        FtpUDLoadModel FtpUDLoadModel = sequence.FtpUDLoadModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(FtpUDLoadModel.itemResult, strvalue);
                        break;

                    case FeatureType.生成直线:
                        TwoPLineModel TwoPLineModel = sequence.TwoPLineModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(TwoPLineModel.itemResult, strvalue);
                        break;

                    //立讯新加的
                    case FeatureType.取线轮廓:
                        LuxGetLineModel LuxGetLineModel = sequence.LuxGetLineModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(LuxGetLineModel.itemResult, strvalue);
                        break;
                    case FeatureType.区域高度:
                        LuxAreaToBaseModel LuxAreaToBaseModel = sequence.LuxAreaToBaseModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(LuxAreaToBaseModel.itemResult, strvalue);
                        break;
                    case FeatureType.结果显示:
                        LuxShowResultModel LuxShowResultModel = sequence.LuxShowResultModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(LuxShowResultModel.itemResult, strvalue);
                        break;

                    case FeatureType.二值图像:
                        ThresholdImgModel ThresholdImgModel = sequence.ThresholdImgModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(ThresholdImgModel.itemResult, strvalue);
                        break;
                    case FeatureType.阈值分割:
                        ThreshodSegModel ThreshodSegModel = sequence.ThreshodSegModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(ThreshodSegModel.itemResult, strvalue);
                        break;
                    case FeatureType.面积中心:
                        AreaCenterModel AreaCenterModel = sequence.AreaCenterModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(AreaCenterModel.itemResult, strvalue);
                        break;
                    case FeatureType.腐蚀膨胀:
                        DErosionModel DErosionModel = sequence.DErosionModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(DErosionModel.itemResult, strvalue);
                        break;
                    case FeatureType.形状筛选:
                        RegionFilterModel RegionFilterModel = sequence.RegionFilterModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(RegionFilterModel.itemResult, strvalue);
                        break;
                    case FeatureType.区域填充:
                        RegionFillModel RegionFillModel = sequence.RegionFillModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(RegionFillModel.itemResult, strvalue);
                        break;
                    case FeatureType.开闭运算:
                        OpenCloseModel OpenCloseModel = sequence.OpenCloseModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(OpenCloseModel.itemResult, strvalue);
                        break;
                    case FeatureType.区域转边:
                        RegionToContourModel RegionToContourModel = sequence.RegionToContourModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(RegionToContourModel.itemResult, strvalue);
                        break;
                    case FeatureType.生成十字:
                        GenCrossTenModel GenCrossTenModel = sequence.GenCrossTenModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(GenCrossTenModel.itemResult, strvalue);
                        break;
                    case FeatureType.区域形状:
                        RegionShapeModel RegionShapeModel = sequence.RegionShapeModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(RegionShapeModel.itemResult, strvalue);
                        break;
                    case FeatureType.轮廓筛选:
                        ContourFilterModel ContourFilterModel = sequence.ContourFilterModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(ContourFilterModel.itemResult, strvalue);
                        break;
                    case FeatureType.轮廓交点:
                        ContourIntersectModel ContourIntersectModel = sequence.ContourIntersectModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(ContourIntersectModel.itemResult, strvalue);
                        break;

                    case FeatureType.读二维码:
                        ReadCode2DModel ReadCode2DModel = sequence.ReadCode2DModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(ReadCode2DModel.itemResult, strvalue);
                        break;
                    case FeatureType.读一维码:
                        ReadBarCodeModel ReadBarCodeModel = sequence.ReadBarCodeModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(ReadBarCodeModel.itemResult, strvalue);
                        break;
                    case FeatureType.字符识别:
                        OcrModel OcrModel = sequence.OcrModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(OcrModel.itemResult, strvalue);
                        break;
                    case FeatureType.图像剪切:
                        ImageCutModel ImageCutModel = sequence.ImageCutModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(ImageCutModel.itemResult, strvalue);
                        break;
                    case FeatureType.测清晰度:
                        DefinitionModel DefinitionModel = sequence.DefinitionModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(DefinitionModel.itemResult, strvalue);
                        break;
                    case FeatureType.文本分割:
                        SpiltTextModel SpiltTextModel = sequence.SpiltTextModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(SpiltTextModel.itemResult, strvalue);
                        break;
                    case FeatureType.创建文本:
                        CreateTextModel CreateTextModel = sequence.CreateTextModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(CreateTextModel.itemResult, strvalue);
                        break;
                    case FeatureType.数据入队:
                        EnQueueModel EnQueueModel = sequence.EnQueueModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(EnQueueModel.itemResult, strvalue);
                        break;
                    case FeatureType.数据出队:
                        DeQueueModel DeQueueModel = sequence.DeQueueModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(DeQueueModel.itemResult, strvalue);
                        break;
                    case FeatureType.清空队列:
                        ClearQueueModel ClearQueueModel = sequence.ClearQueueModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(ClearQueueModel.itemResult, strvalue);
                        break;
                    case FeatureType.变量存储:
                        SaveVarModel SaveVarModel = sequence.SaveVarModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(SaveVarModel.itemResult, strvalue);
                        break;
                    case FeatureType.变量读取:
                        ReadVarModel ReadVarModel = sequence.ReadVarModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(ReadVarModel.itemResult, strvalue);
                        break;
                    case FeatureType.圆弧测量:
                        FindCircleArcModel FindCircleArcModel = sequence.FindCircleArcModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(FindCircleArcModel.itemResult, strvalue);
                        break;
                    case FeatureType.矩形阵列:
                        RectArrayModel RectArrayModel = sequence.RectArrayModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(RectArrayModel.itemResult, strvalue);
                        break;
                    case FeatureType.一维测量:
                        OneMeasureModel OneMeasureModel = sequence.OneMeasureModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(OneMeasureModel.itemResult, strvalue);
                        break;
                    case FeatureType.数组设置:
                        ArraySetModel ArraySetModel = sequence.ArraySetModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(ArraySetModel.itemResult, strvalue);
                        break;
                    case FeatureType.交点匹配:
                        LinePMatchModel LinePMatchModel = sequence.LinePMatchModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(LinePMatchModel.itemResult, strvalue);
                        break;
                    case FeatureType.特征检测:
                        FeatureDetectModel FeatureDetectModel = sequence.FeatureDetectModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        ovalue = GetValue(FeatureDetectModel.itemResult, strvalue);
                        break;

                    default:
                        break;
                }

                return ovalue;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        /// <summary>
        /// 获取ItemResult类中名称为valueName的结果
        /// </summary>
        /// <param name="itemResult">输入的ItemResult类</param>
        /// <param name="valueName">参数名称</param>
        /// <returns></returns>
        public static object GetValue(BaseSeqResultModel itemResult, string valueName)
        {
            Type t = itemResult.GetType();//获得该类的Type

            //再用Type.GetProperties获得PropertyInfo[],然后就可以用foreach 遍历了
            foreach (PropertyInfo pi in t.GetProperties())
            {
                string name = pi.Name;//获得属性的名字,后面就可以根据名字判断来进行些自己想要的操作
                if (name != valueName)
                {
                    continue;
                }
                object value1 = pi.GetValue(itemResult, null);//用pi.GetValue获得值
                //获得属性的类型,进行判断然后进行以后的操作,例如判断获得的属性是整数
                //if (value1.GetType() == typeof(int))
                //{
                //    //进行你想要的操作
                //}


                return value1;
            }

            return null;
        }

        /// <summary>
        /// 设置变量值
        /// </summary>
        /// <param name="sequence">对应的流程</param>
        /// <param name="strvalue">变量名称</param>
        /// <param name="ovalue">需要设定的值</param>
        public static void SetLinkValue(SingleSequenceModel sequence, string strvalue, object ovalue)
        {
            try
            {
                string[] valueArr = strvalue.Split('.');
                string name = valueArr[0];
                //获取全局变量
                if (name == "Global")
                {
                    var varModel = sequenceModelNew.VariableSetModels.FirstOrDefault(x => x.Name == valueArr[1]);
                    if (varModel != null)
                    {
                        if (varModel.variableType == VariableType.Object)
                        {
                            varModel.ObjectValue = ovalue;
                        }
                        else
                        {
                            varModel.TestResult = ovalue.ToString();
                        }
                    }
                }
                else if (name == "Local")//获取局部变量
                {
                    string strName = valueArr[1];
                    string localName = valueArr[2];

                    ChildSequenceModel childModel = sequenceModelNew.ChildSequenceModels.FirstOrDefault(x => x.Name == sequence.BaseSeqName); 
                    List<LocalVariableModel> listModel = new List<LocalVariableModel>();
                    foreach (var item in childModel.SingleSequenceModels)
                    {
                        listModel.AddRange(item.LocalVariableModels);
                    }

                    var checkModel = childModel.GetCheckFeatureList().FirstOrDefault(x => x.Name == strName);
                    if (checkModel != null)
                    {
                        var LocalVarModel = listModel.FirstOrDefault(x => x.Name == strName);
                        LocalVariableModel.Variable var = LocalVarModel.ListVariable.FirstOrDefault(x => x.Name == localName);

                        if (var.variableType == VariableType.Object)
                        {
                            var.ObjectValue = ovalue;
                        }
                        else
                        {
                            var.TestResult = ovalue.ToString();
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 设置变量值
        /// </summary> 
        /// <param name="strvalue">变量名称</param>
        /// <param name="ovalue">需要设定的值</param>
        public static void SetLinkValue(string strvalue, object ovalue)
        {
            try
            {
                string[] valueArr = strvalue.Split('.');
                string name = valueArr[0];
                //获取全局变量
                if (name == "Global")
                {
                    var varModel = sequenceModelNew.VariableSetModels.FirstOrDefault(x => x.Name == valueArr[1]);
                    if (varModel != null)
                    {
                        if (varModel.variableType == VariableType.Object)
                        {
                            varModel.ObjectValue = ovalue;
                        }
                        else
                        {
                            varModel.TestResult = ovalue.ToString();
                        }
                    }
                }
                else if (name == "Local")//获取局部变量
                {
                    string strName = valueArr[1];
                    string localName = valueArr[2];

                    ChildSequenceModel childModel = GetChildModel(strName);
                    List<LocalVariableModel> listModel = new List<LocalVariableModel>();
                    foreach (var item in childModel.SingleSequenceModels)
                    {
                        listModel.AddRange(item.LocalVariableModels);
                    }

                    var checkModel = childModel.GetCheckFeatureList().FirstOrDefault(x => x.Name == strName);
                    if (checkModel != null)
                    {
                        var LocalVarModel = listModel.FirstOrDefault(x => x.Name == strName);
                        LocalVariableModel.Variable var = LocalVarModel.ListVariable.FirstOrDefault(x => x.Name == localName);

                        if (var.variableType == VariableType.Object)
                        {
                            var.ObjectValue = ovalue;
                        }
                        else
                        {
                            var.TestResult = ovalue.ToString();
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 复制Model中需要新增对应项目
        /// </summary>
        /// <param name="tModel">工具单元</param>
        /// <param name="strname">单元名称</param>
        public static void AddCopyItem(CheckFeatureModel tModel, string strname)
        {
            //利用反射实现复制项目
            SingleSequenceModel sequence = XmlControl.sequenceSingle;
            object baseModel = XmlControl.GetModel(tModel, sequence);
            object listObj = sequence.GetType().GetProperty(baseModel.GetType().Name + "s").GetValue(sequence);
            IList ilist = listObj as IList;

            //先实现Clone
            var method = baseModel.GetType().GetMethod("Clone");
            object t = method.Invoke(baseModel, new object[] { });

            //设置名称
            t.GetType().GetProperty("Name").SetValue(t, strname);
            if (!ilist.Contains(t))
            {
                ilist.Add(t);
            }

            return;

            #region 暂时无用
            switch (tModel.featureType)
            {
                case FeatureType.读二维码:
                    ReadCode2DModel model = sequence.ReadCode2DModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (model != null)
                    {
                        ReadCode2DModel newModel = model.Clone();
                        newModel.Name = strname;

                        if (sequence.ReadCode2DModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.ReadCode2DModels.Add(newModel);
                    }
                    break;
                case FeatureType.发送消息:
                    SendMessageModel model2 = sequence.SendMessageModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (model2 != null)
                    {
                        SendMessageModel newModel = model2.Clone();
                        newModel.Name = strname;

                        if (sequence.SendMessageModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.SendMessageModels.Add(newModel);
                    }
                    break;
                case FeatureType.图像保存:
                    ImageSaveModel model3 = sequence.ImageSaveModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (model3 != null)
                    {
                        ImageSaveModel newModel = model3.Clone();
                        newModel.Name = strname;

                        if (sequence.ImageSaveModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.ImageSaveModels.Add(newModel);
                    }
                    break;
                case FeatureType.图像差分:
                    break;
                case FeatureType.图片处理:
                    ProcessImageModel model4 = sequence.ProcessImageModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (model4 != null)
                    {
                        ProcessImageModel newModel = model4.Clone();
                        newModel.Name = strname;

                        if (sequence.ProcessImageModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.ProcessImageModels.Add(newModel);
                    }
                    break;
                case FeatureType.圆心距:
                    CircleDDModel model5 = sequence.CircleDDModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (model5 != null)
                    {
                        CircleDDModel newModel = model5.Clone();
                        newModel.Name = strname;

                        if (sequence.CircleDDModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.CircleDDModels.Add(newModel);
                    }
                    break;
                case FeatureType.基准图像:
                    GenFittingImageModel model6 = sequence.GenFittingImageModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (model6 != null)
                    {
                        GenFittingImageModel newModel = model6.Clone();
                        newModel.Name = strname;

                        if (sequence.GenFittingImageModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.GenFittingImageModels.Add(newModel);
                    }
                    break;
                case FeatureType.基准面:
                    BaseLevelModel modelBase = sequence.BaseLevelModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (modelBase != null)
                    {
                        BaseLevelModel newModel = modelBase.Clone();
                        newModel.Name = strname;

                        if (sequence.BaseLevelModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.BaseLevelModels.Add(newModel);
                    }
                    break;
                case FeatureType.特征匹配:
                    FixedItemModel model7 = sequence.FixedItemModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (model7 != null)
                    {
                        FixedItemModel newModel = model7.Clone();
                        newModel.Name = strname;

                        if (sequence.FixedItemModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.FixedItemModels.Add(newModel);
                    }
                    break;
                case FeatureType.平面度:
                    FlatnessModel model8 = sequence.FlatnessModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (model8 != null)
                    {
                        FlatnessModel newModel = model8.Clone();
                        newModel.Name = strname;

                        if (sequence.FlatnessModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.FlatnessModels.Add(newModel);
                    }
                    break;
                case FeatureType.找圆:
                    FindCircleModel model9 = sequence.FindCircleModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (model9 != null)
                    {
                        FindCircleModel newModel = model9.Clone();
                        newModel.Name = strname;

                        if (sequence.FindCircleModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.FindCircleModels.Add(newModel);
                    }
                    break;
                case FeatureType.外壳内边:
                    break;
                case FeatureType.找线:
                    FindLineModel model10 = sequence.FindLineModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (model10 != null)
                    {
                        FindLineModel newModel = model10.Clone();
                        newModel.Name = strname;

                        if (sequence.FindLineModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.FindLineModels.Add(newModel);
                    }
                    break;
                case FeatureType.检测点:
                    CheckAreaModel model11 = sequence.CheckAreaModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (model11 != null)
                    {
                        CheckAreaModel newModel = model11.Clone();
                        newModel.Name = strname;

                        if (sequence.CheckAreaModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.CheckAreaModels.Add(newModel);
                    }
                    break;
                case FeatureType.检测胶路:

                    break;
                case FeatureType.流程触发:
                    ProcessTrigModel model13 = sequence.ProcessTrigModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (model13 != null)
                    {
                        ProcessTrigModel newModel = model13.Clone();
                        newModel.Name = strname;

                        if (sequence.ProcessTrigModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.ProcessTrigModels.Add(newModel);
                    }
                    break;
                case FeatureType.灰度匹配:
                    break;
                case FeatureType.检测高度:
                    PointToAreaModel model14 = sequence.PointToAreaModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (model14 != null)
                    {
                        PointToAreaModel newModel = model14.Clone();
                        newModel.Name = strname;

                        if (sequence.PointToAreaModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.PointToAreaModels.Add(newModel);
                    }
                    break;
                case FeatureType.两线距离:
                    LineDDModel model15 = sequence.LineDDModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (model15 != null)
                    {
                        LineDDModel newModel = model15.Clone();
                        newModel.Name = strname;

                        if (sequence.LineDDModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.LineDDModels.Add(newModel);
                    }
                    break;
                case FeatureType.胶路缺陷:
                    CheckGlueDefectModel model16 = sequence.CheckGlueDefectModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (model16 != null)
                    {
                        CheckGlueDefectModel newModel = model16.Clone();
                        newModel.Name = strname;

                        if (sequence.CheckGlueDefectModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.CheckGlueDefectModels.Add(newModel);
                    }
                    break;
                case FeatureType.轮廓匹配:
                    break;
                case FeatureType.镭射采集:
                    SnapImageModel model17 = sequence.SnapImageModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (model17 != null)
                    {
                        SnapImageModel newModel = model17.Clone();
                        newModel.Name = strname;

                        if (sequence.SnapImageModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.SnapImageModels.Add(newModel);
                    }
                    break;

                case FeatureType.变量设置:
                    VariableValueModel model18 = sequence.VariableValueModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (model18 != null)
                    {
                        VariableValueModel newModel = model18.Clone();
                        newModel.Name = strname;

                        if (sequence.VariableValueModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.VariableValueModels.Add(newModel);
                    }
                    break;

                case FeatureType.等待变量:
                    WaitVariableModel model19 = sequence.WaitVariableModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (model19 != null)
                    {
                        WaitVariableModel newModel = model19.Clone();
                        newModel.Name = strname;

                        if (sequence.WaitVariableModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.WaitVariableModels.Add(newModel);
                    }
                    break;

                case FeatureType.数据保存:
                    DataSaveModel model20 = sequence.DataSaveModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (model20 != null)
                    {
                        DataSaveModel newModel = model20.Clone();
                        newModel.Name = strname;

                        if (sequence.DataSaveModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.DataSaveModels.Add(newModel);
                    }
                    break;

                case FeatureType.延时等待:
                    DelaySetModel model21 = sequence.DelaySetModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (model21 != null)
                    {
                        DelaySetModel newModel = model21.Clone();
                        newModel.Name = strname;

                        if (sequence.DelaySetModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.DelaySetModels.Add(newModel);
                    }
                    break;

                case FeatureType.循环开始:
                    LoopStartModel model22 = sequence.LoopStartModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (model22 != null)
                    {
                        LoopStartModel newModel = model22.Clone();
                        newModel.Name = strname;

                        if (sequence.LoopStartModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.LoopStartModels.Add(newModel);
                    }
                    break;

                case FeatureType.循环结束:
                    LoopEndModel model23 = sequence.LoopEndModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (model23 != null)
                    {
                        LoopEndModel newModel = model23.Clone();
                        newModel.Name = strname;

                        if (sequence.LoopEndModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.LoopEndModels.Add(newModel);
                    }
                    break;

                case FeatureType.胶路四边:
                    CheckGlueFourthModel model24 = sequence.CheckGlueFourthModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (model24 != null)
                    {
                        CheckGlueFourthModel newModel = model24.Clone();
                        newModel.Name = strname;

                        if (sequence.CheckGlueFourthModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.CheckGlueFourthModels.Add(newModel);
                    }
                    break;

                case FeatureType.相机采集:
                    Camera2DSnapModel model25 = sequence.Camera2DSnapModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (model25 != null)
                    {
                        Camera2DSnapModel newModel = model25.Clone();
                        newModel.Name = strname;

                        if (sequence.Camera2DSnapModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.Camera2DSnapModels.Add(newModel);
                    }
                    break;

                case FeatureType.测量标定:
                    MeasureCaliModel MeasureCaliModel = sequence.MeasureCaliModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (MeasureCaliModel != null)
                    {
                        MeasureCaliModel newModel = MeasureCaliModel.Clone();
                        newModel.Name = strname;

                        if (sequence.MeasureCaliModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.MeasureCaliModels.Add(newModel);
                    }
                    break;

                case FeatureType.N点标定:
                    NineCaliModel NineCaliModel = sequence.NineCaliModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (NineCaliModel != null)
                    {
                        NineCaliModel newModel = NineCaliModel.Clone();
                        newModel.Name = strname;

                        if (sequence.NineCaliModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.NineCaliModels.Add(newModel);
                    }
                    break;

                case FeatureType.坐标映射:
                    CoordMappingModel CoordMappingModel = sequence.CoordMappingModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (CoordMappingModel != null)
                    {
                        CoordMappingModel newModel = CoordMappingModel.Clone();
                        newModel.Name = strname;

                        if (sequence.CoordMappingModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.CoordMappingModels.Add(newModel);
                    }
                    break;


                case FeatureType.预处理图:
                    PretreatImageModel PretreatImageModel = sequence.PretreatImageModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (PretreatImageModel != null)
                    {
                        PretreatImageModel newModel = PretreatImageModel.Clone();
                        newModel.Name = strname;

                        if (sequence.PretreatImageModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.PretreatImageModels.Add(newModel);
                    }
                    break;

                case FeatureType.算表达式:
                    ExpressModel ExpressModel = sequence.ExpressModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (ExpressModel != null)
                    {
                        ExpressModel newModel = ExpressModel.Clone();
                        newModel.Name = strname;

                        if (sequence.ExpressModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.ExpressModels.Add(newModel);
                    }
                    break;

                case FeatureType.检测亮度:
                    CheckLightModel CheckLightModel = sequence.CheckLightModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (CheckLightModel != null)
                    {
                        CheckLightModel newModel = CheckLightModel.Clone();
                        newModel.Name = strname;

                        if (sequence.CheckLightModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.CheckLightModels.Add(newModel);
                    }
                    break;

                case FeatureType.畸变标定:
                    DistrotionCaliModel DistrotionCaliModel = sequence.DistrotionCaliModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (DistrotionCaliModel != null)
                    {
                        DistrotionCaliModel newModel = DistrotionCaliModel.Clone();
                        newModel.Name = strname;

                        if (sequence.DistrotionCaliModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.DistrotionCaliModels.Add(newModel);
                    }
                    break;

                case FeatureType.仿射变换:
                    AffineTransModel AffineTransModel = sequence.AffineTransModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (AffineTransModel != null)
                    {
                        AffineTransModel newModel = AffineTransModel.Clone();
                        newModel.Name = strname;

                        if (sequence.AffineTransModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.AffineTransModels.Add(newModel);
                    }
                    break;

                case FeatureType.创建ROI:
                    CreateRegionModel CreateRegionModel = sequence.CreateRegionModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (CreateRegionModel != null)
                    {
                        CreateRegionModel newModel = CreateRegionModel.Clone();
                        newModel.Name = strname;

                        if (sequence.CreateRegionModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.CreateRegionModels.Add(newModel);
                    }
                    break;

                case FeatureType.统计像素:
                    CountPixModel CountPixModel = sequence.CountPixModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (CountPixModel != null)
                    {
                        CountPixModel newModel = CountPixModel.Clone();
                        newModel.Name = strname;

                        if (sequence.CountPixModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.CountPixModels.Add(newModel);
                    }
                    break;

                case FeatureType.算法引擎:
                    HADevEngineModel HADevEngineModel = sequence.HADevEngineModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (HADevEngineModel != null)
                    {
                        HADevEngineModel newModel = HADevEngineModel.Clone();
                        newModel.Name = strname;

                        if (sequence.HADevEngineModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.HADevEngineModels.Add(newModel);
                    }
                    break;

                case FeatureType.局部变量:
                    LocalVariableModel LocalVariableModel = sequence.LocalVariableModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (LocalVariableModel != null)
                    {
                        LocalVariableModel newModel = LocalVariableModel.Clone();
                        newModel.Name = strname;

                        if (sequence.LocalVariableModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.LocalVariableModels.Add(newModel);
                    }
                    break;

                case FeatureType.数组定义:
                    LocalArrayModel LocalArrayModel = sequence.LocalArrayModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (LocalArrayModel != null)
                    {
                        LocalArrayModel newModel = LocalArrayModel.Clone();
                        newModel.Name = strname;

                        if (sequence.LocalArrayModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.LocalArrayModels.Add(newModel);
                    }
                    break;

                case FeatureType.显示图像:
                    DisplayImgModel DisplayImgModel = sequence.DisplayImgModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (DisplayImgModel != null)
                    {
                        DisplayImgModel newModel = DisplayImgModel.Clone();
                        newModel.Name = strname;

                        if (sequence.DisplayImgModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.DisplayImgModels.Add(newModel);
                    }
                    break;

                case FeatureType.时间统计:
                    TimeComputeModel TimeComputeModel = sequence.TimeComputeModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (TimeComputeModel != null)
                    {
                        TimeComputeModel newModel = TimeComputeModel.Clone();
                        newModel.Name = strname;

                        if (sequence.TimeComputeModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.TimeComputeModels.Add(newModel);
                    }
                    break;
                case FeatureType.执行片段:
                    StartPartModel StartPartModel = sequence.StartPartModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (StartPartModel != null)
                    {
                        StartPartModel newModel = StartPartModel.Clone();
                        newModel.Name = strname;

                        if (sequence.StartPartModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.StartPartModels.Add(newModel);
                    }
                    break;
                case FeatureType.停止片段:
                    StopPartModel StopPartModel = sequence.StopPartModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (StopPartModel != null)
                    {
                        StopPartModel newModel = StopPartModel.Clone();
                        newModel.Name = strname;

                        if (sequence.StopPartModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.StopPartModels.Add(newModel);
                    }
                    break;
                case FeatureType.条件分支:
                    IfElseModel IfElseModel = sequence.IfElseModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (IfElseModel != null)
                    {
                        IfElseModel newModel = IfElseModel.Clone();
                        newModel.Name = strname;

                        if (sequence.IfElseModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.IfElseModels.Add(newModel);
                    }
                    break;
                case FeatureType.线圆交点:
                    LCPointCrossModel LCPointCrossModel = sequence.LCPointCrossModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (LCPointCrossModel != null)
                    {
                        LCPointCrossModel newModel = LCPointCrossModel.Clone();
                        newModel.Name = strname;

                        if (sequence.LCPointCrossModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.LCPointCrossModels.Add(newModel);
                    }
                    break;

                case FeatureType.两线交点:
                    LLPointCrossModel LLPointCrossModel = sequence.LLPointCrossModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (LLPointCrossModel != null)
                    {
                        LLPointCrossModel newModel = LLPointCrossModel.Clone();
                        newModel.Name = strname;

                        if (sequence.LLPointCrossModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.LLPointCrossModels.Add(newModel);
                    }
                    break;

                case FeatureType.两线角度:
                    LLAngleModel LLAngleModel = sequence.LLAngleModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (LLAngleModel != null)
                    {
                        LLAngleModel newModel = LLAngleModel.Clone();
                        newModel.Name = strname;

                        if (sequence.LLAngleModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.LLAngleModels.Add(newModel);
                    }
                    break;

                case FeatureType.两圆距离:
                    CCDistanceModel CCDistanceModel = sequence.CCDistanceModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (CCDistanceModel != null)
                    {
                        CCDistanceModel newModel = CCDistanceModel.Clone();
                        newModel.Name = strname;

                        if (sequence.CCDistanceModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.CCDistanceModels.Add(newModel);
                    }
                    break;

                case FeatureType.点线距离:
                    PLDistanceModel PLDistanceModel = sequence.PLDistanceModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (PLDistanceModel != null)
                    {
                        PLDistanceModel newModel = PLDistanceModel.Clone();
                        newModel.Name = strname;

                        if (sequence.PLDistanceModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.PLDistanceModels.Add(newModel);
                    }
                    break;

                case FeatureType.点圆距离:
                    PCDistanceModel PCDistanceModel = sequence.PCDistanceModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (PCDistanceModel != null)
                    {
                        PCDistanceModel newModel = PCDistanceModel.Clone();
                        newModel.Name = strname;

                        if (sequence.PCDistanceModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.PCDistanceModels.Add(newModel);
                    }
                    break;

                case FeatureType.PLC通讯:
                    PLCStartModel PLCStartModel = sequence.PLCStartModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (PLCStartModel != null)
                    {
                        PLCStartModel newModel = PLCStartModel.Clone();
                        newModel.Name = strname;

                        if (sequence.PLCStartModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.PLCStartModels.Add(newModel);
                    }
                    break;

                case FeatureType.PLC写入:
                    PLCWriteModel PLCWriteModel = sequence.PLCWriteModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (PLCWriteModel != null)
                    {
                        PLCWriteModel newModel = PLCWriteModel.Clone();
                        newModel.Name = strname;

                        if (sequence.PLCWriteModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.PLCWriteModels.Add(newModel);
                    }
                    break;

                case FeatureType.PLC读取:
                    PLCReadModel PLCReadModel = sequence.PLCReadModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (PLCReadModel != null)
                    {
                        PLCReadModel newModel = PLCReadModel.Clone();
                        newModel.Name = strname;

                        if (sequence.PLCReadModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.PLCReadModels.Add(newModel);
                    }
                    break;

                case FeatureType.接收文本:
                    RecieveFileModel RecieveFileModel = sequence.RecieveFileModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (RecieveFileModel != null)
                    {
                        RecieveFileModel newModel = RecieveFileModel.Clone();
                        newModel.Name = strname;

                        if (sequence.RecieveFileModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.RecieveFileModels.Add(newModel);
                    }
                    break;

                case FeatureType.发送文本:
                    SendFileModel SendFileModel = sequence.SendFileModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (SendFileModel != null)
                    {
                        SendFileModel newModel = SendFileModel.Clone();
                        newModel.Name = strname;

                        if (sequence.SendFileModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.SendFileModels.Add(newModel);
                    }
                    break;

                case FeatureType.生成直线:
                    TwoPLineModel TwoPLineModel = sequence.TwoPLineModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (TwoPLineModel != null)
                    {
                        TwoPLineModel newModel = TwoPLineModel.Clone();
                        newModel.Name = strname;

                        if (sequence.TwoPLineModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.TwoPLineModels.Add(newModel);
                    }
                    break;

                case FeatureType.创建文本:
                    CreateTextModel CreateTextModel = sequence.CreateTextModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (CreateTextModel != null)
                    {
                        CreateTextModel newModel = CreateTextModel.Clone();
                        newModel.Name = strname;

                        if (sequence.CreateTextModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.CreateTextModels.Add(newModel);
                    }
                    break;

                case FeatureType.文本分割:
                    SpiltTextModel SpiltTextModel = sequence.SpiltTextModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (SpiltTextModel != null)
                    {
                        SpiltTextModel newModel = SpiltTextModel.Clone();
                        newModel.Name = strname;

                        if (sequence.SpiltTextModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.SpiltTextModels.Add(newModel);
                    }
                    break;

                case FeatureType.脚本分析:
                    ScriptModel ScriptModel = sequence.ScriptModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (ScriptModel != null)
                    {
                        ScriptModel newModel = ScriptModel.Clone();
                        newModel.Name = strname;

                        if (sequence.ScriptModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.ScriptModels.Add(newModel);
                    }
                    break;

                case FeatureType.圆弧测量:
                    FindCircleArcModel FindCircleArcModel = sequence.FindCircleArcModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (FindCircleArcModel != null)
                    {
                        FindCircleArcModel newModel = FindCircleArcModel.Clone();
                        newModel.Name = strname;

                        if (sequence.FindCircleArcModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.FindCircleArcModels.Add(newModel);
                    }
                    break;

                case FeatureType.读一维码:
                    ReadBarCodeModel ReadBarCodeModel = sequence.ReadBarCodeModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (ReadBarCodeModel != null)
                    {
                        ReadBarCodeModel newModel = ReadBarCodeModel.Clone();
                        newModel.Name = strname;

                        if (sequence.ReadBarCodeModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.ReadBarCodeModels.Add(newModel);
                    }
                    break;

                case FeatureType.字符识别:
                    OcrModel OcrModel = sequence.OcrModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (OcrModel != null)
                    {
                        OcrModel newModel = OcrModel.Clone();
                        newModel.Name = strname;

                        if (sequence.OcrModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.OcrModels.Add(newModel);
                    }
                    break;

                case FeatureType.清空队列:
                    ClearQueueModel ClearQueueModel = sequence.ClearQueueModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (ClearQueueModel != null)
                    {
                        ClearQueueModel newModel = ClearQueueModel.Clone();
                        newModel.Name = strname;

                        if (sequence.ClearQueueModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.ClearQueueModels.Add(newModel);
                    }
                    break;

                case FeatureType.数据入队:
                    EnQueueModel EnQueueModel = sequence.EnQueueModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (EnQueueModel != null)
                    {
                        EnQueueModel newModel = EnQueueModel.Clone();
                        newModel.Name = strname;

                        if (sequence.EnQueueModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.EnQueueModels.Add(newModel);
                    }
                    break;

                case FeatureType.数据出队:
                    DeQueueModel DeQueueModel = sequence.DeQueueModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (DeQueueModel != null)
                    {
                        DeQueueModel newModel = DeQueueModel.Clone();
                        newModel.Name = strname;

                        if (sequence.DeQueueModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.DeQueueModels.Add(newModel);
                    }
                    break;

                case FeatureType.变量存储:
                    SaveVarModel SaveVarModel = sequence.SaveVarModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (SaveVarModel != null)
                    {
                        SaveVarModel newModel = SaveVarModel.Clone();
                        newModel.Name = strname;

                        if (sequence.SaveVarModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.SaveVarModels.Add(newModel);
                    }
                    break;

                case FeatureType.变量读取:
                    ReadVarModel ReadVarModel = sequence.ReadVarModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (ReadVarModel != null)
                    {
                        ReadVarModel newModel = ReadVarModel.Clone();
                        newModel.Name = strname;

                        if (sequence.ReadVarModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.ReadVarModels.Add(newModel);
                    }
                    break;

                case FeatureType.一维测量:
                    OneMeasureModel OneMeasureModel = sequence.OneMeasureModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (OneMeasureModel != null)
                    {
                        OneMeasureModel newModel = OneMeasureModel.Clone();
                        newModel.Name = strname;

                        if (sequence.OneMeasureModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.OneMeasureModels.Add(newModel);
                    }
                    break;

                case FeatureType.数组设置:
                    ArraySetModel ArraySetModel = sequence.ArraySetModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (ArraySetModel != null)
                    {
                        ArraySetModel newModel = ArraySetModel.Clone();
                        newModel.Name = strname;

                        if (sequence.ArraySetModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.ArraySetModels.Add(newModel);
                    }
                    break;

                case FeatureType.圆形阵列:
                    CircleArrayModel CircleArrayModel = sequence.CircleArrayModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (CircleArrayModel != null)
                    {
                        CircleArrayModel newModel = CircleArrayModel.Clone();
                        newModel.Name = strname;

                        if (sequence.CircleArrayModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.CircleArrayModels.Add(newModel);
                    }
                    break;

                case FeatureType.矩形阵列:
                    RectArrayModel RectArrayModel = sequence.RectArrayModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (RectArrayModel != null)
                    {
                        RectArrayModel newModel = RectArrayModel.Clone();
                        newModel.Name = strname;

                        if (sequence.RectArrayModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.RectArrayModels.Add(newModel);
                    }
                    break;

                case FeatureType.取线轮廓:
                    LuxGetLineModel LuxGetLineModel = sequence.LuxGetLineModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (LuxGetLineModel != null)
                    {
                        LuxGetLineModel newModel = LuxGetLineModel.Clone();
                        newModel.Name = strname;

                        if (sequence.LuxGetLineModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.LuxGetLineModels.Add(newModel);
                    }
                    break;

                case FeatureType.区域高度:
                    LuxAreaToBaseModel LuxAreaToBaseModel = sequence.LuxAreaToBaseModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (LuxAreaToBaseModel != null)
                    {
                        LuxAreaToBaseModel newModel = LuxAreaToBaseModel.Clone();
                        newModel.Name = strname;

                        if (sequence.LuxAreaToBaseModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.LuxAreaToBaseModels.Add(newModel);
                    }
                    break;

                case FeatureType.结果显示:
                    LuxShowResultModel LuxShowResultModel = sequence.LuxShowResultModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (LuxShowResultModel != null)
                    {
                        LuxShowResultModel newModel = LuxShowResultModel.Clone();
                        newModel.Name = strname;

                        if (sequence.LuxShowResultModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.LuxShowResultModels.Add(newModel);
                    }
                    break;

                case FeatureType.交点匹配:
                    LinePMatchModel LinePMatchModel = sequence.LinePMatchModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (LinePMatchModel != null)
                    {
                        LinePMatchModel newModel = LinePMatchModel.Clone();
                        newModel.Name = strname;

                        if (sequence.LinePMatchModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.LinePMatchModels.Add(newModel);
                    }
                    break;

                case FeatureType.特征检测:
                    FeatureDetectModel FeatureDetectModel = sequence.FeatureDetectModels.FirstOrDefault(x => x.Name == tModel.Name);
                    if (FeatureDetectModel != null)
                    {
                        FeatureDetectModel newModel = FeatureDetectModel.Clone();
                        newModel.Name = strname;

                        if (sequence.FeatureDetectModels.FindIndex(x => x.Name == strname) == -1)
                            sequence.FeatureDetectModels.Add(newModel);
                    }
                    break;
            }
            #endregion

        }

        /// <summary>
        /// 获取对应模块的Model  ------新增模块需在此处增加对应类型
        /// </summary>
        /// <param name="checkFeatureModel"></param>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static BaseSeqModel GetModel(CheckFeatureModel checkFeatureModel, SingleSequenceModel sequence)
        {
            try
            {
                BaseSeqModel baseModel = null;
                switch (checkFeatureModel.featureType)
                {
                    #region 图像处理
                    case FeatureType.镭射采集:
                        baseModel = sequence.SnapImageModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.图像保存:
                        baseModel = sequence.ImageSaveModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.相机采集:
                        baseModel = sequence.Camera2DSnapModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.预处理图:
                        baseModel = sequence.PretreatImageModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.显示图像:
                        baseModel = sequence.DisplayImgModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.二值图像:
                        baseModel = sequence.ThresholdImgModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.阈值分割:
                        baseModel = sequence.ThreshodSegModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.图像剪切:
                        baseModel = sequence.ImageCutModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.图像展开:
                        baseModel = sequence.PolarImageModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.图像相减:
                        baseModel = sequence.ImageSubModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.颜色转换:
                        baseModel = sequence.TransRGBModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.图像拆分:
                        baseModel = sequence.ImageSplitModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.多图采集:
                        baseModel = sequence.MultiCameraSnapModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    #endregion

                    #region 检测识别

                    case FeatureType.读二维码:
                        baseModel = sequence.ReadCode2DModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.读一维码:
                        baseModel = sequence.ReadBarCodeModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.字符识别:
                        baseModel = sequence.OcrModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.检测点:
                        baseModel = sequence.CheckAreaModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.基准面:
                        baseModel = sequence.BaseLevelModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.检测亮度:
                        baseModel = sequence.CheckLightModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.创建ROI:
                        baseModel = sequence.CreateRegionModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.统计像素:
                        baseModel = sequence.CountPixModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.测清晰度:
                        baseModel = sequence.DefinitionModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.矩形阵列:
                        baseModel = sequence.RectArrayModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.圆形阵列:
                        baseModel = sequence.CircleArrayModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.检测正反:
                        baseModel = sequence.CheckReverseModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.斑点检测:
                        baseModel = sequence.SpotCheckModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.字符训练:
                        baseModel = sequence.OcrTrainModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.颜色抽取:
                        baseModel = sequence.ColorExtractModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    #endregion

                    #region 几何测量

                    case FeatureType.找圆:
                        baseModel = sequence.FindCircleModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.圆弧测量:
                        baseModel = sequence.FindCircleArcModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.一维测量:
                        baseModel = sequence.OneMeasureModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.找线:
                        baseModel = sequence.FindLineModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.圆心距:
                        baseModel = sequence.CircleDDModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.两线距离:
                        baseModel = sequence.LineDDModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.线圆交点:
                        baseModel = sequence.LCPointCrossModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.两线交点:
                        baseModel = sequence.LLPointCrossModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.两线角度:
                        baseModel = sequence.LLAngleModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.两圆距离:
                        baseModel = sequence.CCDistanceModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.点线距离:
                        baseModel = sequence.PLDistanceModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.点圆距离:
                        baseModel = sequence.PCDistanceModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.两圆交点:
                        baseModel = sequence.CCPointCrossModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.区域找圆:
                        baseModel = sequence.FindRegionCircleModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.矩形测量:
                        baseModel = sequence.MeasureRectModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    #endregion

                    #region 定位工具

                    case FeatureType.特征匹配:
                        baseModel = sequence.FixedItemModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.轮廓匹配:
                        baseModel = sequence.MatchingModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.灰度匹配:
                        baseModel = sequence.NccMatchingModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.交点匹配:
                        baseModel = sequence.LinePMatchModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.直线匹配:
                        baseModel = sequence.LineMatchModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.手绘模板:
                        baseModel = sequence.DrawMatchModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    #endregion

                    #region 变量工具

                    case FeatureType.变量设置:
                        baseModel = sequence.VariableValueModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.等待变量:
                        baseModel = sequence.WaitVariableModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.局部变量:
                        baseModel = sequence.LocalVariableModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.数组定义:
                        baseModel = sequence.LocalArrayModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.数组设置:
                        baseModel = sequence.ArraySetModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.文本分割:
                        baseModel = sequence.SpiltTextModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.创建文本:
                        baseModel = sequence.CreateTextModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.数据入队:
                        baseModel = sequence.EnQueueModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.数据出队:
                        baseModel = sequence.DeQueueModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.清空队列:
                        baseModel = sequence.ClearQueueModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.变量存储:
                        baseModel = sequence.SaveVarModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.变量读取:
                        baseModel = sequence.ReadVarModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    #endregion

                    #region 系统流程

                    case FeatureType.流程触发:
                        baseModel = sequence.ProcessTrigModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.发送消息:
                        baseModel = sequence.SendMessageModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.延时等待:
                        baseModel = sequence.DelaySetModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.调用流程:
                        baseModel = sequence.CallSubProModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.循环开始:
                        baseModel = sequence.LoopStartModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.循环结束:
                        baseModel = sequence.LoopEndModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.执行片段:
                        baseModel = sequence.StartPartModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.停止片段:
                        baseModel = sequence.StopPartModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.条件分支:
                        baseModel = sequence.IfElseModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.接收消息:
                        baseModel = sequence.ReciveMsgModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.判断跳转:
                        baseModel = sequence.IfGoModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.弹框提示:
                        baseModel = sequence.ShowBoxModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.条件选择:
                        baseModel = sequence.SwitchCaseModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.切换方案:
                        baseModel = sequence.ChangeProjectModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.流程结果:
                        baseModel = sequence.SeqResultModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    #endregion

                    #region 数据操作

                    case FeatureType.数据保存:
                        baseModel = sequence.DataSaveModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.数据运算:
                        baseModel = sequence.DataOperateModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.算表达式:
                        baseModel = sequence.ExpressModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.算法引擎:
                        baseModel = sequence.HADevEngineModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.时间统计:
                        baseModel = sequence.TimeComputeModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.脚本分析:
                        baseModel = sequence.ScriptModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.产品选择:
                        baseModel = sequence.SelectProductModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.脚本编辑:
                        baseModel = sequence.SharpScriptModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.显示数据:
                        baseModel = sequence.DisplayDataModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.系统时间:
                        baseModel = sequence.SystemTimeModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    #endregion

                    #region 检测3D 

                    case FeatureType.图片处理:
                        baseModel = sequence.ProcessImageModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.平面度:
                        baseModel = sequence.FlatnessModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.检测高度:
                        baseModel = sequence.PointToAreaModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.基准图像:
                        baseModel = sequence.GenFittingImageModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.图像差分:
                        baseModel = sequence.GetSubImageModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.胶路缺陷:
                        baseModel = sequence.CheckGlueDefectModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.胶路四边:
                        baseModel = sequence.CheckGlueFourthModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.外壳内边:
                        baseModel = sequence.CheckKEdgeContourModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.检测胶路:
                        baseModel = sequence.CheckGlueModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    //立讯新加的
                    case FeatureType.取线轮廓:
                        baseModel = sequence.LuxGetLineModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    case FeatureType.区域高度:
                        baseModel = sequence.LuxAreaToBaseModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    case FeatureType.结果显示:
                        baseModel = sequence.LuxShowResultModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    #endregion

                    #region 标定工具

                    case FeatureType.测量标定:
                        baseModel = sequence.MeasureCaliModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.N点标定:
                        baseModel = sequence.NineCaliModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.坐标映射:
                        baseModel = sequence.CoordMappingModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.仿射变换:
                        baseModel = sequence.AffineTransModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.畸变标定:
                        baseModel = sequence.DistrotionCaliModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.矩阵变换:
                        baseModel = sequence.MatTransModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.区域映射:
                        baseModel = sequence.RegionMapModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.旋转中心:
                        baseModel = sequence.RotateCenterModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    #endregion

                    #region 区域处理 
                    case FeatureType.面积中心:
                        baseModel = sequence.AreaCenterModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    case FeatureType.腐蚀膨胀:
                        baseModel = sequence.DErosionModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    case FeatureType.开闭运算:
                        baseModel = sequence.OpenCloseModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    case FeatureType.区域运算:
                        baseModel = sequence.RegionComputeModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    case FeatureType.区域填充:
                        baseModel = sequence.RegionFillModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    case FeatureType.区域转边:
                        baseModel = sequence.RegionToContourModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    case FeatureType.形状筛选:
                        baseModel = sequence.RegionFilterModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    case FeatureType.区域形状:
                        baseModel = sequence.RegionShapeModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    case FeatureType.特征检测:
                        baseModel = sequence.FeatureDetectModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    case FeatureType.区域排序:
                        baseModel = sequence.SortRegionModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    case FeatureType.区域分割:
                        baseModel = sequence.PartitionRegionModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    case FeatureType.顶帽处理:
                        baseModel = sequence.GrayTophatModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    #endregion

                    #region 轮廓处理 
                    case FeatureType.边缘提取:
                        baseModel = sequence.EdgeExtractModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    case FeatureType.轮廓筛选:
                        baseModel = sequence.ContourFilterModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    case FeatureType.轮廓交点:
                        baseModel = sequence.ContourIntersectModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    case FeatureType.生成十字:
                        baseModel = sequence.GenCrossTenModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    case FeatureType.生成直线:
                        baseModel = sequence.TwoPLineModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    case FeatureType.点构建:
                        baseModel = sequence.PointCreateModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    case FeatureType.点拟合圆:
                        baseModel = sequence.PointGenCircleModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    case FeatureType.边转区域:
                        baseModel = sequence.ContourToRegionModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    case FeatureType.点排序:
                        baseModel = sequence.SortPointModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    #endregion

                    #region 通讯工具

                    case FeatureType.PLC通讯:
                        baseModel = sequence.PLCStartModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.PLC写入:
                        baseModel = sequence.PLCWriteModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.PLC读取:
                        baseModel = sequence.PLCReadModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.接收文本:
                        baseModel = sequence.RecieveFileModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.发送文本:
                        baseModel = sequence.SendFileModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.读取文本:
                        baseModel = sequence.ReadFileModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.写入文本:
                        baseModel = sequence.WriteFileModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.Ftp操作:
                        baseModel = sequence.FtpUDLoadModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.光源控制:
                        baseModel = sequence.LightControlModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.批处理:
                        baseModel = sequence.RunBatchModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.连接螺批:
                        baseModel = sequence.ScrewInitModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.读螺丝批:
                        baseModel = sequence.ScrewReadModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.批量写入:
                        baseModel = sequence.PLCLotsWriteModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.上传SFC:
                        baseModel = sequence.UploadSFCModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.上传SAP:
                        baseModel = sequence.SAPControlModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    case FeatureType.斑马打印:
                        baseModel = sequence.ZebraPrintModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;

                    #endregion

                    #region 运动控制

                    case FeatureType.IO操作:
                        baseModel = sequence.IOControlModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    case FeatureType.IO检测:
                        baseModel = sequence.IOCheckModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    case FeatureType.轴回零:
                        baseModel = sequence.AxisHomeModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    case FeatureType.工站走点:
                        baseModel = sequence.CoordMoveModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    case FeatureType.走偏移量:
                        baseModel = sequence.MoveOffSetModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    case FeatureType.设置速度:
                        baseModel = sequence.SetSpeedModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    case FeatureType.停止运动:
                        baseModel = sequence.StopMoveModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    case FeatureType.等待就绪:
                        baseModel = sequence.WaitDoneModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    case FeatureType.点位修改:
                        baseModel = sequence.ModifyPosModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    case FeatureType.获取位置:
                        baseModel = sequence.GetPosModels.FirstOrDefault(x => x.Name == checkFeatureModel.Name);
                        break;
                    #endregion

                    default:
                        break;
                }

                return baseModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取子Sequence
        /// </summary>
        /// <param name="strName">模块名称</param>
        /// <returns></returns>
        public static ChildSequenceModel GetChildModel(string strName)
        {
            try
            {
                ChildSequenceModel model = new ChildSequenceModel(); 
                int index = strName.IndexOf('-');
                if(index != -1)
                {
                    string name = strName.Substring(0, index); 
                    model = sequenceModelNew.ChildSequenceModels.FirstOrDefault(x => x.Name == name);
                }

                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
    }

}
