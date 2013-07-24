// ======================================================================================================================
// Author:		BRUNO-NOTEBOOK\Bruno
// Create date:	20.2.2011. 9:57:27
// Description:	GEM2 Generated class for table Kmis.dbo.Downtimes
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Kmis.Model
{
	public class DowntimeDAL : GEMDataAccess<Downtime>, IDowntimeOperations
	{
		public DowntimeDAL() : base() { }
		public DowntimeDAL(string dataSourceId) : base(dataSourceId) { }

		#region IDowntimeOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_Downtimes_GetCurrentActiveDowntimesByCountryID", OperationType = GEMOperationType.Select)]
        public List<Downtime> GetCurrentActiveDowntimesByCountryID(Int32 countryID)
        {
            DateTime methodStart = DateTime.Now;
            List<Downtime> entities = new List<Downtime>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("CountryID", countryID, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_Downtimes_GetEntitiesByCountryID", OperationType = GEMOperationType.Select)]
        public List<Downtime> GetEntitiesByCountryID(Int32 countryID, int pageNumber, int pageSize, out int totalRecordsCount)
        {
            DateTime methodStart = DateTime.Now;
            List<Downtime> entities = new List<Downtime>();
            totalRecordsCount = 0;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("CountryID", countryID, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("PageNum", pageNumber, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("PageSize", pageSize, DbType.Int32, ParameterDirection.Input));

                entities = base.ExecuteProcedureReturnEntities(parameters, out outputValues);
                totalRecordsCount = (int)outputValues["totalRecordsCount"];

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

		#region ICRUDOperations<Downtime> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_Downtimes_GetEntity", OperationType = GEMOperationType.Select)]
		public override Downtime GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_Downtimes_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Downtime> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_Downtimes_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Downtime> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_Downtimes_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Downtime> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_Downtimes_Save", OperationType = GEMOperationType.Save)]
		public override Downtime Save(Downtime entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_Downtimes_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Downtime> SaveCollection(List<Downtime> entities)
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
