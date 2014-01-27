
namespace Dynamix.Metadata
{
    public enum MemberAccessModifier
    {
        Private,
        Protected,
        Internal,
        ProtectedInternal,
        Public
    }

    public enum TypeAccessModifier
    {
        Internal,
        Public
    }

    public enum InheritanceModifier
    {
        Default,
        Abstract,
        Sealed,
        Static
    }
    
    public enum FieldAttribute
    {
        Default,
        Readonly,
        Const,
        Volatile
    }

    public enum PolymorphicMemberAttribute
    {
        Default,
        Static,
        Virtual,
        Abstract,
        Override,
        Sealed
    }

    public enum ParameterAttribute
    {
        Default,
        Ref,
        Out
    }

    public enum ConstructorAttribute
    {
        Default,
        Static
    }

    public enum EnumBaseType
    {
        Int,
        Byte,
        Short,
        Long,
        SByte,
        UShort,
        UInt,
        ULong
    }
}
