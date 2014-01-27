using Dynamix.Builder;
using Dynamix.Utils;

namespace Dynamix.Metadata
{
    public class InterfaceEvent : InterfaceMember
    {
        public InterfaceEvent()
        {
            _child = new Child<InterfaceBase, InterfaceEvent>(this);
        }

        private readonly Child<InterfaceBase, InterfaceEvent> _child;
        public override InterfaceBase Parent
        {
            get { return _child.Parent; }
            set { _child.Parent = value; }
        }

        public ITypeInfo DelegateType { get; set; }

        private IMemberBuilder _builder;
        internal override IMemberBuilder Builder
        {
            get { return _builder ?? (_builder = new InterfaceEventBuilder(this)); }
        }
    }
}
