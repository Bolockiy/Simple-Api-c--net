using ApiToDo.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using toDoList.Security;
using toDoList.User;

namespace toDoList.Services
{
    public class JwtService
    {
        private readonly AppDbContext m_dbContext;
        private readonly IConfiguration m_configuration;

        public JwtService(AppDbContext _dbContext, IConfiguration _configuration)
        {
            m_dbContext = _dbContext;
            m_configuration = _configuration;

        }
        public async Task<LoginResponse?> Authenticate(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
                return null;
            var userAcc = await m_dbContext.UserAccounts.FirstOrDefaultAsync(x => x.UserName == request.UserName);
            if (userAcc == null || !PasswordHasher.VerifyPassword(request.Password, userAcc.Password!)) return null;
            var issuer = m_configuration["JwtConfig:Issuer"];
            var audience = m_configuration["JwtConfig:Audience"];
            var Key = m_configuration["JwtConfig:Key"];
            var tokenValidityMins = m_configuration.GetValue<int>("JwtConfig:TokenValidityMins");
            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Name, request.UserName),
                    new Claim(ClaimTypes.Role, userAcc.Role.ToString())
                }),
                Issuer = issuer,
                Expires = tokenExpiryTimeStamp,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key)),
                SecurityAlgorithms.HmacSha512Signature),
            };
            var TokenHandler = new JwtSecurityTokenHandler();
            var securityToken = TokenHandler.CreateToken(tokenDescriptor);
            var accessToken = TokenHandler.WriteToken(securityToken);

            return new LoginResponse
            {
                AccessToken = accessToken,
                UserName = request.UserName,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds
            };
        }

    }
}