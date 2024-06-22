using System.Text;
using System.Text.Json;
using Jws.Signature.Signing;
using Microsoft.AspNetCore.WebUtilities;

namespace Jws.Signature.Jws;

internal sealed class ParseJwsService : IParseJwsService
{
    private readonly IVerifySignService _verifySignService;

    public ParseJwsService(IVerifySignService verifySignService)
    {
        _verifySignService = verifySignService;
    }

    public T ParseJws<T>(string jws)
    {
        var payload = ParsePayload(jws);
        var result = JsonSerializer.Deserialize<T>(payload);

        if (result == null)
        {
            throw new InvalidOperationException($"Cannot deserialize payload to type {typeof(T).Name}.");
        }

        return result;
    }

    private byte[] ParsePayload(string jws)
    {
        var lastIndexOfDot = jws.LastIndexOf('.');
        var data = jws.Substring(0, lastIndexOfDot);

        var parts = jws.Split('.');
        var signature = parts[2];

        if (!_verifySignService.VerifySign(data, signature))
        {
            throw new InvalidOperationException("Verification failed.");
        }

        var payloadBase64 = parts[1];

        var payload = Base64UrlTextEncoder.Decode(payloadBase64);
        return payload;
    }

    public string GetSerializedResponse(string jws)
    {
        var payload = ParsePayload(jws);
        return Encoding.UTF8.GetString(payload);
    }
}