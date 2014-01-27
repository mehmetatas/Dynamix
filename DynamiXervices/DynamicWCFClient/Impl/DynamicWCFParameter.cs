using System.Reflection;
using Taga.DynamicServices.Client.Base;

namespace Taga.DynamicServices.WCFClient.Impl
{
    internal class DynamicWCFParameter : DynamicParameterBase
    {
        internal DynamicWCFParameter(ParameterInfo parameterInfo)
            : base(parameterInfo)
        {
        }
    }
}
