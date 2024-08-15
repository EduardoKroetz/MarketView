using Microsoft.AspNetCore.Diagnostics;
using src.DTOs;
using System.Net;

namespace src.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var result = Result.BadResult(exception.Message);

        switch (exception)
        {
            case ArgumentException e:
            case InvalidOperationException:
            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            break;
            default:
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            result.Message = "Ocorreu um erro ao buscar os dados, tente novamente mais tarde";
            break;
        }

        await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);
        return true;
    }
}
