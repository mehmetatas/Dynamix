using System.Collections.Generic;

namespace Dynamix.Metadata
{
    public abstract class DelegateBase : GenericType
    {
        protected DelegateBase()
        {
            Parameters = new List<Parameter>();
        }

        public ReturnValue ReturnValue { get; set; }
        public IList<Parameter> Parameters { get; private set; }
    }
}
