using System.Collections.Generic;
using Dynamix.Builder;
using Dynamix.Utils;

namespace Dynamix.Metadata
{
    public class InterfaceMethod : InterfaceMember
    {
        public InterfaceMethod()
        {
            Parameters = new List<Parameter>();
            _child = new Child<InterfaceBase, InterfaceMethod>(this);
        }

        private readonly Child<InterfaceBase, InterfaceMethod> _child;
        public override InterfaceBase Parent
        {
            get { return _child.Parent; }
            set { _child.Parent = value; }
        }

        public ReturnValue ReturnValue { get; set; }
        public IList<Parameter> Parameters { get; private set; }

        public IList<GenericTypeParameter> GenericParameters { get; private set; }

        private IMemberBuilder _builder;
        internal override IMemberBuilder Builder
        {
            get { return _builder ?? (_builder = new InterfaceMethodBuilder(this)); }
        }
    }
}
