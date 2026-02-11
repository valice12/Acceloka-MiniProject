using Acceloka.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Acceloka.Models
{
    public class ModelBookedTicketSearchRequest
    {
        public Guid? BookedTicketId { get; set; }

        public Guid? TicketCode { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }

        public string? CreatedBy { get; set; } = null!;

        public DateTimeOffset? UpdatedAt { get; set; }

        public string? UpdatedBy { get; set; } = null!;

        public DateTimeOffset? PurchaseDate { get; set; }

        public DateTimeOffset? ScheduledDate { get; set; } 

    }
}
