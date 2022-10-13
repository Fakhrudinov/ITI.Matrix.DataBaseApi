using DataAbstraction.Models;

namespace DataAbstraction.Responses
{
    public class ClientDepoPositionsResponse
    {
        public ListStringResponseModel Response { get; set; } = new ListStringResponseModel();

        public List<ClientDepoPositionModel> Clients { get; set; } = new List<ClientDepoPositionModel>();
    }
}
