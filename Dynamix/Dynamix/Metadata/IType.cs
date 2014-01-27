
using Dynamix.Utils;

namespace Dynamix.Metadata
{
    public interface IType : IChild<Assembly>
    {
        TypeAccessModifier AccessModifier { get; set; }
    }
}
