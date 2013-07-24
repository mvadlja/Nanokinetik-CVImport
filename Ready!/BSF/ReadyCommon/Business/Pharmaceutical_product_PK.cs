// ======================================================================================================================
// Author:		ACER\Kiki
// Create date:	24.10.2011. 11:54:40
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PHARMACEUTICAL_PRODUCT
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    [GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "PHARMACEUTICAL_PRODUCT", Active=true)]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Pharmaceutical_product_PK
	{
        public enum CalculatedColumn
        {
            All,
            Products,
            ActiveSubstances,
            Excipients,
            Adjuvants,
            AdministrationRoutes,
            MedicalDevices
        }

		private Int32? _pharmaceutical_product_PK;
		private String _name;
        private String _iD;
		private Int32? _responsible_user_FK;
		private String _description;
		private String _comments;
		private Int32? _pharmform_FK;
        private String _booked_slots;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? pharmaceutical_product_PK
		{
			get { return _pharmaceutical_product_PK; }
			set { _pharmaceutical_product_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String name
		{
			get { return _name; }
			set { _name = value; }
		}

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String ID
		{
			get { return _iD; }
			set { _iD = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? responsible_user_FK
		{
			get { return _responsible_user_FK; }
			set { _responsible_user_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String description
		{
			get { return _description; }
			set { _description = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String comments
		{
			get { return _comments; }
			set { _comments = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? Pharmform_FK
		{
			get { return _pharmform_FK; }
			set { _pharmform_FK = value; }
		}
        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String booked_slots
        {
            get { return _booked_slots; }
            set { _booked_slots = value; }
        }

		#endregion

		public Pharmaceutical_product_PK() { }
		public Pharmaceutical_product_PK(Int32? pharmaceutical_product_PK, String name, String iD, Int32? responsible_user_FK, String description, String comments, Int32? pharmform_FK, String booked_slots)
		{
			this.pharmaceutical_product_PK = pharmaceutical_product_PK;
			this.name = name;
			this.ID = iD;
			this.responsible_user_FK = responsible_user_FK;
			this.description = description;
			this.comments = comments;
			this.Pharmform_FK = pharmform_FK;
            this.booked_slots = booked_slots;
		}
	}

	public interface IPharmaceutical_product_PKOperations : ICRUDOperations<Pharmaceutical_product_PK>
	{
        DataSet GetPPSearcher(String name, String concise,int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        DataSet GetTabMenuItemsCount(Int32 pp_PK);

        Int32? GetNextAlphabeticalEntity(Int32? pp_PK);
        Int32? GetPrevAlphabeticalEntity(Int32? pp_PK);

        List<Pharmaceutical_product_PK> GetEntitiesByProduct(int productPk);
	    DataSet GetEntitiesFullNameByProduct(int productPk);
	    bool AbleToDeleteEntity(int pharmaceuticalProductPk);
        DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        DataSet GetListFormSearchDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        void UpdateCalculatedColumn(int pharmaceuticalProductPk, Pharmaceutical_product_PK.CalculatedColumn calculatedColumn);
        void UpdateCalculatedColumnByProduct(int productPk, Pharmaceutical_product_PK.CalculatedColumn calculatedColumn);
	}

}
