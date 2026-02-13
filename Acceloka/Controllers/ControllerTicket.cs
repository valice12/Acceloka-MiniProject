using Acceloka.Features.Tickets.Queries.GetAvailableTickets;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Acceloka.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ControllerTicket : ControllerBase
    {
        private readonly IMediator _mediator;

        public ControllerTicket(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-available-ticket")]
        public async Task<IActionResult> GetTickets([FromQuery] GetAvailableTicketsQuery query)
        {
            
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}