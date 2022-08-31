using System.Net.Http.Headers;
using System.Text.Json;
using TwitterIntegration.Logic.Interface;
using TwitterIntegration.Models;

namespace TwitterIntegration.Logic
{
    public class TwitterSampleLogic : ITwitterSampleLogic
    {
        private readonly ILogger<TwitterSampleLogic> logger;
        private readonly string bearerToken = "AAAAAAAAAAAAAAAAAAAAAGSngQEAAAAAXsj9L%2FsacwQU6rkcSHCM5L7TFcA%3DoWtqSLUJzqPtlKE19Im3kqvmIcbOANLEX9AhA0LV6V3vThL5kZ";

        public TwitterSampleLogic(ILogger<TwitterSampleLogic> logger)
        {
            this.logger = logger;
        }
        public async Task<TwitterSampleResponse> GetTwitterSampleStream(TwitterSampleRequest request, CancellationToken cancellationToken)
        {
            logger.LogTrace("GetTwitterSampleStream : start");

            if (cancellationToken.IsCancellationRequested)
            {
                logger.LogDebug($"GetTwitterSampleStream cancelled: {DateTime.UtcNow}", new object[] { request });
                throw new OperationCanceledException("Getting twitter sample stream operation is cancelled");
            }

            TwitterSampleResponse? result;
            using HttpClient httpClient = new HttpClient();
            var uri = new Uri("https://api.twitter.com/2/tweets/sample/stream");
            var authHeader = new AuthenticationHeaderValue("Bearer", bearerToken);
            httpClient.DefaultRequestHeaders.Add("Bearer", bearerToken);

            var response = await httpClient.GetAsync(uri).ConfigureAwait(false);

            if (cancellationToken.IsCancellationRequested)
            {
                logger.LogDebug($"GetTwitterSampleStream cancelled: {DateTime.UtcNow}", new object[] { request });
                throw new OperationCanceledException("Getting twitter sample stream operation is cancelled");
            }

            if (!response.IsSuccessStatusCode)
            {
                logger.LogDebug($"GetTwitterSampleStream failed on API call: {DateTime.UtcNow}", new object[] { response.ReasonPhrase ?? "" });
                throw new InvalidOperationException("Unable to get the Twitter sample stream");
            }

            var responseContent = "";
            using (var reader = new StreamReader(response.Content.ReadAsStream(cancellationToken)))
            {
                responseContent = await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            result = JsonSerializer.Deserialize<TwitterSampleResponse>(responseContent);
            if (result is null)
            {
                logger.LogDebug($"GetTwitterSampleStream failed: {DateTime.UtcNow}", responseContent);
                throw new InvalidOperationException("Twitter sample stream is null or empty");
            }

            logger.LogTrace("GetTwitterSampleStream : end");
            return result;
        }
    }
}
