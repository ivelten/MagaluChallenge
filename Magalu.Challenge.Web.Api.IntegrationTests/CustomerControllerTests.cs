using FluentAssertions;
using Magalu.Challenge.Application.Models.Customer;
using Magalu.Challenge.Data.Development;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Magalu.Challenge.Web.Api.IntegrationTests
{
    public class CustomerInlineData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            return DatabaseSeeds.Customers.Select(c => new object[] { c.Id }).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class CustomerControllerTests : TestBase
    {
        public CustomerControllerTests(TestServerFixture testServerFixture) 
            : base(testServerFixture)
        {
        }

        [Theory]
        [ClassData(typeof(CustomerInlineData))]
        public async Task Get_Should_Return_Expected_Value(Guid id)
        {
            using (var client = Server.CreateClient())
            {
                var response = await client.GetAsync($"/api/customer/{id}");

                response.EnsureSuccessStatusCode();

                var expected = DatabaseSeeds.Customers.FirstOrDefault(c => c.Id == id);
                var actual = await response.Content.ReadAsAsync<GetCustomerModel>();

                actual.Id.Should().Be(id);
                actual.Name.Should().Be(expected.Name);
                actual.Email.Should().Be(expected.Email);
            }
        }

        [Fact]
        public async Task GetPage_Without_Page_Number_Should_Return_First_Page()
        {
            using (var client = Server.CreateClient())
            {
                var response = await client.GetAsync($"/api/customer");

                response.EnsureSuccessStatusCode();

                var expectedItems = DatabaseSeeds.Customers.Take(PaginationOptions.DefaultPageSize).ToArray();
                var actualItems = await response.Content.ReadAsAsync<GetCustomerModel[]>();

                actualItems.Length.Should().Be(expectedItems.Length);

                actualItems.ForEach((i, actual) =>
                {
                    var expected = expectedItems[i];

                    actual.Id.Should().Be(expected.Id);
                    actual.Name.Should().Be(expected.Name);
                    actual.Email.Should().Be(expected.Email);
                });
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetPage_With_Page_Number_Should_Return_Desired_Page(int pageNumber)
        {
            using (var client = Server.CreateClient())
            {
                var response = await client.GetAsync($"/api/customer/?page={pageNumber}");

                response.EnsureSuccessStatusCode();

                var expectedItems = DatabaseSeeds.Customers.SelectPage(pageNumber, PaginationOptions.DefaultPageSize).ToArray();
                var actualItems = await response.Content.ReadAsAsync<GetCustomerModel[]>();

                actualItems.Length.Should().Be(expectedItems.Length);

                actualItems.ForEach((i, actual) =>
                {
                    var expected = expectedItems[i];

                    actual.Id.Should().Be(expected.Id);
                    actual.Name.Should().Be(expected.Name);
                    actual.Email.Should().Be(expected.Email);
                });
            }
        }

        [Fact]
        public async Task Should_Be_Able_To_Execute_Insert_Update_And_Delete_Operations_As_Administrator()
        {
            using (var client = Server.CreateClient())
            {
                var authenticator = new ApiAuthenticator(client);

                await authenticator.AuthenticateClientAsync(client, DatabaseSeeds.AdminUsername, DatabaseSeeds.AdminPassword);

                var postRequest = new SendCustomerModel { Name = "Jessie", Email = "jessie@at.com" };

                var postResponse = await client.PostAsJsonAsync("/api/customer", postRequest);

                postResponse.EnsureSuccessStatusCode();

                var postContent = await postResponse.Content.ReadAsAsync<GetCustomerModel>();

                postContent.Name.Should().Be(postRequest.Name);
                postContent.Email.Should().Be(postRequest.Email);

                var getResponse = await client.GetAsync($"/api/customer/{postContent.Id}");

                getResponse.EnsureSuccessStatusCode();

                var getContent = await getResponse.Content.ReadAsAsync<GetCustomerModel>();

                getContent.Id.Should().Be(postContent.Id);
                getContent.Name.Should().Be(postContent.Name);
                getContent.Email.Should().Be(postContent.Email);

                var putRequest = new SendCustomerModel { Name = "Bellona", Email = "bellona@at.com" };

                var putResponse = await client.PutAsJsonAsync($"/api/customer/{postContent.Id}", putRequest);

                putResponse.EnsureSuccessStatusCode();

                var putContent = await putResponse.Content.ReadAsAsync<GetCustomerModel>();

                putContent.Id.Should().Be(postContent.Id);
                putContent.Name.Should().Be(putRequest.Name);
                putContent.Email.Should().Be(putRequest.Email);

                var deleteResponse = await client.DeleteAsync($"/api/customer/{postContent.Id}");

                deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

                var notFoundContent = await client.GetAsync($"/api/customer/{postContent.Id}");

                notFoundContent.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }
    }
}
