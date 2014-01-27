using System.Linq;
using System.Reflection;

namespace DynamicWebServiceClient
{
    internal class WebMethod
    {
        private readonly WebService _parentService;
        private readonly MethodInfo _methodInfo;

        private WebParameters _inputs;
        private WebParameter _output;
        
        internal WebMethod(WebService parentService, MethodInfo methodInfo)
        {
            _parentService = parentService;
            _methodInfo = methodInfo;
        }

        internal string Name
        {
            get { return _methodInfo.Name; }
        }

        internal WebParameters Inputs
        {
            get
            {
                if (_inputs == null)
                    LoadInputs();
                return _inputs;
            }
        }

        internal WebParameter Output
        {
            get { return _output ?? (_output = new WebParameter("Return", _methodInfo.ReturnType)); }
        }

        private void LoadInputs()
        {
            _inputs = new WebParameters();
            _inputs.AddRange(
                _methodInfo.GetParameters().Select(
                    pi => new WebParameter(pi.Name, pi.ParameterType)));
        }

        internal void Invoke()
        {
            var args = Inputs.Select(inp => inp.ToObject()).ToArray();
            var returnValue = _parentService.Invoke(_methodInfo.Name, args);

            Output.Load(returnValue);
        }

        public override string ToString()
        {
            return _methodInfo.Name;
        }
    }
}
