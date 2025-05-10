using AuthService.Core;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Services
{
    public class AuthentificationService
    {
        private readonly UserService _userService;

        private readonly IConfiguration _configuration;

        public AuthentificationService(UserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        public string GenerateUserJWTToken(string login, string password)
        {
            var user = _userService.GetUser(login);
            var verify = _userService.VerifyUser(user.Id, password);

            if (!verify)
            {
                throw new ValidationException("Uncorrect login or password");
            }

            var now = DateTime.UtcNow;
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString())
            };
            if (user.Admin)
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, "Admin"));
            }

            var config = _configuration.GetSection("JWT");
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: config["Issuer"],
                audience: config["Audience"],
                claims: claims,
                expires: now.Add(TimeSpan.FromMinutes(30)),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Key"])), SecurityAlgorithms.HmacSha256)
            );
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return token;
        }
    }
}
