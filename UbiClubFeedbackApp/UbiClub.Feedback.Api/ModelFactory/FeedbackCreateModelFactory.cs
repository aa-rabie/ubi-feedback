using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Newtonsoft.Json;
using UbiClub.Feedback.Api.Interfaces;
using UbiClub.Feedback.Core.Models;

namespace UbiClub.Feedback.Api.ModelFactory
{
    internal class FeedbackCreateModelFactory : IFeedbackCreateModelFactory
    {
        private const string UserIdHeaderName = "Ubi-UserId";
        public async Task<FeedbackCreateModel> CreateAsync(HttpRequest req, Guid? sessionId)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var ratingModel = JsonConvert.DeserializeObject<RatingModel>(requestBody);
            var model = new FeedbackCreateModel()
            {
                SessionId = sessionId,
                Rating = ratingModel.Rating
            };

            if (req.Headers.TryGetValue(UserIdHeaderName, out var headerValue))
            {
                var strUserId = headerValue.FirstOrDefault();
                if (!String.IsNullOrEmpty(strUserId)
                    && Guid.TryParse(strUserId, out var userId))
                {
                    model.UserId = userId;
                }
            }

            return model;
        }
    }
}