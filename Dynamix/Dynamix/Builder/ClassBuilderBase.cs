using System;
using Dynamix.Metadata;

namespace Dynamix.Builder
{
    abstract class ClassBuilderBase : ConstructBuilderBase
    {
        protected ClassBase ClassBase
        {
            get { return Construct as ClassBase; }
        }

        protected ClassBuilderBase(ClassBase dynamicClassBase)
            : base(dynamicClassBase)
        {
        }

        protected override void BuildMembers()
        {
            if (ClassBase.BaseType is TypeBase)
                (ClassBase.BaseType as TypeBase).Builder.CreateType();

            base.BuildMembers();
        }

        internal override Type BaseType
        {
            get { return ClassBase.BaseType.ResolveType(); }
        }
    }
}
