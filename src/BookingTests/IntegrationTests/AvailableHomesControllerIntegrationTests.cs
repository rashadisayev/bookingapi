using System.Net;
using System.Text.Json;
using FluentAssertions;
using BookingApi.Models;
using BookingApp.Models;
using BookingApp.Homes.Queries.GetAvailableHomesWithPagination;

namespace BookingTests.IntegrationTests;

public class AvailableHomesControllerIntegrationTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory = factory;
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetAvailableHomes_WithValidDateRange_ReturnsCorrectHomes()
    {
        // Arrange
        var startDate = new DateOnly(2025, 7, 15);
        var endDate = new DateOnly(2025, 7, 17);

        // Act
        var response = await _client.GetAsync($"/api/AvailableHomes?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer.Deserialize<ApiResponse<PaginatedList<HomeAvailabilityDto>>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        apiResponse.Should().NotBeNull();
        apiResponse!.IsSuccess.Should().BeTrue();
        apiResponse.Data.Should().NotBeNull();
        apiResponse.Data!.Items.Should().HaveCount(3); // Beach House, City Apartment, Lake House

        var homeIds = apiResponse.Data.Items.Select(h => h.HomeId).ToHashSet();
        homeIds.Should().Contain("home-1"); 
        homeIds.Should().Contain("home-3"); 
        homeIds.Should().Contain("home-5");
    }

    [Fact]
    public async Task GetAvailableHomes_WithOverlappingDateRange_ReturnsCorrectHomes()
    {
        // Arrange
        var startDate = new DateOnly(2025, 7, 17);
        var endDate = new DateOnly(2025, 7, 18);

        // Act
        var response = await _client.GetAsync($"/api/AvailableHomes?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer.Deserialize<ApiResponse<PaginatedList<HomeAvailabilityDto>>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        apiResponse.Should().NotBeNull();
        apiResponse!.IsSuccess.Should().BeTrue();
        apiResponse.Data.Should().NotBeNull();
        apiResponse.Data!.Items.Should().HaveCount(4); // Beach House, Mountain Cabin, City Apartment, Lake House

        var homeIds = apiResponse.Data.Items.Select(h => h.HomeId).ToHashSet();
        homeIds.Should().Contain("home-1"); 
        homeIds.Should().Contain("home-2"); 
        homeIds.Should().Contain("home-3"); 
        homeIds.Should().Contain("home-5"); 
    }


    [Fact]
    public async Task GetAvailableHomes_WithSingleDay_ReturnsCorrectHomes()
    {
        // Arrange
        var startDate = new DateOnly(2025, 7, 17);
        var endDate = new DateOnly(2025, 7, 17);

        // Act
        var response = await _client.GetAsync($"/api/AvailableHomes?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer.Deserialize<ApiResponse<PaginatedList<HomeAvailabilityDto>>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        apiResponse.Should().NotBeNull();
        apiResponse!.IsSuccess.Should().BeTrue();
        apiResponse.Data.Should().NotBeNull();
        apiResponse.Data!.Items.Should().HaveCount(4); // Beach House, Mountain Cabin, City Apartment, Lake House

        // Verify that each home has exactly one available slot (the requested date)
        foreach (var home in apiResponse.Data.Items)
        {
            home.AvailableSlots.Should().HaveCount(1);
            home.AvailableSlots.Should().Contain(startDate);
        }
    }

    [Fact]
    public async Task GetAvailableHomes_WithNoAvailableHomes_ReturnsEmptyList()
    {
        // Arrange
        var startDate = new DateOnly(2025, 8, 1);
        var endDate = new DateOnly(2025, 8, 5);

        // Act
        var response = await _client.GetAsync($"/api/AvailableHomes?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer.Deserialize<ApiResponse<PaginatedList<HomeAvailabilityDto>>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        apiResponse.Should().NotBeNull();
        apiResponse!.IsSuccess.Should().BeTrue();
        apiResponse.Data.Should().NotBeNull();
        apiResponse.Data!.Items.Should().BeEmpty();
        apiResponse.Data.TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task GetAvailableHomes_WithPagination_ReturnsCorrectPage()
    {
        // Arrange
        var startDate = new DateOnly(2025, 7, 15);
        var endDate = new DateOnly(2025, 7, 20);
        var pageNumber = 1;
        var pageSize = 2;

        // Act
        var response = await _client.GetAsync($"/api/AvailableHomes?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}&pageNumber={pageNumber}&pageSize={pageSize}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer.Deserialize<ApiResponse<PaginatedList<HomeAvailabilityDto>>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        apiResponse.Should().NotBeNull();
        apiResponse!.IsSuccess.Should().BeTrue();
        apiResponse.Data.Should().NotBeNull();
        apiResponse.Data!.Items.Should().HaveCount(2);
        apiResponse.Data.TotalCount.Should().Be(2); // Total available homes for this date range
        apiResponse.Data.PageNumber.Should().Be(pageNumber);
    }


    [Fact]
    public async Task GetAvailableHomes_WithInvalidDateRange_ReturnsBadRequest()
    {
        // Arrange
        var startDate = new DateOnly(2025, 7, 20);
        var endDate = new DateOnly(2025, 7, 15); // End date before start date

        // Act
        var response = await _client.GetAsync($"/api/AvailableHomes?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetAvailableHomes_WithDefaultPagination_ReturnsCorrectResults()
    {
        // Arrange
        var startDate = new DateOnly(2025, 7, 15);
        var endDate = new DateOnly(2025, 7, 17);

        // Act
        var response = await _client.GetAsync($"/api/AvailableHomes?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer.Deserialize<ApiResponse<PaginatedList<HomeAvailabilityDto>>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        apiResponse.Should().NotBeNull();
        apiResponse!.IsSuccess.Should().BeTrue();
        apiResponse.Data.Should().NotBeNull();
        apiResponse.Data!.PageNumber.Should().Be(1); // Default page number
    }
} 