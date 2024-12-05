using EclipseTaskManager.Models;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace EclipseTaskManager.Extensions;

public static class ApiExceptionMiddlewareExtensions
{
    public static void ConfigureDevelopmentExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    await context.Response.WriteAsync(new ErrorDetails()
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = contextFeature.Error.Message,
                        Trace = contextFeature.Error.StackTrace
                    }.ToString());
                }
            });
        });
    }

    public static void ConfigureProductionExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(new ErrorDetails
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "An unexpected error occurred. Please try contact our support team."
                }.ToString());
            });
        });

    }

}
