using System.Xml.Serialization;

namespace Taga.DynamicServices.Wsdl
{
    [XmlRoot(ElementName = "swsdl")]
    public class SimpleWsdl
    {
        [XmlElement("types")]
        public WsdlTypes Types { get; set; }

        [XmlElement("services")]
        public WsdlServices Services { get; set; }
    }
}
