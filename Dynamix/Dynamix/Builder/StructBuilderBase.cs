using System;
using Dynamix.Metadata;

namespace Dynamix.Builder
{
    abstract class StructBuilderBase : ConstructBuilderBase
    {
        protected StructBase StructBase
        {
            get { return Construct as StructBase; }
        }

        internal StructBuilderBase(StructBase dynamicStruct)
            : base(dynamicStruct)
        {
        }

        internal override Type BaseType
        {
            get { return StaticType.Struct.Type; }
        }
    }
}
