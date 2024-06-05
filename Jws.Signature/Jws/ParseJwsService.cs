using System.Text.Json;
using Jws.Signature.Signing;

namespace Jws.Signature.Jws;

internal class ParseJwsService : IParseJwsService
{
    private readonly IVerifySignService _verifySignService;

    public ParseJwsService(IVerifySignService verifySignService)
    {
        _verifySignService = verifySignService;
    }

    public T ParseJws<T>(string jws)
    {
        var lastIndexOfDot = jws.LastIndexOf('.');
        var data = jws.Substring(0, lastIndexOfDot);

        var parts = jws.Split('.');
        var signature = parts[2];

        if (!_verifySignService.VerifySign(data, signature))
        {
            throw new InvalidOperationException("todo");
        }

        var payloadBase64 = parts[1];
        //Convert.TryFromBase64String(payloadBase64, out var span,out var ) todo
        var payload = Convert.FromBase64String(payloadBase64);
        var result = JsonSerializer.Deserialize<T>(payload);

        if (result == null)
        {
            throw new InvalidOperationException("todo");
        }

        return result;
    }
}