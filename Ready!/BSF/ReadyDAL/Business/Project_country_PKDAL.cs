// ======================================================================================================================
// Author:		Mateo-HP\Mateo
// Create date:	8.12.2011. 10:11:06
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PROJECT_COUNTRY_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Project_country_PKDAL : GEMDataAccess<Project_country_PK>, IProject_country_PKOperations
	{
		public Project_country_PKDAL() : base() { }
		public Project_country_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IProject_country_PKOperations Members
        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PROJECT_COUNTRY_MN_GetByProjectId", OperationType = GEMOperationType.Select)]
        public DataSet GetCountriesByProject(int projectID)
        {
            DataSet ds = null;
            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("ProjecId", projectID, DbType.Int32, ParameterDirection.Input));

                ds = base.ExecuteProcedureReturnDataSet(parameters, out outputValues);

            }

            catch (Exception ex)
            {
                base.HandleException(ex);
            }
            return ds;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PROJECT_COUNTRY_MN_DeleteByProjectId", OperationType = GEMOperationType.Delete)]
        public void deleteByProject(int projectID)
        {
            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (projectID != null) parameters.Add(new GEMDbParameter("projectId", projectID, DbType.Int32, ParameterDirection.Input));

                base.ExecuteProcedureReturnDataSet(parameters, out outputValues);

            }

            catch (Exception ex)
            {
                base.HandleException(ex);
            }
        }
		#endregion

		#region ICRUDOperations<Project_country_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PROJECT_COUNTRY_MN_GetEntity", OperationType = GEMOperationType.Select)]
		public override Project_country_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PROJECT_COUNTRY_MN_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Project_country_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PROJECT_COUNTRY_MN_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Project_country_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PROJECT_COUNTRY_MN_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Project_country_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PROJECT_COUNTRY_MN_Save", OperationType = GEMOperationType.Save)]
		public override Project_country_PK Save(Project_country_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PROJECT_COUNTRY_MN_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Project_country_PK> SaveCollection(List<Project_country_PK> entities)
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
