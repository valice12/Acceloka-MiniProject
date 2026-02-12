namespace Acceloka.Models
{
    public class ModelBookTicketRequest
    {
        public List<BookTicketItemDto> Tickets { get; set; }
    }

    public class BookTicketItemDto
    {
        public Guid ticketCode { get; set; }
        public int quantity { get; set; }
    }   
}
