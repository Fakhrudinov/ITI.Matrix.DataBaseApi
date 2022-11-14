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


        [HttpGet("Get/SingleClient/Money/SpotLimits/ByMatrixAccount/{account}")]
        public async Task<IActionResult> GetSingleClientMoneySpotLimitsByMatrixAccount(string account)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet Get/SingleClient/Money/SpotLimits/ByMatrixAccount/{account} Call");

            SingleClientPortfoliosMoneyResponse result = new SingleClientPortfoliosMoneyResponse();

            //проверим корректность входных данных
            result.Response = Validator.ValidateMatrixClientAccount(account);
            if (!result.Response.IsSuccess)
            {
                _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet Get/SingleClient/Money/SpotLimits/ByMatrixAccount/{account} " +
                    $"Error: {result.Response.Messages[0]}");
                return Ok(result);
            }

            result = await _repository.GetSingleClientMoneySpotLimitsByMatrixAccount(account);

            return Ok(result);
        }

        [HttpGet("Get/SingleClient/ActualPositionsLimits/ByMatrixAccount/{account}")]
        public async Task<IActionResult> GetSingleClientActualPositionsLimitsByMatrixAccount(string account)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Get/SingleClient/ActualPositionsLimits/ByMatrixAccount/{account} Call");

            SingleClientPortfoliosPositionResponse result = new SingleClientPortfoliosPositionResponse();

            //проверим корректность входных данных
            result.Response = Validator.ValidateMatrixClientAccount(account);
            if (!result.Response.IsSuccess)
            {
                _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Get/SingleClient/ActualPositionsLimits/ByMatrixAccount/{account}" +
                    $" Error: {result.Response.Messages[0]}");
                return Ok(result);
            }

            result = await _repository.GetSingleClientActualPositionsLimitsByMatrixAccount(account);

            return Ok(result);
        }

        [HttpGet("Get/SingleClient/ClosedPositionsLimits/ByMatrixAccount/{account}/daysShift/{days}")]
        public async Task<IActionResult> GetSingleClientClosedPositionsLimitsByMatrixAccount(string account, int days)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Get/SingleClient/ClosedPositionsLimits/ByMatrixAccount/{account}/daysShift={days} Call");

            SingleClientPortfoliosPositionResponse result = new SingleClientPortfoliosPositionResponse();

            //проверим корректность входных данных
            result.Response = Validator.ValidateMatrixClientAccount(account);
            if (!result.Response.IsSuccess)
            {
                _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Get/SingleClient/ClosedPositionsLimits/ByMatrixAccount/{account}/daysShift={days}" +
                    $" Error: {result.Response.Messages[0]}");
                return Ok(result);
            }

            result = await _repository.GetSingleClientClosedPositionsLimitsByMatrixAccount(account, days);

            return Ok(result);
        }

        [HttpGet("Get/SingleClient/ZeroPositionToTKSLimits/ByMatrixAccount/{account}")]
        public async Task<IActionResult> GetSingleClientZeroPositionToTKSLimitsByMatrixAccount(string account)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Get/SingleClient/ZeroPositionToTKSLimits/ByMatrixAccount/{account} Call");

            SingleClientPortfoliosPositionResponse result = new SingleClientPortfoliosPositionResponse();

            //проверим корректность входных данных
            result.Response = Validator.ValidateMatrixClientAccount(account);
            if (!result.Response.IsSuccess)
            {
                _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Get/SingleClient/ZeroPositionToTKSLimits/ByMatrixAccount/{account}" +
                    $" Error: {result.Response.Messages[0]}");
                return Ok(result);
            }

            result = await _repository.GetSingleClientZeroPositionToTKSLimitsByMatrixAccount(account);

            return Ok(result);
        }

        [HttpGet("Get/SingleClient/DoTrades/ByMatrixAccount/{account}/daysAgoShift/{days}")]
        public async Task<IActionResult> GetSingleClientDoTradesByMatrixAccount(string account, int days)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Get/SingleClient/DoTrades/ByMatrixAccount/{account}/daysAgoShift/{days} Call");

            BoolResponse result = new BoolResponse();

            //проверим корректность входных данных
            result.Response = Validator.ValidateMatrixClientAccount(account);
            if (!result.Response.IsSuccess)
            {
                _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Get/SingleClient/DoTrades/ByMatrixAccount/{account}/daysAgoShift/{days}" +
                    $" Error: {result.Response.Messages[0]}");
                return Ok(result);
            }

            result = await _repository.GetSingleClientDoTradesByMatrixAccount(account, days);

            return Ok(result);
        }

    }
}
