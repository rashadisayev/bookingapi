using BookingApi.Models;
using BookingApp.Homes.Queries.GetAvailableHomesWithPagination;
using BookingApp.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AvailableHomesController(ISender _sender) : ControllerBase
{

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PaginatedList<HomeAvailabilityDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get(
    [FromQuery] DateOnly startDate,
    [FromQuery] DateOnly endDate,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
    {

        var result = await _sender.Send(
            new GetAvailableHomesWithPaginationQuery(
                startDate,
                endDate,
                pageNumber,
                pageSize
            )
        );

        return Ok(new ApiResponse<PaginatedList<HomeAvailabilityDto>>(true, result, "Success"));
    }
}