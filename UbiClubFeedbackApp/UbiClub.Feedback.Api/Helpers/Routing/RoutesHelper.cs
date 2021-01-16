using System;

namespace UbiClub.Feedback.Api.Helpers.Routing
{
    public static class RoutesHelper
    {
        public const string ApiPrefix = "api";
        public static Uri BuildFeedbackGetUrl(Guid id)
        {
            return new Uri($"{ApiPrefix}/{PathSegments.Feedback}/{id}", UriKind.Relative);
        }
    }
}