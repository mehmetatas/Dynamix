using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Taga.DynamicServices.Client.Base
{
    public abstract class DynamicMethodBase : IDynamicMethod
    {
        private readonly MethodInfo _methodInfo;
        private readonly object _serviceInstance;
        private IEnumerable<IDynamicParameter> _parameters;

        protected DynamicMethodBase(MethodInfo methodInfo, object serviceInstance)
        {
            _methodInfo = methodInfo;
            _serviceInstance = serviceInstance;
        }

        public string Name
        {
            get { return _methodInfo.Name; }
        }

        public Type ReturnType
        {
            get { return _methodInfo.ReturnType; }
        }

        public IEnumerable<IDynamicParameter> Parameters
        {
            get
            {
                if (_parameters == null)
                    LoadParamaters();
                return _parameters;
            }
        }

        public object Invoke()
        {
            return _methodInfo.Invoke(_serviceInstance, Parameters.Select(p => p.Value).ToArray());
        }

        private void LoadParamaters()
        {
            _parameters = _methodInfo.GetParameters().Select(CreateParameter).ToList();
        }
        
        public IDynamicParameter GetParameter(string parameterName)
        {
            return Parameters.SingleOrDefault(p => p.Name == parameterName);
        }

        protected abstract IDynamicParameter CreateParameter(ParameterInfo pi);
    }
}
