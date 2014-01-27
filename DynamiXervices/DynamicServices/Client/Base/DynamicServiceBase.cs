using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Taga.DynamicServices.Client.Base
{
    public abstract class DynamicServiceBase : IDynamicService
    {
        private readonly Type _serviceType;
        private IEnumerable<IDynamicMethod> _methods;

        protected DynamicServiceBase(Type serviceType)
        {
            _serviceType = serviceType;
        }

        public string Name
        {
            get { return _serviceType.FullName; }
        }

        public virtual string Address { get; set; }

        public IEnumerable<IDynamicMethod> Methods
        {
            get
            {
                if (_methods == null)
                    LoadMethods();
                return _methods;
            }
        }

        public Type Type
        {
            get { return _serviceType; }
        }

        public IDynamicMethod GetMethod(string methodName)
        {
            return Methods.SingleOrDefault(m => m.Name == methodName);
        }

        private void LoadMethods()
        {
            _methods = Type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod)
                           .Where(IsDynamicMethod)
                           .Select(CreateMethod).ToList();
        }

        protected virtual bool IsDynamicMethod(MethodInfo mi)
        {
            return true;
        }

        protected abstract IDynamicMethod CreateMethod(MethodInfo mi);
    }
}
