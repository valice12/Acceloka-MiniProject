namespace Acceloka.Models
{
    public class ModelBookTicketResponse
    {
        public decimal priceSummary { get; set; }
        public List<CategorySummaryDto> TicketsPerCategory { get; set; }
    }

    public class CategorySummaryDto
    {
        public string categoryName { get; set; }
        public decimal summaryPrice { get; set; }
        public List<BookedTicketDetailDto> Tickets { get; set; }
    }

    public class BookedTicketDetailDto
    {
        public Guid ticketCode { get; set; }
        public string ticketName { get; set; }
        public decimal price { get; set; }
    }
}
