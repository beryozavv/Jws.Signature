namespace Jws.Signature;

public interface IVerifySignService
{
    /// <summary>
    /// Verifies that a digital signature is valid for the specified data
    /// </summary>
    /// <param name="data">string data in UTF8</param>
    /// <param name="signature">Base64-encoded signature</param>
    /// <returns>true if signature is valid</returns>
    bool VerifySign(string data, string signature);
}