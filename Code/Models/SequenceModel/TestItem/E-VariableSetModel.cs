using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

/// <summary>
/// 变量工具 集合
/// </summary>
namespace SequenceTestModel
{
    /// <summary>
    /// 全局变量的Model
    /// </summary>
    public class VariableSetModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public VariableType variableType { get; set; }
        
        public string Expression { get; set; }
        
        public string Description { get; set; }
        
        public string TestResult { get; set; }

        [XmlIgnore]
        public object ObjectValue { get; set; }
      
    }

    /// <summary>
    /// 变量值配置Model
    /// </summary>
    public class VariableValueModel : BaseSeqModel, IVariableSet
    {
        public string VariableName { get; set; }

        public string VariableValue { get; set; } 

        public bool IsProcessSet { get; set; }

        public string ValueForm { get; set; } 

        public VariableValueModel Clone()
        {
            VariableValueModel tModel = new VariableValueModel();
            tModel.Name = Name;
            tModel.VariableName = VariableName;
            tModel.VariableValue = VariableValue; 
            tModel.IsProcessSet = IsProcessSet;
            tModel.ValueForm = ValueForm;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.变量设置.ToString();
            }

            set
            {
                value = FeatureType.变量设置.ToString();
            }
        }

        public VariableValueModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            
        }
    }

    /// <summary>
    /// 等待变量值Model
    /// </summary>
    public class WaitVariableModel : BaseSeqModel, IVariableSet
    {
        public string VariableName { get; set; }

        public string VariableValue { get; set; }

        public string VarResetValue { get; set; }

        public bool IsVarReset { get; set; }

        public WaitVariableModel Clone()
        {
            WaitVariableModel tModel = new WaitVariableModel();
            tModel.Name = Name;
            tModel.VariableName = VariableName;
            tModel.VariableValue = VariableValue;
            tModel.VarResetValue = VarResetValue;
            tModel.IsVarReset = IsVarReset;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.等待变量.ToString();
            }

            set
            {
                value = FeatureType.等待变量.ToString();
            }
        }

        public WaitVariableModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
        }
    }

    /// <summary>
    /// 分割文本Model
    /// </summary>
    public class SpiltTextModel : BaseSeqModel, IVariableSet
    { 
        public string TextForm { get; set; }
        public bool IsFixedLength { get; set; }
        public int FixedLength { get; set; }
        public bool IsSpiltChar { get; set; }
        public string SpiltChar { get; set; } 
        public List<string> ListVar { get; set; }

        public SpiltTextModel Clone()
        {
            SpiltTextModel tModel = new SpiltTextModel();
            tModel.Name = Name;
            tModel.TextForm = TextForm;
            tModel.IsFixedLength = IsFixedLength;
            tModel.FixedLength = FixedLength;
            tModel.IsSpiltChar = IsSpiltChar;
            tModel.SpiltChar = SpiltChar;
            tModel.ListVar = ListVar;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.文本分割.ToString();
            }

            set
            {
                value = FeatureType.文本分割.ToString();
            }
        }

        public SpiltTextModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }


    /// <summary>
    /// 创建文本Model
    /// </summary>
    public class CreateTextModel : BaseSeqModel, IVariableSet
    {
        public string FormatText { get; set; }
        public string TrueConvert { get; set; }
        public string FalseConvert { get; set; }
        public int DecimalNum { get; set; }
        public string SpiltChar { get; set; }
        public bool IsSaveText { get; set; }
        public string SavePath { get; set; }

        public List<LinkValue> ListValue { get; set; }

        public CreateTextModel Clone()
        {
            CreateTextModel tModel = new CreateTextModel();
            tModel.Name = Name;
            tModel.FormatText = FormatText;
            tModel.TrueConvert = TrueConvert;
            tModel.FalseConvert = FalseConvert;
            tModel.DecimalNum = DecimalNum;
            tModel.SpiltChar = SpiltChar;
            tModel.IsSaveText = IsSaveText;
            tModel.SavePath = SavePath;
            tModel.ListValue = ListValue;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.创建文本.ToString();
            }

            set
            {
                value = FeatureType.创建文本.ToString();
            }
        }

        public CreateTextModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            
        }


        public class LinkValue
        {
            public string Name { get; set; }
            public string NameForm { get; set; }
        }
    }


    /// <summary>
    /// 变量存储Model
    /// </summary>
    public class SaveVarModel : BaseSeqModel, IVariableSet
    {
        public string SavePath { get; set; }

        public List<SaveVarValue> ListVar { get; set; }

        public SaveVarModel Clone()
        {
            SaveVarModel tModel = new SaveVarModel();
            tModel.Name = Name; 
            tModel.SavePath = SavePath;
            tModel.ListVar = ListVar;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.变量存储.ToString();
            }

            set
            {
                value = FeatureType.变量存储.ToString();
            }
        }

        public SaveVarModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }


        public class SaveVarValue
        {
            public int Id { get; set; }

            public string ValueType { get; set; }

            public string Name { get; set; }
        }
    }

    /// <summary>
    /// 变量读取Model
    /// </summary>
    public class ReadVarModel : BaseSeqModel, IVariableSet
    {
        public string SavePath { get; set; }

        public List<ReadVarValue> ListVar { get; set; }

        public ReadVarModel Clone()
        {
            ReadVarModel tModel = new ReadVarModel();
            tModel.Name = Name;
            tModel.SavePath = SavePath;
            tModel.ListVar = ListVar;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.变量读取.ToString();
            }

            set
            {
                value = FeatureType.变量读取.ToString();
            }
        }

        public ReadVarModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }


        public class ReadVarValue
        {
            public int Id { get; set; }

            public string ValueType { get; set; }

            public string Name { get; set; }

            public string Value { get; set; }
        }
    }


    /// <summary>
    /// 局部变量Model
    /// </summary>
    public class LocalVariableModel : BaseSeqModel, IVariableSet
    {
        public override int Id { get; set; }
        public override string Name { get; set; }

        public List<Variable> ListVariable { get; set; }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.局部变量.ToString();
            }

            set
            {
                value = FeatureType.局部变量.ToString();
            }
        }

        public LocalVariableModel()
        {
            itemResult = new ItemResult();
        }

        public LocalVariableModel Clone()
        {
            LocalVariableModel tModel = new LocalVariableModel();
            tModel.ListVariable = ListVariable;

            return tModel;
        }

        public class ItemResult : BaseSeqResultModel
        {

        }

        public class Variable
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public VariableType variableType { get; set; }

            public string Expression { get; set; }

            public string Description { get; set; }

            [XmlIgnore]
            public string TestResult { get; set; }

            [XmlIgnore]
            public object ObjectValue { get; set; }
        }
    }

    /// <summary>
    /// 局部数值定义Model
    /// </summary>
    public class LocalArrayModel : BaseSeqModel, IVariableSet
    {
        public override int Id { get; set; }
        public override string Name { get; set; }

        public List<ArrayVar> ListArray { get; set; }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.数组定义.ToString();
            }

            set
            {
                value = FeatureType.数组定义.ToString();
            }
        }


        public LocalArrayModel()
        {
            itemResult = new ItemResult();
        }

        public class ItemResult : BaseSeqResultModel
        {

        }

        public LocalArrayModel Clone()
        {
            LocalArrayModel tModel = new LocalArrayModel();
            tModel.ListArray = ListArray;

            return tModel;
        }
        
        /// <summary>
        /// 数组类
        /// </summary>
        public class ArrayVar
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public ArrayType arrayType { get; set; }

            public string Expression { get; set; }

            public string Description { get; set; }

            [XmlIgnore]
            public string TestResult { get; set; }

            [XmlIgnore]
            public object ObjectValue { get; set; }
        }
    }


    /// <summary>
    /// 数组设置Model
    /// </summary>
    public class ArraySetModel : BaseSeqModel, IVariableSet
    {
        public string ArrayForm { get; set; }
        public string VariableName { get; set; }
        public List<string> ListValue { get; set; }

        public ArraySetModel Clone()
        {
            ArraySetModel tModel = new ArraySetModel();
            tModel.Name = Name;
            tModel.ArrayForm = ArrayForm;
            tModel.VariableName = VariableName;
            tModel.ListValue = ListValue;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.数组设置.ToString();
            }

            set
            {
                value = FeatureType.数组设置.ToString();
            }
        }

        public ArraySetModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
        }
    }

    /// <summary>
    /// 数据入队 Model
    /// </summary>
    public class EnQueueModel : BaseSeqModel, IVariableSet
    {
        public override int Id { get; set; }
        public override string Name { get; set; }

        public string DeQueueForm { get; set; }

        public int StartIndex { get; set; }

        public List<QueueValue> ListValue { get; set; }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.数据入队.ToString();
            }

            set
            {
                value = FeatureType.数据入队.ToString();
            }
        }


        public EnQueueModel()
        {
            itemResult = new ItemResult();
        }

        public class ItemResult : BaseSeqResultModel
        {
            [Description("队列入队数量")]
            public int Count { get; set; }
        }

        public EnQueueModel Clone()
        {
            EnQueueModel tModel = new EnQueueModel();
            tModel.ListValue = ListValue;
            tModel.DeQueueForm = DeQueueForm;

            return tModel;
        }

        /// <summary>
        /// 队列类
        /// </summary>
        public class QueueValue
        {
            public int Id { get; set; }

            public string Name { get; set; } 
        }
    }


    /// <summary>
    /// 数据出队 Model
    /// </summary>
    public class DeQueueModel : BaseSeqModel, IVariableSet
    {
        public override int Id { get; set; }

        public override string Name { get; set; } 

        public bool IsDelData { get; set; }

        public List<DeQueueValue> ListDeValue { get; set; }
        
        [XmlIgnore]
        public Queue<object> QueueValues  { get; set; }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        public string EnQueueForm { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.数据出队.ToString();
            }

            set
            {
                value = FeatureType.数据出队.ToString();
            }
        }
        
        public DeQueueModel()
        {
            itemResult = new ItemResult();
        }

        public class ItemResult : BaseSeqResultModel
        {

        }

        public DeQueueModel Clone()
        {
            DeQueueModel tModel = new DeQueueModel(); 

            return tModel;
        }

        public class DeQueueValue
        {
            public int Id { get; set; }

            public string ValueType { get; set; }

            public string Name { get; set; }

            public string Value { get; set; }
        }

    }


    /// <summary>
    /// 清空队列 Model
    /// </summary>
    public class ClearQueueModel : BaseSeqModel, IVariableSet
    {
        public override int Id { get; set; }
        public override string Name { get; set; }
        public string DeQueueForm { get; set; }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.清空队列.ToString();
            }

            set
            {
                value = FeatureType.清空队列.ToString();
            }
        }


        public ClearQueueModel()
        {
            itemResult = new ItemResult();
        }

        public class ItemResult : BaseSeqResultModel
        {

        }

        public ClearQueueModel Clone()
        {
            ClearQueueModel tModel = new ClearQueueModel();

            return tModel;
        }
    }

    /// <summary>
    /// 变量类型Enum
    /// </summary>
    public enum VariableType
    {
        Int,
        Double,
        Bool,
        String,
        Object,
    }

    /// <summary>
    /// 数组类型Enum
    /// </summary>
    public enum ArrayType
    {
        IntArray,
        DoubleArray,
        BoolArray,
        StringArray,
        ObjectArray,
    }
}
