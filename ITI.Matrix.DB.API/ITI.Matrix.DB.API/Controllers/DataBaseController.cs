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

            if (result.Response.IsSuccess)
            {
                if (result.Response.Messages.Count > 0 && result.Response.Messages[0].Equals("(404)"))
                {
                    return NotFound("(404) not found portfolio " + clientRfPortfolio);
                }

                if (!result.IsTrue)
                {
                    result.Response.Messages.Add("False. Portfolio belong to EDP.");
                }
                else
                {
                    result.Response.Messages.Add("Ok. Portfolio non EDP.");
                }
                
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Response.Messages);
            }
        }

        [HttpGet("GetUser/PersonalInfo/{clientCode}")]
        public async Task<IActionResult> GetUserPersonalInfo(string clientCode)
        {
            _logger.LogInformation($"HttpGet GetUser/PersonalInfo/{clientCode} Call");

            ClientInformationResponse result = await _repository.GetUserPersonalInfo(clientCode);

            if (result.Response.IsSuccess)
            {
                if (result.ClientInformation.LastName is null)
                {
                    return NotFound();
                }

                if (clientCode.StartsWith("BC") || clientCode.StartsWith("AA"))
                {
                    result = WorkWithOrganizationClientName(result);
                }
                else
                {
                    result = WorkWithPersonClientName(result);
                }
                

                return Ok(result);
            }
            else
            {
                return BadRequest(result.Response.Messages);
            }
        }

        private ClientInformationResponse WorkWithPersonClientName(ClientInformationResponse result)
        {
            if (result.ClientInformation.LastName.Contains(" "))
            {
                var nameArray = result.ClientInformation.LastName.Split(" ");

                if (nameArray.Length == 3)//Иванов Иван Иваныч
                {
                    result.ClientInformation.LastName = nameArray[0];
                    result.ClientInformation.FirstName = nameArray[1];
                    result.ClientInformation.MiddleName = nameArray[2];
                }
                else if (nameArray.Length == 2)//Иванов Иван // BP67352
                {
                    result.ClientInformation.LastName = nameArray[0];
                    result.ClientInformation.FirstName = nameArray[1];
                }
                else//больше 3х слов в имени //BP66476
                {                    
                    result.ClientInformation.FirstName = result.ClientInformation.LastName
                        .Replace(nameArray[0], "")
                        .Trim();
                    result.ClientInformation.LastName = nameArray[0];
                }
            }

            return result;
        }

        private ClientInformationResponse WorkWithOrganizationClientName(ClientInformationResponse result)
        {
            Dictionary<string, string> keyWords = new Dictionary<string, string>()
            {
                { "ООО", "общество с ограниченной ответственностью" },
                { "ЗАО", "закрытое акционерное общество" },
                { "ПАО", "публичное акционерное общество"},
                { "АО", "акционерное общество"}
            };
            foreach (KeyValuePair<string, string> kvp in keyWords)
            {
                if (result.ClientInformation.LastName.ToLower().Contains(kvp.Value))
                {
                    int index = result.ClientInformation.LastName.ToLower().IndexOf(kvp.Value);
                    int lenght = kvp.Value.Length;

                    result.ClientInformation.LastName = result.ClientInformation.LastName
                        .Remove(index, lenght)
                        .Trim();

                    result.ClientInformation.FirstName = kvp.Key;
                }
            }
            
            if (result.ClientInformation.FirstName is null)
            {
                if (result.ClientInformation.LastName.Contains("/"))
                {
                    var nameArray = result.ClientInformation.LastName.Split("/");
                    
                    result.ClientInformation.FirstName = result.ClientInformation.LastName
                        .Replace(nameArray[0], "");
                    result.ClientInformation.FirstName = result.ClientInformation.FirstName
                        .Substring(1);

                    result.ClientInformation.LastName = nameArray[0];
                }
                else if (result.ClientInformation.LastName.Contains(" "))
                {
                    var nameArray = result.ClientInformation.LastName.Split(" ");

                    result.ClientInformation.FirstName = result.ClientInformation.LastName
                        .Replace(nameArray[0], "");
                    result.ClientInformation.FirstName = result.ClientInformation.FirstName
                        .Substring(1);

                    result.ClientInformation.LastName = nameArray[0];
                }
            }

            return result;
        }
    }
}
