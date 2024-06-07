using Jws.Signature.Handlers;
using Jws.Signature.Jws;
using Jws.Signature.KeyExtractor;
using Jws.Signature.Signing;
using Microsoft.Extensions.DependencyInjection;

namespace Jws.Signature.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddResponseSigning(this IServiceCollection services)
    {
        services
            .AddSingleton<IPrivateKeyExtractor, DefaultPrivateKeyExtractor>();

        services
            .AddTransient<ISignDataService, SignDataRsaService>()
            .AddTransient<IMakeJwsService, MakeJwsService>();

        return services;
    }

    public static IServiceCollection AddResponseVerifying(this IServiceCollection services)
    {
        services
            .AddSingleton<IPublicKeyExtractor, DefaultPublicKeyExtractor>();

        services
            .AddTransient<IVerifySignService, VerifySignService>()
            .AddTransient<IParseJwsService, ParseJwsService>()
            .AddTransient<SignatureVerificationHandler>();

        return services;
    }
}
