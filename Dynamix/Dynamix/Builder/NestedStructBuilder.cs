using System.Reflection;
using Emit = System.Reflection.Emit;
using Dynamix.Metadata;
using Dynamix.Utils;

namespace Dynamix.Builder
{
    class NestedStructBuilder : StructBuilderBase
    {
        private NestedStruct Struct
        {
            get { return StructBase as NestedStruct; }
        }

        internal NestedStructBuilder(NestedStruct dynamicStruct)
            : base(dynamicStruct)
        {
        }

        protected override Emit.TypeBuilder DefineType()
        {
            return Struct.Parent.Builder.DefineNestedType(Struct);
        }

        internal override TypeAttributes TypeAttributes
        {
            get
            {
                return TypeAttributes.Sealed |
                       Struct.AccessModifier.GetNestedTypeAccessAttributes();
            }
        }
    }
}
