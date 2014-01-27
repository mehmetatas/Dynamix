using System.Collections.Generic;

namespace Dynamix.Metadata
{
    public abstract class GenericType : TypeBase
    {
        protected GenericType()
        {
            GenericParameters = new List<GenericTypeParameter>();
        }

        public IList<GenericTypeParameter> GenericParameters { get; private set; }
    }
}
