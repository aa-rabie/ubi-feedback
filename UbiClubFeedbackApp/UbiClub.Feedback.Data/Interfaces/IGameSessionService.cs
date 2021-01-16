using System;
using System.Threading.Tasks;
using UbiClub.Feedback.Core.Dto;

namespace UbiClub.Feedback.Data.Interfaces
{
    public interface IGameSessionService
    {
        Task<GameSessionDto> GetAsync(Guid sessionId);
    }
}