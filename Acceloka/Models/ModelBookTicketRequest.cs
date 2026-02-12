namespace Acceloka.Models
{
    public class ModelBookTicketRequest
    {
        public List<BookTicketItemDTO> Tickets { get; set; }
    }

    public class BookTicketItemDTO
    {
        public Guid TicketCode { get; set; }
        public int Quantity { get; set; }
    }   
}
