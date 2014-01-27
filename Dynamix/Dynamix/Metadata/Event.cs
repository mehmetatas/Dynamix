using Dynamix.Builder;
using Dynamix.Utils;

namespace Dynamix.Metadata
{
    public class Event : PolymorphicMethod
    {
        public Event()
        {
            Attribute = PolymorphicMemberAttribute.Default;
            AdderBody = new MethodBody(this);
            RemoverBody = new MethodBody(this);
            _child = new Child<Construct, Event>(this);
        }

        private readonly Child<Construct, Event> _child;
        public override Construct Parent
        {
            get { return _child.Parent; }
            set { _child.Parent = value; }
        }

        public ITypeInfo DelegateType { get; set; }

        private IMemberBuilder _builder;
        internal override IMemberBuilder Builder
        {
            get { return _builder ?? (_builder = new EventBuilder(this)); }
        }

        public MethodBody AdderBody { get; private set; }
        public MethodBody RemoverBody { get; private set; }
    }
}
