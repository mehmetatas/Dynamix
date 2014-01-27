
namespace Taga.DynamicServices.Invocation
{
    public interface IDynamicInvocationHandler
    {
        object Handle(IDynamicInvocationContext context);
    }
}
