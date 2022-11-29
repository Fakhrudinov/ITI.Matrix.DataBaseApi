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
        Task<MatrixClientCodeModelResponse> GetAllCDPortfolios();
        Task<MatrixClientCodeModelResponse> GetAllRestrictedCDPortfolios();
        Task<MatrixClientAccountsModelResponse> GetAllUsersWithOptionWorkshop();

        //Task<MatrixClientCodeModelResponse> GetAllFrendlyNonResidentSpotPortfolios();
        Task<SecurityAndBoardResponse> GetSecuritiesSpotBlackListForNekval();
        Task<MatrixClientCodeModelResponse> GetAllEnemyNonResidentSpotPortfolios();
        Task<MatrixClientCodeModelResponse> GetAllEnemyNonResidentCdPortfolios();
        Task<ClientAndMoneyResponse> GetClientsSpotPortfoliosWhoTradesYesterday(int daysShift);
        Task<MatrixClientCodeModelResponse> GetAllFrendlyNonResidentCdPortfolios();
        Task<MatrixClientCodeModelResponse> GetAllNonKvalKpurUsersCdPortfolios();
        Task<MatrixClientCodeModelResponse> GetAllAllowedCDPortfolios();
        Task<MatrixClientCodeModelResponse> GetAllNonKvalUsersKsurCdPortfolios();
        Task<MatrixClientCodeModelResponse> GetAllKvalUsersKsurCdPortfolios();
        Task<MatrixClientCodeModelResponse> GetAllKvalUsersKpurCdPortfolios();
        Task<MatrixClientCodeModelResponse> GetAllKvalUsersKpurSpotPortfolios();
        Task<ClientDepoPositionsResponse> GetClientsPositionsByMatrixPortfolioList(IEnumerable<string> portfolios);
        Task<BoolResponse> GetIsUserHaveOptionWorkshop(string clientCode);
        Task<FortsClientCodeModelResponse> GetAllEnemyNonResidentFortsCodes();
        Task<FortsClientCodeModelResponse> GetAllNonKvalUsersWithTest16FortsCodes();
        Task<FortsClientCodeModelResponse> GetAllKvalUsersFortsCodes();
        Task<MatrixClientCodeModelResponse> GetAllKvalUsersSpotPortfolios();
        Task<PortfoliosAndTestForComplexProductResponse> GetAllNonKvalUsersSpotPortfoliosAndTestForComplexProduct();
        Task<MatrixClientCodeModelResponse> GetAllFrendlyNonResidentKvalSpotPortfolios();
        Task<MatrixClientCodeModelResponse> GetAllFrendlyNonResidentNonKvalSpotPortfolios();
        Task<SingleClientPortfoliosMoneyResponse> GetSingleClientMoneySpotLimitsByMatrixAccount(string account);
        Task<ClientDepoPositionsResponse> GetSingleClientActualPositionsLimitsByMatrixAccount(string account);
        Task<ClientDepoPositionsResponse> GetSingleClientZeroPositionToTKSLimitsByMatrixAccount(string account);
        Task<ClientDepoPositionsResponse> GetSingleClientClosedPositionsLimitsByMatrixAccount(string account, int days);
        Task<BoolResponse> GetSingleClientDoTradesByMatrixAccount(string account, int days);
    }
}