using DataAbstraction.Interfaces;
using DataAbstraction.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Matrix.DB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecuritiesController : ControllerBase
    {
        private ILogger<SecuritiesController> _logger;
        private IDataBaseRepository _repository;

        public SecuritiesController(ILogger<SecuritiesController> logger, IDataBaseRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("Get/Securities/SpotBlackList/ForNekval")]
        public async Task<IActionResult> GetSecuritiesSpotBlackListForNekval()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet Get/Securities/SpotBlackList/ForNekval");

            SecurityAndBoardResponse result = new SecurityAndBoardResponse();

            result = await _repository.GetSecuritiesSpotBlackListForNekval();

            if (result.Response.IsSuccess)
            {
                if (result.SecurityAndBoardList.Count == 0)
                {
                    result.Response.Messages.Add("(404) No seс and boards found ");
                }
            }

            return Ok(result);
        }

    }
}
