// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	21.10.2011. 11:41:09
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PRODUCT_DOMAIN_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Product_domain_mn_PKDAL : GEMDataAccess<Product_domain_mn_PK>, IProduct_domain_mn_PKOperations
	{
		public Product_domain_mn_PKDAL() : base() { }
		public Product_domain_mn_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IProduct_domain_mn_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_DOMAIN_MN_GetDomainByProduct", OperationType = GEMOperationType.Select)]
        public DataSet GetDomainByProduct(Int32? Product_FK)
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_DOMAIN_MN_DeleteByProductPK", OperationType = GEMOperationType.Select)]
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

		#endregion

		#region ICRUDOperations<Product_domain_mn_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_DOMAIN_MN_GetEntity", OperationType = GEMOperationType.Select)]
		public override Product_domain_mn_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_DOMAIN_MN_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Product_domain_mn_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_DOMAIN_MN_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Product_domain_mn_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_DOMAIN_MN_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Product_domain_mn_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_DOMAIN_MN_Save", OperationType = GEMOperationType.Save)]
		public override Product_domain_mn_PK Save(Product_domain_mn_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_DOMAIN_MN_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Product_domain_mn_PK> SaveCollection(List<Product_domain_mn_PK> entities)
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
