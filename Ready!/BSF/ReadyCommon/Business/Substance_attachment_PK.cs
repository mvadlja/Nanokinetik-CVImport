// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	4.1.2012. 12:02:23
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.SUBSTANCE_ATTACHMENT
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "SUBSTANCE_ATTACHMENT")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Substance_attachment_PK
	{
		private Int32? _substance_attachment_PK;
		private String _attachmentreference;
		private Int32? _resolutionmode;
		private Int32? _validitydeclaration;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? substance_attachment_PK
		{
			get { return _substance_attachment_PK; }
			set { _substance_attachment_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String attachmentreference
		{
			get { return _attachmentreference; }
			set { _attachmentreference = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? resolutionmode
		{
			get { return _resolutionmode; }
			set { _resolutionmode = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? validitydeclaration
		{
			get { return _validitydeclaration; }
			set { _validitydeclaration = value; }
		}

		#endregion

		public Substance_attachment_PK() { }
		public Substance_attachment_PK(Int32? substance_attachment_PK, String attachmentreference, Int32? resolutionmode, Int32? validitydeclaration)
		{
			this.substance_attachment_PK = substance_attachment_PK;
			this.attachmentreference = attachmentreference;
			this.resolutionmode = resolutionmode;
			this.validitydeclaration = validitydeclaration;
		}
	}

	public interface ISubstance_attachment_PKOperations : ICRUDOperations<Substance_attachment_PK>
	{

        List<Substance_attachment_PK> GetSubAttByAs(int? as_PK);
    }
}
