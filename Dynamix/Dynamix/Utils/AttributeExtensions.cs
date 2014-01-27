using System.Reflection;
using Dynamix.Metadata;

namespace Dynamix.Utils
{
    public static class AttributeExtensions
    {
        public static TypeAttributes GetNestedTypeAccessAttributes(this MemberAccessModifier modifier)
        {
            switch (modifier)
            {
                case MemberAccessModifier.Public:
                    return TypeAttributes.NestedPublic;
                case MemberAccessModifier.ProtectedInternal:
                    return TypeAttributes.NestedFamORAssem;
                case MemberAccessModifier.Internal:
                    return TypeAttributes.NestedAssembly;
                case MemberAccessModifier.Protected:
                    return TypeAttributes.NestedFamily;
                default:
                    return TypeAttributes.NestedPrivate;
            }
        }

        public static FieldAttributes GetFieldAccessAttributes(this MemberAccessModifier modifier)
        {
            switch (modifier)
            {
                case MemberAccessModifier.Public:
                    return FieldAttributes.Public;
                case MemberAccessModifier.ProtectedInternal:
                    return FieldAttributes.FamORAssem;
                case MemberAccessModifier.Internal:
                    return FieldAttributes.Assembly;
                case MemberAccessModifier.Protected:
                    return FieldAttributes.Family;
                default:
                    return FieldAttributes.Private;
            }
        }

        public static MethodAttributes GetMethodAccessAttributes(this MemberAccessModifier modifier)
        {
            switch (modifier)
            {
                case MemberAccessModifier.Public:
                    return MethodAttributes.Public;
                case MemberAccessModifier.ProtectedInternal:
                    return MethodAttributes.FamORAssem;
                case MemberAccessModifier.Internal:
                    return MethodAttributes.Assembly;
                case MemberAccessModifier.Protected:
                    return MethodAttributes.Family;
                default:
                    return MethodAttributes.Private;
            }
        }

        public static MethodAttributes GetMethodAttributes(this PolymorphicMemberAttribute modifier)
        {
            switch (modifier)
            {
                case PolymorphicMemberAttribute.Sealed:
                    return MethodAttributes.Final;
                case PolymorphicMemberAttribute.Abstract:
                    return MethodAttributes.Abstract;
                case PolymorphicMemberAttribute.Override:
                    return MethodAttributes.NewSlot;
                case PolymorphicMemberAttribute.Static:
                    return MethodAttributes.Static;
                case PolymorphicMemberAttribute.Virtual:
                    return MethodAttributes.Virtual;
                default:
                    return default(MethodAttributes);
            }
        }

        public static TypeAttributes GetTypeAccessAttributes(this TypeAccessModifier modifier)
        {
            switch (modifier)
            {
                case TypeAccessModifier.Public:
                    return TypeAttributes.Public;
                default:
                    return TypeAttributes.NotPublic;
            }
        }

        public static TypeAttributes GetInheritanceAttributes(this InheritanceModifier modifier)
        {
            switch (modifier)
            {
                case InheritanceModifier.Static:
                    return TypeAttributes.Sealed | TypeAttributes.Abstract;
                case InheritanceModifier.Abstract:
                    return TypeAttributes.Abstract;
                case InheritanceModifier.Sealed:
                    return TypeAttributes.Sealed;
                default:
                    return default(TypeAttributes);
            }
        }
    }
}
