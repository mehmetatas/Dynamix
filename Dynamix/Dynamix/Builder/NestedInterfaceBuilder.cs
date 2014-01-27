using System.Reflection;
using Dynamix.Metadata;
using Dynamix.Utils;

namespace Dynamix.Builder
{
    class NestedInterfaceBuilder : InterfaceBuilderBase
    {
        private NestedInterface Interface
        {
            get { return InterfaceBase as NestedInterface; }
        }

        internal NestedInterfaceBuilder(NestedInterface dynamicInterface)
            : base(dynamicInterface)
        {

        }

        protected override System.Reflection.Emit.TypeBuilder DefineType()
        {
            return Interface.Parent.Builder.DefineNestedType(Interface);
        }

        internal override TypeAttributes TypeAttributes
        {
            get
            {
                return TypeAttributes.Interface | TypeAttributes.Abstract |
                       Interface.AccessModifier.GetNestedTypeAccessAttributes();
            }
        }
    }
}
