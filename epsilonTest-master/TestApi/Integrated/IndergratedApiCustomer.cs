using Common;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Model;
using MongoDB.Bson;
using System;
using System.Linq;
using System.Threading.Tasks;
using TestApi.Mocks;
using WebAPI.Controllers;
using WebAPI.DbSettings;
using WebAPI.Services;
using Xunit;
using Xunit.Priority;

namespace TestApi.Integrated
{
    public class IndergratedApiCustomer
    {
        private readonly DatabaseSettings dbSettings;
        private readonly DbContext<Customer> context;
        private readonly CustomerController controller;
        private readonly CustomerService service;

        public IndergratedApiCustomer()
        {
            dbSettings = DatabaseSettingsMock.GetDatabaseSettings();
            context = new DbContext<Customer>(dbSettings);
            service = new CustomerService(context);
            controller = new CustomerController(service);

        }

        [Fact]
        public async Task Create_ShouldReturnOk_WhenAddedNewCustomer()
        {

            // Arrange
            var newCustomer = new MockDocument()
            {
                Address = "Address",
                City = "City",
                Phone = "Phone",
                CompanyName = "CompanyName",
                ContactName = "ContactName",
                Country = "Country",
                PostalCode = "PostalCode",
                Region = "Region"
            };

            //Act
            var savedCustomer = await controller.Post(newCustomer);

            //Assert
            Assert.NotNull(savedCustomer.Result);
        }

        [Fact]
        public async Task Delete_ShouldReturnOkResult_WhenFindedAndDeletedCustomer()
        {
            //Arrange
            Customer mockCustomer = MockData.GetOneCustomerMockData();
            context.InsertOne(mockCustomer);

            //Act
            var result = await controller.Delete(mockCustomer.Id);

            //Assert
            var viewResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, viewResult.StatusCode);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenCyustomerDoesNotExists()
        {
            //Arrange
            Customer mockCustomer = MockData.GetOneCustomerMockData();
            context.InsertOne(mockCustomer);

            //Act
            var result = await controller.Delete(ObjectId.GenerateNewId().ToString());

            //Assert
            var viewResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, viewResult.StatusCode);
        }

        [Fact]
        public void GetItems_ShouldNotListCustomersWithOffset()
        {
            //Act
            var result = controller.GetItems(2);

            //Assert
            var viewResult = Assert.IsType<ActionResult<PaginationList<Customer>>>(result);
            var model = Assert.IsAssignableFrom<OkObjectResult>(viewResult.Result as OkObjectResult);

            Assert.True((model.Value as PaginationList<Customer>).Items.Count == 0, $"has no customers");
        }

        [Fact]
        public async Task GetItems_ShouldNotListCustomers()
        {
            //Arrange
            await context.DeleteManyAsync(x => x.Id != null);

            //Act
            var result = controller.GetItems(1);

            var viewResult = Assert.IsType<ActionResult<PaginationList<Customer>>>(result);
            var model = Assert.IsAssignableFrom<OkObjectResult>(viewResult.Result as OkObjectResult);

            //Assert
            Assert.True((model.Value as PaginationList<Customer>).Items.Count == 0, $"has no customers");
        }

        [Fact]
        public async Task Update_ShouldReturnResultOK_WhenCyustomerUpdate()
        {
            //Arrange
            Customer mockCustomer = MockData.GetOneCustomerMockData();
            context.InsertOne(mockCustomer);

            Customer customer = context.FindById(mockCustomer.Id);

            //Act
            customer.Country = "Holland";

            var result = await controller.Put(customer);

            //Assert
            var viewResult = Assert.IsType<ActionResult<Customer>>(result);
            var model = Assert.IsAssignableFrom<OkObjectResult>(viewResult.Result as OkObjectResult);

            Assert.Equal(200, model.StatusCode);
            Assert.NotEqual(mockCustomer.Country, (model.Value as Customer).Country);
        }
    }
}
