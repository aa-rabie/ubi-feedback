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
using UbiClub.Feedback.Api.Extensions;
using UbiClub.Feedback.Api.Helpers.Errors;
using UbiClub.Feedback.Api.Helpers.Routing;
using UbiClub.Feedback.Api.Interfaces;
using UbiClub.Feedback.Core.Models;
using HttpMethods = Microsoft.AspNetCore.Http.HttpMethods;

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
            [HttpTrigger(AuthorizationLevel.Function, HttpVerbs.Post, Route = 
                PathSegments.Feedback + "/{sessionId:Guid?}")] HttpRequest req, Guid? sessionId, ILogger logger)
        {
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var model = _modelFactory.Create(requestBody, req.Headers, sessionId);

                var result = _validator.Validate(model);
                if (!result.IsValid)
                {
                    return ErrorsBuilder.BuildBadArgumentError("Feedback Create Request Data contains invalid/missing arguments",
                        result.Errors);
                }

                var feedbackDto = await _handler.HandleAsync(model);
                var location = RoutesHelper.BuildFeedbackGetUrl(feedbackDto.Id, req.Host.Value ?? string.Empty, req.IsHttps);
                return new CreatedResult(location, feedbackDto);
            }
            catch (SessionNotFoundException snfEx)
            {
                return ErrorsBuilder.BuildNotFoundError(snfEx.Message);
            }
            catch (FeedbackCreateRequestNotAllowedException snfEx)
            {
                return ErrorsBuilder.BuildNotAllowedError(snfEx.Message);
            }
            catch (Exception ex)
            {
                //TODO : configure app to log to Azure AppInsights
                if (ex is AggregateException agx)
                {
                    logger.WriteAggregateException(agx);
                }
                else
                {
                    logger.WriteException(ex);
                }
                return ErrorsBuilder.BuildInternalServerError();
            }
        }
    }
}
