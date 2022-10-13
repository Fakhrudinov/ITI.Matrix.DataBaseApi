using DataAbstraction.Interfaces;
using DataAbstraction.Responses;
using DataValidationService;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Matrix.DB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientBOServicesController : ControllerBase
    {
        private ILogger<ClientBOServicesController> _logger;
        private IDataBaseRepository _repository;

        public ClientBOServicesController(ILogger<ClientBOServicesController> logger, IDataBaseRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("Get/AllUsersAccounts/With/OptionWorkshop")]
        public async Task<IActionResult> GetAllUsersWithOptionWorkshop()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet Get/AllUsersAccounts/With/OptionWorkshop Call");

            MatrixClientAccountsModelResponse result = new MatrixClientAccountsModelResponse();

            result = await _repository.GetAllUsersWithOptionWorkshop();

            return Ok(result);
        }

        [HttpGet("Get/IsUserHave/OptionWorkshop/{clientCode}")]
        public async Task<IActionResult> GetIsUserHaveOptionWorkshop(string clientCode)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet Get/IsUserHave/OptionWorkshop/{clientCode} Call");

            BoolResponse result = new BoolResponse();

            result.Response = Validator.ValidateClientCode(clientCode);
            if (!result.Response.IsSuccess)
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet Get/IsUserHave/OptionWorkshop/{clientCode} Validation Fail: {result.Response.Messages[0]}");
                return Ok(result);
            }

            result = await _repository.GetIsUserHaveOptionWorkshop(clientCode);

            return Ok(result);
        }
    }
}
