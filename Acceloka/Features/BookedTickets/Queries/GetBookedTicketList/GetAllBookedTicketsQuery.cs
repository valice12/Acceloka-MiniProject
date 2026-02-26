using Acceloka.Features.BookedTickets.Queries.GetAllBookedTickets;
using Acceloka.Models;
using MediatR;


namespace Acceloka.Features.BookedTickets.Queries.GetBookedTicketList
{
    public class GetAllBookedTicketsQuery : IRequest<ModelBookedTicketListResponse>

    {
    }
}
