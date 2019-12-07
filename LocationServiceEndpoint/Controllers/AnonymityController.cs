using LocationServiceEndpoint.Anonymizer;
using Microsoft.AspNetCore.Mvc;

namespace LocationServiceEndpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnonymityController : ControllerBase
    {
        private static Anonymizer.Anonymizer _anonymizer;

        public AnonymityController()
        {
            if (_anonymizer == null)
            {
                _anonymizer = new Anonymizer.Anonymizer();
            }
        }

        // POST: api/anonymity
        [HttpPost]
        public AnonymizedLocation AnonymizeLocation([FromBody] OriginalLocation location)
        {
            return _anonymizer.Anonymize(location);
        }

        // POST: api/anonymity/fictive
        [HttpPost]
        [Route("fictive")]
        public IActionResult AddFictive([FromBody] OriginalLocation location)
        {
            _anonymizer.AddMessage(location);
            return new ObjectResult("Success");
        }

        // GET: api/anonymity/reset
        [HttpGet]
        [Route("reset")]
        public IActionResult Reset()
        {
            _anonymizer = new Anonymizer.Anonymizer();
            return new ObjectResult("Success");
        }
    }
}
