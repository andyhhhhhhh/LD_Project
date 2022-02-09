using SequenceTestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

/// <summary>
/// 运动控制集合
/// </summary>
namespace SequenceTestModel
{ 
    /// <summary>
    /// IO操作 Model
    /// </summary>
    public class IOControlModel : BaseSeqModel, IMotorModel
    {
        public string IOName1 { get; set; }
        public string IOName2 { get; set; }
        public string IOName3 { get; set; }
        public bool IsWrite1 { get; set; }
        public bool IsWrite2 { get; set; }
        public bool IsWrite3 { get; set; }

        public IOControlModel Clone()
        {
            IOControlModel tModel = new IOControlModel();
            tModel.Name = Name;
            tModel.IOName1 = IOName1;
            tModel.IOName2 = IOName2;
            tModel.IOName3 = IOName3;
            tModel.IsWrite1 = IsWrite1;
            tModel.IsWrite2 = IsWrite2;
            tModel.IsWrite3 = IsWrite3;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.IO操作.ToString();
            }

            set
            {
                value = FeatureType.IO操作.ToString();
            }
        }

        public IOControlModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            
        }
    }
    /// <summary>
    /// IO检测 Model
    /// </summary>
    public class IOCheckModel : BaseSeqModel, IMotorModel
    {
        public string IOName1 { get; set; }
        public string IOName2 { get; set; }
        public string IOName3 { get; set; }
        public bool IsRead1 { get; set; }
        public bool IsRead2 { get; set; }
        public bool IsRead3 { get; set; } 

        public IOCheckModel Clone()
        {
            IOCheckModel tModel = new IOCheckModel();
            tModel.Name = Name;
            tModel.IOName1 = IOName1;
            tModel.IOName2 = IOName2;
            tModel.IOName3 = IOName3;
            tModel.IsRead1 = IsRead1;
            tModel.IsRead2 = IsRead2;
            tModel.IsRead3 = IsRead3;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.IO检测.ToString();
            }

            set
            {
                value = FeatureType.IO检测.ToString();
            }
        }

        public IOCheckModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }
    /// <summary>
    /// 轴回零 Model
    /// </summary>
    public class AxisHomeModel : BaseSeqModel, IMotorModel
    {
        public string StationName { get; set; }

        public bool IsWait { get; set; }

        public AxisHomeModel Clone()
        {
            AxisHomeModel tModel = new AxisHomeModel();
            tModel.Name = Name;
            tModel.StationName = StationName;
            tModel.IsWait = IsWait;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.轴回零.ToString();
            }

            set
            {
                value = FeatureType.轴回零.ToString();
            }
        }

        public AxisHomeModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }
    /// <summary>
    /// 工站走点 Model
    /// </summary>
    public class CoordMoveModel : BaseSeqModel, IMotorModel
    {
        public string StationName { get; set; }

        public bool IsWait { get; set; }

        public string PointName { get; set; }

        public double Speed { get; set; }
        public double Acc { get; set; }
        public double Dec { get; set; }

        public CoordMoveModel Clone()
        {
            CoordMoveModel tModel = new CoordMoveModel();
            tModel.Name = Name;
            tModel.StationName = StationName;            
            tModel.IsWait = IsWait;
            tModel.PointName = PointName;
            tModel.Speed = Speed;
            tModel.Acc = Acc;
            tModel.Dec = Dec;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.工站走点.ToString();
            }

            set
            {
                value = FeatureType.工站走点.ToString();
            }
        }

        public CoordMoveModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }
    /// <summary>
    /// 走偏移量 Model
    /// </summary>
    public class MoveOffSetModel : BaseSeqModel, IMotorModel
    {
        public string StationName { get; set; }

        public bool IsWait { get; set; }

        public string PointName { get; set; }

        public string OffSetX { get; set; }
        public string OffSetY { get; set; }
        public string OffSetZ { get; set; }
        public string OffSetU { get; set; } 

        public MoveOffSetModel Clone()
        {
            MoveOffSetModel tModel = new MoveOffSetModel();
            tModel.Name = Name;
            tModel.StationName = StationName;
            tModel.IsWait = IsWait;
            tModel.PointName = PointName;
            tModel.OffSetX = OffSetX;
            tModel.OffSetY = OffSetY;
            tModel.OffSetZ = OffSetZ;
            tModel.OffSetU = OffSetU;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.走偏移量.ToString();
            }

            set
            {
                value = FeatureType.走偏移量.ToString();
            }
        }

        public MoveOffSetModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }
    /// <summary>
    /// 设置速度 Model
    /// </summary>
    public class SetSpeedModel : BaseSeqModel, IMotorModel
    {
        public string StationName { get; set; } 

        public string AxisName { get; set; }

        public double Speed { get; set; }
        public double Acc { get; set; }
        public double Dec { get; set; }

        public SetSpeedModel Clone()
        {
            SetSpeedModel tModel = new SetSpeedModel();
            tModel.Name = Name;
            tModel.StationName = StationName;
            tModel.AxisName = AxisName;
            tModel.Speed = Speed;
            tModel.Acc = Acc;
            tModel.Dec = Dec;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.设置速度.ToString();
            }

            set
            {
                value = FeatureType.设置速度.ToString();
            }
        }

        public SetSpeedModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }

    /// <summary>
    /// 停止运动 Model
    /// </summary>
    public class StopMoveModel : BaseSeqModel, IMotorModel
    {
        public string StationName { get; set; }

        public StopMoveModel Clone()
        {
            StopMoveModel tModel = new StopMoveModel();
            tModel.Name = Name;
            tModel.StationName = StationName;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.停止运动.ToString();
            }

            set
            {
                value = FeatureType.停止运动.ToString();
            }
        }

        public StopMoveModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }

    /// <summary>
    /// 等待就绪 Model
    /// </summary>
    public class WaitDoneModel : BaseSeqModel, IMotorModel
    {
        public string StationName { get; set; }

        public WaitDoneModel Clone()
        {
            WaitDoneModel tModel = new WaitDoneModel();
            tModel.Name = Name;
            tModel.StationName = StationName;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.等待就绪.ToString();
            }

            set
            {
                value = FeatureType.等待就绪.ToString();
            }
        }

        public WaitDoneModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }
    /// <summary>
    /// 点位修改 Model
    /// </summary>
    public class ModifyPosModel : BaseSeqModel, IMotorModel
    {
        public string StationName { get; set; }

        public bool IsOffSet { get; set; }

        public string BasePoint { get; set; }
        public string ModifyPoint { get; set; }

        public string OffSetX { get; set; }
        public string OffSetY { get; set; }
        public string OffSetZ { get; set; }
        public string OffSetU { get; set; }


        public ModifyPosModel Clone()
        {
            ModifyPosModel tModel = new ModifyPosModel();
            tModel.Name = Name;
            tModel.StationName = StationName;
            tModel.IsOffSet = IsOffSet;
            tModel.BasePoint = BasePoint;
            tModel.ModifyPoint = ModifyPoint;
            tModel.OffSetX = OffSetX;
            tModel.OffSetY = OffSetY;
            tModel.OffSetZ = OffSetZ;
            tModel.OffSetU = OffSetU;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.点位修改.ToString();
            }

            set
            {
                value = FeatureType.点位修改.ToString();
            }
        }

        public ModifyPosModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {

        }
    }
    /// <summary>
    /// 获取位置 Model
    /// </summary>
    public class GetPosModel : BaseSeqModel, IMotorModel
    {
        public string StationName { get; set; }
        public string PointName { get; set; }

        public GetPosModel Clone()
        {
            GetPosModel tModel = new GetPosModel();
            tModel.Name = Name;
            tModel.StationName = StationName;
            tModel.PointName = PointName;

            return tModel;
        }

        [XmlIgnore]
        public ItemResult itemResult { get; set; }

        [XmlIgnore]
        public override string BaseType
        {
            get
            {
                return FeatureType.获取位置.ToString();
            }

            set
            {
                value = FeatureType.获取位置.ToString();
            }
        }

        public GetPosModel()
        {
            itemResult = new ItemResult();
        }

        //结果
        public class ItemResult : BaseSeqResultModel
        {
            public double posX { get; set; }
            public double posY { get; set; }
            public double posZ { get; set; }
            public double posU { get; set; }
        }
    }
}
