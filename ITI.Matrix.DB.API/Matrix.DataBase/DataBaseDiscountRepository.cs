using DataAbstraction.Connections;
using DataAbstraction.Interfaces;
using DataAbstraction.Models;
using DataAbstraction.Responses;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;

namespace Matrix.DataBase
{
    public class DataBaseDiscountRepository : IDataBaseDiscountRepository
    {
        private ILogger<DataBaseDiscountRepository> _logger;
        private readonly string _connectionString = "";

        //команды sql не должны содержать ; в конце! 

        public DataBaseDiscountRepository(IOptions<DataBaseConnectionConfiguration> connection, ILogger<DataBaseDiscountRepository> logger)
        {
            _logger = logger;
            _connectionString = connection.Value.ConnectionString + " User Id=" + connection.Value.Login + "; Password=" + connection.Value.Password + ";";
        }

        public async Task<DiscountSingleResponse> GetSingleDiscount(string security)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DataBaseDiscountRepository GetSingleDiscount {security} Called");

            DiscountSingleResponse result = new DiscountSingleResponse();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "gueryDiscountGetSingle.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                result.IsSuccess = false;
                result.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string gueryDiscountGetSingle = File.ReadAllText(filePath);

            return await GetDiscountSingle(gueryDiscountGetSingle, security);
        }

        public async Task<DiscountSingleResponse> GetSingleDiscountForts(string security)
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DataBaseDiscountRepository GetSingleDiscountForts {security} Called");

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "gueryDiscountGetSingleForts.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                DiscountSingleResponse result = new DiscountSingleResponse();
                result.IsSuccess = false;
                result.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string gueryDiscountGetSingleForts = File.ReadAllText(filePath);

            return await GetDiscountSingle(gueryDiscountGetSingleForts, security);
        }

        private async Task<DiscountSingleResponse> GetDiscountSingle(string guery, string security)
        {
            DiscountSingleResponse result = new DiscountSingleResponse();

            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    OracleCommand command = new OracleCommand(guery, connection);
                    command.Parameters.Add(":security", security);

                    _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DataBaseDiscountRepository GetDiscountSingle " +
                        $"{security} try to connect");
                    await connection.OpenAsync();

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Discount.Discount = reader.GetDecimal(0);

                            if (reader.IsDBNull(1))
                            {
                                result.Discount.IsShort = false;
                            }
                            else
                            {
                                if (reader.GetDecimal(1) == 1) // в бд еще 0 может быть в этом поле.
                                    result.Discount.IsShort = true;
                            }

                            result.Discount.Tiker = reader.GetString(2);
                        }
                    }

                    command.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DataBaseDiscountRepository GetDiscountSingle " +
                    $"{security} Failed, Exception: " + ex.Message);

                result.IsSuccess = false;
                result.Messages.Add($"DataBaseDiscountRepository GetDiscountSingle {security} Failed, Exception: " + ex.Message);

                return result;
            }

            //нашли что нибудь?
            if (result.Discount.Tiker is null)
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DataBaseDiscountRepository GetDiscountSingle " +
                    $"{security} - discount data not found!");

                result.Discount = null;
                result.IsSuccess = false;
                result.Messages.Add($"Не найдено данных по дисконтам для {security}");
            }

            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DataBaseDiscountRepository GetDiscountSingle " +
                $"{security} Success is {result.IsSuccess}");
            return result;
        }

        public async Task<DiscountsListResponse> GetDiscountsListCets()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DataBaseDiscountRepository GetDiscountsListCets Called");

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "gueryDiscountGetListCets.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);
                
                DiscountsListResponse result = new DiscountsListResponse();
                result.IsSuccess = false;
                result.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string gueryDiscountGetListCets = File.ReadAllText(filePath);

            return await GetDiscountsList(gueryDiscountGetListCets);
        }

        public async Task<DiscountsListResponse> GetDiscountsListEQ()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DataBaseDiscountRepository GetDiscountsListEQ Called");

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "gueryDiscountGetListEQ.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                DiscountsListResponse result = new DiscountsListResponse();
                result.IsSuccess = false;
                result.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string gueryDiscountGetListEQ = File.ReadAllText(filePath);

            return await GetDiscountsList(gueryDiscountGetListEQ);
        }

        public async Task<DiscountsListResponse> GetDiscountsListForts()
        {
            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DataBaseDiscountRepository GetDiscountsListForts Called");            

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "SqlQuerys", "gueryDiscountGetListForts.sql");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} Error! File with SQL script not found at " + filePath);

                DiscountsListResponse result = new DiscountsListResponse();
                result.IsSuccess = false;
                result.Messages.Add("Error! File with SQL script not found at " + filePath);
                return result;
            }

            string gueryDiscountGetListForts = File.ReadAllText(filePath);

            return await GetDiscountsList(gueryDiscountGetListForts);
        }

        private async Task<DiscountsListResponse> GetDiscountsList(string guery)
        {
            DiscountsListResponse result = new DiscountsListResponse();

            try
            {
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    OracleCommand command = new OracleCommand(guery, connection);

                    _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DataBaseDiscountRepository GetDiscountsList " +
                        $"try to connect");
                    await connection.OpenAsync();

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            DiscountModel newDiscount = new DiscountModel();

                            newDiscount.Discount = reader.GetDecimal(0);

                            if (reader.IsDBNull(1))
                            {
                                newDiscount.IsShort = false;
                            }
                            else
                            {
                                if (reader.GetDecimal(1) == 1) // в бд еще 0 может быть в этом поле.
                                    newDiscount.IsShort = true;
                            }

                            newDiscount.Tiker = reader.GetString(2);

                            result.Discounts.Add(newDiscount);
                        }
                    }

                    command.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DataBaseDiscountRepository GetDiscountsList " +
                    $" Failed, Exception: " + ex.Message);

                result.IsSuccess = false;
                result.Messages.Add($"DataBaseDiscountRepository GetDiscountsList Failed, Exception: " + ex.Message);

                return result;
            }

            _logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss:fffff")} DataBaseDiscountRepository GetDiscountsList " +
                $" Success");
            return result;
        }
    }
}
