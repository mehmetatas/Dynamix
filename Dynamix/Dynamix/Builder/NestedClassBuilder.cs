using System.Reflection;
using Dynamix.Metadata;
using Dynamix.Utils;

namespace Dynamix.Builder
{
    class NestedClassBuilder : ClassBuilderBase
    {
        private NestedClass Class
        {
            get { return ClassBase as NestedClass; }
        }

        internal NestedClassBuilder(NestedClass dynamicClass)
            : base(dynamicClass)
        {
        }

        protected override System.Reflection.Emit.TypeBuilder DefineType()
        {
            return Class.Parent.Builder.DefineNestedType(Class);
        }

        internal override TypeAttributes TypeAttributes
        {
            get
            {
                return Class.InheritanceModifier.GetInheritanceAttributes() |
                       Class.AccessModifier.GetNestedTypeAccessAttributes();
            }
        }
    }
}