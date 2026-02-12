using Acceloka.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Acceloka.Features.Tickets.Queries.GetAvailableTickets
{
    public class GetAvailableTicketsHandler : IRequestHandler<GetAvailableTicketsQuery, List<Ticket>>
    {
        private readonly AccelokaContext _db;

        public GetAvailableTicketsHandler(AccelokaContext db)
        {
            _db = db;
        }

        public async Task<List<Ticket>> Handle(GetAvailableTicketsQuery request, CancellationToken cancellationToken)
        {
            var query = _db.Tickets.AsQueryable();

            if (!string.IsNullOrEmpty(request.ticketName))
                query = query.Where(t => t.TicketName.Contains(request.ticketName));

            if (!string.IsNullOrEmpty(request.categoryName))
                query = query.Where(t => t.CategoryName.Contains(request.categoryName));

            if (request.priceMin.HasValue)
                query = query.Where(t => t.Price >= request.priceMin.Value);

            if (request.priceMax.HasValue)
                query = query.Where(t => t.Price <= request.priceMax.Value);

            if (request.eventDateMin.HasValue)
                query = query.Where(t => t.EventDateStart >= request.eventDateMin.Value);

            if (request.eventDateMax.HasValue)
                query = query.Where(t => t.EventDateStart <= request.eventDateMax.Value);

            // 2. 
            if (string.IsNullOrEmpty(request.orderBy))
            {
                query = query.OrderBy(t => t.TicketName);
            }
            else
            {
                bool isAsc = string.IsNullOrEmpty(request.orderState) || request.orderState.ToLower() == "asc";

                query = request.orderBy.ToLower() switch
                {
                    "price" => isAsc ? query.OrderBy(t => t.Price) : query.OrderByDescending(t => t.Price),
                    "eventdate" => isAsc ? query.OrderBy(t => t.EventDateStart) : query.OrderByDescending(t => t.EventDateStart),
                    "categoryname" => isAsc ? query.OrderBy(t => t.CategoryName) : query.OrderByDescending(t => t.CategoryName),
                    _ => isAsc ? query.OrderBy(t => t.TicketName) : query.OrderByDescending(t => t.TicketName),
                };
            }

            // 3. Execution (Async Await)
            return await query.AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}