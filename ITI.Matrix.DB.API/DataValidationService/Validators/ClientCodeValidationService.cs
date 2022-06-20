using FluentValidation;

namespace DataValidationService.Validators
{
    internal sealed class ClientCodeValidationService : AbstractValidator<string>
    {
        internal ClientCodeValidationService()
        {
            RuleFor(x => x)
                .Matches("^(AA|B[PC])[0-9]{4,6}$")
                    .WithMessage("Client code '{PropertyValue}' is not in format 'BP12345'")
                    .WithErrorCode("CV100");
        }
    }
}
