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

        public static ListStringResponseModel ValidateClientPortfoliosList(IEnumerable<string> portfolios)
        {
            ListStringResponseModel responseList = new ListStringResponseModel();

            foreach (string str in portfolios)
            {
                if (str.StartsWith("B") || str.StartsWith("A"))
                {
                    ClientCodeSpotMatrixMsMoFxRsCdValidator validator = new ClientCodeSpotMatrixMsMoFxRsCdValidator();
                    ValidationResult validationResult = validator.Validate(str);
                    if (!validationResult.IsValid)
                    {
                        responseList = SetResponseFromValidationResult.SetResponse(validationResult, responseList);
                    }
                }
                else
                {
                    responseList.IsSuccess = false;
                    responseList.Messages.Add($"VC100 '{str}' is not in expected formats 'BP12345-XX-01' or 'C0xxxxx'");
                }
            }

            return responseList;
        }

        public static ListStringResponseModel ValidateMatrixClientAccount(string account)
        {
            var responseList = new ListStringResponseModel();

            var validator = new ClientCodeValidationService();
            ValidationResult validationResult = validator.Validate(account);

            if (!validationResult.IsValid)
            {
                responseList = SetResponse(validationResult, responseList);
            }

            return responseList;
        }
    }
}