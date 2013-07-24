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
	[Serializable()]
	[GEMOperationsLogging(DataSourceId = "Default", Active = false)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class QuickLink
	{
        private Int32? _quickLinkId;
	    private String _name;
	    private bool? _isPublic;

        #region Properties

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? QuickLinkId
        {
            get { return _quickLinkId; }
            set { _quickLinkId = value; }
        }

	    public string Name
	    {
	        get { return _name; }
	        set { _name = value; }
	    }

	    public bool? IsPublic
	    {
	        get { return _isPublic; }
	        set { _isPublic = value; }
	    }

	    #endregion

        public QuickLink() { }
	}

    public interface IQuickLinkOperations : ICRUDOperations<QuickLink>
	{
        DataSet GetEntitiesByUserOrPublic(int userPk);
        DataSet GetPublicEntities();
	}
}
