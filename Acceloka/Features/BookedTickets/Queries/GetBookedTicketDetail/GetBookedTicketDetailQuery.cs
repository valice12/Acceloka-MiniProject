using MediatR;
using Acceloka.Models;

namespace Acceloka.Features.BookedTickets.Queries.GetBookedTicketDetail
{
    public class GetBookedTicketDetailQuery : IRequest<List<ModelBookedTicketDetailResponse>>
    {
        public Guid BookedTicketId { get; set; }
    }
}