using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Taga.DynamicServices.WCFService
{
    class DynamicOperationBehavior : IOperationBehavior
    {
        private readonly IOperationInvoker _operationInvoker;

        internal DynamicOperationBehavior(IOperationInvoker operationInvoker)
        {
            _operationInvoker = operationInvoker;
        }

        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {

        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            dispatchOperation.Invoker = _operationInvoker;
        }

        public void Validate(OperationDescription operationDescription)
        {

        }
    }
}
