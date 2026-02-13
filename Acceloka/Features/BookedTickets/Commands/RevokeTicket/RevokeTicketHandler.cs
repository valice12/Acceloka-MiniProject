using Acceloka.Entities;
using Acceloka.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Acceloka.Features.BookedTickets.Commands.RevokeTicket
{
    public class RevokeTicketHandler : IRequestHandler<RevokeTicketCommand, ModelRevokeTicketResponse>
    {
        private readonly AccelokaContext _db;

        public RevokeTicketHandler(AccelokaContext db)
        {
            _db = db;
        }

        public async Task<ModelRevokeTicketResponse> Handle(RevokeTicketCommand request, CancellationToken cancellationToken)
        {
            var bookedTicket = await _db.BookedTickets
                .Include(bt => bt.TicketCodeNavigation)
                .FirstOrDefaultAsync(bt => bt.BookedTicketId == request.BookedTicketId && bt.TicketCode == request.TicketCode, cancellationToken);

            if (bookedTicket == null)
                throw new KeyNotFoundException($"Booked Ticket not found.");

            if (request.Quantity > bookedTicket.Quantity)
                throw new ArgumentException("Quantity to revoke exceeds booked quantity");

            bookedTicket.Quantity -= request.Quantity;
            bookedTicket.TicketCodeNavigation.Quota += request.Quantity; // Balikin Quota

            var response = new ModelRevokeTicketResponse
            {
                ticketCode = bookedTicket.TicketCode.ToString(),
                ticketName = bookedTicket.TicketCodeNavigation.TicketName,
                categoryName = bookedTicket.TicketCodeNavigation.CategoryName,
                remainingQuantity = bookedTicket.Quantity
            };

            if (bookedTicket.Quantity == 0)
                _db.BookedTickets.Remove(bookedTicket);
            else
            {
                bookedTicket.UpdatedAt = DateTimeOffset.UtcNow;
                bookedTicket.UpdatedBy = "System";
            }

            await _db.SaveChangesAsync(cancellationToken);
            return response;
        }
    }
}