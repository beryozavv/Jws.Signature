using Jws.Signature.Jws;
using System.Text;

namespace Jws.Signature.Handlers;

public class SignatureVerificationHandler : DelegatingHandler
{
    private readonly IParseJwsService _parseJwsService;

    public SignatureVerificationHandler(IParseJwsService parseJwsService)
        : base()
    {
        _parseJwsService = parseJwsService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        var content = await response.Content.ReadAsStringAsync();

        var decodedResponse = _parseJwsService.ParseJws<string>(content);

        response.Content = new StringContent(decodedResponse, Encoding.UTF8, response.Content.Headers.ContentType?.MediaType);

        return response;
    }
}