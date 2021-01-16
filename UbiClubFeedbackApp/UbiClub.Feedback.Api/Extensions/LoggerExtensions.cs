using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace UbiClub.Feedback.Api.Extensions
{
    public static class LoggerExtensions
    {
        public static void WriteError(this ILogger logger, string msg, Exception exp, Dictionary<string, object> srcProps = null)
        {
            var _msg = $"{msg}";
            Dictionary<string, object> props = new Dictionary<string, object>();
            if (exp != null)
            {
                props = SaveExceptionProperties(exp);
            }

            if (srcProps != null && srcProps.Count > 0)
            {
                foreach (var item in srcProps)
                {
                    if (!props.ContainsKey(item.Key) && item.Value != null)
                    {
                        props[item.Key] = item.Value;
                    }
                }
            }

            if (props != null && props.Count > 0)
            {
                _msg = $"{_msg}, Props: {props.Dump()}";
            }
            logger.LogError(_msg);
        }

        public static void WriteException(this ILogger logger, Exception exception)
        {
            var exceptionType = exception.GetType().Name;
            WriteError(logger, $"{exceptionType} was thrown", exception);
        }

        public static void WriteAggregateException(this ILogger logger, AggregateException aggregate)
        {
            foreach (var exception in aggregate.InnerExceptions)
                if (exception is AggregateException agEx)
                    WriteAggregateException(logger, agEx);
                else
                {
                    WriteException(logger, exception);
                }
        }


        private static Dictionary<string, object> SaveExceptionProperties(Exception exp)
        {
            if (exp == null) return null;
            var properties = new Dictionary<string, object>();

            properties.Add("ExceptionName", exp.GetType().Name);
            properties.Add("Message", exp.Message);
            properties.Add("StackTrace", exp.StackTrace);

            var innerException = exp.InnerException;
            var baseException = exp.GetBaseException();
            if (innerException != null)
            {
                properties.Add("InnerExceptionName", innerException.GetType().Name);
                properties.Add("InnerExceptionMessage", innerException.Message);
                properties.Add("InnerExceptionStackTrace", innerException.StackTrace);
                //log root-cause exception 
                properties.Add("BaseExceptionName", baseException.GetType().Name);
                properties.Add("BaseExceptionMessage", baseException.Message);
                properties.Add("BaseExceptionStackTrace", baseException.StackTrace);
            }
            return properties;
        }
    }
}