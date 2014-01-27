using System;
using System.Runtime.Serialization;

namespace PaymentWebServices
{
    [DataContract]
    public class IskiServisIstegi
    {
        [DataMember]
        public DateTime IstekZamani { get; set; }

        [DataMember]
        public string IstekNo { get; set; }
    }
}