// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	21.10.2011. 11:32:26
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.LANGUAGE_CODE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Languagecode_PKDAL : GEMDataAccess<Languagecode_PK>, ILanguagecode_PKOperations
	{
		public Languagecode_PKDAL() : base() { }
		public Languagecode_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region ILanguagecode_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_LANGUAGE_CODE_GetLanguageCodeByDocument", OperationType = GEMOperationType.Select)]
        public List<Languagecode_PK> GetLanguageCodeByDocument(Int32? document_PK)
        {
            DateTime methodStart = DateTime.Now;
            List<Languagecode_PK> entities = new List<Languagecode_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("document_PK", document_PK, DbType.Int32, ParameterDirection.Input));

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

		#region ICRUDOperations<Languagecode_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_LANGUAGE_CODE_GetEntity", OperationType = GEMOperationType.Select)]
		public override Languagecode_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_LANGUAGE_CODE_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Languagecode_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_LANGUAGE_CODE_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Languagecode_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_LANGUAGE_CODE_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Languagecode_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_LANGUAGE_CODE_Save", OperationType = GEMOperationType.Save)]
		public override Languagecode_PK Save(Languagecode_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_LANGUAGE_CODE_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Languagecode_PK> SaveCollection(List<Languagecode_PK> entities)
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
