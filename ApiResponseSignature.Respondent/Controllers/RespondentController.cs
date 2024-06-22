using Jws.Signature.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ApiResponseSignature.Respondent.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RespondentController : ControllerBase
    {
        [HttpGet]
        [SignResponseFilter]
        public ActionResult<object> Get()
        {
            return Ok(new
            {
                TestNum = 180,
                TestString = "Test",
                TestDate = DateTime.Now
            });
        }
    }
}
