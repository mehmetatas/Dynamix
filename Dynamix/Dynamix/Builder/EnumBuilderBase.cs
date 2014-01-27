using System;
using System.Reflection;
using Dynamix.Metadata;

namespace Dynamix.Builder
{
    abstract class EnumBuilderBase : TypeBuilder
    {
        protected readonly EnumBase EnumBase;

        internal EnumBuilderBase(EnumBase dynamicEnum)
        {
            EnumBase = dynamicEnum;
        }

        protected override void BuildMembers()
        {
            DefineEnumValues();
        }

        private void DefineEnumValues()
        {
            Builder.DefineField("value__", EnumBase.UnderlyingType, FieldAttributes.Private | FieldAttributes.SpecialName);
            foreach (var dynamicEnumValue in EnumBase.Values)
            {
                var fb = Builder.DefineField(dynamicEnumValue.Name, EnumBase.UnderlyingType,
                    FieldAttributes.Public | FieldAttributes.Literal | FieldAttributes.Static);
                fb.SetConstant(GetTypedValue(dynamicEnumValue));
            }
        }

        internal override Type BaseType
        {
            get { return StaticType.Enum.Type; }
        }

        private object GetTypedValue(EnumValue dynamicEnumValue)
        {
            switch (EnumBase.BaseType)
            {
                case EnumBaseType.ULong:
                    return Convert.ToUInt64(dynamicEnumValue.Value);
                case EnumBaseType.Byte:
                    return Convert.ToByte(dynamicEnumValue.Value);
                case EnumBaseType.Long:
                    return Convert.ToInt64(dynamicEnumValue.Value);
                case EnumBaseType.SByte:
                    return Convert.ToSByte(dynamicEnumValue.Value);
                case EnumBaseType.Short:
                    return Convert.ToInt16(dynamicEnumValue.Value);
                case EnumBaseType.UInt:
                    return Convert.ToUInt32(dynamicEnumValue.Value);
                case EnumBaseType.UShort:
                    return Convert.ToUInt16(dynamicEnumValue.Value);
                default:
                    return Convert.ToInt32(dynamicEnumValue.Value);
            }
        }
    }
}
