using System.Security.Cryptography;

namespace Jws.Signature.KeyExtractor;

public interface IPublicKeyExtractor
{
    RSA GetKey();

    Task<RSA> GetKeyAsync(CancellationToken cancellationToken);
}
