using DataAbstraction.Responses;
using DataValidationService.Validators;
using FluentValidation.Results;

namespace DataValidationService
{
    public static class Validator
    {
        public static ListStringResponseModel ValidateClientCode(string clientCode)
        {
            var responseList = new ListStringResponseModel();

            var validator = new ClientCodeValidationService();
            ValidationResult validationResult = validator.Validate(clientCode);

            if (!validationResult.IsValid)
            {
                responseList = SetResponse(validationResult, responseList);
            }

            return responseList;
        }

        public static ListStringResponseModel ValidateClientPortfolio(string clientCode)
        {
            var responseList = new ListStringResponseModel();

            var validator = new ClientportfolioValidationService();
            ValidationResult validationResult = validator.Validate(clientCode);

            if (!validationResult.IsValid)
            {
                responseList = SetResponse(validationResult, responseList);
            }

            return responseList;
        }


        public static ListStringResponseModel SetResponse(ValidationResult validationResultAsync, ListStringResponseModel response)
        {
            List<string> ValidationMessages = new List<string>();

            response.IsSuccess = false;
            foreach (ValidationFailure failure in validationResultAsync.Errors)
            {
                ValidationMessages.Add(failure.ErrorCode + " " + failure.ErrorMessage);
            }
            response.Messages = ValidationMessages;

            return response;
        }
    }
}