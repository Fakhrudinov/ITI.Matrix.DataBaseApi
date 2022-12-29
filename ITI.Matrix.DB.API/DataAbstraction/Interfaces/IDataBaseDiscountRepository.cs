using DataAbstraction.Responses;

namespace DataAbstraction.Interfaces
{
    public interface IDataBaseDiscountRepository
    {
        Task<DiscountSingleResponse> GetSingleDiscount(string security);
    }
}
