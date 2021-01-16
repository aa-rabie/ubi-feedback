using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UbiClub.Feedback.Core.Dto;

namespace UbiClub.Feedback.Data.Interfaces
{
    public interface IFeedbackService
    {
        Task<SessionFeedbackDto> AddFeedbackAsync(Guid sessionId, Guid userId, byte rating);
        Task<int> GetFeedbackCountPerUserSessionAsync(Guid sessionId, Guid userId);

        Task<List<SessionFeedbackDto>> GetFeedbackListAsync(byte? rating
            , int offset, int limit);
    }
}