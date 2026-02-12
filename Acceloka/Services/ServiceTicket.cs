using Acceloka.Entities;
using Acceloka.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Versioning;

namespace Acceloka.Services
{
    public class ServiceTicket
    {
        private readonly AccelokaContext _db;

        public ServiceTicket(AccelokaContext db)
        {
            _db = db;
        }

        public async Task<List<object>> GetAvailableTicketList(ModelTicketSearchRequest request)
        {
            var query = _db.Tickets
                .Where(t => t.Quota > 0)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.categoryName))
            {
                query = query.Where(t => t.CategoryName == request.categoryName);
            }
            
            if (!string.IsNullOrEmpty(request.ticketCode))
            {
                query = query.Where(t => t.TicketCode.ToString().Contains(request.ticketCode));
            }

            if (!string.IsNullOrEmpty(request.ticketName))
            {
                query = query.Where(t => t.TicketName.Contains(request.ticketName));
            }

            if (request.priceMin.HasValue)
            {
                query = query.Where(t => t.Price == request.priceMin.Value);
            }

            if (request.priceMax.HasValue)
            {
                query = query.Where(t => t.Price == request.priceMax.Value);
            }

            if (request.minEventDate.HasValue)
            {
                query = query.Where(t => t.EventDateStart >= request.minEventDate.Value);
            }

            if (request.maxEventDate.HasValue)
            {
                query = query.Where(t => t.EventDateStart <= request.maxEventDate.Value);
            }

            string SortColumn = request.orderBy.ToLower() ?? "TicketCode";

            bool IsAscending = (request.orderState.ToLower() ?? "asc") == "asc";

            query = SortColumn switch
            {
                "Categoryname" => IsAscending ? query.OrderBy(t => t.CategoryName) : query.OrderByDescending(t => t.CategoryName),
                "ticketname" => IsAscending ? query.OrderBy(t => t.TicketName) : query.OrderByDescending(t => t.TicketName),
                "price" => IsAscending ? query.OrderBy(t => t.Price) : query.OrderByDescending(t => t.Price),
                "eventdate" => IsAscending ? query.OrderBy(t => t.EventDateStart) : query.OrderByDescending(t => t.EventDateStart),
                _ => IsAscending ? query.OrderBy(t => t.TicketCode) : query.OrderByDescending(t => t.TicketCode),

            };

            var result = await query
                .Select(t => new
                {
                   ticketCode = t.TicketCode,
                   ticketName = t.TicketName,
                   categoryName = t.CategoryName,
                   price = t.Price,
                   eventDate = t.EventDateStart,
                   eventDateEnd = t.EventDateEnd,
                   quota = t.Quota
                })
                .ToListAsync();

            return result.Cast<object>().ToList();
        }
    }
}
