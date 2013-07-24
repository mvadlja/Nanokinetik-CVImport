// ======================================================================================================================
// Author:		TomoZ560\Tomo
// Create date:	13.10.2011. 22:14:14
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.AUTHORISED_PRODUCT
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    public class AuthorisedProductDAL : GEMDataAccess<AuthorisedProduct>, IAuthorisedProductOperations
    {
        public AuthorisedProductDAL() : base() { }
        public AuthorisedProductDAL(string dataSourceId) : base(dataSourceId) { }

        #region IAuthorisedProductOperations Members
        /// <summary>
        /// Returns number of Auth Product which are artice 57 reporting for given product
        /// </summary>
        /// <param name="product_PK"></param>
        /// <returns></returns>
        /// 
        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AUTHORISED_PRODUCT_GetA57RelEntityIDsWithoutXevprmByProduct", OperationType = GEMOperationType.Select)]
        public List<int> GetA57RelEntityIDsWithoutXevprmByProduct(int? product_FK)
        {
            DateTime methodStart = DateTime.Now;
            List<int> result = new List<int>();

            try
            {
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (product_FK.HasValue) parameters.Add(new GEMDbParameter("product_FK", product_FK, DbType.Int32, ParameterDirection.Input));

                DataSet apIDs = base.ExecuteProcedureReturnDataSet(parameters);

                if (apIDs != null &&
                    apIDs.Tables.Count > 0 &&
                    apIDs.Tables[0].Columns.Contains("ap_PK") &&
                    apIDs.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in apIDs.Tables[0].Rows)
                    {
                        if (row["ap_PK"] != DBNull.Value)
                        {
                            result.Add((int)row["ap_PK"]);
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

            return result;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AUTHORISED_PRODUCT_IS_ARTICLE_57", OperationType = GEMOperationType.Select)]
        public Int32? IsArticle57(Int32? product_PK)
        {
            DateTime methodStart = DateTime.Now;
            Int32? ds = null;

            try
            {
                // Generating order by clause

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (product_PK.HasValue) parameters.Add(new GEMDbParameter("product_PK", product_PK, DbType.Int32, ParameterDirection.Input));

                ds = (Int32?)base.ExecuteProcedureReturnScalar(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return ds;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AUTHORISED_PRODUCT_GetNextAlphabeticalEntity", OperationType = GEMOperationType.Select)]
        public Int32? GetNextAlphabeticalEntity(Int32? ap_PK) 
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = null;
            Int32? retApPK = null;
            try
            {
                // Generating order by clause

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("ap_PK", ap_PK, DbType.Int32, ParameterDirection.Input));

                ds = base.ExecuteProcedureReturnDataSet(parameters, out outputValues);

                if (ds != null && ds.Tables.Count != 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count != 0)
                    {

                        if (dt.Rows[0]["ap_PK"] != null)
                        {
                            retApPK = Convert.ToInt32(dt.Rows[0]["ap_PK"]);
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

            return retApPK;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AUTHORISED_PRODUCT_GetPrevAlphabeticalEntity", OperationType = GEMOperationType.Select)]
        public Int32? GetPrevAlphabeticalEntity(Int32? ap_PK)
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = null;
            Int32? retApPK = null;
            try
            {
                // Generating order by clause

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("ap_PK", ap_PK, DbType.Int32, ParameterDirection.Input));

                ds = base.ExecuteProcedureReturnDataSet(parameters, out outputValues);

                if (ds != null && ds.Tables.Count != 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count != 0)
                    {

                        if (dt.Rows[0]["ap_PK"] != null)
                        {
                            retApPK = Convert.ToInt32(dt.Rows[0]["ap_PK"]);
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

            return retApPK;
        }
        
        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AUTHORISED_PRODUCT_GetTabMenuItemsCount", OperationType = GEMOperationType.Select)]
        public DataSet GetTabMenuItemsCount(Int32 ap_PK, int? personFk)
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = null;

            try
            {
                // Generating order by clause

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("ap_PK", ap_PK, DbType.Int32, ParameterDirection.Input));
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AUTHORISED_PRODUCT_ProductsSearcher", OperationType = GEMOperationType.Select)]
        public DataSet AProductsSearcher(String name, String description, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
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
                if (description != null && description.ToString() != String.Empty) parameters.Add(new GEMDbParameter("description", description, DbType.String, ParameterDirection.Input));
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AUTHORISED_PRODUCT_AbleToDeleteEntity", OperationType = GEMOperationType.Select)]
        public bool AbleToDeleteEntity(int authorisedProductPk)
        {
            DateTime methodStart = DateTime.Now;
            var ableToDelete = false;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("authorisedProductPk", authorisedProductPk, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_AUTHORISED_PRODUCT_GetListFormDataSet]", OperationType = GEMOperationType.Select)]
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_AUTHORISED_PRODUCT_GetListFormSearchDataSet]", OperationType = GEMOperationType.Select)]
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AUTHORISED_PRODUCT_GetEntitiesByQppvCode", OperationType = GEMOperationType.Select)]
        public List<AuthorisedProduct> GetEntitiesByQppvCode(int qppvCodeFk)
        {
            var methodStart = DateTime.Now;
            List<AuthorisedProduct> entities = null;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("QppvCodeFk", qppvCodeFk, DbType.Int32, ParameterDirection.Input));
               
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AUTHORISED_PRODUCT_GetEntityByEVCode", OperationType = GEMOperationType.Select)]
        public AuthorisedProduct GetEntityByEVCode(String evcode)
        {
            var methodStart = DateTime.Now;
            AuthorisedProduct entity = null;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("EVCode", evcode, DbType.String, ParameterDirection.Input));

                entity = base.ExecuteProcedureReturnEntity(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entity;
        }

        #endregion

        #region ICRUDOperations<AuthorisedProduct> Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AUTHORISED_PRODUCT_GetEntity", OperationType = GEMOperationType.Select)]
        public override AuthorisedProduct GetEntity<PKType>(PKType entityId)
        {
            return base.GetEntity<PKType>(entityId);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AUTHORISED_PRODUCT_GetEntities", OperationType = GEMOperationType.Select)]
        public override List<AuthorisedProduct> GetEntities()
        {
            return base.GetEntities();
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AUTHORISED_PRODUCT_GetEntitiesWP", OperationType = GEMOperationType.Select)]
        public override List<AuthorisedProduct> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
        {
            return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AUTHORISED_PRODUCT_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
        public override List<AuthorisedProduct> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
        {
            return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AUTHORISED_PRODUCT_Save", OperationType = GEMOperationType.Save)]
        public override AuthorisedProduct Save(AuthorisedProduct entity)
        {
            return base.Save(entity);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AUTHORISED_PRODUCT_Delete", OperationType = GEMOperationType.Delete)]
        public override void Delete<PKType>(PKType entityId)
        {
            base.Delete<PKType>(entityId);
        }

        public override List<AuthorisedProduct> SaveCollection(List<AuthorisedProduct> entities)
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
