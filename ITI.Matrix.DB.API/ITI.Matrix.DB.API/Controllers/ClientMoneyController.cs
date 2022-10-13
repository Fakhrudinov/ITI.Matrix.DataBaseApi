using DataAbstraction.Interfaces;
using DataAbstraction.Responses;
using Microsoft.AspNetCore.Mvc;
using DataValidationService;

namespace ITI.Matrix.DB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientMoneyController : ControllerBase
    {
        private ILogger<ClientMoneyController> _logger;
        private IDataBaseRepository _repository;

        public ClientMoneyController(ILogger<ClientMoneyController> logger, IDataBaseRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("GetClients/WhoTrade/SpotPortfoliosAndMoney/{daysShift}")]
        public async Task<IActionResult> GetClientsSpotPortfoliosWhoTradesYesterday(int daysShift)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetClients/WhoTrade/SpotPortfoliosAndMoney/{daysShift} Call");

            ClientAndMoneyResponse result = new ClientAndMoneyResponse();

            //Validate daysShift
            if (daysShift < 0 || daysShift > 7)
            {
                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error: daysShift must be between 0 and 7");

                return Ok(result);
            }

            result = await _repository.GetClientsSpotPortfoliosWhoTradesYesterday(daysShift);

            return Ok(result);
        }

        [HttpGet("GetClients/Positions/ByMatrixPortfolioList")]
        public async Task<IActionResult> GetClientsPositionsByMatrixPortfolioList([FromQuery] IEnumerable<string> portfolios)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetClients/Positions/ByMatrixPortfolioList Call");

            ClientDepoPositionsResponse result = new ClientDepoPositionsResponse();

            //проверим корректность входных данных
            if (portfolios.Count() == 0)
            {
                result.Response.IsSuccess = false;
                result.Response.Messages.Add("portfolios list must contain at least 1 portfolios");
                return Ok(result);
            }
            result.Response = Validator.ValidateClientPortfoliosList(portfolios);
            if (!result.Response.IsSuccess)
            {
                _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetAllClientsFromTemplate/PoKomissii Error: {result.Response.Messages[0]}");
                return Ok(result);
            }

            result = await _repository.GetClientsPositionsByMatrixPortfolioList(portfolios);

            return Ok(result);
        }
    }
}
