using System;
using System.Collections.Generic;

namespace Acceloka.Entities;

public partial class Ticket
{
    public Guid TicketCode { get; set; }

    public string TicketName { get; set; } = null!;

    public string CategoryName { get; set; } = null!;

    public int Quota { get; set; }

    public decimal Price { get; set; }

    public DateTimeOffset EventDateStart { get; set; }

    public DateTimeOffset EventDateEnd { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTimeOffset UpdatedAt { get; set; }

    public string UpdatedBy { get; set; } = null!;

    public virtual ICollection<BookedTicket> BookedTickets { get; set; } = new List<BookedTicket>();
}
