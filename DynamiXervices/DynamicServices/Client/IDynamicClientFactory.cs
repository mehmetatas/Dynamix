
namespace Taga.DynamicServices.Client
{
    public interface IDynamicClientFactory
    {
        IDynamicClient GetClient(string wsdlUri);
    }
}
