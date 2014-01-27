using System.Runtime.Serialization;

namespace PaymentWebServices
{
    [DataContract]
    public class IskiFaturaOdemeSonucBilgisi: IskiServisSonucu
    {
        [DataMember]
        public string IstekNo { get; set; }

        [DataMember]
        public string Bilgi { get; set; }
    }
}
