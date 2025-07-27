using BookingApp.Models;
using MediatR;

namespace BookingApp.Homes.Queries.GetAvailableHomesWithPagination;

public record GetAvailableHomesWithPaginationQuery(
    DateOnly StartDate,
    DateOnly EndDate,
    int PageNumber = 1,
    int PageSize = 10) : IRequest<PaginatedList<HomeAvailabilityDto>>;