using System.Linq;

namespace DynamicWebServiceClient
{
    internal class WebParameters : WebCollection<WebParameter>
    {
        internal override WebParameter this[string name]
        {
            get { return this.FirstOrDefault(p => p.Name == name); }
        }
    }
}
