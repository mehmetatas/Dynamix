using System.Collections.Generic;
using System.Linq;
using System.Web.Services.Protocols;
using Taga.DynamicServices.Client;
using Taga.DynamicServices.Client.Base;

namespace Taga.DynamicServices.AsmxClient.Impl
{
    class DynamicWsClient : DynamicClientBase
    {
        private readonly WsdlParser _wsdlParser;

        internal DynamicWsClient(string wsdlUri)
        {
            _wsdlParser = new WsdlParser(wsdlUri);
            _wsdlParser.Parse();
        }

        protected override IEnumerable<IDynamicService> LoadServices()
        {
            return _wsdlParser.AvailableTypes
                .Where(type => type.BaseType == typeof (SoapHttpClientProtocol))
                .Select(type => new DynamicWsService(type))
                .ToList();
        }
    }
}
