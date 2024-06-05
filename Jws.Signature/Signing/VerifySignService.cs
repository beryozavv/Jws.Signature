using System.Security.Cryptography;
using System.Text;

namespace Jws.Signature.Signing;

internal class VerifySignService : IVerifySignService
{
    private readonly RSA _publicKey;
    
    public VerifySignService(string publicKeyPem) //todo вынести?
    {
        // todo inject service-getter for public key
        
        _publicKey = RSA.Create();
        _publicKey.ImportFromPem(publicKeyPem.ToCharArray());
    }

    /// <summary>
    /// Verifies that a digital signature is valid for the specified data
    /// </summary>
    /// <param name="data">string data in UTF8</param>
    /// <param name="signature">Base64-encoded signature</param>
    /// <returns>true if signature is valid</returns>
    public bool VerifySign(string data, string signature)
    {
        var dataBytes = Encoding.UTF8.GetBytes(data);
        var signatureBytes = Convert.FromBase64String(signature);
        return _publicKey.VerifyData(dataBytes, signatureBytes, SigningConstants.HashAlgorithmName,
            SigningConstants.Padding);
    }
}