using System;

namespace UbiClub.Feedback.Api.Helpers.Routing
{
    public static class RoutesHelper
    {
        public const string ApiPrefix = "api";
        public static Uri BuildFeedbackGetUrl(Guid id, string baseUrl)
        {
            var url = $"{ApiPrefix}/{PathSegments.Feedback}/{id}";
            return string.IsNullOrEmpty(baseUrl) ? new Uri(url, UriKind.Relative)
                : new Uri($"{baseUrl}/{url}");
        }
    }
}