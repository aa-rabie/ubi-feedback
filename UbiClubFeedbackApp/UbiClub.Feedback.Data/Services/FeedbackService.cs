using AutoMapper;
using System;
using System.Threading.Tasks;
using UbiClub.Feedback.Core.Dto;
using UbiClub.Feedback.Data.Interfaces;
using UbiClub.Feedback.Entities;

namespace UbiClub.Feedback.Data.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IGenericRepository _repo;
        private readonly IMapper _mapper;

        public FeedbackService(IMapper mapper, IGenericRepository repo)
        {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<SessionFeedbackDto> AddFeedbackAsync(Guid sessionId, Guid userId, byte rating)
        {
            var entity = new SessionFeedback()
            {
                SessionId = sessionId,
                UserId = userId,
                Rating = rating
            };
            _repo.Add(entity);

            await _repo.SaveAsync();
            return _mapper.Map<SessionFeedbackDto>(entity);
        }
    }
}