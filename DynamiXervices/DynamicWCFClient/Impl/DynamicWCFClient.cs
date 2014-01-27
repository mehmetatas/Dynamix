using System.Collections.Generic;
using System.Linq;
using Taga.DynamicServices.Client;
using Taga.DynamicServices.Client.Base;

namespace Taga.DynamicServices.WCFClient.Impl
{
    class DynamicWCFClient : DynamicClientBase
    {
        private readonly DynamicProxyFactory _factory;

        internal DynamicWCFClient(string wsdlUri)
        {
            _factory = new DynamicProxyFactory(wsdlUri);
            _factory.Init();
        }

        protected override IEnumerable<IDynamicService> LoadServices()
        {
            return _factory.Contracts.Select(c => new DynamicWCFService(_factory.CreateProxy(c.Name))).ToList();
        }
    }
}
