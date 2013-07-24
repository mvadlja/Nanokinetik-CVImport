// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	9.11.2011. 10:32:08
// Description:	GEM2 Generated class for table SSI.dbo.STRUCT_REPRESENTATION_TYPE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Struct_repres_type_PKDAL : GEMDataAccess<Struct_repres_type_PK>, IStruct_repres_type_PKOperations
	{
		public Struct_repres_type_PKDAL() : base() { }
		public Struct_repres_type_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IStruct_repres_type_PKOperations Members



		#endregion

		#region ICRUDOperations<Struct_repres_type_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_STRUCT_REPRESENTATION_TYPE_GetEntity", OperationType = GEMOperationType.Select)]
		public override Struct_repres_type_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_STRUCT_REPRESENTATION_TYPE_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Struct_repres_type_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_STRUCT_REPRESENTATION_TYPE_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Struct_repres_type_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_STRUCT_REPRESENTATION_TYPE_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Struct_repres_type_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_STRUCT_REPRESENTATION_TYPE_Save", OperationType = GEMOperationType.Save)]
		public override Struct_repres_type_PK Save(Struct_repres_type_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_STRUCT_REPRESENTATION_TYPE_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Struct_repres_type_PK> SaveCollection(List<Struct_repres_type_PK> entities)
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
