namespace Acceloka.Models
{
    public class ModelEditBookedTicketResponse
    {
        public string ticketCode { get; set; }
        public string ticketName { get; set; }
        public string categoryName { get; set; }
        public int remainingQuantity { get; set; }
    }
}
