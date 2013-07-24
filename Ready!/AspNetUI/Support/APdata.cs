using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetUI.Support
{
    public class APdata
    {

        private string authorized_product;
        private string eV_CODE;
        private string xEVPRM_status;
        private string country;
        private string related_product;
        private string auth_status;
        private string auth_number;
        private string number_auth_date;
        private string mAH_name;
        private string expiry_date;
        private string extra_field;


        public APdata(string authorized_product,
                        string EV_CODE,
                        string XEVPRM_status,
                        string country,
                        string related_product,
                        string auth_status,
                        string auth_number,
                        string number_auth_date,
                        string MAH_name,
                        string expiry_date)
        {

            this.Authorized_product = authorized_product;
            this.EV_CODE = EV_CODE;
            this.XEVPRM_status = XEVPRM_status;
            this.Country = country;
            this.Related_product = related_product;
            this.Auth_status = auth_status;
            this.Auth_number = auth_number;
            this.Number_auth_date = number_auth_date;
            this.MAH_name = MAH_name;
            this.EV_CODE = EV_CODE;
            this.Expiry_date = expiry_date;

        }

        public APdata(string authorized_product,
                        string EV_CODE,
                        string XEVPRM_status,
                        string country,
                        string related_product,
                        string auth_status,
                        string auth_number,
                        string number_auth_date,
                        string MAH_name,
                        string expiry_date,
                        string extra_field)
        {

            this.Authorized_product = authorized_product;
            this.EV_CODE = EV_CODE;
            this.XEVPRM_status = XEVPRM_status;
            this.Country = country;
            this.Related_product = related_product;
            this.Auth_status = auth_status;
            this.Auth_number = auth_number;
            this.Number_auth_date = number_auth_date;
            this.MAH_name = MAH_name;
            this.EV_CODE = EV_CODE;
            this.Expiry_date = expiry_date;
            this.Extra_field = extra_field;
        }

        public APdata(string authorized_product,
                        string EV_CODE,
                        string XEVPRM_status,
                        string country,
                        string related_product)
        {

            this.Authorized_product = authorized_product;
            this.EV_CODE = EV_CODE;
            this.XEVPRM_status = XEVPRM_status;
            this.Country = country;
            this.Related_product = related_product;
        }



        public string Authorized_product
        {
            get { return authorized_product; }
            set { this.authorized_product = value; }
        }

        public string EV_CODE
        {
            get { return eV_CODE; }
            set { this.eV_CODE = value; }
        }

        public string XEVPRM_status
        {
            get { return xEVPRM_status; }
            set { this.xEVPRM_status = value; }
        }

        public string Country
        {
            get { return country; }
            set { this.country = value; }
        }

        public string Related_product
        {
            get { return related_product; }
            set { this.related_product = value; }
        }

        public string Auth_status
        {
            get { return auth_status; }
            set { this.auth_status = value; }
        }

        public string Auth_number
        {
            get { return auth_number; }
            set { this.auth_number = value; }
        }

        public string Number_auth_date
        {
            get { return number_auth_date; }
            set { this.number_auth_date = value; }
        }

        public string MAH_name
        {
            get { return mAH_name; }
            set { this.mAH_name = value; }
        }

        public string Expiry_date
        {
            get { return expiry_date; }
            set { this.expiry_date = value; }
        }

        public string Extra_field
        {
            get { return extra_field; }
            set { this.extra_field = value; }
        }


    }
}