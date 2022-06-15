namespace DataAbstraction.Responses
{
    public class BoolResponse
    {
        public ListStringResponseModel Response { get; set; } = new ListStringResponseModel();
        public bool IsTrue { get; set; } = false;
    }
}