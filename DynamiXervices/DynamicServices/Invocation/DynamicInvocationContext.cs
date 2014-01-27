using System;
using System.Linq;
using Taga.DynamicServices.Wsdl;

namespace Taga.DynamicServices.Invocation
{
    public class DynamicInvocationContext : IDynamicInvocationContext
    {
        private readonly WsdlService _service;
        private readonly WsdlMethod _method;
        private readonly Type _returnType;
        private readonly Parameter[] _inputParameters;

        public DynamicInvocationContext(WsdlService service, WsdlMethod method)
        {
            _service = service;
            _method = method;
            _returnType = method.Type;
            _inputParameters = _method.Inputs
                        .Select(inp => new Parameter {Name = inp.Name, Type = inp.Type})
                        .ToArray();
        }

        public Parameter[] InputParameters
        {
            get { return _inputParameters; }
        }

        public string MethodName
        {
            get { return _method.Name; }
        }

        public string ServiceName
        {
            get { return _service.FullTypeName; }
        }
        
        public Type ReturnType
        {
            get { return _returnType; }
        }

        public string RouteKey { get; set; }
    }
}
