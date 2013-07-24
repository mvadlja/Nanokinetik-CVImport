// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	28.10.2011. 9:44:53
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.DOCUMENT_LANGUAGE_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Document_language_mn_PKDAL : GEMDataAccess<Document_language_mn_PK>, IDocument_language_mn_PKOperations
	{
		public Document_language_mn_PKDAL() : base() { }
		public Document_language_mn_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IDocument_language_mn_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_DOCUMENT_LANGUAGE_MN_GetLanguagesByDocument", OperationType = GEMOperationType.Select)]
        public List<Document_language_mn_PK> GetLanguagesByDocument(Int32? document_FK)
        {
            DateTime methodStart = DateTime.Now;
            List<Document_language_mn_PK> entities = new List<Document_language_mn_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (document_FK != null) parameters.Add(new GEMDbParameter("document_PK", document_FK, DbType.Int32, ParameterDirection.Input));

                entities = base.ExecuteProcedureReturnEntities(parameters, out outputValues);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entities;
        }

		#endregion

		#region ICRUDOperations<Document_language_mn_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_DOCUMENT_LANGUAGE_MN_GetEntity", OperationType = GEMOperationType.Select)]
		public override Document_language_mn_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_DOCUMENT_LANGUAGE_MN_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Document_language_mn_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_DOCUMENT_LANGUAGE_MN_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Document_language_mn_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_DOCUMENT_LANGUAGE_MN_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Document_language_mn_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_DOCUMENT_LANGUAGE_MN_Save", OperationType = GEMOperationType.Save)]
		public override Document_language_mn_PK Save(Document_language_mn_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_DOCUMENT_LANGUAGE_MN_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Document_language_mn_PK> SaveCollection(List<Document_language_mn_PK> entities)
		{
			return base.SaveCollection(entities);
		}

		public override void DeleteCollection<PKType>(List<PKType> entityPKs)
		{
			base.DeleteCollection<PKType>(entityPKs);
		}

		#endregion
	}
}
