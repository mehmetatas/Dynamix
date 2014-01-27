using System;
using System.Web.Services;

namespace PaymentWebServices
{
    [WebService(Namespace = "http://payment.avea.com.tr/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Avea : WebService
    {
        [WebMethod]
        public AveaPaymentResponse DoPayment(AveaPaymentRequest request)
        {
            return new AveaPaymentResponse
            {
                ResultCode = 0,
                Message = String.Format("{0} TL payment received for {1}", request.Amount, request.MSISDN)
            };
        }
    }
}
