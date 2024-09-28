using Custom.Utils;
using System;
using System.Collections.Generic;

namespace Custom.Services.Network.RestAPI
{
    public static class APIUtils
    {
        public static string GenerateParamsUrl(string baseUrl, string endPoint, Dictionary<string, string> queryParams)
        {
            UriBuilder builder = new UriBuilder(baseUrl + endPoint);
            if (queryParams != null)
            {
                int count = 0;
                string query = "";
                foreach (var item in queryParams)
                {
                    if (count == 0)
                    {
                        query += (item.Key + "=" + item.Value);
                    }
                    else
                    {
                        query += "&" + (item.Key + "=" + item.Value);
                    }
                    count++;
                }
                builder.Query = query;

            }
            string url = Uri.EscapeUriString(builder.Uri.AbsoluteUri);
            LogUtils.Log("URL", url);
            return url;
        }
    }
}