using System.Collections.Generic;
using System.Xml.Serialization;

namespace Taga.DynamicServices.Wsdl
{
    public class WsdlService : WsdlElement
    {
        [XmlAttribute("namespace")]
        public string Namespace { get; set; }

        [XmlElement("method")]
        public List<WsdlMethod> Methods { get; set; }
    }
}
