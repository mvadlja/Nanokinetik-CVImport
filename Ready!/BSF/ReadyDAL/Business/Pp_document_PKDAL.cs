// ======================================================================================================================
// Author:		POSSIMUSIT-MATE\Mateo
// Create date:	23.3.2012. 8:41:10
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PP_DOCUMENT_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Pp_document_PKDAL : GEMDataAccess<Pp_document_PK>, IPp_document_PKOperations
	{
		public Pp_document_PKDAL() : base() { }
		public Pp_document_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IPp_document_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_DOCUMENT_MN_AbleToDeleteEntity", OperationType = GEMOperationType.Select)]
        public bool AbleToDeleteEntity(int documentPk)
        {
            var methodStart = DateTime.Now;
            var ableToDelete = false;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("documentPk", documentPk, DbType.Int32, ParameterDirection.Input));

                var result = (int?)ExecuteProcedureReturnScalar(parameters);

                ableToDelete = result == 1;

                LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return ableToDelete;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_DOCUMENT_MN_GetPProductsByDocumentFK", OperationType = GEMOperationType.Select)]
        public List<Pp_document_PK> GetPProductsByDocumentFK(Int32? document_FK)
        {
            DateTime methodStart = DateTime.Now;
            List<Pp_document_PK> entities = new List<Pp_document_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (document_FK != null) parameters.Add(new GEMDbParameter("document_FK", document_FK, DbType.Int32, ParameterDirection.Input));

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

		#region ICRUDOperations<Pp_document_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_DOCUMENT_MN_GetEntity", OperationType = GEMOperationType.Select)]
		public override Pp_document_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_DOCUMENT_MN_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Pp_document_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_DOCUMENT_MN_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Pp_document_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_DOCUMENT_MN_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Pp_document_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_DOCUMENT_MN_Save", OperationType = GEMOperationType.Save)]
		public override Pp_document_PK Save(Pp_document_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_DOCUMENT_MN_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Pp_document_PK> SaveCollection(List<Pp_document_PK> entities)
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
