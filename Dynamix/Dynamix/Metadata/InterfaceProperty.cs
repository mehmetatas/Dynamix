using Dynamix.Builder;
using Dynamix.Utils;

namespace Dynamix.Metadata
{
    public class InterfaceProperty : InterfacePropertyBase
    {
        public InterfaceProperty()
        {
            _child = new Child<InterfaceBase, InterfaceProperty>(this);
        }

        private readonly Child<InterfaceBase, InterfaceProperty> _child;
        public override InterfaceBase Parent
        {
            get { return _child.Parent; }
            set { _child.Parent = value; }
        }

        private IMemberBuilder _builder;
        internal override IMemberBuilder Builder
        {
            get { return _builder ?? (_builder = new InterfacePropertyBuilder(this)); }
        }
    }
}
