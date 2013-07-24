// ======================================================================================================================
// Author:		ACER\Kiki
// Create date:	3.11.2011. 13:08:15
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PP_ACTIVE_INGREDIENT
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    public class QuickLinkDAL : GEMDataAccess<QuickLink>, IQuickLinkOperations
	{
		public QuickLinkDAL() : base() { }
        public QuickLinkDAL(string dataSourceId) : base(dataSourceId) { }

		#region IActiveingredient_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_QuickLink_GetEntitiesByUserOrPublic", OperationType = GEMOperationType.Select)]
        public DataSet GetEntitiesByUserOrPublic(int userPk)
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = null;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("UserPk", userPk, DbType.Int32, ParameterDirection.Input));

                ds = base.ExecuteProcedureReturnDataSet(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return ds;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_QuickLink_GetPublicEntities", OperationType = GEMOperationType.Select)]
        public DataSet GetPublicEntities()
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = null;

            try
            {
                var parameters = new List<GEMDbParameter>();

                ds = base.ExecuteProcedureReturnDataSet(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return ds;
        }

		#endregion

        #region ICRUDOperations<QuickLink> Members

        #endregion
    }
}
