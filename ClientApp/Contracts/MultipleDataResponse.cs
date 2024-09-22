using System.Collections.Generic;

namespace ClientApp.Contracts
{
    public class MultipleDataResponse<T> : BaseResponse
    {
        public List<T> Data { get; set; }
    }
}