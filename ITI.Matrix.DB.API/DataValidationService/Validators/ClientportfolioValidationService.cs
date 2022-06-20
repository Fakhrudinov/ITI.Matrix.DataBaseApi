using FluentValidation;

namespace DataValidationService.Validators
{
    internal sealed class ClientportfolioValidationService : AbstractValidator<string>
    {
        internal ClientportfolioValidationService()
        {
            RuleFor(x => x)
                .Matches("^(AA|B[PC])[0-9]{4,6}-(MS|MO|FX|RS|CD|RF|SF)-[0-9]{2}$")
                    .WithMessage("{PropertyName} '{PropertyValue}' is not in format 'BP12345-XX-01'. Accept only MS|MO|FX|RS|CD|RF|SF portfolio")
                    .WithErrorCode("CV101");
        }
    }
}
