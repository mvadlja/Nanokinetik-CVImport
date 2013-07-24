// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	21.10.2011. 11:56:22
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PP_MD_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Pp_md_mn_PKDAL : GEMDataAccess<Pp_md_mn_PK>, IPp_md_mn_PKOperations
	{
		public Pp_md_mn_PKDAL() : base() { }
		public Pp_md_mn_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IPp_md_mn_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_MD_MN_GetMedDevicesByPPPK", OperationType = GEMOperationType.Select)]
        public List<Pp_md_mn_PK> GetMedDevicesByPPPK(Int32? pharmaceutical_product_FK)
        {
            DateTime methodStart = DateTime.Now;
            List<Pp_md_mn_PK> entities = new List<Pp_md_mn_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (pharmaceutical_product_FK != null) parameters.Add(new GEMDbParameter("pharmaceutical_product_FK", pharmaceutical_product_FK, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_MD_MN_DeleteByPharmaceuticalProduct", OperationType = GEMOperationType.Select)]
        public void DeleteByPharmaceuticalProduct(int pharmaceuticalProductPk)
        {
            DateTime methodStart = DateTime.Now;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("PharmaceuticalProductPk", pharmaceuticalProductPk, DbType.Int32, ParameterDirection.Input));

                base.ExecuteProcedureReturnScalar(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }
        }

		#endregion

		#region ICRUDOperations<Pp_md_mn_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_MD_MN_GetEntity", OperationType = GEMOperationType.Select)]
		public override Pp_md_mn_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_MD_MN_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Pp_md_mn_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_MD_MN_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Pp_md_mn_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_MD_MN_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Pp_md_mn_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_MD_MN_Save", OperationType = GEMOperationType.Save)]
		public override Pp_md_mn_PK Save(Pp_md_mn_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PP_MD_MN_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Pp_md_mn_PK> SaveCollection(List<Pp_md_mn_PK> entities)
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
