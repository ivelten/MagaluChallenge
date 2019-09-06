using Magalu.Challenge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

using Claim = System.Security.Claims.Claim;
using ClaimsIdentity = System.Security.Claims.ClaimsIdentity;
using Magalu.Challenge.Application;

namespace Magalu.Challenge.ApplicationServices
{
    public interface IAuthenticationService
    {
        Task<string> AuthenticateAsync(string username, string password);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRepository<User> userRepository;

        private readonly IHashingService hashingService;

        private readonly SecurityOptions securityOptions;

        public AuthenticationService(IRepository<User> userRepository, IHashingService hashingService, IOptions<SecurityOptions> securityOptions)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.hashingService = hashingService ?? throw new ArgumentNullException(nameof(hashingService));
            this.securityOptions = securityOptions?.Value ?? throw new ArgumentNullException(nameof(securityOptions));
        }

        public async Task<string> AuthenticateAsync(string username, string password)
        {
            var user = await userRepository.GetFirstOrDefaultAsync(
                u => u,
                predicate: u => u.Username == username,
                include: source => source.Include(u => u.Customer));

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
                userClaims.Add(new Claim(ClaimTypes.CustomerId, user.Customer.Id.ToString()));
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
