using Jws.Signature.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Jws.Signature.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseResponseSigning(this IApplicationBuilder applicationBuilder)
    {
        return applicationBuilder.UseMiddleware<ResponseSigningMiddleware>();
    }
}
