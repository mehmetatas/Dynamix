using System;
using System.Configuration;

namespace Taga.DynamicServices.Invocation
{
    public class DynamicInvocationHandlerFactory : IDynamicInvocationHandlerFactory
    {
        private static readonly object LockObj = new Object();

        private static volatile IDynamicInvocationHandlerFactory _instance;

        private string _handlerTypeName;

        public static IDynamicInvocationHandlerFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (LockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = GetFactoryInstance();
                        }
                    }
                }

                return _instance;
            }
        }

        private static IDynamicInvocationHandlerFactory GetFactoryInstance()
        {
            var mode = ConfigurationManager.AppSettings["DynamicInvocationHandlerMode"];

            switch (mode)
            {
                case "PerRequest":
                    return new PerRequestDynamicInvocationHandlerFactory();
                case "Singleton":
                    return new SingletonDynamicInvocationHandlerFactory();
                default:
                    return new DynamicInvocationHandlerFactory();
            }
        }

        protected DynamicInvocationHandlerFactory()
        {
        }

        public virtual IDynamicInvocationHandler GetHandler()
        {
            lock (LockObj)
            {
                if (String.IsNullOrWhiteSpace(_handlerTypeName))
                    _handlerTypeName = ConfigurationManager.AppSettings["DynamicInvocationHandler"];

                var handlerType = Type.GetType(_handlerTypeName);

                if (handlerType == null)
                    throw new Exception("Unknown handler type: " + _handlerTypeName);

                return Activator.CreateInstance(handlerType) as IDynamicInvocationHandler;
            }
        }
    }
}
