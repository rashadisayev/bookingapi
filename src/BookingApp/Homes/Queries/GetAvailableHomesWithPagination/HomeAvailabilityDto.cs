namespace BookingApp.Homes.Queries.GetAvailableHomesWithPagination;

public class HomeAvailabilityDto
{
    public string HomeId { get; set; } = default!;
    public string HomeName { get; set; } = default!;
    public List<DateOnly> AvailableSlots { get; set; } = [];
}
