using System.Reflection;
using Dynamix.Metadata;
using Dynamix.Utils;
using Emit = System.Reflection.Emit;

namespace Dynamix.Builder
{
    class EnumBuilder : EnumBuilderBase
    {
        private Enum Enum
        {
            get { return EnumBase as Enum; }
        }

        internal EnumBuilder(Enum dynamicEnum)
            : base(dynamicEnum)
        {
        }

        protected override Emit.TypeBuilder DefineType()
        {
            return Enum.Parent.Builder.DefineType(Enum);
        }

        internal override TypeAttributes TypeAttributes
        {
            get
            {
                return TypeAttributes.Sealed |
                       Enum.AccessModifier.GetTypeAccessAttributes();
            }
        }
    }
}
