using System;
using System.Linq;
using System.Reflection;
using Dynamix.Metadata;

namespace Dynamix.Builder
{
    abstract class DelegateBuilderBase : TypeBuilder
    {
        protected readonly DelegateBase DelegateBase;

        internal DelegateBuilderBase(DelegateBase dynamicDelegate)
        {
            DelegateBase = dynamicDelegate;
        }

        protected override void BuildMembers()
        {
            BuildConstructor();
            ImplementInvokeMethod();
        }

        private void BuildConstructor()
        {
            var constructorBuilder = Builder.DefineConstructor(
                MethodAttributes.RTSpecialName | MethodAttributes.HideBySig | MethodAttributes.Public,
                CallingConventions.Standard,
                new[] { typeof(object), typeof(IntPtr) });
            constructorBuilder.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);
        }

        private void ImplementInvokeMethod()
        {
            var paramTypes = DelegateBase.Parameters.Select(p => p.Type.ResolveType()).ToArray();
            var methodBuilder = Builder.DefineMethod(
                "Invoke",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual,
                DelegateBase.ReturnValue.ReturnType.ResolveType(), paramTypes);
            methodBuilder.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);
        }

        internal override Type BaseType
        {
            get { return StaticType.Delegate.Type; }
        }
    }
}
