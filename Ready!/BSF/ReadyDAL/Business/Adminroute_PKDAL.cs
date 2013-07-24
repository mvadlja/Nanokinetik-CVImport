// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	21.10.2011. 11:53:22
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PP_ADMINISTRATION_ROUTE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Adminroute_PKDAL : GEMDataAccess<Adminroute_PK>, IAdminroute_PKOperations
	{
		public Adminroute_PKDAL() : base() { }
		public Adminroute_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IAdminroute_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_ADMINISTRATION_ROUTE_GetAdminRoutesByCodeSearcher", OperationType = GEMOperationType.Select)]
        public DataSet GetAdminRoutesByCodeSearcher(String code, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
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

                if (code != null && code.ToString() != String.Empty) parameters.Add(new GEMDbParameter("code", code, DbType.String, ParameterDirection.Input));
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_ADMINISTRATION_ROUTE_GetEntitiesByPharmaceuticalProduct", OperationType = GEMOperationType.Select)]
        public List<Adminroute_PK> GetEntitiesByPharmaceuticalProduct(int pharmaceuticalProductPk)
        {
            var methodStart = DateTime.Now;
            var entities = new List<Adminroute_PK>();

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("PharmaceuticalProductPk", pharmaceuticalProductPk, DbType.Int32, ParameterDirection.Input));

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

		#region ICRUDOperations<Adminroute_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_ADMINISTRATION_ROUTE_GetEntity", OperationType = GEMOperationType.Select)]
		public override Adminroute_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_ADMINISTRATION_ROUTE_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Adminroute_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_ADMINISTRATION_ROUTE_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Adminroute_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_ADMINISTRATION_ROUTE_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Adminroute_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_ADMINISTRATION_ROUTE_Save", OperationType = GEMOperationType.Save)]
		public override Adminroute_PK Save(Adminroute_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_ADMINISTRATION_ROUTE_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Adminroute_PK> SaveCollection(List<Adminroute_PK> entities)
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
