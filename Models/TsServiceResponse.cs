namespace STOCKS.Models;

public class TServiceResponse<T> : ServiceResponse
{
    public T Data { get; set; }
}