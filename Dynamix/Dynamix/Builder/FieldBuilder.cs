using System;
using System.Reflection;
using Dynamix.Metadata;
using Dynamix.Utils;
using Emit = System.Reflection.Emit;

namespace Dynamix.Builder
{
    internal class FieldBuilder : IMemberBuilder
    {
        private readonly Field _dynamicField;

        internal Emit.FieldBuilder Builder { get; private set; }

        internal FieldBuilder(Field dynamicField)
        {
            _dynamicField = dynamicField;
        }

        public void Build()
        {
            var typeBuilder = _dynamicField.Parent.Builder.Builder;
            Builder = typeBuilder.DefineField(_dynamicField.Name, _dynamicField.Type.ResolveType(), Attributes);

            SetValueIfConstant();
        }

        private void SetValueIfConstant()
        {
            if (_dynamicField.Attribute != FieldAttribute.Const)
                return;

            if (_dynamicField.InitialValue == null && Builder.FieldType.IsValueType)
                throw new InvalidOperationException(String.Format("Null constant value cannot be assigned to value typed fields {0}", _dynamicField));

            if (!Builder.FieldType.IsInstanceOfType(_dynamicField.InitialValue))
                throw new InvalidOperationException(String.Format("Value '{0}' cannot be assigned to field {1}", _dynamicField.InitialValue, _dynamicField));

            Builder.SetConstant(_dynamicField.InitialValue);
        }

        private FieldAttributes Attributes
        {
            get
            {
                var attr = _dynamicField.AccessModifier.GetFieldAccessAttributes();
                
                if (_dynamicField.IsStatic)
                    attr |= FieldAttributes.Static;

                switch(_dynamicField.Attribute)
                {
                    case FieldAttribute.Readonly:
                        attr |= FieldAttributes.InitOnly;
                        break;
                    case FieldAttribute.Const:
                        attr |= FieldAttributes.Literal;
                        break;
                }

                return attr;
            }
        }
    }
}
