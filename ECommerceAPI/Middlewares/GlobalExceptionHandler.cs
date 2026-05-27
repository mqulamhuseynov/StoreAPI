using Domain.Commons.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ECommerceAPI.Middlewares
{
    //exception handlerdan miras alirig
    public class GlobalExceptionHandler : IExceptionHandler
    {
        //logger inject edirik ve ctor qururug
        private readonly ILogger<GlobalExceptionHandler> _logger;
        
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            //loger ile exception loglanir
            _logger.LogError(exception, "An unhandled error has occurred : {Message}", exception.Message);

            //exceptionun tipine gore switch ile status code ve title belirlenir
            var (statusCode, title) = exception switch 
            {
                NotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
                BadRequestException => (StatusCodes.Status400BadRequest, "Bad Request"),
                UnauthorizedException => (StatusCodes.Status401Unauthorized, "Unauthorized"),
                ForbiddenException => (StatusCodes.Status403Forbidden, "Forbidden"),
                ConflictException => (StatusCodes.Status409Conflict, "Conflict"),
                //hecbiri ile uygun gelmese server error veririy
                _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
            };

            //problem details obyekti yaradilir ve doldurulur
            var problemDetails = new ProblemDetails
            {
             Status = (int)statusCode,
             Title = title,
             Detail = exception.Message,
             Instance = httpContext.Request.Path
            };

            //eger status code 500 dirsə, istifadəçiyə daha ümumi bir mesaj göstərilir
            if (statusCode == StatusCodes.Status500InternalServerError) 
            {
                problemDetails.Detail = "An unexpected error occurred. Please try again later.";
            }


            //responeseler status code ve content type verilir ve json formatında yazilir
            httpContext.Response.StatusCode = (int)statusCode;  
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;

        }
    }
}
