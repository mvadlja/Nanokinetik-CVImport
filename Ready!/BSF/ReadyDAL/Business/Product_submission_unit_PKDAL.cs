// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	19.12.2011. 14:44:41
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PRODUCT_SUB_UNIT_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Product_submission_unit_PKDAL : GEMDataAccess<Product_submission_unit_PK>, IProduct_submission_unit_PKOperations
	{
		public Product_submission_unit_PKDAL() : base() { }
		public Product_submission_unit_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IProduct_submission_unit_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_SUB_UNIT_MN_DeleteBySubmissionUnitPK", OperationType = GEMOperationType.Select)]
        public void DeleteBySubmissionUnitPK(Int32? submissionUnitPk)
        {
            var methodStart = DateTime.Now;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("submissionUnitPk", submissionUnitPk, DbType.Int32, ParameterDirection.Input));

                base.ExecuteProcedureReturnScalar(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_SUB_UNIT_MN_GetSUByProduct", OperationType = GEMOperationType.Select)]
        public DataSet GetSUByProduct(Int32? product_FK)
        {
            DateTime methodStart = DateTime.Now;
            DataSet entities = new DataSet();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (product_FK != null) parameters.Add(new GEMDbParameter("product_FK", product_FK, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_SUB_UNIT_MN_GetProductsBySU", OperationType = GEMOperationType.Select)]
        public List<Product_submission_unit_PK> GetProductsBySU(Int32? submission_unit_FK)
        {
            DateTime methodStart = DateTime.Now;
            List<Product_submission_unit_PK> entities = new List<Product_submission_unit_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (submission_unit_FK != null) parameters.Add(new GEMDbParameter("submission_unit_FK", submission_unit_FK, DbType.Int32, ParameterDirection.Input));

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

		#region ICRUDOperations<Product_submission_unit_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_SUB_UNIT_MN_GetEntity", OperationType = GEMOperationType.Select)]
		public override Product_submission_unit_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_SUB_UNIT_MN_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Product_submission_unit_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_SUB_UNIT_MN_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Product_submission_unit_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_SUB_UNIT_MN_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Product_submission_unit_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_SUB_UNIT_MN_Save", OperationType = GEMOperationType.Save)]
		public override Product_submission_unit_PK Save(Product_submission_unit_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_SUB_UNIT_MN_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Product_submission_unit_PK> SaveCollection(List<Product_submission_unit_PK> entities)
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
