using FluentValidation;

namespace DataValidationService.Validators
{
    internal class SecurityNameFortsValidator : AbstractValidator<string>
    {
        internal SecurityNameFortsValidator()
        {
            RuleFor(x => x)
                .Length(3)
                    .WithMessage("{PropertyName} '{PropertyValue}' lenght of security is not valid. Must be 3 symbols")
                    .WithErrorCode("SNF100")
                .Matches("^[A-Za-z0-9]{2}[*]$")
                    .WithMessage("{PropertyName} '{PropertyValue}' contain invalid characters or not valid lenght. Only ^[A-Za-z0-9]{2}[*]$")
                    .WithErrorCode("SNF101");

            // ^[A-Z0-9_-]{3,12}$ для фодндовых и валютных
            // ^[A-Za-z0-9]{2}[*]$ для фьючерсов
        }
    }
}
