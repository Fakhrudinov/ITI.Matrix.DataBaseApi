using DataAbstraction.Interfaces;
using DataAbstraction.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Matrix.DB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplatesPoPlechuController : ControllerBase
    {
        private ILogger<TemplatesPoPlechuController> _logger;
        private IDataBaseRepository _repository;

        public TemplatesPoPlechuController(ILogger<TemplatesPoPlechuController> logger, IDataBaseRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("GetAll/CD/Portfolios")]
        public async Task<IActionResult> GetAllCDPortfolios()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetAll/CD/Portfolios");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            result = await _repository.GetAllCDPortfolios();

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
