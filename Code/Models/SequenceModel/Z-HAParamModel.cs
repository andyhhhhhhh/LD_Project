using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

/// <summary>
/// 提取halcon程序中的代码部分
/// </summary>
namespace SequenceTestModel
{
    [XmlRoot("hdevelop")]
    public class HAParamModel
    {
        [XmlAttribute("file_version")]
        public string fileversion { get; set; }

        [XmlAttribute("halcon_version")]
        public string halconversion { get; set; }

        //[XmlElement(typeof(List<Procdeure>))]
        //public List<Procdeure> procedure { get; set; }

        [XmlElement()]
        public List<Procdeure> procedure { get; set; }

        public HAParamModel()
        {
            procedure = new List<Procdeure>();
        }
    }

    public class Procdeure
    {
        [XmlAttribute("name")]
        public string name { get; set; }

        [XmlElement("interface")]
        public Interface Interface { get; set; }

        [XmlElement("body")]
        public Body body { get; set; }

        [XmlElement("docu")]
        public Docu docu { get; set; }

        public Procdeure()
        {
            Interface = new Interface();
            body = new Body();
            docu = new Docu();
        }
    }

    public class Interface
    {
        [XmlElement("io")]
        public Io io { get; set; }
        [XmlElement("ic")]
        public Ic ic { get; set; }
        [XmlElement("oc")]
        public Oc oc { get; set; }

        public class Io
        {
            //[XmlArrayItem("par")] 
            [XmlElement("par")]
            public List<Param> listParam { get; set; }
        }

        public class Ic
        { 
            [XmlElement("par")]
            public List<Param> listParam { get; set; }
        }

        public class Oc
        {
            [XmlElement("par")]
            public List<Param> listParam { get; set; }

            public Oc()
            {
                listParam = new List<Param>();
            }
        }

        public class Param
        {
            [XmlAttribute("name")]
            public string name { get; set; }

            [XmlAttribute("base_type")]
            public string base_type { get; set; }

            [XmlAttribute("dimension")]
            public string dimension { get; set; }
        }
    }

    public class Body
    {
        [XmlElement("c")]
        public List<string> listc;

        [XmlElement("l")]
        public List<string> listl;
        public class c
        {
            [XmlText]
            public string value { get; set; }
        }

        public Body()
        {
             
        }
    }

    public class Docu
    {
        [XmlAttribute("id")]
        public string id { get; set; }

        public List<parameter> parameters { get; set; }
        public class parameter
        {
            [XmlAttribute("id")]
            public string id { get; set; }
        }

        public Docu()
        {
            parameters = new List<parameter>();
        }
    }

    
}
