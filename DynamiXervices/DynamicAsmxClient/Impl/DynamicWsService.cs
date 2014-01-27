using System;
using System.Linq;
using System.Reflection;
using System.Web.Services.Protocols;
using Taga.DynamicServices.Client;
using Taga.DynamicServices.Client.Base;

namespace Taga.DynamicServices.AsmxClient.Impl
{
    class DynamicWsService : DynamicServiceBase
    {
        private object _serviceInstance;

        public DynamicWsService(Type serviceType)
            : base(serviceType)
        {
        }

        private object ServiceInstance
        {
            get { return _serviceInstance ?? (_serviceInstance = Type.CreateInstance()); }
        }

        public override string Address
        {
            get
            {
                return base.Address;
            }
            set
            {
                base.Address = value;
                (ServiceInstance as SoapHttpClientProtocol).Url = value;
            }
        }

        protected override IDynamicMethod CreateMethod(MethodInfo mi)
        {
            return new DynamicWsMethod(mi, ServiceInstance);
        }

        protected override bool IsDynamicMethod(MethodInfo mi)
        {
            return mi.GetCustomAttributes<SoapDocumentMethodAttribute>().Any();
        }
    }
}
