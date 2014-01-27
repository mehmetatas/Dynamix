using Dynamix.Builder;
using Dynamix.Utils;

namespace Dynamix.Metadata
{
    public class NestedDelegate : DelegateBase, INestedType
    {
        public NestedDelegate()
        {
            AccessModifier = MemberAccessModifier.Private;
            _child = new Child<Construct, NestedDelegate>(this);
        }

        private readonly Child<Construct, NestedDelegate> _child;
        public Construct Parent
        {
            get { return _child.Parent; }
            set { _child.Parent = value; }
        }

        public MemberAccessModifier AccessModifier { get; set; }

        private TypeBuilder _builder;
        internal override TypeBuilder Builder
        {
            get { return _builder ?? (_builder = new NestedDelegateBuilder(this)); }
        }
    }
}
