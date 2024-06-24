using Jws.Signature.KeyExtractor;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Cryptography;
using System.Text;

namespace Jws.Signature.Signing;

internal sealed class VerifySignService : IVerifySignService
{
    private readonly RSA _publicKey;
    
    public VerifySignService(IPublicKeyExtractor publicKeyExtractor) //todo вынести?
    {
        _publicKey = publicKeyExtractor.GetKey();
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

        var signatureBytes = Base64UrlTextEncoder.Decode(signature);

        return _publicKey.VerifyData(
            dataBytes, 
            signatureBytes,
            SigningConstants.HashAlgorithmName,
            SigningConstants.Padding);
    }
}