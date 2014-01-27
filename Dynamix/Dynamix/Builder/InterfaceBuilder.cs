using System.Reflection;
using Emit = System.Reflection.Emit;
using Dynamix.Metadata;
using Dynamix.Utils;

namespace Dynamix.Builder
{
    class InterfaceBuilder : InterfaceBuilderBase
    {
        private Interface Interface
        {
            get { return InterfaceBase as Interface; }
        }

        internal InterfaceBuilder(Interface dynamicInterface)
            : base(dynamicInterface)
        {

        }

        protected override Emit.TypeBuilder DefineType()
        {
            return Interface.Parent.Builder.DefineType(Interface);
        }

        internal override TypeAttributes TypeAttributes
        {
            get
            {
                return TypeAttributes.Interface | TypeAttributes.Abstract |
                       Interface.AccessModifier.GetTypeAccessAttributes();
            }
        }
    }
}
