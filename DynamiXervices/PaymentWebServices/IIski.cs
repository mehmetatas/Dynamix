using System.ServiceModel;

namespace PaymentWebServices
{
    [ServiceContract(Namespace = "http://odeme.online.iski.gov.tr")]
    public interface IIski
    {
        [OperationContract]
        IskiFaturaOdemeSonucBilgisi FaturaOde(IskiFaturaOdemeBilgisi faturaBilgisi);
    }
}
