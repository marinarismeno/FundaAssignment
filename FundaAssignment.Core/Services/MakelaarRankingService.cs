using FundaAssignment.Core.Interfaces;
using FundaAssignment.Core.Models;

namespace FundaAssignment.Core.Services
{
    public class MakelaarRankingService : IMakelaarRankingService
    {
        public IEnumerable<MakelaarListingCount> GetTopMakelaars(IEnumerable<Listing> listings)
        {
            IEnumerable<MakelaarListingCount> topMakelaars = listings.GroupBy(l => l.MakelaarId)
                                           .Select(m => new MakelaarListingCount
                                           {
                                               MakelaarId = m.Key,
                                               MakelaarNaam = m.First().MakelaarNaam,
                                               ListingCount = m.Count()
                                           })
                                           .OrderByDescending(m => m.ListingCount)
                                           .Take(10);

            return topMakelaars;
        }
    }
}
