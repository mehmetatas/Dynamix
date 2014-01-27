using Taga.DynamicServices.Client;

namespace Taga.DynamicServices.WCFClient.Impl
{
    public class DynamicWCFClientFactory : IDynamicClientFactory
    {
        public IDynamicClient GetClient(string wsdlUri)
        {
            return new DynamicWCFClient(wsdlUri);
        }
    }
}
