using System;
using System.Runtime.Serialization;

namespace PaymentWebServices
{
    [DataContract]
    public class BedasFatura
    {
        [DataMember]
        public double OdemeMiktari { get; set; }

        [DataMember]
        public DateTime OdemeZamani { get; set; }

        [DataMember]
        public string AboneNo { get; set; }
    }
}
