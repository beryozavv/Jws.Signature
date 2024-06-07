namespace Jws.Signature.Jws;

public interface IMakeJwsService
{
    string MakeJws(object payload, string protectedHeaderAlg = "RS256");
}