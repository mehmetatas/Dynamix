using System.Xml.Serialization;

namespace Taga.DynamicServices.Wsdl
{
    public abstract class WsdlNamedElement : WsdlElement
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        public override string ToString()
        {
            return Name + " : " + FullTypeName;
        }
    }
}
