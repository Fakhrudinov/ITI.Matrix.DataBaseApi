using FluentValidation;

namespace DataValidationService.Validators
{
    internal class ClientCodeSpotMatrixMsMoFxRsCdValidator : AbstractValidator<string>
    {
        internal ClientCodeSpotMatrixMsMoFxRsCdValidator()
        {
            RuleFor(x => x)
                .Matches("^(AA|B[PC])[0-9]{4,6}-(MS|MO|FX|RS|CD)-[0-9]{2,3}$")
                    .WithMessage("{PropertyName} '{PropertyValue}' is not in format 'BP12345-XX-01'. Accept only MS|MO|FX|RS|CD portfolio")
                    .WithErrorCode("MS100");
        }
    }
}
