using System.Collections.Generic;

namespace Taga.DynamicServices.Client
{
    public interface IDynamicClient
    {
        IEnumerable<IDynamicService> Services { get; }
        IDynamicService GetService(string serviceName);
    }
}
