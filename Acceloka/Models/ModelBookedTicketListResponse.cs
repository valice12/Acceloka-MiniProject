using Acceloka.Features.BookedTickets.Queries.GetAllBookedTickets;

namespace Acceloka.Models
{
    public class ModelBookedTicketListResponse
    {
        public List<BookedTicketGroupDto> ListBookedTickets { get; set; } = new();
        public int totalOverallTickets { get; set; }
    }

    public class BookedTicketGroupDto
    {
        public Guid bookedTicketId { get; set; }
        public List<ModelBookedTicketListDTO> Tickets { get; set; } = new();
        public int totalTicketsInOrder { get; set; }
    }
}
