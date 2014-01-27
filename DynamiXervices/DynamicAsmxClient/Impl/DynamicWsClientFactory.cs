using Taga.DynamicServices.Client;

namespace Taga.DynamicServices.AsmxClient.Impl
{
    public class DynamicWsClientFactory : IDynamicClientFactory
    {
        public IDynamicClient GetClient(string wsdlUri)
        {
            return new DynamicWsClient(wsdlUri);
        }
    }
}
