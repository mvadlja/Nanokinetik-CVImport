// ======================================================================================================================
// Author:		ACER\Kiki
// Create date:	1.11.2011. 11:37:51
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.SUBSTANCES
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Substance_PKDAL : GEMDataAccess<Substance_PK>, ISubstance_PKOperations
	{
		public Substance_PKDAL() : base() { }
		public Substance_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region ISubstance_PKOperations Members

        
        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCES_GetSubstancesByNameSearcher", OperationType = GEMOperationType.Select)]
        public bool EntityWithEvCodeExists(string evCode)
        {
            var methodStart = DateTime.Now;
            bool result = false;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("EvCode", evCode, DbType.String, ParameterDirection.Input));

                string numberOfMatchesString = Convert.ToString(base.ExecuteProcedureReturnScalar(parameters));

                int numberOfMatches;
                if (int.TryParse(numberOfMatchesString, out numberOfMatches))
                {
                    result = numberOfMatches > 0;
                }

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return result;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCES_GetSubstancesByNameSearcher", OperationType = GEMOperationType.Select)]
        public DataSet GetSubstancesByNameSearcher(String name, String evcode, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = null;
            totalRecordsCount = 0;
            string orderBy = null;

            try
            {
                // Generating order by clause
                orderBy = base.CreateOrderByClause(orderByConditions);

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (name != null && name.ToString() != String.Empty) parameters.Add(new GEMDbParameter("name", name, DbType.String, ParameterDirection.Input));
                if (evcode != null && evcode.ToString() != String.Empty) parameters.Add(new GEMDbParameter("evcode", evcode, DbType.String, ParameterDirection.Input));
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


        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_SUBSTANCE_GetListFormDataSet]", OperationType = GEMOperationType.Select)]
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

		#endregion

		#region ICRUDOperations<Substance_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCES_GetEntity", OperationType = GEMOperationType.Select)]
		public override Substance_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCES_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Substance_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCES_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Substance_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCES_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Substance_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCES_Save", OperationType = GEMOperationType.Save)]
		public override Substance_PK Save(Substance_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBSTANCES_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Substance_PK> SaveCollection(List<Substance_PK> entities)
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
