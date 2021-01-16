using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using UbiClub.Feedback.Core.Models;

namespace UbiClub.Feedback.Api.Interfaces
{
    public interface IFeedbackCreateModelFactory
    {
        Task<FeedbackCreateModel> CreateAsync(HttpRequest req, Guid? sessionId);
    }
}