using System;

namespace PaymentWebServices
{
    public class VodafoneReponseBase
    {
        public string RequestId { get; set; }
        public string ResponseId { get; set; }
        public DateTime ResponseTime { get; set; }
    }
}
