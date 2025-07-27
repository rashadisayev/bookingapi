using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BookingApp.Common.Contracts;
using BookingInfra.Services;
using Microsoft.AspNetCore.Hosting;
using BookingApi;

namespace BookingTests.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Replace the real home store with a test version
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IInMemoryHomeStore));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddSingleton<IInMemoryHomeStore, TestHomeStore>();
        });
    }
}

public class TestHomeStore : IInMemoryHomeStore
{
    private readonly List<BookingDomain.Entities.Home> _homes = new();

    public TestHomeStore()
    {
        // Setup test data with various date ranges
        _homes.Add(new BookingDomain.Entities.Home
        {
            HomeId = "home-1",
            HomeName = "Beach House",
            AvailableSlots = new HashSet<DateOnly>
            {
                new DateOnly(2025, 7, 15),
                new DateOnly(2025, 7, 16),
                new DateOnly(2025, 7, 17),
                new DateOnly(2025, 7, 18),
                new DateOnly(2025, 7, 19),
                new DateOnly(2025, 7, 20),
            }
        });

        _homes.Add(new BookingDomain.Entities.Home
        {
            HomeId = "home-2",
            HomeName = "Mountain Cabin",
            AvailableSlots = new HashSet<DateOnly>
            {
                new DateOnly(2025, 7, 17),
                new DateOnly(2025, 7, 18),
                new DateOnly(2025, 7, 19),
                new DateOnly(2025, 7, 20),
                new DateOnly(2025, 7, 21),
                new DateOnly(2025, 7, 22),
            }
        });

        _homes.Add(new BookingDomain.Entities.Home
        {
            HomeId = "home-3",
            HomeName = "City Apartment",
            AvailableSlots = new HashSet<DateOnly>
            {
                new DateOnly(2025, 7, 15),
                new DateOnly(2025, 7, 16),
                new DateOnly(2025, 7, 17),
                new DateOnly(2025, 7, 18),
            }
        });

        _homes.Add(new BookingDomain.Entities.Home
        {
            HomeId = "home-4",
            HomeName = "Country Villa",
            AvailableSlots = new HashSet<DateOnly>
            {
                new DateOnly(2025, 7, 20),
                new DateOnly(2025, 7, 21),
                new DateOnly(2025, 7, 22),
                new DateOnly(2025, 7, 23),
                new DateOnly(2025, 7, 24),
            }
        });

        _homes.Add(new BookingDomain.Entities.Home
        {
            HomeId = "home-5",
            HomeName = "Lake House",
            AvailableSlots = new HashSet<DateOnly>
            {
                new DateOnly(2025, 7, 15),
                new DateOnly(2025, 7, 16),
                new DateOnly(2025, 7, 17),
                new DateOnly(2025, 7, 18),
                new DateOnly(2025, 7, 19),
                new DateOnly(2025, 7, 20),
                new DateOnly(2025, 7, 21),
                new DateOnly(2025, 7, 22),
            }
        });
    }

    public IEnumerable<BookingDomain.Entities.Home> GetAllHomes() => _homes;
} 