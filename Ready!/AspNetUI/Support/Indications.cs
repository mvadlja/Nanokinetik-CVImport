using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AspNetUI.Views;
using Ready.Model;

namespace AspNetUI.Support
{
    public static class Indications
    {
        public static string GetFormattedText(DataTable dtIndications, string defaultEmptyValue = Constant.DefaultEmptyValue)
        {
            var formattedText = string.Empty;

            if (dtIndications != null && dtIndications.Rows.Count > 0 &&
                    dtIndications.Columns.Contains("version_type_FK") && 
                    dtIndications.Columns.Contains("level_type_FK") &&
                    dtIndications.Columns.Contains("code") && 
                    dtIndications.Columns.Contains("term"))
            {
                foreach (DataRow row in dtIndications.Rows)
                {
                    var text = GetFormattedText(row, defaultEmptyValue);

                    if (!string.IsNullOrWhiteSpace(formattedText)) formattedText += "<br>" + text;
                    else formattedText = text;
                }
            }
            else
            {
                formattedText = defaultEmptyValue;
            }

            return formattedText;
        }

        public static List<string> GetFormattedTextInStringList(DataTable dtIndications, string defaultEmptyValue = Constant.DefaultEmptyValue)
        {
            var formattedTextList = new List<string>();

            if (dtIndications != null && dtIndications.Rows.Count > 0 &&
                    dtIndications.Columns.Contains("version_type_FK") &&
                    dtIndications.Columns.Contains("level_type_FK") &&
                    dtIndications.Columns.Contains("code") &&
                    dtIndications.Columns.Contains("term"))
            {
                foreach (DataRow row in dtIndications.Rows)
                {
                    formattedTextList.Add(GetFormattedText(row, defaultEmptyValue));
                }
            }

            return formattedTextList;
        }

        public static List<Meddra_pk> GetFormattedTextInEntityList(DataTable dtIndications, string defaultEmptyValue = Constant.DefaultEmptyValue)
        {
            var meddraList = new List<Meddra_pk>();

            if (dtIndications != null && dtIndications.Rows.Count > 0 &&
                    dtIndications.Columns.Contains("version_type_FK") &&
                    dtIndications.Columns.Contains("level_type_FK") &&
                    dtIndications.Columns.Contains("code") &&
                    dtIndications.Columns.Contains("term"))
            {
                foreach (DataRow row in dtIndications.Rows)
                {
                    if (row["meddra_pk"] != null && !string.IsNullOrWhiteSpace(row["meddra_pk"].ToString()))
                    {
                        meddraList.Add( new Meddra_pk {
                             meddra_pk = Convert.ToInt32(row["meddra_pk"].ToString()),
                             version_type_FK = Convert.ToInt32(row["version_type_FK"].ToString()),
                             level_type_FK = Convert.ToInt32(row["level_type_FK"].ToString()),
                             code = row["code"].ToString(),
                             term = row["term"].ToString(),
                             MeddraFullName = GetFormattedText(row, defaultEmptyValue)
                        });
                    }
                }
            }

            return meddraList;
        }

        public static string GetFormattedText(DataRow indication, string defaultEmptyValue = Constant.DefaultEmptyValue)
        {
            var typeOperations = new Type_PKDAL();

            var meddraVersion = typeOperations.GetEntity(indication["version_type_FK"]);
            var meddraLevel = typeOperations.GetEntity(indication["level_type_FK"]);
         
            var meddraVersionPart = meddraVersion != null && !string.IsNullOrWhiteSpace(meddraVersion.name) ? meddraVersion.name : string.Empty;
            var meddraLevelPart = meddraLevel != null && !string.IsNullOrWhiteSpace(meddraLevel.name) ? meddraLevel.name : string.Empty;
            var meddraCode = !(indication["code"] is DBNull) && indication["code"] != null ? indication["code"].ToString() : string.Empty;
            var meddraTerm = !(indication["term"] is DBNull) && indication["term"] != null ? indication["term"].ToString() : string.Empty;

            return GetFormatedText(meddraVersionPart, meddraLevelPart, meddraCode, meddraTerm, defaultEmptyValue);
        }

        public static string GetFormattedText(Meddra_pk indication, string defaultEmptyValue = Constant.DefaultEmptyValue)
        {
            var typeOperations = new Type_PKDAL();

            var meddraVersion = typeOperations.GetEntity(indication.version_type_FK);
            var meddraLevel = typeOperations.GetEntity(indication.level_type_FK);
            var meddraCode = indication.code;
            var meddraTerm = indication.term;

            var meddraVersionPart = meddraVersion != null && !string.IsNullOrWhiteSpace(meddraVersion.name) ? meddraVersion.name : string.Empty;
            var meddraLevelPart = meddraLevel != null && !string.IsNullOrWhiteSpace(meddraLevel.name) ? meddraLevel.name : string.Empty;

            return GetFormatedText(meddraVersionPart, meddraLevelPart, meddraCode, meddraTerm, defaultEmptyValue);
        }

        private static string GetFormatedText(string meddraVersionPart, string meddraLevelPart, string meddraCodePart, string meddraTermPart, string defaultEmptyValue = Constant.DefaultEmptyValue)
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
}