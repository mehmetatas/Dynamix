using Dynamix.Metadata;
using System.Collections.Generic;

namespace Dynamix
{
    internal static class Compiler
    {
        public static System.Reflection.Assembly Compile(Assembly assembly)
        {
            assembly.Builder.Init();

            DefineTypes(assembly);
            BuildTypes(assembly);

            return assembly.Builder.Build();
        }

        private static void DefineTypes(Assembly assembly)
        {
            Define(assembly.Enums);
            Define(assembly.Delegates);
            Define(assembly.Interfaces);
            Define(assembly.Structs);
            Define(assembly.Classes);
            DefineNestedTypes(assembly.Structs);
            DefineNestedTypes(assembly.Classes);
        }

        private static void DefineNestedTypes(IEnumerable<Construct> parents)
        {
            foreach (var parent in parents)
            {
                Define(parent.NestedEnums);
                Define(parent.NestedDelegates);
                Define(parent.NestedInterfaces);
                Define(parent.NestedStructs);
                Define(parent.NestedClasses);
                DefineNestedTypes(parent.NestedStructs);
                DefineNestedTypes(parent.NestedClasses);
            }
        }

        private static void Define(IEnumerable<TypeBase> dynamicTypes)
        {
            foreach (var dynamicType in dynamicTypes)
                dynamicType.Builder.Define();
        }

        private static void BuildTypes(Assembly assembly)
        {
            Build(assembly.Enums);
            Build(assembly.Delegates);
            Build(assembly.Interfaces);
            Build(assembly.Structs);
            Build(assembly.Classes);
            BuildNestedTypes(assembly.Structs);
            BuildNestedTypes(assembly.Classes);
        }

        private static void BuildNestedTypes(IEnumerable<Construct> parents)
        {
            foreach (var parent in parents)
            {
                Build(parent.NestedEnums);
                Build(parent.NestedDelegates);
                Build(parent.NestedInterfaces);
                Build(parent.NestedStructs);
                Build(parent.NestedClasses);
                BuildNestedTypes(parent.NestedStructs);
                BuildNestedTypes(parent.NestedClasses);
            }
        }

        private static void Build(IEnumerable<TypeBase> dynamicTypes)
        {
            foreach (var dynamicType in dynamicTypes)
                dynamicType.Builder.CreateType();
        }
    }
}
