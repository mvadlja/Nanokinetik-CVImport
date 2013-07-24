// ======================================================================================================================
// Author:		Kiki-PC\Kiki
// Create date:	8/21/2012 2:16:17 PM
// Description:	GEM2 Generated class for table ready_billev_production.dbo.XEVPRM_ENTITY_DETAILS_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    [GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "XEVPRM_ENTITY_DETAILS_MN")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Xevprm_entity_details_mn_PK
	{
		private Int32? _xevprm_entity_details_mn_PK;
		private Int32? _xevprm_message_FK;
		private Int32? _xevprm_entity_details_FK;
		private Int32? _xevprm_entity_type_FK;
		private Int32? _xevprm_entity_FK;
		private Int32? _xevprm_operation_type;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? xevprm_entity_details_mn_PK
		{
			get { return _xevprm_entity_details_mn_PK; }
			set { _xevprm_entity_details_mn_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? xevprm_message_FK
		{
			get { return _xevprm_message_FK; }
			set { _xevprm_message_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? xevprm_entity_details_FK
		{
			get { return _xevprm_entity_details_FK; }
			set { _xevprm_entity_details_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? xevprm_entity_type_FK
		{
			get { return _xevprm_entity_type_FK; }
			set { _xevprm_entity_type_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? xevprm_entity_FK
		{
			get { return _xevprm_entity_FK; }
			set { _xevprm_entity_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? xevprm_operation_type
		{
			get { return _xevprm_operation_type; }
			set { _xevprm_operation_type = value; }
		}

        public XevprmEntityType XevprmEntityType
        {
            get { return xevprm_entity_type_FK != null && Enum.IsDefined(typeof(XevprmEntityType), xevprm_entity_type_FK) ? (XevprmEntityType)xevprm_entity_type_FK : XevprmEntityType.NULL; }
            set { xevprm_entity_type_FK = (int)value == 0 ? (int?)null : (int)value; }
        }

        public XevprmOperationType XevprmOperationType
        {
            get { return xevprm_operation_type != null && Enum.IsDefined(typeof(XevprmOperationType), xevprm_operation_type) ? (XevprmOperationType)xevprm_operation_type : XevprmOperationType.NULL; }
            set { xevprm_operation_type = (int)value == 0 ? (int?)null : (int)value; }
        }

		#endregion

		public Xevprm_entity_details_mn_PK() { }
		public Xevprm_entity_details_mn_PK(Int32? xevprm_entity_details_mn_PK, Int32? xevprm_message_FK, Int32? xevprm_entity_details_FK, Int32? xevprm_entity_type_FK, Int32? xevprm_entity_FK, Int32? xevprm_operation_type)
		{
			this.xevprm_entity_details_mn_PK = xevprm_entity_details_mn_PK;
			this.xevprm_message_FK = xevprm_message_FK;
			this.xevprm_entity_details_FK = xevprm_entity_details_FK;
			this.xevprm_entity_type_FK = xevprm_entity_type_FK;
			this.xevprm_entity_FK = xevprm_entity_FK;
			this.xevprm_operation_type = xevprm_operation_type;
		}
	}

	public interface IXevprm_entity_details_mn_PKOperations : ICRUDOperations<Xevprm_entity_details_mn_PK>
	{
        List<Xevprm_entity_details_mn_PK> GetEntitiesByXevprm(int? xevprm_message_PK);
	}
}
