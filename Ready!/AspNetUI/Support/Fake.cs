using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetUI.Support
{
    public class Fake
    {
        private static Random rnd = new Random();
        private static List<string> countries = new List<string>() { "", "HU, RO", "BE", "BG", "CY", "CZ", "EE", "EU", "FI", "FR", "DE", "EL", "FI", "HU" };
        private static List<string> countriesDDL = new List<string>() { "", "AT", "BE", "BG", "CY", "CZ", "EE", "EU", "FI", "FR", "DE", "EL", "FI", "HU" };
        private static List<string> countries2 = new List<string>() { "", "HU", "RO", "BG", "CY", "CZ", "EE", "EU", "FI", "FR", "DE", "EL", "FI", "HU" };
        private static List<string> authStatus = new List<string>() { "", "Approved", "Approved", "Approved", "Withdrawn", "Rejected" };
        private static List<string> authProcedure = new List<string>() { "", "mutual" };
        private static List<string> authProcedure2 = new List<string>() { "", "Centralised", "National", "Mutual recognition", "Decentralised" };
        private static List<string> MAH_names = new List<string>() { "", "Krka", "Krka", "Medika", "Belupo", "Bayer", "Oktal Pharma", "Roche", "Mecrk", "Eisai", "Amgen", "Pfizer", "Wyeth", "Eli Lily", "Novartis" };
        private static List<string> shortNames = new List<string>() { "", "Atenolol", "Gestodelle", "Amoxicillin", "Calciject", "Gestodelle", "Amoxicillin", "Atenolol", "Gestodelle", "Amoxicillin", "Atenolol", "Gestodelle", "Calciject", "Atenolol", "Gestodelle", "Calciject", "Atenolol", "Gestodelle", "Amoxicillin", "Calciject" };
        private static List<string> legalStatus = new List<string>() { "", "Rx", "OTC" };
        private static List<string> responsibleUsers = new List<string>() { "", "User1", "User2", "User3", "User4" };
        private static List<string> comments = new List<string>() { "", "", "This is another comment", "And one more comment" };
        private static List<string> descpriptions = new List<string>() { "", "Description numero due", "This is first description", "Description for this cure", "Description for cancer cure" };
        private static List<string> pharmaProducts = new List<string>() { "", "<a href='PRODUCTView.aspx?tab=2'>1</a>&nbsp;<a href='PRODUCTView.aspx?tab=2&e=n'>Add</a>", "<a href='PRODUCTView.aspx?tab=2'>2</a>&nbsp;<a href='PRODUCTView.aspx?tab=2&e=n'>Add</a>", "<a href='PRODUCTView.aspx?tab=2'>8</a>&nbsp;<a href='PRODUCTView.aspx?tab=2&e=n'>Add</a>", "<a href='PRODUCTView.aspx?tab=2'>1</a>&nbsp;<a href='PRODUCTView.aspx?tab=2&e=n'>Add</a>" };
        private static List<string> authProduct = new List<string>() { "", "<a href='PRODUCTView.aspx?tab=1'>2</a>&nbsp;<a href='PRODUCTView.aspx?tab=1&e=n'>Add</a>", "<a href='PRODUCTView.aspx?tab=1'>1</a>&nbsp;<a href='PRODUCTView.aspx?tab=1&e=n'>Add</a>", "<a href='PRODUCTView.aspx?tab=1'>2</a>&nbsp;<a href='PRODUCTView.aspx?tab=1&e=n'>Add</a>", "<a href='PRODUCTView.aspx?tab=1'>1</a>&nbsp;<a href='PRODUCTView.aspx?tab=1&e=n'>Add</a>" };
        private static List<string> ingredientStrength = new List<string>() { "", "VENLAFAXINE HYDROCHLORIDE / 75mg; CAPSULES", "aripiprazol / 5; Orodispersible tablet", "adrenalin / 10; Solution for injection", "oxytetracycline dihydrate / 10; Suspension for injection", "alendronate sodium / 12; Tablet" };
        private static List<string> indications = new List<string>() { "", "Rash", "MedDRA", "Redness", "Redness", "Itching", "Redness", "Rash", "Itching", "Rash", "Itching", "Redness", "Rash", "Itching", "Itching", "Redness", "Rash", "Itching", "Redness" };
        private static List<string> types = new List<string>() { "", "Active ingredient-CEP", "Active ingredient-ASMF", "Advanced theraphy medical product", "Herbal medical product", "Homeopatic medical product", "Human medicine", "Imunological medicinal product", "Medicinal product derived from human blood or human plasma", "Radiopharmaceutical", "Traditional herbal medicinal product" };
        private static List<string> domains = new List<string>() { "", "Cosmetic", "Food supplement", "Human medicine", "Medical device", "N/A", "Veterinary" };
        private static List<string> documents = new List<string>() { "", "<a href='PRODUCTView.aspx?tab=3'>3</a>&nbsp;<a href='PRODUCTView.aspx?tab=3&e=n'>Add</a>", "0&nbsp;<a href='PRODUCTView.aspx?tab=3&e=n'>Add</a>", "0&nbsp;<a href='PRODUCTView.aspx?tab=3&e=n'>Add</a>", "0&nbsp;<a href='PRODUCTView.aspx?tab=3&e=n'>Add</a>" };
        private static List<string> EV_CODES = new List<string>() {"", "5875", "4567", "8799", "7899", "4617", "8977", "1324", "2345", "5342", "8362", "7294", "3468", "2372", "1252", "7428", "1474" };
        private static List<string> xEVPRM_status = new List<string>() { "", "internal", "approved", "internal", "internal", "approved", "internal", "internal", "approved", "approved", "internal", "internal", "internal", "internal" };
        private static List<string> authNumbers = new List<string>() { "", "BE283997", "147788", "14723", "14712", "46217", "BE283972", "32046", "32432", "234222", "23434", "23439", "BE283997", "12322", "23423", "12322", "23332" };
        private static List<string> dates = new List<string>() { "", "2011-09-22", "2011-09-30", "2011-09-18", "2011-09-22", "2011-09-22", "2011-07-20", "2011-09-12", "2011-02-2", "2011-08-9", "2011-09-30", "2011-10-02", "2011-06-04", "2011-08-07", "2011-01-05", "2011-08-19", "2011-05-31" };
        private static List<string> parentProduct = new List<string>() { "", "Venlafaxine 75 mg prolonged release capsules", "Venlafaxine 75 mg prolonged release capsules" };

        
        public Fake() { }

        public static string ParentProduct(int index)
        {
            if (index < parentProduct.Count)
                return parentProduct[index];
            else
                return "";
        }

        public static string Country(int index)
        {
            if (index < countries.Count)
                return countries[index];
            else
                return "";
        }

        public static string Country2(int index)
        {
            if (index < countries2.Count)
                return countries2[index];
            else
                return "";
        }

        public static string Pharma(int index)
        {
            if (index < pharmaProducts.Count)
                return pharmaProducts[index];
            else
                return "0&nbsp;<a href='PRODUCTView.aspx?tab=2&e=n'>Add</a>";
        }

        public static string AuthProduct(int index)
        {
            if (index < authProduct.Count)
                return authProduct[index];
            else
                return "0&nbsp;<a href='PRODUCTView.aspx?tab=1&e=n'>Add</a>";
        }

        public static string Type(int index)
        {
            if (index < types.Count)
                return types[index];
            else
                return "Pill";
        }

        public static string Domain(int index)
        {
            if (index < domains.Count)
                return domains[index];
            else
                return "Veterinary";
        }

        public static string Indications(int index)
        {
            if (index < indications.Count)
                return indications[index];
            else
                return "Redness";
        }

        public static string IngredientStrength(int index)
        {
            if (index < ingredientStrength.Count)
                return ingredientStrength[index];
            else
                return "alendronate sodium / 12; Tablet";
        }

        public static string AuthStatus(int index)
        {
            if (index < authStatus.Count)
                return authStatus[index];
            else
                return "Centralised";
        }

        public static string AuthProcedure(int index)
        {
            if (index < authProcedure.Count)
                return authProcedure[index];
            else
                return "Centralised";
        }

        public static string AuthProcedure2(int index)
        {
            if (index < authProcedure2.Count)
                return authProcedure2[index];
            else
                return "Centralised";
        }

        public static string EV_CODE(int index)
        {
            //if (index < EV_CODES.Count)
            //    return EV_CODES[index];
            //else
                return "-";
        }

        public static string MAH_name(int index)
        {
            if (index < MAH_names.Count)
                return MAH_names[index];
            else
                return "Krka";
        }

        public static string XEVPRM_status(int index)
        {
            //if (index < xEVPRM_status.Count)
            //    return xEVPRM_status[index];
            //else
                return "-";
        }

        public static string ShortName(int index)
        {
            if (index < shortNames.Count)
                return shortNames[index];
            else
                return "Amoxicillin";
        }

        public static string AuthNumber(int index)
        {
            if (index < authNumbers.Count)
                return authNumbers[index];
            else
                return "11433";
        }

        public static string GetDate(int index)
        {
            if (index < dates.Count)
                return dates[index];
            else
                return "2011-07-18";
        }

        public static string Documents(int index)
        {
            if (index < documents.Count)
                return documents[index];
            else
                return "0&nbsp;<a href='PRODUCTView.aspx?tab=3&e=n'>Add</a>";
        }

        public static string Comments(int index)
        {
            if (index < comments.Count)
                return comments[index];
            else 
                return "This is another comment";
        }

        public static string Descriptions(int index)
        {
            if (index < descpriptions.Count)
                return descpriptions[index];
            else 
                return "This is another description";
        }




        public static List<string> Types
        {
            get { return types; }
        }

        //public static List<string> Domain
        //{
        //    get { return domain; }
        //}

        public static List<string> CountryList
        {
            get { return countriesDDL; }
        }

        public static List<string> ResponsibleUsers
        {
            get { return responsibleUsers; }
        }

        public static List<string> LegalStatus
        {
            get { return legalStatus; }
        }

        public static List<string> AuthStatusList
        {
            get { return authStatus; }
        }

        public static List<string> DomainList
        {
            get { return domains; }
        }

        public static List<string> AuthProcedure2List
        {
            get { return authProcedure2; }
        }

    
    }
}