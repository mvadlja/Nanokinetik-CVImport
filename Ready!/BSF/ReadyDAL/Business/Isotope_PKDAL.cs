// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 13:46:33
// Description:	GEM2 Generated class for table SSI.dbo.ISOTOPE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Isotope_PKDAL : GEMDataAccess<Isotope_PK>, IIsotope_PKOperations
	{
		public Isotope_PKDAL() : base() { }
		public Isotope_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IIsotope_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ISOTOPE_STRUCTURE_MN_GetISOByStructPK", OperationType = GEMOperationType.Select)]
        public List<Isotope_PK> GetISOByStructPK(Int32? StructurePK)
        {
            DateTime methodStart = DateTime.Now;
            List<Isotope_PK> entities = new List<Isotope_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (StructurePK != null) parameters.Add(new GEMDbParameter("StructPK", StructurePK, DbType.Int32, ParameterDirection.Input));

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

		#region ICRUDOperations<Isotope_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ISOTOPE_GetEntity", OperationType = GEMOperationType.Select)]
		public override Isotope_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ISOTOPE_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Isotope_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ISOTOPE_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Isotope_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ISOTOPE_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Isotope_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ISOTOPE_Save", OperationType = GEMOperationType.Save)]
		public override Isotope_PK Save(Isotope_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ISOTOPE_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Isotope_PK> SaveCollection(List<Isotope_PK> entities)
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
