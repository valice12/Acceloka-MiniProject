using Acceloka.Entities;
using Acceloka.Features.BookedTickets.Queries.GetAllBookedTickets;
using Acceloka.Features.BookedTickets.Queries.GetBookedTicketList;
using Acceloka.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetAllBookedTicketsHandler : IRequestHandler<GetAllBookedTicketsQuery, ModelBookedTicketListResponse>
{
    private readonly AccelokaContext _db;

    public GetAllBookedTicketsHandler(AccelokaContext db)
    {
        _db = db;
    }

    public async Task<ModelBookedTicketListResponse> Handle(GetAllBookedTicketsQuery request, CancellationToken cancellationToken)
    {
        var allTickets = await _db.BookedTickets
         .Select(bt => new ModelBookedTicketListDTO
         {
             bookedTicketId = bt.BookedTicketId,
             ticketCode = bt.TicketCode,
             quantity = bt.Quantity,
             price = bt.Price,
             scheduledDate = bt.ScheduledDate,
             purchaseDate = bt.PurchaseDate
         })
         .AsNoTracking()
         .ToListAsync(cancellationToken);

        // 2. Lakukan Grouping berdasarkan bookedTicketId
        var groupedOrders = allTickets
            .GroupBy(t => t.bookedTicketId)
            .Select(group => new BookedTicketGroupDto
            {
                bookedTicketId = group.Key,
                Tickets = group.ToList(),
                totalTicketsInOrder = group.Sum(t => t.quantity)
            })
            .ToList();

        // 3. Kembalikan Response Terstruktur
        return new ModelBookedTicketListResponse
        {
            ListBookedTickets = groupedOrders,
            totalOverallTickets = allTickets.Count // Menghitung total baris data
        };
    }
}