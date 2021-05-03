using Domain.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using WebAPI.ViewModels.Common;

namespace WebAPI.Extensions
{
    public static class ExceptionHandlerExtensions
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(new ExceptionHandlerOptions()
            {
                AllowStatusCode404Response = true,
                ExceptionHandler = async (context) =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var feature = context.Features.Get<IExceptionHandlerFeature>();

                    Exception? exc = feature?.Error;

                    ILogger logger = context.RequestServices.GetRequiredService<ILogger<Startup>>();

                    ErrorViewModel errorModel = CreateError(context, exc, logger);

                    context.Response.StatusCode = errorModel.Status;

                    await context.Response.WriteAsync(errorModel.Error);
                }
            });
        }

        private static ErrorViewModel CreateError(HttpContext context, Exception? exc, ILogger logger)
        {
            string message = exc?.Message ?? string.Empty;

            return exc switch
            {
                AuthenticationException custom => SpecificError(context, custom, StatusCodes.Status403Forbidden, message, logger),
                ActionRejectedException custom => SpecificError(context, custom, StatusCodes.Status422UnprocessableEntity, message, logger),
                _ => DefaultError(context, exc, logger),
            };
        }

        private static ErrorViewModel DefaultError(HttpContext context, Exception? exc, ILogger? logger)
        {
            string messageError = "Ocorreu um erro interno.";
            return SpecificError(context, exc, StatusCodes.Status500InternalServerError, messageError, logger);
        }

        private static ErrorViewModel SpecificError(HttpContext context, Exception? exc, int statusCode, string message, ILogger? logger)
        {
            string rotina = GetRoute(context);

            string logMessage = message + Environment.NewLine + "Rota da API acessada no request: " + rotina;

            logger.LogError(exc, logMessage);

            return new ErrorViewModel(statusCode, message);
        }

        private static string GetRoute(HttpContext context)
        {
            return $"[{context.Request.Method}] {context.Request.Path}";
        }
    }
}