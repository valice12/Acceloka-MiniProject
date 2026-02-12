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

        public async Task<List<ModelBookedTicketDetailResponse>> GetBookedTicketList(Guid BookedTicketId)
        {
            var result = await _db.BookedTickets
                .Include( bt => bt.TicketCodeNavigation)
                .Where(bt => bt.BookedTicketId == BookedTicketId)
                .AsNoTracking()
                .ToListAsync();

            if (!result.Any())
            {
                throw new KeyNotFoundException($"Booked Ticket with ID {BookedTicketId} not found");
            }

            var NewResult = result
                .GroupBy(bt => bt.TicketCodeNavigation.CategoryName)
                .Select(group => new ModelBookedTicketDetailResponse
                {
                    CategoryName = group.Key,
                    QuantityperCategory = group.Sum(bt => bt.Quantity),
                    Tickets = group.Select(bt => new BookedTicketItemDetailDTO
                    {
                        TicketCode = bt.TicketCode,
                        TicketName = bt.TicketCodeNavigation.TicketName,
                        EventDate = bt.TicketCodeNavigation.EventDateStart.ToString()
                    }).ToList()
                }).ToList();

            return NewResult;
        }

        public async Task<ModelBookTicketResponse> PostBookedTicket(ModelBookTicketRequest request)
        {
            var TransactionId = Guid.NewGuid();
            var BookedTicketsToInsert = new List<BookedTicket>();

            var RequestTicketCodes = request.Tickets
                .Select(t => t.TicketCode).ToList();

            var MasterTickets = await _db.Tickets
                .Where(t => RequestTicketCodes.Contains(t.TicketCode))
                .ToListAsync();

            foreach (var Item in request.Tickets)
            {
                var MasterTicket = MasterTickets
                    .FirstOrDefault(mt => mt.TicketCode == Item.TicketCode);
                if (MasterTicket == null)
                {
                    throw new KeyNotFoundException($"Ticket with TicketCode {Item.TicketCode} not found");
                }
                if (MasterTicket.EventDateStart <= DateTimeOffset.UtcNow)
                {
                    throw new InvalidOperationException($"Cannot book ticket for TicketCode {Item.TicketCode} as the event has already started.");
                }
                if (MasterTicket.Quota < Item.Quantity)
                {
                    throw new InvalidOperationException($"Not enough quota for TicketCode {Item.TicketCode}. Available: {MasterTicket.Quota}, Requested: {Item.Quantity}");
                }
                MasterTicket.Quota -= Item.Quantity;

                var NewBooking = new BookedTicket
                {
                    BookedTicketId = TransactionId,
                    TicketCode = MasterTicket.TicketCode,
                    Quantity = Item.Quantity,
                    Price = MasterTicket.Price,
                    ScheduledDate = MasterTicket.EventDateStart,
                    PurchaseDate = DateTimeOffset.UtcNow,
                    CreatedAt = DateTimeOffset.UtcNow,
                    CreatedBy = "System",
                    UpdatedAt = DateTimeOffset.UtcNow,
                    UpdatedBy = "System",

                    TicketCodeNavigation = MasterTicket
                };
                BookedTicketsToInsert.Add(NewBooking);
            }

            _db.BookedTickets.AddRange(BookedTicketsToInsert);
            await _db.SaveChangesAsync();

            var CategoriesGroup = BookedTicketsToInsert
                .GroupBy(bt => bt.TicketCodeNavigation.CategoryName)
                .Select(g => new CategorySummaryDTO
                {
                    CategoryName = g.Key,
                    SummaryPrice = g.Sum(bt => bt.Price * bt.Quantity),
                    Tickets = g.Select(bt => new BookedTicketDetailDTO
                    {
                        TicketCode = bt.TicketCode,
                        TicketName = bt.TicketCodeNavigation.TicketName,
                        Price = bt.Price,
                    }).ToList()
                }).ToList();

            return new ModelBookTicketResponse
            {
                PriceSummary = BookedTicketsToInsert.Sum(bt => bt.Price * bt.Quantity),
                TicketsPerCategory = CategoriesGroup
            };
        }

        public async Task<ModelRevokeTicketResponse> RevokeBookedTicket(Guid BookedTicketId, Guid TicketCode, int QuantityToRevoke )
        {
            var BookedTicket = await _db.BookedTickets
                .Include(bt => bt.TicketCodeNavigation)
                .FirstOrDefaultAsync(bt => bt.BookedTicketId == BookedTicketId && bt.TicketCode == TicketCode);

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

        public async Task<List<ModelEditBookedTicketResponse>> EditBookedTicketQuantity(Guid BookedTicketId, ModelEditBookedTicketRequest request)
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
