// ======================================================================================================================
// Author:		ACER\Kiki
// Create date:	3.11.2011. 13:08:15
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PP_ACTIVE_INGREDIENT
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Activeingredient_PKDAL : GEMDataAccess<Activeingredient_PK>, IActiveingredient_PKOperations
	{
		public Activeingredient_PKDAL() : base() { }
		public Activeingredient_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IActiveingredient_PKOperations Members
        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_PP_ACTIVE_INGREDIENT_GetPPSearcher]", OperationType = GEMOperationType.Select)]
        public DataSet GetPPSearcher(String name, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
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

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_ACTIVE_INGREDIENT_GetIngredientsByPPPK", OperationType = GEMOperationType.Select)]
        public List<Activeingredient_PK> GetIngredientsByPPPK(Int32? pharmaceutical_product_FK)
        {
            DateTime methodStart = DateTime.Now;
            List<Activeingredient_PK> entities = new List<Activeingredient_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (pharmaceutical_product_FK != null) parameters.Add(new GEMDbParameter("pharmaceutical_product_FK", pharmaceutical_product_FK, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_ACTIVE_INGREDIENT_DeleteNULLByUserID", OperationType = GEMOperationType.Delete)]
        public void DeleteNULLByUserID(Int32? userID)
        {
            DateTime methodStart = DateTime.Now;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (userID != null && userID.ToString() != String.Empty) parameters.Add(new GEMDbParameter("userID", userID, DbType.Int32, ParameterDirection.Input));

                base.ExecuteProcedureReturnScalar(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }
        }

		#endregion

		#region ICRUDOperations<Activeingredient_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_ACTIVE_INGREDIENT_GetEntity", OperationType = GEMOperationType.Select)]
		public override Activeingredient_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_ACTIVE_INGREDIENT_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Activeingredient_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_ACTIVE_INGREDIENT_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Activeingredient_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_ACTIVE_INGREDIENT_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Activeingredient_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_ACTIVE_INGREDIENT_Save", OperationType = GEMOperationType.Save)]
		public override Activeingredient_PK Save(Activeingredient_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_ACTIVE_INGREDIENT_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Activeingredient_PK> SaveCollection(List<Activeingredient_PK> entities)
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
