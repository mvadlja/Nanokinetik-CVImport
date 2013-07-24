// ======================================================================================================================
// Author:		Tomo-PC\Tomo
// Create date:	6.9.2012. 10:14:18
// Description:	GEM2 Generated class for table ready_dev.dbo.MA_MA_ENTITY_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    [Serializable()]
    [GEMAuditing(DataSourceId = "Default", Database = "ready_dev", Table = "MA_MA_ENTITY_MN")]
    [GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
    public class Ma_ma_entity_mn_PK
    {
        private Int32? _ma_ma_entity_mn_PK;
        private Int32? _ma_FK;
        private Int32? _ma_entity_FK;
        private Int32? _ma_entity_type_FK;

        #region Properties

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? ma_ma_entity_mn_PK
        {
            get { return _ma_ma_entity_mn_PK; }
            set { _ma_ma_entity_mn_PK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? ma_FK
        {
            get { return _ma_FK; }
            set { _ma_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? ma_entity_FK
        {
            get { return _ma_entity_FK; }
            set { _ma_entity_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? ma_entity_type_FK
        {
            get { return _ma_entity_type_FK; }
            set { _ma_entity_type_FK = value; }
        }

        #endregion

        public Ma_ma_entity_mn_PK() { }
        public Ma_ma_entity_mn_PK(Int32? ma_ma_entity_mn_PK, Int32? ma_FK, Int32? ma_entity_FK, Int32? ma_entity_type_FK)
        {
            this.ma_ma_entity_mn_PK = ma_ma_entity_mn_PK;
            this.ma_FK = ma_FK;
            this.ma_entity_FK = ma_entity_FK;
            this.ma_entity_type_FK = ma_entity_type_FK;
        }
    }

    public interface IMa_ma_entity_mn_PKOperations : ICRUDOperations<Ma_ma_entity_mn_PK>
    {

    }

    public enum MAEntityType
    {
        NULL,
        AuthorisedProduct,
        Attachment,
        XevprmMessage,
        Product,
        PharmaceuticalProduct,
        ActiveIngredient,
        Excipient,
        Adjuvant,
        Meddra,
        Document,
        Organization,
        QppvCode,
        Person
    }
}
