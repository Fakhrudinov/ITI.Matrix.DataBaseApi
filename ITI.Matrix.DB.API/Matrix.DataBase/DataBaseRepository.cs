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

        //команды sql не должны содержать ; в конце! 

        public DataBaseRepository(IOptions<DataBaseConnectionConfiguration> connection, ILogger<DataBaseRepository> logger)
        {
            _logger = logger;
            _connectionString = connection.Value.ConnectionString + " User Id=" + connection.Value.Login + "; Password=" + connection.Value.Password + ";";
        }

        public async Task<ListStringResponseModel> CheckConnections()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository CheckConnections Called");

            ListStringResponseModel response = new ListStringResponseModel();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "queryCheckConnection.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

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

                    _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository CheckConnections try to connect");
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
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository CheckConnections Failed, Exception: " + ex.Message);

                response.IsSuccess = false;
                response.Messages.Add("Exception at DataBase: " + ex.Message);
                return response;
            }

            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository CheckConnections Success");
            return response;
        }

        public async Task<MatrixClientCodeModelResponse> GetUserSpotPortfolios(string clientCode)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetUserSpotPortfolios for {clientCode} Called");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "queryGetAllSpotPortfolios.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

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

                    _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetUserSpotPortfolios try to connect");
                    await connection.OpenAsync();

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            MatrixClientPortfolioModel portfolio = new MatrixClientPortfolioModel();
                            portfolio.MatrixClientPortfolio = reader.GetString(0);

                            result.MatrixClientCodesList.Add(portfolio);
                        }
                    }

                    command.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetUserSpotPortfolios Failed, Exception: " + ex.Message);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add($"DBRepository GetUserSpotPortfolios Failed, Exception: " + ex.Message);
            }

            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetUserSpotPortfolios Success, " +
                $"records count returned {result.MatrixClientCodesList.Count}");
            return result;
        }

        public async Task<MatrixToFortsCodesMappingResponse> GetUserFortsPortfolios(string clientCode)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetUserFortsPortfolios for {clientCode} Called");

            MatrixToFortsCodesMappingResponse result = new MatrixToFortsCodesMappingResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "queryGetAllFortsPortfolios.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

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

                    _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetUserFortsPortfolios try to connect");
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
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetUserFortsPortfolios Failed, Exception: " + ex.Message);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add($"DBRepository GetUserFortsPortfolios Failed, Exception: " + ex.Message);
            }

            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetUserFortsPortfolios Success, " +
                $"records count returned {result.MatrixToFortsCodesList.Count}");
            return result;
        }

        public async Task<MatrixToFortsCodesMappingResponse> GetUserFortsPortfoliosNoEDP(string clientCode)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetUserFortsPortfolios for {clientCode} Called");

            MatrixToFortsCodesMappingResponse result = new MatrixToFortsCodesMappingResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "queryGetAllFortsNoEDPPortfolios.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

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

                    _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetUserFortsPortfolios try to connect");
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
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetUserFortsPortfolios Failed, Exception: " + ex.Message);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add($"DBRepository GetUserFortsPortfolios Failed, Exception: " + ex.Message);
            }

            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetUserFortsPortfolios Success, " +
                $"records count returned {result.MatrixToFortsCodesList.Count}");
            return result;
        }

        public async Task<BoolResponse> GetIsPortfolioInEDP(string clientPortfolio)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetIsPortfolioInEDP for {clientPortfolio} Called");

            BoolResponse result = new BoolResponse();
            string requestResult = null;

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "queryGetPortfolioEDPBelongings.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

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

                    _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetIsPortfolioInEDP try to connect");
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
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetIsPortfolioInEDP Failed, Exception: " + ex.Message);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add($"DBRepository GetIsPortfolioInEDP Failed, Exception: " + ex.Message);
            }

            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetIsPortfolioInEDP Success");

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
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetIsClientBelongsToQUIK for {clientCode} Called");

            BoolResponse result = new BoolResponse();
            string requestResult = null;

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "queryGetIsClientBelongsToQUIK.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

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

                    _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetIsClientBelongsToQUIK try to connect");
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
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetIsClientBelongsToQUIK Failed, Exception: " + ex.Message);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add($"DBRepository GetIsClientBelongsToQUIK Failed, Exception: " + ex.Message);
            }

            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetIsClientBelongsToQUIK Success");

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
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetUserPersonalInfo for {clientCode} Called");

            ClientInformationResponse result = new ClientInformationResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "queryGetPersonalInfo.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

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

                    _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetUserPersonalInfo try to connect");
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
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetUserPersonalInfo Failed, Exception: " + ex.Message);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add($"DBRepository GetUserPersonalInfo Failed, Exception: " + ex.Message);
            }

            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetUserPersonalInfo Success");
            return result;
        }

        public async Task WarmUpBackOfficeDataBase()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository WarmUpBackOfficeDataBase Called");

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "queryWarmUpBackOfficeDataBase.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository WarmUpBackOfficeDataBase Failed, Exception: File with request not found at path: " + filePath);
            }
            string queryWarmUpBackOfficeDataBase = File.ReadAllText(filePath);

            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    OracleCommand command = new OracleCommand(queryWarmUpBackOfficeDataBase, connection);


                    _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository WarmUpBackOfficeDataBase try to connect");
                    await connection.OpenAsync();

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            var result = reader.GetString(0);
                        }
                    }

                    command.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository WarmUpBackOfficeDataBase Failed, Exception: " + ex.Message);
            }

            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository WarmUpBackOfficeDataBase Success");
        }

        public async Task<ClientBOInformationResponse> GetUserBOPersonalInfo(string clientCode)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetUserBOPersonalInfo for {clientCode} Called");

            ClientBOInformationResponse result = new ClientBOInformationResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "queryGetBOPersonalInfo.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

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

                    _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetUserBOPersonalInfo try to connect");
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
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetUserBOPersonalInfo Failed, Exception: " + ex.Message);

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

            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetUserBOPersonalInfo Success");
            return result;
        }

        public async Task<MatrixClientCodeModelResponse> GetAllNonKvalUsersKsurSpotPortfolios()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetAllNonKvalUsersKsurSpotPortfolios Called");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "gueryGetAllNonKvalKsurUsersSpotPortfolios.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string _gueryGetAllNonKvalKsurUsersSpotPortfolios = File.ReadAllText(filePath);

            result = await GetMatrixClientCodeModelResponse(_gueryGetAllNonKvalKsurUsersSpotPortfolios);
            return result;
        }

        public async Task<MatrixClientCodeModelResponse> GetAllNonKvalKpurUsersSpotPortfolios()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetAllNonKvalKpurUsersSpotPortfolios Called");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "gueryGetAllNonKvalKpurUsersSpotPortfolios.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string _gueryGetAllNonKvalKpurUsersSpotPortfolios = File.ReadAllText(filePath);

            result = await GetMatrixClientCodeModelResponse(_gueryGetAllNonKvalKpurUsersSpotPortfolios);
            return result;
        }

        //public async Task<MatrixClientCodeModelResponse> GetAllFrendlyNonResidentSpotPortfolios()
        //{
        //    _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetAllFrendlyNonResidentSpotPortfolios Called");

        //    MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

        //    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "gueryGetAllFrendlyNonResidentSpotPortfolios.sql");
        //    if (!File.Exists(filePath))
        //    {
        //        _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

        //        result.Response.IsSuccess = false;
        //        result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
        //        return result;
        //    }

        //    string _gueryGetAllFrendlyNonResidentSpotPortfolios = File.ReadAllText(filePath);

        //    result = await GetMatrixClientCodeModelResponse(_gueryGetAllFrendlyNonResidentSpotPortfolios);
        //    return result;
        //}

        public async Task<MatrixClientCodeModelResponse> GetAllFrendlyNonResidentKvalSpotPortfolios()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetAllFrendlyNonResidentKvalSpotPortfolios Called");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "gueryGetAllFrendlyNonResidentKvalSpotPortfolios.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string gueryGetAllFrendlyNonResidentKvalSpotPortfolios = File.ReadAllText(filePath);

            result = await GetMatrixClientCodeModelResponse(gueryGetAllFrendlyNonResidentKvalSpotPortfolios);
            return result;
        }

        public async Task<MatrixClientCodeModelResponse> GetAllFrendlyNonResidentNonKvalSpotPortfolios()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetAllFrendlyNonResidentNonKvalSpotPortfolios Called");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "gueryGetAllFrendlyNonResidentNonKvalSpotPortfolios.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string gueryGetAllFrendlyNonResidentNonKvalSpotPortfolios = File.ReadAllText(filePath);

            result = await GetMatrixClientCodeModelResponse(gueryGetAllFrendlyNonResidentNonKvalSpotPortfolios);
            return result;
        }

        public async Task<MatrixClientCodeModelResponse> GetAllEnemyNonResidentSpotPortfolios()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetAllEnemyNonResidentSpotPortfolios Called");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "gueryGetAllEnemyNonResidentSpotPortfolios.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string _gueryGetAllEnemyNonResidentSpotPortfolios = File.ReadAllText(filePath);

            result = await GetMatrixClientCodeModelResponse(_gueryGetAllEnemyNonResidentSpotPortfolios);
            return result;
        }

        public async Task<MatrixClientCodeModelResponse> GetAllEnemyNonResidentCdPortfolios()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetAllEnemyNonResidentCdPortfolios Called");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "gueryGetAllEnemyNonResidentCdPortfolios.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string _gueryGetAllEnemyNonResidentCdPortfolios = File.ReadAllText(filePath);

            result = await GetMatrixClientCodeModelResponse(_gueryGetAllEnemyNonResidentCdPortfolios);
            return result;
        }

        public async Task<MatrixClientCodeModelResponse> GetAllFrendlyNonResidentCdPortfolios()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetAllFrendlyNonResidentCdPortfolios Called");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "gueryGetAllFrendlyNonResidentCdPortfolios.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string _gueryGetAllFrendlyNonResidentCdPortfolios = File.ReadAllText(filePath);

            result = await GetMatrixClientCodeModelResponse(_gueryGetAllFrendlyNonResidentCdPortfolios);
            return result;
        }

        public async Task<MatrixClientCodeModelResponse> GetAllNonKvalKpurUsersCdPortfolios()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetAllNonKvalUsersKpurCdPortfolios Called");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "gueryGetAllNonKvalKpurCdPortfolios.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string _gueryGetAllNonKvalKpurCdPortfolios = File.ReadAllText(filePath);

            result = await GetMatrixClientCodeModelResponse(_gueryGetAllNonKvalKpurCdPortfolios);
            return result;
        }

        public async Task<MatrixClientCodeModelResponse> GetAllNonKvalUsersKsurCdPortfolios()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetAllNonKvalUsersKsurCdPortfolios Called");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "gueryGetAllNonKvalKsurCdPortfolios.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string _gueryGetAllNonKvalKsurCdPortfolios = File.ReadAllText(filePath);

            result = await GetMatrixClientCodeModelResponse(_gueryGetAllNonKvalKsurCdPortfolios);
            return result;
        }

        public async Task<MatrixClientCodeModelResponse> GetAllRestrictedCDPortfolios()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetAllRestrictedCDPortfolios Called");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "queryGetCDPortfolioListForTemplateCD_Restrict.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string queryGetCDPortfolioListForTemplateCD_Restrict = File.ReadAllText(filePath);

            result = await GetMatrixClientCodeModelResponse(queryGetCDPortfolioListForTemplateCD_Restrict);
            return result;
        }

        public async Task<MatrixClientCodeModelResponse> GetAllAllowedCDPortfolios()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetAllAllowedCDPortfolios Called");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "queryGetCDPortfolioListForTemplateCD_portfolio.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string queryGetCDPortfolioListForTemplateCD_portfolio = File.ReadAllText(filePath);

            result = await GetMatrixClientCodeModelResponse(queryGetCDPortfolioListForTemplateCD_portfolio);
            return result;
        }

        public async Task<MatrixClientCodeModelResponse> GetAllKvalUsersKpurSpotPortfolios()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetAllKvalUsersKpurSpotPortfolios Called");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "gueryGetAllKvalKpurSpotPortfolios.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string _gueryGetAllKvalKpurSpotPortfolios = File.ReadAllText(filePath);

            result = await GetMatrixClientCodeModelResponse(_gueryGetAllKvalKpurSpotPortfolios);
            return result;
        }

        public async Task<MatrixClientCodeModelResponse> GetAllKvalUsersKsurCdPortfolios()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetAllKvalUsersKsurCdPortfolios Called");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "gueryGetAllKvalKsurCdPortfolios.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string _gueryGetAllKvalKsurCdPortfolios = File.ReadAllText(filePath);

            result = await GetMatrixClientCodeModelResponse(_gueryGetAllKvalKsurCdPortfolios);
            return result;
        }

        public async Task<MatrixClientCodeModelResponse> GetAllKvalUsersKpurCdPortfolios()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetAllKvalUsersKpurCdPortfolios Called");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "gueryGetAllKvalKpurCdPortfolios.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string _gueryGetAllKvalKpurCdPortfolios = File.ReadAllText(filePath);

            result = await GetMatrixClientCodeModelResponse(_gueryGetAllKvalKpurCdPortfolios);
            return result;
        }

        public async Task<MatrixClientCodeModelResponse> GetAllKvalUsersSpotPortfolios()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetAllKvalUsersSpotPortfolios Called");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "gueryGetAllKvalSpotPortfolios.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string _gueryGetAllKvalSpotPortfolios = File.ReadAllText(filePath);

            result = await GetMatrixClientCodeModelResponse(_gueryGetAllKvalSpotPortfolios);
            return result;
        }

        public async Task<MatrixClientCodeModelResponse> GetAllCDPortfolios()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetAllCDPortfolios Called");

            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "gueryGetAllCDPortfolios.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string _guerygueryGetAllCDPortfolios = File.ReadAllText(filePath);

            result = await GetMatrixClientCodeModelResponse(_guerygueryGetAllCDPortfolios);
            return result;
        }

        private async Task<MatrixClientCodeModelResponse> GetMatrixClientCodeModelResponse(string guery)
        {
            MatrixClientCodeModelResponse result = new MatrixClientCodeModelResponse();

            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    OracleCommand command = new OracleCommand(guery, connection);

                    _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetMatrixClientCodeModelResponse try to connect");
                    await connection.OpenAsync();

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            MatrixClientPortfolioModel portfolio = new MatrixClientPortfolioModel();
                            portfolio.MatrixClientPortfolio = reader.GetString(0);

                            result.MatrixClientCodesList.Add(portfolio);
                        }
                    }

                    command.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetMatrixClientCodeModelResponse Failed, Exception: " + ex.Message);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add($"DBRepository GetMatrixClientCodeModelResponse Failed, Exception: " + ex.Message);
            }

            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetMatrixClientCodeModelResponse Success, " +
                $"records count returned {result.MatrixClientCodesList.Count}");
            return result;
        }

        public async Task<FortsClientCodeModelResponse> GetAllEnemyNonResidentFortsCodes()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetAllEnemyNonResidentFortsCodes Called");

            FortsClientCodeModelResponse result = new FortsClientCodeModelResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "gueryGetAllEnemyNonResidentFortsCodes.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string _gueryGetAllEnemyNonResidentFortsCodes = File.ReadAllText(filePath);

            result = await GetFortsClientCodesModelResponse(_gueryGetAllEnemyNonResidentFortsCodes);
            return result;
        }

        public async Task<FortsClientCodeModelResponse> GetAllNonKvalUsersWithTest16FortsCodes()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetAllNonKvalUsersWithTest16FortsCodes Called");

            FortsClientCodeModelResponse result = new FortsClientCodeModelResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "gueryGetAllNonKvalUsersWithTest16FortsCodes.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string _gueryGetAllNonKvalUsersWithTest16FortsCodes = File.ReadAllText(filePath);

            result = await GetFortsClientCodesModelResponse(_gueryGetAllNonKvalUsersWithTest16FortsCodes);
            return result;
        }

        public async Task<FortsClientCodeModelResponse> GetAllKvalUsersFortsCodes()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetAllKvalUsersFortsCodes Called");

            FortsClientCodeModelResponse result = new FortsClientCodeModelResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "gueryGetAllKvalUsersFortsCodes.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string _gueryGetAllKvalUsersFortsCodes = File.ReadAllText(filePath);

            result = await GetFortsClientCodesModelResponse(_gueryGetAllKvalUsersFortsCodes);
            return result;
        }

        private async Task<FortsClientCodeModelResponse> GetFortsClientCodesModelResponse(string guery)
        {
            FortsClientCodeModelResponse result = new FortsClientCodeModelResponse();

            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    OracleCommand command = new OracleCommand(guery, connection);

                    _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetFortsClientCodesModelResponse try to connect");
                    await connection.OpenAsync();

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            FortsClientCodeModel portfolio = new FortsClientCodeModel();
                            portfolio.FortsClientCode = reader.GetString(0);

                            result.FortsClientCodesList.Add(portfolio);
                        }
                    }

                    command.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetFortsClientCodesModelResponse Failed, Exception: " + ex.Message);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add($"DBRepository GetFortsClientCodesModelResponse Failed, Exception: " + ex.Message);
            }

            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetFortsClientCodesModelResponse Success" +
                $", records count returned {result.FortsClientCodesList.Count}");
            return result;
        }

        public async Task<SecurityAndBoardResponse> GetSecuritiesSpotBlackListForNekval()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetSecuritiesSpotBlackListForNekval Called");

            SecurityAndBoardResponse result = new SecurityAndBoardResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "gueryGetSecuritiesSpotBlackListForNekval.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string _gueryGetSecuritiesSpotBlackListForNekval = File.ReadAllText(filePath);

            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    OracleCommand command = new OracleCommand(_gueryGetSecuritiesSpotBlackListForNekval, connection);

                    _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetSecuritiesSpotBlackListForNekval try to connect");
                    await connection.OpenAsync();

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            SecurityAndBoardModel secAndBord = new SecurityAndBoardModel();
                            
                            secAndBord.Board = reader.GetString(0);
                            secAndBord.Secutity = reader.GetString(1);

                            result.SecurityAndBoardList.Add(secAndBord);
                        }
                    }

                    command.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetSecuritiesSpotBlackListForNekval Failed, Exception: " + ex.Message);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add($"DBRepository GetSecuritiesSpotBlackListForNekval Failed, Exception: " + ex.Message);
            }

            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetSecuritiesSpotBlackListForNekval Success" +
                $", records count returned {result.SecurityAndBoardList.Count}");
            return result;
        }

        public async Task<PortfoliosAndTestForComplexProductResponse> GetAllNonKvalUsersSpotPortfoliosAndTestForComplexProduct()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetAllNonKvalUsersSpotPortfoliosAndTestForComplexProduct Called");

            PortfoliosAndTestForComplexProductResponse result = new PortfoliosAndTestForComplexProductResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "gueryGetNonKvalSpotPortfolioAndComplexProductTests.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string _gueryGetNonKvalSpotPortfolioAndComplexProductTests = File.ReadAllText(filePath);

            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    OracleCommand command = new OracleCommand(_gueryGetNonKvalSpotPortfolioAndComplexProductTests, connection);

                    _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetAllNonKvalUsersSpotPortfoliosAndTestForComplexProduct try to connect");
                    await connection.OpenAsync();

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            PortfoliosAndTestForComplexProductModel PortfolioAndTest = new PortfoliosAndTestForComplexProductModel();

                            PortfolioAndTest.MatrixClientPortfolio = reader.GetString(0);
                            PortfolioAndTest.TestForComplexProduct = reader.GetInt16(1);

                            result.TestForComplexProductList.Add(PortfolioAndTest);
                        }
                    }

                    command.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetAllNonKvalUsersSpotPortfoliosAndTestForComplexProduct Failed, Exception: " + ex.Message);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add($"DBRepository GetAllNonKvalUsersSpotPortfoliosAndTestForComplexProduct Failed, Exception: " + ex.Message);
            }

            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetAllNonKvalUsersSpotPortfoliosAndTestForComplexProduct Success" +
                $", records count returned {result.TestForComplexProductList.Count}");
            return result;
        }

        public async Task<MatrixClientAccountsModelResponse> GetAllUsersWithOptionWorkshop()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetAllUsersWithOptionWorkshop Called");

            MatrixClientAccountsModelResponse result = new MatrixClientAccountsModelResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "queryGetAllClientsWithOptionWorkshop.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string _queryGetAllClientsWithOptionWorkshop = File.ReadAllText(filePath);

            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    OracleCommand command = new OracleCommand(_queryGetAllClientsWithOptionWorkshop, connection);

                    _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetAllUsersWithOptionWorkshop try to connect");
                    await connection.OpenAsync();

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            MatrixClientAccountModel newAccount = new MatrixClientAccountModel();
                            newAccount.MatrixClientAccount = reader.GetString(0);

                            result.MatrixClientAccountList.Add(newAccount);
                        }
                    }

                    command.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetAllUsersWithOptionWorkshop Failed, Exception: " + ex.Message);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add($"DBRepository GetAllUsersWithOptionWorkshop Failed, Exception: " + ex.Message);
            }

            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetAllUsersWithOptionWorkshop Success, " +
                $"records count returned {result.MatrixClientAccountList.Count}");
            return result;
        }

        public async Task<BoolResponse> GetIsUserHaveOptionWorkshop(string clientCode)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetIsUserHaveOptionWorkshop for {clientCode} Called");

            BoolResponse result = new BoolResponse();
            string requestResult = null;

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "queryCheckIsClientHaveOptionWorkshop.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string queryCheckIsClientHaveOptionWorkshop = File.ReadAllText(filePath);

            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    OracleCommand command = new OracleCommand(queryCheckIsClientHaveOptionWorkshop, connection);
                    command.Parameters.Add(":clientCode", clientCode);

                    _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetIsUserHaveOptionWorkshop try to connect");
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
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetIsUserHaveOptionWorkshop Failed, Exception: " + ex.Message);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add($"DBRepository GetIsUserHaveOptionWorkshop Failed, Exception: " + ex.Message);
            }

            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetIsUserHaveOptionWorkshop Success");

            if (requestResult == null)
            {
                result.IsTrue = false;
            }
            else
            {
                result.IsTrue = true;
            }

            return result;
        }

        public async Task<ClientAndMoneyResponse> GetClientsSpotPortfoliosWhoTradesYesterday(int daysShift)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetClientsSpotPortfoliosWhoTradesYesterday Called with daysShift={daysShift}");

            ClientAndMoneyResponse result = new ClientAndMoneyResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "queryGetClientsWithMoneyWhoTradeYesterday.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string queryGetClientsWithMoneyWhoTradeYesterday = File.ReadAllText(filePath);

            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    OracleCommand command = new OracleCommand(queryGetClientsWithMoneyWhoTradeYesterday, connection);
                    command.Parameters.Add(":dayShiftOne", daysShift.ToString());
                    command.Parameters.Add(":dayShiftTwo", (daysShift - 1).ToString());

                    _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetClientsSpotPortfoliosWhoTradesYesterday try to connect");
                    await connection.OpenAsync();
                    
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            ClientAndMoneyModel newClient = new ClientAndMoneyModel();

                            newClient.MatrixClientPortfolio = reader.GetString(0);
                            newClient.Money = reader.GetDecimal(1);

                            if (reader.IsDBNull(2))
                            {
                                newClient.TKS = "";
                            }
                            else
                            {
                                newClient.TKS = reader.GetString(2);
                            }                            

                            result.Clients.Add(newClient);
                        }
                    }

                    command.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetClientsSpotPortfoliosWhoTradesYesterday Failed, Exception: " + ex.Message);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add($"DBRepository GetClientsSpotPortfoliosWhoTradesYesterday Failed, Exception: " + ex.Message);
            }

            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetClientsSpotPortfoliosWhoTradesYesterday Success" +
                $", clients count returned {result.Clients.Count}");

            return result;
        }

        public async Task<ClientDepoPositionsResponse> GetClientsPositionsByMatrixPortfolioList(IEnumerable<string> portfolios)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetClientsPositionsByMatrixPortfolioList Called");

            ClientDepoPositionsResponse result = new ClientDepoPositionsResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "queryGetClientsPositionsByPortfolioList.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string queryGetClientsPositionsByPortfolioList = File.ReadAllText(filePath);

            string parametresPortfolio = "";//сделать 'BP3871-MS-01','BP19195-CD-02', 'BP67722-MS-01', 'BP33736-MS-01'
            foreach (string portfolio in portfolios)
            {
                parametresPortfolio = parametresPortfolio + $",'{portfolio}'";
            }
            parametresPortfolio = parametresPortfolio.Substring(1);//убрать первую запятую
            //подставляем параметры в запрос
            queryGetClientsPositionsByPortfolioList = queryGetClientsPositionsByPortfolioList.Replace(":portfoliosList", parametresPortfolio);

            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    OracleCommand command = new OracleCommand(queryGetClientsPositionsByPortfolioList, connection);
                    //command.Parameters.Add(":portfoliosList", parametresPortfolio);//так почему то не работает.

                    _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetClientsPositionsByMatrixPortfolioList try to connect");
                    await connection.OpenAsync();

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            ClientDepoPositionModel newClient = new ClientDepoPositionModel();

                            newClient.MatrixClientPortfolio = reader.GetString(0);
                            newClient.SecCode = reader.GetString(1);
                            newClient.OpenBalance = reader.GetDecimal(2);
                            if (reader.IsDBNull(3))
                            {
                                newClient.AveragePrice = 0;
                            }
                            else
                            {
                                newClient.AveragePrice = reader.GetDecimal(3);
                            }
                            
                            if (reader.IsDBNull(4))
                            {
                                newClient.SecBoard = "NULL";
                            }
                            else
                            {
                                newClient.SecBoard = reader.GetString(4);
                            }

                            newClient.TKS = reader.GetString(5);

                            result.Clients.Add(newClient);
                        }
                    }

                    command.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetClientsPositionsByMatrixPortfolioList Failed, Exception: " + ex.Message);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add($"DBRepository GetClientsPositionsByMatrixPortfolioList Failed, Exception: " + ex.Message);
            }

            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetClientsPositionsByMatrixPortfolioList Success" +
                $", clients count returned {result.Clients.Count}");

            return result;
        }

        public async Task<SingleClientPortfoliosMoneyResponse> GetSingleClientMoneySpotLimitsByMatrixAccount(string account)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetSingleClientMoneySpotLimitsByMatrixAccount {account} Called");

            SingleClientPortfoliosMoneyResponse result = new SingleClientPortfoliosMoneyResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "queryGetSingleClientMoneySpotLimitsByMatrixAccount.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string queryGetSingleClientMoneySpotLimitsByMatrixAccount = File.ReadAllText(filePath);

            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    OracleCommand command = new OracleCommand(queryGetSingleClientMoneySpotLimitsByMatrixAccount, connection);
                    command.Parameters.Add(":account", account);

                    _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetSingleClientMoneySpotLimitsByMatrixAccount " +
                        $"{account} try to connect");
                    await connection.OpenAsync();

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            PortfoliosAndMoneyModel newPortfolio = new PortfoliosAndMoneyModel();

                            newPortfolio.MatrixClientPortfolio = reader.GetString(0);
                            newPortfolio.MoneyOpenBalanse = reader.GetDecimal(1);
                            newPortfolio.Leverage = reader.GetInt16(2);

                            result.PortfoliosAndMoney.Add(newPortfolio);
                        }
                    }

                    command.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetSingleClientMoneySpotLimitsByMatrixAccount " +
                    $"{account} Failed, Exception: " + ex.Message);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add($"DBRepository GetSingleClientMoneySpotLimitsByMatrixAccount {account} Failed, " +
                    $"Exception: " + ex.Message);
            }

            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetSingleClientMoneySpotLimitsByMatrixAccount " +
                $"{account} Success, portfolios count returned {result.PortfoliosAndMoney.Count}");

            return result;
        }

        public async Task<SingleClientPortfoliosPositionResponse> GetSingleClientActualPositionsLimitsByMatrixAccount(string account)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetSingleClientActualPositionsLimitsByMatrixAccount {account} Called");

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "queryGetSingleClientActualPositionsLimitsByMatrixAccount.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                SingleClientPortfoliosPositionResponse result = new SingleClientPortfoliosPositionResponse();
                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string queryGetSingleClientActualPositionsLimitsByMatrixAccount = File.ReadAllText(filePath);

            return await GetSingleClientPortfoliosPositionResponseByQuery(queryGetSingleClientActualPositionsLimitsByMatrixAccount, account);
        }

        public async Task<SingleClientPortfoliosPositionResponse> GetSingleClientZeroPositionToTKSLimitsByMatrixAccount(string account)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetSingleClientZeroPositionToTKSLimitsByMatrixAccount {account} Called");

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "queryGetSingleClientZeroPositionToTKSLimitsByMatrixAccount.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                SingleClientPortfoliosPositionResponse result = new SingleClientPortfoliosPositionResponse();
                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string queryGetSingleClientZeroPositionToTKSLimitsByMatrixAccount = File.ReadAllText(filePath);

            return await GetSingleClientPortfoliosPositionResponseByQuery(queryGetSingleClientZeroPositionToTKSLimitsByMatrixAccount, account);
        }

        public async Task<SingleClientPortfoliosPositionResponse> GetSingleClientClosedPositionsLimitsByMatrixAccount(string account, int daysShift)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetSingleClientClosedPositionsLimitsByMatrixAccount {account} Called");

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "queryGetSingleClientClosedPositionsLimitsByMatrixAccount.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                SingleClientPortfoliosPositionResponse result = new SingleClientPortfoliosPositionResponse();
                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string queryGetSingleClientClosedPositionsLimitsByMatrixAccount = File.ReadAllText(filePath);

            return await GetSingleClientPortfoliosPositionResponseByQuery(queryGetSingleClientClosedPositionsLimitsByMatrixAccount, account, daysShift);
        }


        private async Task<SingleClientPortfoliosPositionResponse> GetSingleClientPortfoliosPositionResponseByQuery(
            string query, 
            string account, 
            int daysShift = -1)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetSingleClientPortfoliosPositionResponseByQuery {account} {daysShift} Called");

            SingleClientPortfoliosPositionResponse result = new SingleClientPortfoliosPositionResponse();

            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    OracleCommand command = new OracleCommand(query, connection);
                    if (daysShift != -1)
                    {
                        command.Parameters.Add(":dayShiftOne", daysShift.ToString());
                    }
                    command.Parameters.Add(":account", account);

                    _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetSingleClientPortfoliosPositionResponseByQuery " +
                        $"{account} try to connect, query is {command.CommandText}");
                    await connection.OpenAsync();

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            ClientDepoPositionModel newPosition = new ClientDepoPositionModel();

                            newPosition.MatrixClientPortfolio = reader.GetString(0);
                            newPosition.SecCode = reader.GetString(1);
                            newPosition.OpenBalance = reader.GetDecimal(2);
                            if (reader.IsDBNull(3))
                            {
                                newPosition.AveragePrice = 0;
                            }
                            else
                            {
                                newPosition.AveragePrice = reader.GetDecimal(3);
                            }

                            if (reader.IsDBNull(4))
                            {
                                newPosition.SecBoard = "NULL";
                            }
                            else
                            {
                                newPosition.SecBoard = reader.GetString(4);
                            }
                            if (reader.IsDBNull(5))
                            {
                                newPosition.TKS = "";
                            }
                            else
                            {
                                newPosition.TKS = reader.GetString(5);
                            }                            

                            result.PortfoliosAndPosition.Add(newPosition);
                        }
                    }

                    command.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetSingleClientPortfoliosPositionResponseByQuery " +
                    $"{account} Failed, Exception: " + ex.Message);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add($"DBRepository GetSingleClientMoneySpotLimitsByMatrixAccount {account} Failed, " +
                    $"Exception: " + ex.Message);
            }

            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetSingleClientPortfoliosPositionResponseByQuery " +
                $"{account} Success, portfolios count returned {result.PortfoliosAndPosition.Count}");

            return result;
        }

        public async Task<BoolResponse> GetSingleClientDoTradesByMatrixAccount(string account, int daysShift)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetSingleClientDoTradesByMatrixAccount for {account} days={daysShift} Called");

            BoolResponse result = new BoolResponse();
            int requestResult = 0;

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "queryGetSingleClientDoTradesByMatrixAccount.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string queryGetSingleClientDoTradesByMatrixAccount = File.ReadAllText(filePath);

            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    OracleCommand command = new OracleCommand(queryGetSingleClientDoTradesByMatrixAccount, connection);
                    command.Parameters.Add(":clientportfolio", account);
                    command.Parameters.Add(":dayShiftOne", daysShift.ToString());
                    command.Parameters.Add(":dayShiftTwo", (daysShift - 1).ToString());

                    _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetSingleClientDoTradesByMatrixAccount try to connect");
                    await connection.OpenAsync();

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            requestResult = reader.GetInt16(0);
                        }
                    }

                    command.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetSingleClientDoTradesByMatrixAccount Failed, Exception: " + ex.Message);

                result.Response.IsSuccess = false;
                result.Response.Messages.Add($"DBRepository GetSingleClientDoTradesByMatrixAccount Failed, Exception: " + ex.Message);
            }

            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DBRepository GetSingleClientDoTradesByMatrixAccount Success");

            if (requestResult == 0)
            {
                result.Response.Messages.Add($"Deals not found");
                return result;
            }

            result.IsTrue = true;
            return result;
        }
    }
}