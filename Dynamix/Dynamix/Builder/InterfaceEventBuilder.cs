using System;
using Dynamix.Metadata;

namespace Dynamix.Builder
{
    class InterfaceEventBuilder : IMemberBuilder
    {
        private readonly InterfaceEvent _dynamicInterfaceEvent;

        internal InterfaceEventBuilder(InterfaceEvent dynamicInterfaceEvent)
        {
            _dynamicInterfaceEvent = dynamicInterfaceEvent;
        }

        public void Build()
        {
            throw new NotImplementedException();
        }
    }
}
