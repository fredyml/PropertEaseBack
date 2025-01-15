using Microsoft.AspNetCore.Mvc.Filters;
using PropertEase.Domain.Exceptions;
using System.Net;
using Serilog;
using Microsoft.AspNetCore.Mvc;
using PropertEase.Models;

namespace PropertEase.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            ErrorResponse? errorResponse;

            if (context.Exception is PropertyNotFoundException notFoundEx)
            {
                errorResponse = new ErrorResponse
                {
                    Message = "Propiedad no encontrada.",
                    Details = notFoundEx.Message
                };

                context.Result = new JsonResult(errorResponse)
                {
                    StatusCode = (int)HttpStatusCode.NotFound
                };

                Log.Error($"Propiedad no encontrada: {notFoundEx.Message}");
            }
            else
            {
                Log.Error(context.Exception, "Se ha producido una excepción no controlada.");

                errorResponse = new ErrorResponse
                {
                    Message = "Se ha producido un error inesperado.",
                    Details = context.Exception.Message
                };

                context.Result = new JsonResult(errorResponse)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }
    }
}
