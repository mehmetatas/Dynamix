using System;
using System.Collections.Generic;

namespace Dynamix.Metadata
{
    public abstract class EnumBase : TypeBase
    {
        protected EnumBase()
        {
            BaseType = EnumBaseType.Int;
            Values = new List<EnumValue>();
        }

        public EnumBaseType BaseType { get; set; }
        public IList<EnumValue> Values { get; private set; }

        internal Type UnderlyingType
        {
            get
            {
                switch (BaseType)
                {
                    case EnumBaseType.Byte:
                        return StaticType.Byte.Type;
                    case EnumBaseType.Long:
                        return StaticType.Long.Type;
                    case EnumBaseType.SByte:
                        return StaticType.SByte.Type;
                    case EnumBaseType.Short:
                        return StaticType.Short.Type;
                    case EnumBaseType.UInt:
                        return StaticType.UInt.Type;
                    case EnumBaseType.ULong:
                        return StaticType.ULong.Type;
                    case EnumBaseType.UShort:
                        return StaticType.UShort.Type;
                    default:
                        return StaticType.Int.Type;
                }
            }
        }
    }
}
