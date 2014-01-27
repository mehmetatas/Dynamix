
using Dynamix.Builder;

namespace Dynamix.Metadata
{
    public abstract class MemberBase : NamedElement
    {
        internal abstract IMemberBuilder Builder { get; }
    }
}
