using System.Collections.Generic;
using Dynamix.Builder;
using Dynamix.Utils;

namespace Dynamix.Metadata
{
    public class InterfaceIndexer : InterfacePropertyBase
    {
        public InterfaceIndexer()
        {
            Parameters = new List<Parameter>();
            _child = new Child<InterfaceBase, InterfaceIndexer>(this);
        }

        private readonly Child<InterfaceBase, InterfaceIndexer> _child;
        public override InterfaceBase Parent
        {
            get { return _child.Parent; }
            set { _child.Parent = value; }
        }

        public IList<Parameter> Parameters { get; private set; }

        private IMemberBuilder _builder;
        internal override IMemberBuilder Builder
        {
            get { return _builder ?? (_builder = new InterfaceIndexerBuilder(this)); }
        }
    }
}
