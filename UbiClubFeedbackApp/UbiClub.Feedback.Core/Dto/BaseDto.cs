using System;

namespace UbiClub.Feedback.Core.Dto
{
    public abstract class BaseDto
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}