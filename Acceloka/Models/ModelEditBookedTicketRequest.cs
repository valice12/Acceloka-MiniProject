namespace Acceloka.Models
{
    public class ModelEditBookedTicketRequest
    {
        public List<BookedTicketItemDTO> Tickets { get; set; }
    }

    public class BookedTicketItemDTO
    {
        public Guid TicketCode { get; set; }
        public int Quantity { get; set; }
    }
}
