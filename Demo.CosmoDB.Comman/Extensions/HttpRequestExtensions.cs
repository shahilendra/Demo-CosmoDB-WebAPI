using Microsoft.AspNetCore.Http;
using System.Text;

namespace Demo.CosmoDB.Comman.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string ToMapString(this HttpRequest httpRequest)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Path: {httpRequest.Path}");
            stringBuilder.AppendLine($"Query String: ");
            foreach (var query in httpRequest.Query)
                stringBuilder.AppendLine($"\t\t {query.Key}: {query.Value}");
            stringBuilder.AppendLine($"Headers: ");
            foreach (var header in httpRequest.Headers)
                if (!string.Equals(header.Key, "Authorization", System.StringComparison.InvariantCultureIgnoreCase))
                    stringBuilder.AppendLine($"\t\t {header.Key}: {header.Value}");
            return stringBuilder.ToString();
        }
    }
}
