// ======================================================================================================================
// Author:		POSSIMUSIT-MATE\Mateo
// Create date:	20.3.2012. 15:28:02
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.XML_REPORT_MAPPING
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "XML_REPORT_MAPPING")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Xml_report_mapping_PK
	{
		private Int32? _xml_report_mapping_PK;
		private String _xml_tag;
		private String _display_tag;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? xml_report_mapping_PK
		{
			get { return _xml_report_mapping_PK; }
			set { _xml_report_mapping_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String xml_tag
		{
			get { return _xml_tag; }
			set { _xml_tag = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String display_tag
		{
			get { return _display_tag; }
			set { _display_tag = value; }
		}

		#endregion

		public Xml_report_mapping_PK() { }
		public Xml_report_mapping_PK(Int32? xml_report_mapping_PK, String xml_tag, String display_tag)
		{
			this.xml_report_mapping_PK = xml_report_mapping_PK;
			this.xml_tag = xml_tag;
			this.display_tag = display_tag;
		}
	}

	public interface IXml_report_mapping_PKOperations : ICRUDOperations<Xml_report_mapping_PK>
	{

	}
}
