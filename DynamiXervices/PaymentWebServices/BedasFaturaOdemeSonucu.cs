using System.Runtime.Serialization;

namespace PaymentWebServices
{
    [DataContract]
    public class BedasFaturaOdemeSonucu
    {
        [DataMember]
        public bool Sonuc { get; set; }

        [DataMember]
        public string Mesaj { get; set; }
    }
}
