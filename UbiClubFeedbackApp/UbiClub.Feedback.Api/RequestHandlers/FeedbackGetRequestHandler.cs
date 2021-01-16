using System.Collections.Generic;
using System.Threading.Tasks;
using UbiClub.Feedback.Api.Interfaces;
using UbiClub.Feedback.Core.Dto;
using UbiClub.Feedback.Core.Models;
using UbiClub.Feedback.Data.Interfaces;

namespace UbiClub.Feedback.Api.RequestHandlers
{
    public class FeedbackGetRequestHandler : IFeedbackGetRequestHandler
    {
        private readonly IFeedbackService _feedbackService;
        public FeedbackGetRequestHandler(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        public async Task<List<SessionFeedbackDto>> HandleAsync(FeedbackGetModel model)
        {
            var rating = model.Rating;

            return await _feedbackService.GetFeedbackListAsync( rating: rating,offset: 0,limit: 15);
        }
    }
}