using System.Reflection;
using Emit = System.Reflection.Emit;
using Dynamix.Metadata;
using Dynamix.Utils;

namespace Dynamix.Builder
{
    class StructBuilder : StructBuilderBase
    {
        private Struct Struct
        {
            get { return StructBase as Struct; }
        }

        internal StructBuilder(Struct dynamicStruct)
            : base(dynamicStruct)
        {
        }

        protected override Emit.TypeBuilder DefineType()
        {
            return Struct.Parent.Builder.DefineType(Struct);
        }

        internal override TypeAttributes TypeAttributes
        {
            get
            {
                return TypeAttributes.Sealed |
                       Struct.AccessModifier.GetTypeAccessAttributes();
            }
        }
    }
}
