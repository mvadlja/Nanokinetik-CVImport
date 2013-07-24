namespace AspNetUI.Views
{
    public enum Permission
    {
        None,

        EditMy, 
        SaveAsMy,
        DeleteMy,
        View,
        ViewComradeTab,
        DownloadAttachment,
       
        EditAlerter,
        DeleteAlerter,
        InsertAlerter,
        ShowAlertToggleButton,

        ChangePassword,

        CalculateBillingRate,


        CreateCustomerTimeReport,
        CreateCustomerTimeReportMy,
        CreateRegulatoryTimeReport,
        CreatePharmacovigilanceQualityReportMy,
        CreatePharmacovigilanceQualityReport,


        CreateManualAlerts,
        CreateManualAlertsMy,


        QuickLinkCreate,
        QuickLinkDelete,
        QuickLinkShowBar,


        MakeApArticle57Relevant,


        XevprmInsert,
        XevprmUpdate,
        XevprmVariation,
        XevprmNullify,
        XevprmWithdrawn,
        XevprmDelete,
        XevprmAdminDelete,
        XevprmSubmit,
        XevprmResubmit,
        XevprmAbort,
        XevprmShowXmlFile,
        XevprmShowPdfFile,
        XevprmShowRtfFile,
        XevprmShowFormAttributes,


        AS2ServiceShowStatus,
        AS2ServiceStartStop,
        AttachmentDownload,


        InsertAuthorisedProduct,
        EditAuthorisedProduct,
        SaveAsAuthorisedProduct,
        DeleteAuthorisedProduct,


        InsertProduct,
        EditProduct, 
        SaveAsProduct,
        DeleteProduct,


        InsertPharmaceuticalProduct,
        EditPharmaceuticalProduct,
        SaveAsPharmaceuticalProduct,
        DeletePharmaceuticalProduct,
           

        InsertSubmissionUnit,
        EditSubmissionUnit,
        SaveAsSubmissionUnit,
        DeleteSubmissionUnit,


        InsertProject,
        EditProject,
        SaveAsProject,
        DeleteProject,

     
        InsertActivity,
        EditActivity,
        SaveAsActivity,
        DeleteActivity,


        InsertTask,
        EditTask,
        SaveAsTask,
        DeleteTask,
       

        InsertTimeUnit,
        EditTimeUnit,
        SaveAsTimeUnit,
        DeleteTimeUnit,


        InsertDocument,
        EditDocument,
        SaveAsDocument,
        DeleteDocument,


        InsertPerson,
        EditPerson,
        EditPersonSecurity,
        DeletePerson,


        InsertOrganisation,
        EditOrganisation,
        DeleteOrganisation,


        InsertType,
        EditType,
        DeleteType,


        InsertAtc,
        EditAtc,
        DeleteAtc,


        InsertSubstance,
        EditSubstance,
        DeleteSubstance,


        InsertApprovedSubstance,
        EditApprovedSubstance,
        DeleteApprovedSubstance,


        InsertTaskName,
        EditTaskName,
        DeleteTaskName,


        InsertTimeUnitName,
        EditTimeUnitName,
        DeleteTimeUnitName,


        InsertUserRole,
        EditUserRole,
        DeleteUserRole,


        InsertUserAction,
        EditUserAction,
        DeleteUserAction,

        LinkFromEDMS,
        AuthProdAndProdAdditionalAttributes
    }

    public enum PageType
    {
        Other,
        Form,
        List,
        Preview,
        FormOld,
        ListOld,
        PreviewOld,
        Unknown
    }

    public enum EntityContext
    {
        Unknown,
        Default,
        Product,
        AuthorisedProduct,
        PharmaceuticalProduct,
        SubmissionUnit,
        Project,
        ActivityMy,
        Activity,
        Task,
        TimeUnitMy,
        TimeUnit,
        Document,
        Person,
        Organisation,
        Type,
        User,
        TimeUnitName,
        TaskName,
        ApprovedSubstance,
        Substance,
        Atc,
        Alerter,
        ReceivedMessagesStats,
        SentMessagesStats,
        UserRole,
        UserAction,
        UserSecurity,
        AddNewToProductForm,
        As2HandlerLog,
        MaServiceStats,
        ServiceLogStats,
        CustomerTimeReport,
        RegulatoryActivityReport,
        ActivityTimeReport,
        UserAccount,
        Xevprm
    }

    public enum ListType
    {
        Unknown, 
        List,
        Search
    }

    public enum FormType
    {
        Unknown,
        New,
        Edit,
        SaveAs
    }

    public enum PreviewType
    {
        Preview
    }

    public enum SortType
    {
        Asc,
        Desc
    }

    public enum SequenceStatus
    {
        Valid,
        Invalid,
        SequenceAlreadyExist,
        ProductSetAlreadyExist,
        NewSequence,
        OtherType,
        NotZipFile,
        ZipFilenameDontMatchSequence,
        ZipFilenameDontMatchRootFolder,
        ZippedFileContainsMultipleRootFolders,
        InvalidSequenceZipStructure,
        ErrorReadingZipFile,
        ErrorSavingFileToDb,
        IndexXmlDontExist,
        CtdTocPdfDontExist,
        ZipRootIsNotDirectory,
        NullReference,
        CheckForProductSet,
        NotWorkingSequence,
        FilenameWithoutExtension
    }

    public enum SequenceType
    {
        NeeS,
        eCTD,
        Other
    }

    public static class Constant
    {
        public const string UnknownValue = "N/A";
        public const string MissingValue = "Missing";
        public const string DefaultEmptyValue = "-";
        public const string DateTimeFormat = "dd.MM.yyyy";
        public const string NoCaption = "<No caption>";

        public static class PersonRoleName
        {
            public const string ResponsibleUser = "Responsible user";
            public const string Qppv = "QPPV";
            public const string LocalQppv = "Local QPPV";
        }

        public static class OrganizationRoleName
        {
            public const string Client = "Client";
            public const string ClientGroup = "Client Group";
            public const string LocalRepresentative = "Local Representative";
            public const string LicenceHolder = "Licence Holder";
            public const string LicenceHolderGroup = "Licence Holder Group";
            public const string MasterFileLocation = "Master File Location";
            public const string Manufacturer = "Manufacturer";
            public const string Partner = "Partner";
            public const string Applicant = "Applicant";
            public const string Agency = "Agency";
        }

        public static class TypeGroupName
        {
            public const string AuthorisationProcedure = "TP";
            public const string Type = "PR";
            public const string Types = "A";
            public const string InternalStatus = "IS";
            public const string Manufacturer = "MT";
            public const string Partner = "PT";
            public const string MeddraVersion = "MV";
            public const string MeddraLevel = "ML";
            public const string RegulatoryStatus = "APAS";
            public const string ActivityMode = "AM";
            public const string SubmissionUnitFormat = "SF";
            public const string SubmissionUnitDescription = "SD";
            public const string DtdSchemaVersion = "DTD";
            public const string DocumentType = "DT";
            public const string VersionLabel = "VL";
            public const string RegulatoryStatusDocuments = "RS";
            public const string PPIRegulatoryStatusDocuments = "PPIRS";
            public const string OrganizationType = "OR";
            public const string VersionNumber = "VD";
            public const string PPIVersionNumber = "PPIVD";
            public const string AttachmentType = "AT";
            public const string AttachmentTypeEDMS = "ATEDMS";
            public const string PersonStatus = "PS";
            public const string AuthorisationStatus = "AS";
            public const string Region = "RG";
            public const string StorageConditions = "SC";
            public const string PackagingMaterial = "PM";
            public const string PVQPerformanceIndicators = "PVQPI";
        }

        public static class ControlDefault
        {
            public const string DdlText = "- Choose -";
            public const string DdlValue = "";
            public const string LbPrvText = "-";
        }

        public static class DropDownList
        {
            public const string Value = "Value";
        }

        public static class ListBoxSr
        {
            public const string Value = "Value";
        }

        public static class ListBoxExt
        {
            public const string Value = "Value";
        }

        public static class ListBox
        {
            public const string Value = "Value";
        }

        public static class Countries
        {
            public const string DisplayNameFormat = "{0} - {1}||abbreviation,name";
        }

        public static class Atc
        {
            public const string DisplayNameFormat = "{0}({1})||atccode,name";
        }

        public static class CommandArgument
        {
            public const string Select = "Select";
            public const string Delete = "Delete";
            public const string Edit = "Edit";
            public const string Download = "Download";
        }

        public static class SSIListName
        {
            public const string SubstanceClass = "Substance Class";
        }

        public static class UserAction
        {
            public const string Insert = "Insert";
            public const string Edit = "Edit";
            public const string SaveAs = "SaveAs";
            public const string Delete = "Delete";
            public const string View = "View";
        }
    }
}