using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Taga.DynamicServices.TypeBuilder
{
    class DynamicModule
    {
        private const string AssemblyName = "Taga.DynamicServices.DynamicModule";

        private const TypeAttributes ClassAttr = TypeAttributes.Public |
                                                 TypeAttributes.Class |
                                                 TypeAttributes.Serializable;

        private static readonly Type ObjectType = typeof(object);
        private static readonly object LockObj = new Object();

        private static readonly IDictionary<string, DynamicTypeBuilder> ContractBuilders = new Dictionary<string, DynamicTypeBuilder>();

        private readonly AssemblyBuilder _assemblyBuilder;
        private readonly ModuleBuilder _moduleBuilder;

        private DynamicModule()
        {
            _assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(AssemblyName), AssemblyBuilderAccess.RunAndSave);
            _moduleBuilder = _assemblyBuilder.DefineDynamicModule(AssemblyName);
        }

        private static volatile DynamicModule _instance;
        public static DynamicModule Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (LockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new DynamicModule();
                        }
                    }
                }

                return _instance;
            }
        }

        public DynamicTypeBuilder GetBuilder(DynamicType poco)
        {
            lock (ContractBuilders)
            {
                if (ContractBuilders.ContainsKey(poco.FullName))
                    return ContractBuilders[poco.FullName];

                var typeBuilder = _moduleBuilder.DefineType(poco.FullName, ClassAttr, ObjectType, Type.EmptyTypes);
                var contractBuilder = new DynamicTypeBuilder(typeBuilder, poco);

                ContractBuilders.Add(poco.FullName, contractBuilder);
                return contractBuilder;
            }
        }
    }
}
