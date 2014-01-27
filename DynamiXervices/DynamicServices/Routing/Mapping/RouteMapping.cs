using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml.Serialization;

namespace Taga.DynamicServices.Routing.Mapping
{
    [XmlRoot("route-mapping")]
    public class RouteMapping
    {
        [XmlElement("route")]
        public List<RouteInfo> Routes { get; set; }

        public RouteInfo GetRoute(string key, string source)
        {
            var route = Routes.FirstOrDefault(r => r.Key == key && r.Source == source);

            if (route == null)
                throw new ApplicationException(String.Format("No route found for key: {0}, source: {1}", key, source));

            return route;
        }

        private static RouteMapping _instance;
        public static RouteMapping GetRouteMapping()
        {
            if (_instance == null)
            {
                var routeFile = ConfigurationManager.AppSettings["RouteMappingFile"];

                var serializer = new XmlSerializer<RouteMapping>();
                _instance = serializer.Deserialize(routeFile);
            }
            return _instance;
        }
    }
}
