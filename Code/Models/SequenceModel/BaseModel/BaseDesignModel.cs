using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SequenceTestModel
{
    public abstract class BaseDesignModel
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int X { get; set; }
        public virtual int Y { get; set; } 
        public virtual int Height { get; set; }
        public virtual int Width { get; set; }
        public virtual string EBackColor { get; set; }
    }
    
    /// <summary>
    /// Design根节点
    /// </summary>
    [XmlRoot("DesginModel")]
    public class DesginModel : BaseDesignModel
    {
        /// <summary>
        /// EButtonList
        /// </summary>
        [XmlArrayItem("EButtonModels")]
        public List<EButtonModel> EButtonModels { get; set; }

        /// <summary>
        /// EButtonProList
        /// </summary>
        [XmlArrayItem("EButtonProModels")]
        public List<EButtonProModel> EButtonProModels { get; set; }

        /// <summary>
        /// EDataOutputList
        /// </summary>
        [XmlArrayItem("EDataOutputModels")]
        public List<EDataOutputModel> EDataOutputModels { get; set; }

        /// <summary>
        /// EHSmartWindowList
        /// </summary>
        [XmlArrayItem("EHSmartWindowModels")]
        public List<EHSmartWindowModel> EHSmartWindowModels { get; set; }

        /// <summary>
        /// ELblResultList
        /// </summary>
        [XmlArrayItem("ELblResultModels")]
        public List<ELblResultModel> ELblResultModels { get; set; }

        /// <summary>
        /// ELblStatusList
        /// </summary>
        [XmlArrayItem("ELblStatusModels")]
        public List<ELblStatusModel> ELblStatusModels { get; set; }

        /// <summary>
        /// ELogList
        /// </summary>
        [XmlArrayItem("ELogModels")]
        public List<ELogModel> ELogModels { get; set; }

        /// <summary>
        /// ETextBoxList
        /// </summary>
        [XmlArrayItem("ETextBoxModels")]
        public List<ETextBoxModel> ETextBoxModels { get; set; }
         
        /// <summary>
        /// ESetTextList
        /// </summary>
        [XmlArrayItem("ETextBoxModels")]
        public List<ESetTextModel> ESetTextModels { get; set; }

        /// <summary>
        /// EComboProductList
        /// </summary>
        [XmlArrayItem("EComboProductModels")]
        public List<EComboProductModel> EComboProductModels { get; set; }

        /// <summary>
        /// EItemResultList
        /// </summary>
        [XmlArrayItem("EItemResultModels")]
        public List<EItemResultModel> EItemResultModels { get; set; }


        /// <summary>
        /// EButtonProList
        /// </summary>
        [XmlArrayItem("EErrorItemModels")]
        public List<EErrorItemModel> EErrorItemModels { get; set; }

        /// <summary>
        /// ECheckList
        /// </summary>
        [XmlArrayItem("ECheckModels")]
        public List<ECheckModel> ECheckModels { get; set; }

        /// <summary>
        /// EProductSelList
        /// </summary>
        [XmlArrayItem("EProductSelModels")]
        public List<EProductSelModel> EProductSelModels { get; set; }

        /// <summary>
        /// ELightList
        /// </summary>
        [XmlArrayItem("ELightModels")]
        public List<ELightModel> ELightModels { get; set; }
         
        /// <summary>
        /// EGroupBoxList
        /// </summary>
        [XmlArrayItem("EGroupBoxModels")]
        public List<EGroupBoxModel> EGroupBoxModels { get; set; }
         
        /// <summary>
        /// EComboList
        /// </summary>
        [XmlArrayItem("EComboModels")]
        public List<EComboModel> EComboModels { get; set; }

        public DesginModel()
        {
            Id = 0;
            Name = "设计界面";
            EButtonModels = new List<EButtonModel>();
            EButtonProModels = new List<EButtonProModel>();
            EDataOutputModels = new List<EDataOutputModel>();
            EHSmartWindowModels = new List<EHSmartWindowModel>();
            ELblResultModels = new List<ELblResultModel>();
            ELblStatusModels = new List<ELblStatusModel>();
            ELogModels = new List<ELogModel>();
            ETextBoxModels = new List<ETextBoxModel>();
            ESetTextModels = new List<ESetTextModel>();
            EComboProductModels = new List<EComboProductModel>();
            EItemResultModels = new List<EItemResultModel>();
            EErrorItemModels = new List<EErrorItemModel>();
            ECheckModels = new List<ECheckModel>();
            EProductSelModels = new List<EProductSelModel>();
            ELightModels = new List<ELightModel>();
            EGroupBoxModels = new List<EGroupBoxModel>();
            EComboModels = new List<EComboModel>();
        }
    }
}
