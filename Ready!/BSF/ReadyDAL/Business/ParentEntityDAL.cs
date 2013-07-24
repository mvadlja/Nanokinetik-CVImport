using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace Ready.Model.Business
{
    public class ParentEntityDAL 
    {
        public static List<ParentEntity> GetEDMSParentEntities(string EDMSDocumentId)
        {
            var relatedEntities = new List<ParentEntity>();

            IDocument_PKOperations documentOperations = new Document_PKDAL();

            if (string.IsNullOrWhiteSpace(EDMSDocumentId)) return relatedEntities;

            var ds = documentOperations.GetEDMSRelatedEntities(EDMSDocumentId);

            if (ds == null || ds.Tables.Count == 0) return relatedEntities;

            var dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                var parentEntity = new ParentEntity();

                int outId;
                Int32.TryParse(Convert.ToString(dr["ID"]), NumberStyles.None, new CultureInfo("en-US"), out outId);
                parentEntity.ID = outId;

                parentEntity.Name = Convert.ToString(dr["Name"]);
                parentEntity.Description = Convert.ToString(dr["Description"]);

                var outParentEntityType = ParentEntityType.Null;
                Enum.TryParse(Convert.ToString(dr["Type"]), true, out outParentEntityType);
                parentEntity.Type = outParentEntityType;

                parentEntity.ResponsibleUser = Convert.ToString(dr["ResponsibleUser"]);

                relatedEntities.Add(parentEntity);
            }

            return relatedEntities;
        }
    }
}
