using FluentValidation;

namespace BookingApp.Homes.Queries.GetAvailableHomesWithPagination;

public class GetAvailableHomesWithPaginationQueryValidator: AbstractValidator<GetAvailableHomesWithPaginationQuery>
{
    public GetAvailableHomesWithPaginationQueryValidator()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.")
            .LessThanOrEqualTo(x => x.EndDate).WithMessage("Start date must be before end date.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date must be after start date.");


        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
    }
}