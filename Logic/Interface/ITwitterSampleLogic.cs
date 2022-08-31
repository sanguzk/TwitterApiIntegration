using TwitterIntegration.Models;

namespace TwitterIntegration.Logic.Interface
{
    public interface ITwitterSampleLogic
    {
        Task<TwitterSampleResponse> GetTwitterSampleStream(TwitterSampleRequest request, CancellationToken cancellationToken);
    }
}
