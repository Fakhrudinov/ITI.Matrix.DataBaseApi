using DataAbstraction.Interfaces;
using DataAbstraction.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Matrix.DB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplatesPoKomissiiController : ControllerBase
    {
        private ILogger<TemplatesPoKomissiiController> _logger;
        private IDataBaseRepository _repository;

        public TemplatesPoKomissiiController(ILogger<TemplatesPoKomissiiController> logger, IDataBaseRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("GetAll/Restricted/CD/Portfolios/ForCD_Restrict")]
        public async Task<IActionResult> GetAllRestrictedCDPortfolios()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetAll/Restricted/CD/Portfolios/ForCD_Restrict");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            result = await _repository.GetAllRestrictedCDPortfolios();

            if (result.Response.IsSuccess)
            {
                if (result.MatrixClientCodesList.Count == 0)
                {
                    result.Response.Messages.Add("(404) No portfolios found ");
                }
            }

            return Ok(result);
        }

        [HttpGet("GetAll/Allowed/CD/Portfolios/ForCD_portfolio")]
        public async Task<IActionResult> GetAllAllowedCDPortfolios()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetAll/Allowed/CD/Portfolios/ForCD_portfolio");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            result = await _repository.GetAllAllowedCDPortfolios();

            if (result.Response.IsSuccess)
            {
                if (result.MatrixClientCodesList.Count == 0)
                {
                    result.Response.Messages.Add("(404) No portfolios found ");
                }
            }

            return Ok(result);
        }
    }
}
