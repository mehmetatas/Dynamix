using System;
using Dynamix.Builder;

namespace Dynamix.Metadata
{
    public abstract class TypeBase : NamedElement, ITypeInfo
    {
        public string Namespace { get; set; }

        public virtual string FullName
        {
            get
            {
                return String.IsNullOrWhiteSpace(Namespace)
                           ? Name
                           : (Namespace + "." + Name);
            }
        }

        internal abstract TypeBuilder Builder { get; }

        public Type ResolveType()
        {
            return Builder.Type;
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}
