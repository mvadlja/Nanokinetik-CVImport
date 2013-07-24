// ======================================================================================================================
// Author:		KRISTIJAN-HPDV7\Kristijan
// Create date:	18.2.2013. 13:18:45
// Description:	GEM2 Generated class for table ReadyDevRBAC.dbo.LOCATION
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Location_PKDAL : GEMDataAccess<Location_PK>, ILocation_PKOperations
	{
		public Location_PKDAL() : base() { }
		public Location_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region ILocation_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_LOCATION_GetEntityByUniqueName", OperationType = GEMOperationType.Select)]
        public Location_PK GetEntityByUniqueName(string uniqueName)
        {
            var methodStart = DateTime.Now;
            Location_PK location = null;
            try
            {
                var parameters = new List<GEMDbParameter>();
                parameters.Add(new GEMDbParameter("uniqueName", uniqueName, DbType.String, ParameterDirection.Input));

                location = base.ExecuteProcedureReturnEntity(parameters);

                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return location;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_LOCATION_GetUserPermissions", OperationType = GEMOperationType.Select)]
        public List<Location_PK> GetUserPermissions(string username)
        {
            var methodStart = DateTime.Now;
            List<Location_PK> entities = null;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("Username", username, DbType.String, ParameterDirection.Input));

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

		#region ICRUDOperations<Location_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_LOCATION_GetEntity", OperationType = GEMOperationType.Select)]
		public override Location_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_LOCATION_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Location_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_LOCATION_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Location_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_LOCATION_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Location_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_LOCATION_Save", OperationType = GEMOperationType.Save)]
		public override Location_PK Save(Location_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_LOCATION_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Location_PK> SaveCollection(List<Location_PK> entities)
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
