using System.Linq;

namespace DynamicWebServiceClient
{
    internal class WebMethods : WebCollection<WebMethod>
    {
        internal override WebMethod this[string name]
        {
            get { return this.FirstOrDefault(m => m.Name == name); }
        }
    }
}
