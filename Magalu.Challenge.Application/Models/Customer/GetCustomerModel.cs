using System;

namespace Magalu.Challenge.Application.Models.Customer
{
    public class GetCustomerModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
    }
}
