using AutoMapper;
using System;
using System.Threading.Tasks;
using UbiClub.Feedback.Data.Interfaces;
using UbiClub.Feedback.Core.Dto;
using UbiClub.Feedback.Entities;

namespace UbiClub.Feedback.Data.Services
{
    public class GameSessionService : IGameSessionService
    {
        private readonly IGenericRepository _repo;
        private readonly IMapper _mapper;

        public GameSessionService(IMapper mapper, IGenericRepository repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<GameSessionDto> GetAsync(Guid sessionId)
        {
            var entity = await _repo.GetByIdAsync<GameSession>(sessionId);

            return entity == null ? null : _mapper.Map<GameSessionDto>(entity);
        }
    }
}