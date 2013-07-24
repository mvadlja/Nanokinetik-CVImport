// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	9.11.2011. 11:56:20
// Description:	GEM2 Generated class for table SSI.dbo.STRUCT_REPRES_ATTACHMENT
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Struct_repres_attach_PKDAL : GEMDataAccess<Struct_repres_attach_PK>, IStruct_repres_attach_PKOperations
	{
		public Struct_repres_attach_PKDAL() : base() { } 
		public Struct_repres_attach_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IStruct_repres_attach_PKOperations Members



		#endregion

		#region ICRUDOperations<Struct_repres_attach_PK> Members
        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "STRUCT_REPRES_ATTACHMENT_DeleteNULLByUserID", OperationType = GEMOperationType.Delete)]
        public void DeleteNULLByUserID(Int32? userID)
        {
            DateTime methodStart = DateTime.Now;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("UserID", userID, DbType.Int32, ParameterDirection.Input));

                base.ExecuteProcedureReturnScalar(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }
        }

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_STRUCT_REPRES_ATTACHMENT_GetEntity", OperationType = GEMOperationType.Select)]
		public override Struct_repres_attach_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_STRUCT_REPRES_ATTACHMENT_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Struct_repres_attach_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_STRUCT_REPRES_ATTACHMENT_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Struct_repres_attach_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_STRUCT_REPRES_ATTACHMENT_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Struct_repres_attach_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_STRUCT_REPRES_ATTACHMENT_Save", OperationType = GEMOperationType.Save)]
		public override Struct_repres_attach_PK Save(Struct_repres_attach_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_STRUCT_REPRES_ATTACHMENT_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Struct_repres_attach_PK> SaveCollection(List<Struct_repres_attach_PK> entities)
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
