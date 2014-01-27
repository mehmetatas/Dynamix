using System;
using System.Linq;
using Dynamix.Metadata;
using Dynamix.Expressions;
using Dynamix.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BindingFlags = System.Reflection.BindingFlags;
using IReflect = System.Reflection.IReflect;

namespace Dynamix.Tests
{
    [TestClass]
    public class ConstructTests
    {
        #region Field

        [TestMethod]
        [TestCategory("ConstructTests")]
        public void ClassWithFields()
        {
            ConstructWithFields(true);
        }

        [TestMethod]
        [TestCategory("ConstructTests")]
        public void StructWithFields()
        {
            ConstructWithFields(false);
        }

        private static void ConstructWithFields(bool isClass)
        {
            var dynAssembly = CreateAssembly();
            var dynConstruct = CreateConstruct(dynAssembly, "TestConstruct", isClass);

            AddField(dynConstruct, "PublicField", MemberAccessModifier.Public, StaticType.Int, FieldAttribute.Default);
            AddField(dynConstruct, "PrivateField", MemberAccessModifier.Private, StaticType.Char, FieldAttribute.Default);
            AddField(dynConstruct, "ProtectedField", MemberAccessModifier.Protected, StaticType.Bool, FieldAttribute.Default);
            AddField(dynConstruct, "InternalField", MemberAccessModifier.Internal, StaticType.Long, FieldAttribute.Default);
            AddField(dynConstruct, "ProtectedInternalField", MemberAccessModifier.ProtectedInternal, StaticType.Object,
                     FieldAttribute.Default);
            AddField(dynConstruct, "PublicStaticField", MemberAccessModifier.Public, StaticType.Int.AsArray(),
                     FieldAttribute.Default, true);
            AddField(dynConstruct, "PublicReadonlyField", MemberAccessModifier.Public, StaticType.String,
                     FieldAttribute.Readonly, false, "readonly field");
            AddField(dynConstruct, "PublicConstField", MemberAccessModifier.Public, StaticType.String, FieldAttribute.Const,
                     false, "const field");
            AddField(dynConstruct, "ProtectedVolatileField", MemberAccessModifier.Protected, StaticType.Int,
                     FieldAttribute.Volatile);

            var asm = dynAssembly.Compile();
            var type = asm.DefinedTypes.First();

            AssertFields(type, dynConstruct);

            var instance = new DynamicObject(type);
            instance.CallConstructor();
            instance.SetField("PublicField", 12);
            Assert.AreEqual(12, instance.GetField("PublicField"));
        }

        private static void AddField(Construct dynConstruct, string name, MemberAccessModifier accessModifier,
                                     ITypeInfo type, FieldAttribute fieldAttribute, bool isStatic = false,
                                     object initialValue = null)
        {
            dynConstruct.Fields.Add(new Field
            {
                Name = name,
                AccessModifier = accessModifier,
                Type = type,
                Attribute = fieldAttribute,
                InitialValue = initialValue,
                IsStatic = isStatic
            });
        }

        private static void AssertFields(IReflect type, Construct dynConstruct)
        {
            if (dynConstruct == null) throw new ArgumentNullException("dynConstruct");
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance |
                                        BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
                .OrderBy(f => f.Name)
                .ToArray();

            var dynFields = dynConstruct.Fields
                .OrderBy(f => f.Name)
                .ToArray();

            Assert.AreEqual(dynFields.Length, fields.Length);

            for (var i = 0; i < fields.Length; i++)
            {
                Assert.AreEqual(fields[i].Name, dynFields[i].Name);
                Assert.AreEqual(fields[i].FieldType, dynFields[i].Type.ResolveType());
                Assert.AreEqual(fields[i].Attributes & dynFields[i].AccessModifier.GetFieldAccessAttributes(),
                                dynFields[i].AccessModifier.GetFieldAccessAttributes());
                Assert.AreEqual(fields[i].IsStatic, dynFields[i].IsStatic);
                Assert.AreEqual(fields[i].IsLiteral, dynFields[i].Attribute == FieldAttribute.Const);
                Assert.AreEqual(fields[i].IsInitOnly, dynFields[i].Attribute == FieldAttribute.Readonly);
            }
        }

        #endregion

        #region Method

        [TestMethod]
        [TestCategory("ConstructTests")]
        public void ClassWithVoidMethod()
        {
            ConstructWithVoidMethod(true);
        }

        [TestMethod]
        [TestCategory("ConstructTests")]
        public void StructWithVoidMethod()
        {
            ConstructWithVoidMethod(false);
        }

        public void ConstructWithVoidMethod(bool isClass)
        {
            var dynAssembly = CreateAssembly();
            var dynConstruct = CreateConstruct(dynAssembly, "TestConstruct", isClass);

            var method = AddMethod(dynConstruct, "TestMethod", MemberAccessModifier.Public, PolymorphicMemberAttribute.Default, StaticType.Void);
            method.Body.AddExpression(Expression.EmptyMethodBody(method.ReturnValue.ReturnType));

            var asm = dynAssembly.Compile();
            var type = asm.DefinedTypes.First();

            AssertMethods(type, dynConstruct);

            var instance = new DynamicObject(type);
            instance.CallConstructor();
            var res = instance.CallMethod("TestMethod");
            Assert.AreEqual(null, res);
        }

        [TestMethod]
        [TestCategory("ConstructTests")]
        public void ClassWithWithValueTypedMethod()
        {
            ConstructWithValueTypedMethod(true);
        }

        [TestMethod]
        [TestCategory("ConstructTests")]
        public void StructWithWithValueTypedMethod()
        {
            ConstructWithValueTypedMethod(false);
        }

        public void ConstructWithValueTypedMethod(bool isClass)
        {
            var dynAssembly = CreateAssembly();
            var dynConstruct = CreateConstruct(dynAssembly, "TestConstruct", isClass);

            var method = AddMethod(dynConstruct, "TestMethod", MemberAccessModifier.Public, PolymorphicMemberAttribute.Default, StaticType.Int);
            method.Body.AddExpression(Expression.EmptyMethodBody(method.ReturnValue.ReturnType));

            var asm = dynAssembly.Compile();
            var type = asm.DefinedTypes.First();

            AssertMethods(type, dynConstruct);

            var instance = new DynamicObject(type);
            instance.CallConstructor();
            var res = instance.CallMethod("TestMethod");
            Assert.AreEqual(0, res);
        }

        [TestMethod]
        [TestCategory("ConstructTests")]
        public void ClassWithReferenceTypedMethod()
        {
            ConstructWithReferenceTypedMethod(true);
        }

        [TestMethod]
        [TestCategory("ConstructTests")]
        public void StructWithReferenceTypedMethod()
        {
            ConstructWithReferenceTypedMethod(false);
        }

        public void ConstructWithReferenceTypedMethod(bool isClass)
        {
            var dynAssembly = CreateAssembly();
            var dynConstruct = CreateConstruct(dynAssembly, "TestConstruct", isClass);

            var method = AddMethod(dynConstruct, "TestMethod", MemberAccessModifier.Public, PolymorphicMemberAttribute.Default, StaticType.String);
            method.Body.AddExpression(Expression.EmptyMethodBody(method.ReturnValue.ReturnType));

            var asm = dynAssembly.Compile();
            var type = asm.DefinedTypes.First();

            AssertMethods(type, dynConstruct);

            var instance = new DynamicObject(type);
            instance.CallConstructor();
            var res = instance.CallMethod("TestMethod");
            Assert.AreEqual(null, res);
        }

        [TestMethod]
        [TestCategory("ConstructTests")]
        public void ClassWithTypedMethod()
        {
            ConstructWithTypedMethod(true);
        }

        [TestMethod]
        [TestCategory("ConstructTests")]
        public void StructWithTypedMethod()
        {
            ConstructWithTypedMethod(false);
        }

        public void ConstructWithTypedMethod(bool isClass)
        {
            var dynAssembly = CreateAssembly();
            var dynConstruct = CreateConstruct(dynAssembly, "TestConstruct", isClass);
            var dynConstruct2 = CreateConstruct(dynAssembly, "TestConstruct2", isClass);

            var method = AddMethod(dynConstruct, "TestMethod", MemberAccessModifier.Public, PolymorphicMemberAttribute.Default, dynConstruct2);
            method.Body.AddExpression(Expression.EmptyMethodBodyDefaultReturnValue(method.ReturnValue.ReturnType));

            AddField(dynConstruct2, "TestField", MemberAccessModifier.Public, StaticType.Int, FieldAttribute.Default);

            var asm = dynAssembly.Compile();
            var type = asm.DefinedTypes.First();

            AssertMethods(type, dynConstruct);

            var instance = new DynamicObject(type);
            instance.CallConstructor();
            var res = instance.CallMethod("TestMethod");
            Assert.AreEqual(dynConstruct2.FullName, res.GetType().FullName);

            var dynObj = new DynamicObject(res);
            dynObj.SetField("TestField", 12);
            Assert.AreEqual(12, dynObj.GetField("TestField"));
        }

        private static Method AddMethod(Construct dynConstruct, string name, MemberAccessModifier accessModifier, 
                                               PolymorphicMemberAttribute polymorphicMemberAttribute, 
                                               ITypeInfo returnType, params Parameter[] parameters)
        {
            var method = new Method
            {
                Name = name,
                AccessModifier = accessModifier,
                Attribute = polymorphicMemberAttribute,
                ReturnValue = new ReturnValue { ReturnType = returnType }
            };

            foreach (var parameter in parameters)
                method.Parameters.Add(parameter);

            dynConstruct.Methods.Add(method);

            return method;
        }

        private static void AssertMethods(IReflect type, Construct dynConstruct)
        {
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance |
                                          BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
                .OrderBy(m => m.Name)
                .ToArray();

            var dynMethods = dynConstruct.Methods
                .OrderBy(m => m.Name)
                .ToArray();

            Assert.AreEqual(dynMethods.Length, methods.Length);

            for (var i = 0; i < methods.Length; i++)
            {
                Assert.AreEqual(methods[i].Name, dynMethods[i].Name);
                Assert.AreEqual(methods[i].ReturnType, dynMethods[i].ReturnValue.ReturnType.ResolveType());
                Assert.AreEqual(methods[i].Attributes & dynMethods[i].AccessModifier.GetMethodAccessAttributes(),
                                dynMethods[i].AccessModifier.GetMethodAccessAttributes());
                Assert.AreEqual(methods[i].IsStatic, dynMethods[i].Attribute == PolymorphicMemberAttribute.Static);
                Assert.AreEqual(methods[i].IsVirtual, dynMethods[i].Attribute == PolymorphicMemberAttribute.Virtual);
            }
        }

        #endregion

        #region Properties

        [TestMethod]
        [TestCategory("ConstructTests")]
        public void ClassWithProperties()
        {
            ConstructWithProperties(true);
        }

        [TestMethod]
        [TestCategory("ConstructTests")]
        public void StructWithProperties()
        {
            ConstructWithProperties(false);
        }

        public void ConstructWithProperties(bool isClass)
        {
            var dynAssembly = CreateAssembly();
            var dynConstruct = CreateConstruct(dynAssembly, "TestConstruct", isClass);

            AddProperty(dynConstruct, "PublicDefaultProp", MemberAccessModifier.Public, StaticType.Int, PolymorphicMemberAttribute.Default);
            var prop = AddProperty(dynConstruct, "PublicReadonlyProp", MemberAccessModifier.Public, StaticType.Int, PolymorphicMemberAttribute.Default);
            prop.Setter.AccessModifier = MemberAccessModifier.Private;
            
            AddProperty(dynConstruct, "PublicStaticProp", MemberAccessModifier.Public, StaticType.Int, PolymorphicMemberAttribute.Static);
            AddProperty(dynConstruct, "PublicCustomProp", MemberAccessModifier.Public, StaticType.Int, PolymorphicMemberAttribute.Static);

            var asm = dynAssembly.Compile();
            var type = asm.DefinedTypes.First();

            AssertProperties(type, dynConstruct);

            var instance = new DynamicObject(type);
            instance.CallConstructor();
            instance.SetProperty("PublicDefaultProp", 12);
            Assert.AreEqual(12, instance.GetProperty("PublicDefaultProp"));
            Assert.AreEqual(0, instance.GetProperty("PublicReadonlyProp"));

            var staticProps = type.GetProperties(BindingFlags.Static | BindingFlags.Public);
            staticProps[0].SetMethod.Invoke(null, new object[] {13});
            Assert.AreEqual(13, staticProps[0].GetGetMethod().Invoke(null, null));
        }

        private static Property AddProperty(Construct dynConstruct, string name, MemberAccessModifier memberAccessModifier, 
                                                   ITypeInfo typeInfo, PolymorphicMemberAttribute polymorphicMemberAttribute)
        {
            return new Property
            {
                Name = name,
                AccessModifier = memberAccessModifier,
                Parent = dynConstruct,
                Type = typeInfo,
                Attribute = polymorphicMemberAttribute
            };
        }

        private static void AssertProperties(IReflect type, Construct dynConstruct)
        {
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance |
                                        BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
                .OrderBy(f => f.Name)
                .ToArray();

            var dynProps = dynConstruct.Properties
                .OrderBy(f => f.Name)
                .ToArray();

            Assert.AreEqual(dynProps.Length, props.Length);

            for (var i = 0; i < props.Length; i++)
            {
                Assert.AreEqual(props[i].Name, dynProps[i].Name);
                Assert.AreEqual(props[i].PropertyType, dynProps[i].Type.ResolveType());

                var dynGetter = dynProps[i].Getter;
                var dynSetter = dynProps[i].Setter;

                var getMethod = props[i].GetMethod;
                var setMethod = props[i].SetMethod;

                Assert.AreEqual(getMethod.Name, dynGetter.Name);
                Assert.AreEqual(getMethod.ReturnType, dynGetter.Type.ResolveType());
                Assert.AreEqual(getMethod.Attributes & dynGetter.AccessModifier.GetMethodAccessAttributes(),
                                dynGetter.AccessModifier.GetMethodAccessAttributes());
                Assert.AreEqual(getMethod.Attributes & dynProps[i].Attribute.GetMethodAttributes(),
                               dynProps[i].Attribute.GetMethodAttributes());

                Assert.AreEqual(setMethod.Name, dynSetter.Name);
                Assert.AreEqual(setMethod.ReturnType, typeof(void));
                Assert.AreEqual(setMethod.Attributes & dynSetter.AccessModifier.GetMethodAccessAttributes(),
                                dynSetter.AccessModifier.GetMethodAccessAttributes());
                Assert.AreEqual(setMethod.Attributes & dynProps[i].Attribute.GetMethodAttributes(),
                               dynProps[i].Attribute.GetMethodAttributes());
            }
        }

        #endregion

        #region TODO: Indexer
        #endregion

        #region TODO: Event
        #endregion

        private static Construct CreateConstruct(Assembly dynAssembly, string name, bool createClass)
        {
            if (createClass)
            {
                return new Class
                    {
                        AccessModifier = TypeAccessModifier.Public,
                        Parent = dynAssembly,
                        Name = name,
                        BaseType = StaticType.Object,
                        InheritanceModifier = InheritanceModifier.Default,
                        Namespace = "Dynamix.Test"
                    };
            }
            return new Struct
                {
                    AccessModifier = TypeAccessModifier.Public,
                    Parent = dynAssembly,
                    Name = name,
                    Namespace = "Dynamix.Test"
                };
        }

        private static Assembly CreateAssembly()
        {
            return new Assembly { Name = Guid.NewGuid().ToString() };
        }
    }
}