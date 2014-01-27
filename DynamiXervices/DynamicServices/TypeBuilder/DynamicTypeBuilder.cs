using System;
using System.Reflection;
using System.Reflection.Emit;
using EmitTypeBuilder = System.Reflection.Emit.TypeBuilder;

namespace Taga.DynamicServices.TypeBuilder
{
    class DynamicTypeBuilder
    {
        private const MethodAttributes PropAttr = MethodAttributes.Public |
                                                  MethodAttributes.SpecialName |
                                                  MethodAttributes.HideBySig;

        private static readonly Type ObjectType = typeof(object);
        
        private Type _type;
        private readonly EmitTypeBuilder _typeBuilder;
        private readonly DynamicType _poco;

        public string FullTypeName
        {
            get { return _poco.FullName; }
        }

        internal DynamicTypeBuilder(EmitTypeBuilder typeBuilder, DynamicType poco)
        {
            _typeBuilder = typeBuilder;
            _poco = poco;
        }

        public Type GetDeclaredType()
        {
            return _type ?? _typeBuilder;
        }

        public Type BuildContractType()
        {
            return _type ?? BuildType();
        }

        private Type BuildType()
        {
            AddClassAttributes();

            DefineConstructor();

            foreach (var property in _poco.Properties)
                DefineProperty(property);

            return _type = _typeBuilder.CreateType();
        }

        private void AddClassAttributes()
        {
            foreach (var attribute in _poco.Attributes)
                _typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(attribute.GetConstructors()[0], new object[0]));
        }

        private void DefineConstructor()
        {
            var constructorBuilder = _typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.HasThis, Type.EmptyTypes);

            var baseCtor = ObjectType.GetConstructor(Type.EmptyTypes);

            var il = constructorBuilder.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, baseCtor);

            il.Emit(OpCodes.Ret);
        }

        private void DefineProperty(DynamicProperty property)
        {
            var field = _typeBuilder.DefineField("m_" + property.Name, property.Type, FieldAttributes.Private);
            var propBuilder = _typeBuilder.DefineProperty(property.Name, PropertyAttributes.HasDefault, property.Type, Type.EmptyTypes);
            
            AddPropertyAttributes(property, propBuilder);

            // Get
            var getter = _typeBuilder.DefineMethod("get_" + property.Name, PropAttr, property.Type, null);
            var getIl = getter.GetILGenerator();
            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, field);
            getIl.Emit(OpCodes.Ret);
            propBuilder.SetGetMethod(getter);

            // Set
            var setter = _typeBuilder.DefineMethod("set_" + property.Name, PropAttr, null, new[] { property.Type });
            var setIl = setter.GetILGenerator();
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, field);
            setIl.Emit(OpCodes.Ret);
            propBuilder.SetSetMethod(setter);
        }

        private static void AddPropertyAttributes(DynamicProperty property, PropertyBuilder propBuilder)
        {
            foreach (var attribute in property.Attributes)
                propBuilder.SetCustomAttribute(new CustomAttributeBuilder(attribute.GetConstructors()[0], new object[0]));
        }
    }
}
