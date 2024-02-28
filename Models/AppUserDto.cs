using System.IdentityModel.Tokens.Jwt;
using NuGet.Common;

namespace STOCKS.Models;

public class AppUserDto
{
    public string email;
    public string? firstName;
    public string? lastName;
    public string accesLevel;
    public string Token;
}