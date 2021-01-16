using System;

namespace UbiClub.Feedback.Api.Exceptions
{
    public class SessionNotFoundException : Exception
    {
        public SessionNotFoundException(Guid sessionId) : base($"Session with Id:[{sessionId}] does not exist.")
        {
            
        }
    }
}