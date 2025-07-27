using BookingApp.Common.Contracts;
using BookingDomain.Entities;

namespace BookingInfra.Services;

public class InMemoryHomeStore : IInMemoryHomeStore
{
    private readonly List<Home> _homes = new();

    public InMemoryHomeStore()
    {
        _homes.Add(new Home
        {
            HomeId = "123",
            HomeName = "Home 1",
            AvailableSlots = new HashSet<DateOnly>
            {
                new DateOnly(2025, 7, 15),
                new DateOnly(2025, 7, 16),
                new DateOnly(2025, 7, 17),
            }
        });

        _homes.Add(new Home
        {
            HomeId = "456",
            HomeName = "Home 2",
            AvailableSlots = new HashSet<DateOnly>
            {
                new DateOnly(2025, 7, 17),
                new DateOnly(2025, 7, 18),
            }
        });
    }

    public IEnumerable<Home> GetAllHomes() => _homes;
}
