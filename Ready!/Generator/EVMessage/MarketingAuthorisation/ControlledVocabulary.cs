using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ready.Model;

namespace EVMessage.MarketingAuthorisation
{
    public class ControlledVocabulary
    {
        #region Declarations

        private static ControlledVocabulary _instance;

        #region Ready model DAL

        private IOrganization_PKOperations _organization_PKOperations;
        private ICountry_PKOperations _country_PKOperations;
        private IType_PKOperations _type_PKOperations;
        private ILanguagecode_PKOperations _languagecode_PKOperations;
        private IAtc_PKOperations _atc_PKOperations;
        private IPharmaceutical_form_PKOperations _pharmaceutical_form_PKOperations;
        private ISubstance_PKOperations _substance_PKOperations;
        private ISsi__cont_voc_PKOperations _ssi__cont_voc_PKOperations;
        private IAdminroute_PKOperations _adminroute_PKOperations;
        private IMedicaldevice_PKOperations _medicaldevice_PKOperations;
        private IQppv_code_PKOperations _qppv_code_PKOperations;

        #endregion

        #region Controlled vocabullary

        private List<Type_PK> _typeList;
        private List<Type_PK> _attachmentFileTypeList;
        private List<Type_PK> _attachmentVersionNumberList;
        private List<Type_PK> _attachmentTypeList;
        private List<Languagecode_PK> _languageCodeList;
        private List<Country_PK> _countryList;
        private List<Type_PK> _authorisationProcedureList;
        private List<Type_PK> _authorisationStatusList;
        private List<Atc_PK> _atcList;
        private List<Type_PK> _meddraVersionList;
        private List<Type_PK> _meddraLevelList;

        private List<Ssi__cont_voc_PK> _prefixList;
        private List<Ssi__cont_voc_PK> _numeratorUnitList;
        private List<Ssi__cont_voc_PK> _denominatorMeasureUnitList;
        private List<Ssi__cont_voc_PK> _unitList;
        private List<Ssi__cont_voc_PK> _denominatorPresentationUnitList;
        private List<Ssi__cont_voc_PK> _concentrationTypeList;

        private List<Ssi__cont_voc_PK> _expressedByList;
        private Ssi__cont_voc_PK _expressedByUnitsOfMeasure;
        private Ssi__cont_voc_PK _expressedByUnitsOfPresentation;
        private List<Substance_PK> _substanceList;
        private List<Pharmaceutical_form_PK> _pharmaceuticalFormList;
        private List<Medicaldevice_PK> _medicalDeviceList;
        private List<Adminroute_PK> _adminRouteList;

        #endregion

        #endregion

        #region Properties

        public static ControlledVocabulary Instance
        {
            get { return _instance ?? (_instance = new ControlledVocabulary()); }
        }

        public List<Type_PK> TypeList
        {
            get { return _typeList; }
            set { _typeList = value; }
        }

        public List<Type_PK> AttachmentFileTypeList
        {
            get { return _attachmentFileTypeList; }
            set { _attachmentFileTypeList = value; }
        }

        public List<Type_PK> AttachmentVersionNumberList
        {
            get { return _attachmentVersionNumberList; }
            set { _attachmentVersionNumberList = value; }
        }

        public List<Type_PK> AttachmentTypeList
        {
            get { return _attachmentTypeList; }
            set { _attachmentTypeList = value; }
        }

        public List<Languagecode_PK> LanguageCodeList
        {
            get { return _languageCodeList; }
            set { _languageCodeList = value; }
        }

        public List<Country_PK> CountryList
        {
            get { return _countryList; }
            set { _countryList = value; }
        }

        public List<Type_PK> AuthorisationProcedureList
        {
            get { return _authorisationProcedureList; }
            set { _authorisationProcedureList = value; }
        }

        public List<Type_PK> AuthorisationStatusList
        {
            get { return _authorisationStatusList; }
            set { _authorisationStatusList = value; }
        }

        public List<Atc_PK> AtcList
        {
            get { return _atcList; }
            set { _atcList = value; }
        }

        public List<Type_PK> MeddraVersionList
        {
            get { return _meddraVersionList; }
            set { _meddraVersionList = value; }
        }

        public List<Type_PK> MeddraLevelList
        {
            get { return _meddraLevelList; }
            set { _meddraLevelList = value; }
        }

        public List<Ssi__cont_voc_PK> PrefixList
        {
            get { return _prefixList; }
            set { _prefixList = value; }
        }

        public List<Ssi__cont_voc_PK> NumeratorUnitList
        {
            get { return _numeratorUnitList; }
            set { _numeratorUnitList = value; }
        }

        public List<Ssi__cont_voc_PK> DenominatorMeasureUnitList
        {
            get { return _denominatorMeasureUnitList; }
            set { _denominatorMeasureUnitList = value; }
        }

        public List<Ssi__cont_voc_PK> UnitList
        {
            get { return _unitList; }
            set { _unitList = value; }
        }

        public List<Ssi__cont_voc_PK> DenominatorPresentationUnitList
        {
            get { return _denominatorPresentationUnitList; }
            set { _denominatorPresentationUnitList = value; }
        }

        public List<Ssi__cont_voc_PK> ConcentrationTypeList
        {
            get { return _concentrationTypeList; }
            set { _concentrationTypeList = value; }
        }

        public List<Ssi__cont_voc_PK> ExpressedByList
        {
            get { return _expressedByList; }
            set { _expressedByList = value; }
        }

        public Ssi__cont_voc_PK ExpressedByUnitsOfMeasure
        {
            get { return _expressedByUnitsOfMeasure; }
            set { _expressedByUnitsOfMeasure = value; }
        }

        public Ssi__cont_voc_PK ExpressedByUnitsOfPresentation
        {
            get { return _expressedByUnitsOfPresentation; }
            set { _expressedByUnitsOfPresentation = value; }
        }

        public List<Substance_PK> SubstanceList
        {
            get { return _substanceList; }
            set { _substanceList = value; }
        }

        public List<Pharmaceutical_form_PK> PharmaceuticalFormList
        {
            get { return _pharmaceuticalFormList; }
            set { _pharmaceuticalFormList = value; }
        }

        public List<Medicaldevice_PK> MedicalDeviceList
        {
            get { return _medicalDeviceList; }
            set { _medicalDeviceList = value; }
        }

        public List<Adminroute_PK> AdminRouteList
        {
            get { return _adminRouteList; }
            set { _adminRouteList = value; }
        }

        #endregion

        #region Constructors

        private ControlledVocabulary()
        {
            InitDAL();
            InitCV();
        }

        #endregion

        #region Methods

        private void InitDAL()
        {
            _organization_PKOperations = new Organization_PKDAL();
            _country_PKOperations = new Country_PKDAL();
            _type_PKOperations = new Type_PKDAL();
            _languagecode_PKOperations = new Languagecode_PKDAL();
            _atc_PKOperations = new Atc_PKDAL();
            _pharmaceutical_form_PKOperations = new Pharmaceutical_form_PKDAL();
            _substance_PKOperations = new Substance_PKDAL();
            _ssi__cont_voc_PKOperations = new Ssi__cont_voc_PKDAL();
            _adminroute_PKOperations = new Adminroute_PKDAL();
            _medicaldevice_PKOperations = new Medicaldevice_PKDAL();
            _qppv_code_PKOperations = new Qppv_code_PKDAL();
        }

        public void InitCV()
        {
            _typeList = _type_PKOperations.GetEntities();
            _attachmentFileTypeList = TypeList.Where(type => type.group.Trim() == "AT" && !string.IsNullOrWhiteSpace(type.type)).ToList();
            _attachmentVersionNumberList = TypeList.Where(type => type.group.Trim() == "PPIVD" && !string.IsNullOrWhiteSpace(type.name)).ToList();
            _attachmentTypeList = TypeList.Where(type => type.group.Trim() == "DT" && !string.IsNullOrWhiteSpace(type.ev_code)).ToList();
            _languageCodeList = _languagecode_PKOperations.GetEntities().Where(languageCode => !string.IsNullOrWhiteSpace(languageCode.code)).ToList();
            _countryList = _country_PKOperations.GetEntities().Where(country => !string.IsNullOrWhiteSpace(country.abbreviation)).ToList();

            _authorisationProcedureList = TypeList.FindAll(type => type.group == "TP" && !string.IsNullOrWhiteSpace(type.ev_code));
            _authorisationStatusList = TypeList.FindAll(type => type.group == "AS" && !string.IsNullOrWhiteSpace(type.ev_code));
            _atcList = _atc_PKOperations.GetEntities().Where(atc => !string.IsNullOrWhiteSpace(atc.atccode)).ToList();
            _meddraVersionList = TypeList.FindAll(meddraVersion => meddraVersion.group == "MV" && !string.IsNullOrWhiteSpace(meddraVersion.name));
            _meddraLevelList = TypeList.FindAll(meddraLevel => meddraLevel.group == "ML" && !string.IsNullOrWhiteSpace(meddraLevel.name));

            _prefixList = _ssi__cont_voc_PKOperations.GetPrefixes().Where(prefix => !string.IsNullOrWhiteSpace(prefix.term_name_english)).ToList();
            _numeratorUnitList = _ssi__cont_voc_PKOperations.GetEntitiesByListName("Units of Measure").Where(unit => !string.IsNullOrWhiteSpace(unit.term_name_english)).ToList();
            _denominatorMeasureUnitList = _ssi__cont_voc_PKOperations.GetEntitiesByListName("Measure_lower").Where(unit => !string.IsNullOrWhiteSpace(unit.term_name_english)).ToList();
            _denominatorPresentationUnitList = _ssi__cont_voc_PKOperations.GetEntitiesByListName("Units of Presentation").Where(unit => !string.IsNullOrWhiteSpace(unit.term_name_english)).ToList();

            _unitList = new List<Ssi__cont_voc_PK>();
            _unitList.AddRange(NumeratorUnitList);
            _unitList.AddRange(DenominatorMeasureUnitList);
            _unitList.AddRange(DenominatorPresentationUnitList);

            _concentrationTypeList = _ssi__cont_voc_PKOperations.GetConcentrationTypes().Where(concType => !string.IsNullOrWhiteSpace(concType.Evcode)).ToList();

            _expressedByList = _ssi__cont_voc_PKOperations.GetEntitiesByListName("ExpressedBy");
            _expressedByUnitsOfMeasure = ExpressedByList.Find(item => item.term_name_english != null && item.term_name_english.ToLower().Contains("measure"));
            _expressedByUnitsOfPresentation = ExpressedByList.Find(item => item.term_name_english != null && item.term_name_english.ToLower().Contains("presentation"));
            _substanceList = _substance_PKOperations.GetEntities().Where(substance => !string.IsNullOrWhiteSpace(substance.ev_code)).ToList();
            _pharmaceuticalFormList = _pharmaceutical_form_PKOperations.GetEntities().Where(pharmaceticalForm => !string.IsNullOrWhiteSpace(pharmaceticalForm.ev_code)).ToList();

            _adminRouteList = _adminroute_PKOperations.GetEntities().Where(adminRoute => !string.IsNullOrWhiteSpace(adminRoute.ev_code)).ToList();
            _medicalDeviceList = _medicaldevice_PKOperations.GetEntities().Where(medicalDevice => !string.IsNullOrWhiteSpace(medicalDevice.ev_code)).ToList();
        }

        #endregion
    }
}
