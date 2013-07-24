using System.Collections.Generic;
using Ready.Model.Business;

namespace EDMSService
{
    public class ReadyEDMS : IReadyEDMS
    {
        public List<ParentEntity> GetEDMSParentEntities(string EDMSDocumentId)
        {
            return ParentEntityDAL.GetEDMSParentEntities(EDMSDocumentId);
        }
    }
}
