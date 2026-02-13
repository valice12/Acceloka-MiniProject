using Acceloka.Models;
using MediatR;

namespace Acceloka.Features.BookedTickets.Commands.BookTicket
{
    public class BookTicketCommand : IRequest<ModelBookTicketResponse>
    {
        public List<BookTicketItemDto> Tickets { get; set; } = new();
    }
}