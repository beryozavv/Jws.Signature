using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Jws.Signature.Middlewares;


public class ResponseSigningMiddleware
{
    private readonly RequestDelegate _next;
    private readonly RSA _privateKey;

    public ResponseSigningMiddleware(RequestDelegate next)
    {
        _next = next;
        _privateKey = RSA.Create();

        // Load your private key here (this is just a placeholder)
        string privateKey = "-----BEGIN RSA PRIVATE KEY-----\\n...";
        _privateKey.ImportFromPem(privateKey.ToCharArray());
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Keep a reference to the original response body stream
        var originalBodyStream = context.Response.Body;

        using (var responseBody = new MemoryStream())
        {
            // Replace the response body with our stream
            context.Response.Body = responseBody;

            // Continue down the Middleware pipeline
            await _next(context);

            // Read the response body
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            // Sign the response body
            var signature = SignData(responseBodyText);

            // Append the signature to the response body
            var signedResponseBodyText = responseBodyText + "\\n\\n-- Signature --\\n" + signature;

            // Convert the modified text back to a byte array
            var modifiedBytes = Encoding.UTF8.GetBytes(signedResponseBodyText);

            // Write the modified response back to the original response body stream
            await originalBodyStream.WriteAsync(modifiedBytes, 0, modifiedBytes.Length);
        }
    }

    private string SignData(string data)
    {
        var dataBytes = Encoding.UTF8.GetBytes(data);
        var signedBytes = _privateKey.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        return Convert.ToBase64String(signedBytes);
    }
}
