namespace Acceloka.Models
{
    public class ModelBookTicketResponse
    {
        public decimal PriceSummary { get; set; }
        public List<CategorySummaryDTO> TicketsPerCategory { get; set; }
    }

    public class CategorySummaryDTO
    {
        public string CategoryName { get; set; }
        public decimal SummaryPrice { get; set; }
        public List<BookedTicketDetailDTO> Tickets { get; set; }
    }

    public class BookedTicketDetailDTO
    {
        public Guid TicketCode { get; set; }
        public string TicketName { get; set; }
        public decimal Price { get; set; }
    }
}
