using Microsoft.AspNetCore.Http;
using UbiClub.Feedback.Core.Models;

namespace UbiClub.Feedback.Api.Interfaces
{
    public interface IFeedbackGetModelFactory
    {
        FeedbackGetModel Create(IQueryCollection queryCol);
    }
}