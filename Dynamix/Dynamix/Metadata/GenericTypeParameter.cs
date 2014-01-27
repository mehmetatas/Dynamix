using System;
using System.Collections.Generic;

namespace Dynamix.Metadata
{
    public class GenericTypeParameter : NamedElement, ITypeInfo
    {
        public GenericTypeParameter(string name)
        {
            Name = name;
            InterfaceConstraints = new List<ITypeInfo>();
        }

        public IList<ITypeInfo> InterfaceConstraints { get; private set; }
        public ITypeInfo BaseTypeConstraint { get; set; }

        public Type ResolveType()
        {
            throw new System.NotImplementedException();
        }
    }
}
