using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using AspNetUI.NanokinetikEDMS;
using GEM2Common;
using Ready.Model;
using System.Text;

namespace AspNetUI.Services
{
    public class EDMS : IHttpHandler
    {
        string returnSerializedString;
        Dictionary<String, String> asyncReturnValues;

        public void ProcessRequest(HttpContext context)
        {
            asyncReturnValues = new Dictionary<string, string>();

            if (context.Request.Params.AllKeys.Contains("MethodName"))
            {
                string methodName = context.Request.Params["MethodName"];
                try
                {
                    switch (methodName)
                    {
                        case "GetGridResultFromDb":
                            GetGridResultFromLocalDb(context);
                            break;
                        case "GetGridResult":
                            GetGridResult(context);
                            break;
                        default:
                            UpdateResponse("result", "none");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    UpdateResponse("result", "none");
                }
            }
            else UpdateResponse("result", "none");

            returnSerializedString = SerializeResponse(asyncReturnValues);
            context.Response.ContentType = "text/plain";
            context.Response.Write(returnSerializedString);
        }

        public void GetGridResultFromLocalDb(HttpContext context)
        {
            IDocument_PKOperations documentOperations = new Document_PKDAL();

            var columnNameParam = context.Request.Params["ColumnName"];
            if (string.IsNullOrWhiteSpace(columnNameParam)) columnNameParam = "DocumentName.ASC";

            string columnName = "DocumentName";
            if (columnNameParam.LastIndexOf(".", StringComparison.InvariantCulture) != -1)
            {
                columnName = columnNameParam.Substring(0, columnNameParam.LastIndexOf(".", StringComparison.InvariantCulture));
            }

            var sortOrder = GEMOrderByType.ASC;
            if (columnNameParam.LastIndexOf(".", StringComparison.InvariantCulture) != -1)
            {
                var tmpSortOrderStr = columnNameParam.Substring(columnNameParam.LastIndexOf(".", StringComparison.InvariantCulture) + 1, columnNameParam.Length - columnNameParam.LastIndexOf(".", StringComparison.InvariantCulture) - 1);
                GEMOrderByType tmpSortType;
                if (Enum.TryParse(tmpSortOrderStr, out tmpSortType))
                {
                    sortOrder = tmpSortType;
                }
            }

            // TODO: Remove following line after test cases
            if (columnName == "DocumentFormat") columnName = "RegulatoryStatus";

            var filters = new Dictionary<string, string>();
            var gobList = new List<GEMOrderBy>();

            gobList.Add(new GEMOrderBy(columnName, sortOrder));

            int totalRecordCount;
            DataSet documentList = documentOperations.GetListFormDataSetEDMS(filters, 1, Int32.MaxValue, gobList, out totalRecordCount);

            var documentJSONArray = "#array#[";

            if (documentList != null && documentList.Tables.Count > 0)
            {
                foreach (DataRow document in documentList.Tables[0].Rows)
                {
                    documentJSONArray += ("{ \"documentPk\":\"" + document["document_PK"] + "\"," +
                                              "\"DocumentName\" : \"" + document["documentName"] + "\"," +
                                              "\"VersionLabel\" : \"" + document["versionLabel"] + "\"," +
                                              "\"VersionNumber\" : \"" + document["versionNumber"] + "\"," +
                                              "\"DocumentFormat\" : \"" + document["regulatoryStatus"] + "\"" +
                                          "},");
                }
            }

            if (documentJSONArray.EndsWith(",")) documentJSONArray = documentJSONArray.Substring(0, documentJSONArray.Length - 1);
            documentJSONArray += "]";

            UpdateResponse("documents", documentJSONArray);
            //UpdateResponse("page", currentPage.ToString());
            UpdateResponse("totalRecordCount", totalRecordCount.ToString());
        }

        public void GetGridResult(HttpContext httpContext)
        {
            var edmsWsClient = new EDMS_WSClient();

            var folderId = httpContext.Request.Params["folderId"];

            var username = System.Threading.Thread.CurrentPrincipal.Identity.Name;

            var edmsFolders = edmsWsClient.getFolders(folderId, username);

            var documentJSONArray = "#array#[";

            if (edmsFolders != null && edmsFolders.Any())
            {
                documentJSONArray = edmsFolders.Aggregate(documentJSONArray, (current, folder) => current + ("{ " + "\"folderId\":\"" + folder.folderID + "\"," + "\"folderName\" : \"" + folder.folderName + "\"," + "\"children\" : \"" + string.Empty + "\"" + "},"));
            }

            if (documentJSONArray.EndsWith(",")) documentJSONArray = documentJSONArray.Substring(0, documentJSONArray.Length - 1);
            documentJSONArray += "]";

            UpdateResponse("documents", documentJSONArray);
            if (edmsFolders != null) UpdateResponse("totalRecordCount", edmsFolders.Count().ToString());
        }

        /// <summary>
        /// Methods serializes return values to text represented JSON object
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        String SerializeResponse(Dictionary<String, String> args)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            bool first = true;
            foreach (KeyValuePair<String, String> kvp in args)
            {
                if (!first) builder.Append(", ");
                first = false;
                String value = kvp.Value;
                if (!kvp.Value.Trim().StartsWith("#array#["))
                {
                    value = "\"" + JSONEscape(kvp.Value.Trim()) + "\"";
                }
                else
                {
                    value = value.Replace("#array#", "");
                }
                builder.Append("\"" + kvp.Key + "\":" + value);
            }
            builder.Append("}");

            return builder.ToString();
        }

        private void UpdateResponse(String key, String value)
        {
            if (!this.asyncReturnValues.ContainsKey(key))
            {
                asyncReturnValues.Add(key, value);
            }
            else
            {
                asyncReturnValues[key] = value;
            }
        }

        private static String JSONEscape(String str)
        {
            return str.Replace("\"", "\\\"");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}