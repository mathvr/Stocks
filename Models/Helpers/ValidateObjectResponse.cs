namespace STOCKS.Models.Helpers;

public class ValidateObjectResponse
{
    public bool IsValid { get; set; }
    public string Message { get; set; }
    public string PropertyName { get; set; }
}