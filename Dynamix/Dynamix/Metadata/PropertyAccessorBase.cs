
using System.Collections.Generic;

namespace Dynamix.Metadata
{
    public abstract class PropertyAccessorBase
    {
        protected readonly PropertyBase PropertyBase;

        protected PropertyAccessorBase(PropertyBase dynamicPropertyBase)
        {
            PropertyBase = dynamicPropertyBase;
            Body = new MethodBody(PropertyBase);
        }

        public MethodBody Body { get; private set; }

        public ITypeInfo Type
        {
            get { return PropertyBase.Type; }
        }

        private MemberAccessModifier? _accessModifier;
        public MemberAccessModifier AccessModifier
        {
            get { return _accessModifier ?? PropertyBase.AccessModifier; }
            set { _accessModifier = value; }
        }

        public abstract string Name { get; }
    }

    public class PropertyGetAccessor : PropertyAccessorBase
    {
        internal PropertyGetAccessor(Property dynamicProperty)
            : base(dynamicProperty)
        {
        }

        public override string Name
        {
            get { return "get_" + PropertyBase.Name; }
        }
    }

    public class PropertySetAccessor : PropertyAccessorBase
    {
        private readonly ReturnValue _returnValue;

        internal PropertySetAccessor(Property dynamicProperty)
            : base(dynamicProperty)
        {
            _returnValue = new ReturnValue { ReturnType = Type };
        }

        public override string Name
        {
            get { return "set_" + PropertyBase.Name; }
        }

        public IList<ITypeInfo> ReturnAttributes
        {
            get { return _returnValue.CustomAttributes; }
        }
    }
}
