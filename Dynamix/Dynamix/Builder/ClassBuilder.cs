using System.Reflection;
using Dynamix.Metadata;
using Dynamix.Utils;
using Emit = System.Reflection.Emit;

namespace Dynamix.Builder
{
    class ClassBuilder : ClassBuilderBase
    {
        private Class Class
        {
            get { return ClassBase as Class; }
        }

        internal ClassBuilder(Class dynamicClass)
            : base(dynamicClass)
        {
        }

        protected override Emit.TypeBuilder DefineType()
        {
            return Class.Parent.Builder.DefineType(Class);
        }

        internal override TypeAttributes TypeAttributes
        {
            get
            {
                return Class.InheritanceModifier.GetInheritanceAttributes() |
                       Class.AccessModifier.GetTypeAccessAttributes();
            }
        }
    }
}
