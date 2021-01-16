using FluentValidation;
using UbiClub.Feedback.Core.Models;

namespace UbiClub.Feedback.Api.Validators
{
    public class FeedbackGetModelValidator : AbstractValidator<FeedbackGetModel>
    {
        public FeedbackGetModelValidator()
        {
            RuleFor(model => model.Rating).InclusiveBetween((byte)1, (byte)5);
        }
    }
}