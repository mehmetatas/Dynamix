using System.Xml.Serialization;

namespace Taga.DynamicServices.Routing.Mapping
{
    public abstract class Mapping
    {
        [XmlAttribute("source")]
        public string Source { get; set; }

        [XmlAttribute("target")]
        public string Target { get; set; }

        public override string ToString()
        {
            return Source + " -> " + Target;
        }
    }
}
