using System.Reflection;
using Taga.DynamicServices.Client;
using Taga.DynamicServices.Client.Base;

namespace Taga.DynamicServices.WCFClient.Impl
{
    class DynamicWCFMethod : DynamicMethodBase
    {
        internal DynamicWCFMethod(MethodInfo methodInfo, object serviceInstance)
            : base(methodInfo, serviceInstance)
        {
        }

        protected override IDynamicParameter CreateParameter(ParameterInfo pi)
        {
            return new DynamicWCFParameter(pi);
        }
    }
}
