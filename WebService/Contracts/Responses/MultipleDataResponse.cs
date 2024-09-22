namespace WebService.Contracts.Responses;

public class MultipleDataResponse<T> : BaseResponse
{
    public List<T> Data { get; set; }
}