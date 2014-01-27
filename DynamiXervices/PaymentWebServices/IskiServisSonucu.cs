using System;
using System.Runtime.Serialization;

namespace PaymentWebServices
{
    [DataContract]
    public class IskiServisSonucu
    {
        [DataMember]
        public string IslemNo { get; set; }

        [DataMember]
        public DateTime IslemZamani { get; set; }

        [DataMember]
        public int IslemSonucKodu { get; set; }
    }
}
