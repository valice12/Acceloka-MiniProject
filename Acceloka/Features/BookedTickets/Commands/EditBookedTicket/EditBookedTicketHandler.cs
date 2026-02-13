using Acceloka.Entities;
using Acceloka.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Acceloka.Features.BookedTickets.Commands.EditBookedTicket
{
    public class EditBookedTicketHandler : IRequestHandler<EditBookedTicketCommand, List<ModelEditBookedTicketResponse>>
    {
        private readonly AccelokaContext _db;

        public EditBookedTicketHandler(AccelokaContext db)
        {
            _db = db;
        }

        public async Task<List<ModelEditBookedTicketResponse>> Handle(EditBookedTicketCommand request, CancellationToken cancellationToken)
        {
            var responseList = new List<ModelEditBookedTicketResponse>();

            var existingBookings = await _db.BookedTickets
                .Include(bt => bt.TicketCodeNavigation)
                .Where(bt => bt.BookedTicketId == request.BookedTicketId)
                .ToListAsync(cancellationToken);

            if (!existingBookings.Any())
                throw new KeyNotFoundException($"Booked Ticket with ID {request.BookedTicketId} not found");

            foreach (var itemRequest in request.Tickets)
            {
                var bookedItem = existingBookings.FirstOrDefault(b => b.TicketCode == itemRequest.ticketCode);

                if (bookedItem == null)
                    throw new KeyNotFoundException($"Ticket {itemRequest.ticketCode} not found in booking.");

                int difference = itemRequest.quantity - bookedItem.Quantity;
                if (difference == 0) continue;

                if (difference > 0 && bookedItem.TicketCodeNavigation.Quota < difference)
                {
                    throw new InvalidOperationException($"Not enough quota for {itemRequest.ticketCode}.");
                }

                bookedItem.TicketCodeNavigation.Quota -= difference;
                bookedItem.Quantity = itemRequest.quantity;
                bookedItem.UpdatedAt = DateTimeOffset.UtcNow;
                bookedItem.UpdatedBy = "System";

                responseList.Add(new ModelEditBookedTicketResponse
                {
                    ticketCode = bookedItem.TicketCode.ToString(),
                    ticketName = bookedItem.TicketCodeNavigation.TicketName,
                    categoryName = bookedItem.TicketCodeNavigation.CategoryName,
                    remainingQuantity = bookedItem.Quantity
                });
            }

            await _db.SaveChangesAsync(cancellationToken);
            return responseList;
        }
    }
}