using Demo.CosmoDB.Models;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using Demo.CosmoDB.Comman.Extensions;
namespace Demo.CosmoDB.WebAPI.Middlewares
{
    public static class ExceptionMiddlewareExtensions
    {
        private static readonly string _internalErrorMessage = "Internal error occured durring the processing of the request. Please contact system Administrator @9718623229. Tracking number: ";
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var trackingNumber = Guid.NewGuid().ToString();
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        logger.LogError(contextFeature.Error, $"An {contextFeature.Error.GetType().Name} error occured durring the processing of the request. Tracking Number {{TrackingNumber}}. Request: {{HttpReqeust}}", trackingNumber, context.Request.ToMapString());
                        var message = $"{_internalErrorMessage}{trackingNumber}";
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = message
                        }.ToString());
                    }
                });
            });
        }
    }
}
