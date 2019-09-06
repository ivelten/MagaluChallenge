using Magalu.Challenge.Application.Models.User;
using System;

namespace Magalu.Challenge.ApplicationServices
{
    public struct AuthenticationResult
    {
        private AuthenticatedUserModel authenticatedUser;

        public AuthenticatedUserModel AuthenticatedUser
        {
            get
            {
                if (authenticatedUser == null)
                    throw new InvalidOperationException($"User is not authenticated.");

                return authenticatedUser;
            }
            set
            {
                authenticatedUser = value;
            }
        }

        public string ErrorMessage { get; set; }

        public bool IsAuthenticated => AuthenticatedUser != null;

        public bool IsError => AuthenticatedUser == null;

        public static AuthenticationResult Success(string username, string token)
        {
            return new AuthenticationResult
            {
                AuthenticatedUser = new AuthenticatedUserModel
                {
                    Username = username,
                    JwtToken = token
                }
            };
        }

        public static AuthenticationResult Error(string message)
        {
            return new AuthenticationResult
            {
                ErrorMessage = message
            };
        }
    }
}
