using DataAbstraction.Interfaces;
using DataAbstraction.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Matrix.DB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthStateController : ControllerBase
    {
        private ILogger<HealthStateController> _logger;
        private IDataBaseRepository _repository;

        public HealthStateController(ILogger<HealthStateController> logger, IDataBaseRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("OK")]
        public IActionResult IsOk()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet IsOk Call");

            return Ok("Yes");
        }

        [HttpGet("CheckConnections/MatrixDataBase")]
        public async Task<IActionResult> CheckConnection()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet CheckConnections/MatrixDataBase Call");

            ListStringResponseModel result = await _repository.CheckConnections();

            return Ok(result);
        }
    }
}
