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
            var Query = _db.Tickets
                .Where(t => t.Quota > 0)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.CategoryName))
            {
                Query = Query.Where(t => t.CategoryName == request.CategoryName);
            }
            
            if (!string.IsNullOrEmpty(request.TicketCode))
            {
                Query = Query.Where(t => t.TicketCode.ToString().Contains(request.TicketCode));
            }

            if (!string.IsNullOrEmpty(request.TicketName))
            {
                Query = Query.Where(t => t.TicketName.Contains(request.TicketName));
            }

            if (request.Price.HasValue)
            {
                Query = Query.Where(t => t.Price == request.Price.Value);
            }

            if (request.MinEventDate.HasValue)
            {
                Query = Query.Where(t => t.EventDateStart >= request.MinEventDate.Value);
            }

            if (request.MaxEventDate.HasValue)
            {
                Query = Query.Where(t => t.EventDateStart <= request.MaxEventDate.Value);
            }

            // Ganti Urutan Datanya Dari Sini Nanti
            string SortColumn = request.OrderBy.ToLower() ?? "TicketCode";

            // Ini Buat Ascending Atau Descending
            bool IsAscending = (request.OrderState.ToLower() ?? "asc") == "asc";

            Query = SortColumn switch
            {
                "Categoryname" => IsAscending ? Query.OrderBy(t => t.CategoryName) : Query.OrderByDescending(t => t.CategoryName),
                "ticketname" => IsAscending ? Query.OrderBy(t => t.TicketName) : Query.OrderByDescending(t => t.TicketName),
                "price" => IsAscending ? Query.OrderBy(t => t.Price) : Query.OrderByDescending(t => t.Price),
                "eventdate" => IsAscending ? Query.OrderBy(t => t.EventDateStart) : Query.OrderByDescending(t => t.EventDateStart),
                _ => IsAscending ? Query.OrderBy(t => t.TicketCode) : Query.OrderByDescending(t => t.TicketCode),

            };

            var result = await Query
                .Select(t => new
                {
                   TicketCode = t.TicketCode,
                   TicketName = t.TicketName,
                   CategoryName = t.CategoryName,
                   Price = t.Price,
                   EventDate = t.EventDateStart,
                   EventDateEnd = t.EventDateEnd,
                   Quota = t.Quota
                })
                .ToListAsync();

            return result.Cast<object>().ToList();
        }
    }
}
