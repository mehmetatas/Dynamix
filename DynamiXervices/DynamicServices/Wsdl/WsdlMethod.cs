using System.Collections.Generic;
using System.Xml.Serialization;

namespace Taga.DynamicServices.Wsdl
{
    public class WsdlMethod : WsdlNamedElement
    {
        [XmlElement("input")]
        public List<WsdlInput> Inputs { get; set; }
    }
}
