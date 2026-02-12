using Acceloka.Models;
using FluentValidation;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace Acceloka.Validators
{
    public class BookTicketValidator :AbstractValidator<ModelBookTicketRequest>
    {
        public BookTicketValidator()
        {
            RuleFor( x => x.Tickets)
                .NotNull().WithMessage("Tickets list cannot be null")
                .NotEmpty().WithMessage("Tickets list cannot be empty");

            RuleForEach(x => x.Tickets).ChildRules(items =>
            {
                items.RuleFor(i => i.ticketCode)
                    .NotEmpty().WithMessage("Ticket code cannot be empty");

                items.RuleFor(i => i.quantity)
                    .GreaterThan(0).WithMessage("Quantity must be greater than zero");  
            });
        }
    }
}
