using FundaAssignment.Core.Models;
using FundaAssignment.Core.Services;

namespace FundaAssignment.Tests;

public class MakelaarRankingServiceTests
{
    private readonly MakelaarRankingService _sut;

    public MakelaarRankingServiceTests()
    {
        _sut = new MakelaarRankingService();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(20)]
    public void GetTopMakelaars_WithMoreThanTenMakelaars_ReturnsOnlyTopTen(int extraMakelaars)
    {
        // Arrange
        var listings = Enumerable.Range(1, 10 + extraMakelaars)
            .SelectMany(makelaarId => Enumerable.Repeat(
                new Listing { MakelaarId = makelaarId, MakelaarNaam = $"Makelaar {makelaarId}" },
                makelaarId))
            .ToList();

        // Act
        var result = _sut.GetTopMakelaars(listings);

        // Assert
        Assert.Equal(10, result.Count());
    }

    [Fact]
    public void GetTopMakelaars_WithListings_ReturnsResultsOrderedByListingCountDescending()
    {
        // Arrange
        var listings = new List<Listing>
        {
            new() { MakelaarId = 1, MakelaarNaam = "Makelaar A" },
            new() { MakelaarId = 2, MakelaarNaam = "Makelaar B" },
            new() { MakelaarId = 2, MakelaarNaam = "Makelaar B" },
            new() { MakelaarId = 3, MakelaarNaam = "Makelaar C" },
            new() { MakelaarId = 3, MakelaarNaam = "Makelaar C" },
            new() { MakelaarId = 3, MakelaarNaam = "Makelaar C" },
        };

        // Act
        var result = _sut.GetTopMakelaars(listings).ToList();

        // Assert
        Assert.Equal(3, result[0].MakelaarId);
        Assert.Equal(2, result[1].MakelaarId);
        Assert.Equal(1, result[2].MakelaarId);
    }

    [Fact]
    public void GetTopMakelaars_WithEmptyList_ReturnsEmptyResult()
    {
        // Arrange
        var listings = new List<Listing>();

        // Act
        var result = _sut.GetTopMakelaars(listings);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetTopMakelaars_WithListings_CorrectlyCountsListingsPerMakelaar()
    {
        // Arrange
        var listings = new List<Listing>
        {
            new() { MakelaarId = 1, MakelaarNaam = "Makelaar A" },
            new() { MakelaarId = 1, MakelaarNaam = "Makelaar A" },
            new() { MakelaarId = 1, MakelaarNaam = "Makelaar A" },
        };

        // Act
        var result = _sut.GetTopMakelaars(listings).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal(3, result[0].ListingCount);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(9)]
    public void GetTopMakelaars_WithFewerThanTenMakelaars_ReturnsAllMakelaars(int makelaarCount)
    {
        // Arrange
        var listings = Enumerable.Range(1, makelaarCount)
            .Select(id => new Listing { MakelaarId = id, MakelaarNaam = $"Makelaar {id}" })
            .ToList();

        // Act
        var result = _sut.GetTopMakelaars(listings);

        // Assert
        Assert.Equal(makelaarCount, result.Count());
    }
}