using System;
using System.Collections.Generic;

namespace Acceloka.Entities;

public partial class Ticket
{
    public int TicketCode { get; set; }

    public string TicketName { get; set; } = null!;

    public DateOnly? EventDateStart { get; set; }

    public int? Quantity { get; set; }

    public string CategoryName { get; set; } = null!;

    public int Price { get; set; }

    public virtual ICollection<BookedTicket> BookedTickets { get; set; } = new List<BookedTicket>();
}
