namespace Acceloka.Features.BookedTickets.Queries.GetAllBookedTickets
{
    public class ModelBookedTicketListDTO
    {
        public Guid bookedTicketId { get; set; }
        public Guid ticketCode { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
        public DateTimeOffset scheduledDate { get; set; }
        public DateTimeOffset purchaseDate { get; set; }
    }
}