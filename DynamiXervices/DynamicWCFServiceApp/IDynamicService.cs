using System.ServiceModel;

namespace DynamicWebServiceApp
{
    [ServiceContract(Namespace = "http://dynamic.services.Taga.com.tr/")]
    public interface IDynamicService
    {
        [OperationContract]
        void IsAlive();
    }
}
