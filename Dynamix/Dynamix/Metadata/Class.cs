using Dynamix.Builder;
using Dynamix.Utils;

namespace Dynamix.Metadata
{
    public class Class : ClassBase, IType
    {
        public Class()
        {
            AccessModifier = TypeAccessModifier.Internal;
            _child = new Child<Assembly, Class>(this);
        }

        private readonly Child<Assembly, Class> _child;
        public Assembly Parent
        {
            get { return _child.Parent; }
            set { _child.Parent = value; }
        }

        private TypeBuilder _builder;
        internal override TypeBuilder Builder
        {
            get { return _builder ?? (_builder = new ClassBuilder(this)); }
        }

        public TypeAccessModifier AccessModifier { get; set; }
    }
}
