using System.Runtime.Serialization;

namespace PaymentWebServices
{
    public class IskiFaturaOdemeBilgisi : IskiServisIstegi
    {
        [DataMember]
        public string FaturaNo { get; set; }

        [DataMember]
        public string DekontNo { get; set; }

        [DataMember]
        public string BankaKodu { get; set; }

        [DataMember]
        public string OdemeNotu { get; set; }
    }
}
