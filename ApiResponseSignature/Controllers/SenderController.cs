using ApiResponseSignature.Sender.RefitClients;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace ApiResponseSignature.Controllers;

[ApiController]
[Route("[controller]")]
public class SenderController : ControllerBase
{
    private readonly HttpClient _client;
    private readonly IRespondenApi _refitClient;

    public SenderController(IHttpClientFactory factory, IRespondenApi refitClient)
    {
        _client = factory.CreateClient("TestClient");
        _refitClient = refitClient;
    }

    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var response = await _client.GetAsync("Respondent");

        return Ok(await response.Content.ReadAsStringAsync());
    }

    [HttpGet("refit")]
    public async Task<ActionResult> GetRefit()
    {
        var response = await _refitClient.Test();
        return Ok(response);
    }
}