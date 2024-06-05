namespace Jws.Signature.Jws;

public interface IParseJwsService
{
    T ParseJws<T>(string jws);
}