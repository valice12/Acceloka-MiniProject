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

        public async Task<List<Ticket>> GetAvailableTicketList(ModelTicketSearchRequest request)
        {
            var tickets = await _db.Tickets
                .Where(t => t.Quota > 0)
                .ToListAsync();

            return tickets;
        }
    }
}
