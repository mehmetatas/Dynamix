using Dynamix.Builder;
using Dynamix.Utils;

namespace Dynamix.Metadata
{
    public class Delegate : DelegateBase, IType
    {
        public Delegate()
        {
            AccessModifier = TypeAccessModifier.Internal;
            _child = new Child<Assembly, Delegate>(this);
        }

        private readonly Child<Assembly, Delegate> _child;
        public Assembly Parent
        {
            get { return _child.Parent; }
            set { _child.Parent = value; }
        }

        private TypeBuilder _builder;
        internal override TypeBuilder Builder
        {
            get { return _builder ?? (_builder = new DelegateBuilder(this)); }
        }

        public TypeAccessModifier AccessModifier { get; set; }
    }
}
