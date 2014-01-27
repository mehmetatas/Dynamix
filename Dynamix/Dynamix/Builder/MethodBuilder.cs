using System;
using System.Linq;
using System.Reflection;
using Dynamix.Metadata;
using Dynamix.Utils;
using DynMethod = Dynamix.Metadata.Method;
using Emit = System.Reflection.Emit;

namespace Dynamix.Builder
{
    class MethodBuilder : IMemberBuilder
    {
        private readonly DynMethod _dynamicMethod;

        internal Emit.MethodBuilder Builder { get; private set; }

        internal MethodBuilder(DynMethod dynamicMethod)
        {
            _dynamicMethod = dynamicMethod;
        }

        public void Build()
        {
            var typeBuilder = _dynamicMethod.Parent.Builder.Builder;
            Builder = typeBuilder.DefineMethod(_dynamicMethod.Name, Attributes, CallingConventions,
                                                         _dynamicMethod.ReturnValue.ReturnType.ResolveType(),
                                                         ParameterTypes);
            BuildBody();
        }

        private void BuildBody()
        {
            var il = Builder.GetILGenerator();
            _dynamicMethod.Body.Builder.Build(il);
        }

        private MethodAttributes Attributes
        {
            get
            {
                return MethodAttributes.HideBySig |
                       _dynamicMethod.Attribute.GetMethodAttributes() |
                       _dynamicMethod.AccessModifier.GetMethodAccessAttributes();
            }
        }

        private Type[] ParameterTypes
        {
            get
            {
                return _dynamicMethod.Parameters.Select(t => t.Type.ResolveType()).ToArray();
            }
        }

        private CallingConventions CallingConventions
        {
            get
            {
                return _dynamicMethod.Attribute == PolymorphicMemberAttribute.Static
                    ? CallingConventions.Standard
                    : CallingConventions.HasThis;
            }
        }
    }
}
