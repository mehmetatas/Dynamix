using System;
using System.Configuration;
using System.ServiceModel.Description;
using Taga.DynamicServices.Invocation;
using Taga.DynamicServices.Wsdl;

namespace Taga.DynamicServices.WCFService
{
    class DynamicServiceBinder
    {
        internal void AddCustomOperations(ServiceDescription serviceDescription)
        {
            var ws = new XmlSerializer<SimpleWsdl>();
            var swsdlPath = ConfigurationManager.AppSettings["DynamicSwsdl"];
            var wsdl = ws.Deserialize(swsdlPath);
            
            WsdlTypeBuilder.BuildTypes(wsdl);

            foreach (var endpoint in serviceDescription.Endpoints)
            {
                foreach (var service in wsdl.Services.Services)
                {
                    var description = endpoint.Contract;

                    foreach (var method in service.Methods)
                    {
                        var inputMsg = DefineInputMessage(method, description);
                        var outputMsg = DefineOutputMessage(method, description);
                        var operation = DefineOperation(service, method, description, inputMsg, outputMsg);
                        description.Operations.Add(operation);
                    }
                }
            }
        }

        private static MessageDescription DefineInputMessage(WsdlMethod method, ContractDescription description)
        {
            var inputMsg = new MessageDescription(String.Format("{0}{1}/{2}",
                                                       description.Namespace,
                                                       description.Name,
                                                       method.Name),
                                                   MessageDirection.Input);

            var i = 0;
            foreach (var input in method.Inputs)
            {
                var param = new MessagePartDescription(input.Name, description.Namespace)
                {
                    Type = input.Type,
                    Index = i++
                };
                inputMsg.Body.Parts.Add(param);
            }
            return inputMsg;
        }

        private static MessageDescription DefineOutputMessage(WsdlMethod method, ContractDescription description)
        {
            var outputMsg = new MessageDescription(String.Format("{0}{1}/{2}Response",
                                                       description.Namespace,
                                                       description.Name,
                                                       method.Name),
                                                   MessageDirection.Output);

            var retVal = new MessagePartDescription(String.Format("{0}Result", method.Name), description.Namespace)
            {
                Type = method.Type
            };
            outputMsg.Body.ReturnValue = retVal;
            return outputMsg;
        }

        private static OperationDescription DefineOperation(WsdlService service, WsdlMethod method, ContractDescription description, MessageDescription inputMsg, MessageDescription outputMsg)
        {
            var operation = new OperationDescription(method.Name, description);
            operation.Messages.Add(inputMsg);
            operation.Messages.Add(outputMsg);
            operation.Behaviors.Add(new DataContractSerializerOperationBehavior(operation));
            operation.Behaviors.Add(new DynamicOperationBehavior(new DynamicOperationInvoker(new DynamicInvocationContext(service, method))));
            return operation;
        }
    }
}