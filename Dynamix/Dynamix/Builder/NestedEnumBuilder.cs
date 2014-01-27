using System.Reflection;
using Emit = System.Reflection.Emit;
using Dynamix.Metadata;
using Dynamix.Utils;

namespace Dynamix.Builder
{
    class NestedEnumBuilder : EnumBuilderBase
    {
        private NestedEnum Enum
        {
            get { return EnumBase as NestedEnum; }
        }

        internal NestedEnumBuilder(NestedEnum dynamicEnum)
            : base(dynamicEnum)
        {
        }

        protected override Emit.TypeBuilder DefineType()
        {
            return Enum.Parent.Builder.DefineNestedType(Enum);
        }

        internal override TypeAttributes TypeAttributes
        {
            get
            {
                return TypeAttributes.Sealed |
                       Enum.AccessModifier.GetNestedTypeAccessAttributes();
            }
        }
    }
}
