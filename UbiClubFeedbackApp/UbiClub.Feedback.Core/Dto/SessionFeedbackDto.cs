using System;

namespace UbiClub.Feedback.Core.Dto
{
    public class SessionFeedbackDto
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public Guid SessionId { get; set; }
        public Guid UserId { get; set; }
        public byte Rating { get; set; }
    }
}