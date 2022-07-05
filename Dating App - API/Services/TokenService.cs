using Dating_App___API.Entities;
using Dating_App___API.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Dating_App___API.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public BinaryReader JwtRegisteredClaims { get; private set; }

        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.AppUserId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
