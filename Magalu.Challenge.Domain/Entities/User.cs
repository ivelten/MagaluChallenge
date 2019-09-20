using System;

namespace Magalu.Challenge.Domain.Entities
{
    public class User
    {
        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string Role { get; set; }

        public Guid? CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
