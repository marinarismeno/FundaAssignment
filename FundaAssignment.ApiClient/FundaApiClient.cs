using System.Text.Json;
using FundaAssignment.Core.Interfaces;
using FundaAssignment.Core.Models;

namespace FundaAssignment.ApiClient
{
    public class FundaApiClient(HttpClient httpClient, FundaApiSettings settings) : IFundaApiClient
    {
        public async Task<IEnumerable<Listing>> GetAllListingsAsync(string searchQuery, CancellationToken cancellationToken = default)
        {
            string uri = settings.BaseUrl + "/" + settings.ApiKey  + searchQuery;

            int totalNumOfPages = -1;
            int currentPage = 1;
            List<Listing> fullListOfListings = new();

            do
            {
                string stringResponse = await httpClient.GetStringAsync(uri + "&page=" + currentPage + "&pagesize=" + settings.PageSize, cancellationToken);
                FundaApiResponse? fundaApiResponse = JsonSerializer.Deserialize<FundaApiResponse>(stringResponse);
                if(fundaApiResponse == null)
                {
                    break;
                }

                fullListOfListings.AddRange(fundaApiResponse.Objects);

                if (totalNumOfPages == -1)
                {
                    totalNumOfPages = fundaApiResponse.Paging.AantalPaginas;
                }

                currentPage++;
            } while (totalNumOfPages >= currentPage);

            return fullListOfListings;
        }
    }
}
