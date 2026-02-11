namespace Acceloka.Models
{
    public class ModelRevokeTicketResponse
    {
        public string TicketCode { get; set; }
        public string TicketName { get; set; }
        public string CategoryName { get; set; }
        public int RemainingQty { get; set; }
    }
}
