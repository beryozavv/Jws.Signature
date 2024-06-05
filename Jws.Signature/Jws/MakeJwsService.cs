using System.Text;
using System.Text.Json;
using Jws.Signature.Signing;

namespace Jws.Signature.Jws;

internal class MakeJwsService : IMakeJwsService
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

        var headerBase64 = Convert.ToBase64String(headerBytes);
        var payloadBase64 = Convert.ToBase64String(payloadBytes);

        var dataToSign = headerBase64 + '.' + payloadBase64;

        var signatureData = _signDataService.SignData(dataToSign);

        return dataToSign + '.' + signatureData;
    }
}