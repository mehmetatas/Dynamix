using Taga.DynamicServices.Invocation;

namespace Taga.DynamicServices.Routing
{
    public interface IDynamicCallRouter
    {
        object Call(IDynamicInvocationContext context);
    }
}
