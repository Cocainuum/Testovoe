namespace WebService.Contracts.Responses;

public class SingleDataResponse<T> : BaseResponse
{
    public T Data { get; set; }
}