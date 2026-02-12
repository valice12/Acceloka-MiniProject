using Acceloka.Models;
using MediatR;

namespace Acceloka.Features.BookedTickets.Commands.EditBookedTicket
{
    public class EditBookedTicketCommand : IRequest<List<ModelEditBookedTicketResponse>>
    {
        public Guid BookedTicketId { get; set; }
        public List<BookedTicketItemDto> Tickets { get; set; } = new();
    }
}