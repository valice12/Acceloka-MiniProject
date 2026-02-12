using Acceloka.Entities;

namespace Acceloka.Models
{
    public class ModelTicketSearchRequest
    {
        // Nggak tau boleh atau nggak, tapi model kita bikin banyakan supaya ga ganggu fungsi yang lain.
        // untuk Search tidak perlu Validation, karena kita mau read dari database

        public string? categoryName { get; set; }
        public string? ticketCode { get; set; }
        public string? ticketName { get; set; }
        public decimal? priceMin { get; set; }
        public decimal? priceMax { get; set; }
        public DateTimeOffset? minEventDate { get; set; }

        public DateTimeOffset?  maxEventDate { get; set; }

        public string orderBy { get; set; } = "TicketCode";
        public string orderState { get; set; } = "Asc";

    }
}
