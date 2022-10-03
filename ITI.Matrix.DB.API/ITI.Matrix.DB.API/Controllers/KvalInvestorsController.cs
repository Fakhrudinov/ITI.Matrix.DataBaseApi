using Microsoft.AspNetCore.Mvc;
using DataAbstraction.Interfaces;
using DataAbstraction.Responses;

namespace ITI.Matrix.DB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KvalInvestorsController : ControllerBase
    {
        private ILogger<KvalInvestorsController> _logger;
        private IDataBaseRepository _repository;

        public KvalInvestorsController(ILogger<KvalInvestorsController> logger, IDataBaseRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("GetAll/NonKvalUsers/KSUR/Spot/Portfolios")]
        public async Task<IActionResult> GetAllNonKvalUsersSpotPortfolios()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetAll/NonKvalUsers/KSUR/Spot/Portfolios");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            result = await _repository.GetAllNonKvalUsersKsurSpotPortfolios();

            if (result.Response.IsSuccess)
            {
                if (result.MatrixClientCodesList.Count == 0)
                {
                    result.Response.Messages.Add("(404) No portfolios found ");
                }
            }

            return Ok(result);
        }

        [HttpGet("GetAll/NonKvalUsers/KSUR/Cd/Portfolios")]
        public async Task<IActionResult> GetAllNonKvalUsersCdPortfolios()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetAll/NonKvalUsers/KSUR/Cd/Portfolios");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            result = await _repository.GetAllNonKvalUsersKsurCdPortfolios();

            if (result.Response.IsSuccess)
            {
                if (result.MatrixClientCodesList.Count == 0)
                {
                    result.Response.Messages.Add("(404) No portfolios found ");
                }
            }

            return Ok(result);
        }

        [HttpGet("GetAll/NonKvalUsers/KPUR/Spot/Portfolios")]
        public async Task<IActionResult> GetAllNonKvalKpurUsersSpotPortfolios()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetAll/NonKvalUsers/KPUR/Spot/Portfolios");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            result = await _repository.GetAllNonKvalKpurUsersSpotPortfolios();

            if (result.Response.IsSuccess)
            {
                if (result.MatrixClientCodesList.Count == 0)
                {
                    result.Response.Messages.Add("(404) No portfolios found ");
                }
            }

            return Ok(result);
        }

        [HttpGet("GetAll/NonKvalUsers/KPUR/Cd/Portfolios")]
        public async Task<IActionResult> GetAllNonKvalKpurUsersCdPortfolios()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetAll/NonKvalUsers/KPUR/Cd/Portfolios");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            result = await _repository.GetAllNonKvalKpurUsersCdPortfolios();

            if (result.Response.IsSuccess)
            {
                if (result.MatrixClientCodesList.Count == 0)
                {
                    result.Response.Messages.Add("(404) No portfolios found ");
                }
            }

            return Ok(result);
        }

        [HttpGet("GetAll/KvalUsers/KPUR/Spot/Portfolios")]
        public async Task<IActionResult> GetAllKvalUsersKpurSpotPortfolios()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetAll/KvalUsers/KPUR/Spot/Portfolios");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            result = await _repository.GetAllKvalUsersKpurSpotPortfolios();

            if (result.Response.IsSuccess)
            {
                if (result.MatrixClientCodesList.Count == 0)
                {
                    result.Response.Messages.Add("(404) No portfolios found ");
                }
            }

            return Ok(result);
        }

        [HttpGet("GetAll/KvalUsers/KSUR/Cd/Portfolios")]
        public async Task<IActionResult> GetAllKvalUsersKsurCdPortfolios()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetAll/KvalUsers/KSUR/Cd/Portfolios");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            result = await _repository.GetAllKvalUsersKsurCdPortfolios();

            if (result.Response.IsSuccess)
            {
                if (result.MatrixClientCodesList.Count == 0)
                {
                    result.Response.Messages.Add("(404) No portfolios found ");
                }
            }

            return Ok(result);
        }

        [HttpGet("GetAll/KvalUsers/KPUR/Cd/Portfolios")]
        public async Task<IActionResult> GetAllKvalUsersKpurCdPortfolios()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetAll/KvalUsers/KPUR/Cd/Portfolios");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            result = await _repository.GetAllKvalUsersKpurCdPortfolios();

            if (result.Response.IsSuccess)
            {
                if (result.MatrixClientCodesList.Count == 0)
                {
                    result.Response.Messages.Add("(404) No portfolios found ");
                }
            }

            return Ok(result);
        }

        //[HttpGet("GetAll/Frendly/NonResident/Spot/Portfolios")]
        //public async Task<IActionResult> GetAllFrendlyNonResidentSpotPortfolios()
        //{
        //    _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetAll/Frendly/NonResident/Spot/Portfolios");

        //    MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

        //    result = await _repository.GetAllFrendlyNonResidentSpotPortfolios();

        //    if (result.Response.IsSuccess)
        //    {
        //        if (result.MatrixClientCodesList.Count == 0)
        //        {
        //            result.Response.Messages.Add("(404) No portfolios found ");
        //        }
        //    }

        //    return Ok(result);
        //}

        [HttpGet("GetAll/Frendly/NonResident/Kval/Spot/Portfolios")]
        public async Task<IActionResult> GetAllFrendlyNonResidentKvalSpotPortfolios()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetAll/Frendly/NonResident/Kval/Spot/Portfolios");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            result = await _repository.GetAllFrendlyNonResidentKvalSpotPortfolios();

            if (result.Response.IsSuccess)
            {
                if (result.MatrixClientCodesList.Count == 0)
                {
                    result.Response.Messages.Add("(404) No portfolios found ");
                }
            }

            return Ok(result);
        }
        [HttpGet("GetAll/Frendly/NonResident/NonKval/Spot/Portfolios")]
        public async Task<IActionResult> GetAllFrendlyNonResidentNonKvalSpotPortfolios()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetAll/Frendly/NonResident/NonKval/Spot/Portfolios");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            result = await _repository.GetAllFrendlyNonResidentNonKvalSpotPortfolios();

            if (result.Response.IsSuccess)
            {
                if (result.MatrixClientCodesList.Count == 0)
                {
                    result.Response.Messages.Add("(404) No portfolios found ");
                }
            }

            return Ok(result);
        }


        [HttpGet("GetAll/Frendly/NonResident/Cd/Portfolios")]
        public async Task<IActionResult> GetAllFrendlyNonResidentCdPortfolios()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetAll/Frendly/NonResident/Cd/Portfolios");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            result = await _repository.GetAllFrendlyNonResidentCdPortfolios();

            if (result.Response.IsSuccess)
            {
                if (result.MatrixClientCodesList.Count == 0)
                {
                    result.Response.Messages.Add("(404) No portfolios found ");
                }
            }

            return Ok(result);
        }

        [HttpGet("GetAll/Enemy/NonResident/Spot/Portfolios")]
        public async Task<IActionResult> GetAllEnemyNonResidentSpotPortfolios()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetAll/Enemy/NonResident/Spot/Portfolios");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            result = await _repository.GetAllEnemyNonResidentSpotPortfolios();

            if (result.Response.IsSuccess)
            {
                if (result.MatrixClientCodesList.Count == 0)
                {
                    result.Response.Messages.Add("(404) No portfolios found ");
                }
            }

            return Ok(result);
        }

        [HttpGet("GetAll/Enemy/NonResident/Cd/Portfolios")]
        public async Task<IActionResult> GetAllEnemyNonResidentCdPortfolios()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetAll/Enemy/NonResident/Cd/Portfolios");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            result = await _repository.GetAllEnemyNonResidentCdPortfolios();

            if (result.Response.IsSuccess)
            {
                if (result.MatrixClientCodesList.Count == 0)
                {
                    result.Response.Messages.Add("(404) No portfolios found ");
                }
            }

            return Ok(result);
        }

        [HttpGet("GetAll/Enemy/NonResident/Forts/Codes")]
        public async Task<IActionResult> GetAllEnemyNonResidentFortsCodes()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetAll/Enemy/NonResident/Forts/Codes");

            FortsClientCodeModelResponse result = new FortsClientCodeModelResponse();

            result = await _repository.GetAllEnemyNonResidentFortsCodes();

            if (result.Response.IsSuccess)
            {
                if (result.FortsClientCodesList.Count == 0)
                {
                    result.Response.Messages.Add("(404) No portfolios found ");
                }
            }

            return Ok(result);
        }

        [HttpGet("GetAll/NonKvalUsers/WithTest16/Forts/Codes")]
        public async Task<IActionResult> GetAllNonKvalUsersWithTest16FortsCodes()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetAll/NonKvalUsers/WithTest16/Forts/Codes");

            FortsClientCodeModelResponse result = new FortsClientCodeModelResponse();

            result = await _repository.GetAllNonKvalUsersWithTest16FortsCodes();

            if (result.Response.IsSuccess)
            {
                if (result.FortsClientCodesList.Count == 0)
                {
                    result.Response.Messages.Add("(404) No portfolios found ");
                }
            }

            return Ok(result);
        }

        [HttpGet("GetAll/KvalUsers/Forts/Codes")]
        public async Task<IActionResult> GetAllKvalUsersFortsCodes()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetAll/KvalUsers/Forts/Codes");

            FortsClientCodeModelResponse result = new FortsClientCodeModelResponse();

            result = await _repository.GetAllKvalUsersFortsCodes();

            if (result.Response.IsSuccess)
            {
                if (result.FortsClientCodesList.Count == 0)
                {
                    result.Response.Messages.Add("(404) No portfolios found ");
                }
            }

            return Ok(result);
        }


        [HttpGet("GetAll/KvalUsers/Spot/Portfolios")]
        public async Task<IActionResult> GetAllKvalUsersSpotPortfolios()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetAll/KvalUsers/Spot/Portfolios");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            result = await _repository.GetAllKvalUsersSpotPortfolios();

            if (result.Response.IsSuccess)
            {
                if (result.MatrixClientCodesList.Count == 0)
                {
                    result.Response.Messages.Add("(404) No portfolios found ");
                }
            }

            return Ok(result);
        }

        [HttpGet("GetAll/NonKvalUsers/SpotPortfolios/and/TestForComplexProduct")]
        public async Task<IActionResult> GetAllNonKvalUsersSpotPortfoliosAndTestForComplexProduct()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetAll/NonKvalUsers/SpotPortfolios/and/TestForComplexProduct");

            PortfoliosAndTestForComplexProductResponse result = new PortfoliosAndTestForComplexProductResponse();

            result = await _repository.GetAllNonKvalUsersSpotPortfoliosAndTestForComplexProduct();

            if (result.Response.IsSuccess)
            {
                if (result.TestForComplexProductList.Count == 0)
                {
                    result.Response.Messages.Add("(404) No Portfolios and Test For Complex Product found ");
                }
            }

            return Ok(result);
        }
    }
}
