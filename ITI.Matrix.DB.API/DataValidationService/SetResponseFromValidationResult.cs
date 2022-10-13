using DataAbstraction.Responses;
using FluentValidation.Results;

namespace DataValidationService
{
    public static class SetResponseFromValidationResult
    {
        public static ListStringResponseModel SetResponse(ValidationResult validationResultAsync, ListStringResponseModel response)
        {
            List<string> ValidationMessages = new List<string>();

            response.IsSuccess = false;
            foreach (ValidationFailure failure in validationResultAsync.Errors)
            {
                ValidationMessages.Add(failure.ErrorCode + " " + failure.ErrorMessage);
            }
            response.Messages.AddRange(ValidationMessages);// = ValidationMessages;

            return response;
        }

        public static string GetErrorsCodeFromValidationResult(ValidationResult validationResult)
        {
            string result = "";

            foreach (ValidationFailure failure in validationResult.Errors)
            {
                result = result + failure.ErrorCode + " ";
            }

            return result;
        }
    }
}
