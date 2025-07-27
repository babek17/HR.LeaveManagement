using System;
using System.Net;
using HR.LeaveManagement.Api.Models;
using HR.LeaveManagement.Application.Exceptions;

namespace HR.LeaveManagement.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
    {
        HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
        object problem;

        switch (ex)
        {
            case BadRequestException badRequestException:
                httpStatusCode = HttpStatusCode.BadRequest;
                problem = new CustomValidationProblemDetails
                {
                    Title = badRequestException.Message,
                    Status = (int)httpStatusCode,
                    Detail = badRequestException.InnerException?.Message,
                    Type = nameof(BadRequestException),
                    Errors = badRequestException.ValidationErrors
                };
                break;
            case NotFoundException notFoundException:
                httpStatusCode = HttpStatusCode.NotFound;
                problem = new
                {
                    Title = notFoundException.Message,
                    Status = (int)httpStatusCode,
                    Detail = notFoundException.InnerException?.Message,
                    Type = nameof(NotFoundException)
                };
                break;
            default:
                problem = new
                {
                    Title = ex.Message,
                    Status = (int)httpStatusCode,
                    Detail = ex.StackTrace,
                    Type = ex.GetType().Name
                };
                break;
        }

        httpContext.Response.StatusCode = (int)httpStatusCode;
        await httpContext.Response.WriteAsJsonAsync(problem);
    }
}
