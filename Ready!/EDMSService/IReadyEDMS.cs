using System.Collections.Generic;
using System.ServiceModel;
using Ready.Model.Business;

namespace EDMSService
{
    [ServiceContract]
    public interface IReadyEDMS
    {
        [OperationContract]
        List<ParentEntity> GetEDMSParentEntities(string EDMSDocumentId);
    }
}
