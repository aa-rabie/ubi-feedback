using Microsoft.AspNetCore.Http;
using System;
using UbiClub.Feedback.Core.Models;

namespace UbiClub.Feedback.Api.Interfaces
{
    public interface IFeedbackCreateModelFactory
    {
        FeedbackCreateModel Create(string requestBody, IHeaderDictionary headers, Guid? sessionId);
    }
}