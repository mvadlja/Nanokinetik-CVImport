// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	19.10.2011. 16:52:47
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.AP_DOCUMENT_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Ap_document_mn_PKDAL : GEMDataAccess<Ap_document_mn_PK>, IAp_document_mn_PKOperations
	{
		public Ap_document_mn_PKDAL() : base() { }
		public Ap_document_mn_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IAp_document_mn_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AP_DOCUMENT_MN_DeleteByAuthorisedProduct", OperationType = GEMOperationType.Select)]
        public void DeleteByAuthorisedProduct(Int32? authorisedProductPk)
        {
            DateTime methodStart = DateTime.Now;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (authorisedProductPk.HasValue) parameters.Add(new GEMDbParameter("AuthorisedProductPk", authorisedProductPk, DbType.Int32, ParameterDirection.Input));

                base.ExecuteProcedureReturnScalar(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AP_DOCUMENT_MN_AbleToDeleteEntity", OperationType = GEMOperationType.Select)]
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AP_DOCUMENT_MN_GetAttachmentsByAP", OperationType = GEMOperationType.Select)]
        public DataSet GetAttachmentsByAP(Int32? ap_FK)
        {
            DateTime methodStart = DateTime.Now;
            DataSet entities = new DataSet();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (ap_FK != null) parameters.Add(new GEMDbParameter("ap_FK", ap_FK, DbType.Int32, ParameterDirection.Input));

                entities = base.ExecuteProcedureReturnDataSet(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entities;
        }


        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AP_DOCUMENT_MN_GetDocumentsByAP", OperationType = GEMOperationType.Select)]
        public DataSet GetDocumentsByAP(Int32? ap_FK)
        {
            DateTime methodStart = DateTime.Now;
            DataSet entities = new DataSet();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (ap_FK != null) parameters.Add(new GEMDbParameter("ap_FK", ap_FK, DbType.Int32, ParameterDirection.Input));

                entities = base.ExecuteProcedureReturnDataSet(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entities;
        }


        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_DOCUMENT_GetDocumentsByAPWP", OperationType = GEMOperationType.Select)]
        public DataSet GetDocumentsByAPWP(Int32 ap_FK, int pageNumber, int pageSize, out int totalRecordsCount)
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = new DataSet();
            totalRecordsCount = 0;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (ap_FK != null) parameters.Add(new GEMDbParameter("ap_FK", ap_FK, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("PageNum", pageNumber, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("PageSize", pageSize, DbType.Int32, ParameterDirection.Input));

                ds = base.ExecuteProcedureReturnDataSet(parameters, out outputValues);
                totalRecordsCount = (int)outputValues["totalRecordsCount"];

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return ds;
        }


        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AP_DOCUMENT_MN_GetAuthorizedProductsByDocumentFK", OperationType = GEMOperationType.Select)]
        public List<Ap_document_mn_PK> GetAuthorizedProductsByDocumentFK(Int32? document_FK)
        {
            DateTime methodStart = DateTime.Now;
            List<Ap_document_mn_PK> entities = new List<Ap_document_mn_PK>();

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

		#region ICRUDOperations<Ap_document_mn_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AP_DOCUMENT_MN_GetEntity", OperationType = GEMOperationType.Select)]
		public override Ap_document_mn_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AP_DOCUMENT_MN_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Ap_document_mn_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AP_DOCUMENT_MN_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Ap_document_mn_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AP_DOCUMENT_MN_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Ap_document_mn_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AP_DOCUMENT_MN_Save", OperationType = GEMOperationType.Save)]
		public override Ap_document_mn_PK Save(Ap_document_mn_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AP_DOCUMENT_MN_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Ap_document_mn_PK> SaveCollection(List<Ap_document_mn_PK> entities)
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
