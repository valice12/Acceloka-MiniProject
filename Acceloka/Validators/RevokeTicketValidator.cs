using FluentValidation;

namespace Acceloka.Validators
{
    public class RevokeTicketCommand
    {
        public Guid BookedTicketId { get; set; }
        public Guid TicketCode { get; set; }
        public int Quantity { get; set; }
    }

    public class RevokeTicketValidator : AbstractValidator<RevokeTicketCommand>
    {
        public RevokeTicketValidator()
        {
            RuleFor(x => x.BookedTicketId).NotEmpty().WithMessage("BookedTicketId invalid.");
            RuleFor(x => x.TicketCode).NotEmpty().WithMessage("TicketCode invalid.");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity revoke harus lebih besar dari 0.");
        }
    }
}