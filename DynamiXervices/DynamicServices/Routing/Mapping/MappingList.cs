using System.Collections.Generic;
using System.Xml.Serialization;

namespace Taga.DynamicServices.Routing.Mapping
{
    public class MappingList
    {
        [XmlElement("map")]
        public List<ParameterMapping> Mappings { get; set; }
    }
}
