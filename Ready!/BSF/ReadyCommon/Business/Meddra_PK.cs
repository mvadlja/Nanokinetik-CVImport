// ======================================================================================================================
// Author:		space-monkey\dpetek
// Create date:	14.3.2012. 9:34:32
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.MEDDRA
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    //[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "MEDDRA")]
    //[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Meddra_pk
	{
		private Int32? _meddra_pk;
		private Int32? _version_type_FK;
		private Int32? _level_type_FK;
		private String _code;
        private String _term;
        private String _meddraFullName;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? meddra_pk
		{
			get { return _meddra_pk; }
			set { _meddra_pk = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? version_type_FK
		{
			get { return _version_type_FK; }
			set { _version_type_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? level_type_FK
		{
			get { return _level_type_FK; }
			set { _level_type_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String code
		{
			get { return _code; }
			set { _code = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String term
		{
			get { return _term; }
			set { _term = value; }
		}

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
	    public string MeddraFullName
	    {
	        get { return _meddraFullName; }
	        set { _meddraFullName = value; }
	    }

	    #endregion

		public Meddra_pk() { }
		public Meddra_pk(Int32? meddra_pk, Int32? version_type_FK, Int32? level_type_FK, String code, String term, string meddraFullName = "")
		{
			this.meddra_pk = meddra_pk;
			this.version_type_FK = version_type_FK;
			this.level_type_FK = level_type_FK;
			this.code = code;
			this.term = term;
		    this.MeddraFullName = meddraFullName;
		}

        public void UpdateFullName(IType_PKOperations typeOperations)
        {
            var meddraVersion = typeOperations.GetEntity(this.version_type_FK);
            var meddraLevel = typeOperations.GetEntity(this.level_type_FK);
            var meddraCode = this.code;
            var meddraTerm = this.term;

            var meddraVersionPart = meddraVersion != null && !string.IsNullOrWhiteSpace(meddraVersion.name) ? meddraVersion.name : string.Empty;
            var meddraLevelPart = meddraLevel != null && !string.IsNullOrWhiteSpace(meddraLevel.name) ? meddraLevel.name : string.Empty;

            this.MeddraFullName = GetFormatedText(meddraVersionPart, meddraLevelPart, meddraCode, meddraTerm, string.Empty);
        }

        private static string GetFormatedText(string meddraVersionPart, string meddraLevelPart, string meddraCodePart, string meddraTermPart, string defaultEmptyValue)
        {
            if (string.IsNullOrWhiteSpace(meddraVersionPart) &&
                string.IsNullOrWhiteSpace(meddraLevelPart) &&
                string.IsNullOrWhiteSpace(meddraCodePart) &&
                string.IsNullOrWhiteSpace(meddraTermPart))
            {
                return defaultEmptyValue;
            }
            return "<" + meddraVersionPart + ">, " + meddraLevelPart + ", " + meddraCodePart + (!string.IsNullOrWhiteSpace(meddraTermPart) ? ", " + meddraTermPart : string.Empty);
        }
	}

	public interface IMeddra_pkOperations : ICRUDOperations<Meddra_pk>
	{
        List<Meddra_pk> GetMeddraByAp(int? authorisedProductFk);
	}
}
