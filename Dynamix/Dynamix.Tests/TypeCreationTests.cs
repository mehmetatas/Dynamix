using System.Linq;
using Dynamix.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dynamix.Tests
{
    [TestClass]
    public class TypeCreationTests
    {
        [TestMethod]
        [TestCategory("TypeCreation")]
        public void PublicClassType()
        {
            var dynAssembly = CreateAssembly();

            var dynClass = new Class
            {
                AccessModifier = TypeAccessModifier.Public,
                Parent = dynAssembly,
                Name = "TestClass",
                BaseType = StaticType.Object,
                InheritanceModifier = InheritanceModifier.Default,
                Namespace = "Dynamix.Test"
            };

            var asm = dynAssembly.Compile();
            var type = asm.DefinedTypes.First();
            Assert.IsTrue(type.IsClass);
            Assert.AreEqual(dynClass.FullName, type.FullName);
            Assert.IsTrue(type.IsPublic);
        }

        [TestMethod]
        [TestCategory("TypeCreation")]
        public void InternalClassType()
        {
            var dynAssembly = CreateAssembly();

            var dynClass = new Class
                {
                    AccessModifier = TypeAccessModifier.Internal,
                    Parent = dynAssembly,
                    Name = "TestClass",
                    BaseType = StaticType.Object,
                    InheritanceModifier = InheritanceModifier.Default,
                    Namespace = "Dynamix.Test"
                };

            var asm = dynAssembly.Compile();
            var type = asm.DefinedTypes.First();
            Assert.IsTrue(type.IsClass);
            Assert.AreEqual(dynClass.FullName, type.FullName);
            Assert.IsTrue(type.IsNotPublic);
            Assert.IsFalse(type.IsSealed);
            Assert.IsFalse(type.IsAbstract);
        }

        [TestMethod]
        [TestCategory("TypeCreation")]
        public void InternalAbstractClassType()
        {
            var dynAssembly = CreateAssembly();

            var dynClass = new Class
                {
                    AccessModifier = TypeAccessModifier.Internal,
                    Parent = dynAssembly,
                    Name = "TestClass",
                    BaseType = StaticType.Object,
                    InheritanceModifier = InheritanceModifier.Abstract,
                    Namespace = "Dynamix.Test"
                };

            var asm = dynAssembly.Compile();
            var type = asm.DefinedTypes.First();
            Assert.IsTrue(type.IsClass);
            Assert.AreEqual(dynClass.FullName, type.FullName);
            Assert.IsTrue(type.IsNotPublic);
            Assert.IsTrue(type.IsAbstract);
            Assert.IsFalse(type.IsSealed);
        }

        [TestMethod]
        [TestCategory("TypeCreation")]
        public void NestedProtectedSealadClassType()
        {
            var dynAssembly = CreateAssembly();

            var dynClass = new Class
                {
                    AccessModifier = TypeAccessModifier.Public,
                    Parent = dynAssembly,
                    Name = "TestClass",
                    BaseType = StaticType.Object,
                    InheritanceModifier = InheritanceModifier.Default,
                    Namespace = "Dynamix.Test"
                };

            var dynClass2 = new NestedClass
                {
                    AccessModifier = MemberAccessModifier.Protected,
                    Parent = dynClass,
                    Name = "TestClass",
                    BaseType = StaticType.Object,
                    InheritanceModifier = InheritanceModifier.Sealed,
                    Namespace = "Dynamix.Test"
                };

            var asm = dynAssembly.Compile();
            var type = asm.DefinedTypes.First();
            Assert.IsTrue(type.IsClass);
            Assert.AreEqual(dynClass.FullName, type.FullName);
            Assert.IsTrue(type.IsPublic);

            var nesClsType = type.DeclaredNestedTypes.First();
            Assert.AreEqual(type.FullName + "+" + dynClass2.FullName, nesClsType.FullName);
            Assert.IsTrue(nesClsType.IsNestedFamily);
            Assert.IsTrue(nesClsType.IsClass);
            Assert.IsTrue(nesClsType.IsSealed);
        }

        [TestMethod]
        [TestCategory("TypeCreation")]
        public void ExtendedClassType()
        {
            var dynAssembly = CreateAssembly();

            var dynClass = new Class
                {
                    AccessModifier = TypeAccessModifier.Public,
                    Name = "TestClass",
                    BaseType = StaticType.Object,
                    InheritanceModifier = InheritanceModifier.Default,
                    Namespace = "Dynamix.Test"
                };

            var dynClass2 = new Class
                {
                    AccessModifier = TypeAccessModifier.Public,
                    Name = "TestClass2",
                    BaseType = dynClass,
                    InheritanceModifier = InheritanceModifier.Default,
                    Namespace = "Dynamix.Test"
                };

            dynAssembly.Classes.Add(dynClass2);
            dynAssembly.Classes.Add(dynClass);

            var asm = dynAssembly.Compile();
            var baseType = asm.DefinedTypes.ToArray()[0];
            Assert.IsTrue(baseType.IsClass);
            Assert.IsTrue(baseType.IsPublic);
            Assert.AreEqual(dynClass.FullName, baseType.FullName);

            var subType = asm.DefinedTypes.ToArray()[1];
            Assert.IsTrue(subType.IsClass);
            Assert.IsTrue(subType.IsPublic);
            Assert.AreEqual(subType.BaseType, baseType);
            Assert.AreEqual(dynClass2.FullName, subType.FullName);
        }

        [TestMethod]
        [TestCategory("TypeCreation")]
        public void PublicStaticClassType()
        {
            var dynAssembly = CreateAssembly();

            var dynClass = new Class
                {
                    AccessModifier = TypeAccessModifier.Public,
                    Parent = dynAssembly,
                    Name = "TestClass",
                    BaseType = StaticType.Object,
                    InheritanceModifier = InheritanceModifier.Static,
                    Namespace = "Dynamix.Test"
                };

            var asm = dynAssembly.Compile();
            var type = asm.DefinedTypes.First();
            Assert.IsTrue(type.IsClass);
            Assert.AreEqual(dynClass.FullName, type.FullName);
            Assert.IsTrue(type.IsPublic);
            Assert.IsTrue(type.IsAbstract);
            Assert.IsTrue(type.IsSealed);
        }

        [TestMethod]
        [TestCategory("TypeCreation")]
        public void PublicInterfaceType()
        {
            var dynAssembly = CreateAssembly();

            var dynInterface = new Interface
                {
                    AccessModifier = TypeAccessModifier.Public,
                    Parent = dynAssembly,
                    Name = "TestInterface",
                    Namespace = "Dynamix.Test"
                };

            var asm = dynAssembly.Compile();
            var type = asm.DefinedTypes.First();
            Assert.AreEqual(dynInterface.FullName, type.FullName);
            Assert.IsTrue(type.IsPublic);
            Assert.IsTrue(type.IsInterface);
            Assert.IsTrue(type.IsAbstract);
        }

        [TestMethod]
        [TestCategory("TypeCreation")]
        public void NestedInterfaceType()
        {
            var dynAssembly = CreateAssembly();

            var dynClass = new Class
                {
                    AccessModifier = TypeAccessModifier.Internal,
                    Parent = dynAssembly,
                    Name = "TestClass",
                    BaseType = StaticType.Object,
                    InheritanceModifier = InheritanceModifier.Default,
                    Namespace = "Dynamix.Test"
                };

            var dynNesInterface = new NestedInterface
                {
                    AccessModifier = MemberAccessModifier.Protected,
                    Parent = dynClass,
                    Name = "TestNestedInterface",
                    Namespace = "Dynamix.Test"
                };

            var asm = dynAssembly.Compile();
            var type = asm.DefinedTypes.First();
            Assert.AreEqual(dynClass.FullName, type.FullName);
            Assert.IsTrue(type.IsNotPublic);
            Assert.IsTrue(type.IsClass);

            var nesInterfaceType = type.DeclaredNestedTypes.First();
            Assert.AreEqual(type.FullName + "+" + dynNesInterface.FullName, nesInterfaceType.FullName);
            Assert.IsTrue(nesInterfaceType.IsNestedFamily);
            Assert.IsTrue(nesInterfaceType.IsInterface);
            Assert.IsTrue(nesInterfaceType.IsAbstract);
        }

        [TestMethod]
        [TestCategory("TypeCreation")]
        public void DelegateTypeWithStaticReturnType()
        {
            var dynAssembly = CreateAssembly();

            var dynDelegate = new Delegate
                {
                    AccessModifier = TypeAccessModifier.Internal,
                    Parent = dynAssembly,
                    Name = "TestDelegate",
                    Namespace = "Dynamix.Test",
                    ReturnValue = new ReturnValue
                        {
                            ReturnType = StaticType.Of<System.FileStyleUriParser>()
                        }
                };

            dynDelegate.Parameters.Add(new Parameter { Name = "p1", Type = StaticType.Int });

            var asm = dynAssembly.Compile();
            var type = asm.DefinedTypes.First();
            Assert.AreEqual(dynDelegate.FullName, type.FullName);
            Assert.IsTrue(type.IsNotPublic);
            Assert.IsTrue(type.IsClass);
            Assert.AreEqual(StaticType.Delegate.Type, type.BaseType);
        }

        [TestMethod]
        [TestCategory("TypeCreation")]
        public void DelegateTypeWithReturnType()
        {
            var dynAssembly = CreateAssembly();

            var dynClass = new Class
                {
                    AccessModifier = TypeAccessModifier.Internal,
                    Parent = dynAssembly,
                    Name = "TestClass",
                    BaseType = StaticType.Object,
                    InheritanceModifier = InheritanceModifier.Default,
                    Namespace = "Dynamix.Test"
                };

            var dynClass2 = new Class
                {
                    AccessModifier = TypeAccessModifier.Internal,
                    Parent = dynAssembly,
                    Name = "TestClass2",
                    BaseType = StaticType.Object,
                    InheritanceModifier = InheritanceModifier.Default,
                    Namespace = "Dynamix.Test"
                };

            var dynClass3 = new Class
                {
                    AccessModifier = TypeAccessModifier.Internal,
                    Parent = dynAssembly,
                    Name = "TestClass3",
                    BaseType = StaticType.Object,
                    InheritanceModifier = InheritanceModifier.Default,
                    Namespace = "Dynamix.Test"
                };

            var dynDelegate = new Delegate
                {
                    AccessModifier = TypeAccessModifier.Public,
                    Parent = dynAssembly,
                    Name = "TestDelegate",
                    Namespace = "Dynamix.Test",
                    ReturnValue = new ReturnValue
                        {
                            ReturnType = dynClass
                        }
                };

            dynDelegate.Parameters.Add(new Parameter { Name = "p1", Type = dynClass2 });
            dynDelegate.Parameters.Add(new Parameter { Name = "p2", Type = dynClass3 });

            var asm = dynAssembly.Compile();
            var type = asm.DefinedTypes.First();
            Assert.AreEqual(dynDelegate.FullName, type.FullName);
            Assert.IsTrue(type.IsPublic);
            Assert.IsTrue(type.IsClass);
            Assert.AreEqual(StaticType.Delegate.Type, type.BaseType);
        }

        [TestMethod]
        [TestCategory("TypeCreation")]
        public void PublicStructType()
        {
            var dynAssembly = CreateAssembly();

            var dynStruct = new Struct
                {
                    AccessModifier = TypeAccessModifier.Public,
                    Parent = dynAssembly,
                    Name = "TestStruct",
                    Namespace = "Dynamix.Test"
                };

            var asm = dynAssembly.Compile();
            var type = asm.DefinedTypes.First();
            Assert.IsTrue(type.IsValueType);
            Assert.AreEqual(dynStruct.FullName, type.FullName);
            Assert.IsTrue(type.IsPublic);
        }

        [TestMethod]
        [TestCategory("TypeCreation")]
        public void NestedStructType()
        {
            var dynAssembly = CreateAssembly();

            var dynClass = new Class
                {
                    AccessModifier = TypeAccessModifier.Public,
                    Parent = dynAssembly,
                    Name = "TestClass",
                    BaseType = StaticType.Object,
                    InheritanceModifier = InheritanceModifier.Default,
                    Namespace = "Dynamix.Test"
                };

            var dynStruct = new NestedStruct
                {
                    AccessModifier = MemberAccessModifier.Protected,
                    Parent = dynClass,
                    Name = "TestClass",
                    Namespace = "Dynamix.Test"
                };

            var asm = dynAssembly.Compile();
            var type = asm.DefinedTypes.First();
            Assert.IsTrue(type.IsClass);
            Assert.AreEqual(dynClass.FullName, type.FullName);
            Assert.IsTrue(type.IsPublic);

            var nesClsType = type.DeclaredNestedTypes.First();
            Assert.AreEqual(type.FullName + "+" + dynStruct.FullName, nesClsType.FullName);
            Assert.IsTrue(nesClsType.IsNestedFamily);
            Assert.IsTrue(nesClsType.IsValueType);
        }

        [TestMethod]
        [TestCategory("TypeCreation")]
        public void NestedDelegateTypeWithReturnTypeInStruct()
        {
            var dynAssembly = CreateAssembly();

            var dynStruct = new Struct
                {
                    AccessModifier = TypeAccessModifier.Public,
                    Parent = dynAssembly,
                    Name = "TestStruct",
                    Namespace = "Dynamix.Test"
                };

            var dynDelegate = new NestedDelegate
                {
                    AccessModifier = MemberAccessModifier.Protected,
                    Parent = dynStruct,
                    Name = "TestDelegate",
                    Namespace = "Dynamix.Test",
                    ReturnValue = new ReturnValue
                        {
                            ReturnType = dynStruct
                        }
                };

            dynDelegate.Parameters.Add(new Parameter { Name = "p1", Type = dynStruct });

            var asm = dynAssembly.Compile();
            var type = asm.DefinedTypes.First();
            Assert.IsTrue(type.IsValueType);
            Assert.AreEqual(dynStruct.FullName, type.FullName);
            Assert.IsTrue(type.IsPublic);

            var nesDelegateType = type.DeclaredNestedTypes.First();
            Assert.AreEqual(type.FullName + "+" + dynDelegate.FullName, nesDelegateType.FullName);
            Assert.IsTrue(nesDelegateType.IsNestedFamily);
            Assert.IsTrue(nesDelegateType.IsClass);
            Assert.AreEqual(StaticType.Delegate.Type, nesDelegateType.BaseType);
        }

        [TestMethod]
        [TestCategory("TypeCreation")]
        public void EnumType()
        {
            var dynAssembly = CreateAssembly();

            var dynEnum = new Enum
                {
                    AccessModifier = TypeAccessModifier.Public,
                    Parent = dynAssembly,
                    Name = "TestEnum",
                    Namespace = "Dynamix.Test",
                    BaseType = EnumBaseType.SByte
                };

            dynEnum.Values.Add(new EnumValue { Name = "Value1", Value = 121 });
            dynEnum.Values.Add(new EnumValue { Name = "Value2", Value = 122 });
            dynEnum.Values.Add(new EnumValue { Name = "Value3", Value = 123 });
            dynEnum.Values.Add(new EnumValue { Name = "Value4", Value = 124 });

            var asm = dynAssembly.Compile();
            var type = asm.DefinedTypes.First();
            Assert.IsTrue(type.IsEnum);
            Assert.AreEqual(dynEnum.FullName, type.FullName);
            Assert.IsTrue(type.IsPublic);

            var values = System.Enum.GetValues(type);

            Assert.AreEqual(dynEnum.Values.Count, values.Length);

            for (var i = 0; i < values.Length; i++)
                Assert.AreEqual((sbyte)values.GetValue(i), (sbyte)dynEnum.Values[i].Value);

            for (var i = 0; i < values.Length; i++)
                Assert.AreEqual(values.GetValue(i).ToString(), dynEnum.Values[i].Name);
        }

        [TestMethod]
        [TestCategory("TypeCreation")]
        public void NestedEnumType()
        {
            var dynAssembly = CreateAssembly();

            var dynClass = new Class
                {
                    AccessModifier = TypeAccessModifier.Internal,
                    Parent = dynAssembly,
                    Name = "TestClass",
                    BaseType = StaticType.Object,
                    InheritanceModifier = InheritanceModifier.Default,
                    Namespace = "Dynamix.Test"
                };

            var dynEnum = new NestedEnum
                {
                    Parent = dynClass,
                    Name = "TestEnum",
                    Namespace = "Dynamix.Test",
                    BaseType = EnumBaseType.SByte,
                    AccessModifier = MemberAccessModifier.Protected
                };

            dynEnum.Values.Add(new EnumValue { Name = "Value1", Value = 121 });
            dynEnum.Values.Add(new EnumValue { Name = "Value2", Value = 122 });
            dynEnum.Values.Add(new EnumValue { Name = "Value3", Value = 123 });
            dynEnum.Values.Add(new EnumValue { Name = "Value4", Value = 124 });

            var asm = dynAssembly.Compile();
            var type = asm.DefinedTypes.First();

            var nesEnumType = type.DeclaredNestedTypes.First();
            Assert.AreEqual(type.FullName + "+" + dynEnum.FullName, nesEnumType.FullName);
            Assert.IsTrue(nesEnumType.IsEnum);
            Assert.IsTrue(nesEnumType.IsNestedFamily);

            var values = System.Enum.GetValues(nesEnumType);

            Assert.AreEqual(dynEnum.Values.Count, values.Length);

            for (var i = 0; i < values.Length; i++)
                Assert.AreEqual((sbyte)values.GetValue(i), (sbyte)dynEnum.Values[i].Value);

            for (var i = 0; i < values.Length; i++)
                Assert.AreEqual(values.GetValue(i).ToString(), dynEnum.Values[i].Name);
        }

        private static Assembly CreateAssembly()
        {
            return new Assembly { Name = System.Guid.NewGuid().ToString() };
        }
    }
}