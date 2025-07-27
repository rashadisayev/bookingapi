namespace BookingDomain.Entities;

public class Home
{
    public string HomeId { get; set; } = default!;
    public string HomeName { get; set; } = default!;
    public HashSet<DateOnly> AvailableSlots { get; set; } = [];
}
