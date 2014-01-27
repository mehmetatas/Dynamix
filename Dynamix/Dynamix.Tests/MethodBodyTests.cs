using System;
using System.Linq;
using Dynamix.Metadata;
using Dynamix.Utils;
using Dynamix.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dynamix.Tests
{
    [TestClass]
    public class MethodBodyTests
    {
        #region SimpleSumMethods

        [TestMethod]
        [TestCategory("MethodBodyTests")]
        public void SimpleSumMethod()
        {
            SimpleSumMethod(true);
        }

        /// public int Sum(int x, int y) 
        /// {
        ///     return x + y;
        /// }
        private void SimpleSumMethod(bool isClass)
        {
            var dynAssembly = CreateAssembly();
            var dynConstruct = CreateConstruct(dynAssembly, "TestClass", isClass);

            var method = AddMethod(dynConstruct, "Sum", MemberAccessModifier.Public, PolymorphicMemberAttribute.Default,
                      StaticType.Int,
                      new[]
                          {
                              new Parameter {Name = "x", Type = StaticType.Int},
                              new Parameter {Name = "y", Type = StaticType.Int}
                          });

            method.Body.AddExpression(Expression.LoadArg(1));
            method.Body.AddExpression(Expression.LoadArg(2));
            method.Body.AddExpression(Expression.Add());
            method.Body.AddExpression(Expression.Return());

            var asm = dynAssembly.Compile();
            var type = asm.DefinedTypes.First();

            var instance = new DynamicObject(type);
            instance.CallConstructor();
            var res = instance.CallMethod("Sum", new object[] { 3, 5 });
            Assert.AreEqual(8, res);
        }

        [TestMethod]
        [TestCategory("MethodBodyTests")]
        public void SimpleSumMethod2()
        {
            SimpleSumMethod2(true);
        }

        /// public int Sum(int x, int y) 
        /// {
        ///     int res = x + y;
        ///     return res;
        /// }
        private void SimpleSumMethod2(bool isClass)
        {
            var dynAssembly = CreateAssembly();
            var dynConstruct = CreateConstruct(dynAssembly, "TestClass", isClass);

            var method = AddMethod(dynConstruct, "Sum", MemberAccessModifier.Public, PolymorphicMemberAttribute.Default,
                      StaticType.Int,
                      new[]
                          {
                              new Parameter {Name = "x", Type = StaticType.Int},
                              new Parameter {Name = "y", Type = StaticType.Int}
                          });

            method.Body.AddExpression(Expression.LoadArg(1));
            method.Body.AddExpression(Expression.LoadArg(2));
            method.Body.AddExpression(Expression.Add());
            method.Body.AddExpression(Expression.DeclareLocal(StaticType.Int, "res"));
            method.Body.AddExpression(Expression.SetLocal("res"));
            method.Body.AddExpression(Expression.LoadLocal("res"));
            method.Body.AddExpression(Expression.Return());

            var asm = dynAssembly.Compile();
            var type = asm.DefinedTypes.First();

            var instance = new DynamicObject(type);
            instance.CallConstructor();
            var res = instance.CallMethod("Sum", new object[] { 3, 5 });
            Assert.AreEqual(8, res);
        }

        [TestMethod]
        [TestCategory("MethodBodyTests")]
        public void SimpleSumMethod3()
        {
            SimpleSumMethod3(true);
        }

        /// public int Sum(int x, int y) 
        /// {
        ///     int res = x + y;
        ///     return res;
        /// }
        private void SimpleSumMethod3(bool isClass)
        {
            var dynAssembly = CreateAssembly();
            var dynConstruct = CreateConstruct(dynAssembly, "TestClass", isClass);

            var method = AddMethod(dynConstruct, "Sum", MemberAccessModifier.Public, PolymorphicMemberAttribute.Default,
                      StaticType.Int,
                      new[]
                          {
                              new Parameter {Name = "x", Type = StaticType.Int},
                              new Parameter {Name = "y", Type = StaticType.Int}
                          });

            method.Body.AddExpression(Expression.AddArgs(1, 2));
            method.Body.AddExpression(Expression.SetNewLocal(StaticType.Int, "res"));
            method.Body.AddExpression(Expression.ReturnLocal("res"));

            var asm = dynAssembly.Compile();
            var type = asm.DefinedTypes.First();

            var instance = new DynamicObject(type);
            instance.CallConstructor();
            var res = instance.CallMethod("Sum", new object[] { 3, 5 });
            Assert.AreEqual(8, res);
        }

        [TestMethod]
        [TestCategory("MethodBodyTests")]
        public void SimpleSumMethod4()
        {
            SimpleSumMethod4(true);
        }

        /// public int Sum(int x, int y) 
        /// {
        ///     int res = x + y;
        ///     return res;
        /// }
        private void SimpleSumMethod4(bool isClass)
        {
            var dynAssembly = CreateAssembly();
            var dynConstruct = CreateConstruct(dynAssembly, "TestClass", isClass);

            var method = AddMethod(dynConstruct, "Sum", MemberAccessModifier.Public, PolymorphicMemberAttribute.Default,
                      StaticType.Int,
                      new[]
                          {
                              new Parameter {Name = "x", Type = StaticType.Int},
                              new Parameter {Name = "y", Type = StaticType.Int}
                          });

            method.Body.AddExpression(Expression.AddArgs("x", "y"));
            method.Body.AddExpression(Expression.SetNewLocal(StaticType.Int, "res"));
            method.Body.AddExpression(Expression.ReturnLocal("res"));

            var asm = dynAssembly.Compile();
            var type = asm.DefinedTypes.First();

            var instance = new DynamicObject(type);
            instance.CallConstructor();
            var res = instance.CallMethod("Sum", new object[] { 3, 5 });
            Assert.AreEqual(8, res);
        }

        #endregion

        [TestMethod]
        [TestCategory("MethodBodyTests")]
        public void ComplexSumMethod()
        {
            ComplexSumMethod(true);
        }

        /// public int Sum2(int x, int y) 
        /// {
        ///     TestClass test = new TestClass();
        ///     int res = test.Sum(x, y);
        ///     return res;
        /// }
        private void ComplexSumMethod(bool isClass)
        {
            var dynAssembly = CreateAssembly();

            var dynConstruct = CreateConstruct(dynAssembly, "TestClass", isClass);
            var method = AddMethod(dynConstruct, "Sum", MemberAccessModifier.Public, PolymorphicMemberAttribute.Default,
                      StaticType.Int,
                      new[]
                          {
                              new Parameter {Name = "x", Type = StaticType.Int},
                              new Parameter {Name = "y", Type = StaticType.Int}
                          }); 

            method.Body.AddExpression(Expression.AddArgs("x", "y"));
            method.Body.AddExpression(Expression.SetNewLocal(StaticType.Int, "res"));
            method.Body.AddExpression(Expression.ReturnLocal("res"));

            var dynConstruct2 = CreateConstruct(dynAssembly, "TestClass2", isClass);
            var method2 = AddMethod(dynConstruct2, "Sum2", MemberAccessModifier.Public, PolymorphicMemberAttribute.Default, StaticType.Int);
            
            method2.Body.AddExpression(Expression.NewLocal(dynConstruct, "test"));
            method2.Body.AddExpression(Expression.CallMethod("test.Sum", new object[] { 3, 5 }));
            method2.Body.AddExpression(Expression.SetNewLocal(StaticType.Int, "res"));
            method2.Body.AddExpression(Expression.ReturnLocal("res"));

            var asm = dynAssembly.Compile();
            var type = asm.DefinedTypes.First(t => t.Name == "TestClass2");

            var instance = new DynamicObject(type);
            instance.CallConstructor();
            var res = instance.CallMethod("Sum2", null);
            Assert.AreEqual(8, res);
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
