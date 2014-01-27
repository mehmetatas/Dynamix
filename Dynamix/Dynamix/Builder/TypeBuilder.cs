using System;
using System.Reflection;
using Dynamix.Metadata;

using Emit = System.Reflection.Emit;

namespace Dynamix.Builder
{
    public abstract class TypeBuilder
    {
        internal Emit.TypeBuilder Builder { get; private set; }

        private Type _type;
        public Type Type
        {
            get
            {
                if (IsTypeCreated)
                    return _type;

                Define();

                return Builder;
            }
        }

        private bool IsTypeCreated
        {
            get { return _type != null; }
        }

        private bool IsTypeDefined
        {
            get { return Builder != null; }
        }

        internal void Define()
        {
            if (!IsTypeDefined)
                Builder = DefineType();
        }

        internal Type CreateType()
        {
            if (IsTypeCreated)
                return _type;

            BuildMembers();

            return _type = Builder.CreateType();
        }

        internal Emit.TypeBuilder DefineNestedType(TypeBase dynamicType)
        {
            return Builder.DefineNestedType(dynamicType.FullName,
                                            dynamicType.Builder.TypeAttributes,
                                            dynamicType.Builder.BaseType);
        }


        internal abstract Type BaseType { get; }
        internal abstract TypeAttributes TypeAttributes { get; }

        protected abstract void BuildMembers();
        protected abstract Emit.TypeBuilder DefineType();
    }
}
