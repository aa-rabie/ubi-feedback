﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public async Task<int> GetFeedbackCountPerUserSessionAsync(Guid sessionId, Guid userId)
        {
            return await _repo.CountAsync<SessionFeedback>(f => f.SessionId
                                                                == sessionId && f.UserId == userId);
        }

        public async Task<List<SessionFeedbackDto>> GetFeedbackListAsync(byte? rating
        , int offset, int limit)
        {
            Expression<Func<SessionFeedback, bool>> filter = null;
            if (rating.HasValue)
            {
                filter = feedback => feedback.Rating == rating.Value;
            }
            var data = await _repo.FindAsync<SessionFeedback>(filter, q => q.OrderByDescending(e => e.CreatedDate), string.Empty, limit: limit, offset: offset);
            return _mapper.Map<List<SessionFeedbackDto>>(data);
        }
    }
}