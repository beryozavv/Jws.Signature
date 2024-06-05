using System.Security.Cryptography;
using System.Text;

namespace Jws.Signature.Handlers;

public class SignatureVerificationHandler : DelegatingHandler
{
    private readonly RSA _publicKey;

    public SignatureVerificationHandler(HttpMessageHandler innerHandler, string publicKeyPem)
        : base(innerHandler)
    {
        _publicKey = RSA.Create();
        _publicKey.ImportFromPem(publicKeyPem.ToCharArray());
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);
        var content = await response.Content.ReadAsStringAsync();

        // Extract the signature from the response
        var signatureIndex = content.LastIndexOf("\\n\\n-- Signature --\\n", StringComparison.Ordinal);
        if (signatureIndex == -1)
        {
            throw new InvalidOperationException("Response does not contain a signature.");
        }

        var originalContent = content.Substring(0, signatureIndex);
        var signature = content.Substring(signatureIndex + "\\n\\n-- Signature --\\n".Length);

        // Verify the signature
        if (!VerifySignature(originalContent, signature))
        {
            throw new InvalidOperationException("Invalid signature.");
        }

        // Remove the signature from the response content
        response.Content = new StringContent(originalContent, Encoding.UTF8, response.Content.Headers.ContentType.MediaType);

        return response;
    }

    private bool VerifySignature(string data, string signature)
    {
        var dataBytes = Encoding.UTF8.GetBytes(data);
        var signatureBytes = Convert.FromBase64String(signature);
        return _publicKey.VerifyData(dataBytes, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }
}