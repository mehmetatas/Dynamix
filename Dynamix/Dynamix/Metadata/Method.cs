using System.Collections.Generic;
using Dynamix.Builder;
using Dynamix.Utils;

namespace Dynamix.Metadata
{
    public class Method : PolymorphicMethod
    {
        public Method()
        {
            Attribute = PolymorphicMemberAttribute.Default;
            Parameters = new List<Parameter>();
            Body = new MethodBody(this);
            _child = new Child<Construct, Method>(this);
        }

        private readonly Child<Construct, Method> _child;
        public override Construct Parent
        {
            get { return _child.Parent; }
            set { _child.Parent = value; }
        }

        public ReturnValue ReturnValue { get; set; }
        public MethodBody Body { get; private set; }
        public IList<Parameter> Parameters { get; private set; }

        public IList<GenericTypeParameter> GenericParameters { get; private set; }

        private IMemberBuilder _builder;
        internal override IMemberBuilder Builder
        {
            get { return _builder ?? (_builder = new MethodBuilder(this)); }
        }
    }
}
