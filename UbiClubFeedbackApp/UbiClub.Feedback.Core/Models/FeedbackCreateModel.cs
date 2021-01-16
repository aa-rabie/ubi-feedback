using System;

namespace UbiClub.Feedback.Core.Models
{
    public class FeedbackCreateModel
    {
        public Guid? SessionId { get; set; }
        public Guid? UserId { get; set; }
        public byte? Rating { get; set; }
    }
}