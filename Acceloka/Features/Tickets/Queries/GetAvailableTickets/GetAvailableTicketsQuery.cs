using Acceloka.Entities;
using MediatR;

namespace Acceloka.Features.Tickets.Queries.GetAvailableTickets
{
    public class GetAvailableTicketsQuery : IRequest<List<Ticket>>
    {
        public string? ticketName { get; set; }
        public string? categoryName { get; set; }
        public decimal? priceMin { get; set; }
        public decimal? priceMax { get; set; }
        public DateTimeOffset? eventDateMin { get; set; }
        public DateTimeOffset? eventDateMax { get; set; }
        public string? orderBy { get; set; }
        public string? orderState { get; set; }
    }
}