// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	7.10.2011. 23:39:58
// Description:	GEM2 Generated class for table V2_Project.dbo.PRODUCT
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    public class Product_PKDAL : GEMDataAccess<Product_PK>, IProduct_PKOperations
    {
        public Product_PKDAL() : base() { }
        public Product_PKDAL(string dataSourceId) : base(dataSourceId) { }

        #region IProduct_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_UpdateCalculatedColumn", OperationType = GEMOperationType.Select)]
        public void UpdateCalculatedColumn(int productPk, Product_PK.CalculatedColumn calculatedColumn)
        {
            DateTime methodStart = DateTime.Now;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("ProductPk", productPk, DbType.Int32, ParameterDirection.Input));
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_UpdateCalculatedColumnByPharmaceuticalProduct", OperationType = GEMOperationType.Select)]
        public void UpdateCalculatedColumnByPharmaceuticalProduct(int pharmaceuticalProductPk, Product_PK.CalculatedColumn calculatedColumn)
        {
            DateTime methodStart = DateTime.Now;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("PharmaceuticalProductPk", pharmaceuticalProductPk, DbType.Int32, ParameterDirection.Input));
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_PRODUCT_GetListFormDataSet]", OperationType = GEMOperationType.Select)]
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_PRODUCT_GetListFormSearchDataSet]", OperationType = GEMOperationType.Select)]
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_AbleToDeleteEntity", OperationType = GEMOperationType.Select)]
        public bool AbleToDeleteEntity(int productPk)
        {
            DateTime methodStart = DateTime.Now;
            var ableToDelete = false;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("ProductPk", productPk, DbType.Int32, ParameterDirection.Input));

                var result = (int?)base.ExecuteProcedureReturnScalar(parameters);

                ableToDelete = result == 1;

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return ableToDelete;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_GetNextAlphabeticalEntity", OperationType = GEMOperationType.Select)]
        public Int32? GetNextAlphabeticalEntity(Int32? product_PK)
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = null;
            Int32? retProductPK = null;
            try
            {
                // Generating order by clause

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("product_PK", product_PK, DbType.Int32, ParameterDirection.Input));

                ds = base.ExecuteProcedureReturnDataSet(parameters, out outputValues);

                if (ds != null && ds.Tables.Count != 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count != 0)
                    {

                        if (dt.Rows[0]["product_PK"] != null)
                        {
                            retProductPK = Convert.ToInt32(dt.Rows[0]["product_PK"]);
                        }
                    }
                }

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return retProductPK;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_GetPrevAlphabeticalEntity", OperationType = GEMOperationType.Select)]
        public Int32? GetPrevAlphabeticalEntity(Int32? product_PK)
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = null;
            Int32? retProductPK = null;
            try
            {
                // Generating order by clause

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("product_PK", product_PK, DbType.Int32, ParameterDirection.Input));

                ds = base.ExecuteProcedureReturnDataSet(parameters, out outputValues);

                if (ds != null && ds.Tables.Count != 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count != 0)
                    {

                        if (dt.Rows[0]["product_PK"] != null)
                        {
                            retProductPK = Convert.ToInt32(dt.Rows[0]["product_PK"]);
                        }
                    }
                }

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return retProductPK;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_GetEntitiesByPharmaceuticalProduct", OperationType = GEMOperationType.Select)]
        public List<Product_PK> GetEntitiesByPharmaceuticalProduct(int pharmaceuticalProductPk)
        {
            DateTime methodStart = DateTime.Now;
            var entities = new List<Product_PK>();

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_GetProductsByActivity", OperationType = GEMOperationType.Select)]
        public List<Product_PK> GetProductsByActivity(int? activityPk)
        {
            DateTime methodStart = DateTime.Now;
            var entities = new List<Product_PK>();

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("activity_PK", activityPk, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_GetEntitiesByActivity", OperationType = GEMOperationType.Select)]
        public List<Product_PK> GetEntitiesByActivity(int activityPk)
        {
            DateTime methodStart = DateTime.Now;
            var entities = new List<Product_PK>();

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("ActivityPk", activityPk, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_GetTabMenuItemsCount", OperationType = GEMOperationType.Select)]
        public DataSet GetTabMenuItemsCount(Int32 product_PK, int? personFk)
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = null;

            try
            {
                // Generating order by clause

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("product_PK", product_PK, DbType.Int32, ParameterDirection.Input));
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_ProductsSearcher", OperationType = GEMOperationType.Select)]
        public DataSet ProductsSearcher(String name, String countries, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
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

                parameters.Add(new GEMDbParameter("name", name, DbType.String, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("countries", countries, DbType.String, ParameterDirection.Input));
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_GetAvailableProductsForActivity", OperationType = GEMOperationType.Select)]
        public List<Product_PK> GetAvailableEntitiesByActivity(Int32? activity_FK)
        {
            DateTime methodStart = DateTime.Now;
            List<Product_PK> entities = new List<Product_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("activity_FK", activity_FK, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_GetAssignedProductsForActivity", OperationType = GEMOperationType.Select)]
        public List<Product_PK> GetAssignedEntitiesByActivity(Int32? activity_FK)
        {
            DateTime methodStart = DateTime.Now;
            List<Product_PK> entities = new List<Product_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("activity_FK", activity_FK, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_GetAssignedProductsForSubmissionUnit", OperationType = GEMOperationType.Select)]
        public List<Product_PK> GetAssignedProductsForSubmissionUnit(Int32? submissionUnitPk)
        {
            var methodStart = DateTime.Now;
            var entities = new List<Product_PK>();

            try
            {
                var outputValues = new Dictionary<string, object>();
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("submissionUnitPk", submissionUnitPk, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_GetProductsByAPDocument", OperationType = GEMOperationType.Select)]
        public List<Product_PK> GetProductsByApDocument(int? documentFk, int? apFk)
        {
            DateTime methodStart = DateTime.Now;
            var entities = new List<Product_PK>();

            try
            {
                var outputValues = new Dictionary<string, object>();
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("document_FK", documentFk, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("ap_FK", apFk, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_GetProductsByPDocument", OperationType = GEMOperationType.Select)]
        public List<Product_PK> GetProductsByPDocument(int? documentFk, int? productFk)
        {
            DateTime methodStart = DateTime.Now;
            var entities = new List<Product_PK>();

            try
            {
                var outputValues = new Dictionary<string, object>();
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("document_FK", documentFk, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("product_FK", productFk, DbType.Int32, ParameterDirection.Input));

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


        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_GetProductsByADocument", OperationType = GEMOperationType.Select)]
        public List<Product_PK> GetProductsByADocument(int? documentFk, int? activityFk)
        {
            DateTime methodStart = DateTime.Now;
            var entities = new List<Product_PK>();

            try
            {
                var outputValues = new Dictionary<string, object>();
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("document_FK", documentFk, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("activity_FK", activityFk, DbType.Int32, ParameterDirection.Input));

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

        #region ICRUDOperations<Product_PK> Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_GetEntity", OperationType = GEMOperationType.Select)]
        public override Product_PK GetEntity<PKType>(PKType entityId)
        {
            return base.GetEntity<PKType>(entityId);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_GetEntities", OperationType = GEMOperationType.Select)]
        public override List<Product_PK> GetEntities()
        {
            return base.GetEntities();
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_GetEntitiesWP", OperationType = GEMOperationType.Select)]
        public override List<Product_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
        {
            return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
        public override List<Product_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
        {
            return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_Save", OperationType = GEMOperationType.Save)]
        public override Product_PK Save(Product_PK entity)
        {
            return base.Save(entity);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PRODUCT_Delete", OperationType = GEMOperationType.Delete)]
        public override void Delete<PKType>(PKType entityId)
        {
            base.Delete<PKType>(entityId);
        }

        public override List<Product_PK> SaveCollection(List<Product_PK> entities)
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
