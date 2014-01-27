using Dynamix.Builder;
using Dynamix.Utils;

namespace Dynamix.Metadata
{
    public class Property : PropertyBase
    {
        public Property()
        {
            Getter = new PropertyGetAccessor(this);
            Setter = new PropertySetAccessor(this);
            _child = new Child<Construct, Property>(this);
        }

        private readonly Child<Construct, Property> _child;
        public override Construct Parent
        {
            get { return _child.Parent; }
            set { _child.Parent = value; }
        }

        public bool IsDefault
        {
            get { return Getter.Body.IsEmpty && Setter.Body.IsEmpty; }
        }

        private IMemberBuilder _builder;
        internal override IMemberBuilder Builder
        {
            get { return _builder ?? (_builder = new PropertyBuilder(this)); }
        }

        public PropertyGetAccessor Getter { get; private set; }
        public PropertySetAccessor Setter { get; private set; }
    }
}
