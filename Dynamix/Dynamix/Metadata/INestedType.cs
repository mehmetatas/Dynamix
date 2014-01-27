
using Dynamix.Utils;

namespace Dynamix.Metadata
{
    public interface INestedType : IChild<Construct>
    {
        MemberAccessModifier AccessModifier { get; set; }
    }
}
