using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

namespace AspNetUI.Support
{
    public static class RequestHelper
    {
        public static string ExtractCurrentQuery(this HttpRequest request, IEnumerable<string> queryStringKeys)
        {
            var localPath = request.Url.LocalPath.ToLower();
            var queryString = GetQueryString(request.QueryString, queryStringKeys);
          
            return localPath + (!string.IsNullOrWhiteSpace(queryString) ? "?" + queryString : string.Empty);
        }

        public static string ExtractRefererQuery(this HttpRequest request, IEnumerable<string> queryStringKeys)
        {
            if (request.UrlReferrer != null)
            {
                var localPath = request.UrlReferrer.LocalPath.ToLower();
                var queryString = GetQueryString(HttpUtility.ParseQueryString(request.UrlReferrer.Query), queryStringKeys);

                return localPath + (!string.IsNullOrWhiteSpace(queryString) ? "?" + queryString : string.Empty);
            }

            return null;
        }

        private static string GetQueryString(NameValueCollection originalQueryString, IEnumerable<string> queryStringKeys)
        {
            var queryString = string.Empty;
            var queryStringPairs = new Dictionary<string, string>();

            foreach (var queryStringKey in queryStringKeys)
            {
                var queryStringValue = originalQueryString[queryStringKey];
                if (!string.IsNullOrWhiteSpace(queryStringValue) && !queryStringPairs.ContainsKey(queryStringValue))
                {
                    queryStringPairs.Add(queryStringKey, queryStringValue);
                }
            }

            foreach (var queryStringPair in queryStringPairs)
            {
                if (!string.IsNullOrWhiteSpace(queryString)) queryString += "&";
                queryString += queryStringPair.Key + "=" + queryStringPair.Value;
            }
            return queryString;
        }
    }
}