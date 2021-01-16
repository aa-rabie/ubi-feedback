using System;

namespace UbiClub.Feedback.Api.Exceptions
{
    public class FeedbackCreateRequestNotAllowedException: Exception
    {
        public FeedbackCreateRequestNotAllowedException(Guid sessionId, Guid userId)
        : base($"Feedback cannot be created. User : [{userId}] has already submitted feedback for Game Session: [{sessionId}]")
        { }
    }
}