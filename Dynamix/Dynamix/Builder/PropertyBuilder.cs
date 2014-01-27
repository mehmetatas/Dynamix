using System;
using System.Reflection;
using Dynamix.Metadata;
using Dynamix.Utils;
using Emit = System.Reflection.Emit;
using OpCodes = System.Reflection.Emit.OpCodes;

namespace Dynamix.Builder
{
    class PropertyBuilder : IMemberBuilder
    {
        private const MethodAttributes PropAttr = MethodAttributes.SpecialName | MethodAttributes.HideBySig;

        private readonly Property _dynamicProperty;

        internal Emit.PropertyBuilder Builder { get; private set; }

        internal PropertyBuilder(Property dynamicProperty)
        {
            _dynamicProperty = dynamicProperty;
        }

        public void Build()
        {
            ValidateAccessorsAccessLevels();

            var typeBuilder = _dynamicProperty.Parent.Builder.Builder;
            Builder = typeBuilder.DefineProperty(_dynamicProperty.Name, PropertyAttributes.HasDefault,
                                                         _dynamicProperty.Type.ResolveType(),
                                                         Type.EmptyTypes);

            if (_dynamicProperty.IsDefault)
                BuildWithDefaultBackingField(typeBuilder);
            else
                BuildWithCustomGetSetMethods(typeBuilder);
        }

        #region ValidateAccessorsAccessLevels

        private void ValidateAccessorsAccessLevels()
        {
            ThrowIfBothAccessorsHaveCustomAscessModifiers();
            ThrowIfAccessorModifierIsNotMoreResticitive(_dynamicProperty.Getter);
            ThrowIfAccessorModifierIsNotMoreResticitive(_dynamicProperty.Setter);
        }

        private void ThrowIfAccessorModifierIsNotMoreResticitive(PropertyAccessorBase accessor)
        {
            if (AccessorIsNotMoreRestrictiveThanProperty(accessor) ||
                AccessorIsProtectedWhilePropertyIsInternal(accessor))
                throw new InvalidOperationException(
                    "The accessibility modifier of the accessor must be more restrictive than the property: " +
                    _dynamicProperty);
        }

        private bool AccessorIsNotMoreRestrictiveThanProperty(PropertyAccessorBase accessor)
        {
            return _dynamicProperty.AccessModifier < accessor.AccessModifier;
        }

        private bool AccessorIsProtectedWhilePropertyIsInternal(PropertyAccessorBase accessor)
        {
            return _dynamicProperty.AccessModifier == MemberAccessModifier.Internal &&
                   accessor.AccessModifier == MemberAccessModifier.Protected;
        }

        private void ThrowIfBothAccessorsHaveCustomAscessModifiers()
        {
            if (_dynamicProperty.AccessModifier != _dynamicProperty.Getter.AccessModifier &&
                _dynamicProperty.AccessModifier != _dynamicProperty.Setter.AccessModifier)
                throw new InvalidOperationException(
                    "Cannot specifiy accessibility modifier for both accessors of property: " +
                    _dynamicProperty);
        }

        #endregion

        private void BuildWithDefaultBackingField(Emit.TypeBuilder typeBuilder)
        {
            var fieldAttr = FieldAttributes.Private;

            if (_dynamicProperty.Attribute == PolymorphicMemberAttribute.Static)
                fieldAttr |= FieldAttributes.Static;

            var field = typeBuilder.DefineField("m_" + _dynamicProperty.Name, _dynamicProperty.Type.ResolveType(), fieldAttr);

            var getter = BuildAccessorMethod(typeBuilder, _dynamicProperty.Getter);
            var getIl = getter.GetILGenerator();
            if (_dynamicProperty.Attribute == PolymorphicMemberAttribute.Static)
            {
                getIl.Emit(OpCodes.Ldsfld, field);
            }
            else
            {
                getIl.Emit(OpCodes.Ldarg_0);
                getIl.Emit(OpCodes.Ldfld, field);
            }
            getIl.Emit(OpCodes.Ret);
            Builder.SetGetMethod(getter);

            var setter = BuildAccessorMethod(typeBuilder, _dynamicProperty.Setter);
            var setIl = setter.GetILGenerator();
            if (_dynamicProperty.Attribute == PolymorphicMemberAttribute.Static)
            {
                setIl.Emit(OpCodes.Ldarg_0);
                setIl.Emit(OpCodes.Stsfld, field);
            }
            else
            {
                setIl.Emit(OpCodes.Ldarg_0);
                setIl.Emit(OpCodes.Ldarg_1);
                setIl.Emit(OpCodes.Stfld, field);
            }
            setIl.Emit(OpCodes.Ret);
            Builder.SetSetMethod(setter);
        }

        private void BuildWithCustomGetSetMethods(Emit.TypeBuilder typeBuilder)
        {
            BuildGetterMethod(typeBuilder);
            BuildSetterMethod(typeBuilder);
        }

        private void BuildGetterMethod(Emit.TypeBuilder typeBuilder)
        {
            if (_dynamicProperty.Getter.Body.IsEmpty)
                return;

            var getter = BuildAccessorMethodBody(typeBuilder, _dynamicProperty.Getter);
            Builder.SetGetMethod(getter);
        }

        private void BuildSetterMethod(Emit.TypeBuilder typeBuilder)
        {
            if (_dynamicProperty.Setter.Body.IsEmpty)
                return;

            var setter = BuildAccessorMethodBody(typeBuilder, _dynamicProperty.Setter);
            // TODO: set return value attributes
            Builder.SetSetMethod(setter);
        }

        private Emit.MethodBuilder BuildAccessorMethodBody(Emit.TypeBuilder typeBuilder, PropertyAccessorBase accessor)
        {
            var accessorMethodBuilder = BuildAccessorMethod(typeBuilder, accessor);
            var il = accessorMethodBuilder.GetILGenerator();
            accessor.Body.Builder.Build(il);
            return accessorMethodBuilder;
        }

        private Emit.MethodBuilder BuildAccessorMethod(Emit.TypeBuilder typeBuilder, PropertyAccessorBase accessor)
        {
            var accessorAttr = accessor.AccessModifier.GetMethodAccessAttributes();
            var polyMorpAttr = _dynamicProperty.Attribute.GetMethodAttributes();
            var attr = PropAttr | accessorAttr | polyMorpAttr;

            return (accessor is PropertySetAccessor)
                ? typeBuilder.DefineMethod(accessor.Name, attr, null, new[] { accessor.Type.ResolveType() })
                : typeBuilder.DefineMethod(accessor.Name, attr, accessor.Type.ResolveType(), null);
        }
    }
}
