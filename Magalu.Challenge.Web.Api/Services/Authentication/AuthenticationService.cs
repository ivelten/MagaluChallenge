using Magalu.Challenge.Data;
using Magalu.Challenge.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Magalu.Challenge.Web.Api.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<string> AuthenticateAsync(string username, string password);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly MagaluContext context;

        private readonly IHashingService hashingService;

        private readonly SecurityOptions securityOptions;

        public AuthenticationService(MagaluContext context, IHashingService hashingService, IOptions<SecurityOptions> securityOptions)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.hashingService = hashingService ?? throw new ArgumentNullException(nameof(hashingService));
            this.securityOptions = securityOptions?.Value ?? throw new ArgumentNullException(nameof(securityOptions));
        }

        public async Task<string> AuthenticateAsync(string username, string password)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (!hashingService.VerifyPassword(password, user.PasswordHash))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Convert.FromBase64String(securityOptions.JwtSecret);

            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            if (user.Customer != null)
            {
                userClaims.Add(new Claim(ClaimTypes.Email, user.Customer.Email));
                userClaims.Add(new Claim(ClaimTypes.Name, user.Customer.Name));
                userClaims.Add(new Claim(MagaluClaimTypes.CustomerId, user.Customer.Id.ToString()));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(userClaims),
                Expires = DateTime.UtcNow.AddMinutes(securityOptions.TokenExpirationTimeInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
