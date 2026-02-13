using Acceloka.Features.BookedTickets.Commands.BookTicket;
using Acceloka.Features.BookedTickets.Commands.EditBookedTicket;
using Acceloka.Features.BookedTickets.Commands.RevokeTicket;
using Acceloka.Features.BookedTickets.Queries.GetBookedTicketDetail;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Acceloka.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ControllerBookedTicket : ControllerBase
    {
        private readonly IMediator _mediator;

        public ControllerBookedTicket(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/v1/get-booked-ticket/{bookedTicketId}
        [HttpGet("get-booked-ticket/{bookedTicketId}")]
        public async Task<IActionResult> GetBookedTicket([FromRoute] Guid bookedTicketId)
        {
            // Kirim Query ke Handler
            var query = new GetBookedTicketDetailQuery { BookedTicketId = bookedTicketId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // POST: api/v1/book-ticket
        [HttpPost("book-ticket")]
        public async Task<IActionResult> BookTicket([FromBody] BookTicketCommand command)
        {
            // Kirim Command ke Handler
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // PUT: api/v1/edit-booked-ticket/{bookedTicketId}
        [HttpPut("edit-booked-ticket/{bookedTicketId}")]
        public async Task<IActionResult> PutBookedTicket(
            [FromRoute] Guid bookedTicketId,
            [FromBody] EditBookedTicketCommand command)
        {
            // Mapping ID dari URL ke dalam Command
            command.BookedTicketId = bookedTicketId;

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // DELETE: api/v1/revoke-ticket/...
        [HttpDelete("revoke-ticket/{bookedTicketId}/{ticketCode}/{quantity}")]
        public async Task<IActionResult> RevokeTicket(
            [FromRoute] Guid bookedTicketId,
            [FromRoute] Guid ticketCode,
            [FromRoute] int quantity)
        {
            // Mapping Parameter URL ke Command
            var command = new RevokeTicketCommand
            {
                BookedTicketId = bookedTicketId,
                TicketCode = ticketCode,
                Quantity = quantity
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}