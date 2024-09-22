using System.Net;
using Newtonsoft.Json;
using WebService.Exceptions;

namespace WebService.Middlewares;

public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        if (exception == null)
            return;
        
        if (exception is CommonError commonError)
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorDto(commonError)));
            
            return;
        }

        var error = new CommonError(500, "Unexpected error occured");
            
        logger.LogError(exception, "Unexpected error occured");

        var result = JsonConvert.SerializeObject(error);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsync(result);
    }

    private class ErrorDto(CommonError commonError)
    {
        public int Code { get; set; } = commonError.Code;
        public string Text { get; set; } = commonError.Text;
    }
}