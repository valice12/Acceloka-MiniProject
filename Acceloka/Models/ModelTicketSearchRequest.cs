using Acceloka.Entities;

namespace Acceloka.Models
{
    public class ModelTicketSearchRequest
    {
        // Nggak tau boleh atau nggak, tapi model kita bikin banyakan supaya ga ganggu fungsi yang lain.
        // untuk Search tidak perlu Validation, karena kita mau read dari database

        public string? CategoryName { get; set; }
        public string? TicketCode { get; set; }
        public string? TicketName { get; set; }
        public decimal? Price { get; set; }
        public DateTimeOffset? MinEventDate { get; set; }

        public DateTimeOffset?  MaxEventDate { get; set; }

        public string OrderBy { get; set; } = "TicketCode";
        public string OrderState { get; set; } = "Asc";

    }
}
