using Jws.Signature.Jws;
using System.Text;

namespace Jws.Signature.Handlers;

public class SignatureVerificationHandler : DelegatingHandler
{
    private readonly IParseJwsService _parseJwsService;

    public SignatureVerificationHandler(IParseJwsService parseJwsService)
    {
        _parseJwsService = parseJwsService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        var content = await response.Content.ReadAsStringAsync();

        var serializedResponse = _parseJwsService.GetSerializedResponse(content);

        response.Content = new StringContent(serializedResponse, Encoding.UTF8, response.Content.Headers.ContentType?.MediaType);

        return response;
    }
}