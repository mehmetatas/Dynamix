using System;
using System.Reflection;
using Dynamix.Metadata;

namespace Dynamix.Builder
{
    class InterfacePropertyBuilder : IMemberBuilder
    {
        private readonly InterfaceProperty _dynamicInterfaceProperty;

        internal InterfacePropertyBuilder(InterfaceProperty dynamicInterfaceProperty)
        {
            _dynamicInterfaceProperty = dynamicInterfaceProperty;
        }

        public void Build()
        {
            var typeBuilder = _dynamicInterfaceProperty.Parent.Builder.Builder;
            var propBuilder = typeBuilder.DefineProperty(_dynamicInterfaceProperty.Name, PropertyAttributes.HasDefault,
                                                         _dynamicInterfaceProperty.ReturnValue.ReturnType.ResolveType(),
                                                         Type.EmptyTypes);

            if (_dynamicInterfaceProperty.AllowGet)
            {
                var getter = typeBuilder.DefineMethod("get_" + _dynamicInterfaceProperty.Name,
                                         MethodAttributes.Public | MethodAttributes.Abstract | MethodAttributes.Virtual,
                                         CallingConventions.HasThis,
                                         _dynamicInterfaceProperty.ReturnValue.ReturnType.ResolveType(), null);

                propBuilder.SetGetMethod(getter);
            }

            if (_dynamicInterfaceProperty.AllowSet)
            {
                var setter = typeBuilder.DefineMethod("set_" + _dynamicInterfaceProperty.Name,
                                         MethodAttributes.Public | MethodAttributes.Abstract | MethodAttributes.Virtual,
                                         CallingConventions.HasThis,
                                         null, new[] { _dynamicInterfaceProperty.ReturnValue.ReturnType.ResolveType() });

                propBuilder.SetSetMethod(setter);
            }
        }
    }
}
