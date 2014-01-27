
using Dynamix.Utils;

namespace Dynamix.Metadata
{
    public abstract class ConstructMember : MemberBase, IChild<Construct>
    {
        protected ConstructMember()
        {
            AccessModifier = MemberAccessModifier.Private;
        }

        public abstract Construct Parent { get; set; }

        public MemberAccessModifier AccessModifier { get; set; }

        public override string ToString()
        {
            return Parent + "." + Name;
        }
    }
}
