using Euvic.Cqrs.PipelineBehaviors.Permissions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Euvic.StaffTraining.WebAPI.Filters
{
    public class InsufficientPermissionsExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (!(context.Exception is InsufficientPermissionsExceptions permissionException))
                return;

            context.Result = new ObjectResult(new { Message = "Insufficient permissions" })
            {
                ContentTypes = { "application/json" },
                StatusCode = StatusCodes.Status403Forbidden
            };

            context.ExceptionHandled = true;
        }
    }
}
