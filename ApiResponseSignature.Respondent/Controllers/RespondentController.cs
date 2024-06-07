using Microsoft.AspNetCore.Mvc;

namespace ApiResponseSignature.Respondent.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RespondentController : ControllerBase
    {
        [HttpGet]
        public ActionResult<object> Get()
        {
            return Ok(new
            {
                TestString = "Test"
            });
        }
    }
}
