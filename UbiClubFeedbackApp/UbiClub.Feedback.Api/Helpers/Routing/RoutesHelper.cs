using System;

namespace UbiClub.Feedback.Api.Helpers.Routing
{
    public static class RoutesHelper
    {
        public const string ApiPrefix = "api";
        public static Uri BuildFeedbackGetUrl(Guid id, string baseUrl, bool isHttps)
        {
            var url = $"{ApiPrefix}/{PathSegments.Feedback}/{id}";
            baseUrl = FormatBaseUrl(baseUrl, isHttps);
            return string.IsNullOrEmpty(baseUrl) ? new Uri(url, UriKind.Relative)
                : new Uri($"{baseUrl}/{url}");
        }

        private static string FormatBaseUrl(string baseUrl, bool isHttps)
        {
            if (string.IsNullOrEmpty(baseUrl))
                return string.Empty;

            if (baseUrl.ToLowerInvariant().Contains("http"))
                return baseUrl;

            return $"{(isHttps ? "https" : "http")}://{baseUrl}";
        }
    }
}