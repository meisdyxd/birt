using Microsoft.AspNetCore.Mvc;

namespace StreamingService.Controllers
{
    [ApiController]
    [Route("api/stream")]
    public class StreamingController : ControllerBase
    {
        private readonly ILogger<StreamingController> _logger;

        public StreamingController(ILogger<StreamingController> logger)
        {
            _logger = logger;
        }
    }
}
