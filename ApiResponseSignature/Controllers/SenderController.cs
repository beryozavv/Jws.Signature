using Microsoft.AspNetCore.Mvc;

namespace ApiResponseSignature.Controllers;

[ApiController]
[Route("[controller]")]
public class SenderController : ControllerBase
{
    private readonly HttpClient _client; 

    public SenderController(IHttpClientFactory factory )
    {
        _client = factory.CreateClient("TestClient");
    }

    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var response = await _client.GetAsync("Respondent");

        return Ok(await response.Content.ReadAsStringAsync());
    }
}