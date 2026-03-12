using FundaAssignment.Core.Models;

namespace FundaAssignment.Core.Interfaces
{
    public interface IFundaApiClient
    {
        Task<IEnumerable<Listing>> GetAllListingsAsync(string searchQuery, CancellationToken cancellationToken = default);
    }
}
