using Jws.Signature.Jws;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Jws.Signature.Filters;

public sealed class SignResponseFilterAttribute : Attribute, IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        var makeJwsService = context.HttpContext.RequestServices.GetRequiredService<IMakeJwsService>();

        if (context.Result is ObjectResult objectResult)
        {
            if (objectResult.Value is null)
            {
                throw new InvalidOperationException("Cannot sign empty payload.");
            }

            objectResult.Value = makeJwsService.MakeJws(objectResult.Value);
        }

        await next();
    }
}
