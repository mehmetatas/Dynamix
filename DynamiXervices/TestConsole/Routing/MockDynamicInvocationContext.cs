using System;
using Taga.DynamicServices;
using Taga.DynamicServices.Invocation;

namespace TestConsole.Routing
{
    class MockDynamicInvocationContext : IDynamicInvocationContext
    {
        public Parameter[] InputParameters { get; set; }

        public Type ReturnType { get; set; }
        public string MethodName { get; set; }
        public string ServiceName { get; set; }
        public string RouteKey { get; set; }
    }
}
