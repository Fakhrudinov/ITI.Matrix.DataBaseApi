using DataAbstraction.Connections;
using DataAbstraction.Interfaces;
using DataAbstraction.Models;
using DataAbstraction.Responses;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;

namespace Matrix.DataBase
{
    public class DataBaseRepository : IDataBaseRepository
    {
        private ILogger<DataBaseRepository> _logger;
        private readonly string _connectionString = "";

        //команды не должны содержать ; в конце! 

        public DataBaseRepository(IOptions<DataBaseConnectionConfiguration> connection, ILogger<DataBaseRepository> logger)
        {
            _logger = logger;
            _connectionString = connection.Value.ConnectionString + " User Id=" + connection.Value.Login + "; Password=" + connection.Value.Password + ";";
        }

        public async Task<ListStringResponseModel> CheckConnections()
        {
            _logger.LogInformation($"DBRepository CheckConnections Called");

            ListStringResponseModel response = new ListStringResponseModel();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "queryCheckConnection.sql");
            if (!File.Exists(filePath))
            {
                response.IsSuccess = false;
                response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return response;
            }

            string _queryCheckConnection = File.ReadAllText(filePath);

            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    OracleCommand command = new OracleCommand(_queryCheckConnection, connection);

                    _logger.LogInformation($"DBRepository CheckConnections try to connect");
                    await connection.OpenAsync();

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Messages.Add(reader.GetString(0));
                        }
                    }

                    command.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"DBRepository CheckConnections Failed, Exception: " + ex.Message);

                response.IsSuccess = false;
                response.Messages.Add("Exception at DataBase: " + ex.Message);
                return response;
            }

            _logger.LogInformation($"DBRepository CheckConnections Success");
            return response;
        }

        public async Task<MatrixClientCodeModelResponse> GetUserSpotPortfolios(string clientCode)
        {
            _logger.LogInformation($"DBRepository GetUserSpotPortfolios for {clientCode} Called");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "queryGetAllSpotPortfolios.sql");
            if (!File.Exists(filePath))
            {
                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string _queryGetAllSpotPortfolios = File.ReadAllText(filePath);

            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    OracleCommand command = new OracleCommand(_queryGetAllSpotPortfolios, connection);
                    command.Parameters.Add(":clientCode", clientCode);

                    _logger.LogInformation($"DBRepository GetUserSpotPortfolios try to connect");
                    await connection.OpenAsync();

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            MatrixClientCodeModel portfolio = new MatrixClientCodeModel();
                            portfolio.MatrixClientCode = reader.GetString(0);

                            result.MatrixClientCodesList.Add(portfolio);
                        }
                    }

                    command.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"DBRepository GetUserSpotPortfolios Failed, Exception: " + ex.Message);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add($"DBRepository GetUserSpotPortfolios Failed, Exception: " + ex.Message);
            }

            _logger.LogInformation($"DBRepository GetUserSpotPortfolios Success");
            return result;
        }

        public async Task<MatrixToFortsCodesMappingResponse> GetUserFortsPortfolios(string clientCode)
        {
            _logger.LogInformation($"DBRepository GetUserFortsPortfolios for {clientCode} Called");

            MatrixToFortsCodesMappingResponse result = new MatrixToFortsCodesMappingResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "queryGetAllFortsPortfolios.sql");
            if (!File.Exists(filePath))
            {
                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string _queryGetAllFortsPortfolios = File.ReadAllText(filePath);

            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    OracleCommand command = new OracleCommand(_queryGetAllFortsPortfolios, connection);
                    command.Parameters.Add(":clientCode", clientCode);

                    _logger.LogInformation($"DBRepository GetUserFortsPortfolios try to connect");
                    await connection.OpenAsync();

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            MatrixToFortsCodesMappingModel portfolioMapping = new MatrixToFortsCodesMappingModel();
                            portfolioMapping.MatrixClientCode = reader.GetString(0);
                            portfolioMapping.FortsClientCode = reader.GetString(1);

                            result.MatrixToFortsCodesList.Add(portfolioMapping);
                        }
                    }

                    command.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"DBRepository GetUserFortsPortfolios Failed, Exception: " + ex.Message);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add($"DBRepository GetUserFortsPortfolios Failed, Exception: " + ex.Message);
            }

            _logger.LogInformation($"DBRepository GetUserFortsPortfolios Success");
            return result;
        }

        public async Task<MatrixToFortsCodesMappingResponse> GetUserFortsPortfoliosNoEDP(string clientCode)
        {
            _logger.LogInformation($"DBRepository GetUserFortsPortfolios for {clientCode} Called");

            MatrixToFortsCodesMappingResponse result = new MatrixToFortsCodesMappingResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "queryGetAllFortsNoEDPPortfolios.sql");
            if (!File.Exists(filePath))
            {
                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string _queryGetAllFortsNoEDPPortfolios = File.ReadAllText(filePath);

            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    OracleCommand command = new OracleCommand(_queryGetAllFortsNoEDPPortfolios, connection);
                    command.Parameters.Add(":clientCode", clientCode);

                    _logger.LogInformation($"DBRepository GetUserFortsPortfolios try to connect");
                    await connection.OpenAsync();

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            MatrixToFortsCodesMappingModel portfolioMapping = new MatrixToFortsCodesMappingModel();
                            portfolioMapping.MatrixClientCode = reader.GetString(0);
                            portfolioMapping.FortsClientCode = reader.GetString(1);

                            result.MatrixToFortsCodesList.Add(portfolioMapping);
                        }
                    }

                    command.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"DBRepository GetUserFortsPortfolios Failed, Exception: " + ex.Message);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add($"DBRepository GetUserFortsPortfolios Failed, Exception: " + ex.Message);
            }

            _logger.LogInformation($"DBRepository GetUserFortsPortfolios Success");
            return result;
        }

        public async Task<BoolResponse> GetIsPortfolioInEDP(string clientPortfolio)
        {
            _logger.LogInformation($"DBRepository GetIsPortfolioInEDP for {clientPortfolio} Called");

            BoolResponse result = new BoolResponse();
            string requestResult = null;

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "queryGetPortfolioEDPBelongings.sql");
            if (!File.Exists(filePath))
            {
                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string _queryGetPortfolioEDPBelongings = File.ReadAllText(filePath);

            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    OracleCommand command = new OracleCommand(_queryGetPortfolioEDPBelongings, connection);
                    command.Parameters.Add(":clientportfolio", clientPortfolio);

                    _logger.LogInformation($"DBRepository GetIsPortfolioInEDP try to connect");
                    await connection.OpenAsync();

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            requestResult = reader.GetString(0);
                        }
                    }

                    command.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"DBRepository GetIsPortfolioInEDP Failed, Exception: " + ex.Message);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add($"DBRepository GetIsPortfolioInEDP Failed, Exception: " + ex.Message);
            }

            _logger.LogInformation($"DBRepository GetIsPortfolioInEDP Success");

            if (requestResult == null)
            {
                result.Response.Messages.Add($"(404)");
                return result;
            }

            if (!requestResult.Contains("-MO-"))
            {
                result.IsTrue = true;
            }

            return result;
        }


        public async Task<BoolResponse> GetIsClientBelongsToQUIK(string clientCode)
        {
            _logger.LogInformation($"DBRepository GetIsClientBelongsToQUIK for {clientCode} Called");

            BoolResponse result = new BoolResponse();
            string requestResult = null;

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "queryGetIsClientBelongsToQUIK.sql");
            if (!File.Exists(filePath))
            {
                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string queryGetIsClientBelongsToQUIK = File.ReadAllText(filePath);

            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    OracleCommand command = new OracleCommand(queryGetIsClientBelongsToQUIK, connection);
                    command.Parameters.Add(":clientCode", clientCode);

                    _logger.LogInformation($"DBRepository GetIsClientBelongsToQUIK try to connect");
                    await connection.OpenAsync();

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            requestResult = reader.GetString(0);
                        }
                    }

                    command.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"DBRepository GetIsClientBelongsToQUIK Failed, Exception: " + ex.Message);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add($"DBRepository GetIsClientBelongsToQUIK Failed, Exception: " + ex.Message);
            }

            _logger.LogInformation($"DBRepository GetIsClientBelongsToQUIK Success");

            if (requestResult == null)
            {
                result.Response.Messages.Add($"(404)");
                return result;
            }

            if (requestResult.Equals("Q"))
            {
                result.IsTrue = true;
            }

            return result;
        }

        public async Task<ClientInformationResponse> GetUserPersonalInfo(string clientCode)
        {
            _logger.LogInformation($"DBRepository GetUserPersonalInfo for {clientCode} Called");

            ClientInformationResponse result = new ClientInformationResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "queryGetPersonalInfo.sql");
            if (!File.Exists(filePath))
            {
                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string _queryGetPersonalInfo = File.ReadAllText(filePath);

            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    OracleCommand command = new OracleCommand(_queryGetPersonalInfo, connection);
                    command.Parameters.Add(":clientCode", clientCode);

                    _logger.LogInformation($"DBRepository GetUserPersonalInfo try to connect");
                    await connection.OpenAsync();

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.ClientInformation.LastName = reader.GetString(0);

                            if (!reader.IsDBNull(1))
                            {
                                result.ClientInformation.EMail = reader.GetString(1);
                            }
                            else
                            {
                                result.ClientInformation.EMail = "noEmailFound@inDataBase.mtrx";
                            }
                        }
                    }

                    command.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"DBRepository GetUserPersonalInfo Failed, Exception: " + ex.Message);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add($"DBRepository GetUserPersonalInfo Failed, Exception: " + ex.Message);
            }

            _logger.LogInformation($"DBRepository GetUserPersonalInfo Success");
            return result;
        }

        public async Task<ClientBOInformationResponse> GetUserBOPersonalInfo(string clientCode)
        {
            _logger.LogInformation($"DBRepository GetUserBOPersonalInfo for {clientCode} Called");

            ClientBOInformationResponse result = new ClientBOInformationResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "queryGetBOPersonalInfo.sql");
            if (!File.Exists(filePath))
            {
                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }
            string queryGetBOPersonalInfo = File.ReadAllText(filePath);

            string clientType = "";
            string registerDate = "";
            int clientResidensyType = 0;

            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    OracleCommand command = new OracleCommand(queryGetBOPersonalInfo, connection);
                    command.Parameters.Add(":clientCode", clientCode);

                    _logger.LogInformation($"DBRepository GetUserBOPersonalInfo try to connect");
                    await connection.OpenAsync();

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            clientType = reader.GetString(0);
                            registerDate = reader.GetString(1);

                            if (!reader.IsDBNull(2))
                            {
                                result.ClientBOInformation.Address = reader.GetString(2);
                            }

                            if (!reader.IsDBNull(3))
                            {
                                clientResidensyType = reader.GetInt32(3);
                            }
                        }
                    }

                    command.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"DBRepository GetUserBOPersonalInfo Failed, Exception: " + ex.Message);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add($"DBRepository GetUserBOPersonalInfo Failed, Exception: " + ex.Message);
            }

            //проверка успешности и обработка данных
            if (clientType.Equals(""))
            {
                result.Response.Messages.Add("(404) type not found");
                return result;
            }
            else
            {
                if (clientType.Equals("P"))
                {
                    result.ClientBOInformation.isClientPerson = true;
                }
                else
                {
                    result.ClientBOInformation.isClientPerson = false;
                }
            }

            if (registerDate.Equals(""))
            {
                result.Response.Messages.Add("(404) date not found");
                return result;
            }
            else
            {
                //from db result:       11.10.17         == ДД.ММ.ГГ
                registerDate = registerDate.Replace(",", ".");
                //need to be result:    20160714         == Дата заключения договора.Формат: ГГГГММДД. 
                var dateParts = registerDate.Split(".");

                registerDate = "20" + dateParts[2] + dateParts[1] + dateParts[0];

                result.ClientBOInformation.RegisterDate = Int32.Parse(registerDate);
            }

            if (clientResidensyType == 2)
            {
                result.ClientBOInformation.isClientResident = false;
            }
            else
            {
                result.ClientBOInformation.isClientResident = true;
            }

            _logger.LogInformation($"DBRepository GetUserBOPersonalInfo Success");
            return result;
        }
    }
}