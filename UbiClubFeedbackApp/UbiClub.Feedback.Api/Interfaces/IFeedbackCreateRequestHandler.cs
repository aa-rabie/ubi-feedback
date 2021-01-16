using System.Threading.Tasks;
using UbiClub.Feedback.Core.Dto;
using UbiClub.Feedback.Core.Models;

namespace UbiClub.Feedback.Api.Interfaces
{
    public interface IFeedbackCreateRequestHandler
    {
        Task<SessionFeedbackDto> HandleAsync(FeedbackCreateModel model);
    }
}