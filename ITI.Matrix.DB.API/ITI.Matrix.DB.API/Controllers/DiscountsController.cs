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
            ListStringResponseModel validateResult = Validator.ValidateSecuritySpot(security);
            if (!validateResult.IsSuccess)
            {
                _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet Get/SingleDiscount/{security} " +
                    $"validate Error: {validateResult.Messages[0]}");

                result.Discount = null;
                result.IsSuccess = false;
                result.Messages.AddRange(validateResult.Messages);
                return Ok(result);
            }


            result = await _repository.GetSingleDiscount(security);

            return Ok(result);
        }

        [HttpGet("Get/SingleDiscountForts/{security}")]
        public async Task<IActionResult> GetSingleDiscountForts(string security)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet Get/SingleDiscountForts/{security} Call");

            DiscountSingleResponse result = new DiscountSingleResponse();

            //проверим корректность входных данных
            ListStringResponseModel validateResult = Validator.ValidateSecurityForts(security);
            if (!validateResult.IsSuccess)
            {
                _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet Get/SingleDiscountForts/{security} " +
                    $"validate Error: {validateResult.Messages[0]}");

                result.Discount = null;
                result.IsSuccess = false;
                result.Messages.AddRange(validateResult.Messages);
                return Ok(result);
            }


            result = await _repository.GetSingleDiscountForts(security);

            return Ok(result);
        }

        [HttpGet("Get/DiscountsList/EQ")]
        public async Task<IActionResult> GetDiscountsListEQ()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet Get/DiscountsList/EQ Call");

            DiscountsListResponse result = await _repository.GetDiscountsListEQ();

            return Ok(result);
        }

        [HttpGet("Get/DiscountsList/Cets")]
        public async Task<IActionResult> GetDiscountsListCets()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet Get/DiscountsList/Cets Call");

            DiscountsListResponse result = await _repository.GetDiscountsListCets();

            return Ok(result);
        }

        [HttpGet("Get/DiscountsList/Forts")]
        public async Task<IActionResult> GetDiscountsListForts()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet Get/DiscountsList/Forts Call");

            DiscountsListResponse result = await _repository.GetDiscountsListForts();

            return Ok(result);
        }
    }
}
