using System;

namespace Taga.DynamicServices.Invocation
{
    class SingletonDynamicInvocationHandlerFactory : DynamicInvocationHandlerFactory
    {
        private static readonly object LockObj = new Object();

        private static volatile IDynamicInvocationHandler _handler;

        public override IDynamicInvocationHandler GetHandler()
        {
            if (_handler == null)
            {
                lock (LockObj)
                {
                    if (_handler == null)
                    {
                        _handler = base.GetHandler();
                    }
                }
            }
            return _handler;
        }
    }
}
