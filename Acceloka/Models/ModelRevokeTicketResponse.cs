namespace Acceloka.Models
{
    public class ModelRevokeTicketResponse
    {
        public string ticketCode { get; set; }
        public string ticketName { get; set; }
        public string categoryName { get; set; }
        public int remainingQuantity { get; set; }
    }
}
