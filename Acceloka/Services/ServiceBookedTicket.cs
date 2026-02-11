using Acceloka.Entities;
using Acceloka.Models;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Acceloka.Services
{
    public class ServiceBookedTicket
    {
        private readonly AccelokaContext _db;
        public ServiceBookedTicket(AccelokaContext db)
        {
            _db = db;
        }

        public async Task<List<BookedTicket>> GetBookedTicketList(ModelBookedTicketSearchRequest request)
        {
            var result = await _db.BookedTickets
                .Include( bt => bt.TicketCodeNavigation)
                .ToListAsync();

            foreach (var item in result)
            {
                if (item.TicketCodeNavigation != null) item.TicketCodeNavigation.BookedTickets = null;
            }
            return result;
        }

        public async Task<List<BookedTicket>> GetBookedTicketListById(Guid BookedTicketId)
        {
            var result = await _db.BookedTickets
                .Include(bt => bt.TicketCodeNavigation)
                .Where(bt => bt.BookedTicketId == BookedTicketId)
                .ToListAsync();

            return result;
        }

        public async Task<string> PostBookedTicket(ModelBookedTicketCreateRequest request)
        {
            var data = new BookedTicket()
            {
                // 1. Data System (Wajib di-generate di sini karena tidak ada di JSON user)
                BookedTicketId = Guid.NewGuid(), // Generate ID baru
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = "System", // Atau ambil dari User context jika ada auth
                UpdatedAt = DateTimeOffset.UtcNow,
                UpdatedBy = "System",
                PurchaseDate = DateTimeOffset.UtcNow,

                // 2. Data Input User (Mapping dari Request)
                TicketCode = request.TicketCode,
                Quantity = request.Quantity,
                Price = request.Price,
                ScheduledDate = request.ScheduledDate
            };

            _db.Add(data);
            await _db.SaveChangesAsync();

            return "Success";
        }

        public async Task<ModelRevokeTicketResponse> RevokeBookedTicket(Guid BookedTicketId, Guid TicketCode, int QuantityToRevoke )
        {
            var BookedTicket = await _db.BookedTickets
                .Include(bt => bt.TicketCodeNavigation)
                .FirstOrDefaultAsync(bt => bt.BookedTicketId == BookedTicketId);

            if (BookedTicket == null)
            {
                throw new KeyNotFoundException($"Booked Ticket {BookedTicket} and {TicketCode} not found");
            }

            if (QuantityToRevoke > BookedTicket.Quantity) throw new ArgumentException("Quantity to revoke exceeds booked quantity");

            if (QuantityToRevoke <= 0) throw new ArgumentException("Quantity to revoke must be greater than zero");

            BookedTicket.Quantity -= QuantityToRevoke;

            var response = new ModelRevokeTicketResponse
            {
                TicketCode = BookedTicket.TicketCode.ToString(),
                TicketName = BookedTicket.TicketCodeNavigation.TicketName,
                CategoryName = BookedTicket.TicketCodeNavigation.CategoryName,
                RemainingQty = BookedTicket.Quantity
            };

            if (BookedTicket.Quantity == 0)
            {
                _db.BookedTickets.Remove(BookedTicket);
            }
            else
            {
                _db.BookedTickets.Update(BookedTicket);
            }

            await _db.SaveChangesAsync();

            return response;
        }

        public async Task<List<ModelEditBookedTicketResponse>> EditBookedTIcketQuantity(Guid BookedTicketId, ModelEditBookedTicketRequest request)
        {
            var ResponseList = new List<ModelEditBookedTicketResponse>();

            var ExistingBookings = await _db.BookedTickets
                .Include(bt => bt.TicketCodeNavigation)
                .Where(bt => bt.BookedTicketId == BookedTicketId)
                .ToListAsync();

            if (!ExistingBookings.Any())
            {
                throw new KeyNotFoundException($"Booked Ticket with ID {BookedTicketId} not found");

            } 

            foreach (var ItemRequest in request.Tickets)
            {
                var BookedItem = ExistingBookings.FirstOrDefault(b => b.TicketCode == ItemRequest.TicketCode);


                if (BookedItem == null) throw new KeyNotFoundException($"Booked Ticket with TicketCode {ItemRequest.TicketCode} not found in the existing bookings");

                if (ItemRequest.Quantity < 0) throw new ArgumentException("New quantity cannot be negative");

                int Difference = ItemRequest.Quantity - BookedItem.Quantity;

                if (Difference > 0)
                {
                    if (BookedItem.TicketCodeNavigation.Quota < Difference)
                    {
                        throw new InvalidOperationException($"Not enough quota for TicketCode {ItemRequest.TicketCode}. Available: {BookedItem.TicketCodeNavigation.Quota}, Requested Increase: {Difference}");
                    }
                }
                BookedItem.Quantity = ItemRequest.Quantity;
                BookedItem.UpdatedAt = DateTimeOffset.UtcNow;
                BookedItem.UpdatedBy = "System";

                ResponseList.Add(new ModelEditBookedTicketResponse
                {
                    TicketCode = BookedItem.TicketCode.ToString(),
                    StringName = BookedItem.TicketCodeNavigation.TicketName,
                    CategoryName = BookedItem.TicketCodeNavigation.CategoryName,
                    RemainingQuantity = BookedItem.Quantity
                });
            }

            await _db.SaveChangesAsync();
            return ResponseList;
        }

    }
}
