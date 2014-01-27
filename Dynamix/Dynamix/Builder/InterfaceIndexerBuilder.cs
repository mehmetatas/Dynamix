using System;
using Dynamix.Metadata;

namespace Dynamix.Builder
{
    class InterfaceIndexerBuilder : IMemberBuilder
    {
        private readonly InterfaceIndexer _dynamicInterfaceIndexer;

        internal InterfaceIndexerBuilder(InterfaceIndexer dynamicInterfaceIndexer)
        {
            _dynamicInterfaceIndexer = dynamicInterfaceIndexer;
        }

        public void Build()
        {
            throw new NotImplementedException();
        }
    }
}
