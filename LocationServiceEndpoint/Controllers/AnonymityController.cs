using LocationServiceEndpoint.Anonymizer;
using Microsoft.AspNetCore.Mvc;

namespace LocationServiceEndpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnonymityController : ControllerBase
    {
        private readonly Anonymizer.Anonymizer _anonymizer;

        public AnonymityController(Anonymizer.Anonymizer anonymizer)
        {
            _anonymizer = anonymizer;
        }

        // POST: api/Anonymity
        [HttpPost]
        public AnonymizedLocation AnonymizeLocation([FromBody] OriginalLocation location)
        {
            return _anonymizer.Anonymize(location);
        }
    }
}
