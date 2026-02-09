using System;
using System.Collections.Generic;

namespace Acceloka.Entities;

public partial class BookedTicket
{
    public int BookedId { get; set; }

    public int TicketCode { get; set; }

    public DateOnly EventDate { get; set; }

    public int Quantity { get; set; }

    public int Price { get; set; }

    public virtual Ticket TicketCodeNavigation { get; set; } = null!;
}
