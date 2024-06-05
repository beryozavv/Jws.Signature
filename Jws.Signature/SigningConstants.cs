using System.Security.Cryptography;

namespace Jws.Signature;

internal static class SigningConstants
{
    public static readonly HashAlgorithmName HashAlgorithmName = HashAlgorithmName.SHA256;
    public static readonly RSASignaturePadding Padding = RSASignaturePadding.Pkcs1;
}