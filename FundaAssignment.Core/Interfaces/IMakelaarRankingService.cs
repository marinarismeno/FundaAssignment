using FundaAssignment.Core.Models;

namespace FundaAssignment.Core.Interfaces
{
    public interface IMakelaarRankingService
    {
        public IEnumerable<MakelaarListingCount> GetTopMakelaars(IEnumerable<Listing> listings);
    }
}
