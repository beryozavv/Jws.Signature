using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text;

namespace Jws.Signature.Signing;

internal class SignDataRsaService : ISignDataService
{
    private readonly RSA _privateKey;

    public SignDataRsaService(string privateKey) // todo вынести из конструктора?
    {
        // todo inject service-getter for private key
        
        _privateKey = RSA.Create();

        // Load your private key here (this is just a placeholder)
        
        _privateKey.ImportFromPem(privateKey.ToCharArray());
    }

    /// <summary>
    /// Computes the hash of the specified data and signs this hash
    /// </summary>
    /// <param name="data">string data in UTF8</param>
    /// <returns>Base64 encoded signature for the specified data</returns>
    public string SignData(string data)
    {
        var dataBytes = Encoding.UTF8.GetBytes(data);
        var signedBytes = _privateKey.SignData(dataBytes, SigningConstants.HashAlgorithmName, SigningConstants.Padding);
        return Convert.ToBase64String(signedBytes);
    }
}