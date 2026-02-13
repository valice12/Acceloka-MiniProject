namespace Acceloka.Models
{
    public class ModelEditBookedTicketRequest
    {
        public List<BookedTicketItemDto> Tickets { get; set; }
    }

    public class BookedTicketItemDto
    {
        public Guid ticketCode { get; set; }
        public int quantity { get; set; }
    }
}
