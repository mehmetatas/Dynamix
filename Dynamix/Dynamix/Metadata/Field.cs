
using Dynamix.Builder;
using Dynamix.Utils;

namespace Dynamix.Metadata
{
    public class Field : ConstructMember
    {
        public Field()
        {
            IsStatic = false;
            Attribute = FieldAttribute.Default;
            _child = new Child<Construct, Field>(this);
        }

        private readonly Child<Construct, Field> _child;
        public override Construct Parent
        {
            get { return _child.Parent; }
            set { _child.Parent = value; }
        }

        private IMemberBuilder _builder;
        internal override IMemberBuilder Builder
        {
            get { return _builder ?? (_builder = new FieldBuilder(this)); }
        }

        public ITypeInfo Type { get; set; }
        public bool IsStatic { get; set; }
        public FieldAttribute Attribute { get; set; }
        public object InitialValue { get; set; }
    }
}
