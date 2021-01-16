using System.Collections.Generic;

namespace UbiClub.Feedback.Core.Errors
{
    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public List<Detail> Details { get; set; }
    }
}