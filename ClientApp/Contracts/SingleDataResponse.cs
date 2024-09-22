namespace ClientApp.Contracts
{
    public class SingleDataResponse<T> : BaseResponse
    {
        public T Data { get; set; }
    }
}