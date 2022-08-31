using Microsoft.AspNetCore.Mvc;
using TwitterIntegration.Logic.Interface;
using TwitterIntegration.Models;

namespace TwitterIntegration.Controllers
{
    [Route("TweetsSample")]
    public class TwitterSampleStreamController : Controller
    {
        private readonly ITwitterSampleLogic twitterSampleLogic;
        private readonly ILogger<TwitterSampleStreamController> logger;
        public TwitterSampleStreamController(ILogger<TwitterSampleStreamController> logger, ITwitterSampleLogic twitterSampleLogic)
        {
            this.logger = logger;
            this.twitterSampleLogic = twitterSampleLogic;

        }

        [HttpGet]
        public async Task<IActionResult> GetTwitterSampleStreamAsync(
            [FromBody] TwitterSampleRequest request,
            CancellationToken cancellationToken)
        {
            if (request == null)
                return BadRequest();

            try
            {
                var result = await twitterSampleLogic.GetTwitterSampleStream(request, cancellationToken).ConfigureAwait(false);

                if (result is not null)
                    return Ok(result);

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
