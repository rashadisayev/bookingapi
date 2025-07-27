using BookingApp.Common.Contracts;
using BookingApp.Models;
using MediatR;

namespace BookingApp.Homes.Queries.GetAvailableHomesWithPagination;

public class GetAvailableHomesWithPaginationQueryHandler(IInMemoryHomeStore _homeStore) : IRequestHandler<GetAvailableHomesWithPaginationQuery, PaginatedList<HomeAvailabilityDto>>
{

    public async Task<PaginatedList<HomeAvailabilityDto>> Handle(GetAvailableHomesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var requiredDates = Enumerable
                .Range(0, request.EndDate.DayNumber - request.StartDate.DayNumber + 1)
                .Select(offset => request.StartDate.AddDays(offset))
                .ToHashSet();

        return await Task.Run(() =>
        {
            var filtered = _homeStore.GetAllHomes()
                .Where(home => requiredDates.All(d => home.AvailableSlots.Contains(d)))
                .Select(home => new HomeAvailabilityDto
                {
                    HomeId = home.HomeId,
                    HomeName = home.HomeName,
                    AvailableSlots = requiredDates.ToList()
                })
                .ToList();
            
            var totalCount = filtered.Count;
            var pagedItems = filtered
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();


            return new PaginatedList<HomeAvailabilityDto>(
                pagedItems.Select(h => new HomeAvailabilityDto
                {
                    HomeId = h.HomeId,
                    HomeName = h.HomeName,
                    AvailableSlots = h.AvailableSlots
                }).ToList(),
                totalCount,
                request.PageNumber,
                request.PageSize
            );
        });
    }
}