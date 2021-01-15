using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace UbiClub.Feedback.Entities
{
    [Table("t_SessionFeedback")]
    public class SessionFeedback : BaseEntity
    {
        public Guid SessionId { get; set; }
        public Guid UserId { get; set; }
        public byte Rating { get; set; }
    }
}