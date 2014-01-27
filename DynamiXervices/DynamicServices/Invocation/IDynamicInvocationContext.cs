using System;

namespace Taga.DynamicServices.Invocation
{
    public interface IDynamicInvocationContext
    {
        Parameter[] InputParameters { get; }

        Type ReturnType { get; }
        string MethodName { get; }
        string ServiceName { get; }
        string RouteKey { get; set; }
    }
}
