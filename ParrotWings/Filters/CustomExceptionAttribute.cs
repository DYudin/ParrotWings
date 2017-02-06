using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using TransactionSubsystem.Infrastructure.Exceptions;

namespace ParrotWings.Filters
{
    public class CustomExceptionAttribute : Attribute, IExceptionFilter
    {
        public Task ExecuteExceptionFilterAsync(HttpActionExecutedContext actionExecutedContext,
              CancellationToken cancellationToken)
        {
            if (actionExecutedContext.Exception != null)
            {
                if (actionExecutedContext.Exception is TransactionSubsystemException)
                {
                    actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(
                        HttpStatusCode.BadRequest, actionExecutedContext.Exception.Message);
                }
                else
                {
                    // for all unexpected exceptions
                    actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(
                        HttpStatusCode.BadRequest, "Internal server error. Please, contact with developers");
                }
            }
            return Task.FromResult<object>(null);
        }
        public bool AllowMultiple
        {
            get { return true; }
        }
    }
}