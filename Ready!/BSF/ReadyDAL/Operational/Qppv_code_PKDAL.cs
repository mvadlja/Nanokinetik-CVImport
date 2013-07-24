// ======================================================================================================================
// Author:		ACER\Kiki
// Create date:	17.10.2011. 10:57:22
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PERSON
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    public class Qppv_code_PKDAL : GEMDataAccess<Qppv_code_PK>, IQppv_code_PKOperations
    {
        public Qppv_code_PKDAL() : base() { }
        public Qppv_code_PKDAL(string dataSourceId) : base(dataSourceId) { }

        #region IQppv_codePKOperations Members
        
        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_QPPV_CODE_GetQppvCodeByPerson", OperationType = GEMOperationType.Select)]
        public List<Qppv_code_PK> GetQppvCodeByPerson(int? person_PK)
        {
            DateTime methodStart = DateTime.Now;
            List<Qppv_code_PK> entities = new List<Qppv_code_PK>();

            try
            {

                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (person_PK != null) parameters.Add(new GEMDbParameter("person_FK", person_PK, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_QPPV_CODE_GetQppvByPersonCodeSearcher", OperationType = GEMOperationType.Select)]
        public DataSet GetQppvByPersonCodeSearcher(String personName, String qppv_code, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
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

                parameters.Add(new GEMDbParameter("name", personName, DbType.String, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("qppv_code", qppv_code, DbType.String, ParameterDirection.Input));
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_QPPV_CODE_GetLocalQppvByPersonRoleSearcher", OperationType = GEMOperationType.Select)]
        public DataSet GetLocalQppvByPersonRoleSearcher(String personName, String qppv_code, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
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

                parameters.Add(new GEMDbParameter("name", personName, DbType.String, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("qppv_code", qppv_code, DbType.String, ParameterDirection.Input));
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

        #endregion

        #region ICRUDOperations<Person_PK> Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_QPPV_CODE_GetEntity", OperationType = GEMOperationType.Select)]
        public override Qppv_code_PK GetEntity<PKType>(PKType entityId)
        {
            return base.GetEntity<PKType>(entityId);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_QPPV_CODE_GetEntities", OperationType = GEMOperationType.Select)]
        public override List<Qppv_code_PK> GetEntities()
        {
            return base.GetEntities();
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_QPPV_CODE_GetEntitiesWP", OperationType = GEMOperationType.Select)]
        public override List<Qppv_code_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
        {
            return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_QPPV_CODE_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
        public override List<Qppv_code_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
        {
            return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_QPPV_CODE_Save", OperationType = GEMOperationType.Save)]
        public override Qppv_code_PK Save(Qppv_code_PK entity)
        {
            return base.Save(entity);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_QPPV_CODE_Delete", OperationType = GEMOperationType.Delete)]
        public override void Delete<PKType>(PKType entityId)
        {
            base.Delete<PKType>(entityId);
        }

        public override List<Qppv_code_PK> SaveCollection(List<Qppv_code_PK> entities)
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
