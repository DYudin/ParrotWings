
using System.Web.Http;

namespace ParrotWings.Controllers
{
    public class WarmupController : ApiController
    {
        // Needed for warm up start
        [Route("api/warmup/start")]
        [HttpGet]
        public IHttpActionResult Start()
        {
            return Ok("Warm up completed");
        }
    }
}
