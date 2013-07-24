// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	24.2.2012. 13:16:59
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PHARMACEUTICAL_PRODUCT_SAVED_SEARCH
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "PHARMACEUTICAL_PRODUCT_SAVED_SEARCH")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Pharmaceutical_product_saved_search_PK
	{
		private Int32? _pharmaceutical_products_PK;
		private String _name;
		private Int32? _responsible_user_FK;
		private String _description;
		private Int32? _product_FK;
		private Int32? _pharmform_FK;
        private String _administrationRoutes;
        private String _activeIngridients;
        private String _excipients;
        private String _adjuvants;
        private String _medicalDevices;
		private String _comments;
		private String _displayName;
		private Int32? _user_FK;
		private String _gridLayout;
		private Boolean? _isPublic;
        private String _booked_slots;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? pharmaceutical_products_PK
		{
			get { return _pharmaceutical_products_PK; }
			set { _pharmaceutical_products_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String name
		{
			get { return _name; }
			set { _name = value; }
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

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? product_FK
		{
			get { return _product_FK; }
			set { _product_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? Pharmform_FK
		{
			get { return _pharmform_FK; }
			set { _pharmform_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String comments
		{
			get { return _comments; }
			set { _comments = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String displayName
		{
			get { return _displayName; }
			set { _displayName = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? user_FK
		{
			get { return _user_FK; }
			set { _user_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String gridLayout
		{
			get { return _gridLayout; }
			set { _gridLayout = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
		public Boolean? isPublic
		{
			get { return _isPublic; }
			set { _isPublic = value; }
		}

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String administrationRoutes
        {
            get { return _administrationRoutes; }
            set { _administrationRoutes = value; }
        }
        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String activeIngridients
        {
            get { return _activeIngridients; }
            set { _activeIngridients = value; }
        }
        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String excipients
        {
            get { return _excipients; }
            set { _excipients = value; }
        }
        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String adjuvants
        {
            get { return _adjuvants; }
            set { _adjuvants = value; }
        }
        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String medical_devices
        {
            get { return _medicalDevices; }
            set { _medicalDevices = value; }
        }
        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String booked_slots
        {
            get { return _booked_slots; }
            set { _booked_slots = value; }
        }


		#endregion

		public Pharmaceutical_product_saved_search_PK() { }
        public Pharmaceutical_product_saved_search_PK(Int32? pharmaceutical_products_PK, String name, Int32? responsible_user_FK, String description, Int32? product_FK, Int32? pharmform_FK, String comments, String adminRoutes, String actIng, String excipients, String adjuvants, String medicalDev, String displayName, Int32? user_FK, String gridLayout, Boolean? isPublic, String booked_slots)
		{
			this.pharmaceutical_products_PK = pharmaceutical_products_PK;
			this.name = name;
			this.responsible_user_FK = responsible_user_FK;
			this.description = description;
			this.product_FK = product_FK;
			this.Pharmform_FK = pharmform_FK;
			this.comments = comments;
            this.displayName = displayName;
			this.user_FK = user_FK;
			this.gridLayout = gridLayout;
			this.isPublic = isPublic;
            this.administrationRoutes = adminRoutes;
            this.activeIngridients = actIng;
            this.excipients = excipients;
            this.adjuvants = adjuvants;
            this.medical_devices = medicalDev;
            this.booked_slots = booked_slots;
		}
	}

	public interface IPharmaceutical_product_saved_search_PKOperations : ICRUDOperations<Pharmaceutical_product_saved_search_PK>
    {
        List<Pharmaceutical_product_saved_search_PK> GetEntitiesByUserOrPublic(Int32? user_fk);
    }
}
