using Jws.Signature.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Jws.Signature.Extensions;

public static class HttpClientBuilderExtensions
{
    public static IHttpClientBuilder AddResponseVerifyingHandler(this IHttpClientBuilder builder)
    {
        return builder.AddHttpMessageHandler<SignatureVerificationHandler>();
    }
}
