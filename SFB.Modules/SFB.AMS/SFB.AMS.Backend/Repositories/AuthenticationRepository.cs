using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SFB.AMS.Shared.Models;
using SFB.Infrastructure.Contexts;
using SFB.Infrastructure.Entities.AMS;
using SFB.Shared.Backend.Helpers;
using SFB.Shared.Backend.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace SFB.AMS.Backend.Repositories
{
    public class AuthenticationRepository : BaseRepository<SFBContext>
    {
        private readonly IConfiguration _configuration;
        public AuthenticationRepository(SFBContext context, IConfiguration configuration)
        : base(context)
        {
            _configuration = configuration;
        }

        internal async Task<string> Login(MLogin login)
        {
            EUser? user = await Context.AMSUsers.Include(u=> u.Person).FirstOrDefaultAsync(x => x.NameUser == login.User && x.Password == login.Password);

            if (user == null)
                throw new ControllerException("Usuario o contraseña Incorrecto");

            return GenerateToken(user);
        }

        private string GenerateToken(EUser user)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenLifetime = int.Parse(_configuration["Jwt:TokenLifetimeMinutes"]);
            var minToken = GenerateMinToken(user);
            var minutesRefresh = _configuration["Jwt:MinutesRefresh"];

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("NameUser", user.NameUser),
                    new Claim("NroPerson", user.Person?.NroPerson.ToString() ?? "0"),
                    new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                    new Claim(ClaimTypes.Role, user.UserType),
                    new Claim("MinRefresh", minutesRefresh),
                    new Claim("MinToken", minToken)
                }),
                Expires = DateTime.UtcNow.AddMinutes(tokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private string GenerateMinToken(EUser user)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenLifetime = int.Parse(_configuration["Jwt:TokenLifetimeMinutes"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("NameUser", user.NameUser),
                    new Claim("NroPerson", user.Person?.NroPerson.ToString() ?? "0"),
                    new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                    new Claim(ClaimTypes.Role, user.UserType)
                    
                }),
                Expires = DateTime.UtcNow.AddMinutes(tokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        internal async Task<string> RefreshToken(string nameUser)
        {
            EUser? user = await Context.AMSUsers.FirstOrDefaultAsync(u => u.NameUser == nameUser);

            if (user == null)
                throw new ControllerException("Error de Usuario");

            return GenerateToken(user);
        }
    }
}
