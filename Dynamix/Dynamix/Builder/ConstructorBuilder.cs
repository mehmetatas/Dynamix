using System;
using Dynamix.Metadata;

namespace Dynamix.Builder
{
    internal class ConstructorBuilder : IMemberBuilder
    {
        private readonly Constructor _dynamicConstructor;

        internal ConstructorBuilder(Constructor dynamicConstructor)
        {
            _dynamicConstructor = dynamicConstructor;
        }

        public void Build()
        {
            throw new NotImplementedException();
        }
    }
}
