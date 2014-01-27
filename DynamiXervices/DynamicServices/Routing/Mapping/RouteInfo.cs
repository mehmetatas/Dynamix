using System.Xml.Serialization;

namespace Taga.DynamicServices.Routing.Mapping
{
    public class RouteInfo : Mapping
    {
        [XmlAttribute("key")]
        public string Key { get; set; }

        [XmlAttribute("wsdlUri")]
        public string WsdlUri { get; set; }

        [XmlAttribute("mode")]
        public string Mode { get; set; }

        [XmlAttribute("address")]
        public string Address { get; set; }

        [XmlElement("input")]
        public MappingList InputMapping { get; set; }

        [XmlElement("output")]
        public MappingList OutputMapping { get; set; }
        
        public string SourceServiceName
        {
            get { return GetServiceName(Source); }
        }

        public string SourceMethodName
        {
            get { return GetMethodName(Source); }
        }

        public string TargetServiceName
        {
            get { return GetServiceName(Target); }
        }

        public string TargetMethodName
        {
            get { return GetMethodName(Target); }
        }

        private static string GetServiceName(string sourceOrTarget)
        {
            var lastIndexOfDot = sourceOrTarget.LastIndexOf('.');
            return lastIndexOfDot < 0
                ? sourceOrTarget
                : sourceOrTarget.Substring(0, lastIndexOfDot);
        }

        private static string GetMethodName(string sourceOrTarget)
        {
            var lastIndexOfDot = sourceOrTarget.LastIndexOf('.');
            return lastIndexOfDot < 0
                ? sourceOrTarget
                : sourceOrTarget.Substring(lastIndexOfDot + 1);
        }
    }
}