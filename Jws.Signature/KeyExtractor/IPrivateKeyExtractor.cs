using System.Security.Cryptography;

namespace Jws.Signature.KeyExtractor;

public interface IPrivateKeyExtractor
{
    RSA GetKey();

    Task<RSA> GetKeyAsync(CancellationToken cancellationToken);
}
