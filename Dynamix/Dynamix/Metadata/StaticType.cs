using System;

namespace Dynamix.Metadata
{
    public class StaticType : ITypeInfo
    {
        public static readonly StaticType Void = new StaticType(typeof(void));
        public static readonly StaticType Struct = new StaticType(typeof(ValueType));
        public static readonly StaticType Enum = new StaticType(typeof(System.Enum));
        public static readonly StaticType Delegate = new StaticType(typeof(MulticastDelegate));
        public static readonly StaticType Object = Of<object>();
        public static readonly StaticType Bool = Of<bool>();
        public static readonly StaticType Char = Of<char>();
        public static readonly StaticType String = Of<string>();
        public static readonly StaticType Byte = Of<byte>();
        public static readonly StaticType SByte = Of<sbyte>();
        public static readonly StaticType Short = Of<short>();
        public static readonly StaticType UShort = Of<ushort>();
        public static readonly StaticType Int = Of<int>();
        public static readonly StaticType UInt = Of<uint>();
        public static readonly StaticType Long = Of<long>();
        public static readonly StaticType ULong = Of<ulong>();
        public static readonly StaticType Float = Of<float>();
        public static readonly StaticType Double = Of<double>();
        public static readonly StaticType Decimal = Of<decimal>();
         
        public Type Type { get; private set; }

        public StaticType AsArray()
        {
            return new StaticType(Type.MakeArrayType());
        }

        private StaticType(Type type)
        {
            Type = type;
        }

        public static StaticType Of<T>()
        {
            return new StaticType(typeof(T));
        }

        public Type ResolveType()
        {
            return Type;
        }

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}
