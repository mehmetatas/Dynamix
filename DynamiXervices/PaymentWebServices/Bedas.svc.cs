using System;
using System.ServiceModel;

namespace PaymentWebServices
{
    [ServiceBehavior(Namespace = "http://fatura.bedas.gov.tr")]
    public class Bedas : IBedas
    {
        public BedasFaturaOdemeSonucu FaturaOde(BedasFatura fatura)
        {
            return new BedasFaturaOdemeSonucu
            {
                Sonuc = true,
                Mesaj = String.Format("{0} abonesi için {1} tarihinde {2} TL fatura ödemesi alındı", fatura.AboneNo, fatura.OdemeZamani, fatura.OdemeMiktari)
            };
        }
    }
}
