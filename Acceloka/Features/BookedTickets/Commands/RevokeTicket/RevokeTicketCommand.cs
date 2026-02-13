using Acceloka.Models;
using MediatR;

namespace Acceloka.Features.BookedTickets.Commands.RevokeTicket
{
    public class RevokeTicketCommand : IRequest<ModelRevokeTicketResponse>
    {
        public Guid BookedTicketId { get; set; }
        public Guid TicketCode { get; set; }
        public int Quantity { get; set; }
    }
}