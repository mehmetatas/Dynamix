using System.Reflection;
using Taga.DynamicServices.Client.Base;

namespace Taga.DynamicServices.AsmxClient.Impl
{
    class DynamicWsParameter : DynamicParameterBase
    {
        internal DynamicWsParameter(ParameterInfo parameterInfo)
            : base(parameterInfo)
        {
        }
    }
}
