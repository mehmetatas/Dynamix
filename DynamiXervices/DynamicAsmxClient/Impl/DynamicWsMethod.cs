using System.Reflection;
using Taga.DynamicServices.Client;
using Taga.DynamicServices.Client.Base;

namespace Taga.DynamicServices.AsmxClient.Impl
{
    class DynamicWsMethod : DynamicMethodBase
    {
        internal DynamicWsMethod(MethodInfo methodInfo, object serviceInstance)
            : base(methodInfo, serviceInstance)
        {
        }

        protected override IDynamicParameter CreateParameter(ParameterInfo pi)
        {
            return new DynamicWsParameter(pi);
        }
    }
}
