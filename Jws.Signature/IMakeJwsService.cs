using System.Text;
using System.Text.Json;

namespace Jws.Signature;

public interface IMakeJwsService
{
    string MakeJws(object content, string protectedHeaderAlg);
}

public interface IParseJwsService
{
    T ParseJws<T>(string jws);
}

internal class MakeJwsService : IMakeJwsService
{
    private readonly ISignDataService _signDataService;

    public MakeJwsService(ISignDataService signDataService)
    {
        _signDataService = signDataService;
    }

    public string MakeJws(object content, string protectedHeaderAlg)
    {
        var headerBytes = Encoding.UTF8.GetBytes(protectedHeaderAlg);

        var payloadBytes = JsonSerializer.SerializeToUtf8Bytes(content);

        var headerBase64 = Convert.ToBase64String(headerBytes);
        var payloadBase64 = Convert.ToBase64String(payloadBytes);

        var dataToSign = headerBase64 + '.' + payloadBase64;

        var signatureData = _signDataService.SignData(dataToSign);

        return dataToSign + '.' + signatureData;
    }

    public T ParseJws<T>(string jws)
    {
        throw new NotImplementedException();
    }
}

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
        var data = jws.Substring(0,lastIndexOfDot);
        var signature = jws.Substring(lastIndexOfDot);

        if (!_verifySignService.VerifySign(data, signature))
        {
            throw new InvalidOperationException("todo");
        }

        var firstIndexOfDot = jws.IndexOf('.');
        var payloadBase64 = jws.Substring(firstIndexOfDot,lastIndexOfDot);
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