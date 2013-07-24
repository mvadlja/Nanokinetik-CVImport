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
	public class Xml_report_mapping_PKDAL : GEMDataAccess<Xml_report_mapping_PK>, IXml_report_mapping_PKOperations
	{
		public Xml_report_mapping_PKDAL() : base() { }
		public Xml_report_mapping_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IXml_report_mapping_PKOperations Members



		#endregion

		#region ICRUDOperations<Xml_report_mapping_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XML_REPORT_MAPPING_GetEntity", OperationType = GEMOperationType.Select)]
		public override Xml_report_mapping_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XML_REPORT_MAPPING_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Xml_report_mapping_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XML_REPORT_MAPPING_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Xml_report_mapping_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XML_REPORT_MAPPING_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Xml_report_mapping_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XML_REPORT_MAPPING_Save", OperationType = GEMOperationType.Save)]
		public override Xml_report_mapping_PK Save(Xml_report_mapping_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XML_REPORT_MAPPING_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Xml_report_mapping_PK> SaveCollection(List<Xml_report_mapping_PK> entities)
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
