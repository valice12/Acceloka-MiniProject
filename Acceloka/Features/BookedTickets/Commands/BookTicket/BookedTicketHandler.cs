using Acceloka.Entities;
using Acceloka.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Acceloka.Features.BookedTickets.Commands.BookTicket
{
    public class BookTicketHandler : IRequestHandler<BookTicketCommand, ModelBookTicketResponse>
    {
        private readonly AccelokaContext _db;

        public BookTicketHandler(AccelokaContext db)
        {
            _db = db;
        }

        public async Task<ModelBookTicketResponse> Handle(BookTicketCommand request, CancellationToken cancellationToken)
        {
            var transactionId = Guid.NewGuid();
            var bookedTicketsToInsert = new List<BookedTicket>();

            var requestTicketCodes = request.Tickets.Select(t => t.ticketCode).ToList();

            var masterTickets = await _db.Tickets
                .Where(t => requestTicketCodes.Contains(t.TicketCode))
                .ToListAsync(cancellationToken);

            foreach (var item in request.Tickets)
            {
                var masterTicket = masterTickets.FirstOrDefault(mt => mt.TicketCode == item.ticketCode);

                if (masterTicket == null)
                    throw new KeyNotFoundException($"Ticket with TicketCode {item.ticketCode} not found");

                if (masterTicket.EventDateStart <= DateTimeOffset.UtcNow)
                    throw new InvalidOperationException($"Cannot book ticket {item.ticketCode}, event has started.");

                if (masterTicket.Quota < item.quantity)
                    throw new InvalidOperationException($"Not enough quota for {item.ticketCode}. Available: {masterTicket.Quota}");

                masterTicket.Quota -= item.quantity;

                bookedTicketsToInsert.Add(new BookedTicket
                {
                    BookedTicketId = transactionId,
                    TicketCode = masterTicket.TicketCode,
                    Quantity = item.quantity,
                    Price = masterTicket.Price,
                    ScheduledDate = masterTicket.EventDateStart,
                    PurchaseDate = DateTimeOffset.UtcNow,
                    TicketCodeNavigation = masterTicket 
                });
            }

            _db.BookedTickets.AddRange(bookedTicketsToInsert);
            await _db.SaveChangesAsync(cancellationToken);

            // Mapping Response
            var categoriesGroup = bookedTicketsToInsert
                .GroupBy(bt => bt.TicketCodeNavigation.CategoryName)
                .Select(g => new CategorySummaryDto
                {
                    categoryName = g.Key,
                    summaryPrice = g.Sum(bt => bt.Price * bt.Quantity),
                    Tickets = g.Select(bt => new BookedTicketDetailDto
                    {
                        ticketCode = bt.TicketCode,
                        ticketName = bt.TicketCodeNavigation.TicketName,
                        price = bt.Price
                    }).ToList()
                }).ToList();

            return new ModelBookTicketResponse
            {
                priceSummary = bookedTicketsToInsert.Sum(bt => bt.Price * bt.Quantity),
                TicketsPerCategory = categoriesGroup
            };
        }
    }
}