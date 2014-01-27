using Dynamix.Builder;
using Dynamix.Utils;

namespace Dynamix.Metadata
{
    public class Struct : StructBase, IType
    {
        public Struct()
        {
            AccessModifier = TypeAccessModifier.Internal;
            _child = new Child<Assembly, Struct>(this);
        }

        private readonly Child<Assembly, Struct> _child;
        public Assembly Parent
        {
            get { return _child.Parent; }
            set { _child.Parent = value; }
        }

        public TypeAccessModifier AccessModifier { get; set; }

        private TypeBuilder _builder;
        internal override TypeBuilder Builder
        {
            get { return _builder ?? (_builder = new StructBuilder(this)); }
        }
    }
}
