using System.ServiceModel;
using Taga.DynamicServices.WCFService;

namespace DynamicWebServiceApp
{
    [DynamicServiceBehavior]
    [ServiceBehavior(Namespace = "http://dynamic.services.Taga.com.tr/")]
    public class DynamicService : IDynamicService
    {
        public void IsAlive()
        {
        }
    }
}