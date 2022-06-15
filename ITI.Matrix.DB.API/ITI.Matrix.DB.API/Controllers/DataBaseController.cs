using DataAbstraction.Interfaces;
using DataAbstraction.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Matrix.DB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataBaseController : ControllerBase
    {
        private ILogger<DataBaseController> _logger;
        private IDataBaseRepository _repository;


        public DataBaseController(ILogger<DataBaseController> logger, IDataBaseRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("CheckConnections/MatrixDataBase")]
        public async Task<IActionResult> CheckConnection()
        {
            _logger.LogInformation("HttpGet CheckConnections/MatrixDataBase Call");

            ListStringResponseModel result = await _repository.CheckConnections();

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Messages);
            }
        }

        [HttpGet("GetUser/SpotPortfolios/{clientCode}")]
        public async Task<IActionResult> GetUserSpotPortfolios(string clientCode)
        {
            _logger.LogInformation($"HttpGet GetUser/SpotPortfolios {clientCode} Call");

            MatrixClientCodeModelResponse result = await _repository.GetUserSpotPortfolios(clientCode);

            if (result.Response.IsSuccess)
            {
                if (result.MatrixClientCodesList.Count == 0)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            else
            {
                return BadRequest(result.Response.Messages);
            }
        }

        [HttpGet("GetUser/FortsPortfolios/{clientCode}")]
        public async Task<IActionResult> GetUserFortsPortfolios(string clientCode)
        {
            _logger.LogInformation($"HttpGet GetUser/FortsPortfolios {clientCode} Call");

            MatrixToFortsCodesMappingResponse result = await _repository.GetUserFortsPortfolios(clientCode);

            if (result.Response.IsSuccess)
            {
                if (result.MatrixToFortsCodesList.Count == 0)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            else
            {
                return BadRequest(result.Response.Messages);
            }
        }

        [HttpGet("GetUser/FortsPortfolios/NoEDP/{clientCode}")]
        public async Task<IActionResult> GetUserFortsNoEDPPortfolios(string clientCode)
        {
            _logger.LogInformation($"HttpGet GetUser/NoEDP/FortsPortfolios {clientCode} Call");

            MatrixToFortsCodesMappingResponse result = await _repository.GetUserFortsPortfoliosNoEDP(clientCode);

            if (result.Response.IsSuccess)
            {
                if (result.MatrixToFortsCodesList.Count == 0)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            else
            {
                return BadRequest(result.Response.Messages);
            }
        }

        [HttpGet("Get/IsPortfolios/InEDP/{clientRfPortfolio}")]
        public async Task<IActionResult> GetIsPortfolioInEDP(string clientRfPortfolio)
        {
            _logger.LogInformation($"HttpGet Get/IsPortfolios/InEDP/{clientRfPortfolio} Call");

            BoolResponse result = await _repository.GetIsPortfolioInEDP(clientRfPortfolio);

            if (result.IsSuccess)
            {
                if (result.Messages.Count > 0 && result.Messages[0].Equals("(404)"))
                {
                    return NotFound("(404) not found portfolio " + clientRfPortfolio);
                }

                if (!result.IsTrue)
                {
                    result.Messages.Add("False. Portfolio belong to EDP.");
                }
                else
                {
                    result.Messages.Add("Ok. Portfolio non EDP.");
                }
                
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Messages);
            }
        }

        // get personal info
        // check RF - not in MO
    }
}
