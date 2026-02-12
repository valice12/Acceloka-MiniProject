namespace Acceloka.Models
{
    public class ModelBookedTicketDetailResponse
    {
        public int QuantityperCategory { get; set; }
        public string CategoryName { get; set; }

        public List<BookedTicketItemDetailDTO> Tickets { get; set; }
    }

    public class BookedTicketItemDetailDTO
    {
        public Guid TicketCode { get; set; }
        public string TicketName { get; set; }
        public string EventDate { get; set; }
    }
}
