using FluentValidation;
using Acceloka.Features.BookedTickets.Queries.GetBookedTicketDetail;

namespace Acceloka.Validators
{
    public class BookedTicketDetailValidator : AbstractValidator<GetBookedTicketDetailQuery>
    {
        public BookedTicketDetailValidator()
        {
            RuleFor(x => x.BookedTicketId)
                .NotEmpty().WithMessage("BookedTicketId cannot be empty.")
                .NotEqual(Guid.Empty).WithMessage("BookedTicketId cannot be an empty GUID.");
        }
    }
}