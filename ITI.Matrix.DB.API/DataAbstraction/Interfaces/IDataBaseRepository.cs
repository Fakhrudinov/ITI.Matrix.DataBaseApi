using DataAbstraction.Responses;

namespace DataAbstraction.Interfaces
{
    public interface IDataBaseRepository
    {
        Task<ListStringResponseModel> CheckConnections();
        Task<MatrixClientCodeModelResponse> GetUserSpotPortfolios(string clientCode);
        Task<MatrixToFortsCodesMappingResponse> GetUserFortsPortfolios(string clientCode);
        Task<MatrixToFortsCodesMappingResponse> GetUserFortsPortfoliosNoEDP(string clientCode);
        Task<BoolResponse> GetIsPortfolioInEDP(string clientPortfolio);
        Task<ClientInformationResponse> GetUserPersonalInfo(string clientCode);
        Task<ClientBOInformationResponse> GetUserBOPersonalInfo(string clientCode);
        Task<BoolResponse> GetIsClientBelongsToQUIK(string clientCode);
        Task WarmUpBackOfficeDataBase();
        Task<MatrixClientCodeModelResponse> GetAllNonKvalUsersKsurSpotPortfolios();
        Task<MatrixClientCodeModelResponse> GetAllNonKvalKpurUsersSpotPortfolios();
        Task<MatrixClientCodeModelResponse> GetAllFrendlyNonResidentSpotPortfolios();
        Task<SecurityAndBoardResponse> GetSecuritiesSpotBlackListForNekval();
        Task<MatrixClientCodeModelResponse> GetAllEnemyNonResidentSpotPortfolios();
        Task<MatrixClientCodeModelResponse> GetAllEnemyNonResidentCdPortfolios();
        Task<MatrixClientCodeModelResponse> GetAllFrendlyNonResidentCdPortfolios();
        Task<MatrixClientCodeModelResponse> GetAllNonKvalKpurUsersCdPortfolios();
        Task<MatrixClientCodeModelResponse> GetAllNonKvalUsersKsurCdPortfolios();
        Task<MatrixClientCodeModelResponse> GetAllKvalUsersKsurCdPortfolios();
        Task<MatrixClientCodeModelResponse> GetAllKvalUsersKpurCdPortfolios();
        Task<MatrixClientCodeModelResponse> GetAllKvalUsersKpurSpotPortfolios();
        Task<FortsClientCodeModelResponse> GetAllEnemyNonResidentFortsCodes();
        Task<FortsClientCodeModelResponse> GetAllNonKvalUsersWithTest16FortsCodes();
        Task<FortsClientCodeModelResponse> GetAllKvalUsersFortsCodes();
        Task<MatrixClientCodeModelResponse> GetAllKvalUsersSpotPortfolios();
        Task<PortfoliosAndTestForComplexProductResponse> GetAllNonKvalUsersSpotPortfoliosAndTestForComplexProduct();
    }
}