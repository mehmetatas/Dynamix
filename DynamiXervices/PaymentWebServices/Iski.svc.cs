using System;
using System.ServiceModel;

namespace PaymentWebServices
{
    [ServiceBehavior(Namespace = "http://odeme.online.iski.gov.tr")]
    public class Iski : IIski
    {
        public IskiFaturaOdemeSonucBilgisi FaturaOde(IskiFaturaOdemeBilgisi faturaBilgisi)
        {
            return new IskiFaturaOdemeSonucBilgisi
            {
                Bilgi = String.Format("{0} numaralı fatura için {1} bankasından {2} tarihinde {3} dekont no ile ödeme alındı!", faturaBilgisi.FaturaNo, faturaBilgisi.BankaKodu, faturaBilgisi.IstekZamani, faturaBilgisi.DekontNo),
                IslemNo = Guid.NewGuid().ToString(),
                IslemSonucKodu = 1,
                IslemZamani = DateTime.Now,
                IstekNo = faturaBilgisi.IstekNo
            };
        }
    }
}
