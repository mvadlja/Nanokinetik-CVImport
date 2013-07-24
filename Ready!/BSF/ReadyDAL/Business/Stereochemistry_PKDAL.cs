// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	9.11.2011. 10:31:47
// Description:	GEM2 Generated class for table SSI.dbo.STEREOCHEMISTRY
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Stereochemistry_PKDAL : GEMDataAccess<Stereochemistry_PK>, IStereochemistry_PKOperations
	{
		public Stereochemistry_PKDAL() : base() { }
		public Stereochemistry_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IStereochemistry_PKOperations Members



		#endregion

		#region ICRUDOperations<Stereochemistry_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_STEREOCHEMISTRY_GetEntity", OperationType = GEMOperationType.Select)]
		public override Stereochemistry_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_STEREOCHEMISTRY_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Stereochemistry_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_STEREOCHEMISTRY_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Stereochemistry_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_STEREOCHEMISTRY_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Stereochemistry_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_STEREOCHEMISTRY_Save", OperationType = GEMOperationType.Save)]
		public override Stereochemistry_PK Save(Stereochemistry_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_STEREOCHEMISTRY_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Stereochemistry_PK> SaveCollection(List<Stereochemistry_PK> entities)
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
