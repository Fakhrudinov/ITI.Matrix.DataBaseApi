using DataAbstraction.Models;

namespace DataAbstraction.Responses
{
    public class MatrixClientAccountsModelResponse
    {
        public ListStringResponseModel Response { get; set; } = new ListStringResponseModel();
        public List<MatrixClientAccountModel> MatrixClientAccountList { get; set; } = new List<MatrixClientAccountModel>();
    }
}
