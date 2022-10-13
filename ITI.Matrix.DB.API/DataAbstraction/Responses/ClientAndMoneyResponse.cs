using DataAbstraction.Models;

namespace DataAbstraction.Responses
{
    public class ClientAndMoneyResponse
    {
        public ListStringResponseModel Response { get; set; } = new ListStringResponseModel();

        public List<ClientAndMoneyModel> Clients { get; set; } = new List<ClientAndMoneyModel>();
    }
}
