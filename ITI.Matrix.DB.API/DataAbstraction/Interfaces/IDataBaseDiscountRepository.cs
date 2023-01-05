using DataAbstraction.Responses;

namespace DataAbstraction.Interfaces
{
    public interface IDataBaseDiscountRepository
    {
        Task<DiscountsListResponse> GetDiscountsListCets();
        Task<DiscountsListResponse> GetDiscountsListEQ();
        Task<DiscountsListResponse> GetDiscountsListForts();
        Task<DiscountSingleResponse> GetSingleDiscount(string security);
        Task<DiscountSingleResponse> GetSingleDiscountForts(string security);
    }
}
