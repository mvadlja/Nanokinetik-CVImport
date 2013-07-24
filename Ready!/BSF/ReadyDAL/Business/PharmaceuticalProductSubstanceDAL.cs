// ======================================================================================================================
// Author:		ACER\Kiki
// Create date:	3.11.2011. 13:08:15
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PP_ACTIVE_INGREDIENT
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    public class PharmaceuticalProductSubstanceDAL : GEMDataAccess<PharmaceuticalProductSubstance>, IPharmaceuticalProductSubstanceOperations
	{
		public PharmaceuticalProductSubstanceDAL() : base() { }
        public PharmaceuticalProductSubstanceDAL(string dataSourceId) : base(dataSourceId) { }

        #region IPharmaceuticalProductSubstanceOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_PP_SUBSTANCE_GetPreviewFormDataSet]", OperationType = GEMOperationType.Select)]
        public DataSet GetPreviewFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_PP_SUBSTANCE_GetListFormDataSet]", OperationType = GEMOperationType.Select)]
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_PP_SUBSTANCE_DeleteByTypeAndSessionId]", OperationType = GEMOperationType.Select)]
        public void DeleteByTypeAndSessionId(string substanceType, string sessionId)
        {
            var methodStart = DateTime.Now;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("SubstanceType", substanceType, DbType.String, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("SessionId", sessionId, DbType.String, ParameterDirection.Input));

                base.ExecuteProcedureReturnScalar(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return;
        }

        public void DeleteBySessionId(string sessionId)
        {
            foreach (var substanceType in Enum.GetNames(typeof(PharmaceuticalProductSubstance.SubstanceType)))
            {
                DeleteByTypeAndSessionId(substanceType, sessionId);
            }
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_SUBSTANCE_GetEntitiesByTypeAndSessionId", OperationType = GEMOperationType.Select)]
        public List<PharmaceuticalProductSubstance> GetEntitiesByTypeAndSessionId(string substanceType, string sessionId)
        {
            var methodStart = DateTime.Now;
            var entities = new List<PharmaceuticalProductSubstance>();

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("SubstanceType", substanceType, DbType.String, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("SessionId", sessionId, DbType.String, ParameterDirection.Input));

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

		#endregion

        #region ICRUDOperations<Ppsubstance_PK> Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_SUBSTANCE_GetEntity", OperationType = GEMOperationType.Select)]
        public override PharmaceuticalProductSubstance GetEntity<PKType>(PKType entityId)
        {
            return base.GetEntity<PKType>(entityId);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_SUBSTANCE_GetEntities", OperationType = GEMOperationType.Select)]
        public override List<PharmaceuticalProductSubstance> GetEntities()
        {
            return base.GetEntities();
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_SUBSTANCE_GetEntitiesWP", OperationType = GEMOperationType.Select)]
        public override List<PharmaceuticalProductSubstance> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
        {
            return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_SUBSTANCE_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
        public override List<PharmaceuticalProductSubstance> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
        {
            return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_SUBSTANCE_Save", OperationType = GEMOperationType.Save)]
        public override PharmaceuticalProductSubstance Save(PharmaceuticalProductSubstance entity)
        {
            return base.Save(entity);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_SUBSTANCE_Delete", OperationType = GEMOperationType.Delete)]
        public override void Delete<PKType>(PKType entityId)
        {
            base.Delete<PKType>(entityId);
        }

        public override List<PharmaceuticalProductSubstance> SaveCollection(List<PharmaceuticalProductSubstance> entities)
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
