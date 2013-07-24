using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using GEM2Common;
using CommonTypes;
using Ready.Model;
using System.Drawing;
using System.Data;
using System.Transactions;

namespace AspNetUI.Support
{
    public class SSIForm
    {

        #region Declarations
        static IIsotope_PKOperations _isotopeOperations;
        static ISubstance_code_PKOperations _substance_code_PKOperations;
        static IReference_source_PKOperations _reference_source_PKOperations;
        static IStructure_PKOperations _structureOperations;

        private static Dictionary<string, object> SSIToSave;
        private static List<Isotope_PK> isotopeList;
        private static List<Substance_code_PK> substanceCodeList;
        private static List<Reference_source_PK> referenceSourceList;
        private static List<Structure_PK> structureList;
        private static List<List<string>> Common;
        private static List<List<string>> Structure;
        private static List<Version_PK> versionList;
        private static List<Subst_clf_PK> sclfList;
        private static List<Substance_relationship_PK> subRelList;
        private static List<Gene_PK> geneList;
        private static List<Gene_element_PK> geneElementList;
        private static List<Target_PK> targetList;
        private static List<Subtype_PK> subtypeList;
        private static List<Reference_source_PK> refSourceList;
        private static List<Property_PK> propertyList;
        private static List<Moiety_PK> moietyList;
        private static List<Amount_PK> amountList;

        private static List<string> ddlCommon;
        private static List<string> sDCommon;
        private static string ctlLanguages = "";
        private static string ctlSubstanceClass = "";
        private static object EVCODESearcherDisplay = "";

        private static List<string> ddlStructure;
        private static List<string> txtStructure;
        private static List<string> fileStructure;
        private static string ctlStructRepresentation = "";
        private static string ctlOpticalActivity = "";
        private static object ctlMolecularFormula = "";
        private static string ctlStructRepresentationType = "";
        private static object ctlStereochemistry = "";
        private static Dictionary<int, List<Isotope_PK>> structureIsotop; // int - struct_id, list<int> list of iso ids
        private static Dictionary<int, List<Moiety_PK>> nsMoiety;
        private static Dictionary<int, List<Property_PK>> nsProperty;
        #endregion


        #region Dictionary operations
        /// <summary>
        /// Initializes memory container for SSIForm
        /// </summary>
        #region Initializations

        public static void InitializeMemoryContainer()
        {
            _isotopeOperations = new Isotope_PKDAL();
            _substance_code_PKOperations = new Substance_code_PKDAL();
            _reference_source_PKOperations = new Reference_source_PKDAL();
            _structureOperations = new Structure_PKDAL();

            SSIToSave = new Dictionary<string, object>();
            isotopeList = new List<Isotope_PK>();
            substanceCodeList = new List<Substance_code_PK>();
            referenceSourceList = new List<Reference_source_PK>();
            structureList = new List<Structure_PK>();
            structureIsotop = new Dictionary<int, List<Isotope_PK>>();
            versionList = new List<Version_PK>();
            sclfList = new List<Subst_clf_PK>();
            subRelList = new List<Substance_relationship_PK>();
            geneElementList = new List<Gene_element_PK>();
            geneList = new List<Gene_PK>();
            targetList = new List<Target_PK>();
            subtypeList = new List<Subtype_PK>();
            refSourceList = new List<Reference_source_PK>();

            // Common controls
            Common = new List<List<string>>();
            ddlCommon = new List<string>() { "ctlLanguages", "ctlSubstanceClass" };
            sDCommon = new List<string>() { "EVCODESearcherDisplay" };
            Common.Add(ddlCommon);
            Common.Add(sDCommon);
            AddPairToDictionary("ddlCommon", ddlCommon);
            AddPairToDictionary("sDCommon", sDCommon);
            AddPairToDictionary("ctlLanguages", ctlLanguages);
            AddPairToDictionary("ctlSubstanceClass", ctlSubstanceClass);
            AddPairToDictionary("EVCODESearcherDisplay", EVCODESearcherDisplay);
            AddPairToDictionary("Common", Common);

            // Structure controls
            Structure = new List<List<string>>();
            ddlStructure = new List<string>() { "ctlStructRepresentationType", "ctlStereochemistry" };
            txtStructure = new List<string>() { "ctlStructRepresentation", "ctlOpticalActivity", "ctlMolecularFormula" };
            fileStructure = new List<string>() { "lblUploadedFile" };
            Structure.Add(ddlStructure);
            Structure.Add(txtStructure);
            AddPairToDictionary("ddlStructure", ddlStructure);
            AddPairToDictionary("txtStructure", txtStructure);
            AddPairToDictionary("fileStructure", fileStructure);
            AddPairToDictionary("ctlStructRepresentationType", ctlStructRepresentationType);
            AddPairToDictionary("ctlStereochemistry", ctlStereochemistry);
            AddPairToDictionary("ctlStructRepresentation", ctlStructRepresentation);
            AddPairToDictionary("ctlOpticalActivity", ctlOpticalActivity);
            AddPairToDictionary("ctlMolecularFormula", ctlMolecularFormula);
            AddPairToDictionary("Isotope", isotopeList);
            AddPairToDictionary("Structure", Structure);

            AddPairToDictionary("listStructure", structureList);
            AddPairToDictionary("SubstanceCode", substanceCodeList);

        }

        private static void AddPairToDictionary(string key, object value)
        {
            if (!SSIToSave.ContainsKey(key)) SSIToSave.Add(key, value);
        }

        public static void AddIsoToStruct(int structID, List<Isotope_PK> isoList)
        {
            if (structureIsotop.ContainsKey(structID))
            {
                structureIsotop[structID].Clear();
                structureIsotop[structID].AddRange(isoList);
            }
            else
            {
                structureIsotop.Add(structID, isoList);
            }

        }

        public static List<Isotope_PK> GetIsotope(int structID)
        {
            return structureIsotop[structID];
        }
        #endregion


        //private static void AddContainerToDictionary(object container) {

        //    if (container.GetType() == typeof(List<List<string>>))
        //    {
        //        List<List<string>> contList = (List<List<string>>)container;
        //        foreach (List<string> list in contList)
        //        {
        //            AddContainerToDictionary(list);
        //        }
        //    }
        //    else if(container.GetType() == typeof(List<string>) {
        //        List<string> list = (List<string>)container;
        //        foreach (string item in list)
        //        {
        //            AddPairToDictionary(list, object);
        //        }
        //    } 
        //    else return;     
        //}

        /// <summary>
        /// Adds new item to dictionary collection
        /// </summary>
        /// <param name="item">Item to add</param>
        /// <param name="type">Item type</param>
        public static void AddItemToDictionary(object item, string type)
        {
            switch (type)
            {
                case "Isotope":
                    IsotopeList.Add((Isotope_PK)item);
                    break;
                case "SubstanceCode":
                    SubstanceCodeList.Add((Substance_code_PK)item);
                    break;
                case "ctlMolecularFormula":
                    SSIToSave["ctlMolecularFormula"] = (string)item;
                    break;
                case "ctlOpticalActivity":
                    SSIToSave["ctlOpticalActivity"] = (string)item;
                    break;
                case "ctlStereochemistry":
                    SSIToSave["ctlStereochemistry"] = (string)item;
                    break;
                case "ctlStructRepresentation":
                    SSIToSave["ctlStructRepresentation"] = (string)item;
                    break;
                case "ctlStructRepresentationType":
                    SSIToSave["ctlStructRepresentationType"] = (string)item;
                    break;
                case "Structure":
                    structureList.Add((Structure_PK)item);
                    break;
                case "Version":
                    versionList.Add((Version_PK)item);
                    break;
                case "Sclf":
                    sclfList.Add((Subst_clf_PK)item);
                    break;
                case "SubstanceRelationship":
                    subRelList.Add((Substance_relationship_PK)item);
                    break;
                case "Gene":
                    geneList.Add((Gene_PK)item);
                    break;
                case "GeneElement":
                    geneElementList.Add((Gene_element_PK)item);
                    break;
                case "Target":
                    targetList.Add((Target_PK)item);
                    break;
                case "Subtype":
                    subtypeList.Add((Subtype_PK)item);
                    break;
                case "ReferenceSource":
                    referenceSourceList.Add((Reference_source_PK)item);
                    break;

                // TODO : add other cases
            }
        }

        /// <summary>
        /// Retrieves a value from the dictionary
        /// </summary>
        /// <param name="key">Dictionary key</param>
        /// <returns>Dictionary value</returns>
        private static object GetValueFromDictionary(string key)
        {
            object value;
            SSIToSave.TryGetValue(key, out value);
            return value;
        }


        // TODO
        //public static object GetAllControls(string panel, string type) {

        //    GetValueFromDictionary(type);
        //}

        /// <summary>
        /// Adds new list
        /// </summary>
        /// <typeparam name="T">List type</typeparam>
        /// <param name="newList">List</param>
        /// <param name="listName">List name</param>
        public static void AddNewList<T>(List<T> newList, string listName)
        {
            switch (listName)
            {
                case "Isotope":
                    IsotopeList.Clear();
                    foreach (T item in newList)
                        IsotopeList.Add(item as Isotope_PK);
                    break;
                case "SubstanceCode":
                    SubstanceCodeList.Clear();
                    foreach (T item in newList)
                        SubstanceCodeList.Add(item as Substance_code_PK);
                    break;
                case "Structure":
                    structureList.Clear();
                    foreach (T item in newList)
                        structureList.Add(item as Structure_PK);
                    break;
                case "Version":
                    versionList.Clear();
                    foreach (T item in newList)
                        versionList.Add(item as Version_PK);
                    break;
                case "Sclf":
                    sclfList.Clear();
                    foreach (T item in newList)
                        sclfList.Add(item as Subst_clf_PK);
                    break;
                case "SubstanceRelationship":
                    subRelList.Clear();
                    foreach (T item in newList)
                        subRelList.Add(item as Substance_relationship_PK);
                    break;
                case "Gene":
                    geneList.Clear();
                    foreach (T item in newList)
                        geneList.Add(item as Gene_PK);
                    break;
                case "GeneElement":
                    geneElementList.Clear();
                    foreach (T item in newList)
                        geneElementList.Add(item as Gene_element_PK);
                    break;
                case "Target":
                    targetList.Clear();
                    foreach (T item in newList)
                        targetList.Add(item as Target_PK);
                    break;
                case "Subtype":
                    subtypeList.Clear();
                    foreach (T item in newList)
                        subtypeList.Add(item as Subtype_PK);
                    break;
                case "ReferenceSource":
                    referenceSourceList.Clear();
                    foreach (T item in newList)
                        referenceSourceList.Add(item as Reference_source_PK);
                    break;

            }

        }

        /// <summary>
        /// Saves entire dictionary collection to database
        /// </summary>
        public static void Save()
        {
            SaveStructures();
        }

        private static void SaveStructures()
        {
            foreach (var item in structureList)
            {
                _structureOperations.Save(item);
            }
            _isotopeOperations.SaveCollection(IsotopeList);
        }



        #endregion


        #region List properties

        public static List<Isotope_PK> IsotopeList
        {
            get { return (List<Isotope_PK>)GetValueFromDictionary("Isotope"); }
        }
        public static List<Substance_code_PK> SubstanceCodeList
        {
            get { return (List<Substance_code_PK>)GetValueFromDictionary("SubstanceCode"); }
        }

        public static List<Structure_PK> StructureList
        {
            get { return (List<Structure_PK>)GetValueFromDictionary("listStructure"); }
        }

        public static List<Version_PK> VersionList
        {
            get { return versionList; }
        }

        public static List<Subst_clf_PK> SclfList
        {
            get { return sclfList; }
        }

        public static List<Substance_relationship_PK> SubstanceRelationshipList
        {
            get { return subRelList; }
        }

        public static List<Gene_PK> GeneList
        {
            get { return geneList; }
        }

        public static List<Gene_element_PK> GeneElementList
        {
            get { return geneElementList; }
        }

        public static List<Target_PK> TargetList
        {
            get { return targetList; }
        }

        public static List<Subtype_PK> SubtypeList
        { get { return subtypeList; } }

        public static List<Reference_source_PK> ReferenceSourceList
        { get { return referenceSourceList; } }

        public static List<Amount_PK> AmountList
        {
            get { return amountList; }
        }

        public static List<Moiety_PK> MoietyList
        {
            get { return moietyList; }
        }

        public static List<Property_PK> PropertyList
        {
            get { return propertyList; }
        }
        #endregion

    }

}