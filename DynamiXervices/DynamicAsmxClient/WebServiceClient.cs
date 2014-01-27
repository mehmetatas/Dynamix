using System.Linq;
using System.Web.Services.Protocols;

namespace DynamicWebServiceClient
{
    class WebServiceClient
    {
        private readonly WsdlParser _wsdlParser;
        private WebServices _services;

        internal WebServiceClient(string wsdlUri)
        {
            _wsdlParser = new WsdlParser(wsdlUri);
        }

        internal WebServices Services
        {
            get
            {
                if (_services == null)
                    LoadServices();
                return _services;
            }
        }

        private void LoadServices()
        {
            _wsdlParser.Parse();
            _services = new WebServices();
            _services.AddRange(_wsdlParser.AvailableTypes.Values
                .Where(t => t.BaseType == typeof(SoapHttpClientProtocol))
                .Select(t => new WebService(t)));
        }
    }
}
