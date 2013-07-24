using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using AspNetUI.Support;

namespace AspNetUI.Model.XlsImport
{
    public class XlsAuthorisedProduct
    {
        // Reference to Product
        public string ProductNumber { get; set; }
        public string RegistrationNumber { get; set; }

        public string Status { get; set; }
        public string NameOfProduct { get; set; }
        public string Strength { get; set; }
        public string ProductDescription { get; set; }
        public string LegalStatus { get; set; }
        public string LicenceHolder { get; set; }
        public string LicenceHolderGroup { get; set; }
        public string ReservationConfirmed { get; set; }
        public string ReservedTo { get; set; }
        public string Country { get; set; }
        public string ProcedureType { get; set; }
        public string LocalCodes { get; set; }
        public string PRApproval { get; set; }

        public DateTime? DeliveryDateToCustomer { get; set; }
        public DateTime? EstimatedSubmissionDate { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public DateTime? Day0 { get; set; }
        public DateTime? EstimatedProcedureApprovalDate { get; set; }
        public DateTime? ProcedureApprovalDate { get; set; }
        public DateTime? SubmissionPhaseNational { get; set; }
        public DateTime? EstimatedNationalApprovalDate { get; set; }
        public DateTime? NationalApprovalDate { get; set; }
        public DateTime? EffectivelyMarketedDate { get; set; }
        public DateTime? SunsetClauseDate { get; set; }
        public DateTime? PatentExpire { get; set; }
        public DateTime? SubmissionOfRenewalDate { get; set; }
        public DateTime? RenewalDate { get; set; }

        public string Comments { get; set; }
        public string PackSize { get; set; }
        public string ShelfLife { get; set; }

        public string Serizalize()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<Item ");
            XMLSerializationHelper.AppendIfNotNull(sb, "ProductNumber", ProductNumber == null ? null : ProductNumber.Trim());
            XMLSerializationHelper.AppendIfNotNull(sb, "RegistrationNumber", RegistrationNumber == null ? null : RegistrationNumber.Trim());

            XMLSerializationHelper.AppendIfNotNull(sb, "Status", Status == null ? null : Status.Trim());
            XMLSerializationHelper.AppendIfNotNull(sb, "NameOfProduct", NameOfProduct == null ? null : NameOfProduct.Trim());
            XMLSerializationHelper.AppendIfNotNull(sb, "Strength", Strength == null ? null : Strength.Trim());
            XMLSerializationHelper.AppendIfNotNull(sb, "ProductDescription", ProductDescription == null ? null : ProductDescription.Trim());
            XMLSerializationHelper.AppendIfNotNull(sb, "LegalStatus", LegalStatus == null ? null : LegalStatus.Trim());
            XMLSerializationHelper.AppendIfNotNull(sb, "LicenceHolder", LicenceHolder == null ? null : LicenceHolder.Trim());
            XMLSerializationHelper.AppendIfNotNull(sb, "LicenceHolderGroup", LicenceHolderGroup == null ? null : LicenceHolderGroup.Trim());
            XMLSerializationHelper.AppendIfNotNull(sb, "ReservationConfirmed", ReservationConfirmed == null ? null : ReservationConfirmed.Trim());
            XMLSerializationHelper.AppendIfNotNull(sb, "ReservedTo", ReservedTo == null ? null : ReservedTo.Trim());
            XMLSerializationHelper.AppendIfNotNull(sb, "Country", Country == null ? null : Country.Trim());
            XMLSerializationHelper.AppendIfNotNull(sb, "ProcedureType", ProcedureType == null ? null : ProcedureType.Trim());
            XMLSerializationHelper.AppendIfNotNull(sb, "LocalCodes", LocalCodes == null ? null : LocalCodes.Trim());
            XMLSerializationHelper.AppendIfNotNull(sb, "PRApproval", PRApproval == null ? null : PRApproval.Trim());

            XMLSerializationHelper.AppendDateIfNotNull(sb, "DeliveryDateToCustomer", DeliveryDateToCustomer);
            XMLSerializationHelper.AppendDateIfNotNull(sb, "EstimatedSubmissionDate", EstimatedSubmissionDate);
            XMLSerializationHelper.AppendDateIfNotNull(sb, "SubmissionDate", SubmissionDate);
            XMLSerializationHelper.AppendDateIfNotNull(sb, "Day0", Day0);
            XMLSerializationHelper.AppendDateIfNotNull(sb, "EstimatedProcedureApprovalDate", EstimatedProcedureApprovalDate);
            XMLSerializationHelper.AppendDateIfNotNull(sb, "ProcedureApprovalDate", ProcedureApprovalDate);
            XMLSerializationHelper.AppendDateIfNotNull(sb, "SubmissionPhaseNational", SubmissionPhaseNational);
            XMLSerializationHelper.AppendDateIfNotNull(sb, "EstimatedNationalApprovalDate", EstimatedNationalApprovalDate);
            XMLSerializationHelper.AppendDateIfNotNull(sb, "NationalApprovalDate", NationalApprovalDate);
            XMLSerializationHelper.AppendDateIfNotNull(sb, "EffectivelyMarketedDate", EffectivelyMarketedDate);
            XMLSerializationHelper.AppendDateIfNotNull(sb, "SunsetClauseDate", SunsetClauseDate);
            XMLSerializationHelper.AppendDateIfNotNull(sb, "PatentExpire", PatentExpire);
            XMLSerializationHelper.AppendDateIfNotNull(sb, "SubmissionOfRenewalDate", SubmissionOfRenewalDate);
            XMLSerializationHelper.AppendDateIfNotNull(sb, "RenewalDate", RenewalDate);

            XMLSerializationHelper.AppendIfNotNull(sb, "Comments", Comments == null ? null : Comments.Trim());
            XMLSerializationHelper.AppendIfNotNull(sb, "PackSize", PackSize == null ? null : PackSize.Trim());
            XMLSerializationHelper.AppendIfNotNull(sb, "ShelfLife", ShelfLife == null ? null : ShelfLife.Trim());

            sb.Append("/>");
            return sb.ToString().Replace("\n", " ") + "\n";
        }

    }
}