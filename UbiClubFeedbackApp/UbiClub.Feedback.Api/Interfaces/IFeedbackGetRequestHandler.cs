using System.Collections.Generic;
using System.Threading.Tasks;
using UbiClub.Feedback.Core.Dto;
using UbiClub.Feedback.Core.Models;

namespace UbiClub.Feedback.Api.Interfaces
{
    public interface IFeedbackGetRequestHandler
    {
        Task<List<SessionFeedbackDto>> HandleAsync(FeedbackGetModel model);
    }
}