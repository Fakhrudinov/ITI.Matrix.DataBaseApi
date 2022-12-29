using DataAbstraction.Interfaces;
using DataAbstraction.Responses;
using DataValidationService;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Matrix.DB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : ControllerBase
    {
        private ILogger<DiscountsController> _logger;
        private IDataBaseDiscountRepository _repository;

        public DiscountsController(ILogger<DiscountsController> logger, IDataBaseDiscountRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("Get/SingleDiscount/{security}")]
        public async Task<IActionResult> GetSingleDiscount(string security)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet Get/SingleDiscount/{security} Call");

            DiscountSingleResponse result = new DiscountSingleResponse();

            //проверим корректность входных данных
            ListStringResponseModel validateResult = Validator.ValidateSecurity(security);
            if (!validateResult.IsSuccess)
            {
                _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet Get/SingleDiscount/FromGlobal/{security} " +
                    $"validate Error: {validateResult.Messages[0]}");

                result.Discount = null;
                result.IsSuccess = false;
                result.Messages.AddRange(validateResult.Messages);
                return Ok(result);
            }


            result = await _repository.GetSingleDiscount(security);

            return Ok(result);
        }
    }
}
