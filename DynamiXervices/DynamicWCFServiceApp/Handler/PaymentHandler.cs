using System;
using Taga.DynamicServices;
using Taga.DynamicServices.AsmxClient.Impl;
using Taga.DynamicServices.Client;
using Taga.DynamicServices.Invocation;
using Taga.DynamicServices.WCFClient.Impl;

namespace DynamicWebServiceApp.Handler
{
    public class PaymentHandler : DynamicInvocationHandler
    {
        protected override IDynamicClientFactory GetClientFactory(string mode)
        {
            if (mode == "asmx")
                return new DynamicWsClientFactory();
            if (mode == "wcf")
                return new DynamicWCFClientFactory();

            throw new ApplicationException("Unknown route mode: " + mode);
        }

        protected override string GetRouteKey(IDynamicInvocationContext context)
        {
            var dynRequest = new DynamicObject(context.InputParameters[0].Value);
            return dynRequest.GetProperty("CompanyCode").ToString();
        }
    }
}