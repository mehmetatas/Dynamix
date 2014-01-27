using Taga.DynamicServices.Client;
using Taga.DynamicServices.Routing;
using Taga.DynamicServices.Routing.Mapping;

namespace Taga.DynamicServices.Invocation
{
    public abstract class DynamicInvocationHandler : IDynamicInvocationHandler
    {
        public object Handle(IDynamicInvocationContext context)
        {
            context.RouteKey = GetRouteKey(context);

            var routeMapping = RouteMapping.GetRouteMapping();
            var route = routeMapping.GetRoute(context.RouteKey, context.ServiceName + "." + context.MethodName);

            var clientFactory = GetClientFactory(route.Mode);

            var client = clientFactory.GetClient(route.WsdlUri);

            var service = client.GetService(route.TargetServiceName);
            service.Address = route.Address;

            var method = service.GetMethod(route.TargetMethodName);

            var router = new DynamicCallRouter(route, method);
            return router.Call(context);
        }

        protected abstract string GetRouteKey(IDynamicInvocationContext context);
        protected abstract IDynamicClientFactory GetClientFactory(string mode);
    }
}
