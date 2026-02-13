using FluentValidation;
using Acceloka.Features.Tickets.Queries.GetAvailableTickets; // Update namespace

namespace Acceloka.Validators
{
    // Ubah T menjadi GetAvailableTicketsQuery
    public class TicketSearchValidator : AbstractValidator<GetAvailableTicketsQuery>
    {
        public TicketSearchValidator()
        {
            // Logic validasi tetap sama seperti sebelumnya
            RuleFor(x => x)
                .Must(x => !x.priceMin.HasValue || !x.priceMax.HasValue || x.priceMin <= x.priceMax)
                .WithMessage("PriceMin tidak boleh lebih besar dari PriceMax.");

            RuleFor(x => x)
                .Must(x => !x.eventDateMin.HasValue || !x.eventDateMax.HasValue || x.eventDateMin <= x.eventDateMax)
                .WithMessage("EventDateMin tidak boleh lebih besar dari EventDateMax.");

            RuleFor(x => x.orderBy)
                .Must(o => string.IsNullOrEmpty(o) ||
                           new[] { "ticketname", "categoryname", "price", "eventdate" }
                           .Contains(o.ToLower()))
                .WithMessage("OrderBy tidak valid.");

            RuleFor(x => x.orderState)
                .Must(o => string.IsNullOrEmpty(o) ||
                           new[] { "asc", "desc" }
                           .Contains(o.ToLower()))
                .WithMessage("OrderState harus 'asc' atau 'desc'.");
        }
    }
}