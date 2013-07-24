// ======================================================================================================================
// Author:		ACER\Kiki
// Create date:	20.10.2011. 10:05:30
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.DOCUMENT
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Document_PKDAL : GEMDataAccess<Document_PK>, IDocument_PKOperations
	{
		public Document_PKDAL() : base() { }
		public Document_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IDocument_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_DOCUMENT_UpdateCalculatedColumn", OperationType = GEMOperationType.Select)]
        public void UpdateCalculatedColumn(int documentPk, Document_PK.CalculatedColumn calculatedColumn)
        {
            DateTime methodStart = DateTime.Now;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("DocumentPk", documentPk, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("CalculatedColumn", calculatedColumn.ToString(), DbType.String, ParameterDirection.Input));

                base.ExecuteProcedureReturnScalar(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_DOCUMENT_DocumentsSearcher", OperationType = GEMOperationType.Select)]
        public DataSet GetDocumentSearcher(String name, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = null;
            totalRecordsCount = 0;

            try
            {
                // Generating order by clause
                string orderBy = base.CreateOrderByClause(orderByConditions);

                var outputValues = new Dictionary<string, object>();
                var parameters = new List<GEMDbParameter>();

                if (!string.IsNullOrWhiteSpace(name)) parameters.Add(new GEMDbParameter("name", name, DbType.String, ParameterDirection.Input));
                
                parameters.Add(new GEMDbParameter("PageNum", pageNumber, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("PageSize", pageSize, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("OrderByQuery", orderBy, DbType.String, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_DOCUMENT_GetTabMenuItemsCount", OperationType = GEMOperationType.Select)]
        public DataSet GetTabMenuItemsCount(Int32 document_PK, int? personFk)
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = null;

            try
            {
                // Generating order by clause

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("document_PK", document_PK, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("ResponsibleUserPk", personFk, DbType.Int32, ParameterDirection.Input));

                ds = base.ExecuteProcedureReturnDataSet(parameters, out outputValues);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return ds;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_DOCUMENT_GetDocumentsByAP", OperationType = GEMOperationType.Select)]
        public List<Document_PK> GetDocumentsByAP(Int32 ap_FK, string documentTypeName = null)
        {
            DateTime methodStart = DateTime.Now;
            List<Document_PK> entities = new List<Document_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (ap_FK != null) parameters.Add(new GEMDbParameter("ap_FK", ap_FK, DbType.Int32, ParameterDirection.Input));
                if (documentTypeName != null) parameters.Add(new GEMDbParameter("documentTypeName", documentTypeName, DbType.String, ParameterDirection.Input));

                entities = base.ExecuteProcedureReturnEntities(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entities;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_DOCUMENT_GetDocumentsByProduct", OperationType = GEMOperationType.Select)]
        public List<Document_PK> GetDocumentsByProduct(Int32 product_FK)
        {
            DateTime methodStart = DateTime.Now;
            List<Document_PK> entities = new List<Document_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (product_FK != null) parameters.Add(new GEMDbParameter("product_FK", product_FK, DbType.Int32, ParameterDirection.Input));

                entities = base.ExecuteProcedureReturnEntities(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entities;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_DOCUMENT_GetDocumentRelatedEntities", OperationType = GEMOperationType.Select)]
        public DataSet GetDocumentRelatedEntities(Int32 doc_PK)
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = new DataSet();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (doc_PK != null) parameters.Add(new GEMDbParameter("doc_PK", doc_PK, DbType.Int32, ParameterDirection.Input));

                ds = base.ExecuteProcedureReturnDataSet(parameters, out outputValues);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return ds;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_DOCUMENT_GetEDMSRelatedEntities", OperationType = GEMOperationType.Select)]
        public DataSet GetEDMSRelatedEntities(string EDMSDocumentId)
        {
            var methodStart = DateTime.Now;
            var ds = new DataSet();

            try
            {
                var outputValues = new Dictionary<string, object>();
                var parameters = new List<GEMDbParameter>();

                if (string.IsNullOrWhiteSpace(EDMSDocumentId)) return ds;

                if (EDMSDocumentId != null) parameters.Add(new GEMDbParameter("EDMSDocumentId", EDMSDocumentId, DbType.String, ParameterDirection.Input));

                ds = base.ExecuteProcedureReturnDataSet(parameters, out outputValues);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return ds;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_DOCUMENT_GetListFormDataSet]", OperationType = GEMOperationType.Select)]
        public DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
        {
            var methodStart = DateTime.Now;
            DataSet ds = null;
            totalRecordsCount = 0;

            try
            {
                // Generating order by clause
                var orderBy = base.CreateOrderByClause(orderByConditions);
                var parameters = (from pair in filters where !String.IsNullOrWhiteSpace(pair.Value) select new GEMDbParameter(pair.Key, pair.Value, DbType.String, ParameterDirection.Input)).ToList();

                parameters.Add(new GEMDbParameter("PageNum", pageNumber, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("PageSize", pageSize, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("OrderByQuery", orderBy, DbType.String, ParameterDirection.Input));

                Dictionary<string, object> outputValues;

                ds = base.ExecuteProcedureReturnDataSet(parameters, out outputValues);

                if (outputValues.ContainsKey("totalRecordsCount"))
                {
                    totalRecordsCount = (int)outputValues["totalRecordsCount"];
                }

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return ds;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_DOCUMENT_GetListFormDataSetEDMS]", OperationType = GEMOperationType.Select)]
        public DataSet GetListFormDataSetEDMS(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
        {
            var methodStart = DateTime.Now;
            DataSet ds = null;
            totalRecordsCount = 0;

            try
            {
                // Generating order by clause
                var orderBy = CreateOrderByClause(orderByConditions);
                var parameters = (from pair in filters where !String.IsNullOrWhiteSpace(pair.Value) select new GEMDbParameter(pair.Key, pair.Value, DbType.String, ParameterDirection.Input)).ToList();

                parameters.Add(new GEMDbParameter("PageNum", pageNumber, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("PageSize", pageSize, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("OrderByQuery", orderBy, DbType.String, ParameterDirection.Input));

                Dictionary<string, object> outputValues;

                ds = base.ExecuteProcedureReturnDataSet(parameters, out outputValues);

                if (outputValues.ContainsKey("totalRecordsCount"))
                {
                    totalRecordsCount = (int)outputValues["totalRecordsCount"];
                }

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return ds;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_DOCUMENT_GetListFormSearchDataSet]", OperationType = GEMOperationType.Select)]
        public DataSet GetListFormSearchDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
        {
            var methodStart = DateTime.Now;
            DataSet ds = null;
            totalRecordsCount = 0;

            try
            {
                // Generating order by clause
                var orderBy = base.CreateOrderByClause(orderByConditions);
                var parameters = (from pair in filters where !String.IsNullOrWhiteSpace(pair.Value) select new GEMDbParameter(pair.Key, pair.Value, DbType.String, ParameterDirection.Input)).ToList();

                parameters.Add(new GEMDbParameter("PageNum", pageNumber, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("PageSize", pageSize, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("OrderByQuery", orderBy, DbType.String, ParameterDirection.Input));

                Dictionary<string, object> outputValues;

                ds = base.ExecuteProcedureReturnDataSet(parameters, out outputValues);

                if (outputValues.ContainsKey("totalRecordsCount"))
                {
                    totalRecordsCount = (int)outputValues["totalRecordsCount"];
                }

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return ds;
        }

		#endregion

		#region ICRUDOperations<Document_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_DOCUMENT_GetEntity", OperationType = GEMOperationType.Select)]
		public override Document_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_DOCUMENT_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Document_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_DOCUMENT_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Document_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_DOCUMENT_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Document_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_DOCUMENT_Save", OperationType = GEMOperationType.Save)]
		public override Document_PK Save(Document_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_DOCUMENT_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Document_PK> SaveCollection(List<Document_PK> entities)
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
