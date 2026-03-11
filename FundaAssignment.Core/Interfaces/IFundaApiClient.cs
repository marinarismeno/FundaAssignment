using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FundaAssignment.Core.Models;

namespace FundaAssignment.Core.Interfaces
{
    public interface IFundaApiClient
    {
        Task<IEnumerable<Listing>> GetAllListingsAsync(string searchQuery, CancellationToken cancellationToken = default);
    }
}
