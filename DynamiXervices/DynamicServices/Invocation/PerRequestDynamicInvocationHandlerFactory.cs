using System.Web;

namespace Taga.DynamicServices.Invocation
{
    class PerRequestDynamicInvocationHandlerFactory : DynamicInvocationHandlerFactory
    {
        private static IDynamicInvocationHandler Handler
        {
            get { return HttpContext.Current.Items["DynamicInvocationHandlerFactory"] as IDynamicInvocationHandler; }
            set { HttpContext.Current.Items["DynamicInvocationHandlerFactory"] = value; }
        }

        public override IDynamicInvocationHandler GetHandler()
        {
            return Handler ?? (Handler = base.GetHandler());
        }
    }
}
