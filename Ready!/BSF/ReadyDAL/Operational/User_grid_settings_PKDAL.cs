// ======================================================================================================================
// Author:		Acer\Kiki
// Create date:	20.2.2012. 14:51:38
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.USER_GRID_SETTINGS
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class User_grid_settings_PKDAL : GEMDataAccess<User_grid_settings_PK>, IUser_grid_settings_PKOperations
	{
		public User_grid_settings_PKDAL() : base() { }
		public User_grid_settings_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IUser_grid_settings_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_GRID_SETTINGS_GetDefaultLayoutByUsernameAndGrid", OperationType = GEMOperationType.Select)]
        public User_grid_settings_PK GetDefaultLayoutByUsernameAndGrid(string username, string grid_ID)
        {
            DateTime methodStart = DateTime.Now;
            User_grid_settings_PK entity = null;

            try
            {
                // Input parameters validation
                if (String.IsNullOrWhiteSpace(username) ||
                    String.IsNullOrWhiteSpace(grid_ID))
                    return entity;

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("username", username, DbType.String, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("grid_ID", grid_ID, DbType.String, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_GRID_SETTINGS_GetLayoutsByUsernameAndGrid", OperationType = GEMOperationType.Select)]
        public List<User_grid_settings_PK> GetLayoutsByUsernameAndGrid(string username, string grid_ID)
        {
            DateTime methodStart = DateTime.Now;
            List<User_grid_settings_PK> entities = new List<User_grid_settings_PK>();

            try
            {
                // Input parameters validation
                if (String.IsNullOrWhiteSpace(username) || 
                    String.IsNullOrWhiteSpace(grid_ID)) 
                    return entities;

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("username", username, DbType.String, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("grid_ID", grid_ID, DbType.String, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_GRID_SETTINGS_SetDefaultAndKeepFirstNLayouts", OperationType = GEMOperationType.Complex)]
        public void SetDefaultAndKeepFirstNLayouts(string username, string grid_ID, Int32? default_ugs_PK, Int32 num_to_keep)
        {
            DateTime methodStart = DateTime.Now;

            try
            {
                // Input parameters validation
                if (String.IsNullOrWhiteSpace(username) ||
                    String.IsNullOrWhiteSpace(grid_ID))
                    return;

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("username", username, DbType.String, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("grid_ID", grid_ID, DbType.String, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("default_ugs_PK", default_ugs_PK, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("num_to_keep", num_to_keep, DbType.Int32, ParameterDirection.Input));

                base.ExecuteProcedureReturnScalar(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_GRID_SETTINGS_DeleteLayoutsByUsernameAndGrid", OperationType = GEMOperationType.Delete)]
        public void DeleteLayoutsByUsernameAndGrid(string username, string grid_ID)
        {
            DateTime methodStart = DateTime.Now;

            try
            {
                // Input parameters validation
                if (String.IsNullOrWhiteSpace(username) || String.IsNullOrWhiteSpace(grid_ID)) return;
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("username", username, DbType.String, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("grid_ID", grid_ID, DbType.String, ParameterDirection.Input));

                base.ExecuteProcedureReturnEntity(parameters);
                
                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }


        }
		#endregion

		#region ICRUDOperations<User_grid_settings_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_GRID_SETTINGS_GetEntity", OperationType = GEMOperationType.Select)]
		public override User_grid_settings_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_GRID_SETTINGS_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<User_grid_settings_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_GRID_SETTINGS_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<User_grid_settings_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_GRID_SETTINGS_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<User_grid_settings_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_GRID_SETTINGS_Save", OperationType = GEMOperationType.Save)]
		public override User_grid_settings_PK Save(User_grid_settings_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_GRID_SETTINGS_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<User_grid_settings_PK> SaveCollection(List<User_grid_settings_PK> entities)
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
