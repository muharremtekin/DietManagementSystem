using DietManagementSystem.Application.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace DietManagementSystem.WebApi.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this WebApplication app)
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

                    var errorDetail = new ErrorDetail
                    {
                        StatusCode = contextFeature.Error switch
                        {
                            NotFoundException => StatusCodes.Status404NotFound,
                            ForbiddenException => StatusCodes.Status403Forbidden,
                            BadRequestException => StatusCodes.Status400BadRequest,
                            ValidationException validationEx => StatusCodes.Status400BadRequest,
                            _ => StatusCodes.Status500InternalServerError
                        },
                        Message = contextFeature.Error.Message
                    };


                    context.Response.StatusCode = errorDetail.StatusCode;

                    // ValidationException için özel hata listesi
                    if (contextFeature.Error is ValidationException validationException)
                    {
                        errorDetail.Message = "Doğrulama hatası oluştu.";
                        errorDetail.Errors = validationException.Errors.Select(e => e.ErrorMessage).ToList();
                    }

                    await context.Response.WriteAsync(errorDetail.ToString());
                }
            });
        });
    }
}