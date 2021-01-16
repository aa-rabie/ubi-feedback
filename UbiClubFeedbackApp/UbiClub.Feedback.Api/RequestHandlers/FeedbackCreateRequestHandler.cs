using System.Threading.Tasks;
using UbiClub.Feedback.Api.Exceptions;
using UbiClub.Feedback.Api.Interfaces;
using UbiClub.Feedback.Core.Dto;
using UbiClub.Feedback.Core.Models;
using UbiClub.Feedback.Data.Interfaces;

namespace UbiClub.Feedback.Api.RequestHandlers
{
    public class FeedbackCreateRequestHandler : IFeedbackCreateRequestHandler
    {
        private readonly IGameSessionService _gameSessionService;
        private readonly IFeedbackService _feedbackService;
        public FeedbackCreateRequestHandler(IGameSessionService gameSessionService
            , IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
            _gameSessionService = gameSessionService;
        }
        public async Task<SessionFeedbackDto> HandleAsync(FeedbackCreateModel model)
        {
            var sessionId = model.SessionId.Value;
            var userId = model.UserId.Value;
            var rating = model.Rating.Value;

            var session = await _gameSessionService.GetAsync(sessionId);
            if(session == null)
                throw new SessionNotFoundException(model.SessionId.Value);

            var count = await _feedbackService.GetFeedbackCountPerUserSessionAsync(sessionId: sessionId, userId: userId);

            if(count > 0)
                throw new FeedbackCreateRequestNotAllowedException(sessionId:sessionId, userId: userId);

            return await _feedbackService.AddFeedbackAsync(
                sessionId: sessionId,
                userId: userId,
                rating: rating
            );
        }
    }
}