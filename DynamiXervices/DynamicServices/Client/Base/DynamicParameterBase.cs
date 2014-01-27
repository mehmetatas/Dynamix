using System;
using System.Reflection;

namespace Taga.DynamicServices.Client.Base
{
    public abstract class DynamicParameterBase : IDynamicParameter
    {
        private readonly ParameterInfo _parameterInfo;

        protected DynamicParameterBase(ParameterInfo parameterInfo)
        {
            _parameterInfo = parameterInfo;
        }

        public Type Type
        {
            get { return _parameterInfo.ParameterType; }
        }

        public string Name
        {
            get { return _parameterInfo.Name; }
        }

        public object Value { get; set; }
    }
}
