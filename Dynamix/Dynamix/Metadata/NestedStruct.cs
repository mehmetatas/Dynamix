using Dynamix.Builder;
using Dynamix.Utils;

namespace Dynamix.Metadata
{
    public class NestedStruct : StructBase, INestedType
    {
        public NestedStruct()
        {
            AccessModifier = MemberAccessModifier.Private;
            _child = new Child<Construct, NestedStruct>(this);
        }

        private readonly Child<Construct, NestedStruct> _child;
        public Construct Parent
        {
            get { return _child.Parent; }
            set { _child.Parent = value; }
        }

        public MemberAccessModifier AccessModifier { get; set; }

        private TypeBuilder _builder;
        internal override TypeBuilder Builder
        {
            get { return _builder ?? (_builder = new NestedStructBuilder(this)); }
        }
    }
}
