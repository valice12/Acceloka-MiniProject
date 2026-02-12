using System;
using System.Collections.Generic;

namespace Acceloka.Entities;

public partial class BookedTicket
{
    public int Id { get; set; }

    public Guid BookedTicketId { get; set; }

    public Guid TicketCode { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public DateTimeOffset ScheduledDate { get; set; }

    public DateTimeOffset PurchaseDate { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTimeOffset UpdatedAt { get; set; }

    public string UpdatedBy { get; set; } = null!;

    public virtual Ticket TicketCodeNavigation { get; set; } = null!;
}
