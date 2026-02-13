using Acceloka.Models;
using FluentValidation;

namespace Acceloka.Validators
{
    public class EditBookedTicketValidator : AbstractValidator<ModelEditBookedTicketRequest>
    {
        public EditBookedTicketValidator()
        {
            RuleFor(x => x.Tickets)
                .NotNull().WithMessage("Tickets list cannot be null")
                .NotEmpty().WithMessage("Tickets list cannot be empty");

            RuleForEach(x => x.Tickets).ChildRules(items =>
            {
                items.RuleFor(i => i.ticketCode)
                    .NotEmpty().WithMessage("Ticket code cannot be empty");

                items.RuleFor(i => i.quantity)
                    .GreaterThan(1).WithMessage("Quantity must be greater than zero");
            });
        }
    }
}
