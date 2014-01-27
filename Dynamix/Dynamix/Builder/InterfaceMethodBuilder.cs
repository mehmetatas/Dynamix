using System;
using System.Linq;
using System.Reflection;
using Dynamix.Metadata;

namespace Dynamix.Builder
{
    internal class InterfaceMethodBuilder : IMemberBuilder
    {
        private readonly InterfaceMethod _dynamicInterfaceMethod;

        internal InterfaceMethodBuilder(InterfaceMethod dynamicInterfaceMethod)
        {
            _dynamicInterfaceMethod = dynamicInterfaceMethod;
        }

        public void Build()
        {
            var typeBuilder = _dynamicInterfaceMethod.Parent.Builder.Builder;
            typeBuilder.DefineMethod(_dynamicInterfaceMethod.Name, Attributes, CallingConventions,
                                     _dynamicInterfaceMethod.ReturnValue.ReturnType.ResolveType(),
                                     ParameterTypes);
        }

        private MethodAttributes Attributes
        {
            get
            {
                return MethodAttributes.Public |
                       MethodAttributes.Abstract |
                       MethodAttributes.Virtual |
                       MethodAttributes.HideBySig;
            }
        }

        private Type[] ParameterTypes
        {
            get { return _dynamicInterfaceMethod.Parameters.Select(t => t.Type.ResolveType()).ToArray(); }
        }

        private CallingConventions CallingConventions
        {
            get { return CallingConventions.HasThis; }
        }
    }
}
