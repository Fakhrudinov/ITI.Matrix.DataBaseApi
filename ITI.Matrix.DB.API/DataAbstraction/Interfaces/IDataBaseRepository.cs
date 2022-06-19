using DataAbstraction.Responses;

namespace DataAbstraction.Interfaces
{
    public interface IDataBaseRepository
    {
        Task<ListStringResponseModel> CheckConnections();
        Task<MatrixClientCodeModelResponse> GetUserSpotPortfolios(string clientCode);
        Task<MatrixToFortsCodesMappingResponse> GetUserFortsPortfolios(string clientCode);
        Task<MatrixToFortsCodesMappingResponse> GetUserFortsPortfoliosNoEDP(string clientCode);
        Task<BoolResponse> GetIsPortfolioInEDP(string clientRfPortfolio);
        Task<ClientInformationResponse> GetUserPersonalInfo(string clientCode);
        Task<ClientBOInformationResponse> GetUserBOPersonalInfo(string clientCode);
        Task<BoolResponse> GetIsClientBelongsToQUIK(string clientCode);
    }
}