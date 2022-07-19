using DataAbstraction.Interfaces;
using DataAbstraction.Models;
using DataAbstraction.Responses;
using DataValidationService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace ITI.Matrix.DB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DBClientController : ControllerBase
    {
        private ILogger<DBClientController> _logger;
        private IDataBaseRepository _repository;
        private PortfoliosAllowedForNonEDP _portfolioFilter;


        public DBClientController(ILogger<DBClientController> logger, IDataBaseRepository repository, IOptions<PortfoliosAllowedForNonEDP> portfolioFilter)
        {
            _logger = logger;
            _repository = repository;
            _portfolioFilter = portfolioFilter.Value;
        }


        [HttpGet("GetUser/SpotPortfolios/{clientCode}")]
        public async Task<IActionResult> GetUserSpotPortfolios(string clientCode)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetUser/SpotPortfolios {clientCode} Call");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            result.Response = Validator.ValidateClientCode(clientCode);
            if (!result.Response.IsSuccess)
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetUser/SpotPortfolios {clientCode} Validation Fail: {result.Response.Messages[0]}");
                return Ok(result);
            }

            result = await _repository.GetUserSpotPortfolios(clientCode);

            if (result.Response.IsSuccess)
            {
                if (result.MatrixClientCodesList.Count == 0)
                {
                    result.Response.IsSuccess = false;
                    result.Response.Messages.Add("(404) Not found: " + clientCode);
                }
            }

            return Ok(result);
        }

        [HttpGet("GetUser/SpotPortfolios/Filtered/{clientCode}")]
        public async Task<IActionResult> GetUserSpotPortfoliosFiltered(string clientCode)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetUser/SpotPortfolios/Filtered {clientCode} Call");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            result.Response = Validator.ValidateClientCode(clientCode);
            if (!result.Response.IsSuccess)
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetUser/SpotPortfolios {clientCode} Validation Fail: {result.Response.Messages[0]}");
                return Ok(result);
            }

            result = await _repository.GetUserSpotPortfolios(clientCode);

            if (result.Response.IsSuccess)
            {
                if (result.MatrixClientCodesList.Count == 0)
                {
                    result.Response.IsSuccess = false;
                    result.Response.Messages.Add("(404) Not found: " + clientCode);
                }
                else
                {
                    // remove portfolios - which not passed filter
                    for (int i = result.MatrixClientCodesList.Count - 1; i >= 0; i--)
                    {
                        if (!_portfolioFilter.PortfolioList.Contains(result.MatrixClientCodesList[i].MatrixClientPortfolio.Split("-")[1]))
                        {
                            result.MatrixClientCodesList.Remove(result.MatrixClientCodesList[i]);
                        }
                    }
                }
            }

            return Ok(result);
        }

        [HttpGet("GetUser/FortsPortfolios/{clientCode}")]
        public async Task<IActionResult> GetUserFortsPortfolios(string clientCode)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetUser/FortsPortfolios {clientCode} Call");

            MatrixToFortsCodesMappingResponse result = new MatrixToFortsCodesMappingResponse();

            result.Response = Validator.ValidateClientCode(clientCode);
            if (!result.Response.IsSuccess)
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetUser/FortsPortfolios {clientCode} Validation Fail: {result.Response.Messages[0]}");
                return Ok(result);
            }

            result = await _repository.GetUserFortsPortfolios(clientCode);

            if (result.Response.IsSuccess)
            {
                if (result.MatrixToFortsCodesList.Count == 0)
                {
                    result.Response.IsSuccess = false;
                    result.Response.Messages.Add("(404) Not found: " + clientCode);
                }
            }

            return Ok(result);
        }

        [HttpGet("GetUser/FortsPortfolios/NoEDP/{clientCode}")]
        public async Task<IActionResult> GetUserFortsNoEDPPortfolios(string clientCode)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetUser/NoEDP/FortsPortfolios {clientCode} Call");

            MatrixToFortsCodesMappingResponse result = new MatrixToFortsCodesMappingResponse();

            result.Response = Validator.ValidateClientCode(clientCode);
            if (!result.Response.IsSuccess)
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetUser/FortsPortfolios {clientCode} Validation Fail: {result.Response.Messages[0]}");
                return Ok(result);
            }

            result = await _repository.GetUserFortsPortfoliosNoEDP(clientCode);

            if (result.Response.IsSuccess)
            {
                if (result.MatrixToFortsCodesList.Count == 0)
                {
                    result.Response.IsSuccess = false;
                    result.Response.Messages.Add("(404) Not found: " + clientCode);
                }
            }

            return Ok(result);
        }

        [HttpGet("Get/IsPortfolios/nonEDP/{clientPortfolio}")]
        public async Task<IActionResult> GetIsPortfolioInEDP(string clientPortfolio)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet Get/IsPortfolios/nonEDP/{clientPortfolio} Call");

            BoolResponse result = new BoolResponse();

            result.Response = Validator.ValidateClientPortfolio(clientPortfolio);
            if (!result.Response.IsSuccess)
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet Get/IsPortfolios/nonEDP/{clientPortfolio} Validation Fail: {result.Response.Messages[0]}");
                return Ok(result);
            }

            result = await _repository.GetIsPortfolioInEDP(clientPortfolio);

            if (result.Response.IsSuccess)
            {
                if (result.Response.Messages.Count > 0 && result.Response.Messages[0].Equals("(404)"))
                {
                    result.Response.IsSuccess = false;
                    result.Response.Messages.Add("(404) Not found: " + clientPortfolio);
                }
                else
                {
                    if (!result.IsTrue)
                    {
                        result.Response.Messages.Add("False. Portfolio belong to EDP.");
                    }
                    else
                    {
                        result.Response.Messages.Add("Ok. Portfolio non EDP.");
                    }
                }
            }

            return Ok(result);
        }

        [HttpGet("Get/IsClient/inQUIK/{clientCode}")]
        public async Task<IActionResult> GetIsClientBelongsToQUIK(string clientCode)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet Get/IsClient/inQUIK/{clientCode} Call");

            BoolResponse result = new BoolResponse();

            result.Response = Validator.ValidateClientCode(clientCode);
            if (!result.Response.IsSuccess)
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet Get/IsClient/inQUIK/{clientCode} Validation Fail: {result.Response.Messages[0]}");
                return Ok(result);
            }

            result = await _repository.GetIsClientBelongsToQUIK(clientCode);

            if (result.Response.IsSuccess)
            {
                if (result.Response.Messages.Count > 0 && result.Response.Messages[0].Equals("(404)"))
                {
                    result.Response.IsSuccess = false;
                    result.Response.Messages.Add("(404) Not found: " + clientCode);
                }
                else
                {
                    if (result.IsTrue)
                    {
                        result.Response.Messages.Add("Ok. It is QUIK client");
                    }
                    else
                    {
                        result.Response.Messages.Add("False. It is MATRIX client");
                    }
                }
            }

            return Ok(result);
        }

        [HttpGet("GetUser/PersonalInfo/{clientCode}")]
        public async Task<IActionResult> GetUserPersonalInfo(string clientCode)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetUser/PersonalInfo/{clientCode} Call");

            ClientInformationResponse result = new ClientInformationResponse();

            result.Response = Validator.ValidateClientCode(clientCode);
            if (!result.Response.IsSuccess)
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetUser/PersonalInfo/{clientCode} Validation Fail: {result.Response.Messages[0]}");
                return Ok(result);
            }

            result = await _repository.GetUserPersonalInfo(clientCode);

            if (result.Response.IsSuccess)
            {
                if (result.ClientInformation.LastName is null)
                {
                    result.Response.IsSuccess = false;
                    result.Response.Messages.Add("(404) Not found: " + clientCode);
                }
                else
                {
                    if (clientCode.StartsWith("BC") || clientCode.StartsWith("AA"))
                    {
                        result = WorkWithOrganizationClientName(result);
                    }
                    else
                    {
                        result = WorkWithPersonClientName(result);
                    }
                }
            }

            return Ok(result);
        }

        [HttpGet("WarmUp/BackOfficeDataBase")]
        public async Task<IActionResult> WarmUpBackOfficeDataBase()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet WarmUp/BackOfficeDataBase Call");

            await _repository.WarmUpBackOfficeDataBase();

            return Ok();
        }

        [HttpGet("GetUser/PersonalInfo/BackOffice/{clientCode}")]
        public async Task<IActionResult> GetUserBOPersonalInfo(string clientCode)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetUser/PersonalInfo/BackOffice/{clientCode} Call");

            ClientBOInformationResponse result = new ClientBOInformationResponse();

            result.Response = Validator.ValidateClientCode(clientCode);
            if (!result.Response.IsSuccess)
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} HttpGet GetUser/PersonalInfo/BackOffice/{clientCode} Validation Fail: {result.Response.Messages[0]}");
                return Ok(result);
            }

            result = await _repository.GetUserBOPersonalInfo(clientCode);

            if (result.Response.IsSuccess)
            {
                if (result.ClientBOInformation.RegisterDate == 0)
                {
                    result.Response.IsSuccess = false;
                    result.Response.Messages.Add("(404) Not found: " + clientCode);
                }
                else
                {
                    //clean address
                    result.ClientBOInformation.Address = CleadClientAddress(result.ClientBOInformation.Address);
                }
            }

            return Ok(result);
        }

        private string CleadClientAddress(string address)
        {
            string pattern = @"[^ |A-Za-z|А-яа-яЁё|\.|,|/|\d|-]";
            
            Regex regex = new Regex(pattern);
            string result = regex.Replace(address, "");
            result = Regex.Replace(result, "\\,+", ",");
            result = Regex.Replace(result, ",+", ", ");
            result = Regex.Replace(result, "\\s+", " ");//, ,
            result = Regex.Replace(result, ", , ", ", ");
            result = result.Trim();
            
            return result;
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
