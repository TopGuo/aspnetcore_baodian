
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB.AspNetCore.Domain.Config;
using TB.AspNetCore.Infrastructrue.Extensions;
using TB.AspNetCore.Infrastructrue.Logs;

namespace TB.AspNetCore.Infrastructrue.Middleware.ErrorHandlerMiddleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger _logger;

        private readonly StatusCodePagesOptions _options;

        private static readonly int[] SuccessCode = new int[3]
        {
            200,
            301,
            302
        };

        public ErrorHandlerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IOptions<StatusCodePagesOptions> options)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<ErrorHandlerMiddleware>();
            _options = options.Value;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
                if (!SuccessCode.Contains(context.Response.StatusCode))
                {
                    int statusCode2 = context.Response.StatusCode;
                    string reasonPhrase = ReasonPhrases.GetReasonPhrase(statusCode2);
                    string msg = string.Format("Status Code: {0}, {1}; {2}{3}", new object[4]
                    {
                    statusCode2,
                    reasonPhrase,
                    context.Request.Path,
                    context.Request.QueryString
                    });
                    if (context.IsAjaxRequest())
                    {
                        context.Response.ContentType = "application/json;charset=utf-8";
                        ResponsResult response2 = new ResponsResult(statusCode2, msg, (Exception)null);
                        await context.Response.WriteAsync(response2.GetJson());
                    }
                }
            }
            catch (Exception exception)
            {
                int statusCode = SuccessCode.Contains(context.Response.StatusCode) ? 500 : context.Response.StatusCode;
                if (exception.TargetSite.DeclaringType != (Type)null)
                {
                    string name = exception.TargetSite.DeclaringType.Name;
                    if (name.Contains("ChallengeAsync"))
                    {
                        statusCode = 401;
                    }
                    else if (name.Contains("ForbidAsync"))
                    {
                        statusCode = 403;
                    }
                }
                Log4Net.Error($"[ErrorHandler中间件：]{exception}");
                Exception ex = exception;
                StringBuilder message = new StringBuilder();
                while (ex != null)
                {
                    message.AppendLine(ex.Message);
                    ex = ex.InnerException;
                }
                if (context.IsAjaxRequest() && !context.Response.HasStarted)
                {
                    context.Response.ContentType = "application/json;charset=utf-8";
                    ResponsResult response = new ResponsResult(statusCode, message.ToString(), (Exception)null);
                    await context.Response.WriteAsync(response.GetJson());
                }
            }
        }
    }

    /// <summary>
    /// mvc ErrorHandler 错误处理中间件
    /// </summary>
    public static partial class MiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
