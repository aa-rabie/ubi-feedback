using System;
using System.IO;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using UbiClub.Feedback.Api.Constants;
using UbiClub.Feedback.Api.Exceptions;
using UbiClub.Feedback.Api.Helpers.Errors;
using UbiClub.Feedback.Api.Helpers.Routing;
using UbiClub.Feedback.Api.Interfaces;
using UbiClub.Feedback.Core.Models;

namespace UbiClub.Feedback.Api.Functions
{
    public class CreateFeedback
    {
        private readonly IValidator<FeedbackCreateModel> _validator;
        private readonly IFeedbackCreateModelFactory _modelFactory;
        private readonly IFeedbackCreateRequestHandler _handler;
        public CreateFeedback(IValidator<FeedbackCreateModel> validator
        , IFeedbackCreateModelFactory modelFactory
        , IFeedbackCreateRequestHandler handler)
        {
            _validator = validator;
            _modelFactory = modelFactory;
            _handler = handler;
        }

        [FunctionName(FunctionNames.CreateFeedback)]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = 
                PathSegments.Feedback + "/{sessionId:Guid?}")] HttpRequest req, Guid? sessionId, ILogger log)
        {
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var model = _modelFactory.Create(requestBody, req.Headers, sessionId, out var errors);
                if (errors.Any())
                {
                    return ErrorsBuilder.BuildBadArgumentError("Feedback Create Request Data are invalid",
                        errors);
                }
                var result = _validator.Validate(model);
                if (!result.IsValid)
                {
                    return ErrorsBuilder.BuildBadArgumentError("Feedback Create Request Data are invalid/missing",
                        result.Errors);
                }

                var feedbackDto = await _handler.HandleAsync(model);
                var location = RoutesHelper.BuildFeedbackGetUrl(feedbackDto.Id);
                return new CreatedResult(location, feedbackDto);
            }
            catch (SessionNotFoundException snfEx)
            {
                return ErrorsBuilder.BuildBadArgumentError(snfEx.Message);
            }
            catch (FeedbackCreateRequestNotAllowedException snfEx)
            {
                return ErrorsBuilder.BuildNotAllowedError(snfEx.Message);
            }
            catch (Exception ex)
            {
                return ErrorsBuilder.BuildInternalServerError();
            }
        }
    }
}
