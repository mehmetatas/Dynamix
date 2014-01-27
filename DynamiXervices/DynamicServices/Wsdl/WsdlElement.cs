using System;
using System.Xml.Serialization;

namespace Taga.DynamicServices.Wsdl
{
    public abstract class WsdlElement
    {
        [XmlAttribute("type")]
        public string FullTypeName { get; set; }

        [XmlIgnore]
        public Type Type { get; internal set; }

        public override string ToString()
        {
            return FullTypeName;
        }
    }
}
