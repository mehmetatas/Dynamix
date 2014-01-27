using System.Linq;

namespace DynamicWebServiceClient
{
    internal class WebServices : WebCollection<WebService>
    {
        internal override WebService this[string name]
        {
            get { return this.FirstOrDefault(s => s.Name == name); }
        }
    }
}
