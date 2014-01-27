using System.Collections.Generic;

namespace Dynamix.Metadata
{
    public abstract class ImplementorBase : GenericType
    {
        protected  ImplementorBase()
        {
            Interfaces = new List<ITypeInfo>();
        }

        public IList<ITypeInfo> Interfaces { get; private set; }
    }
}
