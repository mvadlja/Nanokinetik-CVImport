// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	21.10.2011. 12:08:56
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PHARMACEUTICAL_FORM
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Pharmaceutical_form_PKDAL : GEMDataAccess<Pharmaceutical_form_PK>, IPharmaceutical_form_PKOperations
	{
		public Pharmaceutical_form_PKDAL() : base() { }
		public Pharmaceutical_form_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IPharmaceutical_form_PKOperations Members



		#endregion

		#region ICRUDOperations<Pharmaceutical_form_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PHARMACEUTICAL_FORM_GetEntity", OperationType = GEMOperationType.Select)]
		public override Pharmaceutical_form_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PHARMACEUTICAL_FORM_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Pharmaceutical_form_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PHARMACEUTICAL_FORM_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Pharmaceutical_form_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PHARMACEUTICAL_FORM_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Pharmaceutical_form_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PHARMACEUTICAL_FORM_Save", OperationType = GEMOperationType.Save)]
		public override Pharmaceutical_form_PK Save(Pharmaceutical_form_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PHARMACEUTICAL_FORM_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Pharmaceutical_form_PK> SaveCollection(List<Pharmaceutical_form_PK> entities)
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
