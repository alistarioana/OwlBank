using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace OwlBank.Exceptions;

public class ProblemException(string error, string message)
    : Exception(message)
{
    public string Error { get; } = error;
}

public class ProblemExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(
    $"CUSTOM HANDLER CALLED: {exception.GetType().FullName}: {exception.Message}");
        if (exception is not ProblemException problemException)
        {
            return false;
        }

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = problemException.Error,
            Detail = problemException.Message,
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            Instance =
                $"{httpContext.Request.Method} {httpContext.Request.Path}"
        };

        problemDetails.Extensions["requestId"] =
            httpContext.TraceIdentifier;

        var activity = httpContext.Features
            .Get<Microsoft.AspNetCore.Http.Features.IHttpActivityFeature>()
            ?.Activity;

        if (activity?.Id is not null)
        {
            problemDetails.Extensions["traceId"] = activity.Id;
        }

        httpContext.Response.StatusCode =
            StatusCodes.Status400BadRequest;

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }
}