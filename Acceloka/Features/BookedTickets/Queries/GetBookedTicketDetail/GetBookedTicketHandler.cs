using Acceloka.Entities;
using Acceloka.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Acceloka.Features.BookedTickets.Queries.GetBookedTicketDetail
{
    public class GetBookedTicketHandler : IRequestHandler<GetBookedTicketDetailQuery, List<ModelBookedTicketDetailResponse>>
    {
        private readonly AccelokaContext _db;

        public GetBookedTicketHandler(AccelokaContext db)
        {
            _db = db;
        }

        public async Task<List<ModelBookedTicketDetailResponse>> Handle(GetBookedTicketDetailQuery request, CancellationToken cancellationToken)
        {
            var result = await _db.BookedTickets
                .Include(bt => bt.TicketCodeNavigation)
                .Where(bt => bt.BookedTicketId == request.BookedTicketId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            if (!result.Any())
            {
                throw new KeyNotFoundException($"Booked Ticket with ID {request.BookedTicketId} not found");
            }

            return result
                .GroupBy(bt => bt.TicketCodeNavigation.CategoryName)
                .Select(group => new ModelBookedTicketDetailResponse
                {
                    categoryName = group.Key,
                    quantityPerCategory = group.Sum(bt => bt.Quantity),
                    Tickets = group.Select(bt => new BookedTicketItemDetailDto
                    {
                        ticketCode = bt.TicketCode,
                        ticketName = bt.TicketCodeNavigation.TicketName,
                        eventDate = bt.TicketCodeNavigation.EventDateStart.ToString("dd-MM-yyyy HH:mm")
                    }).ToList()
                }).ToList();
        }
    }
}