using Taga.DynamicServices.Invocation;
using Taga.DynamicServices.Routing;
using Taga.DynamicServices.Routing.Mapping;

namespace TestConsole.Routing
{
    class MockDynamicInvocationHandler : IDynamicInvocationHandler
    {
        public object Handle(IDynamicInvocationContext context)
        {
            var routeMapping = RouteMapping.GetRouteMapping();
            var route = routeMapping.GetRoute(context.RouteKey, context.ServiceName + "." + context.MethodName);

            var client = new MockDynamicClient(typeof(TargetService));
            var service = client.GetService(route.TargetServiceName);
            var method = service.GetMethod(route.TargetMethodName);

            var router = new DynamicCallRouter(route, method);
            return router.Call(context);
        }
    }
}
