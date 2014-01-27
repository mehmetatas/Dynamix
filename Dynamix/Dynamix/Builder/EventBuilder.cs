using System;
using Dynamix.Metadata;

namespace Dynamix.Builder
{
    class EventBuilder : IMemberBuilder
    {
        private readonly Event _dynamicEvent;

        internal EventBuilder(Event dynamicEvent)
        {
            _dynamicEvent = dynamicEvent;
        }

        public void Build()
        {
            throw new NotImplementedException("EventBuilder.Build");
        }
    }
}
