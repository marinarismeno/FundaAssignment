using System.Text.Json;
using FundaAssignment.Core.Interfaces;
using FundaAssignment.Core.Models;
using Microsoft.Extensions.Logging;

namespace FundaAssignment.ApiClient
{
    public class FundaApiClient(HttpClient httpClient, FundaApiSettings settings, ILogger<FundaApiClient> logger) : IFundaApiClient
    {
        private const int maxNumberOfRetries = 3;
        private const int DelayForRetryInMilliseconds = 30000;
        private const int maxNumberOfRequestsPerMinute = 100;
        private int requestCount = 0;
        private DateTime windowStartTime = DateTime.UtcNow;

        public async Task<IEnumerable<Listing>> GetAllListingsAsync(string searchQuery, CancellationToken cancellationToken = default)
        {
            string uri = settings.BaseUrl + "/" + settings.ApiKey  + searchQuery;

            int totalNumOfPages = -1;
            int currentPage = 1;
            int retryCount = 0;
            List<Listing> fullListOfListings = new();

            do
            {
                string stringResponse = string.Empty; 

                try
                {
                    stringResponse = await httpClient.GetStringAsync(uri + "&page=" + currentPage + "&pagesize=" + settings.PageSize, cancellationToken);
                }
                catch(HttpRequestException)
                {
                    if (retryCount >= maxNumberOfRetries) // too many tries. stop altogether and return
                    {
                        logger.LogError("Failed to fetch page {Page} after {Retries} retries", currentPage, maxNumberOfRetries);
                        break;
                    }

                    await Task.Delay(DelayForRetryInMilliseconds, cancellationToken); // delay a bit before trying again

                    retryCount++;
                    logger.LogWarning("Retrying page {Page}, attempt {Attempt}", currentPage, retryCount);
                    continue; // try again
                }
                catch(TaskCanceledException ex)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    logger.LogError("Failed to fetch page {Page}: {Error}", currentPage, ex.Message);
                    continue; // it was a timeout, retry
                }
                catch (Exception ex)
                {
                    logger.LogError("Unexpected error on page {Page}: {Error}", currentPage, ex.Message);
                    break;
                }

                retryCount = 0;

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

                requestCount++;

                await ThrottleIfNeededAsync();

                currentPage++;
            } while (totalNumOfPages >= currentPage);

            return fullListOfListings;
        }

        private async Task ThrottleIfNeededAsync()
        {
            TimeSpan timePassed = DateTime.UtcNow.Subtract(windowStartTime);

            if (timePassed.Minutes >= 1)
            {
                ResetCounting();
                return;
            }

            if (requestCount >= maxNumberOfRequestsPerMinute)
            {
                TimeSpan timeToAMinute = TimeSpan.FromMinutes(1) - timePassed;

                await Task.Delay((int)timeToAMinute.TotalMilliseconds);

                ResetCounting();
            }
        }

        private void ResetCounting()
        {
            windowStartTime = DateTime.UtcNow;
            requestCount = 0;
        }
    }
}
