using System;
using Dynamix.Metadata;

namespace Dynamix.Builder
{
    class IndexerBuilder : IMemberBuilder
    {
        private readonly Indexer _dynamicIndexer;

        internal IndexerBuilder(Indexer dynamicIndexer)
        {
            _dynamicIndexer = dynamicIndexer;
        }

        public void Build()
        {
            throw new NotImplementedException("IndexerBuilder.Build");
        }
    }
}
