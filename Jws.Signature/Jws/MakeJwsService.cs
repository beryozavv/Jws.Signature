using System.Text;
using System.Text.Json;
using Jws.Signature.Signing;
using Microsoft.AspNetCore.WebUtilities;

namespace Jws.Signature.Jws;

internal sealed class MakeJwsService : IMakeJwsService
{
    private readonly ISignDataService _signDataService;

    public MakeJwsService(ISignDataService signDataService)
    {
        _signDataService = signDataService;
    }

    public string MakeJws(object payload, string protectedHeaderAlg = "RS256")
    {
        var headerBytes = Encoding.UTF8.GetBytes($"{{\"alg\":\"{protectedHeaderAlg}\"}}");

        var payloadBytes = JsonSerializer.SerializeToUtf8Bytes(payload);

        var headerBase64 = Base64UrlTextEncoder.Encode(headerBytes);
        var payloadBase64 = Base64UrlTextEncoder.Encode(payloadBytes);

        var dataToSign = headerBase64 + '.' + payloadBase64;

        var signatureData = _signDataService.SignData(dataToSign);

        return dataToSign + '.' + signatureData;
    }
}