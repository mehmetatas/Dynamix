using System;
using System.Linq;
using Dynamix.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BindingFlags = System.Reflection.BindingFlags;
using IReflect = System.Reflection.IReflect;

namespace Dynamix.Tests
{
    [TestClass]
    public class InterfaceTests
    {
        [TestMethod]
        [TestCategory("InterfaceTests")]
        public void InterfaceWithMethods()
        {
            var dynAssembly = CreateAssembly();
            var dynInterface = CreateInterface(dynAssembly, "ITest");

            var dynClass = CreateConstruct(dynAssembly, "TestClass", true);
            var dynStruct = CreateConstruct(dynAssembly, "TestStruct", false);

            AddMethod(dynInterface, "VoidMethod", StaticType.Void);
            AddMethod(dynInterface, "StringMethod", StaticType.String);
            AddMethod(dynInterface, "ClassMethod", dynClass);
            AddMethod(dynInterface, "StructMethod", dynStruct);

            var asm = dynAssembly.Compile();
            var type = asm.DefinedTypes.First();

            AssertMethods(type, dynInterface);
        }

        [TestMethod]
        [TestCategory("InterfaceTests")]
        public void InterfaceWithProperties()
        {
            var dynAssembly = CreateAssembly();
            var dynInterface = CreateInterface(dynAssembly, "ITest");

            var dynClass = CreateConstruct(dynAssembly, "TestClass", true);
            var dynStruct = CreateConstruct(dynAssembly, "TestStruct", false);

            AddProperty(dynInterface, "StringProperty", StaticType.String, true, true);
            AddProperty(dynInterface, "ClassProperty", dynClass, true, false);
            AddProperty(dynInterface, "StructProperty", dynStruct, false, true);

            var asm = dynAssembly.Compile();
            var type = asm.DefinedTypes.First();

            AssertProperties(type, dynInterface);
        }

        private static void AddMethod(InterfaceBase dynInterface, string name, ITypeInfo returnType, params Parameter[] parameters)
        {
            var method = new InterfaceMethod
            {
                Name = name,
                ReturnValue = new ReturnValue { ReturnType = returnType }
            };

            foreach (var parameter in parameters)
                method.Parameters.Add(parameter);

            dynInterface.Methods.Add(method);
        }

        private static void AssertMethods(IReflect type, InterfaceBase dynConstruct)
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
                Assert.AreEqual(methods[i].IsPublic, true);
                Assert.AreEqual(methods[i].IsAbstract, true);
                Assert.AreEqual(methods[i].IsVirtual, true);
                Assert.AreEqual(methods[i].IsStatic, false);
            }
        }

        private static void AddProperty(InterfaceBase dynInterface, string name, ITypeInfo typeInfo, bool get, bool set)
        {
            dynInterface.Properties.Add(new InterfaceProperty
                {
                    Name = name,
                    ReturnValue = new ReturnValue { ReturnType = typeInfo },
                    AllowGet = get,
                    AllowSet = set
                });
        }

        private static void AssertProperties(IReflect type, InterfaceBase dynInterface)
        {
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance |
                                        BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
                .OrderBy(f => f.Name)
                .ToArray();

            var dynProps = dynInterface.Properties
                .OrderBy(f => f.Name)
                .ToArray();

            Assert.AreEqual(dynProps.Length, props.Length);

            for (var i = 0; i < props.Length; i++)
            {
                Assert.AreEqual(props[i].Name, dynProps[i].Name);
                Assert.AreEqual(props[i].PropertyType, dynProps[i].ReturnValue.ReturnType.ResolveType());
                Assert.AreEqual(props[i].GetMethod != null, dynProps[i].AllowGet);
                Assert.AreEqual(props[i].SetMethod != null, dynProps[i].AllowSet);
            }
        }

        private static Interface CreateInterface(Assembly dynAssembly, string name)
        {
            return new Interface
                {
                    AccessModifier = TypeAccessModifier.Public,
                    Parent = dynAssembly,
                    Name = name,
                    Namespace = "Dynamix.Test"
                };
        }

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