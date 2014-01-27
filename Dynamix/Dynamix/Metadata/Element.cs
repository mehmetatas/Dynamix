using System.Collections.Generic;

namespace Dynamix.Metadata
{
    public abstract class Element
    {
        protected Element()
        {
            CustomAttributes = new List<ITypeInfo>();
        }

        public IList<ITypeInfo> CustomAttributes { get; private set; }
    }
}
