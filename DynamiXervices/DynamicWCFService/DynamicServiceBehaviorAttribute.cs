using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace Taga.DynamicServices.WCFService
{
    public class DynamicServiceBehaviorAttribute : Attribute, IServiceBehavior
    {
        public Type InvocationHandlerType { get; set; }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {

        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            serviceDescription.Endpoints.ToList().ForEach(ep => ep.Behaviors.Add(new DynamicEndpointBehavior()));

            var binder = new DynamicServiceBinder();
            binder.AddCustomOperations(serviceDescription);
        }
    }
}