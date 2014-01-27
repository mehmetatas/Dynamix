using System.Collections.Generic;
using System.Xml.Serialization;

namespace Taga.DynamicServices.Wsdl
{
    public class WsdlTypes
    {
        [XmlElement("type")]
        public List<WsdlType> Types { get; set; }
    }
}
