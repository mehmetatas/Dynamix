using System.Collections.Generic;
using System.Xml.Serialization;

namespace Taga.DynamicServices.Wsdl
{
    public class WsdlServices
    {
        [XmlElement("service")]
        public List<WsdlService> Services { get; set; }
    }
}
