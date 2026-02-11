using Acceloka.Entities;

namespace Acceloka.Models
{
    public class ModelTicketSearchRequest
    {
        public Guid? TicketCode { get; set; }

        public string? TicketName { get; set; }

        public string? CategoryName { get; set; }

        public DateTimeOffset? EventDateStart { get; set; }

        public int? Quota { get; set; }

        public decimal? Price { get; set; }

        public DateTimeOffset? EventDateEnd { get; set; }
    }
}
