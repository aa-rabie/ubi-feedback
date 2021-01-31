using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using UbiClub.Feedback.Api.Interfaces;
using UbiClub.Feedback.Core.Models;
[assembly: InternalsVisibleToAttribute("UbiClub.Feedback.Tests")]

namespace UbiClub.Feedback.Api.ModelFactory
{
    internal class FeedbackCreateModelFactory : IFeedbackCreateModelFactory
    {
        private const string UserIdHeaderName = "Ubi-UserId";
        public FeedbackCreateModel Create(string requestBody, IHeaderDictionary headers, Guid? sessionId)
        {
            var ratingModel = JsonConvert.DeserializeObject<RatingModel>(requestBody ?? string.Empty);

            var model = new FeedbackCreateModel()
            {
                SessionId = sessionId
            };

            if (!String.IsNullOrEmpty(ratingModel.Rating))
            {
               if(byte.TryParse(ratingModel.Rating, out var ratingValue))
               {
                    model.Rating = ratingValue;
               }
            }

            if (headers != null && headers.Any() && headers.TryGetValue(UserIdHeaderName, out var headerValue))
            {
                var strUserId = headerValue.FirstOrDefault();
                if (!String.IsNullOrEmpty(strUserId))
                {
                    if (Guid.TryParse(strUserId, out var userId))
                    {
                        model.UserId = userId;
                    }
                }
            }

            return model;
        }
    }
}