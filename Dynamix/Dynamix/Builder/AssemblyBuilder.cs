using System;
using Dynamix.Metadata;
using Emit = System.Reflection.Emit;
using Reflection = System.Reflection;

namespace Dynamix.Builder
{
    internal class AssemblyBuilder
    {
        private readonly Assembly _assembly;

        private Emit.ModuleBuilder _moduleBuilder;
        private Emit.AssemblyBuilder _assemblyBuilder;

        internal AssemblyBuilder(Assembly assembly)
        {
            _assembly = assembly;
        }

        internal void Init()
        {
            if (_moduleBuilder != null)
                return;

            _assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new Reflection.AssemblyName(_assembly.Name),
                                                                                 Emit.AssemblyBuilderAccess.RunAndSave);
            _moduleBuilder = _assemblyBuilder.DefineDynamicModule(_assembly.Name);
        }

        internal Emit.TypeBuilder DefineType(TypeBase dynamicType)
        {
            return _moduleBuilder.DefineType(dynamicType.FullName,
                                             dynamicType.Builder.TypeAttributes,
                                             dynamicType.Builder.BaseType);
        }

        internal Reflection.Assembly Build()
        {
            return _assemblyBuilder;
        }
    }
}
