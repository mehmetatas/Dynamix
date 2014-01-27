using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taga.DynamicServices;
using Taga.DynamicServices.Client;

namespace TestConsole.Client
{
    abstract class DynamicTestClient
    {
        public static readonly IDictionary<string, string> ServiceWsdlUris = new Dictionary<string, string>();

        private readonly string _company;

        static DynamicTestClient()
        {
            ServiceWsdlUris.Add("Avea", "http://localhost:38857/Avea.asmx?wsdl");
            ServiceWsdlUris.Add("Vodafone", "http://localhost:38857/Vodafone.asmx?wsdl");
            ServiceWsdlUris.Add("Bedas", "http://localhost:38857/Bedas.svc?singleWsdl");
            ServiceWsdlUris.Add("Iski", "http://localhost:38857/Iski.svc?singleWsdl");
            //ServiceWsdlUris.Add("Turkcell", "http://localhost:9080/Turkcell/services/PaymentService?wsdl");
            //ServiceWsdlUris.Add("TurkcellAxis", "http://localhost:8080/Axis/services/PaymentService?wsdl");
            //ServiceWsdlUris.Add("Payment", "http://localhost:36338/DynamicService.svc?singleWsdl");
        }

        protected DynamicTestClient(string company)
        {
            _company = company;
        }

        internal static DynamicTestClient GetTestClient(string company)
        {
            if (company == "Avea")
                return new AveaTestClient();
            if (company == "Vodafone")
                return new VodafoneTestClient();
            if (company == "Iski")
                return new IskiTestClient();
            if (company == "Bedas")
                return new BedasTestClient();
            if (company == "Turkcell")
                return new TurkcellTestClient();
            if (company == "TurkcellAxis")
                return new TurkcellAxisTestClient();
            if (company == "Payment")
                return new PaymentTestClient();

            throw new ApplicationException("Unknown company: " + company);
        }

        internal void ShowServiceInfo(IDynamicClientFactory factory)
        {
            Writer.DynamicWriteLine("\n-----------------------------------------------------------");
            Writer.DynamicWriteLine("Company: " + _company + " With " + factory.GetType().Name);
            Writer.DynamicWriteLine("-----------------------------------------------------------\n");

            try
            {
                var serviceInfo = new StringBuilder();

                var client = GetClient(factory);

                foreach (var service in client.Services)
                {
                    foreach (var method in service.Methods)
                    {
                        serviceInfo.Append(method.ReturnType + " " + service.Name + "." + method.Name + "(");

                        if (method.Parameters.Any())
                        {
                            foreach (var parameter in method.Parameters)
                                serviceInfo.Append(parameter.Type + " " + parameter.Name + ", ");
                            serviceInfo.Remove(serviceInfo.Length - 2, 2);
                        }

                        serviceInfo.AppendLine(")");
                        if (method.Parameters.Any())
                        {
                            serviceInfo.AppendLine("  Input");
                            foreach (var parameter in method.Parameters)
                            {
                                var props = parameter.Type.GetProperties();
                                foreach (var prop in props)
                                    serviceInfo.AppendLine("    " + parameter.Name + "." + prop.Name + " : " +
                                                           prop.PropertyType);
                            }
                        }

                        if (method.ReturnType != typeof(void))
                        {
                            serviceInfo.AppendLine("  Output");
                            var respProps = method.ReturnType.GetProperties();
                            foreach (var prop in respProps)
                                serviceInfo.AppendLine("    output" + "." + prop.Name + " : " + prop.PropertyType);
                        }
                    }
                }

                Writer.DynamicWriteLine(serviceInfo);
            }
            catch (Exception ex)
            {
                Writer.DynamicWriteLine(ex);
            }
        }

        internal void TestPayment(IDynamicClientFactory factory)
        {
            Writer.DynamicWriteLine("\n-----------------------------------------------------------");
            Writer.DynamicWriteLine("Testing " + _company + " With " + factory.GetType().Name);
            Writer.DynamicWriteLine("-----------------------------------------------------------\n");

            try
            {
                var client = GetClient(factory);
                var service = client.Services.First();
                var method = service.Methods.First(m => m.Parameters.Any());
                var request = method.Parameters.First();

                var dynReq = new DynamicObject(request.Type);
                dynReq.CallConstructor();

                SetRequest(dynReq);

                // Console.WriteLine("CompanyCode: " + dynReq.GetProperty("CompanyCode") + "\n");

                request.Value = dynReq.ObjectInstance;
                dynamic response = method.Invoke();

                ShowResponse(response);
            }
            catch (Exception ex)
            {
                Writer.DynamicWriteLine(ex);
            }
        }

        private IDynamicClient GetClient(IDynamicClientFactory factory)
        {
            return factory.GetClient(ServiceWsdlUris[_company]);
        }

        protected abstract void ShowResponse(dynamic response);
        protected abstract void SetRequest(DynamicObject dynReq);
    }

    class PaymentTestClient : DynamicTestClient
    {
        internal PaymentTestClient()
            : base("Payment")
        {
        }

        protected override void ShowResponse(dynamic response)
        {
            Writer.DynamicWriteLine(response.IsSuccessful);
            Writer.DynamicWriteLine(response.Message);
            Writer.DynamicWriteLine(response.ResponseId);
            Writer.DynamicWriteLine(response.RequestId);
            Writer.DynamicWriteLine(response.ResponseDate);
            Writer.DynamicWriteLine(response.ResultCode);
            Writer.DynamicWriteLine(response.ProcessNumber);
        }

        protected override void SetRequest(DynamicObject dynReq)
        {
            dynReq.SetProperty("SubscriberNumber", "subs1234");
            dynReq.SetProperty("Amount", 12.567d);
            dynReq.SetProperty("Currency", "TL");
            dynReq.SetProperty("RequestId", "reqId1234");
            dynReq.SetProperty("RequestDate", DateTime.Now);
            dynReq.SetProperty("ReceiptNumber", "recNum1234");
            dynReq.SetProperty("BankCode", "bank1234");
            dynReq.SetProperty("Message", "Message Received");
            dynReq.SetProperty("InvoiceNumber", "invNum1234");
            dynReq.SetProperty("IdentityNumber", "idn155141");
            dynReq.SetProperty("FullName", "Mehmet Ataş");
            dynReq.SetProperty("CompanyCode", RouteKeys[_keyIndex++ % RouteKeys.Length]);
        }

        private static int _keyIndex;

        private static readonly string[] RouteKeys = new[]
        {
            "Avea",
            "Vodafone",
            "Bedas",
            "Iski",
            "Turkcell",
            "TurkcellAxis"
        };
    }

    class AveaTestClient : DynamicTestClient
    {
        internal AveaTestClient()
            : base("Avea")
        {
        }

        protected override void ShowResponse(dynamic response)
        {
            Writer.DynamicWriteLine(response.ResultCode);
            Writer.DynamicWriteLine(response.Message);
        }

        protected override void SetRequest(DynamicObject dynReq)
        {
            dynReq.SetProperty("MSISDN", "5051234567");
            dynReq.SetProperty("Amount", 12.65);
        }
    }

    class VodafoneTestClient : DynamicTestClient
    {
        internal VodafoneTestClient()
            : base("Vodafone")
        {
        }

        protected override void ShowResponse(dynamic response)
        {
            Writer.DynamicWriteLine(response.RequestId);
            Writer.DynamicWriteLine(response.ResponseId);
            Writer.DynamicWriteLine(response.ResponseTime);
            Writer.DynamicWriteLine(response.IsSuccessful);
            Writer.DynamicWriteLine(response.Info);
        }

        protected override void SetRequest(DynamicObject dynReq)
        {
            dynReq.SetProperty("MSISDN", "5421234567");
            dynReq.SetProperty("RequestId", "reqId1234");
            dynReq.SetProperty("RequestTime", DateTime.Now);
        }
    }


    class IskiTestClient : DynamicTestClient
    {
        internal IskiTestClient()
            : base("Iski")
        {
        }

        protected override void ShowResponse(dynamic response)
        {
            Writer.DynamicWriteLine(response.IslemSonucKodu);
            Writer.DynamicWriteLine(response.IslemZamani);
            Writer.DynamicWriteLine(response.IslemNo);
            Writer.DynamicWriteLine(response.IstekNo);
            Writer.DynamicWriteLine(response.Bilgi);
        }

        protected override void SetRequest(DynamicObject dynReq)
        {
            dynReq.SetProperty("IstekNo", "reqId1234");
            dynReq.SetProperty("IstekZamani", DateTime.Now);
            dynReq.SetProperty("FaturaNo", "fatNo1234");
            dynReq.SetProperty("DekontNo", "dekNo1234");
            dynReq.SetProperty("BankaKodu", "bank1234");
            dynReq.SetProperty("OdemeNotu", "not girildi");
        }
    }

    class BedasTestClient : DynamicTestClient
    {
        internal BedasTestClient()
            : base("Bedas")
        {
        }

        protected override void ShowResponse(dynamic response)
        {
            Writer.DynamicWriteLine(response.Sonuc);
            Writer.DynamicWriteLine(response.Mesaj);
        }

        protected override void SetRequest(DynamicObject dynReq)
        {
            dynReq.SetProperty("OdemeMiktari", 12.56d);
            dynReq.SetProperty("OdemeZamani", DateTime.Now);
            dynReq.SetProperty("AboneNo", "subs1234");
        }
    }

    class TurkcellTestClient : DynamicTestClient
    {
        internal TurkcellTestClient()
            : base("Turkcell")
        {
        }

        protected override void ShowResponse(dynamic response)
        {
            Writer.DynamicWriteLine(response.paymentCode);
            Writer.DynamicWriteLine(response.message);
            Writer.DynamicWriteLine(response.requestId);
            Writer.DynamicWriteLine(response.responseDate);
            Writer.DynamicWriteLine(response.resultCode);
        }

        protected override void SetRequest(DynamicObject dynReq)
        {
            dynReq.SetProperty("requestDate", DateTime.Now);
            dynReq.SetProperty("requestId", "reqId1234");
            dynReq.SetProperty("subscriberInfo.msisdn", "5321234567", true);
            dynReq.SetProperty("subscriberInfo.identityNumber", "idn155141");
            dynReq.SetProperty("subscriberInfo.fullName", "Mehmet Ataş");
            dynReq.SetProperty("paymentInfo.invoiceNumber", "invNum1234", true);
            dynReq.SetProperty("paymentInfo.payment.amount", 12.56d, true);
            dynReq.SetProperty("paymentInfo.payment.currency", "TL");
        }
    }

    class TurkcellAxisTestClient : DynamicTestClient
    {
        internal TurkcellAxisTestClient()
            : base("TurkcellAxis")
        {
        }

        protected override void ShowResponse(dynamic response)
        {
            Writer.DynamicWriteLine(response.paymentCode);
            Writer.DynamicWriteLine(response.message);
            Writer.DynamicWriteLine(response.requestId);
            Writer.DynamicWriteLine(response.responseDate);
            Writer.DynamicWriteLine(response.resultCode);
        }

        protected override void SetRequest(DynamicObject dynReq)
        {
            dynReq.SetProperty("requestDate", DateTime.Now);
            dynReq.SetProperty("requestId", "reqId1234");
            dynReq.SetProperty("subscriberInfo.msisdn", "5321234567", true);
            dynReq.SetProperty("subscriberInfo.identityNumber", "idn155141");
            dynReq.SetProperty("subscriberInfo.fullName", "Mehmet Ataş");
            dynReq.SetProperty("paymentInfo.invoiceNumber", "invNum1234", true);
            dynReq.SetProperty("paymentInfo.payment.amount", 12.56d, true);
            dynReq.SetProperty("paymentInfo.payment.currency", "TL");
        }
    }

    static class Writer
    {
        public static void DynamicWriteLine(dynamic dynObj)
        {
            if (dynObj != null)
                Console.WriteLine(dynObj);
        }
    }
}
