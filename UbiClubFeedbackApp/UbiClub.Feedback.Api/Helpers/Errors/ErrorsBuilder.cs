using System.Linq;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UbiClub.Feedback.Core.Errors;

namespace UbiClub.Feedback.Api.Helpers.Errors
{
    public class ErrorsBuilder
    {
        public static ObjectResult BuildInternalServerError()
        {
            return new ObjectResult(new Error
            {
                Code = ErrorCodes.NotAllowed,
                Message = "Something wrong. Please contact system support."
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
        public static BadRequestObjectResult BuildNotAllowedError(string msg)
        {
            return new BadRequestObjectResult(new Error
            {
                Code = ErrorCodes.NotAllowed,
                Message = msg
            });
        }

        public static BadRequestObjectResult BuildBadArgumentError(string msg)
        {
            return new BadRequestObjectResult(new Error
            {
                Code = ErrorCodes.BadArgument,
                Message = msg
            });
        }

        public static BadRequestObjectResult BuildBadArgumentError(string msg, ValidationResult validationResult)
        {
            return new BadRequestObjectResult(new Error
            {
                Code = ErrorCodes.BadArgument,
                Message = msg,
                Details = validationResult.Errors.Select(ve => new Detail()
                {
                    Target = ve.PropertyName,
                    Message = ve.ErrorMessage
                }).ToList()
            });
        }
    }
    
}