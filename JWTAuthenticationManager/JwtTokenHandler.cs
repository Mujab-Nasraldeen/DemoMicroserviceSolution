using JWTAuthenticationManager.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTAuthenticationManager;
public class JwtTokenHandler
{
    public const string JWT_SECURITY_KEY = "xTfYjkVtu0l6hKnO2DbxVocIod61X8x3";
    private const int JWT_TOKEN_VALIDITY_MINS = 20;
    private readonly List<UserAccount> _userAccountList;

    public JwtTokenHandler()
    {
        _userAccountList = new List<UserAccount>()
        {
            new UserAccount{ UserName = "Admin", Password = "admin123", Role = "Administrator" },
            new UserAccount{ UserName = "Mujab", Password = "Mjab123", Role = "User" }
        };
    }

    public AuthenticationResponse? GenerateJwtToken(AuthenticationRequest req)
    {
        if(string.IsNullOrWhiteSpace(req.UserName) || string.IsNullOrWhiteSpace(req.Password))
            return null;

        /* Validation */
        var userAccount = _userAccountList.Where(x => x.UserName == req.UserName && x.Password == req.Password).FirstOrDefault();
        if(userAccount == null) return null;

        var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINS);
        var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);
        var claimsIdentity = new ClaimsIdentity(new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Name, req.UserName),
            new Claim(ClaimTypes.Role, userAccount.Role),
        });

        var signingCredential = new SigningCredentials(
            new SymmetricSecurityKey(tokenKey),
            SecurityAlgorithms.HmacSha256Signature);

        var securityTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Expires = tokenExpiryTimeStamp,
            SigningCredentials = signingCredential
        };

        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
        var token = jwtSecurityTokenHandler.WriteToken(securityToken);

        return new AuthenticationResponse
        {
            UserName = userAccount.UserName,
            ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds,
            JwtToken = token,
        };
    }
}