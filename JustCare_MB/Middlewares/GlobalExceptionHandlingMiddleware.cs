using JustCare_MB.Helpers;
using JustCare_MB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JustCare_MB.Middlewares
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(
            ILogger<GlobalExceptionHandlingMiddleware> logger)
            => _logger = logger;

        private ProblemDetails problem { get; set; }
        private bool flag = true;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
                flag = false;
            }
            catch (InvalidUserPasswordOrUserNotExistException ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode
                    = (int)HttpStatusCode.BadRequest;
                problem = new()
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Type = "Invalid Password",
                    Title = "Invalid Password",
                    Detail = "a Invalid Password Error has occurred"

                };
            }catch(NotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode
                    = (int)HttpStatusCode.NotFound;
                problem = new()
                {
                    Status = (int)HttpStatusCode.NotFound,
                    Type = ex.Message,
                    Title = ex.Message,
                    Detail =string.Format( "a {0} Error", ex.Message)
                };
            }
            catch (EmptyFieldException ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode
                    = (int)HttpStatusCode.BadRequest;
                problem = new()
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Type = ex.Message,
                    Title = ex.Message,
                    Detail = string.Format("a {0} Error", ex.Message)
                };
            }
            catch (ExistsException ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode
                    = (int)HttpStatusCode.Conflict;
                problem = new()
                {
                    Status = (int)HttpStatusCode.Conflict,
                    Type = ex.Message,
                    Title = ex.Message,
                    Detail = string.Format("a {0} Error", ex.Message)
                };

            }
            catch (InvalidIdException ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode
                    = (int)HttpStatusCode.BadRequest;
                problem = new()
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Type = ex.Message,
                    Title = ex.Message,
                    Detail = string.Format("a {0} Error", ex.Message)
                };
            }
            catch (TimeNotValid ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode
                    = (int)HttpStatusCode.BadRequest;
                problem = new()
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Type = ex.Message,
                    Title = ex.Message,
                    Detail = string.Format("a {0} Error", ex.Message)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode
                    = (int)HttpStatusCode.BadRequest;
                problem = new()
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Type = ex.Message,
                    Title = ex.Message,
                    Detail = string.Format("a {0} Error", ex.Message)
                };
            }
            finally
            {
                if (flag)
                {
                    //var problemJson = JsonConvert.SerializeObject(problem);
                    //// Set the response content type to JSON
                    //context.Response.ContentType = "application/json";
                    //// Write the serialized problem object to the response body
                    //await context.Response.WriteAsync(problemJson);

                    string json = JsonSerializer.Serialize(problem);
                    // Set the response content type to JSON
                    context.Response.ContentType = "application/json";
                    // Write the serialized problem object to the response body
                    await context.Response.WriteAsync(json);

                }
            }
        }
    }
}
