using System.ComponentModel.DataAnnotations;

namespace Magalu.Challenge.Web.Api.Models.Customer
{
    public class GetCustomerModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
    }
}
