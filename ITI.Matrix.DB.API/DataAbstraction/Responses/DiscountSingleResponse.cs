using DataAbstraction.Models;

namespace DataAbstraction.Responses
{
    public class DiscountSingleResponse
    {
        public DiscountSingleResponse()
        {
            Discount = new DiscountModel();

            IsSuccess = true;
            Messages = new List<string>();
        }

        public DiscountModel Discount { get; set; }

        public bool IsSuccess { get; set; }
        public List<string> Messages { get; set; }
    }
}
