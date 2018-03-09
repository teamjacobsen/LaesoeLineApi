using LaesoeLineApi;
using Microsoft.Net.Http.Headers;
using System;
using System.Text;

namespace Microsoft.AspNetCore.Http
{
    public static class HttpRequestExtensions
    {
        public static Credentials GetCredentials(this HttpRequest request)
        {
            var scheme = "Basic";

            var header = request.Headers[HeaderNames.Authorization].ToString();

            if (header == null)
            {
                return null;
            }

            if (!header.StartsWith(scheme + " ", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(header.Substring(scheme.Length + 1)));

            var delimiterIndex = credentials.IndexOf(':');

            if (delimiterIndex == -1)
            {
                return null;
            }

            return new Credentials()
            {
                Username = credentials.Substring(0, delimiterIndex),
                Password = credentials.Substring(delimiterIndex + 1)
            };
        }
    }
}
