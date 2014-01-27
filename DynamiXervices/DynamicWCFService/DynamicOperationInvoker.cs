using System;
using System.ServiceModel.Dispatcher;
using Taga.DynamicServices.Invocation;

namespace Taga.DynamicServices.WCFService
{
    class DynamicOperationInvoker : IOperationInvoker
    {
        private readonly IDynamicInvocationContext _invocationContext;

        internal DynamicOperationInvoker(IDynamicInvocationContext invocationContext)
        {
            _invocationContext = invocationContext;
        }

        public object[] AllocateInputs()
        {
            return new object[_invocationContext.InputParameters.Length];
        }

        public object Invoke(object instance, object[] inputs, out object[] outputs)
        {
            outputs = new object[0];
            var invocationHandler = DynamicInvocationHandlerFactory.Instance.GetHandler();
            
            for (var i = 0; i < inputs.Length; i++)
                _invocationContext.InputParameters[i].Value = inputs[i];
            
            return invocationHandler.Handle(_invocationContext);
        }

        public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public bool IsSynchronous
        {
            get { return true; }
        }
    }
}
