// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	21.10.2011. 11:47:05
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PRODUCT_PP_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Product_mn_PKDAL : GEMDataAccess<Product_mn_PK>, IProduct_mn_PKOperations
	{
		public Product_mn_PKDAL() : base() { }
		public Product_mn_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IProduct_mn_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_PP_MN_GetProductsByPP", OperationType = GEMOperationType.Select)]
        public DataSet GetProductsByPP(Int32? pp_PK)
        {
            DateTime methodStart = DateTime.Now;
            DataSet entities = new DataSet();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (pp_PK != null) parameters.Add(new GEMDbParameter("pp_PK", pp_PK, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_PP_MN_GetPPByProduct", OperationType = GEMOperationType.Select)]
        public DataSet GetPPByProduct(Int32? Product_FK)
        {
            DateTime methodStart = DateTime.Now;
            DataSet entities = new DataSet();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (Product_FK != null) parameters.Add(new GEMDbParameter("Product_FK", Product_FK, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_PP_MN_DeleteByProductPK", OperationType = GEMOperationType.Select)]
        public void DeleteByProductPK(Int32? Product_FK)
        {
            DateTime methodStart = DateTime.Now;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (Product_FK != null) parameters.Add(new GEMDbParameter("Product_FK", Product_FK, DbType.Int32, ParameterDirection.Input));

                base.ExecuteProcedureReturnScalar(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_PP_MN_DeleteByPharmaceuticalProduct", OperationType = GEMOperationType.Select)]
        public void DeleteByPharmaceuticalProduct(int pharmaceuticalProductPk)
        {
            DateTime methodStart = DateTime.Now;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("PharmaceuticalProductPk", pharmaceuticalProductPk, DbType.Int32, ParameterDirection.Input));

                base.ExecuteProcedureReturnScalar(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_PP_MN_GetProductsByPPFK", OperationType = GEMOperationType.Select)]
        public List<Product_mn_PK> GetProductsByPPFK(Int32? pp_FK)
        {
            DateTime methodStart = DateTime.Now;
            List<Product_mn_PK> entities = new List<Product_mn_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (pp_FK != null) parameters.Add(new GEMDbParameter("pp_FK", pp_FK, DbType.Int32, ParameterDirection.Input));

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

		#region ICRUDOperations<Product_mn_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_PP_MN_GetEntity", OperationType = GEMOperationType.Select)]
		public override Product_mn_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_PP_MN_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Product_mn_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_PP_MN_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Product_mn_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_PP_MN_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Product_mn_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_PP_MN_Save", OperationType = GEMOperationType.Save)]
		public override Product_mn_PK Save(Product_mn_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_PP_MN_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Product_mn_PK> SaveCollection(List<Product_mn_PK> entities)
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
