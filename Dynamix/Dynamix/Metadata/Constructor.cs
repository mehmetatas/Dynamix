using System.Collections.Generic;
using Dynamix.Builder;
using Dynamix.Utils;

namespace Dynamix.Metadata
{
    public class Constructor : MemberWithBody
    {
        public Constructor()
        {
            Attribute = ConstructorAttribute.Default;
            AccessModifier = MemberAccessModifier.Private;
            Parameters = new List<Parameter>();
            Body = new MethodBody(this);
            _child = new Child<Construct, Constructor>(this);
        }

        private readonly Child<Construct, Constructor> _child;
        public override Construct Parent
        {
            get { return _child.Parent; }
            set { _child.Parent = value; }
        }

        public ConstructorAttribute Attribute { get; set; }
        public MethodBody Body { get; private set; }
        public IList<Parameter> Parameters { get; private set; }

        private IMemberBuilder _builder;
        internal override IMemberBuilder Builder
        {
            get { return _builder ?? (_builder = new ConstructorBuilder(this)); }
        }
    }
}
