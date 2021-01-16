using FluentValidation;
using UbiClub.Feedback.Core.Models;

namespace UbiClub.Feedback.Api.Validators
{
    public class FeedbackCreateModelValidator : AbstractValidator<FeedbackCreateModel>
    {
        public FeedbackCreateModelValidator()
        {
            RuleFor(model => model.SessionId).NotNull();
            RuleFor(model => model.UserId).NotNull();
            RuleFor(model => model.Rating).NotNull().InclusiveBetween((byte)1, (byte)10);
        }
    }
}