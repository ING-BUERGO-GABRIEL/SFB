using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SFB.Shared.Helpers
{
    public static class ToolsHelper
    {
        public static string ToQueryString(this IDictionary<string, object> @params)
        {
            if (@params == null || @params.Count == 0)
                return string.Empty;

            var queryString = string.Join("&", @params
                    .Where(kv => kv.Value != null)
                    .SelectMany(kv =>
                    {
                        if (kv.Value is IEnumerable<object> list)
                            return list.Select(item => $"{HttpUtility.UrlEncode(kv.Key)}={HttpUtility.UrlEncode(item.ToString())}");
                        return new[] { $"{HttpUtility.UrlEncode(kv.Key)}={HttpUtility.UrlEncode(kv.Value.ToString())}" };
                    }));

            return queryString;
        }

        public static Dictionary<string, object> PrepareQueryParams(string nameFromQuery, object valueQuery)
        {
            return new Dictionary<string, object> { { nameFromQuery, valueQuery } };
        }

        public static string CreateQueryParams(string nameFromQuery, object valueQuery)
        {
            var queryParams = PrepareQueryParams(nameFromQuery,valueQuery);
            return queryParams.ToQueryString();
        }
    }
}
