using LaesoeLineApi.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LaesoeLineApi.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ApiException apiException)
            {
                if (apiException.Status == ApiStatus.GatewayTimeout)
                {
                    context.Result = new JsonResult(new BookErrorResult(apiException.Status))
                    {
                        StatusCode = StatusCodes.Status504GatewayTimeout
                    };
                }
                else
                {
                    context.Result = new JsonResult(new BookErrorResult(apiException.Status))
                    {
                        StatusCode = StatusCodes.Status422UnprocessableEntity
                    };
                }
            }
        }
    }
}
