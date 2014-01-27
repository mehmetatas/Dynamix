using Dynamix.Builder;
using Dynamix.Utils;

namespace Dynamix.Metadata
{
    public class NestedEnum : EnumBase, INestedType
    {
        public NestedEnum()
        {
            AccessModifier = MemberAccessModifier.Private;
            _child = new Child<Construct, NestedEnum>(this);
        }

        private readonly Child<Construct, NestedEnum> _child;
        public Construct Parent
        {
            get { return _child.Parent; }
            set { _child.Parent = value; }
        }

        public MemberAccessModifier AccessModifier { get; set; }

        private TypeBuilder _builder;
        internal override TypeBuilder Builder
        {
            get { return _builder ?? (_builder = new NestedEnumBuilder(this)); }
        }
    }
}
