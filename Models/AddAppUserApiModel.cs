namespace STOCKS.Models;

public class AddAppUserApiModel
{
    public string UserNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Password { get; set; }
    public string AccessLevel { get; set; }
}