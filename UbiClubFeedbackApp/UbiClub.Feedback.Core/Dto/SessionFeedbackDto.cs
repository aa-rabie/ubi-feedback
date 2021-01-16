using System;

namespace UbiClub.Feedback.Core.Dto
{
    public class SessionFeedbackDto : BaseDto
    {
        public Guid SessionId { get; set; }
        public Guid UserId { get; set; }
        public byte Rating { get; set; }
    }
}