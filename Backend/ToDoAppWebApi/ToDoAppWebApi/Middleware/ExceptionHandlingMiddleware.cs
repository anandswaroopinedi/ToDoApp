
using BusinessLogicLayer.Interfaces;
using Models;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace ToDoAppWebApi.Middleware
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IErrorLogManager _errorLogManager;
        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger, IErrorLogManager errorLogManager)
        {
            _logger = logger;
            _errorLogManager = errorLogManager;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message, ex);
                ErrorLogDto error = await CreateErrorModel(ex);
                _errorLogManager.addError(error);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var apiResponse = new ApiResponse
                {
                    Status = 2,
                    Message = "Something Went Wrong"
                };

                var json = JsonSerializer.Serialize(apiResponse);
                await context.Response.WriteAsync(json);
            }
        }
        public async Task<ErrorLogDto> CreateErrorModel(Exception ex)
        {
            ErrorLogDto error = new ErrorLogDto();
            error.Errormessage = ex.Message;
            error.Filename = ex.TargetSite.DeclaringType.FullName;
            error.Stacktrace = ex.StackTrace;
            error.Timestamp = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
            error.Methodname = ex.TargetSite.DeclaringType.Name;
            if (ex.StackTrace != null)
            {
                var stackTraceLines = ex.StackTrace.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                if (stackTraceLines.Length > 0)
                {
                    var firstLine = stackTraceLines[0];
                    var fileLineIndex = firstLine.LastIndexOf(" in ");
                    if (fileLineIndex > -1)
                    {
                        var fileAndLine = firstLine.Substring(fileLineIndex + 4);
                        var fileParts = fileAndLine.Split(':');
                        if (fileParts.Length == 3)
                        {
                            error.Filename = fileParts[0] + ':' + fileParts[1];
                            error.Linenumber = int.Parse(fileParts[2].Split(' ')[1]);
                        }
                    }
                }
            }
            return error;
        }
    }
}
