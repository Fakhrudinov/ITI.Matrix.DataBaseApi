using DataAbstraction.Models;

namespace DataAbstraction.Responses
{
    internal class DiscountsListResponse
    {
        public DiscountsListResponse()
        {
            Discounts = new List<DiscountModel>();

            IsSuccess = true;
            Messages = new List<string>();
        }

        public List<DiscountModel> Discounts { get; set; }

        public bool IsSuccess { get; set; }
        public List<string> Messages { get; set; }
    }
}
