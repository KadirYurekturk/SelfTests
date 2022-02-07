using Microsoft.IdentityModel.Tokens;
using SelfTest.Model.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SelfTest.API.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public string CreateToken(User user)
        {
            var hmac = new HMACSHA512();

            var claims = new List<Claim>
           {
               new Claim(JwtRegisteredClaimNames.NameId,user?.Username),
               new Claim(ClaimTypes.Role , "Admin")
           };

            //ClaimsIdentity claimsIdentities = new ClaimsIdentity() { claims  ,"basic_user"};

            //ClaimsPrincipal cp = new ClaimsPrincipal()
            //_sessionStorageService.SetItemAsync("user", claims);
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds,
            };



            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
