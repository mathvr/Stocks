using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using STOCKS;

namespace stocks.Services.Session;

public class TokenService : ITokenService
{
    private readonly UserManager<Appuser> _userManager;
    private readonly IConfigurationHelper _configuration;

    public TokenService(UserManager<Appuser> userManager, IConfigurationHelper configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<string> GenerateToken(Appuser appuser)
    {
        var claims = new List<Claim>()
        {
            new(ClaimTypes.Email, appuser.UserName ?? appuser.Email),
        };

        var roles = await _userManager.GetRolesAsync(appuser);
        
        roles
            .ForEach(r =>
            {
                claims.Add(new Claim(ClaimTypes.Role, r));
            });
        
        Console.WriteLine(_configuration.ToString());

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetJwtToken()));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
        
        var tokenOptions = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: creds);
        
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }
}