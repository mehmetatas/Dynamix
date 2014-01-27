using System;
using System.Linq;
using System.Reflection;
using System.Web.Services.Protocols;

namespace DynamicWebServiceClient
{
    internal class WebService
    {
        private readonly Type _type;
        private WebMethods _methods;

        internal WebService(Type type)
        {
            _type = type;
        }

        internal string Name
        {
            get { return _type.Name; }
        }

        internal Type Type
        {
            get { return _type; }
        }

        internal WebMethods Methods
        {
            get
            {
                if (_methods == null)
                    LoadMethods();
                return _methods;
            }
        }

        private void LoadMethods()
        {
            _methods = new WebMethods();
            _methods.AddRange(_type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(mi => mi.GetCustomAttributes<SoapDocumentMethodAttribute>().Any())
                .Select(mi => new WebMethod(this, mi)));
        }

        internal object Invoke(string method, object[] args)
        {
            var obj = _type.CreateInstance();
            return _type.InvokeMethod(method, obj, args);
        }

        public override string ToString()
        {
            return _type.Name;
        }
    }
}
