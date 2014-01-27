using System;
using System.Web.Services;

namespace PaymentWebServices
{
    [WebService(Namespace = "http://invoice.vodafone.com.tr/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.None)]
    public class Vodafone : WebService
    {
        [WebMethod]
        public VodafoneInvoiceResponse PayInvioce(VodafoneInvoiceRequest request)
        {
            return new VodafoneInvoiceResponse
            {
                Info = String.Format("Payment recivied for {0} at {1}", request.MSISDN, request.RequestTime),
                IsSuccessful = true,
                RequestId = request.RequestId,
                ResponseId = Guid.NewGuid().ToString(),
                ResponseTime = DateTime.Now
            };
        }
    }
}
