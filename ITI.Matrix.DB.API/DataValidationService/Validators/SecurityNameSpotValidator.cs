using FluentValidation;

namespace DataValidationService.Validators
{
    internal class SecurityNameSpotValidator : AbstractValidator<string>
    {
        internal SecurityNameSpotValidator()
        {
            RuleFor(x => x)
                .Length(3, 12)
                    .WithMessage("{PropertyName} '{PropertyValue}' lenght of security is not valid. Min=3, max=12")
                    .WithErrorCode("SN100")
                .Matches("^[A-Z0-9_-]{3,12}$")
                    .WithMessage("{PropertyName} '{PropertyValue}' contain invalid characters or not valid lenght. Only ^[A-Z0-9_-]{3,12}$")
                    .WithErrorCode("SN101");

            // ^[A-Z0-9_-]{3,12}$ для фодндовых и валютных
            // ^[A-Za-z0-9]{2}[*]$ для фьючерсов
        }
    }
}
