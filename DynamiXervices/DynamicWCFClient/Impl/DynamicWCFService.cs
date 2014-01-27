using System;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using Taga.DynamicServices.Client;
using Taga.DynamicServices.Client.Base;

namespace Taga.DynamicServices.WCFClient.Impl
{
    class DynamicWCFService : DynamicServiceBase
    {
        private readonly DynamicProxy _proxy;

        internal DynamicWCFService(DynamicProxy proxy)
            : base(proxy.ProxyType)
        {
            _proxy = proxy;
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
                _proxy.SetProperty("Endpoint.Address", new EndpointAddress(Address));
            }
        }

        protected override IDynamicMethod CreateMethod(MethodInfo mi)
        {
            return new DynamicWCFMethod(mi, _proxy.Proxy);
        }

        protected override bool IsDynamicMethod(MethodInfo mi)
        {
            return mi.DeclaringType == Type;
        }
    }
}
