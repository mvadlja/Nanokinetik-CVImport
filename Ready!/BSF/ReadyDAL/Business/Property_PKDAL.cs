// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 13:49:00
// Description:	GEM2 Generated class for table SSI.dbo.PROPERTY
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Property_PKDAL : GEMDataAccess<Property_PK>, IProperty_PKOperations
	{
		public Property_PKDAL() : base() { }
		public Property_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IProperty_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_NS_PROPERTY_MN_GetPropertyByNonStoPK", OperationType = GEMOperationType.Select)]
        public List<Property_PK> GetPropertyByNonStoPK(Int32? NonStoPK)
        {
            DateTime methodStart = DateTime.Now;
            List<Property_PK> entities = new List<Property_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (NonStoPK != null) parameters.Add(new GEMDbParameter("NonStoPK", NonStoPK, DbType.Int32, ParameterDirection.Input));

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

		#region ICRUDOperations<Property_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PROPERTY_GetEntity", OperationType = GEMOperationType.Select)]
		public override Property_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PROPERTY_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Property_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PROPERTY_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Property_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PROPERTY_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Property_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PROPERTY_Save", OperationType = GEMOperationType.Save)]
		public override Property_PK Save(Property_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PROPERTY_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Property_PK> SaveCollection(List<Property_PK> entities)
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
