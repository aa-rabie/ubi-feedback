using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using UbiClub.Feedback.Core.Dto;
using UbiClub.Feedback.Entities;

namespace UbiClub.Feedback.Api.Infrastructure.AutoMapper
{
    class FeedbackProfile : Profile
    {
        public FeedbackProfile()
        {
            CreateMap<SessionFeedback, SessionFeedbackDto>().ReverseMap();
        }
    }
}
