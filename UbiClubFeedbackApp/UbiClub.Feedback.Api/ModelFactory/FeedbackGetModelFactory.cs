using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using UbiClub.Feedback.Api.Interfaces;
using UbiClub.Feedback.Core.Models;

namespace UbiClub.Feedback.Api.ModelFactory
{
    internal class FeedbackGetModelFactory : IFeedbackGetModelFactory
    {
        private const string RatingQueryParamName = "rating";
        public FeedbackGetModel Create(IQueryCollection queryCol)
        {
            var model = new FeedbackGetModel();

            if (queryCol != null && queryCol.Any() && queryCol.TryGetValue(RatingQueryParamName, out var queryParamValue))
            {
                var strRating = queryParamValue.FirstOrDefault();
                if (!String.IsNullOrEmpty(strRating))
                {
                    if (byte.TryParse(strRating, out var rating))
                    {
                        model.Rating = rating;
                    }
                }
            }

            return model;
        }
    }
}