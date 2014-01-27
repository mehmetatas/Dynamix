using System.Collections.Generic;
using System.Linq;

namespace Taga.DynamicServices.Client.Base
{
    public abstract class DynamicClientBase : IDynamicClient
    {
        private IEnumerable<IDynamicService> _services;

        //public abstract void Init();

        //public string WsdlUri { get; protected set; }

        public IEnumerable<IDynamicService> Services
        {
            get { return _services ?? (_services = LoadServices()); }
        }

        public IDynamicService GetService(string serviceName)
        {
            return Services.SingleOrDefault(s => s.Name == serviceName);
        }

        protected abstract IEnumerable<IDynamicService> LoadServices();
    }
}
