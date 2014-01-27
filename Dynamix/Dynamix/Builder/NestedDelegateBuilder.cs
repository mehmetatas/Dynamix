using System.Reflection;
using Emit = System.Reflection.Emit;
using Dynamix.Metadata;
using Dynamix.Utils;

namespace Dynamix.Builder
{
    class NestedDelegateBuilder : DelegateBuilderBase
    {
        private NestedDelegate Delegate
        {
            get { return DelegateBase as NestedDelegate; }
        }

        internal NestedDelegateBuilder(NestedDelegate dynamicDelegate)
            : base(dynamicDelegate)
        {

        }

        protected override Emit.TypeBuilder DefineType()
        {
            return Delegate.Parent.Builder.DefineNestedType(Delegate);
        }

        internal override TypeAttributes TypeAttributes
        {
            get
            {
                return TypeAttributes.Class | TypeAttributes.Sealed |
                       Delegate.AccessModifier.GetNestedTypeAccessAttributes();
            }
        }
    }
}
