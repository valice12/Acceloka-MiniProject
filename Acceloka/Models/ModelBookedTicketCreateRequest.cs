using Acceloka.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using System.ComponentModel.DataAnnotations;

namespace Acceloka.Models
{
    public class ModelBookedTicketCreateRequest
    {
        public Guid TicketCode { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public DateTimeOffset ScheduledDate { get; set; } = DateTimeOffset.Now;
    }
}
