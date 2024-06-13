namespace Jws.Signature.Signing;

public interface ISignDataService
{
    /// <summary>
    /// Computes the hash of the specified data and signs this hash
    /// </summary>
    /// <param name="data">string data in UTF8</param>
    /// <returns>The Base64 encoded signature for the specified data</returns>
    string SignData(string data);
    
    string HashAlg { get; }
}