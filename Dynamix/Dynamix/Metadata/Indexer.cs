using System.Collections.Generic;
using Dynamix.Builder;
using Dynamix.Utils;

namespace Dynamix.Metadata
{
    public class Indexer : PropertyBase
    {
        public Indexer()
        {
            Parameters = new List<Parameter>();
            _child = new Child<Construct, Indexer>(this);
        }

        private readonly Child<Construct, Indexer> _child;
        public override Construct Parent
        {
            get { return _child.Parent; }
            set { _child.Parent = value; }
        }

        private IMemberBuilder _builder;
        internal override IMemberBuilder Builder
        {
            get { return _builder ?? (_builder = new IndexerBuilder(this)); }
        }

        public IList<Parameter> Parameters { get; private set; }
    }
}
