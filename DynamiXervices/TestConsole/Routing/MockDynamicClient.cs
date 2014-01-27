using System;
using System.Collections.Generic;
using System.Reflection;
using Taga.DynamicServices;
using Taga.DynamicServices.Client;
using Taga.DynamicServices.Client.Base;

namespace TestConsole.Routing
{
    class MockDynamicClient : DynamicClientBase
    {
        private readonly Type _clientType;

        internal MockDynamicClient(Type clietType)
        {
            _clientType = clietType;
        }

        protected override IEnumerable<IDynamicService> LoadServices()
        {
            return new[] { new MockDynamicService(_clientType) };
        }
    }

    class MockDynamicService : DynamicServiceBase
    {
        private readonly object _service;

        internal MockDynamicService(Type serviceType)
            : base(serviceType)
        {
            _service = Type.CreateInstance();
        }

        protected override IDynamicMethod CreateMethod(MethodInfo mi)
        {
            return new MockDynamicMethod(mi, _service);
        }
    }

    class MockDynamicMethod : DynamicMethodBase
    {
        internal MockDynamicMethod(MethodInfo mi, object serviceInstance)
            : base(mi, serviceInstance)
        {
        }

        protected override IDynamicParameter CreateParameter(ParameterInfo pi)
        {
            return new MockDynamicParamater(pi);
        }
    }

    class MockDynamicParamater : DynamicParameterBase
    {
        internal MockDynamicParamater(ParameterInfo pi)
            : base(pi)
        {
        }
    }
}
