using System.Reflection;
using Dynamix.Metadata;
using Dynamix.Utils;
using Emit = System.Reflection.Emit;

namespace Dynamix.Builder
{
    class DelegateBuilder : DelegateBuilderBase
    {
        private Delegate Delegate
        {
            get { return DelegateBase as Delegate; }
        }

        internal DelegateBuilder(Delegate dynamicDelegate)
            : base(dynamicDelegate)
        {

        }

        protected override Emit.TypeBuilder DefineType()
        {
            return Delegate.Parent.Builder.DefineType(Delegate);
        }

        internal override TypeAttributes TypeAttributes
        {
            get
            {
                return TypeAttributes.Class | TypeAttributes.Sealed |
                       Delegate.AccessModifier.GetTypeAccessAttributes();
            }
        }
    }
}
