using System.ServiceModel;

namespace PaymentWebServices
{
    [ServiceContract(Namespace = "http://fatura.bedas.gov.tr")]
    public interface IBedas
    {
        [OperationContract]
        BedasFaturaOdemeSonucu FaturaOde(BedasFatura fatura);
    }
}
