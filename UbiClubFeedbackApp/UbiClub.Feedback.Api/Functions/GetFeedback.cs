using System;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using UbiClub.Feedback.Api.Constants;
using UbiClub.Feedback.Api.Helpers.Errors;
using UbiClub.Feedback.Api.Helpers.Routing;
using UbiClub.Feedback.Api.Interfaces;
using UbiClub.Feedback.Core.Models;

namespace UbiClub.Feedback.Api.Functions
{
    public class GetFeedback
    {
        private readonly IValidator<FeedbackGetModel> _validator;
        private readonly IFeedbackGetModelFactory _modelFactory;
        private readonly IFeedbackGetRequestHandler _handler;
        public GetFeedback(IValidator<FeedbackGetModel> validator
            , IFeedbackGetModelFactory modelFactory
            , IFeedbackGetRequestHandler handler)
        {
            _validator = validator;
            _modelFactory = modelFactory;
            _handler = handler;
        }

        [FunctionName(FunctionNames.GetFeedback)]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "Get", Route = 
                PathSegments.Feedback)] HttpRequest req, ILogger log)
        {
            try
            {
                var model = _modelFactory.Create(req.Query);

                var result = _validator.Validate(model);
                if (!result.IsValid)
                {
                    return ErrorsBuilder.BuildBadArgumentError("Feedback Get Request Data contains invalid/missing arguments",
                        result.Errors);
                }
                var feedbackData = await _handler.HandleAsync(model);
                return new OkObjectResult(feedbackData);
            }
            catch (Exception ex)
            {
                //TODO: logging
                return ErrorsBuilder.BuildInternalServerError();
            }
        }
    }
}
