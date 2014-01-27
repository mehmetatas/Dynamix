using Dynamix.Builder;
using Dynamix.Utils;

namespace Dynamix.Metadata
{
    public class Interface : InterfaceBase, IType
    {
        public Interface()
        {
            AccessModifier = TypeAccessModifier.Internal;
            _child = new Child<Assembly, Interface>(this);
        }

        private readonly Child<Assembly, Interface> _child;
        public Assembly Parent
        {
            get { return _child.Parent; }
            set { _child.Parent = value; }
        }

        public TypeAccessModifier AccessModifier { get; set; }

        private TypeBuilder _builder;
        internal override TypeBuilder Builder
        {
            get { return _builder ?? (_builder = new InterfaceBuilder(this)); }
        }
    }
}
