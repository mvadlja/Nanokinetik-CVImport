// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 14:45:44
// Description:	GEM2 Generated class for table SSI.dbo.SUBTYPE_SCLF_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Subtype_sclf_mn_PKDAL : GEMDataAccess<Subtype_sclf_mn_PK>, ISubtype_sclf_mn_PKOperations
	{
		public Subtype_sclf_mn_PKDAL() : base() { }
		public Subtype_sclf_mn_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region ISubtype_sclf_mn_PKOperations Members



		#endregion

		#region ICRUDOperations<Subtype_sclf_mn_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBTYPE_SCLF_MN_GetEntity", OperationType = GEMOperationType.Select)]
		public override Subtype_sclf_mn_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBTYPE_SCLF_MN_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Subtype_sclf_mn_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBTYPE_SCLF_MN_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Subtype_sclf_mn_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBTYPE_SCLF_MN_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Subtype_sclf_mn_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBTYPE_SCLF_MN_Save", OperationType = GEMOperationType.Save)]
		public override Subtype_sclf_mn_PK Save(Subtype_sclf_mn_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SUBTYPE_SCLF_MN_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Subtype_sclf_mn_PK> SaveCollection(List<Subtype_sclf_mn_PK> entities)
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
