namespace Acceloka.Models
{
    public class ModelBookedTicketDetailResponse
    {
        public int quantityPerCategory { get; set; }
        public string categoryName { get; set; }

        public List<BookedTicketItemDetailDto> Tickets { get; set; }
    }

    public class BookedTicketItemDetailDto
    {
        public Guid ticketCode { get; set; }
        public string ticketName { get; set; }
        public string eventDate { get; set; }
    }
}
